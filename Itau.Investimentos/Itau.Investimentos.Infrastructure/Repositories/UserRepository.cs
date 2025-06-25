using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Exceptions;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InvestmentsDbContext _context;

        public UserRepository(InvestmentsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new DbOperationException("An error occured while saving the user", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error deleting user ", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error trying to list users", ex);
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);

            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error trying to find user id", ex);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                if (GetByIdAsync(user.Id) != null)
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DbOperationException("Error updating the user", ex);
            }
        }
    }
}
