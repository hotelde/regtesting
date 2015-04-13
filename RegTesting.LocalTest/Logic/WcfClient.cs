using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using RegTesting.Contracts.Services;

namespace RegTesting.LocalTest.Logic
{
	/// <summary>
	/// The WCFClient
	/// </summary>
	public class WcfClient : IDisposable
	{
		private readonly ILocalTestService _channel;
		private readonly ChannelFactory<ILocalTestService> _httpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		public WcfClient()
		{
			_httpFactory = new ChannelFactory<ILocalTestService>("LocalTestServiceEndpoint");
			_channel = _httpFactory.CreateChannel();
		}


		/// <summary>
		/// Dispose method for BusinessDelegate class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_httpFactory != null)
			{
			    _httpFactory.Close();
			}
		}

	    /// <summary>
	    /// Send a file to testserver
	    /// </summary>
	    /// <param name="filename">Filename of the testcases dll</param>
	    /// <param name="testsystem">testsystem of the testcases</param>
		private void SendFile(string filename, string testsystem)
	    {
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				byte[] buffer = new byte[52428800];
				int size = fileStream.Read(buffer, 0, 52428800);
				byte[] bufferShort = buffer.Take(size).ToArray();
				_channel.SendTestcaseFile(testsystem, bufferShort);
			}
		}

		private static string GetUser()
		{
			string userName = WindowsIdentity.GetCurrent().Name;
			int endOfDomainPart = userName.IndexOf("\\");
			return (endOfDomainPart > -1) ? userName.Substring(endOfDomainPart + 1, userName.Length - endOfDomainPart - 1) : null;
		}

		/// <summary>
		/// Get all languages
		/// </summary>
		/// <returns>A list of strings with all languages </returns>
		public List<string> GetLanguages()
		{
			List<string> languages = _channel.GetLanguages().Select(t => t.Languagecode).ToList();
			languages.Sort();
			return languages;
		}

		/// <summary>
		/// Get all browsers
		/// </summary>
		/// <returns>A list of string with all browsers</returns>
		public List<string> GetBrowsers()
		{
			List<string> browsers = _channel.GetBrowsers().Select(t=> t.Name).ToList();
			browsers.Sort();
			return browsers;

		}

		/// <summary>
		/// Start Tests at the Remote Server
		/// </summary>
		/// <param name="fileName">the filename of the testfile</param>
		/// <param name="testsystemUrl">the testsystem url</param>
		/// <param name="browsers">the browsers</param>
		/// <param name="testcases">the testcases</param>
		/// <param name="languages">the languages</param>
		public void TestRemote(string fileName, string testsystemUrl, List<string> browsers, List<string> testcases, List<string> languages)
		{
			testsystemUrl = testsystemUrl.ToLower().Replace("https://", "").Replace("http://", "");

			string username = GetUser();
			string testsystemName = "local/" + testsystemUrl + "-" + username;
		

			SendFile(fileName, testsystemName);
			_channel.AddLocalTestTasks(username, testsystemName, testsystemUrl, browsers, testcases, languages);
			int testsCount = browsers.Count*testcases.Count*languages.Count;
			System.Windows.MessageBox.Show(String.Format("Requested {0} tests on {1}. You'll receive a mail when your results are ready!" , testsCount, testsystemUrl));
		}
	}
}
