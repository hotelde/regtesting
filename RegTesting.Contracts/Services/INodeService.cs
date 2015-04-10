using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the nodeService
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/nodeservice", ConfigurationName = "INodeService")]
	public interface INodeService
	{
		/// <summary>
		/// register at the server
		/// </summary>
		/// <param name="nodeName">the node name</param>
		/// <param name="browsers">the supported browsers</param>
		/// <returns>a bool with success flag</returns>
		[OperationContract]
		void Register(string nodeName, List<string> browsers);

		/// <summary>
		/// get a workItem
		/// </summary>
		/// <param name="nodeName">the node name</param>
		/// <returns>a workItem</returns>
		[OperationContract]
		WorkItemDto GetWork(string nodeName);

		/// <summary>
		/// report the results of a finished test
		/// </summary>
		/// <param name="nodeName">the node name</param>
		/// <param name="testResult">the testresults</param>
		[OperationContract]
		void FinishedWork(string nodeName, TestResult testResult);
		/// <summary>
		/// get the dll for a branch
		/// </summary>
		/// <param name="nodeName">the node name</param>
		/// <param name="branchName">the branch</param>
		/// <returns>the testdll as byte array</returns>
		[OperationContract]
		byte[] FetchDLL(string nodeName, string branchName);
	}
}
