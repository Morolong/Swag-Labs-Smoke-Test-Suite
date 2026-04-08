using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using SmokeTestSuite.Core.Config; 

namespace SmokeTestSuite.Core.Drivers;

public static class  WebDriverFactory
{
    public static IWebDriver CreateDriver()
    {
        var browser = ConfigurationManager.Settings.Browser.ToLower();
        var headless = ConfigurationManager.Settings.headless;
        var implicitWait = ConfigurationManager.Settings.ImplicitWatSeconds;

        IWebDriver driver = browser switch
        {
            "chrome" => CreateChromeDriver(headless),
            "firefox" => CreateFirefoxDriver(headless),
            "edge" => CreateEdgeDriver(headless),
            _ => throw new ArgumentException(
                $"Unsupported browser: '(browser)'.Valid options: chrome, firefox, edge ")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWait);

        driver.Manage().Window.Maximize();

        return driver;
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions(); 

        if (headless)
        {
            options.AddArgument("--headless=new");  

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
        }

        // Suppress Chrome's "DevTools listening on..." console noise
        options.AddArgument("--log-level=3");
        options.AddExcludedArgument("enable-automation");  // Hides "Chrome is being controlled" bar

        return new ChromeDriver(options);
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

