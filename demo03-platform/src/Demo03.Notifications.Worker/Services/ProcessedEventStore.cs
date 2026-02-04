using Demo03.Notifications.Worker.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Demo03.Notifications.Worker.Services
{
    public sealed class ProcessedEventStore
    {
        private readonly string _cs;

        public ProcessedEventStore(IOptions<PostgresOptions> opt)
            => _cs = opt.Value.ConnectionString;

        public async Task EnsureSchemaAsync(CancellationToken ct)
        {
            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync(ct);

            const string sql = """
        CREATE TABLE IF NOT EXISTS processed_events (
          message_id TEXT PRIMARY KEY,
          processed_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
        );
        """;

            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task<bool> TryMarkProcessedAsync(string messageId, CancellationToken ct)
        {
            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync(ct);

            const string sql = """
        INSERT INTO processed_events(message_id) VALUES (@id)
        ON CONFLICT (message_id) DO NOTHING;
        """;

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", messageId);

            var rows = await cmd.ExecuteNonQueryAsync(ct);
            return rows == 1; // true si se insertó, false si ya existía
        }
    }
}
