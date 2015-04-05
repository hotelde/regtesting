using System;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	class ClickBehaviourFactory
	{
		public static IClickable Create(ClickBehaviours enmClickBehaviour, BasicPageElement objElement)
		{
			switch (enmClickBehaviour)
			{
				case ClickBehaviours.Default:
					return new DefaultClickBehaviour(objElement);
				case ClickBehaviours.ClickWithoutScrolling:
					return new ClickElementWithoutScrollingBehaviour(objElement);
				default:
					throw new Exception("Unknown clickBehaviour. RegTesting.Logic.ClickBehaviourFactory does not know the ClickBehaviour '" + enmClickBehaviour + "'.");
			}
		}
	}
}
