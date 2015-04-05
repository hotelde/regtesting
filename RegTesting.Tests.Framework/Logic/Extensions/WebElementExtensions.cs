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
		/// <param name="objElement">Extension hook</param>
		/// <param name="strText">string to type</param>
		public static void Type(this IWebElement objElement, string strText)
		{
			if (strText == null) return;
			try
			{
				objElement.Clear();
			}
				// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
				// ReSharper restore EmptyGeneralCatchClause
			{
				//Some older browser don't like clearing, so we should instead just begin sending our text.
			}
			objElement.SendKeys(strText);
		}

		/// <summary>
		/// Wait for the element to have a specific value
		/// </summary>
		/// <param name="objElement">Element for use</param>
		/// <param name="strValue">Value to wait for</param>
		/// <param name="strErrorMessage">A optional errorMessage</param>
		/// <param name="intTimeout">A optional custom Timeout</param>
		public static void WaitForSpecificValue(this IWebElement objElement, string strValue, string strErrorMessage = null,  int intTimeout = -1)
		{
			if (intTimeout == -1) intTimeout = Properties.Settings.Default.TestTimeout;

			for (int objSecond = 0; ; objSecond++)
			{

				if (objSecond == intTimeout) throw new TimeoutException(strErrorMessage ?? ( "(" + intTimeout + "s): Timeout: Element value is still not " + strValue));

				if (objElement.GetAttribute("value").Equals(strValue, StringComparison.InvariantCultureIgnoreCase))
					break;

				Thread.Sleep(1000);
			}
		}
	}
}