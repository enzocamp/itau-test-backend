using Confluent.Kafka;
using Itau.Investimentos.API.DTOs;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Interfaces;
using Itau.Investimentos.Infrastructure.Messaging.Interfaces;
using Itau.Investimentos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly ILogger<QuoteController> _logger;
        private readonly IMessageProducer _messageProducer;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IAssetRepository _assetRepository;

        public QuoteController(ILogger<QuoteController> logger, IMessageProducer messageProducer, IQuoteRepository quoteRepository, IAssetRepository assetRepository)
        {
            _logger = logger;
            _messageProducer = messageProducer;
            _quoteRepository = quoteRepository;
            _assetRepository = assetRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuoteDTO dto)
        {
            if (dto.AssetId <= 0 || dto.UnitPrice <= 0)
                return BadRequest("AssetId and UnitPrice must be greater than zero.");

            var assetExists = await _assetRepository.GetByIdAsync(dto.AssetId);
            if (assetExists == null)
            { 
                return BadRequest("AssetId doesn't exist");
            }

            var quote = new Quote
            {
                AssetId = dto.AssetId,
                UnitPrice = dto.UnitPrice,
                QuotedAt = DateTime.UtcNow
            };

            try
            {
                // await _quoteRepository.AddAsync(quote);
                await _messageProducer.SendAsync("quotes-topic", quote);

                var response = new QuoteResponseDTO
                {
                    Id = quote.Id,
                    AssetId = quote.AssetId,
                    UnitPrice = quote.UnitPrice,
                    QuotedAt = quote.QuotedAt
                };

                _logger.LogInformation("Quote sent to Kafka: {@Quote}", response);

                return Accepted(new { status = "Message sent to Kafka" });
                // return CreatedAtAction(nameof(GetById), new { id = quote.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to Kafka.");
                return StatusCode(500, "Internal error processing the message.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quote = await _quoteRepository.GetByIdAsync(id);
            if (quote == null)
                return NotFound();

            var response = new QuoteResponseDTO
            {
                Id = quote.Id,
                AssetId = quote.AssetId,
                UnitPrice = quote.UnitPrice,
                QuotedAt = quote.QuotedAt
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByAssetId([FromQuery] int assetId)
        {
            if (assetId <= 0)
                return BadRequest("AssetId must be greater than zero.");

            var quotes = await _quoteRepository.GetByAssetIdAsync(assetId);

            if (quotes == null || !quotes.Any())
            {
                return BadRequest("AssetId doesn't have a quote linked");
            }

            var response = quotes.Select(q => new QuoteResponseDTO
            {
                Id = q.Id,
                AssetId = q.AssetId,
                UnitPrice = q.UnitPrice,
                QuotedAt = q.QuotedAt
            });

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var quotes = await _quoteRepository.GetAllAsync();

            if (quotes == null || !quotes.Any())
            {
                return NoContent();
            }

            var response = quotes.Select(t => new QuoteResponseDTO
            {
                Id = t.Id,
                AssetId = t.AssetId,
                UnitPrice = t.UnitPrice,
                QuotedAt = t.QuotedAt
            });

            return Ok(response);
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastByAssetId([FromQuery] int assetId)
        {
            if (assetId <= 0)
                return BadRequest("AssetId must be greater than zero.");

            var quote = await _quoteRepository.GetLastQuoteByAssetIdAsync(assetId);

            if (quote == null)
                return NotFound();

            var response = new QuoteResponseDTO
            {
                Id = quote.Id,
                AssetId = quote.AssetId,
                UnitPrice = quote.UnitPrice,
                QuotedAt = quote.QuotedAt
            };

            return Ok(response);
        }
    }
}
