using System;

namespace RegTesting.Tests.Framework.Attributes
{
	/// <summary>
	/// Attribute for supported Browsers
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SupportedBrowsers : Attribute
	{
		/// <summary>
		/// The supported Browsers
		/// </summary>
		public string[] Browsers { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="browsers">the supported Browsers</param>
		public SupportedBrowsers(params string[] browsers )
		{
			Browsers = browsers;
		}

	}
}
