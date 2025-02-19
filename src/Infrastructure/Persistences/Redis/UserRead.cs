using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Infrastructure.Persistences.Redis.Documents;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Persistences.Redis;

public class UserRead : IUserRead
{
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<UserDocument> _users;

    public UserRead(
        RedisConnectionProvider provider
    )
    {
        _provider = provider;
        _users = _provider.RedisCollection<UserDocument>();
        CreateIndex().Wait();
    }
    
    private async Task CreateIndex()
    {
        try
        {
            await _provider.Connection.CreateIndexAsync(typeof(UserDocument));
        }
        catch (Exception)
        {
            // Index might already exist, ignore the error
        }
    }

    public async Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var user = await _users.FindByIdAsync(id.ToString());

        if (user == null)
            return null;

        return new UserDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            Name = user.Name,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified
        };
    }
    
    public async Task<IEnumerable<UserDto>> GetAllAsync(
        CancellationToken cancellationToken
    )
    {
        var users = await _users.ToListAsync();
    
        return users.Select(user => new UserDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            Name = user.Name,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified
        });
    }
}