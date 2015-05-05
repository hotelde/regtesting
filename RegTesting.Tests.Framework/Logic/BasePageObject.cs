using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Logic
{
	abstract public class BasePageObject
	{
		private readonly object _lock = new object();
		protected readonly IWebDriver _driver;
		private readonly AsyncWebDriverCalls _asyncCalls;

		public abstract IDictionary<string, object> DefaultPageSettings { get; }
		public IDictionary<string, object> CurrentPageSettings { get; set; }

		protected Actions Actions
		{
			get
			{
				return new Actions(_driver);
			}
		}

		public BasePageObject(IWebDriver driver)
		{
			_driver = driver;
			_asyncCalls = new AsyncWebDriverCalls();
		}




		public virtual void ApplySettings()
		{

		}

		public virtual string CreatePageUrl(params string[] urlParameters)
		{
			//Override this method when the pageObject is not available at the root of your BaseUrl.
			return String.Empty;
		}



		public void HandleAlert(bool accept)
		{
			_driver.HandleAlert(accept);
		}

		/// <summary>
		/// Waits for an element to get visible.
		/// </summary>
		/// <param name="element">the element</param>
		/// <param name="timeout">A optional custom timeoutInSeconds (how long should we wait?)</param>
		public void WaitForElementDisplayed(BasicPageElement element, TimeSpan? timeout = null)
		{
			TestLog.Add("WaitForElementDisplayed: " + element.By);
			_driver.WaitForElement(element.By, timeout: timeout);
		}

		[Obsolete]
		public IWebElement WaitAndFindElement(By findBy)
		{
			TestLog.Add("WaitAndFindElement: " + findBy);
			return _driver.WaitForElement(findBy);
		}


		public ReadOnlyCollection<IWebElement> WaitAndFindElements(By findBy)
		{
			TestLog.Add("WaitAndFindElements: " + findBy);
			return _driver.WaitForElements(findBy);
		}


		public void WaitForElementNotDisplayed(BasicPageElement element)
		{
			TestLog.Add("WaitForElementNotDisplayed: " + element.By);
			_driver.WaitForElement(element.By, Visibility.Hidden);
		}

		public void NavigateTo(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException("url");
			_driver.Navigate().GoToUrl(url);
		}



		public void WaitFor(BasicPageElement element, Visibility visibility = Visibility.Visible)
		{
			_driver.WaitForElement(element.By, visibility);
		}

		/// <summary>
		/// Get a element which is not defined in the pageobject, because it is dynamically generated some time later after the page has loaded.
		/// DO NOT USE FOR REGULAR OBJECTS THAT COULD BE DEFINED IN THE PAGEOBJECT!
		/// </summary>
		/// <typeparam name="T">The type of the requested pageobject. E.g. <see cref="Link"/>.</typeparam>
		/// <param name="findBy">The locator object.</param>
		/// <param name="waitModel">The wait-model for the requested dynamic element.</param>
		/// <param name="clickBehaviour">The ClickBehaviour.</param>
		/// <returns>T</returns>
		public T GetDynamicElement<T>(By findBy, WaitModel waitModel = null, ClickBehaviours clickBehaviour = ClickBehaviours.Default) where T : BasicPageElement
		{
			return (T)PageObjectFactory.CreateElementObject(typeof(T), _driver, findBy, waitModel ?? new WaitModel(), this, clickBehaviour);
		}

		/// <summary>
		/// Change to a specific Frame
		/// </summary>
		/// <param name="frameName">The Frame name</param>
		public void ChangeFrame(string frameName)
		{
			if (string.IsNullOrWhiteSpace(frameName))
				throw new ArgumentNullException(frameName);

			_driver.SwitchTo().Frame(frameName);
		}

		/// <summary>
		/// Selects either the first frame on the page or the main document when a page contains iFrames.
		/// </summary>
		public void SwitchToDefaultContent()
		{
			_driver.SwitchTo().DefaultContent();
		}

		/// <summary>
		/// Let selenium switch to a tab beginning with the string in url
		/// </summary>
		public void SwitchToTab()
		{

			string pageObjectUrl = CreatePageUrl();

			if(string.IsNullOrWhiteSpace(pageObjectUrl))
				throw new ArgumentException("You´re trying to switch the WebDriverActions to a tab with a PageObject that doesn´t have a non empty result for CreatePageUrl. " +
				                            "To fix this you must override the CreatePageUrl method in the PageObject class. Called without parameters, it should return the relative pageurl.");

			_driver.SwitchToTab(pageObjectUrl);
		}

		/// <summary>
		/// Switching to a tab which coontains the Url-Sequence.
		/// </summary>
		/// <param name="urlSequence">String which should be contained in the target tab url.</param>
		public void SwitchToTab(string urlSequence)
		{
			if (string.IsNullOrWhiteSpace(urlSequence))
				throw new ArgumentNullException("urlSequence");
			_driver.SwitchToTab(urlSequence);
		}

		/// <summary>
		/// Close the tab which has the focus of the Webdriver instance 
		/// </summary>
		public void CloseCurrentTab()
		{
			TestLog.Add("Closing Current Tab.");
			_driver.Close();
		}

		public string GetCurrentUrl()
		{
			
			Task<string> task = _asyncCalls.GetCurrentUrlTask(_driver);
			task.Wait();
			string currentUrl = task.Result;
			TestLog.Add("Get current Url. " + currentUrl);
			return currentUrl;
		}

		public void Redirect()
		{
			
			string currentUrl = GetCurrentUrl();
			TestLog.Add("Redirecting to: " + currentUrl);
			_driver.Navigate().GoToUrl(currentUrl);
		}

	}
}
