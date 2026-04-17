using OpenQA.Selenium;
using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;

namespace SmokeTestSuite.Pages;

public class LoginPage : BasePage
{
    private readonly By _usernameField = By.Id("user-name");
    private readonly By _passwordField = By.Id("password");
    private readonly By _loginButton = By.Id("login-button");
    private readonly By _errorMessage = By.CssSelector("h3[data-test='error']");
    private readonly By _loginFormContainer = By.ClassName("login-box");

    public LoginPage(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() => IsElementVisible(_loginFormContainer);

    public bool IsAtPage() => base.IsAtPage("/"); 

    public LoginPage EnterUsername(string username)
    {
        TypeText(_usernameField, username);
        return this;
    }

    public LoginPage EnterPassword(string password)
    {
        TypeText(_passwordField, password);
        return this;
    }

    public ProductInventory ClickLoginExpectingSuccess()
    {
        Click(_loginButton);
        return new ProductInventory(Driver);
    }

    public LoginPage ClickLoginExpectingFailure()
    {
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