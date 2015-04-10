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
		/// <param name="session">the session</param>
		public BrowserRepository(Func<ISession> session)
			: base(session)
		{
		}


		Browser IBrowserRepository.GetByName(string browserName)
		{
			return Session
					.CreateCriteria(typeof(Browser))
					.Add(Restrictions.Eq("Name", browserName))
					.UniqueResult<Browser>();
		}
	}
}
