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
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="resultsSince">the results since</param>
		/// <returns>a list of grouped results (grouped by testcase and different errors)</returns>
		IList<GroupedResult> GetResults(int testsystemId, int testsuiteId, DateTime? resultsSince);

		/// <summary>
		/// Get the details for a testcase
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="testcaseId">the testcase</param>
		/// <returns>the details for the testcase</returns>
		TestcaseDetailsModel GetTestcaseDetails(int testsystemId, int testsuiteId, int testcaseId);

		/// <summary>
		/// Get the id of the tester by name
		/// </summary>
		/// <param name="strName">the name</param>
		/// <returns>a tester id</returns>
		int GetTesterIDByName(string strName);

		/// <summary>
		/// Get the grouped errors for a testsuite
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="testsuiteId">the testsuite</param>
		/// <returns>a list of errorOccurrenceGroups</returns>
		IList<ErrorOccurrenceGroup> GetCurrentErrorOccurrenceGroups(int testsystemId, int testsuiteId);

		/// <summary>
		/// Get the grouped old errors for a testsuite
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="fromDate">the date from where to get the errors</param>
		/// <param name="toDate">the date until when to get the errors</param>
		/// <returns>a list of errorOccurrenceGroups</returns>
		IList<ErrorOccurrenceGroup> GetHistoryErrorOccurrenceGroups(int testsystemId, int testsuiteId, DateTime fromDate, DateTime toDate);

		/// <summary>
		/// Get old results
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="testcaseId">the testcase</param>
		/// <param name="browserId">the browser</param>
		/// <param name="languageId">the language</param>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="maxResults">the maximum count of results</param>
		/// <returns>a list of historyresults</returns>
		IList<HistoryResult> GetResultsHistory(int testsystemId, int testcaseId, int browserId, int languageId, int testsuiteId, int maxResults);

		/// <summary>
		/// Gets the error occurrence groups for a testjob.
		/// </summary>
		/// <param name="testjobId">The testjob identifier.</param>
		/// <returns></returns>
		IList<ErrorOccurrenceGroup> GetErrorOccurrenceGroupsForTestjob(int testjobId);
	}
}
