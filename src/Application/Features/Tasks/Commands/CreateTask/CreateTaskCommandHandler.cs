// Features/Tasks/Commands/CreateTask/CreateTaskCommandHandler.cs

using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using Application.Features.Tasks.Commands.CreateTask;
using Domain.ValueObjects;
using MediatR;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskEventStore _eventStore;
    private readonly ITaskSnapshotRepository _snapshotRepository;

    public CreateTaskCommandHandler(
        ITaskEventStore eventStore, 
        ITaskSnapshotRepository snapshotRepository)
    {
        _eventStore = eventStore;
        _snapshotRepository = snapshotRepository;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var taskId = TaskId.New();
        var task = Domain.Aggregates.Task.Create(
            taskId,
            request.Title,
            request.Description);

        await _eventStore.AppendEventsAsync(taskId, task.Events, cancellationToken);
        await _snapshotRepository.SaveSnapshotAsync(task, cancellationToken);
        
        return taskId.Value;
    }
}