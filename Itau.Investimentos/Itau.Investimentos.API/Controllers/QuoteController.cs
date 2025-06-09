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

            return CreatedAtAction(nameof(Create), new { id = quote.Id }, quote);
        }
    }
}
