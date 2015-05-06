using System;
using System.Collections.Generic;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public interface IClickable
	{
		void Click(TimeSpan waitBeforeClick, TimeSpan waitAfterClick);

		T ClickToPageObject<T>(TimeSpan waitBeforeClick) where T : BasePageObject;

		T ClickToPageObject<T>(TimeSpan waitBeforeClick, IDictionary<string, object> pageSettings) where T : BasePageObject;
	}
}
