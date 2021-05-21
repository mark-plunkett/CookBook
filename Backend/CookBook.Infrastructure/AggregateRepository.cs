using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookBook.Infrastructure
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStoreConnection eventStore;

        public AggregateRepository(IEventStoreConnection eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task SaveAsync<T>(T aggregate) where T : Aggregate, new()
        {
            var events = aggregate
                .GetChanges()
                .Select(e => new EventData(
                    Guid.NewGuid(),
                    e.GetType().Name,
                    true,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize((object)e)),
                    Encoding.UTF8.GetBytes(e.GetType().FullName!)))
                .ToArray();

            if (!events.Any())
                return;

            var streamName = GetStreamName(aggregate, aggregate.ID);
            await eventStore.AppendToStreamAsync(streamName, aggregate.Version, events);
        }

        public async Task<T> LoadAsync<T>(Guid aggregateId) where T : Aggregate, new()
        {
            if (aggregateId == Guid.Empty)
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(aggregateId));

            var aggregate = new T();
            var streamName = GetStreamName(aggregate, aggregateId);
            var nextPageStart = 0L;

            do
            {
                var page = await eventStore.ReadStreamEventsForwardAsync(streamName, nextPageStart, 4096, false);

                if (page.Events.Length > 0)
                {
                    aggregate.Load(
                        page.Events.Last().Event.EventNumber,
                        page.Events
                            .Select(e => new BaseEvent(
                                e.OriginalEvent.EventNumber,
                                DeserializeEvent<T>(e)))
                            .ToArray());
                }

                nextPageStart = !page.IsEndOfStream ? page.NextEventNumber : -1;
            } while (nextPageStart != -1);

            return aggregate;
        }

        private IDomainEvent DeserializeEvent<T>(ResolvedEvent e) where T : Aggregate, new()
        {
            return (IDomainEvent)JsonSerializer.Deserialize(
                Encoding.UTF8.GetString(e.OriginalEvent.Data),
                Type.GetType($"CookBook.Domain.Recipes.Events.{e.OriginalEvent.EventType}, {typeof(T).Assembly.FullName}")!)!;
        }

        private string GetStreamName<T>(T type, Guid aggregateId) where T : Aggregate, new() => $"{type.GetType().Name}-{aggregateId}";
    }
}
