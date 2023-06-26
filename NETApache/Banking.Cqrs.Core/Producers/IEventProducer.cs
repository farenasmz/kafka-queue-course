using Banking.Cqrs.Core.Event;

namespace Banking.Cqrs.Core.Producers
{
    public interface IEventProducer
    {
        void Produce(string topic, BaseEvent @event);
    }
}
