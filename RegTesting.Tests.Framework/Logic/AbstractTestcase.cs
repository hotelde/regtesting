using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using RegTesting.Tests.Core;
using RegTesting.Tests.Framework.Attributes;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic.Init;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// A abstract Testcase
	/// </summary>
	public abstract class AbstractTestcase : MarshalByRefObject, ITestable, IComparable, IDisposable
	{

		// Track whether Dispose has been called.
		private bool _disposedCalled;

		/// <summary>
		/// The Baseurl to test on
		/// </summary>
		protected string Baseurl;

		/// <summary>
		/// Languagecode for Test
		/// </summary>
		protected string Languagecode;

		/// <summary>
		/// CultureInfo for Test
		/// </summary>
		protected CultureInfo CultureInfo;

		/// <summary>
		/// The Webdriver to test on
		/// </summary>
		protected IWebDriver WebDriver;

		private IWebDriver _webDriver;


		#region ITest Members

		/// <summary>
		/// The id of the testcase, set on executiontime.
		/// </summary>
		public int ID { get; set; }

		private Attribute[] Attributs
		{
			get { return _attributs ?? (_attributs = Attribute.GetCustomAttributes(GetType())); }
		}

		private Attribute[] _attributs;

		/// <summary>
		/// Get and save a screenshot.
		/// </summary>
		/// <returns>the filename (not the path) of the screenshot</returns>
		string ITestable.SaveScreenshot(string folderPath)
		{

			if (_webDriver == null) return "";
			try
			{
				Screenshot screenshot = ((ITakesScreenshot)_webDriver).GetScreenshot();
				if (screenshot != null)
				{
					return screenshot.AsBase64EncodedString;
				}
				return String.Empty;
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// Returns the name of the testcase.
		/// </summary>
		/// <returns>A string with the name of the testcase.</returns>
		public abstract String GetName();

		/// <summary>
		/// Initialize the test with a driver and a baseURL. Then start this specific test with Test().
		/// </summary>
		/// <param name="webDriver">Webdriverobject to test on. In most cases some RemoteWebdriver.</param>
		/// <param name="baseurl">The baseURL to test on<example>gamma</example></param>
		/// <param name="languagecode">Languagecode for Test<example>DE</example></param>
		private void SetupTest(IWebDriver webDriver, string baseurl, string languagecode)
		{
			WebDriver = webDriver;
			Baseurl = "http://" + baseurl;
			Languagecode = languagecode;
			CultureInfo = new CultureInfo(languagecode);
			Thread.CurrentThread.CurrentUICulture = CultureInfo;
			Thread.CurrentThread.CurrentCulture = CultureInfo;
			AdaptResolution();
			webDriver.Manage().Cookies.DeleteAllCookies(); //Delete only cookies for current domain
		}

		/// <summary>
		/// Adapt the resolution for the test
		/// </summary>
		private void AdaptResolution()
		{
			IEnumerable<Resolution> resolutions = GetSupportedResolutions();

			//Currently we don't test all resolutions like all language or browser combinations.
			//Thus this implies an
			//TODO: Add resolution as testwide parameter like language or browser.
			if (resolutions!=null && resolutions.Contains(Resolution.Size320x480))
			{
				WebDriver.Manage().Window.Size = new Size(320, 480);
			}
			else
			{
				//If we don't have a specific resolution -> Maximize the window.
				WebDriver.Manage().Window.Maximize();				
			}

			
		}




		/// <summary>
		/// Setup our test
		/// </summary>
		/// <param name="webDriverInitStrategy">the webDriver init strategy</param>
		/// <param name="browser">the template to test on</param>
		/// <param name="baseURL">the baseURL of our testsystem</param>
		/// <param name="languageCode">the languageCode to test on</param>
		/// <param name="timeOut">A optional timeout</param>
		void ITestable.SetupTest(WebDriverInitStrategy webDriverInitStrategy,  Browser browser, string baseURL, string languageCode, int timeOut)
		{
			RunBeforeTestStartMethods();

			TestStatusManager.IsCanceled = false;
			/*if (timeOut != 0)
				Settings.Default.TestTimeout = timeOut;*/


			IWebDriverFactory webDriverFactory;
			switch (webDriverInitStrategy)
			{
				case WebDriverInitStrategy.SeleniumGrid:
					webDriverFactory = new RemoteWebDriverFactory();
					break;
				case WebDriverInitStrategy.SeleniumLocal:
					webDriverFactory = new LocalWebDriverFactory();
					break;
				default:
					webDriverFactory = new LocalWebDriverFactory();
					break;
			}

			_webDriver = webDriverFactory.GetWebDriver(browser, languageCode);
			SetupTest(_webDriver, baseURL, languageCode);
		}

		/// <summary>
		/// Get the testing log.
		/// </summary>
		/// <returns>The log entries as list of string</returns>
		List<string> ITestable.GetLogLastTime()
		{
			return TestLog.GetAndDelete();
		}

		/// <summary>
		/// Get the testing log.
		/// </summary>
		/// <returns>The log entries as list of string</returns>
		List<string> ITestable.GetLog()
		{
			return TestLog.Get();
		}

		/// <summary>
		/// Cancel a test
		/// </summary>
		void ITestable.CancelTest()
		{
			TestStatusManager.IsCanceled = true;
		}

		string[] ITestable.GetSupportedLanguages()
		{
			return Attributs.OfType<SupportedLanguages>().Select(attr => attr.Languages).SingleOrDefault();
		}

		string[] ITestable.GetSupportedBrowsers()
		{
			return Attributs.OfType<SupportedBrowsers>().Select(attr => attr.Browsers).SingleOrDefault();
		}


		private IEnumerable<Resolution> GetSupportedResolutions()
		{
			return Attributs.OfType<SupportedResolutions>().Select(attr => attr.Resolutions).SingleOrDefault();
		}

		/// <summary>
		/// Check if there is a server error
		/// </summary>
		/// <returns>a string with the error name</returns>
		ServerErrorModel ITestable.CheckForServerError()
		{
			if (WebDriver == null)
				return null;

			try
			{
				string pagesource = WebDriver.PageSource;
				if (pagesource.Contains("<title>Service Unavailable</title>"))
				{
					return new ServerErrorModel{Message= "Service Unavailable"};
				}
				if (pagesource.Contains("<span><H1>Server Error in "))
				{
					return new ServerErrorModel {Message = "Server Error"};
				}
			}
			catch
			{ }
			return null;
		}


		/// <summary>
		/// Our mainfunction of the Testclasses, here comes the logic of the testcases.
		/// </summary>
		public abstract void Test();


		/// <summary>
		/// The cleaningfunction. Here we dispose things and close our driverconnections etc.
		/// </summary>
		/// <returns>Returning a empty string, or an errormessage.</returns>
		void ITestable.TeardownTest()
		{
			if(WebDriver!=null) WebDriver.Quit();
			TestLog.Add("Test ended at " + DateTime.Now);
		}

		#endregion

		/// <summary>
		/// Compares a Testcase to another testcase (compares by name)
		/// </summary>
		/// <param name="obj">other testcase to compare with this testcase</param>
		/// <returns>difference beteween the two testcases</returns>
		public int CompareTo(object obj)
		{
			if (!(obj is AbstractTestcase))
			{
				throw new ArgumentException("Compare: Object is not an Testcase");
			}
			AbstractTestcase otherAbstractTestcase = (AbstractTestcase)obj;
			return (StringComparer.InvariantCultureIgnoreCase).Compare(GetName(), otherAbstractTestcase.GetName());
		}

		/// <summary>
		/// Override Lifetime to avoid RemotingExceptions
		/// </summary>
		/// <returns>null</returns>
		public override Object InitializeLifetimeService()
		{
			return null;
		}

		/// <summary>
		/// Implement IDisposable.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the
		/// runtime from inside the finalizer and you should not reference
		/// other objects. Only unmanaged resources can be disposed.
		/// </summary>
		/// <param name="disposing">disposing boolean</param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposedCalled)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					if (WebDriver!=null) WebDriver.Dispose();
				}

				// Note disposing has been done.
				_disposedCalled = true;

			}
		}

		/// <summary>
		/// Search for classes that implement IRunBeforeTestStart and execute the run method
		/// </summary>
		private void RunBeforeTestStartMethods()
		{
			Type type = typeof(IRunBeforeTestStart);
			IEnumerable<IRunBeforeTestStart> runBeforeTestStartInstances = AppDomain.CurrentDomain.GetAssemblies()
																			.SelectMany(s => s.GetTypes())
																			.Where(p => type.IsAssignableFrom(p) && !p.IsInterface)
																			.Select(t => Activator.CreateInstance(t) as IRunBeforeTestStart);


			foreach (IRunBeforeTestStart runBeforeTestStart in runBeforeTestStartInstances)
			{
				runBeforeTestStart.Run();
			}
		}




	}
}