using System;
using System.Threading;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	class Input : BasicPageElement, IFillable, ITimedFillable
	{
		private readonly IFillable _fillBehaviour;

		public Input(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default, FillBehaviour fillBehaviour = FillBehaviour.Default)
			: base(objBy, objWebDriver, waitModel, parentPageObject, objClickBehaviour)
		{
			_fillBehaviour = FillbehaviourFactory.Create(fillBehaviour, this);
		}

		public void Type(string strText)
		{
			_fillBehaviour.Type(strText);
		}

		public void Type(DateTime datDateTime)
		{
			_fillBehaviour.Type(datDateTime);
		}

		public void Type(string text, TimeSpan timeToWaitAfterTyping)
		{
			Thread.Sleep(timeToWaitAfterTyping);
			Type(text);
		}

		public void Type(DateTime dateTime, TimeSpan timeToWaitAfterTyping)
		{
			Thread.Sleep(timeToWaitAfterTyping);
			Type(dateTime);
		}

		public string GetValue()
		{
			TestLog.Add("ReadInputValue: " + By);
			return WebDriver.WaitForElement(By).GetAttribute("value");
		}

		public void WaitForSpecificValue(string strValue)
		{
			TestLog.Add("WaitForSpecificValue: " + By + " should be " + strValue);
			WebDriver.WaitForElement(By).WaitForSpecificValue(strValue);
		}
	}


}
