using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class LoginPage : BasePage
{
    private readonly By _usernameField = By.Id("email");
    private readonly By _passwordField = By.Id("password");
    private readonly By _loginButton = By.CssSelector("[data-testId='login-btn']");
    private readonly By _errorMessage = By.CssSelector(".alert-danger");
    private readonly By _loginFormContainer = By.CssSelector(".login-container");

    protected override string PagePath => "/login";

    public LoginPage(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() => IsElementVisible(_loginFormContainer);

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
}