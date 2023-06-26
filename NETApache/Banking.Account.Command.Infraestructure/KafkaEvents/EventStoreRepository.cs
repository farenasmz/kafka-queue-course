using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Domain;
using Banking.Account.Command.Infraestructure.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class EventStoreRepository : MongoRepository<EventModel>, IEventStoreRepository
    {
        public EventStoreRepository(IOptions<MongoSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<EventModel>> FindByAggregateIdentifier(string aggregateIdentifier)
        {
            var filter = Builders<EventModel>.Filter.Eq(doc => doc.AggregateIdentifier, aggregateIdentifier);
            return await Collection.Find(filter).ToListAsync();
        }
    }
}
