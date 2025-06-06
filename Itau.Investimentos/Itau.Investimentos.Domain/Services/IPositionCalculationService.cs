using Itau.Investimentos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Services
{
    public interface IPositionCalculationService
    {
        Task<Position> CalculatePositionAsync(int userId, int assetId);
    }
}
