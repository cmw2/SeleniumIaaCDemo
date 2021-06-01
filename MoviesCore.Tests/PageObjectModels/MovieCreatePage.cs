/* This Sample Code is provided for the purpose of illustration only 
 * and is not intended to be used in a production environment.   
 */

using MoviesCore.Tests.Extensions;
using OpenQA.Selenium;
using System;

namespace MoviesCore.Tests.PageObjectModels
{
    public class MovieCreatePage
    {
		IWebDriver driver;
		string pageUrl;
		
		public MovieCreatePage(IWebDriver driver)
		{
			this.driver = driver;
			Uri baseUri = new Uri(driver.Url); // assumption for now.  need code to strip it down because won't always be true.
			Uri pageUri = new Uri(baseUri, "/Movies/Create/");
			this.pageUrl = pageUri.AbsoluteUri;
		}

		public MovieCreatePage OpenPage()
		{
			driver.Navigate().GoToUrl(pageUrl);
			return this;
		}

		public IWebElement TitleField()
		{
			return driver.FindElement(UIMap.MovieCreatePage.TitleField);
		}

		public MovieCreatePage SetTitle(string title)
		{
			TitleField().SendKeys(title);
			return this;
		}

		public MovieCreatePage SetReleaseDate(string releaseDate)
		{
			var movieTitleField = driver.FindElement(UIMap.MovieCreatePage.ReleaseDateField);
			movieTitleField.SendKeys(releaseDate);
			return this;
		}

		public MovieCreatePage SetGenre(string genre)
		{
			var movieTitleField = driver.FindElement(UIMap.MovieCreatePage.GenreField);
			movieTitleField.SendKeys(genre);
			return this;
		}

		public MovieCreatePage SetPrice(string price)
		{
			var movieTitleField = driver.FindElement(UIMap.MovieCreatePage.PriceField);
			movieTitleField.SendKeys(price);
			return this;
		}

		public MovieIndexPage Save()
		{
			var titleField = TitleField();
			titleField.Submit();
			driver.WaitUntilStale(titleField, 5);
			return new MovieIndexPage(driver);
		}
	}
}
