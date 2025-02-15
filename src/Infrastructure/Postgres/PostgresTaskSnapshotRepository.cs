using Dapper;
using Npgsql;
using Application.Common.Interfaces;
using Domain.ValueObjects;

namespace Infrastructure.Postgres;

public class PostgresTaskSnapshotRepository : ITaskSnapshotRepository
{
    private readonly string _connectionString;

    public PostgresTaskSnapshotRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Domain.Aggregates.Task?> GetLatestSnapshotAsync(TaskId id, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM task_snapshots WHERE id = @Id ORDER BY version DESC LIMIT 1";
        return await connection.QueryFirstOrDefaultAsync<Domain.Aggregates.Task>(sql, new { Id = id.Value });
    }

    public async Task SaveSnapshotAsync(Domain.Aggregates.Task task, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO task_snapshots (id, title, description, status, priority, created_at, last_modified, assigned_to, version)
            VALUES (@Id, @Title, @Description, @Status, @Priority, @CreatedAt, @LastModified, @AssignedTo, @Version)";
        
        await connection.ExecuteAsync(sql, task);
    }
}