using Demo03.Requests.Domain.Entities;

namespace Demo03.Requests.Application.Abstractions
{
    public interface IRequestRepository
    {
        Task<Request?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Request request, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
