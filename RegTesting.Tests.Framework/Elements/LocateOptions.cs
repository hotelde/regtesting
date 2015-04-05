using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Elements
{
	class LocateOptions
	{
		private Visibility _visibility = Visibility.Visible;
		public By By { get; set; }

		public Visibility Visibility
		{
			get { return _visibility; }
			set { _visibility = value; }
		}
	}
}
