using System;
using System.Threading;
using OpenQA.Selenium;

namespace RegTesting.Tests.Framework.Logic.Extensions
{
	/// <summary>
	/// A extensions class for the WebElement class.
	/// </summary>
	public static class WebElementExtensions
	{
		/// <summary>
		///  Like Sendkeys, but with with an Clear() before SendKeys().
		/// </summary>
		/// <param name="webElement">Extension hook</param>
		/// <param name="text">string to type</param>
		public static void Type(this IWebElement webElement, string text)
		{
			if (text == null) return;
			try
			{
				webElement.Clear();
			}
				// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
				// ReSharper restore EmptyGeneralCatchClause
			{
				//Some older browser don't like clearing, so we should instead just begin sending our text.
			}
			webElement.SendKeys(text);
		}

		/// <summary>
		/// Wait for the element to have a specific value
		/// </summary>
		/// <param name="webElement">Element for use</param>
		/// <param name="value">Value to wait for</param>
		/// <param name="errorMessage">A optional errorMessage</param>
		/// <param name="timeoutInSeconds">A optional custom Timeout in seconds</param>
		public static void WaitForSpecificValue(this IWebElement webElement, string value, string errorMessage = null,  int timeoutInSeconds = -1)
		{
			if (timeoutInSeconds == -1) timeoutInSeconds = Properties.Settings.Default.TestTimeout;

			for (int second = 0; ; second++)
			{

				if (second == timeoutInSeconds) throw new TimeoutException(errorMessage ?? ( "(" + timeoutInSeconds + "s): Timeout: Element value is still not " + value));

				if (webElement.GetAttribute("value").Equals(value, StringComparison.InvariantCultureIgnoreCase))
					break;

				Thread.Sleep(1000);
			}
		}
	}
}