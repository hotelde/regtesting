using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a TesterRepository
	/// </summary>
	public interface ITesterRepository : IRepository<Tester>
	{

		/// <summary>
		/// Get a Tester by Name
		/// </summary>
		/// <param name="testerName">The name</param>
		/// <returns>A Tester</returns>
		Tester GetByName(string testerName);
	}
}
