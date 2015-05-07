using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.UnitTests.PageObjects
{
	class EmptyPageObject : BasePageObject
	{
		public EmptyPageObject(IWebDriver driver) : base(driver)
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
