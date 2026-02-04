using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Domain.Entities;
using Demo03.Requests.Infrastructure.Persistence;
using System.Text.Json;

namespace Demo03.Requests.Infrastructure.Auditing
{
    public sealed class EfAuditWriter : IAuditWriter
    {
        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        private readonly RequestsDbContext _db;

        public EfAuditWriter(RequestsDbContext db) => _db = db;

        public async Task WriteAsync(
            Guid requestId,
            string action,
            string actor,
            string? correlationId,
            object? data,
            CancellationToken ct)
        {
            var row = new AuditLog
            {
                RequestId = requestId,
                Action = action,
                Actor = string.IsNullOrWhiteSpace(actor) ? "system" : actor,
                CorrelationId = correlationId,
                DataJson = data is null ? null : JsonSerializer.Serialize(data, JsonOpts),
                At = DateTimeOffset.UtcNow
            };

            _db.AuditLogs.Add(row);
            await _db.SaveChangesAsync(ct);
        }
    }
}
