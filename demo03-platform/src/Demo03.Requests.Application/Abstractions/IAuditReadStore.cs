using Demo03.Requests.Application.Contracts.Requests.Audit;

namespace Demo03.Requests.Application.Abstractions
{
    public interface IAuditReadStore
    {
        Task<IReadOnlyList<RequestAuditDto>> GetByRequestIdAsync(
            Guid requestId,
            CancellationToken ct);
    }
}
