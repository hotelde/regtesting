using System.ServiceModel;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the testManager for starting test tasks.
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/testservice", ConfigurationName = "ITestService")]
	public interface ITestService
	{

		/// <summary>
		/// Test a testsuite
		/// </summary>
		/// <param name="testerId">ID of tester</param>
		/// <param name="testsystemId">ID of testsystem</param>
		/// <param name="testsuiteId">ID of testsuite</param>
		[OperationContract]
		void TestTestsuite(int testerId, int testsystemId, int testsuiteId);

		/// <summary>
		/// Test a testcase
		/// </summary>
		/// <param name="testerId">ID of tester</param>
		/// <param name="testsystemId">ID of testsystem</param>
		/// <param name="testsuiteId">ID of testsuite</param>
		/// <param name="testcaseId">ID of testcase</param>
		[OperationContract]
		void TestTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId);
		
		/// <summary>
		/// Test the failed tests of a testsuite
		/// </summary>
		/// <param name="testerId">ID of tester</param>
		/// <param name="testsystemId">ID of testsystem</param>
		/// <param name="testsuiteId">ID of testsuite</param>
		[OperationContract]
		void TestFailedTestsOfTestsuite(int testerId, int testsystemId, int testsuiteId);

		/// <summary>
		/// Test the failed tests of a testcase
		/// </summary>
		/// <param name="testerId">ID of tester</param>
		/// <param name="testsystemId">ID of testsystem</param>
		/// <param name="testsuiteId">ID of testsuite</param>
		/// <param name="testcaseId">ID of testcase</param>
		[OperationContract]
		void TestFailedTestsOfTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId);
	}

}
