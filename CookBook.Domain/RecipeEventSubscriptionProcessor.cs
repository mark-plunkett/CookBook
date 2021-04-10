using CookBook.Domain.Events;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
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
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ILogger<RecipeEventSubscriptionProcessor> logger;
        private readonly IDocumentStore documentStore;
        private readonly IAggregateRepository aggregateRepository;

        private EventStoreStreamCatchUpSubscription subscription;

        public RecipeEventSubscriptionProcessor(
            IEventStoreConnection eventStoreConnection,
            ILogger<RecipeEventSubscriptionProcessor> logger,
            IDocumentStore documentStore,
            IAggregateRepository aggregateRepository)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.logger = logger;
            this.documentStore = documentStore;
            this.aggregateRepository = aggregateRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation($"{nameof(ExecuteAsync)} started");

            // Wait for events from the $ce-recipe stream

            var checkpointRepo = new CheckpointRespository(this.documentStore.OpenAsyncSession());
            var pos = await checkpointRepo.Get("recipes");

            var settings = new CatchUpSubscriptionSettings(
                maxLiveQueueSize: 10000,
                readBatchSize: 500,
                verboseLogging: false,
                resolveLinkTos: true,
                subscriptionName: "Recipes");

            this.subscription = this.eventStoreConnection.SubscribeToStreamFrom(
                "$ce-Recipe",
                pos?.CommitPosition ?? StreamCheckpoint.StreamStart,
                settings,
                ProcessEvent,
                _ => this.logger.LogInformation("Subscription processing started"),
                (_, reason, ex) => this.logger.LogError($"Subscription dropped because {reason}: {ex}")
                );
        }

        private async Task ProcessEvent(EventStoreCatchUpSubscription subscription, ResolvedEvent e)
        {
            this.logger.LogInformation($"{nameof(ProcessEvent)} {e.Event.EventType} #{e.OriginalEventNumber}");

            // Build aggregate from current snapshot of recipe list
            // Apply new events
            // Save snapshot

            using var ravenSession = this.documentStore.OpenAsyncSession();
            var checkpointRepo = new CheckpointRespository(ravenSession);

            try
            {
                var eventType = Type.GetType(Encoding.UTF8.GetString(e.Event.Metadata));
                if (!EventRegister.Events.Contains(eventType))
                    return;

                var eventData = JsonSerializer.Deserialize(Encoding.UTF8.GetString(e.Event.Data), eventType);
                var recipe = await this.aggregateRepository.LoadAsync<Recipe>(StreamNameToID(e.Event.EventStreamId));
                await ravenSession.StoreAsync(recipe);
                await ravenSession.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error processing event {e}: {ex}");
            }
        }

        private bool TryGetType(string name, out Type type)
        {
            type = Type.GetType(name);
            return type != null;
        }

        private Guid StreamNameToID(string streamName) => new Guid(streamName.Substring(streamName.IndexOf("-") + 1));
    }
}
