using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public class Image : BasicPageElement
	{
		public Image(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default)
			: base(byLocator, webDriver, waitModel, parentPageObject, clickBehaviour)
		{
		}
	}
}
