using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Exceptions;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly InvestmentsDbContext _context;

        public PositionRepository(InvestmentsDbContext context)
        {
            _context = context;
        }
        public async Task AddOrUpdateAsync(Position position)
        {
            try
            {
                var existing = await GetByUserAndAssetAsync(position.UserId, position.AssetId);

                if (existing == null)
                {
                    await _context.Positions.AddAsync(position);
                }
                else
                {
                    existing.Quantity = position.Quantity;
                    existing.AveragePrice = position.AveragePrice;
                    existing.PnL = position.PnL;
                    _context.Positions.Update(existing);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error saving position.", ex);
            }
        }

        public async Task<Position?> GetByUserAndAssetAsync(int userId, int assetId)
        {
            try
            {
                return await _context.Positions
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetId == assetId);
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error fetching position by user and asset.", ex);
            }
        }
    }
}
