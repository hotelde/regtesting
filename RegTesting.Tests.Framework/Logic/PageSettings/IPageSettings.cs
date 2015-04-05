namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	internal interface IPageSettings
	{
		void ApplySettings();

		bool PageUsesJquery { get; set; }

		bool HasEndlessJQueryAnimation { get; set; }
	}
}
