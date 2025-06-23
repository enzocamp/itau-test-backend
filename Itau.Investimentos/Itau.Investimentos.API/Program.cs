using Itau.Investimentos.API.Extensions;
using System.Text.Json.Serialization;
using Itau.Investimentos.Infrastructure.Messaging.Interfaces;
using Itau.Investimentos.Infrastructure.Messaging.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ita� Investimentos API",
        Version = "v1",
        Description = "API REST para c�lculo de posi��o, opera��es, cota��es e lucro/preju�zo de ativos de investimento.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Desafio T�cnico Ita�",
            Url = new Uri("https://github.com/enzocamp/itau-test-backend")
        }
    });
});

// CORS para permitir o frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Kafka Producer
builder.Services.AddSingleton<IMessageProducer, KafkaProducerService>();


// Application Services e Inje��es gerais
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAll");

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
