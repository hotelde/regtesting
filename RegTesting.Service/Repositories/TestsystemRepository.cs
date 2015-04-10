using System;
using NHibernate;
using NHibernate.Criterion;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// The TestsystemRepository
	/// </summary>
	public class TestsystemRepository : BaseRepository<Testsystem>, ITestsystemRepository
	{

		/// <summary>
		/// Create a new TestsystemRepository
		/// </summary>
		/// <param name="session">the session</param>
		public TestsystemRepository(Func<ISession> session)
			: base(session)
		{
		}

		Testsystem ITestsystemRepository.GetByName(string testsystemName)
		{
				Testsystem testsystem = Session
					.CreateCriteria(typeof(Testsystem))
					.Add(Restrictions.Eq("Name", testsystemName))
					.UniqueResult<Testsystem>();
				if (testsystem != null)
					return testsystem;

				testsystem = new Testsystem { Name = testsystemName, Url = testsystemName, Description = "Created at " + DateTime.Now ,LastUpdated = DateTime.Now};
				((IRepository<Testsystem>)this).Store(testsystem);
				return testsystem;
		}

	}
}
