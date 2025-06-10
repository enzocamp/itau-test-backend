using Confluent.Kafka;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Infrastructure.Interfaces;
using Itau.Investimentos.Worker.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Itau.Investimentos.Worker.Workers
{
    public class QuoteConsumerWorker : BackgroundService
    {

        private readonly ILogger<QuoteConsumerWorker> _logger;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IConsumer<Ignore, string> _consumer;

        public QuoteConsumerWorker(
            ILogger<QuoteConsumerWorker> logger,
            IQuoteRepository quoteRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _quoteRepository = quoteRepository;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _consumer.Subscribe(configuration["Kafka:Topic"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka consumer started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var message = JsonSerializer.Deserialize<QuoteKafkaMessage>(result.Message.Value);

                    if (message != null)
                    {
                        var existing = await _quoteRepository
                            .GetByAssetIdAsync(message.AssetId);

                        var alreadySaved = existing.Any(q =>
                            q.UnitPrice == message.UnitPrice &&
                            q.QuotedAt == message.QuotedAt);

                        if (!alreadySaved)
                        {
                            await _quoteRepository.AddAsync(new Quote
                            {
                                AssetId = message.AssetId,
                                UnitPrice = message.UnitPrice,
                                QuotedAt = message.QuotedAt
                            });

                            _logger.LogInformation("Quote saved from Kafka: AssetId={AssetId}, Price={Price}", message.AssetId, message.UnitPrice);
                        }
                        else
                        {
                            _logger.LogInformation("Duplicate quote ignored (idempotent).");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming/saving quote from Kafka.");
                    await Task.Delay(2000, stoppingToken); // simple retry
                }
            }

            _consumer.Close();
        }
    }
}

