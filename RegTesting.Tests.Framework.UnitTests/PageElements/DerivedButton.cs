using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.UnitTests.PageElements
{
	class DerivedButton : Button
	{
		public DerivedButton(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentBasePageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default) : base(byLocator, webDriver, waitModel, parentBasePageObject, clickBehaviour)
		{
		}
	}
}
