using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class CheckBox : BasicPageElement
	{
		public CheckBox(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default)
			: base(objBy, objWebDriver, waitModel, parentPageObject, objClickBehaviour)
		{
		}

		public bool IsSelected()
		{
			return WebDriver.WaitForElement(By).Selected;
		}
	}
}
