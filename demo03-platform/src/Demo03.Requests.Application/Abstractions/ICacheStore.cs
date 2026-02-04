namespace Demo03.Requests.Application.Abstractions
{
    public interface ICacheStore
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct);
        Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct);
        Task RemoveAsync(string key, CancellationToken ct);
    }
}
