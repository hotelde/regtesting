using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a TestsystemRepository
	/// </summary>
	public interface ITestsystemRepository : IRepository<Testsystem>
	{
	
		/// <summary>
		/// Get a Testsystem by Name
		/// </summary>
		/// <param name="strName">The name</param>
		/// <returns>A Testsystem</returns>
		Testsystem GetByName(string strName);

	}
}
