using Itau.Investimentos.Domain.Entities;

namespace Itau.Investimentos.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(int id);
        Task<IEnumerable<Asset>> GetAllAsync();
        Task AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(int id);
    }
}
