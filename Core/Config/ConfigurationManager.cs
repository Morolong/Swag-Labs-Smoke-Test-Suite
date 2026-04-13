using Microsoft.Extensions.Configuration;

namespace SmokeTestSuite.Core.Config;

public class TestSettings
{
    public string BaseUrl { get; set; }
    public string Browser { get; set; }
    public bool Headless { get; set; }
    public int ImplicitWaitSeconds { get; set; }
    public int ExplicitWaitSeconds { get; set; }
    public bool TakeScreenshotOnFailure { get; set; }
    public string ScreenshotDirectory { get; set; }
    public string ReportDirectory { get; set; }
    public string ReportName { get; set; }
}

public class Credentials
{
    public string PositiveTestUser { get; set; }
    public string LoginPassword { get; set; }
    public string NegativeTestUser { get; set; }
    public string NegativePassword { get; set; }
}

public static class ConfigurationManager
{
    private static readonly Lazy<(TestSettings Settings, Credentials Credentials)> _config =
        new(() => LoadConfiguration());

    public static TestSettings Settings => _config.Value.Settings;
    public static Credentials Credentials => _config.Value.Credentials;

    private static (TestSettings Settings, Credentials Credentials) LoadConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var settings = configuration.GetSection("TestSettings").Get<TestSettings>()
            ?? throw new InvalidOperationException("TestSettings section missing from appsettings.json");

        settings.ReportDirectory = ResolvePath(settings.ReportDirectory);
        settings.ScreenshotDirectory = ResolvePath(settings.ScreenshotDirectory);

        var credentials = configuration.GetSection("Credentials").Get<Credentials>()
            ?? new Credentials();

        return (settings, credentials);
    }

    private static string ResolvePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return AppContext.BaseDirectory;

        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, path));
    }
}