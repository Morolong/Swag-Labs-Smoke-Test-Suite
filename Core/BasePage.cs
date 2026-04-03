using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Interaction;
using OpenQA.Selenium.Interactions;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Core.Helpers;

namespace SmokeTestSuite.Core;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WaitHelper Wait;
    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WaitHelper(driver, ConfigurationManager.Settings.ExplicitWaitSeconds);
    }

    protected abstract string PagePath { get; }
    protected abstract bool IsPageLoaded();

    public virtual BasePage Open()
    {
        var baseUrl = ConfigurationManager.Settings.BaseUrl.TrimEnd('/');
        var path = PagePath.TrimStart('/');
        Driver.Navigate().GoToUrl($"{baseUrl}/{path}");

        Wait.Until(() => IsPageLoaded(),
            $"Page '{GetType().Name}' did not load at {Driver.Url}");
        return this;
    }

    protected void Click(By locator)
    {
        var element = Wait.ForElementToBeClickable(locator);
        element.Click();
    }

    protected void TypeText(By locator, string text)
    {
        var element = Wait.ForElementToBeVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetText(By locator)
    {
        var element = Wait.ForElementToBeVisible(locator);
        return element.Text;
    }

    protected string GetAttribute(By locator, string attribute)
    {
        var element = Wait.ForElementToBeVisible(locator);
        return element.GetAttribute(attribute) ?? string.Empty;
    }

    protected bool IsElementVisible(By locator)
    {
        try
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
            var elements = Driver.FindElements(locator);
            return elements.Count > 0 && elements[0].Displayed; 
        }
        catch (Exception)
        {
            return false; 
        }
        //May have to update this to not have such waits 
        finally
        {
            Driver.Manage().Timeouts().ImplicitWait =
                TimeSpan.FromSeconds(ConfigurationManager.Settings.ImplicitWaitSeconds);
        }
    }

    protected void ScrollToElement(By locator)
    {
        var element = Driver.FindElement(locator); 
        new Actions(Driver)
            .MoveToElement(element)
            .Perform();
    }

    protected void WaitForText(By locator, string expectedText)
    {
        Wait.Until(
            () => GetText(locator).Contains(expectedText, StringComparison.OrdinalIgnoreCase),
            $"Expected text '{expectedText}' not found in element {locator}"
        );
    }
}
