﻿using System;
using System.Collections.Generic;
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
		/// <param name="objSession">the session</param>
		public TestJobRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}


		IList<TestJob> ITestJobRepository.GetTestJobsForTestsuiteOnTestsystem(int intTestsystemID, int intTestsuiteID)
		{
			IList<TestJob> objTestsuite = Session
				.CreateCriteria(typeof(TestJob))
				.Add(Restrictions.Eq("Testsystem", intTestsystemID))
				.Add(Restrictions.Eq("Testsuite", intTestsuiteID))
				.List<TestJob>();
			return objTestsuite;
		}
	}
}
