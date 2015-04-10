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
		public Task<ReadOnlyCollection<string>> GetWindowHandlesAsync(IWebDriver webDriver)
		{
			Task<ReadOnlyCollection<string>> task = Task.Factory.StartNew(() => GetWindowHandles(webDriver));
			return task;
		}

		private ReadOnlyCollection<string> GetWindowHandles(IWebDriver webDriver)
		{
			ReadOnlyCollection<string> result;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				result = webDriver.WindowHandles;
				if (stopwatch.ElapsedMilliseconds > _timeOut)
				{
					throw new Exception("Could not get WindowHandles from WebDriver.");
				}
			} while (result == null);
			stopwatch.Stop();

			return result;
		}

		public Task<bool> TabActuallySwitched(IWebDriver webDriver, string targetUrl, string urlBeforeSwitch)
		{
			Task<bool> task;
			try
			{
				task = Task.Factory.StartNew(() => EnsureTabSwitchedWithTimeOut(webDriver, targetUrl, urlBeforeSwitch));
			}
			catch (Exception e)
			{
				throw e.InnerException ?? e;
			}
			
			return task;
		}

		private bool EnsureTabSwitchedWithTimeOut(IWebDriver webDriver, string targetUrl, string urlBeforeSwitch)
		{
			bool result;
			string currentUrl;
			targetUrl = targetUrl.ToLowerInvariant();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				string currentUrlWithTimeout = GetCurrentUrlWithTimeout(webDriver);

				currentUrl = currentUrlWithTimeout;
				result = currentUrlWithTimeout.ToLowerInvariant().Contains(targetUrl);
			} while (!result && stopwatch.ElapsedMilliseconds < _tabSwitchTimeOut && urlBeforeSwitch == currentUrl);
			stopwatch.Stop();
			
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds to ensure tabswitch.");
			
			return result;
		}

		public Task<string> GetCurrentUrlTask(IWebDriver webDriver)
		{
			Task<string> task;
			try
			{
				task = Task.Factory.StartNew(() => GetCurrentUrlWithTimeout(webDriver));
			}
			catch (Exception e)
			{
				throw e.InnerException ?? e;
			}

			return task;
		}

		private string GetCurrentUrlWithTimeout(IWebDriver webDriver)
		{
			string currentUrl ;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				currentUrl = webDriver.Url;
			} while (string.IsNullOrWhiteSpace(currentUrl) && stopwatch.ElapsedMilliseconds < _timeOut);
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for for current url.");
			stopwatch.Stop();
			return currentUrl;
		}

		public void WaitWindowMaximize(IWebDriver webDriver)
		{
			Task<IWebDriver> task = Task.Factory.StartNew(() => WindowMaximize(webDriver));
			task.Wait();
		}

		private IWebDriver WindowMaximize(IWebDriver webDriver)
		{
			int currentHeight;
			int currentWidth;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			webDriver.Manage().Window.Maximize();
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			do
			{
				currentHeight = webDriver.Manage().Window.Size.Height;
				currentWidth = webDriver.Manage().Window.Size.Width;
			} while ((currentHeight < workingArea.Height || currentWidth < workingArea.Width) && stopwatch.ElapsedMilliseconds < _timeOut);
			TestLog.AddWithoutTime("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for window to maximize.");
			stopwatch.Stop();
			
			return webDriver;
		}
	}
}
