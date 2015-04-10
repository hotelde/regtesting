using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class DefaultSelectBehaviour : ISelectable
	{
		private readonly BasicPageElement _pageElement;

		public DefaultSelectBehaviour(BasicPageElement pageElement)
		{
			_pageElement = pageElement;
		}

		public void Type(string text)
		{
			_pageElement.WebDriver.WaitForElement(_pageElement.By).Type(text);
		}

		public void SelectByText(string text)
		{
			IWebElement webElement =  _pageElement.WebDriver.WaitForElement(_pageElement.By);

			SelectElement selectElement = new SelectElement(webElement);
			selectElement.SelectByText(text);
		}

		public void SelectByValue(string value)
		{
			IWebElement webElement = _pageElement.WebDriver.WaitForElement(_pageElement.By);
			SelectValue(webElement, value);
		}

		public void SelectByIndex(int index)
		{
			IWebElement webElement = _pageElement.WebDriver.WaitForElement(_pageElement.By);
			SelectElement selectElement = new SelectElement(webElement);
			selectElement.SelectByIndex(index);
		}


		private bool SelectValue(IWebElement webElement, string valueToSelect)
		{
			try
			{
				webElement.FindElement(By.CssSelector(string.Format("option[value='{0}']", valueToSelect))).Click();
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}
	}
}
