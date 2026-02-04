namespace Demo03.Requests.Domain.Entities
{
    public sealed class AuditLog
    {
        public long Id { get; set; }
        public Guid RequestId { get; set; }
        public string Action { get; set; } = default!;
        public string Actor { get; set; } = "system";
        public DateTimeOffset At { get; set; }
        public string? CorrelationId { get; set; }
        public string? DataJson { get; set; }
    }
}
