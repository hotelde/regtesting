using System;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	public class ClickBehaviourFactory : IClickBehaviourFactory
	{
		IClickable IClickBehaviourFactory.Create(ClickBehaviours clickBehaviour, BasicPageElement pageElement)
		{
			switch (clickBehaviour)
			{
				case ClickBehaviours.Default:
					return new DefaultClickBehaviour(pageElement);
				case ClickBehaviours.ClickWithoutScrolling:
					return new ClickElementWithoutScrollingBehaviour(pageElement);
				default:
					throw new Exception("Unknown clickBehaviour. RegTesting.Logic.ClickBehaviourFactory does not know the ClickBehaviour '" + clickBehaviour + "'.");
			}
		}
	}
}
