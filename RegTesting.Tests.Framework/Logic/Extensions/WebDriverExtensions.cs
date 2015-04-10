using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic.PageSettings;
using RegTesting.Tests.Framework.Properties;

namespace RegTesting.Tests.Framework.Logic.Extensions
{
	/// <summary>
	/// A extensions class for the WebDriver. Here we have some more useful functions we use often on a WebDriver
	/// </summary>
	public static class WebDriverExtensions
	{
		/// <summary>
		/// Perfom a safe click on an element
		/// </summary>
		/// <param name="webDriver">the webdriver</param>
		/// <param name="pageElement">the webelement to click</param>
		public static void ClickElement(this IWebDriver webDriver, BasicPageElement pageElement)
		{
			Stopwatch startNew = Stopwatch.StartNew();
			Click(FindAndScrollToElement(pageElement.By, Visibility.Visible, pageElement.ParentPageObject.PageSettings), pageElement);
			startNew.Stop();
			TestLog.Add("Waited " + startNew.ElapsedMilliseconds + " milliseconds to scroll and click element '" + pageElement.By + "'.");
		}

		/// <summary>
		/// Perfom a safe click on an element
		/// </summary>
		/// <param name="webDriver">the webdriver</param>
		/// <param name="pageElement">the webelement to click</param>
		public static void ClickElementWithoutScrolling(this IWebDriver webDriver, BasicPageElement pageElement)
		{
			Stopwatch startNew = Stopwatch.StartNew();
			Click(Find(pageElement.By, Visibility.Visible), pageElement);
			startNew.Stop();
			TestLog.Add("Waited " + startNew.ElapsedMilliseconds + " milliseconds to click element '" + pageElement.By + "'.");
		}

		private static void Click(Func<IWebDriver, IWebElement> findFunction, BasicPageElement pageElement)
		{
			IWebElement element = null;
			TimeSpan timeout = new TimeSpan(0, 0, Settings.Default.TestTimeout);
			WebDriverWait wait = new WebDriverWait(pageElement.WebDriver, timeout);
			
			if (pageElement.ParentPageObject.PageSettings.PageUsesJquery && !pageElement.ParentPageObject.PageSettings.HasEndlessJQueryAnimation)
			{
				string errorMessage = null;
				try
				{
					Stopwatch stopwatch = Stopwatch.StartNew();
					wait.Timeout = new TimeSpan(0, 0, 0, 0, Settings.Default.WatiForAnimationsToComplete);
					wait.Until(driver => AllAnimationsFinished(driver, pageElement.ParentPageObject.PageSettings, out errorMessage));
					TestLog.Add("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for all anmiations to complete.");
				}
				catch (WebDriverTimeoutException e)
				{
					throw new TimeoutException(errorMessage + "  Waited " + Settings.Default.WatiForAnimationsToComplete + " milliseconds to complete animations. Inner-exception: ", e);
				}
			}
			
			try
			{
				wait.Timeout = timeout;
				element = wait.Until(findFunction);
			}
			catch (WebDriverTimeoutException e)
			{
				string timeoutMessage = BuildTimeoutMessage(pageElement.By, Visibility.Hidden, e.Message, Settings.Default.TestTimeout, element);
				throw new TimeoutException(timeoutMessage, e);
			}
			
			element.Click();
		}

		private static string GetAnimatedObjectMessage(IEnumerable<object> animatedObjects)
		{
			if (animatedObjects == null)
				return String.Empty;

			StringBuilder stringBuilder = new StringBuilder();
			
			foreach (IWebElement animatedObject in animatedObjects)
			{
				string id = animatedObject.GetAttribute("id");
				if (!string.IsNullOrEmpty(id))
				{
					stringBuilder.Append("Element with id: " + id);
				}
				else
				{
					stringBuilder.Append("A element with TagName: " + animatedObject.TagName);
				}
				stringBuilder.Append(", ");
			}
			
			return stringBuilder.ToString();
		}

