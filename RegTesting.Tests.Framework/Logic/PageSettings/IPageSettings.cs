namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	public interface IPageSettings
	{
		void ApplySettings();

		bool PageUsesJquery { get; set; }

		bool HasEndlessJQueryAnimation { get; set; }
	}
}
