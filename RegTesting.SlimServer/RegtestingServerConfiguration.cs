using System;
using System.Configuration;

namespace RegTesting.SlimServer
{

	/// <summary>
	/// Configuration for RegTesting
	/// </summary>
	public class RegtestingServerConfiguration
	{
		/// <summary>
		/// The folder where the tests are in
		/// </summary>
		public static string Testsfolder
		{
			get
			{
				return ConfigurationManager.AppSettings["Testsfolder"];
			}
		}

		/// <summary>
		/// The folder where the screenshots are in
		/// </summary>
		public static string Screenshotsfolder
		{
			get
			{
				return ConfigurationManager.AppSettings["Screenshotsfolder"];
			}
		}

		/// <summary>
		/// the Port for the services
		/// </summary>
		public static int Port
		{
			get
			{
				string port = ConfigurationManager.AppSettings["Port"];
				return String.IsNullOrWhiteSpace(port) ? 8005 : int.Parse(port);
			}
		}


	}
}