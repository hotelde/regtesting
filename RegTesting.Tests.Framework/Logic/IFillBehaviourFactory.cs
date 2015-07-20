using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	public interface IFillBehaviourFactory
	{
		IFillable Create(FillBehaviour fillBehaviour, BasicPageElement pageElement);
	}

}
