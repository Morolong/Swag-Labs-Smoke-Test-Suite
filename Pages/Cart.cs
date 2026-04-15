using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class Cart : BasePage
{
    private readonly By _cartItem = By.ClassName("cart_item");

    public Cart(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
      Wait.ForElementToBeVisible(_cartItem);
}
