using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class ProductInventory : BasePage
{
    private readonly By _inventoryContainer = By.Id("inventory_container");
    private readonly By _appLogo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _pageHeading = By.ClassName("title");
    private readonly By _filter = By.ClassName("select_container");
    private readonly By _stockItem = By.ClassName("inventory_item");
    private readonly By _addBackPack = By.Id("add-to-cart-sauce-labs-backpack");
    private readonly By _stockImage = By.CssSelector(".inventory_item_img img");
    private readonly By _stockPrice = By.ClassName("inventory_item_price");
    private readonly By _addToCartButton = By.CssSelector("[id^='add-to-cart-']");
    private readonly By _removeButton = By.Id("remove-sauce-labs-backpack");
    private readonly By _redTShirt = By.CssSelector("#item_3_img_link > img");
    private readonly By _logOutOption = By.Id("logout_sidebar_link");
    private readonly By _menu = By.Id("react-burger-menu-btn");

    public bool IsAtPage() => base.IsAtPage("/inventory.html");

    public ProductInventory(IWebDriver driver) : base(driver) { }

    public bool IsRemoveButtonVisible() => IsElementVisible(_removeButton);
    public bool IsPageDisplayed() => IsElementVisible(_inventoryContainer);

    protected override void WaitForPageToLoad()
    {
        LogStep("Waiting for Inventory Page to load");

        try
        {
            Wait.ForElementToBeVisible(_inventoryContainer);
            LogStep("Inventory Page loaded successfully");
        }
        catch (WebDriverTimeoutException ex)
        {
            LogStep($"Inventory Page failed to load - container element not visible within timeout: {ex.Message}");
            throw;
        }
    }

    public IEnumerable<string> GetInventoryPageElements()
    {
        LogStep($"Checking Inventory Page elements are visible.");
        var elements = new Dictionary<string, By>
        {
            { "App Logo",             _appLogo },
            { "Check-out Cart",       _checkOutCart },
            { "Page Title",           _pageHeading },
            { "Filter",               _filter },
        };
        var missingElements = elements
        .Where(e => !IsElementVisible(e.Value))
        .Select(e => e.Key)
        .ToList();

        if (missingElements.Any())
            LogStep($"Missing elements: {string.Join(", ", missingElements)}");
        else
            LogStep("All Inventory Page elements are visible");

        return missingElements;
    }

    public Dictionary<string, int> GetStockCounts()
    {
        LogStep("Checking Inventory Page stock counts");

        var counts = new Dictionary<string, int>
    {
        { "Stock Items",         GetElementCount(_stockItem) },
        { "Stock Images",        GetElementCount(_stockImage) },
        { "Stock Labels",        GetElementCount(_stockPrice) },
        { "Add To Cart Buttons", GetElementCount(_addToCartButton) }
    };

        var mismatchedCounts = counts
            .Where(c => c.Value != 6)
            .ToDictionary(c => c.Key, c => c.Value);

        if (mismatchedCounts.Any())
            LogStep($"Stock count mismatches: {string.Join(", ", mismatchedCounts.Select(c => $"{c.Key}: found {c.Value}, expected 6"))}");
        else
            LogStep("All stock counts are correct — 6 of each");

        return mismatchedCounts;
    }

    public bool IsAtInventoryPage()
    {
        return Driver.Url.Contains("/inventory.html");
    }
    public ProductInventory ClickAddToCart(string productId)
    {
        LogStep($"Clicking on Add To Cart button to add {productId} to the cart.");
        Click(By.Id($"add-to-cart-{productId}"));
        return this;
    }

    public Cart GoToCart()
    {
        LogStep($"Clicking on Check-out Cart button");
        Click(_checkOutCart);
        return new Cart(Driver);
    }

    public ProductDetail GoToProductDetail()
    {
        LogStep($"Clicking on the red t shirt image.");
        Click(_redTShirt);
        return new ProductDetail(Driver);
    }

    public LoginPage Logout()
    {
        LogStep($"Clicking on the Menu button");
        Click(_menu);

        LogStep($"Waiting for the Log Out option to be visible");
        Wait.ForElementToBeClickable(_logOutOption);

        LogStep($"Clicking on the Log Out option");
        Click(_logOutOption);

        return new LoginPage(Driver);
    }
    public ProductInventory ClickRemoveButton()
    {
        LogStep($"Clicking the remove item button.");
        Click(_removeButton);
        return this;
    }
}