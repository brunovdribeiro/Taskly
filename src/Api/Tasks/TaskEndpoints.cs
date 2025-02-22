using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Dtos;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task = Domain.Aggregates.Task;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(
        this IEndpointRouteBuilder routes
    )
    {
        var group = routes.MapGroup("/tasks");

        group.MapGet("/", GetAllTasks);
        group.MapGet("/{id}", GetTaskById);
        group.MapPost("/", CreateTask);
        group.MapPut("/{id}", UpdateTask);
        group.MapDelete("/{id}", DeleteTask);

        return routes;
    }

    private static async Task<Ok<IEnumerable<TaskDto>>> GetAllTasks(
        ITaskSnapshotRepository repository,
        CancellationToken cancellationToken
    )
    {
        // Implementation would go here
        return TypedResults.Ok(Enumerable.Empty<TaskDto>());
    }

    private static async Task<Results<Ok<TaskDto>, NotFound>> GetTaskById(
        Guid id,
        ITaskSnapshotRepository repository,
        CancellationToken cancellationToken
    )
    {
        var task = await repository.GetLatestSnapshotAsync(TaskId.From(id), cancellationToken);

        if (task is null)
            return TypedResults.NotFound();

        var dto = MapToDto(task);
        return TypedResults.Ok(dto);
    }

    private static async Task<Results<Created<TaskDto>, ValidationProblem>> CreateTask(
        CreateTaskDto createTaskDto,
        [FromServices] IMediator mediator,
        ITaskSnapshotRepository repository,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateTaskCommand
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description
        };

        var taskId = await mediator.Send(command, cancellationToken);
        var task = await repository.GetLatestSnapshotAsync(TaskId.From(taskId), cancellationToken);
        var taskDto = MapToDto(task);

        return TypedResults.Created($"/api/tasks/{taskDto.Id}", taskDto);
    }

    private static async Task<Results<Ok<TaskDto>, NotFound, ValidationProblem>> UpdateTask(
        Guid id,
        TaskDto taskDto,
        ITaskEventStore eventStore,
        ITaskSnapshotRepository repository,
        CancellationToken cancellationToken
    )
    {
        // Implementation would go here
        return TypedResults.Ok(taskDto);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteTask(
        Guid id,
        ITaskEventStore eventStore,
        ITaskSnapshotRepository repository,
        CancellationToken cancellationToken
    )
    {
        // Implementation would go here
        return TypedResults.NoContent();
    }

    private static TaskDto MapToDto(
        Task task
    )
    {
        return new TaskDto
        {
            Id = task.Id.Value,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified,
            AssignedTo = task.AssignedTo?.Value
        };
    }
}