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
		/// <param name="name">The name</param>
		/// <returns>A Testsuite</returns>
		Testsuite GetByName(string name);

		/// <summary>
		/// Set the testcases for a testsuite
		/// </summary>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="testcases">the testcases</param>
		void SetTestcasesForTestsuite(int testsuiteId, ICollection<int> testcases);

		/// <summary>
		/// Set the browsers for a testsuite
		/// </summary>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="browsers">the browsers</param>
		void SetBrowsersForTestsuite(int testsuiteId, ICollection<int> browsers);

		/// <summary>
		/// Set the languages for a testsuite
		/// </summary>
		/// <param name="testsuiteId">the testsuite</param>
		/// <param name="languages">the languages</param>
		void SetLanguagesForTestsuite(int testsuiteId, ICollection<int> languages);
	}
}
