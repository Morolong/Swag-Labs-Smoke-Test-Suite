using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class Cart : BasePage
{
    private readonly By _logo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _menu = By.Id("menu_button_container");
    private readonly By _pageTitle = By.ClassName("title");
    private readonly By _cartPageContainer = By.Id("cart_contents_container");
    private readonly By _quantityHeader = By.CssSelector("#cart_contents_container > div > div.cart_list > div.cart_quantity_label");
    private readonly By _descriptionHeader = By.CssSelector("#cart_contents_container > div > div.cart_list > div.cart_desc_label");
    private readonly By _purchaseQuantity = By.CssSelector("#cart_contents_container > div > div.cart_list > div.cart_item > div.cart_quantity");
    private readonly By _totalPrice = By.CssSelector("#cart_contents_container > div > div.cart_list > div.cart_item > div.cart_item_label > div.item_pricebar > div");
    private readonly By _removeItemButton = By.CssSelector("#remove-sauce-labs-backpack");
    private readonly By _continueShoppingButton = By.Id("continue-shopping");
    private readonly By _checkOutButton = By.Id("checkout"); 


    public Cart(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
      Wait.ForElementToBeVisible(_cartPageContainer);

    public bool IsAtPage() => base.IsAtPage("/cart.html");
    public IEnumerable<string> GetCartPageElements()
    {
        var elements = new Dictionary<string, By>
        {
            { "Logo",                  _logo },
            { "Checkout Cart",         _checkOutCart },
            { "Menu",                  _menu },
            { "Page Title",            _pageTitle },
            { "Quantity Header",       _quantityHeader },
            { "Description Header",    _descriptionHeader },
            { "Purchase Quantity",     _purchaseQuantity },
            { "Total Price",           _totalPrice },
            { "Remove Button",         _removeItemButton },
            { "Continue Shopping",     _continueShoppingButton },
            { "Checkout Button",       _checkOutButton }
        };

        return elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key);
    }

    public CheckoutCustomerInformation GoToCustInfo()
    {
        Click(_checkOutButton);
        return new CheckoutCustomerInformation(Driver);
    }
}
