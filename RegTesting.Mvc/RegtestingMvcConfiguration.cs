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
		public static string Defaulttestsuite
		{
			get
			{
				return ConfigurationManager.AppSettings["Defaulttestsuite"];
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

		/// <summary>
		/// an array with all PinnedBranches
		/// </summary>
		public static string[] PinnedBranches
		{
			get
			{
				return ConfigurationManager.AppSettings["PinnedBranches"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			}
		}


	}
}