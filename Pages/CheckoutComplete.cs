using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class CheckoutComplete : BasePage
{
    private readonly By _logo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _menu = By.Id("react-burger-menu-btn");
    private readonly By _checkoutCompletePageTitle = By.ClassName("title");
    private readonly By _pageContainer = By.Id("checkout_complete_container");
    private readonly By _backHomeButton = By.Id("back-to-products");

    public CheckoutComplete(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
    Wait.ForElementToBeVisible(_pageContainer);

    public bool IsAtPage() => base.IsAtPage("checkout-complete.html");

    public IEnumerable<string> GetCheckOutCompleteElements()
    {
        var elements = new Dictionary<string, By>
        {
            { "Logo",                                    _logo },
            { "Checkout Cart",                           _checkOutCart },
            { "Menu",                                    _menu },
            { "Checkout: Complete",                      _checkoutCompletePageTitle },
            { "Checkout Overview Page Container",         _pageContainer },
            { "Back Home Button",                               _backHomeButton},
        };

        return elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key);
    }

    public ProductInventory ClickBackHomeButton()
    {
        Click(_backHomeButton);
        return new ProductInventory(Driver);
    }

}