using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Ardalis.Result;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IUserEventStore eventStore,
    IUserSnapshotRepository snapshotRepository
)
    : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result<UserDto>.Invalid(
                new ValidationError("Email is required")
            );

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<UserDto>.Invalid(
                new ValidationError("Name is required")
            );

        var userId = UserId.New();

        try 
        {
            var user = User.Create(
                userId,
                request.Email,
                request.Name);

            await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
            await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);

            return Result<UserDto>.Success(new UserDto
            {
                Id = userId.Value,
                Email = user.Email,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                LastModified = user.LastModified
            });
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Error(
                $"Failed to create user: {ex.Message}"
            );
        }
    }
}