// Application/Common/Interfaces/ITaskReadModel.cs

using Application.Features.Tasks.Dtos;

namespace Application.Features.Tasks.Interfaces;

public interface ITaskRead
{
    Task<TaskDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    );
}