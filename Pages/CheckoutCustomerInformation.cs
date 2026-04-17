using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class CheckoutCustomerInformation : BasePage
{
    private readonly By _logo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _menu = By.Id("react-burger-menu-btn");
    private readonly By _checkoutInfoPageTitle = By.ClassName("title");
    private readonly By _pageContainer = By.Id("checkout_info_container");
    private readonly By _firstNameInput = By.Id("first-name");
    private readonly By _lastNameInput = By.Id("last-name");
    private readonly By _zipCodeInput = By.Id("postal-code");
    private readonly By _cancelButton = By.Id("cancel");
    private readonly By _continueButton = By.Id("continue");

    public CheckoutCustomerInformation(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
    Wait.ForElementToBeVisible(_pageContainer);

    public bool IsAtPage() => base.IsAtPage("checkout-step-one.html");

    public IEnumerable<string> GetCheckoutCustInfoElements()
    {
        var elements = new Dictionary<string, By>
        {
            { "Logo",                                    _logo },
            { "Checkout Cart",                           _checkOutCart },
            { "Menu",                                    _menu },
            { "Checkout: Your Information Title",        _checkoutInfoPageTitle },
            { "Checkout Info Page Container",            _pageContainer },
            { "First Name",                              _firstNameInput },
            { "Last Name",                               _lastNameInput},
            { "Zip Code",                                _zipCodeInput },
            { "Cancel",                                  _cancelButton },
            { "Continue",                                _continueButton },
        };

        return elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key);
    }

    protected string GetInputValue(By locator)
    {
        return Driver.FindElement(locator).GetAttribute("value");
    }

    public bool AreCustomerFieldsEmpty()
    {
        return string.IsNullOrEmpty(GetInputValue(_firstNameInput)) &&
               string.IsNullOrEmpty(GetInputValue(_lastNameInput)) &&
               string.IsNullOrEmpty(GetInputValue(_zipCodeInput));
    }


    public CheckoutCustomerInformation EmptyFieldSubmit()
    {
        Click(_continueButton);
        return this;
    }
}
