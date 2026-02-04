using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Application.Auditing;
using Demo03.Requests.Application.Contracts.Requests;
using Demo03.Requests.Domain.Entities;

namespace Demo03.Requests.Application.UseCases.CreateRequest
{
    public sealed class CreateRequestHandler
    {
        private readonly IRequestRepository _repo;
        private readonly IClock _clock;
        private readonly ICacheStore _cache;
        private readonly IUserContext _user;
        private readonly IAuditWriter _audit;

        public CreateRequestHandler(IRequestRepository repo, IClock clock, ICacheStore cache, IUserContext user, IAuditWriter audit)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _audit = audit ?? throw new ArgumentNullException(nameof(audit));
        }

        public async Task<RequestDto> HandleAsync(CreateRequestCommand cmd, CancellationToken ct)
        {
            var title = cmd.Data.Title?.Trim();
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(cmd));

            var description = cmd.Data.Description?.Trim() ?? string.Empty;

            var now = _clock.UtcNow;

            var request = new Request(
                id: Guid.NewGuid(),
                title: title,
                description: description,
                now: now
            );

            await _repo.AddAsync(request, ct);
            await _repo.SaveChangesAsync(ct);

            await AuditHelper.WriteStatusChangeAsync(
                _audit,
                _user,
                request.Id,
                "RequestCreated",
                previousStatus: null,
                newStatus: request.Status.ToString(),
                extraData: new { request.Title },
                ct);

            await _cache.RemoveAsync($"requests:{request.Id:D}", ct);

            return new RequestDto(
                Id: request.Id,
                Title: request.Title,
                Description: request.Description,
                Status: request.Status.ToString(),
                CreatedAt: request.CreatedAt,
                UpdatedAt: request.UpdatedAt
            );
        }
    }
}
