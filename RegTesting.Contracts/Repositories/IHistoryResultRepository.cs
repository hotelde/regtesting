using System;
using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// The interface for the result repository
	/// </summary>
	public interface IHistoryResultRepository : IRepository<HistoryResult>
	{
	
		/// <summary>
		/// Get all results of a testsuite
		/// </summary>
		/// <param name="testJobId">the testjob ID</param>
		/// <param name="testsystemId">the testsystem ID</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetHistoryResultsOfTestJob(int testJobId, int testsystemId);

		/// <summary>
		/// Get history results for specific browsers, testcases, languages
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="browsers">the browsers</param>
		/// <param name="testcases">the testcases</param>
		/// <param name="languages">the languages</param>
		/// <param name="maxResults">maximum count of results</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetListOfHistoryResults(int testsystemId, IEnumerable<Browser> browsers, IEnumerable<Testcase> testcases, IEnumerable<Language> languages, int maxResults);


		/// <summary>
		/// store the historyResult
		/// </summary>
		/// <param name="historyResult">the historyResult object</param>
		new void Store(HistoryResult historyResult);

		/// <summary>
		/// Get error history results for specific browsers, testcases, languages
		/// </summary>
		/// <param name="testsystemId">the testsystem</param>
		/// <param name="browsers">the browsers</param>
		/// <param name="testcases">the testcases</param>
		/// <param name="languages">the languages</param>
		/// <param name="fromDate">from date</param>
		/// <param name="toDate">to date</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetListOfErrorHistoryResults(int testsystemId, IList<Browser> browsers, IList<Testcase> testcases, IList<Language> languages, DateTime fromDate, DateTime toDate);

		/// <summary>
		/// Gets the list of error history results.
		/// </summary>
		/// <param name="testjob">The testjob.</param>
		/// <returns></returns>
		IList<HistoryResult> GetListOfErrorHistoryResults(int testjob);

	
	}
}
