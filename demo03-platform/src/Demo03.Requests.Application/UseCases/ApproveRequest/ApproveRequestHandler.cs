using Demo03.Requests.Application.Abstractions;
using Demo03.Requests.Application.Auditing;

namespace Demo03.Requests.Application.UseCases.ApproveRequest
{
    public sealed class ApproveRequestHandler
    {
        private readonly IRequestRepository _repo;
        private readonly IEventBus _bus;
        private readonly IClock _clock;
        private readonly ICacheStore _cache;
        private readonly IUserContext _user;
        private readonly IAuditWriter _audit;

        public ApproveRequestHandler(IRequestRepository repo, IEventBus bus, IClock clock, ICacheStore cache, IUserContext user, IAuditWriter audit)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _audit = audit ?? throw new ArgumentNullException(nameof(audit));
        }

        public async Task HandleAsync(ApproveRequestCommand cmd, CancellationToken ct)
        {
            var request = await _repo.GetByIdAsync(cmd.RequestId, ct)
                ?? throw new InvalidOperationException($"Request '{cmd.RequestId}' not found.");

            var previous = request.Status.ToString();

            request.Approve(cmd.Reason, _clock.UtcNow);

            var current = request.Status.ToString();

            await _repo.SaveChangesAsync(ct);

            await AuditHelper.WriteStatusChangeAsync(
                _audit,
                _user,
                request.Id,
                action: "RequestApproved",
                previousStatus: previous,
                newStatus: current,
                extraData: new { cmd.Reason },
                ct);

            foreach (var ev in request.DomainEvents)
                await _bus.PublishAsync((dynamic)ev, ct);

            request.ClearDomainEvents();
        
            await _cache.RemoveAsync($"requests:{request.Id:D}", ct);
        }
    }
}
