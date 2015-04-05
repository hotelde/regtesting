using System;
using System.Threading;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	class DefaultClickBehaviour : IClickable
	{
		private readonly BasicPageElement _objPageElement;

		public DefaultClickBehaviour(BasicPageElement objPageElement)
		{
			_objPageElement = objPageElement;
		}

		public void Click(TimeSpan waitBeforeClick, TimeSpan waitAfterClick)
		{
			if (waitBeforeClick > TimeSpan.Zero)
			{
				Thread.Sleep(waitBeforeClick);
				TestLog.Add("Waited '" + Convert.ToInt32(waitBeforeClick.TotalMilliseconds) + "' milliseconds before click.");
			}
			_objPageElement.WebDriver.ClickElement(_objPageElement);
			if (waitAfterClick > TimeSpan.Zero)
			{
				
				Thread.Sleep(waitAfterClick);
				TestLog.Add("Waited '" + Convert.ToInt32(waitAfterClick.TotalMilliseconds) + "' milliseconds after click.");
			}
		}

		public T ClickToPageObject<T>(TimeSpan waitBeforeClick) where T : BasePageObject
		{
			Click(waitBeforeClick, TimeSpan.Zero);
			return PageObjectFactory.GetPageObjectByType<T>(_objPageElement.WebDriver);
		}
	}
}
