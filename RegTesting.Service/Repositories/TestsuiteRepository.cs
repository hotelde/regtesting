using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Client;
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
		/// <param name="objSession">the session</param>
		public TestsuiteRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}

		Testsuite ITestsuiteRepository.GetByName(string strName)
		{
			Testsuite objTestsuite = Session
					.CreateCriteria(typeof(Testsuite))
                    .Add(Restrictions.Eq("Name", strName))
					.UniqueResult<Testsuite>();

			if (objTestsuite != null)
				return objTestsuite;

			objTestsuite=new Testsuite {Name = strName, Description = "Auto Added"};
			((IRepository<Testsuite>) this).Store(objTestsuite);
			return objTestsuite;
		}

		void ITestsuiteRepository.SetTestcasesForTestsuite(int intTestsuite, ICollection<int> colTestcases)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Testsuite objTestsuite = Session.Get<Testsuite>(intTestsuite);
				objTestsuite.Testcases = Session.Query<Testcase>().Where(t => colTestcases.Contains(t.ID)).ToList();
				objTransaction.Commit();
			}
		}

		void ITestsuiteRepository.SetBrowsersForTestsuite(int intTestsuite, ICollection<int> colBrowsers)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Testsuite objTestsuite = Session.Get<Testsuite>(intTestsuite);
				objTestsuite.Browsers = Session.Query<Browser>().Where(t => colBrowsers.Contains(t.ID)).ToList();
				objTransaction.Commit();
			}
		}

		void ITestsuiteRepository.SetLanguagesForTestsuite(int intTestsuite, ICollection<int> colLanguages)
		{
			using (ITransaction objTransaction = Session.BeginTransaction())
			{
				Testsuite objTestsuite = Session.Get<Testsuite>(intTestsuite);
				objTestsuite.Languages = Session.Query<Language>().Where(t => colLanguages.Contains(t.ID)).ToList();
				objTransaction.Commit();
			}
		}

		
		IList<Testsuite> IRepository<Testsuite>.GetAll()
		{
			IList<Testsuite> lstTestsuite = Session.CreateCriteria(typeof(Testsuite)).AddOrder(Order.Asc("Name")).List<Testsuite>();
			return lstTestsuite;
		}
	}
}
