using Itau.Investimentos.Domain.Entities;

namespace Itau.Investimentos.Domain.Interfaces
{
    public interface IPositionRepository
    {
        Task<Position?> GetByUserAndAssetAsync(int userId, int assetId);
        Task AddOrUpdateAsync(Position position);
        Task<List<Position>> GetByAssetIdAsync(int assetId);
    }
}
