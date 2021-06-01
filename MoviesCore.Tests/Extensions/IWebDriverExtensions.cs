/* This Sample Code is provided for the purpose of illustration only 
 * and is not intended to be used in a production environment.   
 */

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace MoviesCore.Tests.Extensions
{
    public static class IWebDriverExtensions
	{
		public static void WaitUntilStale(this IWebDriver driver, IWebElement element, int timeoutSeconds)
		{			
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
			wait.Until(d => IsStale(element)); // waits for this page to get unloaded, signalling new page has loaded
			return;

			bool IsStale(IWebElement checkingElement)
			{
				try
				{
					var ignore = checkingElement.Enabled;
					return false;
				}
				catch (StaleElementReferenceException)
				{
					return true;
				}
			}
		}

		public static IWebElement WaitUntilExists(this IWebDriver driver, By selector, int timeoutSeconds)
		{
			IWebElement foundElement = null;
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
			wait.Until(d => foundElement = d.FindElement(selector));
			return foundElement;
		}
	}
}
