# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Demo02.Api/Demo02.Api.csproj src/Demo02.Api/
RUN dotnet restore src/Demo02.Api/Demo02.Api.csproj

COPY . .
RUN dotnet publish src/Demo02.Api/Demo02.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Demo02.Api.dll"]
