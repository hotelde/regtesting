using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// A workItem
	/// </summary>
	public class WorkItem
	{
		/// <summary>
		/// The Browser
		/// </summary>
		public Browser Browser { get; set; }

		/// <summary>
		/// The language
		/// </summary>
		public Language Language { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public Testsystem Testsystem { get; set; }

		/// <summary>
		/// The Testcase
		/// </summary>
		public Testcase Testcase { get; set; }

		/// <summary>
		/// A list of testjobs, containing this workItem
		/// </summary>
		public List<ITestJobManager> TestJobManagers { get; private set; }

		/// <summary>
		/// The current testState
		/// </summary>
		public TestState TestState { get; set; }

		/// <summary>
		/// The current tester
		/// </summary>
		public Tester Tester { get; set; }

		/// <summary>
		/// The current tester
		/// </summary>
		public bool IsCanceled { get; set; }

		/// <summary>
		/// The result of the test
		/// </summary>
		public Result Result { get; set; }

		/// <summary>
		/// How often did we run a workItem.
		/// </summary>
		public int RunCount { get; set; }

		/// <summary>
		/// Create a workItem
		/// </summary>
		public WorkItem()
		{
			TestJobManagers = new List<ITestJobManager>();
		}

		/// <summary>
		/// Create a workItem with an initial testjob
		/// </summary>
		/// <param name="objInitialTestJob">the first assigned testJob</param>
		public WorkItem(ITestJobManager objInitialTestJob)
			: this()
		{
			AddTestJobManager(objInitialTestJob);
		}

		/// <summary>
		/// Add a testJob to TestJobs list
		/// </summary>
		/// <param name="objTestJob">the testJob</param>
		public void AddTestJobManager(ITestJobManager objTestJob)
		{
			TestJobManagers.Add(objTestJob);
		}


	}
}
