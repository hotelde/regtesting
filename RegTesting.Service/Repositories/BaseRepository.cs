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
				return _objSession();
			}
		}
		private readonly Func<ISession> _objSession;

		/// <summary>
		/// Create a new BaseRepostitory
		/// </summary>
		/// <param name="objSession">the func for the session</param>
		protected BaseRepository(Func<ISession> objSession)
		{
			_objSession = objSession;
			if (objSession == null)
					throw new ArgumentNullException("objSession");
			_objSession = objSession;
		}



		IList<TEntity> IRepository<TEntity>.GetAll()
		{
			IList<TEntity> lstTestsuite = Session.CreateCriteria(typeof(TEntity)).List<TEntity>();
			return lstTestsuite;
		}

		TEntity IRepository<TEntity>.GetById(int intID)
		{
			return Session.Get<TEntity>(intID);
		}

		void IRepository<TEntity>.Store(TEntity objEntity)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Session.SaveOrUpdate(objEntity);
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.Store(IEnumerable<TEntity> objEntities)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				foreach (TEntity objEntity in objEntities)
				{
					Session.SaveOrUpdate(objEntity);
				}
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.Remove(TEntity objTestsuite)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Session.Delete(objTestsuite);
				objTransaction.Commit();
			}
		}

		void IRepository<TEntity>.RemoveById(int intID)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Session.Delete(Session.Load<TEntity>(intID));
				objTransaction.Commit();
			}
		}
	}
}
