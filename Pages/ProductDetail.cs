using OpenQA.Selenium;
using SmokeTestSuite.Core;

namespace SmokeTestSuite.Pages;

public class ProductDetail : BasePage
{
    private readonly By _logo = By.ClassName("app_logo");
    private readonly By _checkOutCart = By.Id("shopping_cart_container");
    private readonly By _menu = By.Id("react-burger-menu-btn");
    private readonly By _backToProducts = By.Id("back-to-products");
    private readonly By _productPageContainer = By.CssSelector("#inventory_item_container > div > div");
    private readonly By _productImage = By.CssSelector("#inventory_item_container > div > div > div.inventory_details_img_container > img");
    private readonly By _productTitle = By.CssSelector("#inventory_item_container > div > div > div.inventory_details_desc_container > div.inventory_details_name.large_size");
    private readonly By _productDescription = By.CssSelector("#inventory_item_container > div > div > div.inventory_details_desc_container > div.inventory_details_desc.large_size");
    private readonly By _productPrice = By.CssSelector("#inventory_item_container > div > div > div.inventory_details_desc_container > div.inventory_details_price");
    private readonly By _addToCartButton = By.Id("add-to-cart");
    public ProductDetail(IWebDriver driver) : base(driver) { }

    protected override void WaitForPageToLoad() =>
  Wait.ForElementToBeVisible(_productPageContainer);

    public bool IsAtPage() => base.IsAtPage("inventory-item.html?id=3");

    public IEnumerable<string> GetProductDetailPageElements()
    {
        var elements = new Dictionary<string, By>
        {
            { "Logo",                    _logo },
            { "Checkout Cart",           _checkOutCart },
            { "Menu",                    _menu },
            { "Back To Products",        _backToProducts },
            { "Product Image",           _productImage },
            { "Product Title",           _productTitle },
            { "Product Description",     _productDescription},
            { "Product Price",           _productPrice },
            { "Add To Cart ",            _addToCartButton },
        };

        return elements
            .Where(e => !IsElementVisible(e.Value))
            .Select(e => e.Key);
    }
}