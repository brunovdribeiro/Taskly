using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using Application.Features.Users.Interfaces;
using EventStore.Client;
using Infrastructure.Persistences.EventStore;
using Infrastructure.Persistences.Posgres;
using Infrastructure.Persistences.Posgres.Tasks;
using Infrastructure.Persistences.Posgres.Users;
using Infrastructure.Persistences.Redis;
using Infrastructure.Persistences.Redis.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddEventStore(configuration);
        services.AddPostgres(configuration);
        services.AddRedis(configuration);

        return services;
    }

    private static void AddEventStore(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings = EventStoreClientSettings.Create(configuration.GetConnectionString("EventStore")!);
        services.AddSingleton(new EventStoreClient(settings));
        services.AddSingleton(new EventStorePersistentSubscriptionsClient(settings));
        services.AddScoped<ITaskEventStore, TaskEventStore>();
        services.AddScoped<IUserEventStore, UserEventStore>();
    }

    private static void AddPostgres(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        var connectionString = configuration.GetConnectionString("Postgres")!;
        services.AddScoped<ITaskSnapshotRepository, TaskSnapshotRepository>();
        services.AddScoped<IUserSnapshotRepository, UserSnapshotRepository>();

        services.AddHostedService<UserStreamSubscriptionService>();

    }

    private static void AddRedis(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redisConnection = new RedisConnectionProvider(configuration.GetConnectionString("Redis")!);
        services.AddSingleton(redisConnection);
        services.AddScoped<ITaskRead, TaskRead>();
        services.AddScoped<IUserRead, UserRead>();
    }
}