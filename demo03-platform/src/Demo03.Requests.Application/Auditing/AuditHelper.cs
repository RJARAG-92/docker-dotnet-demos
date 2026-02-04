using Demo03.Requests.Application.Abstractions;

namespace Demo03.Requests.Application.Auditing
{
    public static class AuditHelper
    {
        public static Task WriteStatusChangeAsync(
            IAuditWriter audit,
            IUserContext user,
            Guid requestId,
            string action,
            string? previousStatus,
            string newStatus,
            object? extraData,
            CancellationToken ct)
        {
            return audit.WriteAsync(
                requestId,
                action,
                user.Actor,
                user.CorrelationId,
                new
                {
                    PreviousStatus = previousStatus,
                    NewStatus = newStatus,
                    Extra = extraData
                },
                ct);
        }
    }
}
