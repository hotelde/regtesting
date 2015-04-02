using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegTesting.Contracts.Attributes
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
		/// <param name="arrBrowsers">the supported Browsers</param>
		public SupportedBrowsers(params string[] arrBrowsers )
		{
			Browsers = arrBrowsers;
		}

	}
}
