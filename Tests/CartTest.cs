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
        Assert.That(_cartPage.IsAtPage(), Is.True,
            "Expected to be on the cart page");

        var hiddenElements = _cartPage.GetHiddenElements();

        Assert.That(hiddenElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", hiddenElements)}");
    }
}