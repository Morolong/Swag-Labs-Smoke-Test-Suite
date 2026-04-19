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

    protected override void WaitForPageToLoad() =>
      Wait.ForElementToBeVisible(_inventoryContainer);

    public bool IsPageDisplayed() => IsElementVisible(_inventoryContainer);
    public bool IsLogoVisible() => IsElementVisible(_appLogo);
    public bool IsCartVisible() => IsElementVisible(_checkOutCart);
    public bool IsHeadingVisible() => IsElementVisible(_pageHeading);
    public bool IsFilterVisible() => IsElementVisible(_filter);
    public bool IsRemoveButtonVisible() => IsElementVisible(_removeButton);

    public int GetStockCount() => GetElementCount(_stockItem);
    public int GetStockImageCount() => GetElementCount(_stockImage);
    public int GetStockLabelCount() => GetElementCount(_stockPrice);
    public int GetAddToCartButtonCount() => GetElementCount(_addToCartButton);

    public bool IsAtInventoryPage()
    {
        return Driver.Url.Contains("/inventory.html");
    }
    public ProductInventory ClickAddToCart(string productId)
    {
        Click(By.Id($"add-to-cart-{productId}"));
        return this;
    }

    public Cart GoToCart()
    {
        Click(_checkOutCart);
        return new Cart(Driver);
    }

    public ProductDetail GoToProductDetail()
    {
        Click(_redTShirt);
        return new ProductDetail(Driver);
    }

    public LoginPage Logout()
    {
        Click(_menu);

        Wait.ForElementToBeClickable(_logOutOption);

        Click(_logOutOption);

        return new LoginPage(Driver);
    }
    public ProductInventory ClickRemoveButton()
    {
        Click(_removeButton);
        return this;
    }
}