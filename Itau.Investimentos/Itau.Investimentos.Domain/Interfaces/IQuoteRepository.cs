using Itau.Investimentos.Domain.Entities;

namespace Itau.Investimentos.Domain.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote?> GetByIdAsync(int id);
        Task<IEnumerable<Quote>> GetAllAsync();
        Task<IEnumerable<Quote>> GetByAssetIdAsync(int assetId);
        Task<Quote?> GetLastQuoteByAssetIdAsync(int assetId);
        Task AddAsync(Quote quote);
        Task DeleteAsync(int id);
    }
}
