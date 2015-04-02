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
		/// <param name="objSession">the session</param>
		public TestsystemRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}

		Testsystem ITestsystemRepository.GetByName(string strName)
		{
				Testsystem objTestsystem = Session
					.CreateCriteria(typeof(Testsystem))
					.Add(Restrictions.Eq("Name", strName))
					.UniqueResult<Testsystem>();
				if (objTestsystem != null)
					return objTestsystem;

				objTestsystem = new Testsystem { Name = strName, Url = strName, Description = "Created at " + DateTime.Now ,LastUpdated = DateTime.Now};
				((IRepository<Testsystem>)this).Store(objTestsystem);
				return objTestsystem;
		}

	}
}
