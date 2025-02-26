using EventStore.Client;
using Infrastructure.Persistence.EventStore.Subscriptions;
using Infrastructure.Persistence.Redis.Interfaces;
using Microsoft.Extensions.Hosting;
using Redis.OM;

namespace Infrastructure.Persistence.Redis.Services;

public class UserStreamSubscriptionService(
    EventStoreClient eventStoreClient,
    EventStorePersistentSubscriptionsClient persistentSubscriptionsClient,
    IUserDocumentRepository userDocumentRepository)
    : BackgroundService
{
    private readonly UserStreamSubscription _subscription =
        new(eventStoreClient, persistentSubscriptionsClient, userDocumentRepository);

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        await _subscription.SubscribeToStream(stoppingToken);
    }
}