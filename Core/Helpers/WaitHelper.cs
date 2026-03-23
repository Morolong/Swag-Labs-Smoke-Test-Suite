using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SmokeTestSuite.Core.Helpers
{
    public class WaitHelper
    {
        private readonly WebDriverWait _wait;
        private readonly int _timeoutSeconds;
        public WaitHelper(IWebDriver driver, int timeoutSeconds)
        {
            _timeoutSeconds = timeoutSeconds;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(250),
            };

            _wait.IgnoreExceptionTypes(
                 typeof(NoSuchElementException),
                 typeof(StaleElementReferenceException));
        }
        public IWebElement ForElementToBeVisible(By locator)
        {
            return _wait.Until(SeleniumConditions.ElementIsVisible(locator));
        }
        public IWebElement ForElementToBeClickable(By locator)
        {
            return _wait.Until(SeleniumConditions.ElementToBeClickable(locator));
        }
        public IWebElement ForElementToExist(By locator)
        {
            return _wait.Until(SeleniumConditions.ElementExists(locator));
        }
        public bool ForElementToDisappear(By locator)
        {
            return _wait.Until(SeleniumConditions.InvisibilityOfElementLocated(locator));
        }
        public bool ForUrlToContain(string urlFragment)
        {
            return _wait.Until(SeleniumConditions.UrlContains(urlFragment));
        }
        public bool ForTitleToBe(string expectedTitle)
        {
            return _wait.Until(SeleniumConditions.TitleIs(expectedTitle));
        }
       //May not be necessary as there is already driver in _wait 
        /* public void ForPageToLoad(IWebDriver driver)
        {
            _wait.Until(d =>
            {
                var readyState = ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString();
                return readyState == "complete";
            });
        }*/
        public void Until(Func<bool> condition, string errormessage = "Condition was not met within timeout")
        {
            try
            {
                _wait.Until(_ => condition());
            }
            catch (WebDriverTimeoutException)
            {
                throw new TimeoutException($"{errormessage} (timeout: {_timeoutSeconds}s)");
            }
        }
        //Avoid Hard Waits as far as possible. 
       /* public static void HardWait(int milliseconds)
        {

            Thread.Sleep(milliseconds);
        }*/
    }
}
