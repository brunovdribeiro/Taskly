using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Tasks.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler(
    ITaskRead read
) : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{
    public async Task<Result<TaskDto>> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var task = await read.GetByIdAsync(request.Id, cancellationToken);

        if (task is null)
            throw new NotFoundException($"Task with id {request.Id} not found");

        return task;
    }
}