using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Infrastructure.Persistence.Redis.Documents;
using Infrastructure.Persistence.Redis.Interfaces;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Persistence.Redis;

public class UserRead : IUserRead, IUserDocumentRepository
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

    private async Task<UserDocument?> ByIdAsync(Guid id)
    {
        var user = await _users.FindByIdAsync(id.ToString());

        return user;
    }

    public async Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var user = await ByIdAsync(id);

        if (user is null)
        {
            return null;
        }

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

    async Task<UserDocument?> IUserDocumentRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await ByIdAsync(id);

        return user;
    }

    private async Task<IEnumerable<UserDocument>> UserDocuments()
    {
        var users = await _users.ToListAsync();
        return users;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(
        CancellationToken cancellationToken
    )
    {
        var users = await UserDocuments();

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

    Task<IEnumerable<UserDocument>> IUserDocumentRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        return UserDocuments();
    }


    public async Task AddAsync(UserDocument document, CancellationToken cancellationToken)
    {
        await _users.InsertAsync(document, WhenKey.Always);
    }

    public async Task UpdateAsync(UserDocument document, CancellationToken cancellationToken)
    {
        await _users.UpdateAsync(document);
    }
}