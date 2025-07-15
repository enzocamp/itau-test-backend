using Itau.Investimentos.Worker;
using Itau.Investimentos.Infrastructure.Data;
using Itau.Investimentos.Domain.Interfaces;
using Itau.Investimentos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Itau.Investimentos.Worker.Workers;
using MySqlConnector;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Connection String do appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var retries = 10;
        while (retries > 0)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open(); // se abrir, sai do loop
                Console.WriteLine("Conectado ao MySQL com sucesso.");
                break;
            }
            catch (Exception ex)
            {
                retries--;
                Console.WriteLine($"Aguardando MySQL... Tentativas restantes: {retries}");
                Console.WriteLine($"Erro: {ex.Message}");
                Thread.Sleep(5000); // espera 5 segundos
            }
        }

        services.AddDbContext<InvestmentsDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
             mysqlOptions =>
             {
                 mysqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 10,                      // tenta até 10 vezes
                     maxRetryDelay: TimeSpan.FromSeconds(5), // espera 5s entre cada tentativa
                     errorNumbersToAdd: null
                 );
             }));

        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        // Serviço de cálculo
        services.AddScoped<IPositionCalculationService, PositionCalculationService>();

        services.AddHostedService<QuoteConsumerWorker>();


    })
    .Build();

await host.RunAsync();