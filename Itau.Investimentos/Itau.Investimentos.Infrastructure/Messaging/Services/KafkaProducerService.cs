using Itau.Investimentos.Infrastructure.Messaging.Interfaces;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Itau.Investimentos.Infrastructure.Messaging.Services
{
    public class KafkaProducerService : IMessageProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
            _logger = logger;
        }

        public async Task SendAsync<T>(string topic, T message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);

                _logger.LogInformation("Enviando mensagem para Kafka (topic: {Topic}): {Payload}", topic, json);

                var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
                _logger.LogInformation("Mensagem entregue com sucesso no Kafka (topic: {Topic}, offset: {Offset})",
                   deliveryResult.Topic, deliveryResult.Offset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar mensagem para o Kafka (topic: {Topic})", topic);
                throw;
            }
        }
    }
}

