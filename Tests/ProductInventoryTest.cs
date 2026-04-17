using NUnit.Framework;
using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("ProductInventory")]
[Parallelizable(ParallelScope.All)]
public class ProductInventoryTests : BaseTest
{
    private ProductInventory _inventoryPage;

    [SetUp]
    public void SetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;

        var loginPage = new LoginPage(Driver);
        loginPage.Open();

        _inventoryPage = loginPage.LoginAs(username, password);
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-003")]
    [Description("Product Inventory Page Loads Correctly")]
    public void VerifyInventoryPageUrl()
    {
        var inventoryPage = new ProductInventory(Driver);
        string currentUrl = Driver.Url;

        Assert.That(_inventoryPage.IsAtPage(), Is.True,
            "URL does not contain '/inventory.html'");

        Assert.That(inventoryPage.IsPageDisplayed(), Is.True,
            "Inventory page UI not displayed");

        Assert.That(inventoryPage.IsLogoVisible(), Is.True,
            "Logo did not load");

        Assert.That(inventoryPage.IsCartVisible(), Is.True,
            "Check Out Cart did not load");

        Assert.That(inventoryPage.IsHeadingVisible(), Is.True,
            "Page Heading did not lead");

        Assert.That(inventoryPage.IsFilterVisible(), Is.True,
            "Filter button did not load.");
    }

    [Test]
    public void VerifyStockItemCounts()
    {
        var inventoryPage = new ProductInventory(Driver);

        Assert.Multiple(() =>
        {
            Assert.That(inventoryPage.GetStockCount(), Is.EqualTo(6), "Stock item count mismatch");
            Assert.That(inventoryPage.GetStockImageCount, Is.EqualTo(6), "Stock image count mismatch");
            Assert.That(inventoryPage.GetStockLabelCount, Is.EqualTo(6), "Stock label count mismatch");
            Assert.That(inventoryPage.GetAddToCartButtonCount(), Is.EqualTo(6), "Add to cart button count mismatch");
        });
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-005 & SMK-006")]
    [Description("Check that Remove Button is visible")]
    public void VerifyRemoveButtonAppearsAfterAddToCart()
    {
        var inventoryPage = new ProductInventory(Driver).
            ClickAddToCart("sauce-labs-backpack");

        Assert.That(inventoryPage.IsRemoveButtonVisible(), Is.True,
            "Remove is not visible after adding item to cart");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-013")]
    [Description("User logout")]
    public void VerifyUserCanLogOut()
    {
        var loginPage = _inventoryPage.Logout();

        Assert.That(loginPage.IsAtPage(), Is.True,
            "User was not redirected to base login page after logout");

        Assert.That(loginPage.IsPageDisplayed(), Is.True,
            "Login page UI was not displayed after logout"); 
    }
}