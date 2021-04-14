﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CookBook.Domain
{
    public abstract class Aggregate
    {
        readonly IList<IEvent> _changes = new List<IEvent>();

        public Guid ID { get; protected set; } = Guid.Empty;
        public long Version { get; private set; } = -1;
        public string DocumentID => $"{this.GetType().Name}-{this.ID}";

        protected abstract void When(IEvent @event);

        public void Apply(IEvent @event)
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

        public IEnumerable<IEvent> GetChanges() => _changes.ToArray();
    }
}
