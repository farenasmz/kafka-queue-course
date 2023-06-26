using Banking.Account.Command.Application.Models;
using Banking.Cqrs.Core.Event;
using Banking.Cqrs.Core.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class AccountEventProducer : IEventProducer
    {
        public KafkaSettings KafkaSettings { get; set; }

        public AccountEventProducer(IOptions<KafkaSettings> options)
        {
            KafkaSettings = options.Value;
        }

        public void Produce(string topic, BaseEvent @event)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = $"{KafkaSettings.HostName} : {KafkaSettings.Port}"
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var classEvent = @event.GetType();
            var value = JsonConvert.SerializeObject(@event);
            var message = new Message<Null, string>()
            {
                Value = value,
            };
            producer.ProduceAsync(topic, message).GetAwaiter().GetResult();
        }
    }
}
