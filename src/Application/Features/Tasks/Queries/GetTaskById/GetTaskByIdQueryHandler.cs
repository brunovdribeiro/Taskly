using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Tasks.Dtos;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler(
    ITaskReadModel readModel
) : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    public async Task<TaskDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var task = await readModel.GetByIdAsync(request.Id, cancellationToken);

        if (task is null)
            throw new NotFoundException($"Task with id {request.Id} not found");

        return task;
    }
}