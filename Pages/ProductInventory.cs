using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class ProductInventory : BasePage
{
    private readonly By _inventoryContainer = By.Id("inventory_container");
    private readonly By _appLogo = By.ClassName("app_logo");
    private readonly By _shoppingCart = By.Id("shopping_cart_container");
    private readonly By _pageHeading = By.ClassName("title");
    private readonly By _filter = By.ClassName("select_container");

    protected override string PagePath => "/inventory";

    public ProductInventory(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() => IsElementVisible(_inventoryContainer);

    public bool IsPageDisplayed() => IsElementVisible(_inventoryContainer);
    public bool IsLogoVisible() => IsElementVisible(_appLogo);
    public bool IsCartVisible() => IsElementVisible(_shoppingCart);
    public bool IsHeadingVisible() => IsElementVisible(_pageHeading);
    public bool IsFilterVisible() => IsElementVisible(_filter);
}