using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            // Forçar enum TradeType como UPPERCASE no banco
            var tradeTypeConverter = new EnumToStringConverter<TradeType>();

            modelBuilder.Entity<Trade>()
            .Property(t => t.TradeType)
            .HasConversion(new EnumToStringConverter<TradeType>());

        }
    }
}
