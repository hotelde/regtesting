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
		private readonly ISlimServerService _channel;
		private readonly ChannelFactory<ISlimServerService> _httpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		public WcfClient()
		{
			_httpFactory = new ChannelFactory<ISlimServerService>("SlimServerServiceEndpoint");
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
		private Guid SendFile(string filename, string testsystem)
	    {
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				byte[] buffer = new byte[52428800];
				int size = fileStream.Read(buffer, 0, 52428800);
				byte[] bufferShort = buffer.Take(size).ToArray();

				try
				{
					return _channel.AddTestJob(testsystem, bufferShort);
				}
				catch (Exception e)
				{
					System.Windows.MessageBox.Show("An error occurred while requesting tests on the testserver: " + e);
					return Guid.Empty;
				}

			}
		}

		/// <summary>
		/// Start Tests at the Remote Server
		/// </summary>
		/// <param name="fileName">the filename of the testfile</param>
		/// <param name="testsystemUrl">the testsystem url</param>
		public Guid TestRemote(string fileName, string testsystemUrl)
		{
			testsystemUrl = testsystemUrl.ToLower().Replace("https://", "").Replace("http://", "");
			return SendFile(fileName, testsystemUrl);
		}
		
	}
}
