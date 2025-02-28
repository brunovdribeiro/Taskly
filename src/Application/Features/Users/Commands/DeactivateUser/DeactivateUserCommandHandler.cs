using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Ardalis.Result;
using Domain.ValueObjects;
using MediatR;
using Shared.Errors;

namespace Application.Features.Users.Commands.DeactivateUser;

public record DeactivateUserCommand : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
}

public class DeactivateUserCommandHandler(
    IUserEventStore eventStore,
    IUserSnapshotRepository snapshotRepository
) : IRequestHandler<DeactivateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(
        DeactivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = UserId.From(request.UserId);
        var user = await snapshotRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return Result<UserDto>.Error(
                UserErrors.NotFound(request.UserId).Message
            );

        if (!user.IsActive)
            return Result<UserDto>.Error(
                UserErrors.AlreadyDeactivated(request.UserId).Message
            );

        try 
        {
            user.Deactivate();

            await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
            await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id.Value,
                Email = user.Email,
                Name = user.Name,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastModified = user.LastModified
            });
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Error(
                UserErrors.DeactivationFailed(request.UserId, ex.Message).Message
            );
        }
    }
}