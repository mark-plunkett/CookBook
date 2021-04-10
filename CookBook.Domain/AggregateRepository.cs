using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookBook.Domain
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
            var events = aggregate.GetChanges()
                .Select(@event => new EventData(
                    Guid.NewGuid(),
                    @event.GetType().Name,
                    true,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize((object)@event)),
                    Encoding.UTF8.GetBytes(@event.GetType().FullName)))
                .ToArray();

            if (!events.Any())
            {
                return;
            }

            var streamName = GetStreamName(aggregate, aggregate.ID);

            var result = await eventStore.AppendToStreamAsync(streamName, ExpectedVersion.Any, events);
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
                            .Select(e => JsonSerializer.Deserialize(Encoding.UTF8.GetString(e.OriginalEvent.Data), Type.GetType(Encoding.UTF8.GetString(e.OriginalEvent.Metadata))))
                            .Cast<IEvent>()
                            .ToArray());
                }

                nextPageStart = !page.IsEndOfStream ? page.NextEventNumber : -1;
            } while (nextPageStart != -1);

            return aggregate;
        }

        private string GetStreamName<T>(T type, Guid aggregateId) => $"{type.GetType().Name}-{aggregateId}";
    }
}
