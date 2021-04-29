using CookBook.Domain;
using CookBook.Api.Sync;
using EventStore.ClientAPI;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using Hellang.Middleware.ProblemDetails;
using CookBook.Infrastructure;

namespace CookBook.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            ConfigureEventStore(services);
            services.AddTransient<IAggregateRepository, AggregateRepository>();

            ConfigureRavenDB(services);

            ConfigureMediatR(services);

            services.AddHostedService<RecipeEventSubscriptionProcessor>();

            services.AddSignalR();

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => !Environment.IsDevelopment();
                options.Map<BusinessRuleException>(ex => new BusinessRuleExceptionProblemDetails(ex));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(options =>
                {
                    options.WithOrigins("http://localhost:3000");
                    options.AllowCredentials();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseProblemDetails();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<RecipeHub>("api/recipeHub");
            });
        }

        private void ConfigureEventStore(IServiceCollection services)
        {
            var connectionSettings = ConnectionSettings.Create();
            connectionSettings
                //.EnableVerboseLogging()
                //.UseDebugLogger()
                //.UseConsoleLogger()
                .SetHeartbeatTimeout(TimeSpan.FromSeconds(60))
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30));
            var connection = EventStoreConnection.Create(
                "ConnectTo=tcp://admin:changeit@localhost:1113;UseSslConnection=false;",
                connectionSettings);
            connection.ConnectAsync().GetAwaiter().GetResult();
            services.AddSingleton(connection);
        }

        private void ConfigureMediatR(IServiceCollection services)
        {
            var assemblies = new[] { typeof(Startup).Assembly, typeof(IDomainEvent).Assembly };
            services.AddMediatR(assemblies);
        }

        private void ConfigureRavenDB(IServiceCollection services)
        {
            IDocumentStore store = new DocumentStore
            {
                Urls = new[]                        // URL to the Server,
                {                                   // or list of URLs 
                    "http://localhost:8080"  // to all Cluster Servers (Nodes)
                },
                Database = "cookbook",             // Default database that DocumentStore will interact with
                Conventions = { }                   // DocumentStore customizations
            };

            store.Initialize();

            services.AddSingleton(store);

            services.AddScoped(s => store.OpenAsyncSession());
        }
    }
}
