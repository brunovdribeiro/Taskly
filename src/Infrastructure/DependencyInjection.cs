using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using Application.Features.Users.Interfaces;
using EventStore.Client;
using Infrastructure.EventStore;
using Infrastructure.Postgres;
using Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventStore(configuration);
        services.AddPostgres(configuration);
        services.AddRedis(configuration);
        
        return services;
    }

    private static void AddEventStore(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = EventStoreClientSettings.Create(configuration.GetConnectionString("EventStore")!);
        services.AddSingleton(new EventStoreClient(settings));
        services.AddScoped<ITaskEventStore, TaskEventStore>();
        services.AddScoped<IUserEventStore, UserEventStore>();
    }

    private static void AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITaskSnapshotRepository>(_ => 
            new TaskSnapshotRepository(configuration.GetConnectionString("Postgres")!));
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = new RedisConnectionProvider(configuration.GetConnectionString("Redis")!);
        services.AddSingleton(redisConnection);
        services.AddScoped<ITaskRead, TaskRead>();
        services.AddScoped<IUserRead, UserRead>();
    }
}