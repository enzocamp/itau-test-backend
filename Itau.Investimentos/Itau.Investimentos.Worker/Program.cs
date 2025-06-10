using Itau.Investimentos.Worker;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Infrastructure.Interfaces;
using Itau.Investimentos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Itau.Investimentos.Worker.Workers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Connection String do appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<InvestmentsDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IQuoteRepository, QuoteRepository>();

        services.AddHostedService<QuoteConsumerWorker>();
    })
    .Build();

await host.RunAsync();