namespace Demo03.Requests.Application.UseCases.ApproveRequest
{
    public sealed record ApproveRequestCommand(
        Guid RequestId,
        string? Reason,
        string? ChangedBy
    );
}
