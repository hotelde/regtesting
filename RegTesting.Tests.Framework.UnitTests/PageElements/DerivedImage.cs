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
	class DerivedImage : Image
	{
		public DerivedImage(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default) : base(byLocator, webDriver, waitModel, parentPageObject, clickBehaviour)
		{
		}
	}
}
