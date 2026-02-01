using Npgsql;

public sealed class TaskRepository : ITaskRepository
{
    private readonly string _cs;

    public TaskRepository(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString("Default")
              ?? cfg["CONNECTION_STRING"]
              ?? throw new InvalidOperationException("Missing connection string (ConnectionStrings:Default or CONNECTION_STRING).");
    }

    public async Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
        SELECT id, title, is_done, created_at
        FROM tasks
        ORDER BY created_at DESC;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);

        var items = new List<TaskItem>();
        while (await reader.ReadAsync(ct))
        {
            items.Add(new TaskItem(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetBoolean(2),
                reader.GetFieldValue<DateTimeOffset>(3)
            ));
        }

        return items;
    }

    public async Task<TaskItem> CreateAsync(string title, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("title is required", nameof(title));

        var id = Guid.NewGuid();
        var normalized = title.Trim();

        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
        INSERT INTO tasks (id, title)
        VALUES (@id, @title)
        RETURNING created_at;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("title", normalized);

        var scalar = await cmd.ExecuteScalarAsync(ct);
        var createdAt = scalar switch
        {
            DateTimeOffset dto => dto,
            DateTime dt => new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc)),
            _ => DateTimeOffset.UtcNow
        };

        return new TaskItem(id, normalized, false, createdAt);
    }

    public async Task<bool> MarkDoneAsync(Guid id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
        UPDATE tasks
        SET is_done = true
        WHERE id = @id;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        return await cmd.ExecuteNonQueryAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
        DELETE FROM tasks
        WHERE id = @id;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        return await cmd.ExecuteNonQueryAsync(ct) > 0;
    }
    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
    SELECT id, title, is_done, created_at
    FROM tasks
    WHERE id = @id;
    """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync(ct);

        if (!await reader.ReadAsync(ct))
            return null;

        return new TaskItem(
            reader.GetGuid(0),
            reader.GetString(1),
            reader.GetBoolean(2),
            reader.GetFieldValue<DateTimeOffset>(3)
        );
    }
}