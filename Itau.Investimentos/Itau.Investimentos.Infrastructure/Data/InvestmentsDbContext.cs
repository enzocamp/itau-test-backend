using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itau.Investimentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Itau.Investimentos.Infrastructure.Data
{
    public class InvestmentsDbContext : DbContext
    {
        public InvestmentsDbContext(DbContextOptions<InvestmentsDbContext> options)
    : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
