using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.Features.Tasks.Dtos;

public class TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModified { get; init; }
    public Guid? AssignedTo { get; init; }
}