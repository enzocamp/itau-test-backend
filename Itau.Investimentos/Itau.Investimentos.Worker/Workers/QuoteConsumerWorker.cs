using Confluent.Kafka;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Interfaces;
using Itau.Investimentos.Domain.Services;
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
        private readonly IConfiguration _configuration;
        private readonly IPositionCalculationService _positionCalculationService;

        public QuoteConsumerWorker(
            ILogger<QuoteConsumerWorker> logger,
            IQuoteRepository quoteRepository,
            IPositionCalculationService positionCalculationService,
            IConfiguration configuration)
        {
            _logger = logger;
            _quoteRepository = quoteRepository;
            _positionCalculationService = positionCalculationService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int retryCount = 0;
            const int maxRetries = 10;

            while (retryCount < maxRetries && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = _configuration["Kafka:BootstrapServers"],
                        GroupId = _configuration["Kafka:GroupId"],
                        AutoOffsetReset = AutoOffsetReset.Earliest
                    };

                    using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
                    consumer.Subscribe(_configuration["Kafka:Topic"]);

                    _logger.LogInformation("Kafka consumer connected and listening to topic.");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(stoppingToken);
                            _logger.LogInformation("Menssage received from Kafka: {Json}", result.Message.Value);

                            var message = JsonSerializer.Deserialize<QuoteKafkaMessage>(result.Message.Value);

                            if (message == null)
                            {
                                _logger.LogWarning("Kafka message null or failed to deserialize.");
                                continue; // pula para a próxima mensagem
                            }

                            if (message != null)
                            {

                                var existing = await _quoteRepository.GetByAssetIdAsync(message.AssetId);

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
                                    // ✅ Atualiza o P&L de todas as posições com esse ativo
                                    await _positionCalculationService.RecalculatePnLAsync(message.AssetId);
                                }
                                else
                                {
                                    _logger.LogInformation("Duplicate quote ignored (idempotent). AssetId={AssetId}", message.AssetId);
                                }
                            }
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogWarning(ex, "Kafka consumption failed, retrying...");
                            await Task.Delay(3000, stoppingToken);
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, "Invalid JSON format received from Kafka.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unexpected error while processing Kafka message.");
                            await Task.Delay(3000, stoppingToken);
                        }
                    }

                    consumer.Close();
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogError(ex, "Error initializing Kafka consumer. Attempt {Retry}/{Max}", retryCount, maxRetries);
                    await Task.Delay(5000, stoppingToken);
                }
            }

            if (retryCount >= maxRetries)
            {
                _logger.LogCritical("Max retry attempts reached. Kafka consumer failed to start.");
            }
        }
    }
}
