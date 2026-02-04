namespace Demo03.Requests.Application.Contracts.Requests.Audit
{
    public sealed class RequestAuditDto
    {
        public long Id { get; init; }
        public string Action { get; init; } = default!;
        public string Actor { get; init; } = default!;
        public DateTimeOffset At { get; init; }
        public string? CorrelationId { get; init; }
        public string? DataJson { get; init; }
    }
}
