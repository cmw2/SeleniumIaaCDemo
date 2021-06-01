/* This Sample Code is provided for the purpose of illustration only 
 * and is not intended to be used in a production environment.   
 */

using OpenQA.Selenium;

namespace MoviesCore.Tests
{
    internal static class UIMap
    {
		internal static class MovieIndexPage
		{
			internal static By CreateNewLink = By.LinkText("Create New");
		}

		internal static class MovieCreatePage
		{
			internal static By TitleField = By.Id("Movie_Title");
			internal static By ReleaseDateField = By.Id("Movie_ReleaseDate");
			internal static By GenreField = By.Id("Movie_Genre");
			internal static By PriceField = By.Id("Movie_Price");
			
		}
    }
}
