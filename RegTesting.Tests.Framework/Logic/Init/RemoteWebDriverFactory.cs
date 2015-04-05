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
		/// <param name="objBrowser">the Browser to test on</param>
		/// <returns>a IWebDriver</returns>
		IWebDriver IWebDriverFactory.GetWebDriver(Browser objBrowser)
		{
			DesiredCapabilities objCapabilities;

			//What browser to test on?
			switch (objBrowser.Browserstring.ToLowerInvariant())
			{
				case "firefox":
					objCapabilities = DesiredCapabilities.Firefox();
					objCapabilities.SetCapability("singleWindow", true);
					objCapabilities.SetCapability("handlesAlerts", true);
					break;
				case "chrome":
					objCapabilities = DesiredCapabilities.Chrome();
					objCapabilities.SetCapability("singleWindow", true);
					objCapabilities.SetCapability("handlesAlerts", true);
					objCapabilities.SetCapability("--disable-hang-monitor", true);
					break;
				case "internet explorer":
					objCapabilities = DesiredCapabilities.InternetExplorer();
					objCapabilities.SetCapability("singleWindow", true);
					objCapabilities.SetCapability("ie.ensureCleanSession", true);
					objCapabilities.SetCapability("enableElementCacheCleanup", false);
					objCapabilities.SetCapability("ignoreProtectedModeSettings", true);
					objCapabilities.SetCapability("enablePersistentHover", false);
					break;
				case "safari":
					objCapabilities = DesiredCapabilities.Safari();
					objCapabilities.SetCapability("singleWindow", true);
					objCapabilities.SetCapability("handlesAlerts", true);
					break;
				case "android":
					objCapabilities = DesiredCapabilities.Android();
					break;
				case "ipad":
					objCapabilities = DesiredCapabilities.IPad();
					break;
				case "iphone":
					objCapabilities = DesiredCapabilities.IPhone();
					break;
				case "opera":
					objCapabilities = DesiredCapabilities.Opera();
					break;
				case "htmlunit":
					objCapabilities = DesiredCapabilities.HtmlUnit();
					break;
				case "htmlunitjs":
					objCapabilities = DesiredCapabilities.HtmlUnitWithJavaScript();
					break;
				case "phantomjs":
					objCapabilities = DesiredCapabilities.PhantomJS();
					break;
				default:
					objCapabilities = new DesiredCapabilities();
					objCapabilities.SetCapability(CapabilityType.BrowserName, objBrowser.Browserstring);
					break;
			}

			//If version is set, use this version.
			if (!String.IsNullOrEmpty(objBrowser.Versionsstring))
				objCapabilities.SetCapability(CapabilityType.Version, objBrowser.Versionsstring);
			
			return new RemoteWebDriver(new Uri("http://SELENIUMHUBADDRESS:4444/wd/hub"),objCapabilities, new TimeSpan(0,0,10));

		}
	}

}
