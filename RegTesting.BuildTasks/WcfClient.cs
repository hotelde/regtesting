using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Xml;
using RegTesting.Contracts.Services;

namespace RegTesting.BuildTasks
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
		/// <param name="strEndpointAddress">The endpointAdress to connect to</param>
		public WcfClient(String strEndpointAddress)
		{
			EndpointAddress endpointAddress = new EndpointAddress(strEndpointAddress);
			if (_channel != null) 
				return;

			WSHttpBinding wsHttpBinding = new WSHttpBinding
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


			_httpFactory = new ChannelFactory<ISlimServerService>(wsHttpBinding, endpointAddress);
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
		/// <param name="testurl">The testurl</param>
		public Guid SendFile(string filename, string testurl)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Open))
			{
				byte[] buffer = new byte[52428800];
				int intSize = fileStream.Read(buffer, 0, 52428800);
				byte[] bufferShort = buffer.Take(intSize).ToArray();
				return _channel.AddTestJob(testurl, bufferShort);
			}
		}

		public void WaitForTestJobResult(Guid testJobId)
		{
			bool result;
			do
			{
				Thread.Sleep(10000);
				result = _channel.IsTestJobFinished(testJobId);
			} while (result == false);
		}

		public string GetResultFile(Guid testJobId)
		{
			return _channel.GetResultFile(testJobId);
		}
	}
}
