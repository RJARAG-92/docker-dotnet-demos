using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Infrastructure.Auditing;
using Demo03.Requests.Infrastructure.Caching;
using Demo03.Requests.Infrastructure.Messaging;
using Demo03.Requests.Infrastructure.Persistence;
using Demo03.Requests.Infrastructure.Repositories;
using Demo03.Requests.Infrastructure.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace Demo03.Requests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestsInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var cs = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");

            services.AddDbContext<RequestsDbContext>(opt => opt.UseNpgsql(cs));

            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddSingleton<IClock, SystemClock>();

            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            services.AddScoped<ICacheStore, DistributedCacheStore>();

            services.AddScoped<IAuditWriter, EfAuditWriter>();
            services.AddScoped<IAuditReadStore, EfAuditReadStore>();
            return services;
        }
    }
}
