using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.UnitTests.PageElements;

namespace RegTesting.Tests.Framework.UnitTests.PageObjects
{
	class DerivedElementsPageObject : BasePageObject
	{

		[Locate(How = How.Id, Using = "InputElement")]
		public DerivedInput InputElement { get; private set; }

		[Locate(How = How.Id, Using = "ImageElement")]
		public DerivedImage ImageElement { get; private set; }

		[Locate(How = How.Id, Using = "LinkElement")]
		public DerivedLink LinkElement { get; private set; }

		[Locate(How = How.Id, Using = "ButtonElement")]
		public DerivedButton ButtonElement { get; private set; }

		[Locate(How = How.Id, Using = "CheckBoxElement")]
		public DerivedCheckBox CheckBoxElement { get; private set; }

		[Locate(How = How.Id, Using = "SelectBoxElement")]
		public DerivedSelectBox SelectBoxElement { get; private set; }

		[Locate(How = How.Id, Using = "HiddenElement")]
		public DerivedHiddenElement HiddenElement { get; private set; }

		[Locate(How = How.Id, Using = "BasicPageElement")]
		public DerivedBasicPageElement BasicPageElement { get; private set; }


		public DerivedElementsPageObject(IWebDriver driver)
			: base(driver)
		{
		}

		public override IDictionary<string, object> DefaultPageSettings
		{
			get { return null; }
		}
	}
}
