using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infrastructure.Telemetry;

public static class TelemetrySetup
{
    public static ActivitySource ActivitySource = new("Taskly");

    public static IServiceCollection AddTelemetry(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("Taskly")
                        .AddTelemetrySdk())
                    .AddSource("Taskly")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    // .AddEntityFrameworkCoreInstrumentation()
                    // .AddRedisInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri("http://localhost:4318/v1/traces");
                        opts.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                        opts.Headers = $"Authorization=Splunk {configuration["Splunk:Token"]}";
                    });
            });

        return services;
    }
}