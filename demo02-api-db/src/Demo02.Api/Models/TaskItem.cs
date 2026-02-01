using System;

public record TaskItem(Guid Id, string Title, bool IsDone, DateTimeOffset CreatedAt);
