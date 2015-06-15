using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{

	/// <summary>
	/// The TestJobRepository
	/// </summary>
	public class TestJobRepository : BaseRepository<TestJob>, ITestJobRepository
	{

		/// <summary>
		/// Create a TestJobRepository
		/// </summary>
		/// <param name="session">the session</param>
		public TestJobRepository(Func<ISession> session)
			: base(session)
		{
		}


		IList<TestJob> ITestJobRepository.GetTestJobsForTestsuiteOnTestsystem(int testsystemId, int testsuiteId)
		{
			IList<TestJob> testsuite = Session
				.CreateCriteria(typeof(TestJob))
				.Add(Restrictions.Eq("Testsystem", testsystemId))
				.Add(Restrictions.Eq("Testsuite", testsuiteId))
				.List<TestJob>();
			return testsuite;
		}
	}
}
