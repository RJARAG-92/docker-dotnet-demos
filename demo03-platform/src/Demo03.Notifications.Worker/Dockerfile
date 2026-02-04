# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos csproj primero (cache)
COPY src/Demo03.Notifications.Worker/Demo03.Notifications.Worker.csproj src/Demo03.Notifications.Worker/
RUN dotnet restore src/Demo03.Notifications.Worker/Demo03.Notifications.Worker.csproj

# Copiamos el resto
COPY . .

RUN dotnet publish src/Demo03.Notifications.Worker/Demo03.Notifications.Worker.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Demo03.Notifications.Worker.dll"]
