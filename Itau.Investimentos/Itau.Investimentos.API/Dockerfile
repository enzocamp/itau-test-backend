FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Itau.Investimentos/Itau.Investimentos.API/Itau.Investimentos.API.csproj Itau.Investimentos.API/
COPY Itau.Investimentos/Itau.Investimentos.Domain/Itau.Investimentos.Domain.csproj Itau.Investimentos.Domain/
COPY Itau.Investimentos/Itau.Investimentos.Infrastructure/Itau.Investimentos.Infrastructure.csproj Itau.Investimentos.Infrastructure/

RUN dotnet restore "Itau.Investimentos.API/Itau.Investimentos.API.csproj"

COPY . .

WORKDIR "/src/Itau.Investimentos/Itau.Investimentos.API"
RUN dotnet build "Itau.Investimentos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Itau.Investimentos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Itau.Investimentos.API.dll"]
