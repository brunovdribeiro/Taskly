using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Infrastructure.Persistences.Posgres.Tasks;

public class TaskSnapshot
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public Guid? AssignedTo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public int Version { get; set; }
}