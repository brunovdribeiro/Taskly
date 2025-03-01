using Application.Features.Users.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<UserDto>>
{
    public string Email { get; init; }
    public string Name { get; init; }
}