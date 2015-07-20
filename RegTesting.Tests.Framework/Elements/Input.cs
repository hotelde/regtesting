using System;
using System.Threading;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class Input : BasicPageElement, IFillable, ITimedFillable
	{
		private readonly IFillable _fillBehaviour;

		public Input(By objBy, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default, FillBehaviour fillBehaviour = FillBehaviour.Default)
			: base(objBy, webDriver, waitModel, parentPageObject, clickBehaviour)
		{
			_fillBehaviour = TestContext.FillBehaviourFactory.Create(fillBehaviour, this);
		}

		public void Type(string text)
		{
			_fillBehaviour.Type(text);
		}

		public void Type(DateTime dateTime)
		{
			_fillBehaviour.Type(dateTime);
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

		public void WaitForSpecificValue(string value)
		{
			TestLog.Add("WaitForSpecificValue: " + By + " should be " + value);
			WebDriver.WaitForElement(By).WaitForSpecificValue(value);
		}
	}


}
