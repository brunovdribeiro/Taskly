using FastEndpoints;

namespace Api.Versioning;
public class VersionResponse
{
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public DateTime BuildDate { get; set; }
}


public class VersionEndpoint : EndpointWithoutRequest<VersionResponse>
{
    private readonly IGitVersionCalculator _gitVersionCalculator;

    public VersionEndpoint(IGitVersionCalculator gitVersionCalculator)
    {
        _gitVersionCalculator = gitVersionCalculator;
    }

    public override void Configure()
    {
        Get("/api/version");
        AllowAnonymous();
        Description(d => d
            .WithName("GetVersion")
            .WithTags("System")
            .WithSummary("Returns the application version information"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var versionInfo = _gitVersionCalculator.GetVersion();
        
        var response = new VersionResponse
        {
            Version = versionInfo.SemVer, // or FullSemVer for more detailed version
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            BuildDate = DateTime.UtcNow
        };

        await SendAsync(response, cancellation: ct);
    }
}
