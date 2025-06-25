using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Exceptions;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Itau.Investimentos.Infrastructure.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly InvestmentsDbContext _context;

        public TradeRepository(InvestmentsDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Trade trade)
        {
            try
            {
                await _context.Trades.AddAsync(trade);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error adding trade.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var trade = await GetByIdAsync(id);
                if (trade != null)
                {
                    _context.Trades.Remove(trade);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error deleting trade ", ex);
            }
        }

        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            try
            {
                return await _context.Trades
                    .Include(t => t.Asset)
                    .Include(t => t.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing trades ", ex);
            }
        }

        public async Task<Trade?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Trades
                .Include(t => t.Asset)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error fetching trade by ID.", ex);
            }
        }

        public async Task<IEnumerable<Trade>> GetByUserAndAssetLast30DaysAsync(int userId, int assetId)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-30);
                return await _context.Trades
                    .Where(t => t.UserId == userId && t.AssetId == assetId && t.ExecutedAt >= cutoffDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing trades for user and asset in the last 30 days.", ex);
            }
        }

        public async Task<IEnumerable<Trade>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Trades
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing trades by user ID.", ex);
            }
        }

        public async Task UpdateAsync(Trade trade)
        {
            try
            {
                if (await GetByIdAsync(trade.Id) != null)
                {
                    _context.Trades.Update(trade);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error updating trade.", ex);
            }
        }
    }
}
