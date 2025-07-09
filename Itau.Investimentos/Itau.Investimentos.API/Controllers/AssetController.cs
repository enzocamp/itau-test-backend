using Itau.Investimentos.API.DTOs;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;

        public AssetController(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetDTO dto)
        {
            var asset = new Asset
            {
                Code = dto.Code,
                Name = dto.Name
            };

            await _assetRepository.AddAsync(asset);

            var response = new AssetResponseDTO
            {
                Id = asset.Id,
                Code = asset.Code,
                Name = asset.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();

            var response = new AssetResponseDTO
            {
                Id = asset.Id,
                Code = asset.Code,
                Name = asset.Name
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetRepository.GetAllAsync();

            if (assets == null)
            {
                return NoContent();
            }

            var response = assets.Select(a => new AssetResponseDTO
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name
            });

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssetDTO dto)
        {
            var existing = await _assetRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Code = dto.Code;
            existing.Name = dto.Name;

            await _assetRepository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();

            await _assetRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}

