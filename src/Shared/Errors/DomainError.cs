// Domain/Shared/Errors/DomainError.cs

using Ardalis.Result;

namespace Shared.Errors;

public class DomainError
{
    /// <summary>
    /// Unique error code for categorization and tracking
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Human-readable error message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Optional additional details or context
    /// </summary>
    public string? Details { get; }

    /// <summary>
    /// Creates a new domain error
    /// </summary>
    /// <param name="code">Unique error code</param>
    /// <param name="message">Error message</param>
    /// <param name="details">Optional additional details</param>
    public DomainError(
        string code, 
        string message, 
        string? details = null)
    {
        Code = code;
        Message = message;
        Details = details;
    }

    /// <summary>
    /// Common error categories
    /// </summary>
    public static class Categories
    {
        public const string Validation = "validation";
        public const string NotFound = "not_found";
        public const string Conflict = "conflict";
        public const string Unauthorized = "unauthorized";
        public const string Forbidden = "forbidden";
        public const string Invalid = "invalid";
    }

    /// <summary>
    /// Creates a validation error
    /// </summary>
    public static DomainError Validation(
        string message, 
        string? details = null) => 
        new DomainError(
            $"{Categories.Validation}.generic", 
            message, 
            details
        );

    /// <summary>
    /// Creates a not found error
    /// </summary>
    public static DomainError NotFound(
        string resourceName, 
        string identifier) => 
        new DomainError(
            $"{Categories.NotFound}.{resourceName.ToLowerInvariant()}", 
            $"{resourceName} not found", 
            $"Identifier: {identifier}"
        );

    /// <summary>
    /// Creates a conflict error
    /// </summary>
    public static DomainError Conflict(
        string resourceName, 
        string reason) => 
        new DomainError(
            $"{Categories.Conflict}.{resourceName.ToLowerInvariant()}", 
            $"Conflict with {resourceName}", 
            reason
        );

    /// <summary>
    /// Converts to Ardalis.Result error
    /// </summary>
    public Result ToResult() => 
        Result.Error(Message);

    /// <summary>
    /// Converts to Ardalis.Result<T> error
    /// </summary>
    public Result<T> ToResult<T>() => 
        Result<T>.Error(Message);

    /// <summary>
    /// Equality comparison
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is DomainError other && 
        Code == other.Code && 
        Message == other.Message;

    /// <summary>
    /// Hash code generation
    /// </summary>
    public override int GetHashCode() =>
        HashCode.Combine(Code, Message);

    /// <summary>
    /// String representation
    /// </summary>
    public override string ToString() => 
        $"[{Code}] {Message}";
}