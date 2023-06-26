
using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Domain;
using Banking.Cqrs.Core.Event;
using Banking.Cqrs.Core.Infraestructure;
using Banking.Cqrs.Core.Producers;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class AccountEventStore : IEventStore
    {
        private readonly IEventStoreRepository EventStoreRepository;
        private readonly IEventProducer Producer;

        public AccountEventStore(IEventProducer producer, IEventStoreRepository eventStoreRepository)
        {
            Producer = producer;
            EventStoreRepository = eventStoreRepository;
        }

        public async Task<List<BaseEvent>> GetEvents(string aggregateId)
        {
            var result = await EventStoreRepository.FindByAggregateIdentifier(aggregateId);

            if(result == null || !result.Any())
            {
                throw new ArgumentException("Not valid account");
            }

            return result.Select(e => e.EventData).ToList();
        }

        public async Task SaveEvents(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await EventStoreRepository.FindByAggregateIdentifier(aggregateId);

            if (expectedVersion != -1 && eventStream.ElementAt(eventStream.Count() - 1).Version != expectedVersion)
            {
                throw new ArgumentException("Concurrency Exception");
            }

            var version = expectedVersion;

            foreach (var item in events)
            {
                version++;
                item.Version = version;

                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.UtcNow,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(AccountAggregate),
                    Version = version,
                    EventType = item.GetType().Name,
                    EventData = item,
                };

                await EventStoreRepository.InsertDocument(eventModel);
                Producer.Produce(item.GetType().Name, item);
            }
        }
    }
}
