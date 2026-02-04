namespace Demo03.Requests.Application.Abstractions
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
