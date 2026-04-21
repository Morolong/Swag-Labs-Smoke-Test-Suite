using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Core.Drivers;
using SmokeTestSuite.Core.Helpers;
using SmokeTestSuite.Core.Reporting;

namespace SmokeTestSuite.Core;

public abstract class BaseTest
{
    [ThreadStatic]
    protected static IWebDriver? Driver;

    [ThreadStatic]
    protected static WaitHelper? Wait;

    [ThreadStatic]
    protected static string? FailureMessage;

    private string _testName = string.Empty;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Directory.CreateDirectory(ConfigurationManager.Settings.ScreenshotDirectory);
        Directory.CreateDirectory(ConfigurationManager.Settings.ReportDirectory);

        ReportManager.InitializeReport();
    }

    [SetUp]
    public void SetUp()
    {
        _testName = TestContext.CurrentContext.Test.Name;
        FailureMessage = null;

        Console.WriteLine($"[START] {_testName}");

        Driver = WebDriverFactory.CreateDriver();
        Wait = new WaitHelper(Driver, ConfigurationManager.Settings.ExplicitWaitSeconds);

        ReportManager.StartTest(_testName);

        var runNumber = Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER");
        var environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT")
                          ?? ConfigurationManager.Settings.Environment
                          ?? "local";

        if (runNumber != null)
            Log($"CI Build: #{runNumber} | Environment: {environment}");
    }

    [TearDown]
    public void TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome;
        var message = TestContext.CurrentContext.Result.Message;

        bool testFailed = outcome.Status is TestStatus.Failed or TestStatus.Warning;

        if (testFailed && Driver != null)
        {
            if (ConfigurationManager.Settings.TakeScreenshotOnFailure)
            {
                var screenshotPath = ScreenshotHelper.TakeScreenshot(Driver, _testName);
                if (screenshotPath != null)
                {
                    ReportManager.AttachScreenshot(screenshotPath);
                    TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");
                }
            }

            var fullMessage = FailureMessage != null
                ? $"{FailureMessage}{Environment.NewLine}Technical Detail: {message}"
                : message ?? "Test failed";

            ReportManager.FailTest(fullMessage);
            Console.WriteLine($"[FAIL] {_testName}: {fullMessage}");
        }
        else
        {
            ReportManager.PassTest();
            Console.WriteLine($"[PASS] {_testName}");
        }

        try
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] Driver quit error (non-fatal): {ex.Message}");
        }
        finally
        {
            Driver = null;
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        ReportManager.FlushReport();

        var runNumber = Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER");
        var reportLabel = runNumber != null
            ? $"[REPORT] Run #{runNumber} — saved to: {ConfigurationManager.Settings.ReportDirectory}"
            : $"[REPORT] Test report saved to: {ConfigurationManager.Settings.ReportDirectory}";

        Console.WriteLine(reportLabel);
    }

    protected void NavigateToBaseUrl()
    {
        if (Driver == null)
            throw new InvalidOperationException("Driver not initialized");

        Driver!.Navigate().GoToUrl(ConfigurationManager.Settings.BaseUrl);
    }

    protected void NavigateTo(string relativeUrl)
    {
        var fullUrl = $"{ConfigurationManager.Settings.BaseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
        Driver!.Navigate().GoToUrl(fullUrl);
    }

    protected void Log(string message)
    {
        ReportManager.LogInfo(message);
        Console.WriteLine($"[LOG] {message}");
    }
}