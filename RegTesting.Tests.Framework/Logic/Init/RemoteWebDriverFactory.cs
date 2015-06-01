using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using RegTesting.Tests.Core;

namespace RegTesting.Tests.Framework.Logic.Init
{
	/// <summary>
	/// WebDriverFactory - Creates WebDriver
	/// </summary>
	public class RemoteWebDriverFactory : MarshalByRefObject, IWebDriverFactory
	{
		/// <summary>
		/// Get a RemoteWebDriver
		/// </summary>
		/// <param name="browser">the Browser to test on</param>
		/// <param name="languageCode">The language that the browser should accept.</param>
		/// <returns>a IWebDriver</returns>
		IWebDriver IWebDriverFactory.GetWebDriver(Browser browser, string languageCode)
		{
			DesiredCapabilities desiredCapabilities;

			//What browser to test on?
			switch (browser.Browserstring.ToLowerInvariant())
			{
				case "firefox":
					desiredCapabilities = DesiredCapabilities.Firefox();
					desiredCapabilities.SetCapability("singleWindow", true);
					desiredCapabilities.SetCapability("handlesAlerts", true);
					break;
				case "chrome":
					desiredCapabilities = DesiredCapabilities.Chrome();
					desiredCapabilities.SetCapability("singleWindow", true);
					desiredCapabilities.SetCapability("handlesAlerts", true);
					desiredCapabilities.SetCapability("--disable-hang-monitor", true);
					break;
				case "internet explorer":
					desiredCapabilities = DesiredCapabilities.InternetExplorer();
					desiredCapabilities.SetCapability("singleWindow", true);
					desiredCapabilities.SetCapability("ie.ensureCleanSession", true);
					desiredCapabilities.SetCapability("enableElementCacheCleanup", false);
					desiredCapabilities.SetCapability("ignoreProtectedModeSettings", true);
					desiredCapabilities.SetCapability("enablePersistentHover", false);
					break;
				case "safari":
					desiredCapabilities = DesiredCapabilities.Safari();
					desiredCapabilities.SetCapability("singleWindow", true);
					desiredCapabilities.SetCapability("handlesAlerts", true);
					break;
				case "android":
					desiredCapabilities = DesiredCapabilities.Android();
					break;
				case "ipad":
					desiredCapabilities = DesiredCapabilities.IPad();
					break;
				case "iphone":
					desiredCapabilities = DesiredCapabilities.IPhone();
					break;
				case "opera":
					desiredCapabilities = DesiredCapabilities.Opera();
					break;
				case "htmlunit":
					desiredCapabilities = DesiredCapabilities.HtmlUnit();
					break;
				case "htmlunitjs":
					desiredCapabilities = DesiredCapabilities.HtmlUnitWithJavaScript();
					break;
				case "phantomjs":
					desiredCapabilities = DesiredCapabilities.PhantomJS();
					break;
				default:
					desiredCapabilities = new DesiredCapabilities();
					desiredCapabilities.SetCapability(CapabilityType.BrowserName, browser.Browserstring);
					break;
			}

			//If version is set, use this version.
			if (!String.IsNullOrEmpty(browser.Versionsstring))
				desiredCapabilities.SetCapability(CapabilityType.Version, browser.Versionsstring);
			
			return new RemoteWebDriver(new Uri("http://SELENIUMHUBADDRESS:4444/wd/hub"),desiredCapabilities, new TimeSpan(0,0,10));

		}
	}

}
