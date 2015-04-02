using System.Collections.Generic;

namespace RegTesting.Contracts.Repositories
{


	/// <summary>
	/// The interface for a basic repository
	/// </summary>
	/// <typeparam name="T">the entity</typeparam>
	public interface IRepository<T>
	{

		/// <summary>
		/// Get all Entities
		/// </summary>
		/// <returns>A list of all Entities</returns>
		IList<T> GetAll();

		/// <summary>
		/// Remove a Result
		/// </summary>
		/// <param name="objResult">Result to remove</param>
		void Remove(T objResult);

		/// <summary>
		/// Get a entity by ID
		/// </summary>
		/// <param name="intID">the ID</param>
		/// <returns>a result</returns>
		T GetById(int intID);
		/// <summary>
		/// Remove a entity by ID
		/// </summary>
		/// <param name="intID">the ID</param>
		void RemoveById(int intID);

		/// <summary>
		/// Store a entity
		/// </summary>
		/// <param name="objEntity">the entity to store</param>
		void Store(T objEntity);
		
		/// <summary>
		/// Stores entities
		/// </summary>
		/// <param name="objEntity">an enumerable of entities</param>
		void Store(IEnumerable<T> objEntity);
	}
}
