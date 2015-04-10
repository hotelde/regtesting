using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public class Button : BasicPageElement
	{

		public Button(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentBasePageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default)
			: base(byLocator, webDriver, waitModel, parentBasePageObject, clickBehaviour)
		{
		}
	}
}
