using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(
    IUserRead read
) : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        return Result.Success(await read.GetAllAsync(cancellationToken));
    }
}