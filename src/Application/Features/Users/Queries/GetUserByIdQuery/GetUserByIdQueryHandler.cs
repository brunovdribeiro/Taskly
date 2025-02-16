using Application.Common.Exceptions;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdQueryHandler(
    IUserRead read
) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await read.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new NotFoundException($"User with id {request.Id} not found");

        return user;
    }
}