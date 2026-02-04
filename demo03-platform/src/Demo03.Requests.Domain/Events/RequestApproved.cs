using Demo03.Requests.Domain.Enums;

namespace Demo03.Requests.Domain.Events
{
    public record RequestApproved(
      Guid RequestId,
      RequestStatus From,
      RequestStatus To,
      string? Reason,
      DateTimeOffset OccurredAt
  );
}
