using CookBook.Domain.Events;
using CookBook.Infrastructure;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class RecipeEventSubscriptionProcessor : BackgroundService
    {
        const string StreamName = "$ce-Recipe";

        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ILogger<RecipeEventSubscriptionProcessor> logger;
        private readonly IDocumentStore documentStore;
        private readonly IAggregateRepository aggregateRepository;
        private readonly IMediator mediator;

        private EventStoreStreamCatchUpSubscription subscription;

        public RecipeEventSubscriptionProcessor(
            IEventStoreConnection eventStoreConnection,
            ILogger<RecipeEventSubscriptionProcessor> logger,
            IDocumentStore documentStore,
            IAggregateRepository aggregateRepository,
            IMediator mediator)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.logger = logger;
            this.documentStore = documentStore;
            this.aggregateRepository = aggregateRepository;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation($"{nameof(ExecuteAsync)} started");

            Subscribe();
        }

        private async Task Subscribe()
        {
            var checkpointRepo = new CheckpointRespository(this.documentStore.OpenAsyncSession());
            var pos = await checkpointRepo.Get(StreamName);
            var settings = new CatchUpSubscriptionSettings(
                maxLiveQueueSize: 10000,
                readBatchSize: 500,
                verboseLogging: false,
                resolveLinkTos: true,
                subscriptionName: "Recipes");

            this.subscription = this.eventStoreConnection.SubscribeToStreamFrom(
                StreamName,
                pos ?? StreamCheckpoint.StreamStart,
                settings,
                ProcessEvent,
                _ => this.logger.LogInformation("Subscription processing started"),
                (_, reason, ex) => this.logger.LogError($"Subscription dropped because {reason}: {ex}")
                );
        }

        private async Task ProcessEvent(EventStoreCatchUpSubscription subscription, ResolvedEvent e)
        {
            this.logger.LogInformation($"{nameof(ProcessEvent)} {e.Event.EventType} #{e.OriginalEventNumber}");

            try
            {
                var eventType = Type.GetType(Encoding.UTF8.GetString(e.Event.Metadata));
                if (!EventRegister.Events.Contains(eventType))
                    return;

                var @event = JsonSerializer.Deserialize(Encoding.UTF8.GetString(e.Event.Data), eventType) as IDomainEvent;
                using var ravenSession = this.documentStore.OpenAsyncSession();
                var recipe = await UpdateRecipe(ravenSession, e.Event.EventStreamId, @event);
                await this.mediator.Publish(new RecipeModifiedNotification(recipe, @event));
                await new CheckpointRespository(ravenSession).Save(StreamName, e.OriginalEventNumber);
            }
            catch (ConnectionClosedException connClosedEx)
            {
                this.logger.LogError($"Error processing event {e}: {connClosedEx}");
                await Subscribe();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error processing event {e}: {ex}");
            }
        }

        private async Task<Recipe> UpdateRecipe(IAsyncDocumentSession ravenSession, string eventStreamId, IDomainEvent @event)
        {
            var snapshotRepo = new SnapshotRepo(ravenSession);
            var recipe = await snapshotRepo.Get<Recipe>(eventStreamId);
            if (recipe != null)
            {
                recipe.Apply(@event);
            }
            else
            {
                recipe = await this.aggregateRepository.LoadAsync<Recipe>(StreamNameToID(eventStreamId));
                await ravenSession.StoreAsync(recipe, recipe.DocumentID);
            }

            await ravenSession.SaveChangesAsync();
            return recipe;
        }

        private Guid StreamNameToID(string streamName) => new Guid(streamName.Substring(streamName.IndexOf("-") + 1));
    }
}
