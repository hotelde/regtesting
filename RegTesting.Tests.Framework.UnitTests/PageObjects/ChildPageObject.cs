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
	class ChildPageObject : BasePageObject
	{

		[Locate(How = How.Id, Using = "elementID")]
		public BasicPageElement Element { get; private set; }


		public ChildPageObject(IWebDriver driver)
			: base(driver)
		{
		}

		public override IDictionary<string, object> DefaultPageSettings
		{
			get { return null; }
		}

		public override string PageUrl
		{
			get { return null; }
		}

		public override string CreatePageUrlWithParameters(IEnumerable<string> urlParams)
		{
			return null;
		}
	}
}
