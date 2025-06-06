using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Exceptions;
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
    internal class QuoteRepository : IQuoteRepository
    {
        private readonly InvestmentsDbContext _context;

        public QuoteRepository(InvestmentsDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Quote quote)
        {
            try
            {
                await _context.Quotes.AddAsync(quote);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error adding quote.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var quote = await GetByIdAsync(id);
                if (quote is not null)
                {
                    _context.Quotes.Remove(quote);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error deleting quote ", ex);
            }
        }

        public async Task<IEnumerable<Quote>> GetAllAsync()
        {
            try
            {
                return await _context.Quotes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing quotes.", ex);
            }
        }

        public async Task<IEnumerable<Quote>> GetByAssetIdAsync(int assetId)
        {
            try
            {
               return await _context.Quotes
                    .Where(q => q.AssetId == assetId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error listing quotes by asset ID.", ex);
            }
        }

        public async Task<Quote?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Quotes.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error fetching quote by ID.", ex);
            }
        }

        public async Task<Quote?> GetLastQuoteByAssetIdAsync(int assetId)
        {
            try
            {
                return await _context.Quotes
                    .Where(q => q.AssetId == assetId)
                    .OrderByDescending(q => q.QuotedAt)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error fetching last quote by asset ID.", ex);
            }
        }
    }
}
