using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;
using RegTesting.Tests.Framework.Properties;

namespace RegTesting.Tests.Framework.Elements
{
	public class FillbehaviourWaitForClearBeforeTyping : IFillable
	{
		private readonly IFillable _fillBehaviour;
		private readonly BasicPageElement _pageElement;
		private readonly TimeSpan _timeOut = new TimeSpan(0, 0, Settings.Default.TestTimeout);
		public FillbehaviourWaitForClearBeforeTyping(IFillable fillBehaviour, BasicPageElement pageElement)
		{
			_fillBehaviour = fillBehaviour;
			_pageElement = pageElement;
		}

		public void Type(string strText)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();


			WebDriverWait wait = new WebDriverWait(new SystemClock(), _pageElement.WebDriver, _timeOut, new TimeSpan(0, 0, 0, 0, 500))
			{
				Message = GetErrorMessage()
			};
			for (int i = 0; i < 3; i++)
			{
				Thread.Sleep(300);
				wait.Until(InputIsEmptyOrNotExampleText);
			}
			stopwatch.Stop();
			TestLog.Add("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for input-field to be empty.");
			_fillBehaviour.Type(strText);
		}

		public void Type(DateTime datDateTime)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			WebDriverWait wait = new WebDriverWait(new SystemClock(), _pageElement.WebDriver, _timeOut, new TimeSpan(0, 0, 0, 0, 500))
			{
				Message = GetErrorMessage()
			};
			wait.Until(InputIsEmptyOrNotExampleText);
			stopwatch.Stop();
			TestLog.Add("Waited " + stopwatch.ElapsedMilliseconds + " milliseconds for input-field to be empty.");
			_fillBehaviour.Type(datDateTime);
		}

		private string GetErrorMessage()
		{
			return "Wait for InputField is empty or not example text before typing timed out after " + _timeOut.Seconds + " seconds.";
		}

		private bool InputIsEmptyOrNotExampleText(IWebDriver driver)
		{
			IWebElement foundElement = null;
			try
			{
				foundElement = driver.WaitForElement(_pageElement.By, timeout: new TimeSpan(0, 0, 1));
			}
			catch (WebDriverTimeoutException)
			{
			}

			if (foundElement == null)
				return false;

			string valueAttribute = foundElement.GetAttribute("value");
			string dbtAttribute = foundElement.GetAttribute("dbt");
			if (valueAttribute == null || dbtAttribute == null)
				return false;

			return valueAttribute.Equals(String.Empty) || !valueAttribute.Equals(dbtAttribute);
		}
	}
}
