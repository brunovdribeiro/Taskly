using Dapper;
using Npgsql;
using Application.Features.Users.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;
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
        
        var parameters = new
        {
            Id = user.Id.Value,
            user.Name,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.LastModified,
            user.Version
        };

        try
        {
            await connection.ExecuteAsync(sql, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
        SELECT id, name, email, is_active, created_at, last_modified, version
        FROM user_snapshots
        WHERE id = @Id";

        var parameters = new { Id = id.Value };

        try
        {
            var userRow = await connection.QuerySingleOrDefaultAsync(sql, parameters);

            if (userRow is null)
                return null;

            return User.Create(
                UserId.From(userRow.id),
                userRow.email,
                userRow.name,
                userRow.is_active,
                userRow.created_at,
                userRow.last_modified
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}