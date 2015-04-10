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
		/// <param name="testsystem">the testsystem</param>
		/// <param name="testcase">the testcase</param>
		/// <param name="browser">the browser</param>
		/// <param name="language">the language</param>
		/// <returns>a result</returns>
		Result Get(Testsystem testsystem, Testcase testcase, Browser browser, Language language);

		/// <summary>
		/// Get the Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="testsystemId">the Testsystem to get</param>
		/// <param name="browsers">the list of browsers</param>
		/// <param name="testcases">the list of testcases</param>
		/// <param name="languages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> GetListOfResults(int testsystemId, IList<Browser> browsers, IList<Testcase> testcases, IList<Language> languages);

		/// <summary>
		/// Get the error Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="testsystemId">the Testsystem to get</param>
		/// <param name="browsers">the list of browsers</param>
		/// <param name="testcases">the list of testcases</param>
		/// <param name="languages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> GetErrorResultsOfTestsuite(int testsystemId, IList<Browser> browsers, IList<Testcase> testcases, IList<Language> languages);

		/// <summary>
		/// store the result
		/// </summary>
		/// <param name="result">the result object</param>
		new void Store(Result result);
	}
}
