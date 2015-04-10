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
	public class TesterRepository : BaseRepository<Tester>, ITesterRepository
	{

		/// <summary>
		/// Create a TestsuiteRepository
		/// </summary>
		/// <param name="session">the session</param>
		public TesterRepository(Func<ISession> session)
			: base(session)
		{
		}

		Tester ITesterRepository.GetByName(string strName)
		{
			Tester tester = Session
					.CreateCriteria(typeof(Tester))
                    .Add(Restrictions.Eq("Name", strName))
					.UniqueResult<Tester>();
			if (tester != null)
				return tester;

			tester = new Tester { Name = strName, Mail = strName + "@hotel.de"};
			((IRepository<Tester>)this).Store(tester);
			return tester;
		}



	}
}
