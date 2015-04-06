using System;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public interface IClickable
	{
		void Click(TimeSpan waitBeforeClick, TimeSpan waitAfterClick);
		T ClickToPageObject<T>(TimeSpan waitBeforeClick) where T : BasePageObject;

	}
}
