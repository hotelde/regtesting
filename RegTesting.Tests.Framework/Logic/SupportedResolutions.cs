using System;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// Attribute for supported Resolutions
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SupportedResolutions : Attribute
	{
		/// <summary>
		/// The supported Resolutions
		/// </summary>
		public Resolution[] Resolutions { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="resolutions">the supported Resolutions</param>
		public SupportedResolutions(params Resolution[] resolutions)
		{
			Resolutions = resolutions;
		}

	}
}
