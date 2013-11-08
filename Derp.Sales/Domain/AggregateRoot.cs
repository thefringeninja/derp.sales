using System;
using System.Collections.Generic;
using Derp.Sales.Infrastructure;
using Derp.Sales.Messaging;

namespace Derp.Sales.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> changes = new List<Event>();

        public abstract Guid Id { get; }
        public int Version { get; private set; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return changes;
        }

        public void MarkChangesAsCommitted()
        {
            changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
                ApplyChange(e, false);
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
                changes.Add(@event);
            Version++;
        }
    }
}
