namespace Demo03.Requests.Application.UseCases.SubmitRequest
{
    public sealed record SubmitRequestCommand(
        Guid RequestId,
        string? Reason,
        string? ChangedBy
    );
}
