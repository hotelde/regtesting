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
		/// <param name="intTestJob">the testjob ID</param>
		/// <param name="intTestsystem">the testsystem ID</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetHistoryResultsOfTestJob(int intTestJob, int intTestsystem);

		/// <summary>
		/// Get history results for specific browsers, testcases, languages
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="lstBrowsers">the browsers</param>
		/// <param name="lstTestcases">the testcases</param>
		/// <param name="lstLanguages">the languages</param>
		/// <param name="intMaxResults">maximum count of results</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetListOfHistoryResults(int intTestsystem, IEnumerable<Browser> lstBrowsers, IEnumerable<Testcase> lstTestcases, IEnumerable<Language> lstLanguages, int intMaxResults);


		/// <summary>
		/// store the historyResult
		/// </summary>
		/// <param name="objHistoryResult">the historyResult object</param>
		new void Store(HistoryResult objHistoryResult);

		/// <summary>
		/// Get error history results for specific browsers, testcases, languages
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <param name="lstBrowsers">the browsers</param>
		/// <param name="lstTestcases">the testcases</param>
		/// <param name="lstLanguages">the languages</param>
		/// <param name="datFromDateTime">from date</param>
		/// <param name="datToDateTime">to date</param>
		/// <returns>a list of historyResults</returns>
		IList<HistoryResult> GetListOfErrorHistoryResults(int intTestsystem, IList<Browser> lstBrowsers, IList<Testcase> lstTestcases, IList<Language> lstLanguages, DateTime datFromDateTime, DateTime datToDateTime);
	}
}
