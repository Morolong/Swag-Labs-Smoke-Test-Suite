using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("CheckoutOverview")]
[Parallelizable(ParallelScope.All)]
public class CheckoutOverviewTest : BaseTest
{
    private CheckoutOverview _checkoutOverview;

    [SetUp]
    public void CheckoutOverviewSetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;
        var firstName = ConfigurationManager.UserDetails.FirstName;
        var lastName = ConfigurationManager.UserDetails.LastName;
        var zipCode = ConfigurationManager.UserDetails.ZipCode;

        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        var inventoryPage = loginPage.LoginAs(username, password);

        _checkoutOverview = inventoryPage
    .ClickAddToCart("sauce-labs-backpack")
    .GoToCart().GoToCustInfo().EnterFirstName(firstName)
    .EnterLastName(lastName).EnterZipCode(zipCode)
    .ClickContinue();
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-010")]
    [Description("Verify Checkout: Overview Page Loads Correctly")]
    public void VerifyCheckoutOverviewPageElements()
    {
        Assert.That(_checkoutOverview.IsAtPage(), Is.True,
            "Expected to be on the Checkout:Customer Information page");

        var checkoutOverviewPageElements = _checkoutOverview.GetCheckoutOverviewElements();

        Assert.That(checkoutOverviewPageElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", checkoutOverviewPageElements)}");
    }
}