using CookBook.Domain;
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

namespace CookBook.Web.React
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            ConfigureEventStore(services);
            services.AddTransient<IAggregateRepository, AggregateRepository>();

            ConfigureRavenDB(services);

            ConfigureMediatR(services);

            services.AddHostedService<RecipeEventSubscriptionProcessor>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
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
            var assemblies = new[] { typeof(Startup).Assembly, typeof(IEvent).Assembly };
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
