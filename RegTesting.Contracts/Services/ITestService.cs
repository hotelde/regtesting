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
		/// <param name="intTester">ID of tester</param>
		/// <param name="intTestsystem">ID of testsystem</param>
		/// <param name="intTestsuite">ID of testsuite</param>
		[OperationContract]
		void TestTestsuite(int intTester, int intTestsystem, int intTestsuite);

		/// <summary>
		/// Test a testcase
		/// </summary>
		/// <param name="intTester">ID of tester</param>
		/// <param name="intTestsystem">ID of testsystem</param>
		/// <param name="intTestsuite">ID of testsuite</param>
		/// <param name="intTestcase">ID of testcase</param>
		[OperationContract]
		void TestTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase);
		
		/// <summary>
		/// Test the failed tests of a testsuite
		/// </summary>
		/// <param name="intTester">ID of tester</param>
		/// <param name="intTestsystem">ID of testsystem</param>
		/// <param name="intTestsuite">ID of testsuite</param>
		[OperationContract]
		void TestFailedTestsOfTestsuite(int intTester, int intTestsystem, int intTestsuite);

		/// <summary>
		/// Test the failed tests of a testcase
		/// </summary>
		/// <param name="intTester">ID of tester</param>
		/// <param name="intTestsystem">ID of testsystem</param>
		/// <param name="intTestsuite">ID of testsuite</param>
		/// <param name="intTestcase">ID of testcase</param>
		[OperationContract]
		void TestFailedTestsOfTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase);
    }

}
