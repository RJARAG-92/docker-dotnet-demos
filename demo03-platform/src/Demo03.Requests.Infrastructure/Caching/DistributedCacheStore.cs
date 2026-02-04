using Demo03.Requests.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Demo03.Requests.Infrastructure.Caching
{
    public sealed class DistributedCacheStore : ICacheStore
    {
        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);
        private readonly IDistributedCache _cache;

        public DistributedCacheStore(IDistributedCache cache) => _cache = cache;

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
        {
            var json = await _cache.GetStringAsync(key, ct);
            return string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json, JsonOpts);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(value, JsonOpts);
            return _cache.SetStringAsync(key, json,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl }, ct);
        }

        public Task RemoveAsync(string key, CancellationToken ct)
            => _cache.RemoveAsync(key, ct);
    }
}