		private static Func<IWebDriver, IWebElement> Find(By locator, Visibility visibilityFilter)
		{
			if (TestStatusManager.IsCanceled)
				throw new TaskCanceledException("Canceled test.");

			return d =>
			{
				try
				{
					IWebElement webElement = d.FindElement(locator);
					if (FilterForVisibility(visibilityFilter, webElement))
						return webElement;
					return null;
				}
				catch (StaleElementReferenceException)
				{
					//For the case, that the objectreference we have, is no longer valid
				}
				catch (InvalidOperationException)
				{
					//For older IE: Sometimes we get an 'Error determining if element is displayed' error, so we can go around this error.
				}
				catch (WebDriverException)
				{
					//For some cases, our node is not responding (OpenQA.Selenium.WebDriverException: No response from server for url ...)
				}

				return null;
			};
		} 
		private static Func<IWebDriver, IWebElement> FindAndScrollToElement(By locator, Visibility visibilityFilter, AbstractPageSettings pageSettings)
		{
			if (TestStatusManager.IsCanceled) 
				throw new TaskCanceledException("Canceled test.");

			return driver =>
			{
				IWebElement webElement = Find(locator, visibilityFilter).Invoke(driver);
				if (webElement != null)
				{
					ScrollIntoView(driver, webElement, pageSettings);
					TimeSpan timeout = new TimeSpan(0, 0, 3);
					WebDriverWait wait = new WebDriverWait(driver, timeout)
					{
						Message = "Element by " + locator + " was not present in the viewport after timeout."
					};
					wait.Until(d => ElementIsInViewPort(d, locator));
					return webElement;
				}

				return null;
			};
		} 
		/// <summary>
		///  Waits for an element to get visible and then returns the element.
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="byLocator">The Locator of the element</param>
		/// <param name="visibility">Wait until the element has this visibility.</param>
		/// <returns>A IWebElement if we found the searched element. Fails with an TimeoutException else.</returns>
		internal static IWebElement WaitForElement(this IWebDriver webDriver, By byLocator, Visibility visibility = Visibility.Visible)
		{
			return WaitForElementImpl(webDriver, byLocator, new TimeSpan(0, 0, Settings.Default.TestTimeout), visibility);
		}

		/// <summary>
		///  Waits for an element to get visible and then returns the element.
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="byLocator">The Locator of the element</param>
		/// <param name="timeout">A optional custom timeout (how long should we wait?)</param>
		/// <param name="visibility">Wait until the element has this visibility.</param>
		/// <returns>A IWebElement if we found the searched element. Fails with an TimeoutException else.</returns>
		public static IWebElement WaitForElement(this IWebDriver webDriver, By byLocator, TimeSpan? timeout, Visibility visibility = Visibility.Visible)
		{
			return WaitForElementImpl(webDriver, byLocator, timeout != null ? (TimeSpan)timeout : new TimeSpan(0, 0, Settings.Default.TestTimeout), visibility);
		}
		

		private static IWebElement WaitForElementImpl(IWebDriver webDriver, By locator, TimeSpan timeout, Visibility visibilityFilter)
		{
			IWebElement element = null;

			WebDriverWait wait = new WebDriverWait(new SystemClock(), webDriver, timeout, new TimeSpan(0,0,0,0,500));
			try
			{
				Stopwatch startNew = Stopwatch.StartNew();
				element = wait.Until(Find(locator, visibilityFilter));
				startNew.Stop();
				TestLog.Add("Waited " + startNew.ElapsedMilliseconds + " milliseconds to find element '" + locator + "'.");
			}
			catch (WebDriverTimeoutException e)
			{
				string timeoutMessage = BuildTimeoutMessage(locator, visibilityFilter, e.Message, Settings.Default.TestTimeout, element);
				throw new TimeoutException(timeoutMessage, e);
			}

			return element;
		}

