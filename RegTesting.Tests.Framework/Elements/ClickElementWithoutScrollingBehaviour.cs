using System;
using System.Collections.Generic;
using System.Threading;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class ClickElementWithoutScrollingBehaviour : IClickable
	{
		private readonly BasicPageElement _pageElement;

		public ClickElementWithoutScrollingBehaviour(BasicPageElement pageElement)
		{
			_pageElement = pageElement;
		}

		public void Click(TimeSpan waitBeforeClick, TimeSpan waitAfterClick)
		{
			if (waitBeforeClick > TimeSpan.Zero)
			{
				Thread.Sleep(waitBeforeClick);
				TestLog.Add("Waited '" + Convert.ToInt32(waitBeforeClick.TotalMilliseconds) + "' milliseconds before click.");
			}
			TestLog.Add("Click(WithoutScrolling): " + _pageElement.By);
			_pageElement.WebDriver.ClickElementWithoutScrolling(_pageElement);

			if (waitAfterClick > TimeSpan.Zero)
			{
				Thread.Sleep(waitAfterClick);
				TestLog.Add("Waited '" + Convert.ToInt32(waitAfterClick.TotalMilliseconds) + "' milliseconds after click.");
			}
		}

		public T ClickToPageObject<T>(TimeSpan waitBeforeClick) where T : BasePageObject
		{
			return ClickToPageObject<T>(waitBeforeClick, null);
		}

		public T ClickToPageObject<T>(TimeSpan waitBeforeClick, IDictionary<string, object> pageSettings) where T : BasePageObject
		{
			Click(waitBeforeClick, TimeSpan.Zero);
			return PageObjectFactory.GetPageObjectByType<T>(_pageElement.WebDriver, pageSettings);
		}
	}
}
