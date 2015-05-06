using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.UnitTests.PageElements
{
	class DerivedSelectBox : SelectBox
	{
		public DerivedSelectBox(By objBy, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ISelectable selectBehaviour = null) : base(objBy, webDriver, waitModel, parentPageObject, selectBehaviour)
		{
		}
	}
}
