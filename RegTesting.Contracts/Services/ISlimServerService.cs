using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the testManager for starting test tasks.
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/slimserverservice", ConfigurationName = "ISlimServerService")]
	public interface ISlimServerService
	{
		/// <summary>
		/// send a testcase file
		/// </summary>
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="data">the testfiledata</param>
		[OperationContract]
		Guid AddTestJob(string testsystemName, byte[] data);


		[OperationContract]
		bool IsTestJobFinished(Guid guid);

		[OperationContract]
		string GetResultFile(Guid testJobId);
	}

}
