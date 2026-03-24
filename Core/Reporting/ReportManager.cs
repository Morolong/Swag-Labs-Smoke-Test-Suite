using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using SmokeTestSuite.Core.Config; 

namespace SmokeTestSuite.Core.Reporting; 

public static class ReportManager
{
    private static ExtentReports? _extent;

    [ThreadStatic]
    private static ExtentTest? _currentTest; 

    public static void InitializeReport()
    {
        var settings = ConfigurationManager.Settings;

        var reportPath = Path.Combine(
            settings.ReportDirectory,
            $"{settings.ReportName}_{DateTime.Now:yyyyMMdd_HHmmss}.html");

        var sparkReporter = new ExtentSparkReporter(reportPath);

        sparkReporter.Config.DocumentTitle = "Test Exectution Report";
        sparkReporter.Config.Theme = Theme.Dark;
        sparkReporter.Config.TimelineEnabled = true;

        _extent = new ExtentReports();
        _extent.AttachReporter(sparkReporter);

        _extent.AddSystemInfo("Framework", "Selenium WebDriver & NUnit");
        _extent.AddSystemInfo("Browser", settings.Browser);
        _extent.AddSystemInfo("Environment", settings.BaseUrl);
        _extent.AddSystemInfo("Machine", Environment.MachineName);
        _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());

        Console.WriteLine($"[REPORT] REport initialised: {reportPath}"); 
    }

    public static void StartTest(string testName)
    {
        if(_extent == null)
        {
            InitializeReport(); 
        }


        _currentTest = _extent!.CreateTest(testName);
        _currentTest.Info($"Test started at {DateTime.Now:HH:mm:ss}");
    }

    public static void LogInfo(string message)
    {
        _currentTest?.Info(message);
    }

    public static void PassTest()
    {
        _currentTest?.Pass("Test passed ✓ "); 
    }

    public static void FailTest(string errorMessage)
    {
        _currentTest?.Fail($"Test failed: {errorMessage}"); 
    }

    public static void AttachScreenshot (string screenshotPath)
    {
        if (_currentTest == null || !File.Exists(screenshotPath))
        return;

        _currentTest.Fail("Screenshot at time of failure: ", 
            MediaEntityBuilder.CreateScreenCaptureFromPath
            (screenshotPath).Build());
    }

    public static void FlushReport()
    {
        _extent?.Flush();
    }
}
