namespace RegTesting.Tests.Framework.Elements
{
	interface ISelectable
	{
		void SelectByText(string strText);
		void SelectByValue(string strValue);
		void SelectByIndex(int intValue);
	}
}
