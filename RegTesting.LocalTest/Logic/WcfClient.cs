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
		private readonly ILocalTestService _objChannel;
		private readonly ChannelFactory<ILocalTestService> _objHttpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		public WcfClient()
		{
			_objHttpFactory = new ChannelFactory<ILocalTestService>("LocalTestServiceEndpoint");
			_objChannel = _objHttpFactory.CreateChannel();
		}


		/// <summary>
		/// Dispose method for BusinessDelegate class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_objHttpFactory != null)
			{
			    _objHttpFactory.Close();
			}
		}

	    /// <summary>
	    /// Send a file to testserver
	    /// </summary>
	    /// <param name="strFilename">Filename of the testcases dll</param>
	    /// <param name="strTestsystem">testsystem of the testcases</param>
		private void SendFile(string strFilename, string strTestsystem)
	    {
			using (FileStream objFileStream = new FileStream(strFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				byte[] arrBuffer = new byte[52428800];
				int intSize = objFileStream.Read(arrBuffer, 0, 52428800);
				byte[] arrBufferShort = arrBuffer.Take(intSize).ToArray();
				_objChannel.SendTestcaseFile(strTestsystem, arrBufferShort);
			}
		}

		private static string GetUser()
		{
			string strUser = WindowsIdentity.GetCurrent().Name;
			int intEndOfDomainPart = strUser.IndexOf("\\");
			return (intEndOfDomainPart > -1) ? strUser.Substring(intEndOfDomainPart + 1, strUser.Length - intEndOfDomainPart - 1) : null;
		}

		/// <summary>
		/// Get all languages
		/// </summary>
		/// <returns>A list of strings with all languages </returns>
		public List<string> GetLanguages()
		{
			List<string> lstLanguages = _objChannel.GetLanguages().Select(t => t.Languagecode).ToList();
			lstLanguages.Sort();
			return lstLanguages;
		}

		/// <summary>
		/// Get all browsers
		/// </summary>
		/// <returns>A list of string with all browsers</returns>
		public List<string> GetBrowsers()
		{
			List<string> lstBrowsers = _objChannel.GetBrowsers().Select(t=> t.Name).ToList();
			lstBrowsers.Sort();
			return lstBrowsers;

		}

		/// <summary>
		/// Start Tests at the Remote Server
		/// </summary>
		/// <param name="strTestsystemUrl">the testsystem url</param>
		/// <param name="lstBrowser">the browsers</param>
		/// <param name="lstTestcases">the testcases</param>
		/// <param name="lstLanguages">the languages</param>
		public void TestRemote(string strTestsystemUrl, List<string> lstBrowser, List<string> lstTestcases, List<string> lstLanguages)
		{
			strTestsystemUrl = strTestsystemUrl.ToLower().Replace("https://", "").Replace("http://", "");

			const string fileName = "RegTesting.Tests.dll";
			string strUser = GetUser();
			string strTestsystemName = "local/" + strTestsystemUrl + "-" + strUser;
		

			SendFile(fileName, strTestsystemName);
			_objChannel.AddLocalTestTasks(strUser, strTestsystemName, strTestsystemUrl, lstBrowser, lstTestcases, lstLanguages);
			int intTestsCount = lstBrowser.Count*lstTestcases.Count*lstLanguages.Count;
			System.Windows.MessageBox.Show(String.Format("Requested {0} tests on {1}. You'll receive a mail when your results are ready!" , intTestsCount, strTestsystemUrl));
		}
	}
}
