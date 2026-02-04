using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Domain.Entities;
using Demo03.Requests.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Demo03.Requests.Infrastructure.Repositories
{
    public sealed class RequestRepository : IRequestRepository
    {
        private readonly RequestsDbContext _db;

        public RequestRepository(RequestsDbContext db) => _db = db;

        public Task<Request?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Requests.SingleOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(Request request, CancellationToken ct)
            => await _db.Requests.AddAsync(request, ct);

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
