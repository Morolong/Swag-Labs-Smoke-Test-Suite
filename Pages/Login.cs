using OpenQA.Selenium;
using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Core.Helpers;
using SmokeTestSuite.Core.Reporting; 

namespace SmokeTestSuite.Pages;

public class LoginPage : BasePage
{
    private readonly By _usernameField = By.Id("user-name");
    private readonly By _passwordField = By.Id("password");
    private readonly By _loginButton = By.Id("login-button");
    private readonly By _errorMessage = By.CssSelector("h3[data-test='error']");
    private readonly By _loginFormContainer = By.ClassName("login-box");

    public LoginPage(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad()
    {
        LogStep("Waiting for Login Page to load");

        try
        {
            Wait.ForElementToBeVisible(_loginFormContainer);
            LogStep("Login Page loaded successfully");
        }
        catch (WebDriverTimeoutException ex)
        {
            LogStep($"Login Page failed to load - container element not visible within timeout: {ex.Message}");
            throw;
        }
    }

    public bool IsAtPage() => base.IsAtPage("/"); 

    public LoginPage EnterUsername(string username)
    {
        LogStep($"Entering username: {username}");
        TypeText(_usernameField, username);
        return this;
    }

    public LoginPage EnterPassword(string password)
    {
        LogStep($"Entering password: {password}");
        TypeText(_passwordField, password);
        return this;
    }

    public ProductInventory ClickLoginExpectingSuccess()
    {
        LogStep($"Clicking Login");
        Click(_loginButton);
        return new ProductInventory(Driver);
    }

    public LoginPage ClickLoginExpectingFailure()
    {
        LogStep($"Clicking Login");
        Click(_loginButton);
        return this;
    }

    public ProductInventory LoginAs(string username, string password)
    {
        return EnterUsername(username)
            .EnterPassword(password)
            .ClickLoginExpectingSuccess();
    }

    public string GetErrorMessage()
    {
        return IsElementVisible(_errorMessage)
            ? GetText(_errorMessage)
            : string.Empty;
    }

    public bool IsErrorMessageDisplayed() => IsElementVisible(_errorMessage);

    public string GetUsernamePlaceholder() => GetAttribute(_usernameField, "placeholder");

    public bool IsPageDisplayed()
    {
        return Driver.FindElement(_loginFormContainer).Displayed;
    }
}