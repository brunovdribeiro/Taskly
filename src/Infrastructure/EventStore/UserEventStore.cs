using EventStore.Client;
using Application.Features.Users.Interfaces;
using System.Text.Json;
using Domain.Common;
using Domain.ValueObjects;

namespace Infrastructure.EventStore;

public class UserEventStore : IUserEventStore
{
    private readonly EventStoreClient _client;

    public UserEventStore(EventStoreClient client)
    {
        _client = client;
    }

    public async Task AppendEventsAsync(UserId userId, IEnumerable<IEvent> events, CancellationToken cancellationToken)
    {
        var eventData = events.Select(e => new EventData(
            Uuid.NewUuid(),
            e.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes(e)
        ));

        await _client.AppendToStreamAsync(
            $"user-{userId.Value}",
            StreamState.Any,
            eventData,
            cancellationToken: cancellationToken
        );
    }
}