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
	class DerivedInput : Input
	{
		public DerivedInput(By objBy, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default, FillBehaviour fillBehaviour = FillBehaviour.Default) : base(objBy, webDriver, waitModel, parentPageObject, clickBehaviour, fillBehaviour)
		{
		}
	}
}
