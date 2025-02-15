using Application.Common.Interfaces;
using Application.Common.Interfacoes;
using EventStore.Client;
using Infrastructure.EventStore;
using Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Redis
        services.AddSingleton(new RedisConnectionProvider(configuration["Redis:ConnectionString"]));
        
        // PostgreSQL
        services.AddScoped<ITaskSnapshotRepository>(sp => 
            new PostgresTaskSnapshotRepository(configuration["Postgres:ConnectionString"]));
        
        // EventStoreDB
        services.AddSingleton(new EventStoreClient(EventStoreClientSettings
            .Create(configuration["EventStore:ConnectionString"])));
        services.AddScoped<ITaskEventStore, EventStoreDbTaskEventStore>();

        return services;
    }
}