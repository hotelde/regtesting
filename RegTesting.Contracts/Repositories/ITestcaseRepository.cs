using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a TestcaseRepository
	/// </summary>
	public interface ITestcaseRepository : IRepository<Testcase>
	{
		/// <summary>
		/// Get a Testcase by Type
		/// </summary>
		/// <param name="strName">The type</param>
		/// <returns>A Testcase</returns>
		Testcase GetByType(string strName);
	}
}
