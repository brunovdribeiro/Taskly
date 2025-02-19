using System.Diagnostics;
using System.Text.Json;
using Application.Features.Users.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using EventStore.Client;
using Infrastructure.Telemetry;

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
        CancellationToken cancellationToken)
    {
        using var activity = TelemetrySetup.ActivitySource.StartActivity("AppendEvents");
        activity?.SetTag("userId", userId.Value);
    
        try
        {
            var eventData = events.Select(e => 
            {
                activity?.AddEvent(new ActivityEvent(e.GetType().Name));
                return new EventData(
                    Uuid.NewUuid(),
                    e.GetType().Name,
                    JsonSerializer.SerializeToUtf8Bytes<object>(e, JsonSerializerOptions.Default)
                );
            });

            await _client.AppendToStreamAsync(
                $"user-{userId.Value}",
                StreamState.Any,
                eventData,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception e)
        {
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            throw;
        }
    }
}