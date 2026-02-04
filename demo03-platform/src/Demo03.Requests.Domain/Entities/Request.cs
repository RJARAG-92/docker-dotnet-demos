using Demo03.Requests.Domain.Enums;
using Demo03.Requests.Domain.Events;

namespace Demo03.Requests.Domain.Entities
{
    public class Request
    {
        private readonly List<object> _domainEvents = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = default!;
        public string Description { get; private set; } = string.Empty;
        public RequestStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        // Constructor que EF Core puede usar (solo params que mapean a propiedades)
        // EF puede usar constructor sin parámetros o uno con parámetros "bindables".
        private Request() { }

        // Constructor de dominio (para crear nuevas Requests)
        public Request(Guid id, string title, string description, DateTimeOffset now)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(id));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));

            Id = id;
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;

            Status = RequestStatus.Draft;
            CreatedAt = now;
            UpdatedAt = now;
        }

        public void UpdateDetails(string title, string description, DateTimeOffset now)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            UpdatedAt = now;
        }

        public void Submit(string? reason, DateTimeOffset now)
        {
            EnsureTransitionAllowed(RequestStatus.Submitted);

            var from = Status;
            Status = RequestStatus.Submitted;
            UpdatedAt = now;

            _domainEvents.Add(new RequestSubmitted(Id, from, Status, reason, now));
        }

        public void Approve(string? reason, DateTimeOffset now)
        {
            EnsureTransitionAllowed(RequestStatus.Approved);

            var from = Status;
            Status = RequestStatus.Approved;
            UpdatedAt = now;

            _domainEvents.Add(new RequestApproved(Id, from, Status, reason, now));
        }

        public void Reject(string? reason, DateTimeOffset now)
        {
            EnsureTransitionAllowed(RequestStatus.Rejected);

            var from = Status;
            Status = RequestStatus.Rejected;
            UpdatedAt = now;

            _domainEvents.Add(new RequestRejected(Id, from, Status, reason, now));
        }

        public void ClearDomainEvents() => _domainEvents.Clear();

        private void EnsureTransitionAllowed(RequestStatus target)
        {
            var ok = (Status, target) switch
            {
                (RequestStatus.Draft, RequestStatus.Submitted) => true,
                (RequestStatus.Submitted, RequestStatus.Approved) => true,
                (RequestStatus.Submitted, RequestStatus.Rejected) => true,
                _ => false
            };

            if (!ok)
                throw new InvalidOperationException($"Invalid transition: {Status} -> {target}");
        }
    }
}
