# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos csproj primero para aprovechar cache
COPY src/Demo03.Requests.Api/Demo03.Requests.Api.csproj src/Demo03.Requests.Api/
COPY src/Demo03.Requests.Application/Demo03.Requests.Application.csproj src/Demo03.Requests.Application/
COPY src/Demo03.Requests.Infrastructure/Demo03.Requests.Infrastructure.csproj src/Demo03.Requests.Infrastructure/
COPY src/Demo03.Requests.Domain/Demo03.Requests.Domain.csproj src/Demo03.Requests.Domain/

RUN dotnet restore src/Demo03.Requests.Api/Demo03.Requests.Api.csproj

# Copiamos el resto
COPY . .

RUN dotnet publish src/Demo03.Requests.Api/Demo03.Requests.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

# ASPNETCORE_URLS para escuchar en el puerto del contenedor
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Demo03.Requests.Api.dll"]