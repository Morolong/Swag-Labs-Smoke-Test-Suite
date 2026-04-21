using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using SmokeTestSuite.Core.Config;

namespace SmokeTestSuite.Core.Drivers;

public static class WebDriverFactory
{
    public static IWebDriver CreateDriver()
    {
        var browser = ConfigurationManager.Settings.Browser.ToLower();
        var headless = ConfigurationManager.Settings.Headless;
        var implicitWait = ConfigurationManager.Settings.ImplicitWaitSeconds;

        IWebDriver driver = browser switch
        {
            "chrome" => CreateChromeDriver(headless),
            "firefox" => CreateFirefoxDriver(headless),
            "edge" => CreateEdgeDriver(headless),
            _ => throw new ArgumentException(
                $"Unsupported browser: '{browser}'. Valid options: chrome, firefox, edge")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWait);

        return driver;
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();

        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
        }

        options.AddArgument("--log-level=3");
        options.AddExcludedArgument("enable-automation");

        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");

        var service = ChromeDriverService.CreateDefaultService();
        service.SuppressInitialDiagnosticInformation = true;
        service.HideCommandPromptWindow = true;

        return new ChromeDriver(service, options, TimeSpan.FromSeconds(120));
    }

    private static IWebDriver CreateFirefoxDriver(bool headless)
    {
        var options = new FirefoxOptions();

        if (headless)
        {
            options.AddArgument("--headless");
            options.AddArgument("--width=1920");
            options.AddArgument("--height=1080");
        }

        return new FirefoxDriver(options);
    }

    private static IWebDriver CreateEdgeDriver(bool headless)
    {
        var options = new EdgeOptions();

        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=1920,1080");
        }

        return new EdgeDriver(options);
    }
}