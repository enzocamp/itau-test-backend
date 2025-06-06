using Itau.Investimentos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Interfaces
{
    public interface IPositionRepository
    {
        Task<Position?> GetByUserAndAssetAsync(int userId, int assetId);
        Task AddOrUpdateAsync(Position position);
    }
}
