using Demo03.Requests.Api.Middlewares;
using Demo03.Requests.Api.System;
using Demo03.Requests.Application;
using Demo03.Requests.Application.Abstractions; 
using Demo03.Requests.Infrastructure;
using Demo03.Requests.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddRequestsInfrastructure(builder.Configuration);
builder.Services.AddRequestsApplication();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration["Cache:Redis"];
});

var app = builder.Build();

DatabaseInitializer.ApplyMigrations(app.Services);

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
