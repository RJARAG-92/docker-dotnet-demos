namespace Demo03.Requests.Api.Contracts
{
    public sealed record CreateRequestHttpRequest(
        string Title,
        string? Description,
        string? CreatedBy
    );
}
