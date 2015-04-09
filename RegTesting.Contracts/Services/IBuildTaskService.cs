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
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="data">the testfiledata</param>
		[OperationContract]
		void SendTestcaseFile(string testsystemName, byte[] data);
		/// <summary>
		/// add a regtest task
		/// </summary>
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="emailReceiver">the releasemanager</param>
		/// <param name="testsuiteName">the testsuite</param>
		/// <param name="strBranch">the branch</param>
		/// <param name="strCommitId">the commitId</param>
		/// <param name="strCommitMessage">the commitMessage</param>
		[OperationContract]
		void AddRegTestTasks(string testsystemName, string emailReceiver, string testsuiteName, string branch, string commitId, string commitMessage);
	}

}
