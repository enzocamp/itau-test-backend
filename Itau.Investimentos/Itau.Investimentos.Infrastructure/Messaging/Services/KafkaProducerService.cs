using Itau.Investimentos.Infrastructure.Messaging.Interfaces;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Itau.Investimentos.Infrastructure.Messaging.Services
{
    public class KafkaProducerService : IMessageProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendAsync<T>(string topic, T message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
        }
    }
}

