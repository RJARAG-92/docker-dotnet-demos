namespace Demo03.Requests.Domain.Entities
{
    public sealed class ProcessedEvent
    {
        public string MessageId { get; set; } = default!;
        public DateTimeOffset ProcessedAt { get; set; }
    }
}
