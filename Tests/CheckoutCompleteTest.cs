using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("CheckoutComplete")]
[Parallelizable(ParallelScope.All)]
public class CheckoutCompleteTest : BaseTest
{
    private CheckoutComplete _checkoutComplete;

    [SetUp]
    public void CheckoutCompleteSetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;
        var firstName = ConfigurationManager.UserDetails.FirstName;
        var lastName = ConfigurationManager.UserDetails.LastName;
        var zipCode = ConfigurationManager.UserDetails.ZipCode;

        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        var inventoryPage = loginPage.LoginAs(username, password);

        _checkoutComplete = inventoryPage
    .ClickAddToCart("sauce-labs-backpack")
    .GoToCart().GoToCustInfo().EnterFirstName(firstName)
    .EnterLastName(lastName).EnterZipCode(zipCode)
    .ClickContinue().ClickFinish();
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-011")]
    [Description("Successful order completion")]
    public void VerifyCheckoutCompletePageElements()
    {
        Assert.That(_checkoutComplete.IsAtPage(), Is.True,
            "Expected to be on the Checkout:Customer Information page");

        var checkoutCompletePageElements = _checkoutComplete.GetCheckOutCompleteElements();

        Assert.That(checkoutCompletePageElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", checkoutCompletePageElements)}");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-012")]
    [Description("Return to Home Page After Order Completed")]
    public void ReturnHomeAfterOrderCompleted()
    {
        var inventoryPage = _checkoutComplete.ClickBackHomeButton();

        Assert.That(inventoryPage.IsAtPage(), Is.True,
            "Expected to be returned to the Inventory page after clicking Back Home");
    }
}
