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
		private readonly INodeService _objChannel;
		private readonly ChannelFactory<INodeService> _objHttpFactory;


		/// <summary>
		/// Called once on applicationstart to initialize WCFService-Client
		/// </summary>
		/// <param name="strEndpointAddress">The endpointAdress to connect to</param>
		public WcfClient(String strEndpointAddress)
		{

			EndpointAddress objEndpointAddress = new EndpointAddress(strEndpointAddress);
			if (_objChannel != null) return;

			_objHttpFactory = new ChannelFactory<INodeService>("NodeServiceEndpoint", objEndpointAddress);
			_objChannel = _objHttpFactory.CreateChannel();

		}

		/// <summary>
		/// Dispose method for BusinessDelegate class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_objHttpFactory != null)
			{
				try
				{
					_objHttpFactory.Close();
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
		/// <param name="strNode">the node</param>
		/// <param name="lstBrowsers">the list of supported browsers</param>
		/// <returns>true if successful</returns>
        public void Register(string strNode, List<string> lstBrowsers)
		{
			_objChannel.Register(strNode, lstBrowsers);
		}

		/// <summary>
		/// Get a WorkItem from server.
		/// </summary>
		/// <param name="strNode">the node</param>
		/// <returns>A workItem for tests or null if no work is available</returns>
		public WorkItemDto GetWork(string strNode)
		{
			return _objChannel.GetWork(strNode);
		}

	    /// <summary>
	    /// The node has finished some WorkItem
	    /// </summary>
	    /// <param name="strNode">the node</param>
	    /// <param name="objTestResult">The results of the testrun</param>
	    public void FinishedWork(string strNode, TestResult objTestResult)
        {
            _objChannel.FinishedWork(strNode, objTestResult);
        }


		/// <summary>
		/// Get the testdll for a branch
		/// </summary>
		/// <param name="strNode">the node</param>
		/// <param name="strBranch">the specific branch</param>
		/// <returns>The Testdll code</returns>
		public byte[] FetchDll(string strNode, string strBranch)
		{
			return _objChannel.FetchDLL(strNode, strBranch);
		}

	}
}
