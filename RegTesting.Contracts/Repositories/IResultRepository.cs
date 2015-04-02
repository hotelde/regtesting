using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// The interface for the result repository
	/// </summary>
	public interface IResultRepository : IRepository<Result>
	{
	
		/// <summary>
		/// Get a single Result
		/// </summary>
		/// <param name="objTestsystem">the testsystem</param>
		/// <param name="objTestcase">the testcase</param>
		/// <param name="objBrowser">the browser</param>
		/// <param name="objLanguage">the language</param>
		/// <returns>a result</returns>
		Result Get(Testsystem objTestsystem, Testcase objTestcase, Browser objBrowser, Language objLanguage);

		/// <summary>
		/// Get the Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="intTestsystem">the Testsystem to get</param>
		/// <param name="lstBrowser">the list of browsers</param>
		/// <param name="lstTestcase">the list of testcases</param>
		/// <param name="lstLanguages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> GetListOfResults(int intTestsystem, IList<Browser> lstBrowser, IList<Testcase> lstTestcase, IList<Language> lstLanguages);

		/// <summary>
		/// Get the error Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="intTestsystem">the Testsystem to get</param>
		/// <param name="lstBrowser">the list of browsers</param>
		/// <param name="lstTestcase">the list of testcases</param>
		/// <param name="lstLanguages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> GetErrorResultsOfTestsuite(int intTestsystem, IList<Browser> lstBrowser, IList<Testcase> lstTestcase, IList<Language> lstLanguages);

		/// <summary>
		/// store the result
		/// </summary>
		/// <param name="objResult">the result object</param>
		new void Store(Result objResult);
	}
}
