using Itau.Investimentos.API.DTOs;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuoteDTO dto)
        {
            if (dto.AssetId <= 0 || dto.UnitPrice <= 0)
                return BadRequest("AssetId and UnitPrice must be greater than zero.");

            var quote = new Quote
            {
                AssetId = dto.AssetId,
                UnitPrice = dto.UnitPrice,
                QuotedAt = DateTime.UtcNow
            };

            await _quoteRepository.AddAsync(quote);

            var response = new QuoteResponseDTO
            {
                Id = quote.Id,
                AssetId = quote.AssetId,
                UnitPrice = quote.UnitPrice,
                QuotedAt = quote.QuotedAt
            };


            return CreatedAtAction(nameof(GetById), new { id = quote.Id }, response);
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

            var response = quotes.Select(q => new QuoteResponseDTO
            {
                Id = q.Id,
                AssetId = q.AssetId,
                UnitPrice = q.UnitPrice,
                QuotedAt = q.QuotedAt
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
