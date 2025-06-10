FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando os arquivos de projeto
COPY Itau.Investimentos/Itau.Investimentos.Worker/Itau.Investimentos.Worker.csproj ./Itau.Investimentos.Worker/
COPY Itau.Investimentos/Itau.Investimentos.Domain/Itau.Investimentos.Domain.csproj ./Itau.Investimentos.Domain/
COPY Itau.Investimentos/Itau.Investimentos.Infrastructure/Itau.Investimentos.Infrastructure.csproj ./Itau.Investimentos.Infrastructure/

# Restaurando as dependências
RUN dotnet restore ./Itau.Investimentos.Worker/Itau.Investimentos.Worker.csproj

# Copiando o restante dos arquivos
COPY . .

# Compilando e publicando
WORKDIR /src/Itau.Investimentos/Itau.Investimentos.Worker
RUN dotnet build "Itau.Investimentos.Worker.csproj" -c Release -o /app/build
RUN dotnet publish "Itau.Investimentos.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Itau.Investimentos.Worker.dll"]
