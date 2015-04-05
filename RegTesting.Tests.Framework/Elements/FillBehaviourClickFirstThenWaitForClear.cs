using System;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	class FillBehaviourClickFirstThenWaitForClear : IFillable
	{
		private readonly IFillable _fillBehaviour;
		private readonly BasicPageElement _pageElement;

		public FillBehaviourClickFirstThenWaitForClear(IFillable fillBehaviour, BasicPageElement pageElement)
		{
			_fillBehaviour = fillBehaviour;
			_pageElement = pageElement;
		}

		public void Type(string strText)
		{
			TestLog.Add(GetTestLogString());
			_pageElement.Click();
			_fillBehaviour.Type(strText);
		}

		public void Type(DateTime datDateTime)
		{
			TestLog.Add(GetTestLogString());
			_pageElement.Click();
			_fillBehaviour.Type(datDateTime);
		}

		private string GetTestLogString()
		{
			return _pageElement.By + "' will be clicked before typing.";
		}
	}
}
