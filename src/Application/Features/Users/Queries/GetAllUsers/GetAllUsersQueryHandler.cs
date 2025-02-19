using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(
    IUserRead read
) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        return await read.GetAllAsync(cancellationToken);
    }
}