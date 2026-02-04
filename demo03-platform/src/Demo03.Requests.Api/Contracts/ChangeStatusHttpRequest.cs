namespace Demo03.Requests.Api.Contracts
{
    public sealed record ChangeStatusHttpRequest(
       string? Reason,
       string? ChangedBy
   );
}
