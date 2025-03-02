using System.Reflection;

namespace Api.Versioning;

public interface IGitVersionCalculator
{
    VersionInfo GetVersion();
}

public class GitVersionCalculator : IGitVersionCalculator
{
    public VersionInfo GetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        var informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        return new VersionInfo
        {
            SemVer = informationalVersion ?? version?.ToString() ?? "Unknown",
            FullSemVer = informationalVersion ?? version?.ToString() ?? "Unknown"
        };
    }
}

public class VersionInfo
{
    public string SemVer { get; set; }
    public string FullSemVer { get; set; }
}