using System;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public class FillBehaviourClickFirstThenWaitForClear : IFillable
	{
		private readonly IFillable _fillBehaviour;
		private readonly BasicPageElement _pageElement;

		public FillBehaviourClickFirstThenWaitForClear(IFillable fillBehaviour, BasicPageElement pageElement)
		{
			_fillBehaviour = fillBehaviour;
			_pageElement = pageElement;
		}

		public void Type(string text)
		{
			TestLog.Add(GetTestLogString());
			_pageElement.Click();
			_fillBehaviour.Type(text);
		}

		public void Type(DateTime dateTime)
		{
			TestLog.Add(GetTestLogString());
			_pageElement.Click();
			_fillBehaviour.Type(dateTime);
		}

		private string GetTestLogString()
		{
			return _pageElement.By + "' will be clicked before typing.";
		}
	}
}
