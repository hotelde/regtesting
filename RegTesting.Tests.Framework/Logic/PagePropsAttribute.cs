using System;

namespace RegTesting.Tests.Framework.Logic
{
	internal class PagePropsAttribute : Attribute
	{
		public string PageUrl { get; set; }
		public Type PageSettings { get; set; }
	}
}
