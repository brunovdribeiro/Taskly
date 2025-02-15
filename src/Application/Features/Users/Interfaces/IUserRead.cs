using Application.Features.Users.Dtos;

namespace Application.Features.Users.Interfaces;

public interface IUserRead
{
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}