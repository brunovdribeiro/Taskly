using Application.Features.Users.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Users.Commands.DeactivateUser;

public record DeactivateUserCommand : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
}