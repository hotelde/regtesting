using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	class Image : BasicPageElement
	{
		public Image(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default)
			: base(objBy, objWebDriver, waitModel, parentPageObject, objClickBehaviour)
		{
		}
	}
}
