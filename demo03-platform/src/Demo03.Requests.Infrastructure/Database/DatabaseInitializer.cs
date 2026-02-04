using Demo03.Requests.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demo03.Requests.Infrastructure.Database
{
    public static class DatabaseInitializer
    {
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RequestsDbContext>();
            db.Database.Migrate();
        }
    }
}
