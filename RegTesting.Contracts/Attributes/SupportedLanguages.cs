using System;

namespace RegTesting.Contracts.Attributes
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
		/// <param name="arrLanguages">the supported languages</param>
		public SupportedLanguages(params string[] arrLanguages )
		{
			Languages = arrLanguages;
		}

	}
}
