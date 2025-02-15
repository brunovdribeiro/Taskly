// Application/Common/Interfaces/ITaskReadModel.cs

using Application.Features.Tasks.Dtos;

namespace Application.Common.Interfaces;

public interface ITaskRead
{
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}