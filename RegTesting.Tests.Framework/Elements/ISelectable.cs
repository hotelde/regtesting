namespace RegTesting.Tests.Framework.Elements
{
	public interface ISelectable
	{
		void SelectByText(string text);
		void SelectByValue(string value);
		void SelectByIndex(int index);
	}
}
