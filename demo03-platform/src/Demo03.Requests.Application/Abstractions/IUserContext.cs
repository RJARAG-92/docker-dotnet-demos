namespace Demo03.Requests.Application.Abstractions
{
    public interface IUserContext
    {
        string Actor { get; }
        string? CorrelationId { get; }
    }
}
