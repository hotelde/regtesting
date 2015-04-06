using System;
using System.Reflection;

namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	public class PageSettingsFactory : IPageSettingsFactory
	{

		public AbstractPageSettings GetPageSettings(BasePageObject pageObject)
		{
			PagePropsAttribute objPageAttributes = (PagePropsAttribute)pageObject.GetType().GetCustomAttribute(typeof(PagePropsAttribute), true);
			
			if (objPageAttributes != null && objPageAttributes.PageSettings != null)
			{
				return (AbstractPageSettings)Activator.CreateInstance(objPageAttributes.PageSettings, pageObject);
			}

			return new DefaultSettings(pageObject);
		}
	}
}
