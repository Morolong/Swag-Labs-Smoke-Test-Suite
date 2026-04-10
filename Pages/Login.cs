using OpenQA.Selenium;
using SmokeTestSuite.Core; 

namespace SmokeTestSuite.Pages; 

public class LoginPage : BasePage
{
    private static readonly By UsernameField = By.Id("email");
    private static readonly By PasswordField = By.Id("password");
    private static readonly By LoginButton = By.CssSelector("[data-testId='login-btn']");
    private static readonly By ErrorMessage = By.CssSelector(".alert-danger");
    private static readonly By LoginFormContainer = By.CssSelector(".login-container");
    
    protected override string PagePath => "/login"; 

    public LoginPage(IWebDriver driver) : base(driver) { }

    public LoginPage EnterUsername(string username)
    {
        TypeText(UsernameField, username);
        return this; 
    }

    public LoginPage EnterPassword(string password)
    {
        TypeText(PasswordField, password); 
        return this; 
    }

    public void ClickLoginExpectingFailure()
    {
        Click(LoginButton); 
    }

    public ProductInventory ClickLogin()
    {
        Click(LoginButton);
        return new ProductInventory(Driver);
    }

    public ProductInventory LoginAs(string username, string password)
    {
        return EnterUsername(username)
            .EnterPassword(password)
            .ClickLogin(); 
    }

    public string GetErrorMessage()
    {
        return IsElementVisible(ErrorMessage)
            ? GetText(ErrorMessage)
            : string.Empty; 
    }

    public bool IsErrorMessageDisplayed() => IsElementVisible(ErrorMessage);

    public string GetUsernamePlaceholder() => GetAttribute(UsernameField, "placeholder");

    protected override bool IsPageLoaded()
    {
        return IsElementVisible(LoginFormContainer);
    }

}
