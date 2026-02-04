namespace Demo03.Requests.Application.Contracts.Requests
{
    public sealed record CreateRequestDto(
        string Title,
        string? Description,
        string? CreatedBy
    );
}
