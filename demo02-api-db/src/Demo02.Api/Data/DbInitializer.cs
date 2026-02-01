using Npgsql;

public static class DbInitializer
{
    public static async Task EnsureCreatedAsync(string connectionString, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync(ct);

        const string sql = """
        CREATE TABLE IF NOT EXISTS tasks (
            id uuid PRIMARY KEY,
            title text NOT NULL,
            is_done boolean NOT NULL DEFAULT false,
            created_at timestamptz NOT NULL DEFAULT now()
        );
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}