namespace Demo03.Requests.Application.Contracts.Requests
{
    public sealed record RequestDto(
        Guid Id,
        string Title,
        string Description,
        string Status,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );
}
