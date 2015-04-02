using System.ServiceModel;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the testManager for starting test tasks.
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/buildtaskservice", ConfigurationName = "IBuildTaskService")]
	public interface IBuildTaskService
	{
		/// <summary>
		/// send a testcase file
		/// </summary>
		/// <param name="strTestsystem">the testsystem</param>
		/// <param name="arrData">the testfiledata</param>
		[OperationContract]
		void SendTestcaseFile(string strTestsystem, byte[] arrData);
		/// <summary>
		/// add a regtest task
		/// </summary>
		/// <param name="strTestsystem">the testsystem</param>
		/// <param name="strReleaseManager">the releasemanager</param>
		/// <param name="strTestsuite">the testsuite</param>
		[OperationContract]
		void AddRegTestTasks(string strTestsystem, string strReleaseManager, string strTestsuite);
	}

}
