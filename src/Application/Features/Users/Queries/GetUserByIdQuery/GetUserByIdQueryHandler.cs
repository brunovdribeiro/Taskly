using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Ardalis.Result;
using MediatR;
using static Shared.Errors.UserErrors;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdQueryHandler(
    IUserRead read
) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await read.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            return Result<UserDto>.NotFound(
                NotFound(request.Id).Message);

        return Result<UserDto>.Success(user);
    }
}