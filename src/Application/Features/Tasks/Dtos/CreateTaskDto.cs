using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Features.Tasks.Dtos;

public class CreateTaskDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public Guid? AssignedTo { get; init; }
}