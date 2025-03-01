using Ardalis.Result;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Shared.Errors;

namespace Application.Features.Users.Commands.Activate;

public class ActivateUserCommandHandler(
    IUserEventStore eventStore,
    IUserSnapshotRepository snapshotRepository
) : IRequestHandler<ActivateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = UserId.From(request.UserId);
        
        // Find user
        var user = await snapshotRepository.GetByIdAsync(userId, cancellationToken);

        // Check if user exists
        if (user is null)
            return Result<UserDto>.Error(
                UserErrors.NotFound(request.UserId).Message
            );

        // Check if user is already active
        if (user.IsActive)
            return Result<UserDto>.Error(
                UserErrors.AlreadyActive(request.UserId).Message
            );

        // Activate user
        user.Activate();

        // Save changes
        try 
        {
            await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
            await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Error(
                UserErrors.ActivationFailed(request.UserId, ex.Message).Message
            );
        }

        // Map and return result
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
}