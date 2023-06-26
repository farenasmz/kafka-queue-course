using Banking.Cqrs.Core.Domain;

namespace Banking.Cqrs.Core.Handler
{
    public interface IEventSourcingHandler<T>
    {
        Task Save(AggregateRoot aggregate);
        Task<T> GetById(string id);
    }
}
