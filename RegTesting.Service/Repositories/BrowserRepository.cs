using System;
using NHibernate;
using NHibernate.Criterion;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// TestsuiteRepository
	/// </summary>
	public class BrowserRepository : BaseRepository<Browser>, IBrowserRepository
	{

		/// <summary>
		/// Create a TestsuiteRepository
		/// </summary>
		/// <param name="objSession">the session</param>
		public BrowserRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}


		Browser IBrowserRepository.GetByName(string strName)
		{
			return Session
					.CreateCriteria(typeof(Browser))
					.Add(Restrictions.Eq("Name", strName))
					.UniqueResult<Browser>();
		}
	}
}
