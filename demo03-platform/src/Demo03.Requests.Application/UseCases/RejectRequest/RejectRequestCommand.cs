namespace Demo03.Requests.Application.UseCases.RejectRequest
{
    public sealed record RejectRequestCommand(
        Guid RequestId,
        string? Reason,
        string? ChangedBy
    );
}
