using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CookBook.Infrastructure
{
    public abstract class Aggregate
    {
        readonly IList<IDomainEvent> _changes = new List<IDomainEvent>();

        public Guid ID { get; protected set; } = Guid.Empty;
        public long Version { get; private set; } = -1;
        public string DocumentID => GetDocumentID(this.GetType(), this.ID);

        public static string GetDocumentID(Type type, Guid id) => $"{type.Name}-{id}";
        public static string GetDocumentID<TAggregate>(Guid id) where TAggregate : Aggregate => GetDocumentID(typeof(TAggregate), id);

        protected abstract void When(IDomainEvent @event);

        public void Apply(IDomainEvent @event)
        {
            When(@event);

            _changes.Add(@event);
        }

        public void Load(long version, IEnumerable<BaseEvent> history)
        {
            Version = version;

            foreach (var e in history)
            {
                When(e.Event);
                this.Version = e.Version;
            }
        }

        public IEnumerable<IDomainEvent> GetChanges() => _changes.ToArray();
    }
}
