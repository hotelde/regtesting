namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	internal interface IPageSettingsFactory
	{
		AbstractPageSettings GetPageSettings(BasePageObject pageObject);
	}
}
