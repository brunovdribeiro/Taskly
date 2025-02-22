using System.Text.Json;
using Application.Features.Users.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using EventStore.Client;

namespace Infrastructure.Persistence.EventStore;

public class UserEventStore(
    EventStoreClient client
) : IUserEventStore
{
    public async Task AppendEventsAsync(
        UserId userId,
        IEnumerable<IEvent> events,
        CancellationToken cancellationToken
    )
    {
        var eventData = events.Select(e => new EventData(
            Uuid.NewUuid(),
            e.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes<object>(e, JsonSerializerOptions.Default)
        ));

        await client.AppendToStreamAsync(
            $"user-{userId.Value}",
            StreamState.Any,
            eventData,
            cancellationToken: cancellationToken
        );
    }
}