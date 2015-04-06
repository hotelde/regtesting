using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	public class FillbehaviourFactory
	{
		public static IFillable Create(FillBehaviour fillBehaviour, BasicPageElement pageElement)
		{
			switch (fillBehaviour)
			{
				case FillBehaviour.ClickFirstThenWaitForClear :
					return new FillBehaviourClickFirstThenWaitForClear(new FillbehaviourWaitForClearBeforeTyping(new DefaultFillBehaviour(pageElement), pageElement), pageElement);
				case FillBehaviour.WaitForClearBeforeTyping : 
					return new FillbehaviourWaitForClearBeforeTyping(new DefaultFillBehaviour(pageElement), pageElement);
				default: 
					return new DefaultFillBehaviour(pageElement);
			}
		}
	}
}
