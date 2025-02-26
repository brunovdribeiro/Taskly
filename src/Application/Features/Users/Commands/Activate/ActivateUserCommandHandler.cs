using Application.Common;
using Application.Common.Exceptions;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands.Activate;

public record ActivateUserCommand : ICommand<UserDto>
{
    public Guid UserId { get; init; }
}

public class DeactivateUserCommandHandler(
    IUserEventStore eventStore,
    IUserSnapshotRepository snapshotRepository
) : IRequestHandler<ActivateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = UserId.From(request.UserId);
        var user = await snapshotRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            throw new NotFoundException($"User with id {request.UserId} not found");

        user.Activate();

        await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
        await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);

        return new UserDto
        {
            Id = user.Id.Value,
            Email = user.Email,
            Name = user.Name,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified
        };
    }
}