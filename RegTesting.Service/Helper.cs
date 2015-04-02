using System;

namespace RegTesting.Service
{
	/// <summary>
	/// A helperclass for some common functions.
	/// </summary>
	public class Helper
	{
		private static readonly Random Rnd = new Random();

		/// <summary>
		/// Get a unique number for screenshot.
		/// </summary>
		/// <returns>a integer for a unique screenshot number</returns>
		public static string GetScreenshotString()
		{
			return DateTime.Now.ToString("ddHHmmss") + "-" + Rnd.Next(999998);
		}

	}
}