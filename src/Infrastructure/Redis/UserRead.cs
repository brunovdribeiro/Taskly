using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Redis;

public class UserRead : IUserRead
{
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<UserDto> _users;

    public UserRead(RedisConnectionProvider provider)
    {
        _provider = provider;
        _users = _provider.RedisCollection<UserDto>();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _users.FindByIdAsync(id.ToString());
    }
}