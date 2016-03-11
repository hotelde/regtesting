using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using RegTesting.Tests.Core;

namespace RegTesting.Tests.Framework.Logic.Init
{
	/// <summary>
	/// WebDriverFactory - Creates WebDriver
	/// </summary>
	public class LocalWebDriverFactory : MarshalByRefObject, IWebDriverFactory
	{
		/// <summary>
		/// Get a RemoteWebDriver
		/// </summary>
		/// <param name="browser">the Browser to test on</param>
		/// <param name="languageCode">The language that the browser should accept.</param>
		/// <returns>a IWebDriver</returns>
		IWebDriver IWebDriverFactory.GetWebDriver(Browser browser, string languageCode)
		{
			//What browser to test on?s
			IWebDriver webDriver;
			switch (browser.Browserstring.ToLowerInvariant())
			{
				case "firefox":
					var firefoxProfile = new FirefoxProfile();
					firefoxProfile.SetPreference("intl.accept_languages", languageCode);
					webDriver = new FirefoxDriver(firefoxProfile);
					break;
				case "chrome":
					ChromeOptions chromeOptions = new ChromeOptions();
					chromeOptions.AddArguments("--test-type", "--disable-hang-monitor", "--new-window", "--no-sandbox", "--lang=" + languageCode);
					webDriver = new ChromeDriver(chromeOptions);
					break;
				case "internet explorer":
					webDriver = new InternetExplorerDriver(new InternetExplorerOptions { BrowserCommandLineArguments = "singleWindow=true", IntroduceInstabilityByIgnoringProtectedModeSettings = true, EnsureCleanSession = true, EnablePersistentHover = false });
					break;
				case "phantomjs":
					PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
					service.IgnoreSslErrors = true;
					service.LoadImages = false;
					service.ProxyType = "none";
					webDriver = new PhantomJSDriver(service);
					break;
				default:
					throw new NotSupportedException("Not supported browser");
			}

			return webDriver;
		}
	}

}
