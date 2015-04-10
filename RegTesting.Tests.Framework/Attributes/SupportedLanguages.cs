using System;

namespace RegTesting.Tests.Framework.Attributes
{
	/// <summary>
	/// The attribute for supported languages
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SupportedLanguages : Attribute
	{
		/// <summary>
		/// The supported languages
		/// </summary>
		public string[] Languages { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="languages">the supported languages</param>
		public SupportedLanguages(params string[] languages )
		{
			Languages = languages;
		}

	}
}
