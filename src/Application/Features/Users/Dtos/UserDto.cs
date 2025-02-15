namespace Application.Features.Users.Dtos;

public class UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModified { get; init; }
}