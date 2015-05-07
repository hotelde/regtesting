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
	class DefaultElementsPageObject : BasePageObject
	{

		[Locate(How = How.Id, Using = "InputElement")]
		public Input InputElement { get; private set; }

		[Locate(How = How.Id, Using = "ImageElement")]
		public Image ImageElement { get; private set; }

		[Locate(How = How.Id, Using = "LinkElement")]
		public Link LinkElement { get; private set; }

		[Locate(How = How.Id, Using = "ButtonElement")]
		public Button ButtonElement { get; private set; }

		[Locate(How = How.Id, Using = "CheckBoxElement")]
		public CheckBox CheckBoxElement { get; private set; }

		[Locate(How = How.Id, Using = "SelectBoxElement")]
		public SelectBox SelectBoxElement { get; private set; }

		[Locate(How = How.Id, Using = "HiddenElement")]
		public HiddenElement HiddenElement { get; private set; }




		public DefaultElementsPageObject(IWebDriver driver)
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
