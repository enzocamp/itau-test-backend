using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionCalculationService _calculationService;
        private readonly IPositionRepository _positionRepository;

        public PositionController(IPositionCalculationService calculationService, IPositionRepository positionRepository)
        {
            _calculationService = calculationService;
            _positionRepository = positionRepository;
        }

        [HttpGet("{userId:int}/{assetId:int}")]
        public async Task<ActionResult<Position>> Get(int userId, int assetId)
        {
            try
            {
                var position = await _calculationService.CalculatePositionAsync(userId, assetId);

                await _positionRepository.AddOrUpdateAsync(position);

                return Ok(position);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to calculate position: {ex.Message}");
            }
        }

    }
}
