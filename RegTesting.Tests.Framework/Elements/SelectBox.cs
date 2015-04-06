using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class SelectBox : BasicPageElement, ISelectable
	{
		private readonly ISelectable _selectBehaviour;

		public SelectBox(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ISelectable objClickBehaviour = null)
			: base(objBy, objWebDriver, waitModel, parentPageObject, ClickBehaviours.Default)
		{
			_selectBehaviour = objClickBehaviour ?? new DefaultSelectBehaviour(this);
		}


		public void SelectByText(string strText)
		{
			_selectBehaviour.SelectByText(strText);
		}

		public void SelectByValue(string strValue)
		{
			_selectBehaviour.SelectByValue(strValue);
		}

		public void SelectByIndex(int intValue)
		{
			_selectBehaviour.SelectByIndex(intValue);
		}

		public string GetValue()
		{
			TestLog.Add("GetValue: " + By);
			return new SelectElement(WebDriver.WaitForElement(By)).SelectedOption.GetAttribute("value");
		}

		public string GetText()
		{
			TestLog.Add("GetText: " + By);
			return new SelectElement(WebDriver.WaitForElement(By)).SelectedOption.Text;			
		}
	}


}
