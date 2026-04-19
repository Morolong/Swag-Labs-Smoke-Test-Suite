using SmokeTestSuite.Core;
using SmokeTestSuite.Core.Config;
using SmokeTestSuite.Pages;

namespace SmokeTestSuite.Tests;

[TestFixture]
[Category("ProductDetail")]
[Parallelizable(ParallelScope.All)]
public class ProductDetailTest : BaseTest
{
    private ProductDetail _productDetail;

    [SetUp]
    public void ProductDetailSetUp()
    {
        var username = ConfigurationManager.Credentials.PositiveTestUser;
        var password = ConfigurationManager.Credentials.LoginPassword;

        var loginPage = new LoginPage(Driver!);
        loginPage.Open();

        Log("Navigating to Login Page");
        var inventoryPage = loginPage.LoginAs(username, password);

        Log("Logging into Sauce Labs.");
        _productDetail = inventoryPage.GoToProductDetail();
    }

    [Test]
    [Category("Smoke")]
    [Property("TestID", "SMK-004")]
    [Description("Product Detail Loads Correctly")]
    public void VerifyProductDetailPageElements()
    {
        Log("Checking that user was directed to the Product Detail page.");
        Assert.That(_productDetail.IsAtPage(), Is.True,
            "Expected to be on the cart page");

        var productDetailPageElements = _productDetail.GetProductDetailPageElements();

        Assert.That(productDetailPageElements, Is.Empty,
            $"Elements not visible: {string.Join(", ", productDetailPageElements)}");
    }
}