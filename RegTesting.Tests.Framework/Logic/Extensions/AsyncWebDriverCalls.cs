using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace RegTesting.Tests.Framework.Logic.Extensions
{
	internal class AsyncWebDriverCalls
	{
		private readonly int _timeOut;
		private readonly int _tabSwitchTimeOut;

		public AsyncWebDriverCalls()
		{
			_timeOut = Properties.Settings.Default.AsyncCallTimeOut;
			_tabSwitchTimeOut = Properties.Settings.Default.SwitchTabTimeOut;
		}
		public Task<ReadOnlyCollection<string>> GetWindowHandlesAsync(IWebDriver driver)
		{
			Task<ReadOnlyCollection<string>> task = Task.Factory.StartNew(() => GetWindowHandles(driver));
			return task;
		}

		private ReadOnlyCollection<string> GetWindowHandles(IWebDriver driver)
		{
			ReadOnlyCollection<string> result;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				result = driver.WindowHandles;
				if (stopwatch.ElapsedMilliseconds > _timeOut)
				{
					throw new Exception("Could not get WindowHandles from WebDriver.");
				}
			} while (result == null);
			stopwatch.Stop();

			return result;
		}

		public Task<bool> TabActuallySwitched(IWebDriver driver, string targetUrl, string urlBeforeSwitch)
		{
			Task<bool> task;
			try
			{
				task = Task.Factory.StartNew(() => EnsureTabSwitchedWithTimeOut(driver, targetUrl, urlBeforeSwitch));
			}
			catch (Exception e)
			{
				throw e.InnerException ?? e;
			}
			
			return task;
		}

		private bool EnsureTabSwitchedWithTimeOut(IWebDriver driver, string targetUrl, string urlBeforeSwitch)
		{
			bool result;
			string currentUrl;
			targetUrl = targetUrl.ToLowerInvariant();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				string currentUrlWithTimeout = GetCurrentUrlWithTimeout(driver);

				currentUrl = currentUrlWithTimeout;
				result = currentUrlWithTimeout.ToLowerInvariant().Contains(targetUrl);
			} while (!result && stopwatch.ElapsedMilliseconds < _tabSwitchTimeOut && urlBeforeSwitch == currentUrl);
			stopwatch.Stop();
			
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds to ensure tabswitch.");
			
			return result;
		}

		public Task<string> GetCurrentUrlTask(IWebDriver driver)
		{
			Task<string> task;
			try
			{
				task = Task.Factory.StartNew(() => GetCurrentUrlWithTimeout(driver));
			}
			catch (Exception e)
			{
				throw e.InnerException ?? e;
			}

			return task;
		}

		private string GetCurrentUrlWithTimeout(IWebDriver driver)
		{
			string currentUrl ;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				currentUrl = driver.Url;
			} while (string.IsNullOrWhiteSpace(currentUrl) && stopwatch.ElapsedMilliseconds < _timeOut);
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for for current url.");
			stopwatch.Stop();
			return currentUrl;
		}

		public void WaitWindowMaximize(IWebDriver driver)
		{
			Task<IWebDriver> task = Task.Factory.StartNew(() => WindowMaximize(driver));
			task.Wait();
		}

		private IWebDriver WindowMaximize(IWebDriver driver)
		{
			int currentHeight;
			int currentWidth;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			driver.Manage().Window.Maximize();
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			do
			{
				currentHeight = driver.Manage().Window.Size.Height;
				currentWidth = driver.Manage().Window.Size.Width;
			} while ((currentHeight < workingArea.Height || currentWidth < workingArea.Width) && stopwatch.ElapsedMilliseconds < _timeOut);
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for window to maximize.");
			stopwatch.Stop();
			
			return driver;
		}
	}
}
