namespace Shared.Errors;

public static class UserErrors
{
    public static DomainError NotFound(Guid userId) => 
        new DomainError(
            "user.not_found", 
            $"User with ID {userId} was not found"
        );

    public static DomainError AlreadyActive(Guid userId) => 
        new DomainError(
            "user.already_active", 
            $"User with ID {userId} is already active"
        );

    public static DomainError AlreadyDeactivated(Guid userId) => 
        new DomainError(
            "user.already_deactivated", 
            $"User with ID {userId} is already deactivated"
        );

    public static DomainError ActivationFailed(Guid userId, string reason) => 
        new DomainError(
            "user.activation_failed", 
            $"Failed to activate user {userId}. Reason: {reason}"
        );

    public static DomainError DeactivationFailed(Guid userId, string reason) => 
        new DomainError(
            "user.deactivation_failed", 
            $"Failed to deactivate user {userId}. Reason: {reason}"
        );
}