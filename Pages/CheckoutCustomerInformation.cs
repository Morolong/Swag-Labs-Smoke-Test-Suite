using OpenQA.Selenium;
using SmokeTestSuite.Core;
using System.Reflection.Emit;

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
    private readonly By _emptyFieldErrorMsg = By.XPath("/html/body/div/div/div/div[2]/div/form/div[1]/div[4]/h3");
    public CheckoutCustomerInformation(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad()
    {
        LogStep("Waiting for Checkout: Customer Information Page to load");

        try
        {
            Wait.ForElementToBeVisible(_pageContainer);
            LogStep("Checkout: Customer Information Page loaded successfully");
        }
        catch (WebDriverTimeoutException ex)
        {
            LogStep($"Checkout: Customer Information Page failed to load - container element not visible within timeout: {ex.Message}");
            throw;
        }
    }

    public bool IsAtPage() => base.IsAtPage("checkout-step-one.html");

    public IEnumerable<string> GetCheckoutCustInfoElements()
    {
        LogStep($"Checking that Checkout: Customer Information Page elements are visible.");
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

        var missingElements = elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key)
            .ToList();

        if (missingElements.Any())
            LogStep($"Missing elements: {string.Join(", ", missingElements)}");
        else
            LogStep("All Checkout: Customer Information Page elements are visible");

        return missingElements;
    }

    protected string GetInputValue(By locator)
    {
        return Driver.FindElement(locator).GetAttribute("value");
    }

    public bool AreCustomerFieldsEmpty()
    {
        LogStep("Checking customer input fields are empty");

        bool firstNameEmpty = string.IsNullOrEmpty(GetInputValue(_firstNameInput));
        bool lastNameEmpty = string.IsNullOrEmpty(GetInputValue(_lastNameInput));
        bool zipCodeEmpty = string.IsNullOrEmpty(GetInputValue(_zipCodeInput));

        if (!firstNameEmpty)
            LogStep("First Name field is not empty");

        if (!lastNameEmpty)
            LogStep("Last Name field is not empty");

        if (!zipCodeEmpty)
            LogStep("Zip Code field is not empty");

        bool allEmpty = firstNameEmpty && lastNameEmpty && zipCodeEmpty;

        if (allEmpty)
            LogStep("All customer fields are empty");

        return allEmpty;
    } 

    public CheckoutCustomerInformation EmptyFieldSubmit()
    {
        LogStep("Click Continue Button");
        Click(_continueButton);
        return this;
    }

    public string GetEmptyInputErrorMessage()
    {
        return IsElementVisible(_emptyFieldErrorMsg)
            ? GetText(_emptyFieldErrorMsg)
            : string.Empty;
    }

    public bool IsEmptyFieldErrorMessageDisplayed() => IsElementVisible(_emptyFieldErrorMsg);

    public CheckoutCustomerInformation EnterFirstName(string firstName)
    {
        LogStep($"Typing {firstName} into the First Name field.");
        TypeText(_firstNameInput, firstName);
        return this;
    }

    public CheckoutCustomerInformation EnterLastName(string lastName)
    {
        LogStep($"Typing {lastName} into the Last Name field.");
        TypeText(_lastNameInput, lastName);
        return this;
    }

    public CheckoutCustomerInformation EnterZipCode(string zipCode)
    {
        LogStep($"Typing {zipCode} into the Zip Code field.");
        TypeText(_zipCodeInput, zipCode);

        return this;
    }

    public CheckoutCustomerInformation EnterUserDetails(string firstName, string lastName, string zipCode)
    {
        return EnterFirstName(firstName)
            .EnterLastName(lastName)
            .EnterZipCode(zipCode);
    }

    public CheckoutOverview ClickContinue()
    {
        LogStep("Clicking the continue Button");
        Click(_continueButton);
        return new CheckoutOverview(Driver);
    }
}
