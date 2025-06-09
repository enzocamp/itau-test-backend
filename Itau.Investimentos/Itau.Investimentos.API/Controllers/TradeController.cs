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

            var response = new TradeResponseDTO
            {
                Id = trade.Id,
                UserId = trade.UserId,
                AssetId = trade.AssetId,
                Quantity = trade.Quantity,
                UnitPrice = trade.UnitPrice,
                Fee = trade.Fee,
                TradeType = trade.TradeType,
                ExecutedAt = trade.ExecutedAt
            };


            return CreatedAtAction(nameof(GetById), new { id = trade.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trades = await _tradeRepository.GetAllAsync();

            var response = trades.Select(t => new TradeResponseDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                AssetId = t.AssetId,
                Quantity = t.Quantity,
                UnitPrice = t.UnitPrice,
                Fee = t.Fee,
                TradeType = t.TradeType,
                ExecutedAt = t.ExecutedAt
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trade = await _tradeRepository.GetByIdAsync(id);

            if(trade == null)
            {
                return NotFound();
            }

            var response = new TradeResponseDTO
            {
                Id = trade.Id,
                UserId = trade.UserId,
                AssetId = trade.AssetId,
                Quantity = trade.Quantity,
                UnitPrice = trade.UnitPrice,
                Fee = trade.Fee,
                TradeType = trade.TradeType,
                ExecutedAt = trade.ExecutedAt
            };

            return Ok(response);   
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


        [HttpGet("filter")]
        public async Task<IActionResult> GetByUserAndAssetLast30Days([FromQuery] int userId, [FromQuery] int assetId)
        {
            if (userId <= 0 || assetId <= 0)
                return BadRequest("UserId and AssetId must be greater than zero.");

            var trades = await _tradeRepository.GetByUserAndAssetLast30DaysAsync(userId, assetId);

            var response = trades.Select(t => new TradeResponseDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                AssetId = t.AssetId,
                Quantity = t.Quantity,
                UnitPrice = t.UnitPrice,
                Fee = t.Fee,
                TradeType = t.TradeType,
                ExecutedAt = t.ExecutedAt
            });

            return Ok(response);
        }

    }
}
