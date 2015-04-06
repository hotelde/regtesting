namespace RegTesting.Tests.Framework.Elements
{
	public interface ISelectable
	{
		void SelectByText(string strText);
		void SelectByValue(string strValue);
		void SelectByIndex(int intValue);
	}
}
