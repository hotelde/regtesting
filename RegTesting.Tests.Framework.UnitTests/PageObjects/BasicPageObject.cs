using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.UnitTests.PageObjects
{
	class BasicPageObject : BasePageObject
	{

		[Locate(How = How.Id, Using = "elementID")]
		public BasicPageElement Element { get; private set; }


		public BasicPageObject(IWebDriver driver) : base(driver)
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
