using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Services
{
    public interface IPositionService
    {
        Task RecalculatePnLAsync(int assetId, decimal latestUnitPrice);
    }
}
