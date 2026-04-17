using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class CheckoutOverview : BasePage
{
    private readonly By _logo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _menu = By.Id("react-burger-menu-btn");
    private readonly By _checkoutOverviewPageTitle = By.ClassName("title");
    private readonly By _pageContainer = By.Id("checkout_summary_container");
    private readonly By _quantityHeader = By.CssSelector("#checkout_summary_container > div > div.cart_list > div.cart_quantity_label");
    private readonly By _descriptionHeader = By.CssSelector("#checkout_summary_container > div > div.cart_list > div.cart_desc_label");
    private readonly By _purchaseQuantity = By.CssSelector("#checkout_summary_container > div > div.cart_list > div.cart_item > div.cart_quantity");
    private readonly By _price = By.CssSelector("#checkout_summary_container > div > div.cart_list > div.cart_item > div.cart_item_label > div.item_pricebar > div");
    private readonly By _payInfoTitle = By.CssSelector("#checkout_summary_container > div > div.summary_info > div:nth-child(1)");
    private readonly By _payInfoDescription = By.CssSelector("#checkout_summary_container > div > div.summary_info > div:nth-child(2)");
    private readonly By _shipInfoTitle = By.CssSelector("#checkout_summary_container > div > div.summary_info > div:nth-child(3)");
    private readonly By _shipInfoDescription = By.CssSelector("#checkout_summary_container > div > div.summary_info > div:nth-child(3)");
    private readonly By _priceTotalTitle = By.CssSelector("#checkout_summary_container > div > div.summary_info > div:nth-child(5)");
    private readonly By _itemTotalDescription = By.CssSelector("#checkout_summary_container > div > div.summary_info > div.summary_subtotal_label");
    private readonly By _taxTotalDescription = By.CssSelector("#checkout_summary_container > div > div.summary_info > div.summary_subtotal_label");
    private readonly By _priceTotal = By.CssSelector("#checkout_summary_container > div > div.summary_info > div.summary_subtotal_label");
    private readonly By _cancelButton = By.Id("cancel");
    private readonly By _finishButton = By.Id("finish");

    public CheckoutOverview(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
    Wait.ForElementToBeVisible(_pageContainer);

    public bool IsAtPage() => base.IsAtPage("checkout-step-two.html");

    public IEnumerable<string> GetCheckoutCustInfoElements()
    {
        var elements = new Dictionary<string, By>
        {
            { "Logo",                                    _logo },
            { "Checkout Cart",                           _checkOutCart },
            { "Menu",                                    _menu },
            { "Checkout: Overview",                      _checkoutOverviewPageTitle },
            { "Checkout Overvew Page Container",         _pageContainer },
            { "QTY Title",                               _quantityHeader },
            { "Quantity",                                _purchaseQuantity},
            { "Item Price",                              _price },
            { "Payment Information Title",               _payInfoTitle },
            { "Payment Information Description",         _payInfoDescription },
            { "Product Description",                     _descriptionHeader },
            { "Shipping Information Title",              _shipInfoTitle},
            { "Shipping Information Description",        _shipInfoDescription},
            { "Price Total Title",                       _priceTotalTitle },
            { "Item Total Price",                        _itemTotalDescription },
            { "Tax Total",                               _taxTotalDescription},
            { "Total Purchas Price",                     _priceTotal  },
            { "Cancel Button",                           _cancelButton },
            { "Finish Button",                           _finishButton},
        };

        return elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key);
    }
}