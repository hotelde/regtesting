using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// The interface for the result repository
	/// </summary>
	public interface IErrorRepository : IRepository<Error>
	{


		/// <summary>
		/// Get a error, if it is already existing. Else add it.
		/// </summary>
		/// <param name="error">the error</param>
		/// <returns>a error</returns>
		Error GetOrStore(Error error);

		/// <summary>
		/// Get a matching error that is already existing in database
		/// </summary>
		/// <param name="error">the error</param>
		/// <returns>the found error</returns>
		Error GetByError(Error error);
	}
}
