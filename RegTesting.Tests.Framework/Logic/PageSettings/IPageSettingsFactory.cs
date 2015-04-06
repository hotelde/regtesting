namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	public interface IPageSettingsFactory
	{
		AbstractPageSettings GetPageSettings(BasePageObject pageObject);
	}
}
