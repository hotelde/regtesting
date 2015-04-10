using System;
using System.Globalization;
using System.Threading;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class DefaultFillBehaviour : IFillable
	{
		private readonly BasicPageElement _pageElement;

		public DefaultFillBehaviour(BasicPageElement pageElement)
		{
			_pageElement = pageElement;
		}

		public void Type(string text)
		{
			TestLog.Add("Fill: " + _pageElement.By + " -> " + text);
			_pageElement.WebDriver.WaitForElement(_pageElement.By).Type(text);
		}

		public void Type(DateTime dateTime)
		{
			CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
			string dateString = dateTime.ToString("d", currentUiCulture);
			TestLog.Add("FillDate (use culture " + currentUiCulture + "): " + _pageElement.By + " -> " + dateString);
			_pageElement.WebDriver.WaitForElement(_pageElement.By).Type(dateString);
		}
	}
}
