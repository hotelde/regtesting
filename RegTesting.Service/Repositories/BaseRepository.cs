using System;
using System.Collections.Generic;
using NHibernate;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// The baseRepository
	/// </summary>
	/// <typeparam name="TEntity">the entity</typeparam>
	public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{

		
		/// <summary>
		/// the session
		/// </summary>
		protected ISession Session
		{
			get
			{
				return _session();
			}
		}
		private readonly Func<ISession> _session;

		/// <summary>
		/// Create a new BaseRepostitory
		/// </summary>
		/// <param name="session">the func for the session</param>
		protected BaseRepository(Func<ISession> session)
		{
			_session = session;
			if (session == null)
					throw new ArgumentNullException("session");
			_session = session;
		}



		IList<TEntity> IRepository<TEntity>.GetAll()
		{
			IList<TEntity> lstTestsuite = Session.CreateCriteria(typeof(TEntity)).List<TEntity>();
			return lstTestsuite;
		}

		TEntity IRepository<TEntity>.GetById(int id)
		{
			return Session.Get<TEntity>(id);
		}

		void IRepository<TEntity>.Store(TEntity entity)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Session.SaveOrUpdate(entity);
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.Store(IEnumerable<TEntity> entities)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				foreach (TEntity entity in entities)
				{
					Session.SaveOrUpdate(entity);
				}
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.Remove(TEntity entity)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Session.Delete(entity);
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.RemoveById(int id)
		{
			using (ITransaction transaction = Session.BeginTransaction())
			{
				Session.Delete(Session.Load<TEntity>(id));
				transaction.Commit();
			}
		}
	}
}
