using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Core.Drivers;
using SmokeTestSuite.Core.Helpers;
using SmokeTestSuite.Core.Reporting; 

namespace SmokeTestSuite.Core;

[TestFixture]
public abstract class BaseTest
{
    [ThreadStatic]
    protected static IWebDriver? Driver;

    [ThreadStatic]
    protected static WaitHelper? Wait; 

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

        Console.WriteLine($"[START] {_testName}");

        Driver = WebDriverFactory.CreateDriver();

        Wait = new WaitHelper(Driver, ConfigurationManager.Settings.ExplicitWaitSeconds);

        ReportManager.StartTest(_testName); 
    }

    [TearDown]
    public void TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome;
        var message = TestContext.CurrentContext.Result.Message;

        bool testFailed = outcome.Status is TestStatus.Failed or TestStatus.Warning;

        if (testFailed && Driver !=null)
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

            ReportManager.FailTest(message ?? "Test failed");
            Console.WriteLine($"[FAIL] {_testName}"); 
        }
        else
        {
            ReportManager.PassTest();
            Console.WriteLine($"[PASS] {_testName}"); 
        }

        Driver?.Quit();
        Driver?.Dispose();
        Driver = null; 
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        ReportManager.FlushReport();
        Console.WriteLine($"[REPORT] Test report save to: {ConfigurationManager.Settings.ReportDirectory}");
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
