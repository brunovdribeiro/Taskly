namespace Infrastructure.Persistences.Posgres.Users;

public class UserSnapshot
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public int Version { get; set; }
}