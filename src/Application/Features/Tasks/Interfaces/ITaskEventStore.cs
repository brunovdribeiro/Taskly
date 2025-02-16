using Domain.Common;
using Domain.ValueObjects;

namespace Application.Common.Interfacoes;

public interface ITaskEventStore
{
    Task AppendEventsAsync(
        TaskId taskId,
        IEnumerable<IEvent> events,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<IEvent>> GetEventsAsync(
        TaskId taskId,
        CancellationToken cancellationToken
    );
}