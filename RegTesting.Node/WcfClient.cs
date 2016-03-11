using System;
using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;

namespace RegTesting.Node
{
	/// <summary>
	/// The WCFClient
	/// </summary>
	public class WcfClient : IDisposable
	{
		private readonly INodeService _channel;
		private readonly ChannelFactory<INodeService> _httpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		/// <param name="strEndpointAddress">The endpointAdress to connect to</param>
		public WcfClient(String strEndpointAddress)
		{

			EndpointAddress endpointAddress = new EndpointAddress(strEndpointAddress);
			if (_channel != null) return;

			_httpFactory = new ChannelFactory<INodeService>("NodeServiceEndpoint", endpointAddress);
			_channel = _httpFactory.CreateChannel();

		}

		/// <summary>
		/// Dispose method for BusinessDelegate class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_httpFactory != null)
			{
				try
				{
					_httpFactory.Close();
				}
				catch
				{
					//Suppress possible errors while closing.
				}

			}
		}




		/// <summary>
		/// Register a node at the server
		/// </summary>
		/// <param name="nodeName">the node</param>
		/// <param name="browsers">the list of supported browsers</param>
		/// <returns>true if successful</returns>
		public void Register(string nodeName, List<string> browsers)
		{
			_channel.Register(nodeName, browsers);
		}

		/// <summary>
		/// Get a WorkItem from server.
		/// </summary>
		/// <param name="nodeName">the node</param>
		/// <returns>A workItem for tests or null if no work is available</returns>
		public WorkItem GetWork(string nodeName)
		{
			return _channel.GetWork(nodeName);
		}

		/// <summary>
		/// The node has finished some WorkItem
		/// </summary>
		/// <param name="nodeName">the node</param>
		/// <param name="testResult">The results of the testrun</param>
		public void FinishedWork(string nodeName, TestResult testResult)
		{
			_channel.FinishedWork(nodeName, testResult);
		}


		/// <summary>
		/// Get the testdll for a branch
		/// </summary>
		/// <param name="nodeName">the node</param>
		/// <param name="branchName">the specific branch</param>
		/// <returns>The Testdll code</returns>
		public byte[] FetchDll(string nodeName, string branchName)
		{
			return _channel.FetchDLL(nodeName, branchName);
		}

	}
}
