using OpenQA.Selenium;
using RegTesting.Tests.Core;

namespace RegTesting.Tests.Framework.Logic.Init
{

	/// <summary>
	/// The interface for the WebDriverFactories
	/// </summary>
	public interface IWebDriverFactory
	{
		/// <summary>
		/// Get a WebDriver
		/// </summary>
		/// <param name="browser">the Browser to test on</param>
		/// <returns>a IWebDriver</returns>
		IWebDriver GetWebDriver(Browser browser);
	}
}
