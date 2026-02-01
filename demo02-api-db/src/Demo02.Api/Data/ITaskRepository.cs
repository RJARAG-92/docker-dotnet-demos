using System;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct);
    Task<TaskItem> CreateAsync(string title, CancellationToken ct);
    Task<bool> MarkDoneAsync(Guid id, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct);
}
