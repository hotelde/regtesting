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
		/// <param name="entity">Result to remove</param>
		void Remove(T entity);

		/// <summary>
		/// Get a entity by ID
		/// </summary>
		/// <param name="id">the ID</param>
		/// <returns>a entity</returns>
		T GetById(int id);
		/// <summary>
		/// Remove a entity by ID
		/// </summary>
		/// <param name="id">the ID</param>
		void RemoveById(int id);

		/// <summary>
		/// Store a entity
		/// </summary>
		/// <param name="entity">the entity to store</param>
		void Store(T entity);
		
		/// <summary>
		/// Stores entities
		/// </summary>
		/// <param name="entities">an enumerable of entities</param>
		void Store(IEnumerable<T> entities);
	}
}
