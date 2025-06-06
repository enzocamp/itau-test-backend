using Itau.Investimentos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Interfaces
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
