using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.UnitTests.PageObjects
{
	class ParentPageObject : BasePageObject
	{

		[PartialPageObject]
		public BasicPageObject ChildPageObject { get; private set; }

		[Locate(How = How.Id, Using = "someElementID")]
		public BasicPageElement Element { get; private set; }



		public ParentPageObject(IWebDriver driver)
			: base(driver)
		{
		}

		public override IDictionary<string, object> DefaultPageSettings
		{
			get { return null; }
		}
	}
}
