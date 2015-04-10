using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a BrowserRepository
	/// </summary>
	public interface IBrowserRepository : IRepository<Browser>
	{
		/// <summary>
		/// Get a Browser by Name
		/// </summary>
		/// <param name="browserName">The name</param>
		/// <returns>A Browser</returns>
		Browser GetByName(string browserName);
	}
}
