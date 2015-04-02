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
		/// <param name="strNode">the node name</param>
		/// <param name="lstBrowsers">the supported browsers</param>
		/// <returns>a bool with success flag</returns>
		[OperationContract]
		void Register(string strNode, List<string> lstBrowsers);

		/// <summary>
		/// get a workItem
		/// </summary>
		/// <param name="strNode">the node name</param>
		/// <returns>a workItem</returns>
		[OperationContract]
		WorkItemDto GetWork(string strNode);

		/// <summary>
		/// report the results of a finished test
		/// </summary>
		/// <param name="strNode">the node name</param>
		/// <param name="objTestResult">the testresults</param>
		[OperationContract]
		void FinishedWork(string strNode, TestResult objTestResult);
		/// <summary>
		/// get the dll for a branch
		/// </summary>
		/// <param name="strNode">the node name</param>
		/// <param name="strBranch">the branch</param>
		/// <returns>the testdll as byte array</returns>
		[OperationContract]
		byte[] FetchDLL(string strNode, string strBranch);
	}
}
