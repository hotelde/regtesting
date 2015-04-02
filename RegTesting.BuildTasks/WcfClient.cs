using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using RegTesting.Contracts.Services;

namespace RegTesting.BuildTasks
{
	/// <summary>
	/// The WCFClient
	/// </summary>
	public class WcfClient : IDisposable
	{
		private readonly IBuildTaskService _objChannel;
		private readonly ChannelFactory<IBuildTaskService> _objHttpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		/// <param name="strEndpointAddress">The endpointAdress to connect to</param>
		public WcfClient(String strEndpointAddress)
		{
			EndpointAddress objEndpointAddress = new EndpointAddress(strEndpointAddress);
			if (_objChannel != null) 
				return;

			WSHttpBinding objWsHttpBinding = new WSHttpBinding
			{
				OpenTimeout = new TimeSpan(0, 1, 0),
				ReceiveTimeout = new TimeSpan(0, 1, 0),
				SendTimeout = new TimeSpan(0, 1, 0),
				BypassProxyOnLocal = false,
				TransactionFlow = false,
				HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
				MaxBufferPoolSize = 2147483647,
				MaxReceivedMessageSize = 2147483647,
				MessageEncoding = WSMessageEncoding.Text,
				TextEncoding = Encoding.UTF8,
				UseDefaultWebProxy = true,
				AllowCookies = false,
				ReaderQuotas = new XmlDictionaryReaderQuotas
				{
					MaxArrayLength = 2147483647,
					MaxBytesPerRead = 2147483647,
					MaxDepth = 2147483647,
					MaxNameTableCharCount = 2147483647,
					MaxStringContentLength = 2147483647
				},
				ReliableSession = new OptionalReliableSession
				{
					Ordered = true,
					InactivityTimeout = new TimeSpan(0, 1, 0),
					Enabled = true
				},
				Security = new WSHttpSecurity
				{
					Mode = SecurityMode.None
				}
			};


			_objHttpFactory = new ChannelFactory<IBuildTaskService>(objWsHttpBinding, objEndpointAddress);
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
	    /// <param name="strReleaseManager">optional emailcontact after test execution</param>
	    /// <param name="strTestsuite">the testsuite to test</param>
	    public void SendFile(string strFilename, string strTestsystem, string strReleaseManager = null, string strTestsuite = null)
		{

			using (FileStream objFileStream = new FileStream(strFilename, FileMode.Open))
			{
				byte[] arrBuffer = new byte[52428800];
				int intSize = objFileStream.Read(arrBuffer, 0, 52428800);
				byte[] arrBufferShort = arrBuffer.Take(intSize).ToArray();
				_objChannel.SendTestcaseFile(strTestsystem, arrBufferShort);
			}

			_objChannel.AddRegTestTasks(strTestsystem, strReleaseManager, strTestsuite);
		}

	}
}
