using Demo03.Requests.Domain.Enums;

namespace Demo03.Requests.Domain.Events
{
    public record RequestRejected(
        Guid RequestId,
        RequestStatus From,
        RequestStatus To,
        string? Reason,
        DateTimeOffset OccurredAt
    );
}
