using Dapper;
using Npgsql;
using Application.Features.Users.Interfaces;
using Domain.Aggregates;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Postgres;

public class UserSnapshotRepository : IUserSnapshotRepository
{
    private readonly string _connectionString;

    public UserSnapshotRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveSnapshotAsync(User user, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO user_snapshots (id, name, email, created_at, last_modified, version)
            VALUES (@Id, @Name, @Email, @CreatedAt, @LastModified, @Version)";
        
        await connection.ExecuteAsync(sql, user);
    }
}