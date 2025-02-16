// Features/Tasks/Commands/CreateTask/CreateTaskCommandHandler.cs

using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using Domain.ValueObjects;
using MediatR;
using Task = Domain.Aggregates.Task;

namespace Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(
    ITaskEventStore eventStore,
    ITaskSnapshotRepository snapshotRepository
)
    : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        var taskId = TaskId.New();

        var task = Task.Create(
            taskId,
            request.Title,
            request.Description);

        await eventStore.AppendEventsAsync(taskId, task.Events, cancellationToken);
        await snapshotRepository.SaveSnapshotAsync(task, cancellationToken);

        return taskId.Value;
    }
}