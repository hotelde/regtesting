using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// Interface for a LanguageRepository
	/// </summary>
	public interface ILanguageRepository : IRepository<Language>
	{
		/// <summary>
		/// Get a Language by LanguageCode
		/// </summary>
		/// <param name="strCode">The languageCode</param>
		/// <returns>A Language</returns>
		Language GetByLanguageCode(string strCode);
	}
}
