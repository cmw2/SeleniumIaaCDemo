/* This Sample Code is provided for the purpose of illustration only 
 * and is not intended to be used in a production environment.   
 */

using MoviesCore.Tests.Extensions;
using OpenQA.Selenium;
using System;

namespace MoviesCore.Tests.PageObjectModels
{
    public class MovieIndexPage
    {
		IWebDriver driver;
		string pageUrl;
		

		public MovieIndexPage(IWebDriver driver)
		{
			this.driver = driver;
			Uri baseUri = new Uri(driver.Url); // assumptiong for now.  need code to strip it down because won't always be true.
			Uri pageUri = new Uri(baseUri, "/Movies/");
			this.pageUrl = pageUri.AbsoluteUri;
		}

		public MovieIndexPage OpenPage()
		{
			driver.Navigate().GoToUrl(pageUrl);
			return this;
		}

		public MovieCreatePage ClickCreateNew()
		{
			IWebElement link = driver.FindElement(UIMap.MovieIndexPage.CreateNewLink);
			link.Click();
			var movieTitleField = driver.WaitUntilExists(UIMap.MovieCreatePage.TitleField, 5);
			return new MovieCreatePage(driver);
		}

		public bool IsOnPage()
		{
			return driver.Title.StartsWith("Index", StringComparison.OrdinalIgnoreCase);
		}

		public IWebElement FindMovieByTitle(string title)
		{
			return driver.FindElement(By.XPath($"//table/tbody/tr/td[contains(text(), '{title}')]"));
		}
	}
}
