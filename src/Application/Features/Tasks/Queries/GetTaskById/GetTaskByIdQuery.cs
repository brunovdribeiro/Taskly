using Application.Common;
using Application.Features.Tasks.Dtos;

namespace Application.Features.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : IQuery<TaskDto>;