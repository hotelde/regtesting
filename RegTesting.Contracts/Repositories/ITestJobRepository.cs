using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.Repositories
{
	/// <summary>
	/// The TestJobRepository Interface
	/// </summary>
	public interface ITestJobRepository : IRepository<TestJob>
	{
		/// <summary>
		/// Get the testjobs for a testsuite on a testsystem
		/// </summary>
		/// <param name="testsystemId">The testsystem ID</param>
		/// <param name="testsuiteId">The testsuite ID</param>
		/// <returns>A Testsuite</returns>
		IList<TestJob> GetTestJobsForTestsuiteOnTestsystem(int testsystemId, int testsuiteId);
	}

}
