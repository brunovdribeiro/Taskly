using Application.Common.Interfaces;
using Application.Features.Users.Dtos;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IQuery<IEnumerable<UserDto>>;