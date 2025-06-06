using Itau.Investimentos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Interfaces
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
