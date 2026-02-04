using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Application.Contracts.Requests.Audit;
using Demo03.Requests.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Demo03.Requests.Infrastructure.Auditing
{
    public sealed class EfAuditReadStore : IAuditReadStore
    {
        private readonly RequestsDbContext _db;

        public EfAuditReadStore(RequestsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IReadOnlyList<RequestAuditDto>> GetByRequestIdAsync(
            Guid requestId,
            CancellationToken ct)
        {
            return await _db.AuditLogs
                .AsNoTracking()
                .Where(x => x.RequestId == requestId)
                .OrderByDescending(x => x.At)
                .Select(x => new RequestAuditDto
                {
                    Id = x.Id,
                    Action = x.Action,
                    Actor = x.Actor,
                    At = x.At,
                    CorrelationId = x.CorrelationId,
                    DataJson = x.DataJson
                })
                .ToListAsync(ct);
        }
    }
}
