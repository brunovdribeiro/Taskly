using Application.Common;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand : ICommand<UserDto>
{
    public string Email { get; init; }
    public string Name { get; init; }
}

public class CreateUserCommandHandler(
    IUserEventStore eventStore,
    IUserSnapshotRepository snapshotRepository
)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = UserId.New();

        var user = User.Create(
            userId,
            request.Email,
            request.Name);

        await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
        await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);

        return new UserDto
        {
            Id = userId.Value,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified
        };
    }
}