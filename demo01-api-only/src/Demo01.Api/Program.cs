using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "OK" }));

app.MapGet("/info", (IConfiguration cfg) =>
{
    var appName = cfg["APP_NAME"] ?? "Demo01.Api";
    var version = cfg["APP_VERSION"] ?? (Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0");
    var greetingPrefix = cfg["GREETING_PREFIX"] ?? "Hola";

    return Results.Ok(new
    {
        appName,
        version,
        environment = app.Environment.EnvironmentName,
        greetingPrefix,
        serverTimeUtc = DateTimeOffset.UtcNow,
        machineName = Environment.MachineName
    });
});

app.MapGet("/greet", (string? name, IConfiguration cfg) =>
{
    var prefix = cfg["GREETING_PREFIX"] ?? "Hola";
    name = string.IsNullOrWhiteSpace(name) ? "mundo" : name.Trim();

    return Results.Ok(new { message = $"{prefix}, {name}!", atUtc = DateTimeOffset.UtcNow });
});

app.Run();
