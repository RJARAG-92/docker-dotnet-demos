using Demo03.Requests.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo03.Requests.Infrastructure.Persistence
{
    public sealed class RequestsDbContext : DbContext
    {
        public RequestsDbContext(DbContextOptions<RequestsDbContext> options) : base(options) { }
        public DbSet<Request> Requests => Set<Request>();
        public DbSet<ProcessedEvent> ProcessedEvents => Set<ProcessedEvent>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RequestsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
