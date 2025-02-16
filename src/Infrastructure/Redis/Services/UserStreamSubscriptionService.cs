using EventStore.Client;
using Microsoft.Extensions.Hosting;
using Redis.OM;

namespace Infrastructure.Redis.Services;

public class UserStreamSubscriptionService : BackgroundService
{
    private readonly UserStreamSubscription _subscription;

    public UserStreamSubscriptionService(
        EventStoreClient eventStoreClient,
        EventStorePersistentSubscriptionsClient persistentSubscriptionsClient,
        RedisConnectionProvider redisProvider
    )
    {
        _subscription = new UserStreamSubscription(eventStoreClient, persistentSubscriptionsClient, redisProvider);
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        await _subscription.SubscribeToStream(stoppingToken);
    }
}