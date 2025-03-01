using Application.Features.Users.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Users.Commands.Activate;

public record ActivateUserCommand : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
}