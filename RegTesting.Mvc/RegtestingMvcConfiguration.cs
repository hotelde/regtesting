using System;
using System.Configuration;

namespace RegTesting.Mvc
{

	/// <summary>
	/// Configuration for RegTesting
	/// </summary>
	public class RegtestingMvcConfiguration
	{

		/// <summary>
		/// The default testsuite
		/// </summary>
		public static string ThorDefaulttestsuite
		{
			get
			{
				return ConfigurationManager.AppSettings["ThorDefaulttestsuite"];
			}
		}

		/// <summary>
		/// The default testsuite
		/// </summary>
		public static string SodaDefaulttestsuite
		{
			get
			{
				return ConfigurationManager.AppSettings["SodaDefaulttestsuite"];
			}
		}

		/// <summary>
		/// The connectionString to use
		/// </summary>
		public static string DefaultConnectionString
		{
			get
			{
				return ConfigurationManager.AppSettings["DefaultConnectionString"];
			}
		}

		/// <summary>
		/// an array with all Testmanager
		/// </summary>
		public static string[] Testmanager
		{
			get
			{
				return ConfigurationManager.AppSettings["Testmanager"].Split(',');
			}
		}


	}
}