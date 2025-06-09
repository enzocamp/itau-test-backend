using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Infrastructure.Interfaces;
using Itau.Investimentos.Infrastructure.Repositories;
using Itau.Investimentos.Infrastructure.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Itau.Investimentos.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InvestmentsDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                             ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IPositionCalculationService, PositionCalculationService>();

        return services;
    }
}
