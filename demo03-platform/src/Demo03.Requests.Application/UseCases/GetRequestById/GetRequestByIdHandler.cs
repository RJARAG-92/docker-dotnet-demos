using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Application.Contracts.Requests;

namespace Demo03.Requests.Application.UseCases.GetRequestById
{
    public sealed class GetRequestByIdHandler
    {
        private readonly IRequestRepository _repo;
        private readonly ICacheStore _cache;

        public GetRequestByIdHandler(IRequestRepository repo, ICacheStore cache)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<RequestDto> HandleAsync(GetRequestByIdQuery query, CancellationToken ct)
        {
            var key = $"requests:{query.RequestId:D}";
            var cached = await _cache.GetAsync<RequestDto>(key, ct);
            if (cached is not null) return cached;

            var request = await _repo.GetByIdAsync(query.RequestId, ct)
                ?? throw new InvalidOperationException($"Request '{query.RequestId}' not found.");

            var dto = new RequestDto(
                Id: request.Id,
                Title: request.Title,
                Description: request.Description,
                Status: request.Status.ToString(),
                CreatedAt: request.CreatedAt,
                UpdatedAt: request.UpdatedAt
            );

            await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(5), ct);

            return dto;
        }
    }
}
