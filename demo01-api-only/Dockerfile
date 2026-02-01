# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Demo01.Api/Demo01.Api.csproj src/Demo01.Api/
RUN dotnet restore src/Demo01.Api/Demo01.Api.csproj

COPY . .
RUN dotnet publish src/Demo01.Api/Demo01.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Demo01.Api.dll"]