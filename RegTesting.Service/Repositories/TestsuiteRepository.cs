using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// TestsuiteRepository
	/// </summary>
	public class TestsuiteRepository : BaseRepository<Testsuite>, ITestsuiteRepository
	{

		/// <summary>
		/// Create a TestsuiteRepository
		/// </summary>
		/// <param name="session">the session</param>
		public TestsuiteRepository(Func<ISession> session)
			: base(session)
		{
		}

		Testsuite ITestsuiteRepository.GetByName(string name)
		{
			Testsuite testsuite = Session
					.CreateCriteria(typeof(Testsuite))
                    .Add(Restrictions.Eq("Name", name))
					.UniqueResult<Testsuite>();

			if (testsuite != null)
				return testsuite;

			testsuite=new Testsuite {Name = name, Description = "Auto Added"};
			((IRepository<Testsuite>) this).Store(testsuite);
			return testsuite;
		}

		void ITestsuiteRepository.SetTestcasesForTestsuite(int testsuiteId, ICollection<int> testcases)
		{
			using (ITransaction transaction = Session.BeginTransaction())
			{
				Testsuite testsuite = Session.Get<Testsuite>(testsuiteId);
				testsuite.Testcases = Session.Query<Testcase>().Where(t => testcases.Contains(t.ID)).ToList();
				transaction.Commit();
			}
		}

		void ITestsuiteRepository.SetBrowsersForTestsuite(int testsuiteId, ICollection<int> browsers)
		{
			using (ITransaction transaction = Session.BeginTransaction())
			{
				Testsuite testsuite = Session.Get<Testsuite>(testsuiteId);
				testsuite.Browsers = Session.Query<Browser>().Where(t => browsers.Contains(t.ID)).ToList();
				transaction.Commit();
			}
		}

		void ITestsuiteRepository.SetLanguagesForTestsuite(int testsuiteId, ICollection<int> languages)
		{
			using (ITransaction transaction = Session.BeginTransaction())
			{
				Testsuite testsuite = Session.Get<Testsuite>(testsuiteId);
				testsuite.Languages = Session.Query<Language>().Where(t => languages.Contains(t.ID)).ToList();
				transaction.Commit();
			}
		}

		
		IList<Testsuite> IRepository<Testsuite>.GetAll()
		{
			IList<Testsuite> testsuites = Session.CreateCriteria(typeof(Testsuite)).AddOrder(Order.Asc("Name")).List<Testsuite>();
			return testsuites;
		}
	}
}
