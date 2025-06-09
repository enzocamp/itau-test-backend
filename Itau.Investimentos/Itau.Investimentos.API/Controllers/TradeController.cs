using Itau.Investimentos.API.DTOs;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Enums;
using Itau.Investimentos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeRepository _tradeRepository;
        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TradeDTO dto)
        {
            if (dto.Quantity <= 0 || dto.UnitPrice <= 0)
                return BadRequest("Quantity and UnitPrice must be greater than zero.");

            var trade = new Trade
            {
                UserId = dto.UserId,
                AssetId = dto.AssetId,
                Quantity = (uint)dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Fee = dto.Fee,
                TradeType = dto.TradeType,
                ExecutedAt = DateTime.UtcNow
            };

            await _tradeRepository.AddAsync(trade);

            return CreatedAtAction(nameof(Create), new { id = trade.Id }, trade);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trades = await _tradeRepository.GetAllAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trade = await _tradeRepository.GetByIdAsync(id);

            if(trade == null)
            {
                return NotFound();
            }

            return Ok(trade);   
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TradeDTO dto)
        {
            var existing = await _tradeRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.UserId = dto.UserId;
            existing.AssetId = dto.AssetId;
            existing.Quantity = (uint)dto.Quantity;
            existing.UnitPrice = dto.UnitPrice;
            existing.Fee = dto.Fee;
            existing.TradeType = dto.TradeType;
            existing.ExecutedAt = DateTime.UtcNow;

            await _tradeRepository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trade = await _tradeRepository.GetByIdAsync(id);
            if (trade == null)
                return NotFound();

            await _tradeRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
