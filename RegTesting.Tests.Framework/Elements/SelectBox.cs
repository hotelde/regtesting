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

		public SelectBox(By objBy, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ISelectable selectBehaviour = null)
			: base(objBy, webDriver, waitModel, parentPageObject, ClickBehaviours.Default)
		{
			_selectBehaviour = selectBehaviour ?? new DefaultSelectBehaviour(this);
		}


		public void SelectByText(string text)
		{
			_selectBehaviour.SelectByText(text);
		}

		public void SelectByValue(string value)
		{
			_selectBehaviour.SelectByValue(value);
		}

		public void SelectByIndex(int index)
		{
			_selectBehaviour.SelectByIndex(index);
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
