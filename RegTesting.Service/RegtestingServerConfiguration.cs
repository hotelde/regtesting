using System;
using System.Configuration;

namespace RegTesting.Service
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
		/// The mailadress to notify in case of errors
		/// </summary>
		public static string Errormailadress
		{
			get
			{
				return ConfigurationManager.AppSettings["Errormailadress"];
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
		/// The TfsUrl
		/// </summary>
		public static string TfsUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["TfsUrl"];
			}
		}

		/// <summary>
		/// The default timeout for longlife tests.
		/// </summary>
		public static string DefaultLongLifeTime
		{
			get
			{
				return ConfigurationManager.AppSettings["DefaultLongLifeTime"];
			}
		}

		/// <summary>
		/// The url to the webportal
		/// </summary>
		public static string Webportal
		{
			get
			{
				return ConfigurationManager.AppSettings["Webportal"];
			}
		}

		/// <summary>
		/// the RecycleTime for testWorker in Seconds
		/// </summary>
		public static int RecycleTime
		{
			get
			{
				string time = ConfigurationManager.AppSettings["RecycleTime"];
				return String.IsNullOrWhiteSpace(time) ? 1800 : int.Parse(time);
			}

		}

		/// <summary>
		/// the RecycleTime for testWorker in Seconds
		/// </summary>
		public static int MaxRunCount
		{
			get
			{
				string maxRunCount = ConfigurationManager.AppSettings["MaxRunCount"];
				return String.IsNullOrWhiteSpace(maxRunCount) ? 1 : int.Parse(maxRunCount);
			}
		}
		
		/// <summary>
		/// the mailadress for the automated test results mail
		/// </summary>
		public static string NotifyAutomatedTestResultsMail
		{
			get
			{
				return ConfigurationManager.AppSettings["NotifyAutomatedTestResultsMail"];
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


		/// <summary>
		/// the address of the imap server to send mails
		/// </summary>
		public static string SmtpServer
		{
			get
			{
				return ConfigurationManager.AppSettings["SmtpServer"];
			}
		}

		/// <summary>
		/// the address of the imap server to send mails
		/// </summary>
		public static string SenderEmail
		{
			get
			{
				return ConfigurationManager.AppSettings["SenderEmail"];
			}
		}

	}
}