using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Application.Contracts.Requests.Audit;

namespace Demo03.Requests.Application.UseCases.GetRequestAudit
{
    public sealed class GetRequestAuditHandler
    {
        private readonly IAuditReadStore _store;

        public GetRequestAuditHandler(IAuditReadStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<IReadOnlyList<RequestAuditDto>> HandleAsync(
            GetRequestAuditQuery query,
            CancellationToken ct)
        {
            return await _store.GetByRequestIdAsync(query.RequestId, ct);
        }
    }
}
