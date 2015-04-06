using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class DefaultSelectBehaviour : ISelectable
	{
		private readonly BasicPageElement _objPageElement;

		public DefaultSelectBehaviour(BasicPageElement objPageElement)
		{
			_objPageElement = objPageElement;
		}

		public void Type(string strText)
		{
			_objPageElement.WebDriver.WaitForElement(_objPageElement.By).Type(strText);
		}

		public void SelectByText(string strText)
		{
			IWebElement objWebElement =  _objPageElement.WebDriver.WaitForElement(_objPageElement.By);

			SelectElement objSelectElement = new SelectElement(objWebElement);
			objSelectElement.SelectByText(strText);
		}

		public void SelectByValue(string strValue)
		{
			IWebElement objElement = _objPageElement.WebDriver.WaitForElement(_objPageElement.By);
			SelectValue(objElement, strValue);
		}

		public void SelectByIndex(int intIndex)
		{
			IWebElement objWebElement = _objPageElement.WebDriver.WaitForElement(_objPageElement.By);
			SelectElement objSelectElement = new SelectElement(objWebElement);
			objSelectElement.SelectByIndex(intIndex);
		}


		private bool SelectValue(IWebElement objElement, string objValueToSelect)
		{
			try
			{
				objElement.FindElement(By.CssSelector(string.Format("option[value='{0}']", objValueToSelect))).Click();
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}
	}
}
