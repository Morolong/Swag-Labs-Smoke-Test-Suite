using NUnit.Framework;
using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("Login")]
[Parallelizable(ParallelScope.All)]
public class LoginTests: BaseTest
{
    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-001")]
    [Description("Successful login with valid credentials")]
    public void LoginWithValidCredentials()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;

        Log("Navigating to Login Page");
        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        Log($"Logging in as: {username}");
        var productInventoryPage = loginPage.LoginAs(username, password);

        Assert.That(productInventoryPage.IsPageDisplayed(),Is.True,
            "ProductInventoryPage should be loaded after successful login");

        Log("Login successful - Inventory is displayed"); 
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-002")]
    [Description("Login rejected with invalid credentials")]
    [Order(2)]
    public void Login_WithInvalidCredentials()
    {
        var invalidUsername = ConfigurationManager.Credentials.NegativeTestUser;
        var invalidPassword = ConfigurationManager.Credentials.NegativePassword;

        Log("Navigating to Login Page");
        var loginPage = new LoginPage(Driver!); 
        loginPage.Open();

        Log("Entering invalid credentials");
        loginPage
            .EnterUsername(invalidUsername)
            .EnterPassword(invalidPassword)
            .ClickLoginExpectingFailure();

        Assert.Multiple(() =>
        {
            Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True,
                "An error message should be displayed for invalid credentials");

            Assert.That(loginPage.GetErrorMessage(), Does.Contain("Epic sadface").Or.Contain("not match"),
                "Error message should indicate credentials are wrong");
        });
    }
}