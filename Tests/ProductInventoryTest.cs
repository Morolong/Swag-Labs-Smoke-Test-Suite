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

        Log("Navigating to Login Page");
        var loginPage = new LoginPage(Driver);
        loginPage.Open();

        Log("Logging in to Sauce Labs.");
        _inventoryPage = loginPage.LoginAs(username, password);
        Log("Inventory Page has been displayed.");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-003")]
    [Description("Product Inventory Page Loads Correctly")]
    public void VerifyInventoryPageUrl()
    {
        var inventoryPage = new ProductInventory(Driver);
        string currentUrl = Driver.Url;

        var inventoryElements = _inventoryPage.GetInventoryPageElements();

        Assert.That(inventoryElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", inventoryElements)}");
    }

    [Test]
    public void VerifyStockItemCounts()
    {
        var inventoryPage = new ProductInventory(Driver);

        var inventoryElementsCount = _inventoryPage.GetStockCounts();

        Assert.That(inventoryElementsCount, Is.Empty,
            $"Elements not visible: {string.Join(", ", inventoryElementsCount)}");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-005")]
    [Description("Add a product to the cart")]
    public void VerifyRemoveButtonAppearsAfterAddToCart()
    {
        var inventoryPage = new ProductInventory(Driver).
            ClickAddToCart("sauce-labs-backpack");

        Assert.That(inventoryPage.IsRemoveButtonVisible(), Is.True,
            "Remove is not visible after adding item to cart");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-006")]
    [Description("Remove a Product from the Cart")]
    public void RemoveProductFromCart()
    {
        var inventoryPage = new ProductInventory(Driver)
        .ClickAddToCart("sauce-labs-backpack")
        .ClickRemoveButton();

        Assert.That(inventoryPage.IsRemoveButtonVisible(), Is.False,
            "Remove button should not be visible after removing item from cart");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-013")]
    [Description("User logout")]
    public void VerifyUserCanLogOut()
    {
        var loginPage = _inventoryPage.Logout();

        Log("Checking that user was directed to the Inventory page.");
        Assert.That(loginPage.IsAtPage(), Is.True,
            "User was not redirected to base login page after logout");

        Assert.That(loginPage.IsPageDisplayed(), Is.True,
            "Login page UI was not displayed after logout"); 
    }
}