using System;
using System.Collections.Generic;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// Interface for viewing testresults
	/// </summary>
	public interface ITestViewerService
	{
		/// <summary>
		/// Get all available testsystems
		/// </summary>
		/// <returns>a list of testsystems</returns>
		IList<TestsystemDto> GetTestsystems();


		/// <summary>
		/// Get all testsuites for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>a list of testsuites</returns>
		IList<TestsuiteDto> GetTestSuites(int testsystem);

		/// <summary>
		/// Get all results
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="datResultsSince">the results since</param>
		/// <returns>a list of grouped results (grouped by testcase and different errors)</returns>
		IList<GroupedResult> GetResults(int intTestsystem, int intTestsuite, DateTime? datResultsSince);

		/// <summary>
		/// Get the details for a testcase
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="intTestcase">the testcase</param>
		/// <returns>the details for the testcase</returns>
		TestcaseDetailsModel GetTestcaseDetails(int intTestsystem, int intTestsuite, int intTestcase);

		/// <summary>
		/// Get the id of the tester by name
		/// </summary>
		/// <param name="strName">the name</param>
		/// <returns>a tester id</returns>
		int GetTesterIDByName(string strName);

		/// <summary>
		/// Get the grouped errors for a testsuite
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="intTestsuite">the testsuite</param>
		/// <returns>a list of errorOccurrenceGroups</returns>
		IList<ErrorOccurrenceGroup> GetCurrentErrorOccurrenceGroups(int intTestsystem, int intTestsuite);

		/// <summary>
		/// Get the grouped old errors for a testsuite
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="datFromDateTime">the date from where to get the errors</param>
		/// <param name="datToDateTime">the date until when to get the errors</param>
		/// <returns>a list of errorOccurrenceGroups</returns>
		IList<ErrorOccurrenceGroup> GetHistoryErrorOccurrenceGroups(int intTestsystem, int intTestsuite, DateTime datFromDateTime, DateTime datToDateTime);

		/// <summary>
		/// Get old results
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="intTestcase">the testcase</param>
		/// <param name="intBrowser">the browser</param>
		/// <param name="intLanguage">the language</param>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="intMaxResults">the maximum count of results</param>
		/// <returns>a list of historyresults</returns>
		IList<HistoryResult> GetResultsHistory(int intTestsystem, int intTestcase, int intBrowser, int intLanguage, int intTestsuite, int intMaxResults);
	}
}
