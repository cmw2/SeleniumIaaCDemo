/* This Sample Code is provided for the purpose of illustration only 
 * and is not intended to be used in a production environment.   
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesCore.Tests.Extensions;
using MoviesCore.Tests.PageObjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace MoviesCore.Tests
{
    [TestClass]
	public class UIUnitTests
	{
		static string BaseTestUrl;
		static IWebDriver Driver;
		static TestContext testContext;

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			testContext = context;
			BaseTestUrl = Environment.GetEnvironmentVariable("BaseTestUrl");
			if (String.IsNullOrEmpty(BaseTestUrl))
			{
				BaseTestUrl = "http://localhost:9317"; // TODO: Get from configuration
			}

			if (Environment.GetEnvironmentVariable("AGENT_NAME") == "Hosted Agent")
			{
				// Running in hosted build agent.  Use the already installed driver so it matches version of browser installed.
				Driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"));
			}
			else
			{
				Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
				//return new ChromeDriver(); // if not in .net core
			}
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
			Driver.Navigate().GoToUrl(BaseTestUrl);
		}

		[TestCategory("UITest")]
		[TestMethod]
		public void ClickAboutLink()
		{
			IWebElement aboutLink = Driver.FindElement(By.LinkText("About"));
			aboutLink.Click();
			TakeScreenshot("AfterAbout");
			Assert.IsTrue(Driver.Title.StartsWith("about", StringComparison.OrdinalIgnoreCase));
		}

		[TestCategory("UITest")]
		[TestMethod]
		public void ClickContactLink()
		{
			IWebElement contactLink = Driver.FindElement(By.LinkText("Contact"));
			contactLink.Click();
			
			Assert.IsTrue(Driver.Title.StartsWith("contact", StringComparison.OrdinalIgnoreCase));
		}

		[TestCategory("UITest")]
		[TestMethod]
		public void AddMovie()
		{
			// Arrange
			Driver.Navigate().GoToUrl(BaseTestUrl + "/Movies/");

			// Act
			IWebElement createLink = Driver.FindElement(By.LinkText("Create New"));
			createLink.Click();
			var movieTitleField = Driver.FindElement(By.Id("Movie_Title"));
			string newMovieTitle = $"Movie-{DateTime.Now.Ticks}";
			movieTitleField.SendKeys(newMovieTitle);
			var releaseDateField = Driver.FindElement(By.Id("Movie_ReleaseDate"));
			releaseDateField.SendKeys("02022012\t06:00A");
			var genreField = Driver.FindElement(By.Id("Movie_Genre"));
			genreField.SendKeys("Testing");
			var priceField = Driver.FindElement(By.Id("Movie_Price"));
			priceField.SendKeys("9.99");
			priceField.Submit();
			Driver.WaitUntilStale(priceField, 5);

			// Assert
			Assert.IsTrue(Driver.Title.StartsWith("Index", StringComparison.OrdinalIgnoreCase));
			var newMovieEntry = Driver.FindElement(By.XPath($"//table/tbody/tr/td[contains(text(), '{newMovieTitle}')]"));
			Assert.AreEqual(newMovieTitle, newMovieEntry.Text);
		}


		[TestCategory("UITest")]
		[TestMethod]
		public void AddMovieUIMap()
		{
			// Arrange
			Driver.Navigate().GoToUrl(BaseTestUrl + "/Movies/");

			// Act
			IWebElement createLink = Driver.FindElement(UIMap.MovieIndexPage.CreateNewLink);
			createLink.Click();
			var movieTitleField = Driver.WaitUntilExists(UIMap.MovieCreatePage.TitleField, 5);
			string newMovieTitle = $"Movie-{DateTime.Now.Ticks}";
			movieTitleField.SendKeys(newMovieTitle);
			var releaseDateField = Driver.FindElement(UIMap.MovieCreatePage.ReleaseDateField);
			releaseDateField.SendKeys("02022012\t06:00A");
			var genreField = Driver.FindElement(UIMap.MovieCreatePage.GenreField);
			genreField.SendKeys("Testing");
			var priceField = Driver.FindElement(UIMap.MovieCreatePage.PriceField);
			priceField.SendKeys("9.99");
			priceField.Submit();
			Driver.WaitUntilStale(priceField, 5);

			// Assert
			Assert.IsTrue(Driver.Title.StartsWith("Index", StringComparison.OrdinalIgnoreCase));
			var newMovieEntry = Driver.FindElement(By.XPath($"//table/tbody/tr/td[contains(text(), '{newMovieTitle}')]"));
			Assert.AreEqual(newMovieTitle, newMovieEntry.Text);
		}

		[TestCategory("UITest")]
		[TestMethod]
		public void AddMoviePOM()
		{
			// Arrange
			var indexPage = new MovieIndexPage(Driver);

			// Act
			indexPage.OpenPage();
			var createpage = indexPage.ClickCreateNew();
			string newMovieTitle = $"Movie-{DateTime.Now.Ticks}";
			var newIndexPage = createpage
				.SetTitle(newMovieTitle)
				.SetReleaseDate("02022012\t06:00A")
				.SetGenre("Testing")
				.SetPrice("9.99")
				.Save();

			// Assert
			Assert.IsTrue(newIndexPage.IsOnPage());
			var newMovieEntry = newIndexPage.FindMovieByTitle(newMovieTitle);
			Assert.AreEqual(newMovieTitle, newMovieEntry.Text);
		}

		private static void TakeScreenshot(string name)
		{
			if (UIUnitTests.Driver is ITakesScreenshot screenshotDriver)
			{
				string fileName = $"{name}.png";
				screenshotDriver
					.GetScreenshot()
					.SaveAsFile(
						fileName,
						ScreenshotImageFormat.Png);
				UIUnitTests.testContext.AddResultFile(fileName);
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			try
			{
				if (Driver != null)
				{
					Driver.Dispose();
				}
			}
			catch (Exception)
			{
				// ignore
			}
		}
	}
}
