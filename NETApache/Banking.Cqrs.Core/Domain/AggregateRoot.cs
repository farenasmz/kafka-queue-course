using Banking.Cqrs.Core.Event;

namespace Banking.Cqrs.Core.Domain
{
    public abstract class AggregateRoot
    {
        public string? Id { get; set; }
        private int Version = -1;
        private List<BaseEvent> Changes = new List<BaseEvent>();

        public int GetVersion()
        {
            return Version;
        }

        public void SetVersion(int version)
        {
            Version = version;
        }

        public List<BaseEvent> GetUncommittedChanges()
        {
            return Changes;
        }

        public void MarkChangesAsCommitted()
        {
            Changes.Clear();
        }

        public void ApplyChanges(BaseEvent @event, bool isNewEvent)
        {
            try
            {
                var eventType = @event.GetType();
                var method = GetType().GetMethod("Apply", new[] { eventType });
                method?.Invoke(this, new object[] { @event });
            }
            finally
            {
                if (isNewEvent)
                {
                    Changes.Add(@event);
                }
            }
        }

        public void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }

        public void ReplyChanges(IEnumerable<BaseEvent> @events)
        {
            foreach (var change in @events)
            {
                ApplyChanges(change, false);
            }
        }
    }
}
