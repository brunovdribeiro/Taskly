using Application.Common;

namespace Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand : ICommand<Guid>
{
    public string Title { get; init; }
    public string Description { get; init; }
}