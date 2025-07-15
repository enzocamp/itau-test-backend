using Itau.Investimentos.API.DTOs;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Interfaces;
using Itau.Investimentos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Investimentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITradeRepository _tradeRepository;

        public UserController(IUserRepository userRepository, ITradeRepository tradeRepository)
        {
            _userRepository = userRepository;
            _tradeRepository = tradeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                FeePercentage = dto.FeePercentage
            };
            await _userRepository.AddAsync(user);

            var response = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                FeePercentage = user.FeePercentage
            };

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var response = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                FeePercentage = user.FeePercentage
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            if(users == null)
            {
                return NotFound();
            }

            var response = users.Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                FeePercentage = user.FeePercentage
            });

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO dto)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.FeePercentage = dto.FeePercentage;

            await _userRepository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var userTrades = await _tradeRepository.GetByUserIdAsync(id);
            if (userTrades.Any())
                return BadRequest("Cannot delete user with existing trades.");

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
