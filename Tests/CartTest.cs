using NUnit.Framework;
using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("Cart")]
[Parallelizable(ParallelScope.All)]
public class CartTests : BaseTest
{
    private Cart _cartPage;

    [SetUp]
    public void CartSetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;

        Log("Navigating to Login Page");
        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        var inventoryPage = loginPage.LoginAs(username, password);

        _cartPage = inventoryPage
            .ClickAddToCart("sauce-labs-backpack")
            .GoToCart();
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-007")]
    [Description("Cart Page Loads Correctly")]
    public void VerifyCartPageElements()
    {
        FailureMessage = "Cart Page did not load correctly or the URL has been changed.";

        Log("Checking that Cart Page has the correct URL.");
        Assert.That(_cartPage.IsAtPage(), Is.True,
            FailureMessage);

        var cartPageElements = _cartPage.GetCartPageElements();

        Assert.That(cartPageElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", cartPageElements)}");
    }
}