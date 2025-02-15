using EventStore.Client;
using Application.Common.Interfaces;
using Domain.Events;
using System.Text.Json;
using Application.Common.Interfacoes;
using Domain.ValueObjects;
using Domain.Common;

namespace Infrastructure.EventStore;

public class EventStoreDbTaskEventStore(
    EventStoreClient client
) : ITaskEventStore
{
    public async Task AppendEventsAsync(TaskId taskId, IEnumerable<IEvent> events, CancellationToken cancellationToken)
    {
        var eventData = events.Select(e => new EventData(
            Uuid.NewUuid(),
            e.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes(e)
        ));

        await client.AppendToStreamAsync(
            $"task-{taskId.Value}",
            StreamState.Any,
            eventData,
            cancellationToken: cancellationToken
        );
    }

    public async Task<IEnumerable<IEvent>> GetEventsAsync(TaskId taskId, CancellationToken cancellationToken)
    {
        var result = client.ReadStreamAsync(
            Direction.Forwards,
            $"task-{taskId.Value}",
            StreamPosition.Start,
            cancellationToken: cancellationToken
        );

        var events = new List<IEvent>();
        await foreach (var @event in result)
        {
            var eventData = JsonSerializer.Deserialize<IEvent>(@event.Event.Data.Span);
            if (eventData != null)
            {
                events.Add(eventData);
            }
        }

        return events;
    }
}