using System.Text.Json;
using Application.Features.Users.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using EventStore.Client;

namespace Infrastructure.Persistences.EventStore;

public class UserEventStore : IUserEventStore
{
    private readonly EventStoreClient _client;

    public UserEventStore(
        EventStoreClient client
    )
    {
        _client = client;
    }

    public async Task AppendEventsAsync(
        UserId userId,
        IEnumerable<IEvent> events,
        CancellationToken cancellationToken
    )
    {
        try
        {

            var eventData = events.Select(e => new EventData(
                Uuid.NewUuid(),
                e.GetType().Name,
                JsonSerializer.SerializeToUtf8Bytes<object>(e, JsonSerializerOptions.Default)
            ));

            await _client.AppendToStreamAsync(
                $"user-{userId.Value}",
                StreamState.Any,
                eventData,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}