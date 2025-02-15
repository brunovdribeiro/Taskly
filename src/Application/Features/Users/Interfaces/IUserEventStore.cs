using Domain.Common;
using Domain.ValueObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Features.Users.Interfaces;

public interface IUserEventStore
{
    Task AppendEventsAsync(UserId userId, IEnumerable<IEvent> events, CancellationToken cancellationToken);
}