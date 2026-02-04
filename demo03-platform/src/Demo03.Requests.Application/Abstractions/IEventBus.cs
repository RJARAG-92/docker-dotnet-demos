namespace Demo03.Requests.Application.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken ct) where T : class;
    }
}
