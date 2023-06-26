using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Handler;
using Banking.Cqrs.Core.Infraestructure;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class AccountEventSourcingHandler : IEventSourcingHandler<AccountAggregate>
    {
        private readonly IEventStore EventStore;

        public AccountEventSourcingHandler(IEventStore eventStore)
        {
            EventStore = eventStore;
        }

        public async Task<AccountAggregate> GetById(string id)
        {
            var aggregate = new AccountAggregate();
            var events = await EventStore.GetEvents(id);

            if (events != null && events.Any())
            {
                aggregate.ReplyChanges(events);
                var latestVersion = events.Max(e => e.Version);
                aggregate.SetVersion(latestVersion);
            }

            return aggregate;
        }

        public async Task Save(AggregateRoot aggregate)
        {
            await EventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.GetVersion());
            aggregate.MarkChangesAsCommitted();
        }
    }
}
