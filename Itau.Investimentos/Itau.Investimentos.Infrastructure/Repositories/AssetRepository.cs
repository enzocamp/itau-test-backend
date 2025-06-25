using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Exceptions;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Itau.Investimentos.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly InvestmentsDbContext _context;

        public AssetRepository(InvestmentsDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Asset asset)
        {
            try
            {
                await _context.Assets.AddAsync(asset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error adding new asset", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var asset = await GetByIdAsync(id);

                if (asset != null)
                {
                    _context.Assets.Remove(asset);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error deleting asset", ex);
            }
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            try
            {
                return await _context.Assets.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing assets", ex);
            }
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Assets.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error fetching asset by id", ex);
            }
        }

        public async Task UpdateAsync(Asset asset)
        {
            try
            {
                if (GetByIdAsync(asset.Id) != null)
                {
                    _context.Assets.Update(asset);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error trying updating asset", ex);
            }
        }
    }
}
