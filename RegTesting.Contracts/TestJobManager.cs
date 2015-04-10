using System;
using System.Collections.Generic;
using System.Linq;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// The TestJobManager
	/// </summary>
	public class TestJobManager : ITestJobManager
	{

		/// <summary>
		/// The ID
		/// </summary>
		public int ID { get
		{
			return TestJob != null ? TestJob.ID : 0;
		}
		}

		/// <summary>
		/// The TestJob
		/// </summary>
		public TestJob TestJob { get; set; }

		/// <summary>
		/// The workItems
		/// </summary>
		public IList<WorkItem> WorkItems { get; set; }

		/// <summary>
		/// The Count of workitems
		/// </summary>
		public int Count { get { return WorkItems.Count - NotSupported - Canceled; } }

		/// <summary>
		/// The count of failured workItems
		/// </summary>
		public int Failured { get { return WorkItems.Count(t => t.TestState == TestState.Error || t.TestState == TestState.KnownError); } }

		/// <summary>
		/// The count of passed workItems
		/// </summary>
		public int Passed { get { return WorkItems.Count(t => t.TestState == TestState.Success); } }

		/// <summary>
		/// The count of passed workItems
		/// </summary>
		public int NotSupported { get { return WorkItems.Count(t => t.TestState == TestState.NotSupported); } }

		/// <summary>
		/// The canceled workItems
		/// </summary>
		public int Canceled { get { return WorkItems.Count(t => t.TestState == TestState.Canceled); } }

		/// <summary>
		/// The count of finished workItems
		/// </summary>
		public int Finished { get { return Failured + Passed; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="testjob">the testJob to manage</param>
		public TestJobManager(TestJob testjob)
		{
			if (testjob == null)
				throw new ArgumentNullException("testjob");
			TestJob = testjob;
			WorkItems = new List<WorkItem>();
		}

		/// <summary>
		/// Add a workItem
		/// </summary>
		/// <param name="workItem">the workItem</param>
		public void AddWorkItem(WorkItem workItem)
		{
			WorkItems.Add(workItem);
		}

		/// <summary>
		/// Indicates that the testJob is finished
		/// </summary>
		/// <returns>a bool, if the testjob is finished</returns>
		public bool IsFinished()
		{
			return Finished == Count;

		}

		/// <summary>
		/// flag if the testJob is canceled 
		/// </summary>
		public bool IsCanceled { get; set; }


	}
}
