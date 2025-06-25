using Itau.Investimentos.Domain.Entities;

namespace Itau.Investimentos.Domain.Interfaces
{
    public interface ITradeRepository
    {
        Task<Trade?> GetByIdAsync(int id);
        Task<IEnumerable<Trade>> GetAllAsync();
        Task<IEnumerable<Trade>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Trade>> GetByUserAndAssetLast30DaysAsync(int userId, int assetId);
        Task AddAsync(Trade trade);
        Task UpdateAsync(Trade trade);
        Task DeleteAsync(int id);
    }
}
