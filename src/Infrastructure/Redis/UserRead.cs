using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Redis;

public class UserRead : IUserRead
{
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<UserDocument> _users;

    public UserRead(RedisConnectionProvider provider)
    {
        _provider = provider;
        _users = _provider.RedisCollection<UserDocument>();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user =  await _users.FindByIdAsync(id.ToString());
        
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
}