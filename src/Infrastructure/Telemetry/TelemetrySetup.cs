using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infrastructure.Telemetry;

public static class TelemetrySetup
{
    public static ActivitySource ActivitySource = new("taskly");

    public static IServiceCollection AddTelemetry(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("taskly").AddTelemetrySdk())
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter = ctx => ctx.Request.Method == "POST" || ctx.Request.Method == "PUT";

                        options.EnrichWithHttpRequest = (
                            activity,
                            request
                        ) =>
                        {
                            request.EnableBuffering();
                            using var reader = new StreamReader(request.Body, leaveOpen: true);
                            var body = reader.ReadToEndAsync().Result;
                            request.Body.Position = 0;
                            activity.SetTag("http.request.body", body);
                        };
                    })
                    .AddRedisInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter();
            });

        return services;
    }
}