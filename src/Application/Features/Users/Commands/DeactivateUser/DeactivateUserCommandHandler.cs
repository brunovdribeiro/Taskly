using Application.Common;
using Application.Common.Exceptions;
using Application.Features.Users.Dtos;
    using Application.Features.Users.Interfaces;
    using Domain.ValueObjects;
    using MediatR;
    
    namespace Application.Features.Users.Commands.DeactivateUser;
    
    public record DeactivateUserCommand : ICommand<UserDto>
    {
        public Guid UserId { get; init; }
    }
    
    public class DeactivateUserCommandHandler(
        IUserEventStore eventStore,
        IUserSnapshotRepository snapshotRepository
    ) : IRequestHandler<DeactivateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = UserId.From(request.UserId);
            var user = await snapshotRepository.GetByIdAsync(userId, cancellationToken);
            
            if (user is null)
                throw new NotFoundException($"User with id {request.UserId} not found");
    
            user.Deactivate();
    
            await eventStore.AppendEventsAsync(userId, user.Events, cancellationToken);
            await snapshotRepository.SaveSnapshotAsync(user, cancellationToken);
            
            return new UserDto
            {
                Id = user.Id.Value,
                Email = user.Email,
                Name = user.Name,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastModified = user.LastModified,
            };
        }
    }