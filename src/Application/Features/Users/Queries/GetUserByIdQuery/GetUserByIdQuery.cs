using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Users.Dtos;
using Ardalis.Result;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(
    Guid Id
) : IQuery<UserDto>;