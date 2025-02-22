using System.Text;
using System.Text.Json;
using Domain.Events;
using EventStore.Client;
using Infrastructure.Persistence.Redis.Documents;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Persistence.EventStore.Subscriptions;

public class UserStreamSubscription
{
    private const string StreamName = "$ce-user"; // Category projection for all user streams
    private const string GroupName = "user-group-sub2";
    private readonly EventStoreClient _eventStoreClient;
    private readonly EventStorePersistentSubscriptionsClient _persistentSubscriptionsClient;
    private readonly RedisConnectionProvider _redisProvider;
    private readonly IRedisCollection<UserDocument> _users;


    public UserStreamSubscription(
        EventStoreClient eventStoreClient,
        EventStorePersistentSubscriptionsClient persistentSubscriptionsClient,
        RedisConnectionProvider redisProvider
    )
    {
        _eventStoreClient = eventStoreClient;
        _persistentSubscriptionsClient = persistentSubscriptionsClient;
        _redisProvider = redisProvider;
        _users = _redisProvider.RedisCollection<UserDocument>();
    }

    public async Task SubscribeToStream(
        CancellationToken cancellationToken
    )
    {
        var settings = new PersistentSubscriptionSettings(
            true,
            Position.Start,
            maxRetryCount: 10,
            maxSubscriberCount: 3,
            checkPointAfter: TimeSpan.FromSeconds(5)
        );

        try
        {
            var info = await _persistentSubscriptionsClient.GetInfoToAllAsync(
                GroupName,
                cancellationToken: cancellationToken
            );
        }
        catch (PersistentSubscriptionNotFoundException)
        {
            try
            {
                await _persistentSubscriptionsClient.CreateToAllAsync(
                    GroupName,
                    EventTypeFilter.ExcludeSystemEvents(),
                    settings,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        await using var subscription = _persistentSubscriptionsClient
            .SubscribeToAll(GroupName,
                cancellationToken: cancellationToken);

        await foreach (var message in subscription.Messages.WithCancellation(cancellationToken))
            switch (message)
            {
                case PersistentSubscriptionMessage.NotFound notFound:
                    Console.WriteLine($"Not found: {notFound}");
                    break;
                case PersistentSubscriptionMessage.Event(var resolvedEvent, _):
                    await HandleEvent(
                        resolvedEvent.Event.EventType,
                        resolvedEvent.Event,
                        cancellationToken);
                    await subscription.Ack(resolvedEvent);
                    break;
            }
    }

    private async Task HandleEvent(
        string eventType,
        EventRecord eventData,
        CancellationToken cancellationToken
    )
    {
        switch (eventType)
        {
            case "UserCreatedEvent":
                await HandleUserCreated(eventData, cancellationToken);
                break;
            // Add more event handlers as needed
        }
    }

    private async Task HandleUserCreated(
        EventRecord eventRecord,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var eventData = DeserializeEvent<UserCreatedEvent>(eventRecord);

            if (eventData is null) return;

            if (eventData.UserId == Guid.Empty) return;

            var userDocument = new UserDocument
            {
                Id = eventData.UserId.ToString(),
                Email = eventData.Email,
                Name = eventData.Name,
                IsActive = eventData.IsActive,
                CreatedAt = eventData.CreatedAt,
                LastModified = eventData.LastModified
            };

            await _users.InsertAsync(userDocument, WhenKey.Always);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }

    private static T? DeserializeEvent<T>(
        EventRecord eventRecord
    )
    {
        try
        {
            var json = Encoding.UTF8.GetString(eventRecord.Data.Span);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception e)
        {
            return default;
        }
    }
}