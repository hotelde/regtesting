using System;
using System.Reflection;

namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	public class PageSettingsFactory : IPageSettingsFactory
	{

		public AbstractPageSettings GetPageSettings(BasePageObject pageObject)
		{
			PagePropsAttribute pageAttributes = (PagePropsAttribute)pageObject.GetType().GetCustomAttribute(typeof(PagePropsAttribute), true);
			
			if (pageAttributes != null && pageAttributes.PageSettings != null)
			{
				return (AbstractPageSettings)Activator.CreateInstance(pageAttributes.PageSettings, pageObject);
			}

			return new DefaultSettings(pageObject);
		}
	}
}
