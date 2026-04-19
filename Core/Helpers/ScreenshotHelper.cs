using OpenQA.Selenium;
using SmokeTestSuite.Core.Config; 

namespace SmokeTestSuite.Core.Helpers; 

public static class ScreenshotHelper
{
    public static string? TakeScreenshot(IWebDriver driver, string testName)
    {
        try
        {
            if (driver is not ITakesScreenshot screenshotDriver)
            {
                Console.WriteLine("[SCREENSHOT] Driver does not support screenshots.");
                return null; 
            }

            var sanitisedName = SanitiseFileName(testName); 
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{sanitisedName}_{timestamp}.png";

            var directory = ConfigurationManager.Settings.ScreenshotDirectory;
            var filePath = Path.Combine(directory, fileName);

            var screenshot = screenshotDriver.GetScreenshot();

            screenshot.SaveAsFile(filePath); 

            Console.WriteLine($"[SCREENSHOT] Saved: {filePath}");
            return filePath;

        }

        catch (Exception ex)
        {
            Console.WriteLine($"[SCREENSHOT] Failed to capture screenshot: {ex.Message}");
            return null;
        }
    }

    public static string? TakeStepScreenshot(IWebDriver driver, string stepDescription)
    {
        return TakeStepScreenshot(driver, stepDescription);
    }

    private static string SanitiseFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();

        var sanitised = string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

        return sanitised.Length > 80 ? sanitised[..80] : sanitised;
    }
}

