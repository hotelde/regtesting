using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class CheckBox : BasicPageElement
	{
		public CheckBox(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default)
			: base(byLocator, webDriver, waitModel, parentPageObject, clickBehaviour)
		{
		}

		public bool IsSelected()
		{
			return WebDriver.WaitForElement(By).Selected;
		}
	}
}
