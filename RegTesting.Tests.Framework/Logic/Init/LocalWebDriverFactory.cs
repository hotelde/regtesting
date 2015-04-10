using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
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
		/// <returns>a IWebDriver</returns>
		IWebDriver IWebDriverFactory.GetWebDriver(Browser browser)
		{
			DesiredCapabilities desiredCapabilities;
			//What browser to test on?s
			IWebDriver webDriver;
			switch (browser.Browserstring.ToLowerInvariant())
			{
				case "firefox":
					desiredCapabilities = DesiredCapabilities.Firefox();
					desiredCapabilities.SetCapability("singleWindow", true);
					desiredCapabilities.SetCapability("handlesAlerts", true);
					webDriver = new FirefoxDriver(desiredCapabilities);
					break;
				case "chrome":
					ChromeOptions chromeOptions = new ChromeOptions();
					chromeOptions.AddArguments("-unexpectedAlertBehaviour=accept", "--test-type", "--disable-hang-monitor", "--new-window", "--no-sandbox");
					webDriver = new ChromeDriver(chromeOptions);
					break;
				case "internet explorer":
					webDriver = new InternetExplorerDriver(new InternetExplorerOptions { BrowserCommandLineArguments = "singleWindow=true", IntroduceInstabilityByIgnoringProtectedModeSettings = true, EnsureCleanSession = true, EnablePersistentHover = false, UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Accept });
					break;
				case "phantomjs":
					webDriver = new PhantomJSDriver(new PhantomJSOptions() {});
					break;
				default:
					throw new NotSupportedException("Not supported browser");
			}

			return webDriver;
		}
	}

}
