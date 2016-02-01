namespace RegTesting.SlimServer.Wcf
{
	/// <summary>
	/// the hostService interface
	/// </summary>
	/// <typeparam name="TEntity">the entity to host</typeparam>
	public interface IHostService<TEntity>
	{
		/// <summary>
		/// init the hosted service
		/// </summary>
		/// <param name="entity">the entity</param>
		void Init(TEntity entity);

	}
}