		private static string BuildTimeoutMessage(By byLocator, Visibility visibility, string errorMessage, int timeoutInSeconds, IWebElement webElement)
		{
			StringBuilder messageBuilder = new StringBuilder();

			messageBuilder.AppendFormat("({0}s): ", timeoutInSeconds);

			if (errorMessage != null)
				messageBuilder.Append(errorMessage + " ");	
			if (webElement == null)
				messageBuilder.AppendFormat("Element still not " + visibility.ToString().ToLower() + " or was never present. Desired element: {0}", byLocator);
			else
				messageBuilder.AppendFormat("Element found {0}, but Element.Displayed={1}, Element.Enabled={2}" ,byLocator, webElement.Displayed, webElement.Enabled);
			
			string messageBuilt = messageBuilder.ToString();
			return messageBuilt;
		}

		private static void ScrollIntoView(IWebDriver webDriver, IWebElement webElement, AbstractPageSettings pageSettings)
		{
			if (webElement == null || !webElement.Displayed) 
				return;

			try
			{
				webDriver.ExecuteScript(pageSettings, "arguments[0].scrollIntoView(true);", webElement);
			}
			catch (StaleElementReferenceException)
			{
				//For the case, that the objectreference we have, is no longer valid
			}
			catch (InvalidOperationException)
			{
				//For older IE: Sometimes we get an 'Error determining if element is displayed' error, so we can go around this error.
			}
			catch (WebDriverException)
			{
				//For some cases, our node is not responding (OpenQA.Selenium.WebDriverException: No response from server for url ...)
			}
		}

		private static bool ElementIsInViewPort(IWebDriver webDriver, By byLocator)
		{
			IWebElement element = Find(byLocator, Visibility.Visible).Invoke(webDriver);
			if (element == null)
				return false;

			if (element.Location.Y >= 0)
				return true;

			return false;
		}
		
		private static bool FilterForVisibility(Visibility visibilityFilter, IWebElement element)
		{
			return visibilityFilter == Visibility.Any ||
				(visibilityFilter == Visibility.Visible && element.Displayed && element.Enabled) ||
				(visibilityFilter == Visibility.Hidden && !element.Displayed);
		}


		/// <summary>
		///  Waits for elements to get visible and then returns the elements.
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="byLocator">The Locator of the element</param>
		/// <param name="expectedMinimumCountOfElements">The minimum count of elements to expect</param>
		/// <returns>A IWebElement if we found the searched element. Fails with an TimeoutException else.</returns>
		public static ReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver webDriver, By byLocator, int expectedMinimumCountOfElements = 1)
		{
			return WaitForElementsImpl(webDriver, byLocator, expectedMinimumCountOfElements, null, -1);
		}


