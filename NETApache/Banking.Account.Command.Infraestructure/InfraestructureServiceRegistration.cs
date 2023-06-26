using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Infraestructure.KafkaEvents;
using Banking.Account.Command.Infraestructure.Repositories;
using Banking.Cqrs.Core.Event;
using Banking.Cqrs.Core.Handler;
using Banking.Cqrs.Core.Infraestructure;
using Banking.Cqrs.Core.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Banking.Account.Command.Infraestructure
{
    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap<BaseEvent>();
            BsonClassMap.RegisterClassMap<AccountOpenedEvent>();
            BsonClassMap.RegisterClassMap<AccountClosedEvent>();
            BsonClassMap.RegisterClassMap<FundsDepositedEvent>();
            BsonClassMap.RegisterClassMap<FundsWithDrawnEvent>();

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IEventProducer, AccountEventProducer>();
            services.AddTransient<IEventStoreRepository, EventStoreRepository>();
            services.AddTransient<IEventStore, AccountEventStore>();
            services.AddTransient<IEventSourcingHandler<AccountAggregate>, AccountEventSourcingHandler>();
            return services;
        }
    }
}
