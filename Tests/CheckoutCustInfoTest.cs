using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("CheckOutCustInfo")]
[Parallelizable(ParallelScope.All)]
public class CheckOutCustInfoTest : BaseTest
{
    private CheckoutCustomerInformation _checkoutCustInfo;

    [SetUp]
    public void CheckOutCustInfoSetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;

        Log("Navigating to Login Page");
        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        Log("Logging in to Sauce Labs.");
        var inventoryPage = loginPage.LoginAs(username, password);

        _checkoutCustInfo = inventoryPage
    .ClickAddToCart("sauce-labs-backpack")
    .GoToCart().GoToCustInfo();
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-008")]
    [Description("Verify Customer Informatin Page Loads Correctly")]
    public void VerifyCheckoutCustInfoPageElements()
    {
        Log("Checking that user was directed to the Checkout: Customer Information page.");
        Assert.That(_checkoutCustInfo.IsAtPage(), Is.True,
            "Expected to be on the Checkout:Customer Information page");

        var checkoutCustInfoPageElements = _checkoutCustInfo.GetCheckoutCustInfoElements();

        Assert.That(checkoutCustInfoPageElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", checkoutCustInfoPageElements)}");
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-009")]
    [Description("Validation on Empty Fields")]
    public void VerifyEmptyFieldSubmission()
    {
        Assert.IsTrue(_checkoutCustInfo.AreCustomerFieldsEmpty(),
    "Expected all checkout fields to be empty before submitting.");

        _checkoutCustInfo.EmptyFieldSubmit();

        Assert.Multiple(() =>
        {
            Assert.That(_checkoutCustInfo.IsEmptyFieldErrorMessageDisplayed(), Is.True,
                "An error message should be displayed for invalid credentials");

            Assert.That(_checkoutCustInfo.GetEmptyInputErrorMessage(), Does.Contain("Error"),
                "Error message should indicate that the field is required");
        });
    }
}