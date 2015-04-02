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
		/// <param name="objSession">the session</param>
		public TesterRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}

		Tester ITesterRepository.GetByName(string strName)
		{
			Tester objTester = Session
					.CreateCriteria(typeof(Tester))
                    .Add(Restrictions.Eq("Name", strName))
					.UniqueResult<Tester>();
			if (objTester != null)
				return objTester;

			objTester = new Tester { Name = strName, Mail = strName + "@hotel.de"};
			((IRepository<Tester>)this).Store(objTester);
			return objTester;
		}



	}
}