		/// <summary>
		///  Waits for elements to get visible and then returns the elements.
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="byLocator">The Locator of the element</param>
		/// <param name="expectedMinimumCountOfElements">The minimum count of elements to expect</param>
		/// <param name="strErrorMessage">A Optional alternative message if we run in a timeout.</param>
		/// <param name="intTimeout">A optional custom timeout (how long should we wait?)</param>
		/// <returns>A IWebElement if we found the searched element. Fails with an TimeoutException else.</returns>
		public static ReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver webDriver, By byLocator, int expectedMinimumCountOfElements, string strErrorMessage, int intTimeout = -1)
		{
			return WaitForElementsImpl(webDriver, byLocator, expectedMinimumCountOfElements, strErrorMessage, intTimeout);
		}

		/// <summary>
		///  Waits for elements to get visible and then returns the elements.
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="byLocator">The Locator of the element</param>
		/// <param name="expectedMinimumCountOfElements">The minimum count of elements to expect</param>
		/// <param name="intTimeout">A optional custom timeout (how long should we wait?)</param>
		/// <returns>A IWebElement if we found the searched element. Fails with an TimeoutException else.</returns>
		public static ReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver webDriver, By byLocator, int expectedMinimumCountOfElements, int intTimeout)
		{
			return WaitForElementsImpl(webDriver, byLocator, expectedMinimumCountOfElements, null, intTimeout);
		}

		private static ReadOnlyCollection<IWebElement> WaitForElementsImpl(IWebDriver webDriver, By byLocator, int expectedMinimumCountOfElements, string errorMessage, int timeoutInSeconds)
		{
			if (timeoutInSeconds == -1) timeoutInSeconds = Settings.Default.TestTimeout;

			for (int intSecond = 0;; intSecond++)
			{
				if (intSecond == timeoutInSeconds)
					throw new TimeoutException(errorMessage ?? string.Format("({0}s): Still not {1} elements located by: {2}", timeoutInSeconds, expectedMinimumCountOfElements, byLocator));
				try
				{
					if (TestStatusManager.IsCanceled) 
						throw new TaskCanceledException("Canceled test.");

					ReadOnlyCollection<IWebElement> objElements = webDriver.FindElements(byLocator);
					if (objElements.Count >= expectedMinimumCountOfElements)
						return objElements;
				}
				catch (TaskCanceledException)
				{
					throw;
				}
				catch (Exception)
				{
				}
				Thread.Sleep(1000);
			}
		}


		/// <summary>
		/// Returns if an element is present. If no element is found the function returns false, there is no timeout and retry.
		/// </summary>
		/// <param name="objDriver">Extension hook</param>
		/// <param name="objBy">The Locator of the element</param>
		/// <returns>A boolean if the element is present</returns>
		public static bool IsElementPresent(this IWebDriver objDriver, By objBy)
		{
			try
			{
				if (TestStatusManager.IsCanceled) throw new TaskCanceledException("Canceled test.");
				objDriver.FindElement(objBy);
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}

		/// <summary>
		/// Returns if an element is present and display. If no element is found the function returns false, there is no retry.
		/// </summary>
		/// <param name="driver">Extension hook</param>
		/// <param name="locator">The Locator of the element</param>
		/// <param name="timeoutInSeconds">An optional timeout.</param>
		/// <returns>A boolean if the element is present</returns>
		public static bool IsElementDisplayed(this IWebDriver driver, By locator, int timeoutInSeconds = 3)
		{
			try
			{
				WaitForElement(driver, locator, new TimeSpan(0,0,timeoutInSeconds));
			}
			catch (TimeoutException)
			{
				return false;
			}
			try
			{
				if (TestStatusManager.IsCanceled) throw new TaskCanceledException("Canceled test.");
				IWebElement objElement = driver.FindElement(locator);
				return objElement.Displayed;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}

		/// <summary>
		/// Handle a alert and click on accept (if accept=true) or dismiss (if accept=false)
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="accept">If we should accept or dismiss the alert.</param>
		public static void HandleAlert(this IWebDriver webDriver, bool accept)
		{
			HandleAlertImpl(webDriver, accept, null, -1);
		}


		/// <summary>
		/// Handle a alert and click on accept (if accept=true) or dismiss (if accept=false)
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="accept">If we should accept or dismiss the alert.</param>
		/// <param name="errorMessage">A Optional alternative message if we run in a timeout.</param>
		/// <param name="timeoutInSeconds">A optional custom timeout (how long should we wait?)</param>
		public static void HandleAlert(this IWebDriver webDriver, bool accept, string errorMessage, int timeoutInSeconds = -1)
		{
			HandleAlertImpl(webDriver, accept, errorMessage, timeoutInSeconds);
		}

		/// <summary>
		/// Handle a alert and click on accept (if accept=true) or dismiss (if accept=false)
		/// </summary>
		/// <param name="webDriver">Extension hook</param>
		/// <param name="accept">If we should accept or dismiss the alert.</param>
		/// <param name="timeoutInSeconds">A optional custom timeout (how long should we wait?)</param>
		public static void HandleAlert(this IWebDriver webDriver, bool accept, int timeoutInSeconds)
		{
			HandleAlertImpl(webDriver, accept, null, timeoutInSeconds);
		}

		private static void HandleAlertImpl(IWebDriver webDriver, bool accept, string errorMessage, int timeoutInSeconds)
		{
			if (timeoutInSeconds == -1) timeoutInSeconds = Settings.Default.TestTimeout;

			for (int second = 0;; second++)
			{
				Thread.Sleep(1000);

				if (second == timeoutInSeconds) throw new NoAlertPresentException(errorMessage ?? ("Timeout: No alert."));
				try
				{
					if (TestStatusManager.IsCanceled) throw new TaskCanceledException("Canceled test.");
					if (accept) webDriver.SwitchTo().Alert().Accept();
					else webDriver.SwitchTo().Alert().Dismiss();
					break;
				}
				catch (NoAlertPresentException)
				{
					//No Alert Present, wait and try again until timeout
				}
			}
		}


		public static void SwitchToTab(this IWebDriver webDriver, string urlSegment)
		{
			if (string.IsNullOrWhiteSpace(urlSegment))
				throw new ArgumentNullException();
			AsyncWebDriverCalls asyncCalls = new AsyncWebDriverCalls();
			Task<ReadOnlyCollection<string>> getWindowHandlesTask = asyncCalls.GetWindowHandlesAsync(webDriver);
			getWindowHandlesTask.Wait();
			ReadOnlyCollection<string> windowHandles = getWindowHandlesTask.Result;

			if (windowHandles.Count <= 1)
			{
				TestLog.Add("Only one Tab recognized. Cancelling tabswitch => unnecessary.");
				return;
			} 
				

			TestLog.Add("Try switching to tab with url containing '" + urlSegment + "'. tabcount: " + windowHandles.Count);
			bool tabFound = false;
			int currentTabWindowIndex = windowHandles.IndexOf(webDriver.CurrentWindowHandle);

			for (int index = currentTabWindowIndex; index <= windowHandles.Count; index++)
			{
				int realIndex = (index + currentTabWindowIndex + 1)%windowHandles.Count;

				if (DoTabSwitchWithIndex(realIndex, webDriver, urlSegment, asyncCalls, windowHandles))
				{
					tabFound = true;
					break;
				}
			}
			if (tabFound) 
				return;

			TestLog.Add("No window with url containing: " + urlSegment);
			TestLog.Add("Current WindowHandle-Url: " + webDriver.Url);
			throw new Exception("No window with url-segment: '" + urlSegment + "'");
		}

		private static bool DoTabSwitchWithIndex(int index, IWebDriver webDriver, string urlSegment, AsyncWebDriverCalls asyncCalls,
			ReadOnlyCollection<string> windowHandles)
		{
			string windowHandle = windowHandles[index];
			Task<string> getCurrentUrlTask = asyncCalls.GetCurrentUrlTask(webDriver);
			getCurrentUrlTask.Wait();
			webDriver.SwitchTo().Window(windowHandle);

			Task<bool> nextUrlTask = asyncCalls.TabActuallySwitched(webDriver, urlSegment, getCurrentUrlTask.Result);
			nextUrlTask.Wait();

			if (!nextUrlTask.Result)
			{
				return false;
			}
			
			Task<string> task = asyncCalls.GetCurrentUrlTask(webDriver);
			task.Wait();
			string urlAfterSwitch = task.Result;
			TestLog.Add("Switched to tab with url-segment " + urlSegment + ". Full url = " + urlAfterSwitch);
			asyncCalls.WaitWindowMaximize(webDriver);
			return true;
		}



		public static TResult ExecuteJavaScriptAsync<TResult>(this IWebDriver webDriver, AbstractPageSettings pageSettings, string script, params object[] args) 
		{
			WaitForJQueryIsLoaded(webDriver, pageSettings);
			return (TResult)GetJavaScriptExecutor(webDriver).ExecuteAsyncScript(script, args);
		}

		public static TResult ExecuteScript<TResult>(this IWebDriver webDriver, AbstractPageSettings pageSettings, string script, params object[] args)
		{
			WaitForJQueryIsLoaded(webDriver, pageSettings);
			dynamic objectToCast = GetJavaScriptExecutor(webDriver).ExecuteScript(script, args);

			if (objectToCast != null)
			{
				// ReSharper disable once SuspiciousTypeConversion.Global
				//The Webdriver casts Collection<object> when the list is empty but when it detects that the elements can be castet to IWebElement it will do so.
				//Therefore the ExecuteScript will give you for an empty javascript list a ReadOnlyCollection<object> 
				//but a ReadOnlyCollection<IWebElements> when there is at least one element within the JavaScript array/object!
				if (objectToCast is ReadOnlyCollection<object> && objectToCast.Count == 0)
				{
					var genericTypeArgument = typeof(TResult).GetGenericArguments()[0];
					Type paraType = typeof(List<>).MakeGenericType(genericTypeArgument);
					var para = Activator.CreateInstance(paraType);
					Type readonlyType = typeof(ReadOnlyCollection<>);
					Type result = readonlyType.MakeGenericType(genericTypeArgument);
					return (TResult)Activator.CreateInstance(result, para);
				}
			}
			return (TResult)objectToCast;
		}

		public static void ExecuteScript(this IWebDriver webDriver, AbstractPageSettings pageSettings, string script, params object[] args)
		{
			WaitForJQueryIsLoaded(webDriver, pageSettings);
			ExecuteScript<object>(webDriver, pageSettings, script, args);
		}
		
		private static IJavaScriptExecutor GetJavaScriptExecutor(this IWebDriver driver)
		{
			return driver as IJavaScriptExecutor;
		}

		/// <summary>
		/// Wait for all animations to be finished.
		/// </summary>
		private static bool AllAnimationsFinished(IWebDriver driver, AbstractPageSettings pageSettings, out string errorMessage)
		{
			if (pageSettings.PageUsesJquery && pageSettings.HasEndlessJQueryAnimation)
			{
				errorMessage = "Page has a endless animation. Cannot wait for animations finished.";
				return true;
			}

			bool jqueryIsLoaded;
			if (pageSettings.PageUsesJquery)
			{
				jqueryIsLoaded = JQueryIsLoaded(driver);
			}
			else
			{
				errorMessage = "Cannot wait for animations, page uses no JQuery. ";
				return true;
			}
			if (jqueryIsLoaded)
			{
				TestLog.Add("Get all running JQuery animations...");
				ReadOnlyCollection<IWebElement>  animatedObjects = driver.ExecuteScript<ReadOnlyCollection<IWebElement>>(pageSettings, "return $(':animated').toArray();");
				if (animatedObjects.Count != 0)
				{
					errorMessage = "Tried to click but there were still running JQuery animations on the webpage which are not excluded from waiting for them to finish."
					               + " || Objects still animated: " + GetAnimatedObjectMessage(animatedObjects) + " ||.";
					return false;
				}
				errorMessage = string.Empty;
				return true;
			}

			errorMessage = "JQuery was not loaded. Cannot wait for Animations";
			return false;
		}

		private static void WaitForJQueryIsLoaded(this IWebDriver webDriver, AbstractPageSettings pageSettings)
		{
			if (!pageSettings.PageUsesJquery)
				return;
			
			TimeSpan timeout = new TimeSpan(0, 0, 5);
			WebDriverWait wait = new WebDriverWait(webDriver, timeout) { Message = "JQuery was not loaded after " + timeout.Seconds + " seconds." };
			wait.Until(JQueryIsLoaded);
		}

		private static bool JQueryIsLoaded(IWebDriver webDriver)
		{
			return (bool)webDriver.GetJavaScriptExecutor().ExecuteScript("return typeof($)==='function'");
		}
	}
}