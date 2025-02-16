using Application.Common;
using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(
    Guid Id
) : IQuery<UserDto>;