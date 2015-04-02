using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a TestsuiteRepository
	/// </summary>
	public interface ITestsuiteRepository : IRepository<Testsuite>
	{

		/// <summary>
		/// Get a Testsuite by Name
		/// </summary>
		/// <param name="strName">The name</param>
		/// <returns>A Testsuite</returns>
		Testsuite GetByName(string strName);

		/// <summary>
		/// Set the testcases for a testsuite
		/// </summary>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="colTestcases">the testcases</param>
		void SetTestcasesForTestsuite(int intTestsuite, ICollection<int> colTestcases);

		/// <summary>
		/// Set the browsers for a testsuite
		/// </summary>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="colBrowsers">the browsers</param>
		void SetBrowsersForTestsuite(int intTestsuite, ICollection<int> colBrowsers);

		/// <summary>
		/// Set the languages for a testsuite
		/// </summary>
		/// <param name="intTestsuite">the testsuite</param>
		/// <param name="colLanguages">the languages</param>
		void SetLanguagesForTestsuite(int intTestsuite, ICollection<int> colLanguages);
	}
}
