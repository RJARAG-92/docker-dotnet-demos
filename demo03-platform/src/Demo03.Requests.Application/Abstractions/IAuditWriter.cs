namespace Demo03.Requests.Application.Abstractions
{
    public interface IAuditWriter
    {
        Task WriteAsync(Guid requestId, string action, string actor, string? correlationId, object? data, CancellationToken ct);
    }
}
