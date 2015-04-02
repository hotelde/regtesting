using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// Interface for a testjobManager
	/// </summary>
	public interface ITestJobManager
	{
		/// <summary>
		/// The ID
		/// </summary>
		int ID { get; }

		/// <summary>
		/// The TestJob
		/// </summary>
		TestJob TestJob { get; set; }

		/// <summary>
		/// The workItems
		/// </summary>
		IList<WorkItem> WorkItems { get; set; }

		/// <summary>
		/// The Count of workitems
		/// </summary>
		int Count { get; }

		/// <summary>
		/// The count of failured workItems
		/// </summary>
		int Failured { get; }

		/// <summary>
		/// The count of passed workItems
		/// </summary>
		int Passed { get; }

		/// <summary>
		/// The count of passed workItems
		/// </summary>
		int NotSupported { get; }

		/// <summary>
		/// The canceled workItems
		/// </summary>
		int Canceled { get; }

		/// <summary>
		/// The count of finished workItems
		/// </summary>
		int Finished { get; }

		/// <summary>
		/// flag if the testJob is canceled 
		/// </summary>
		bool IsCanceled { get; set; }

		/// <summary>
		/// Add a workItem
		/// </summary>
		/// <param name="workItem">the workItem</param>
		void AddWorkItem(WorkItem workItem);

		/// <summary>
		/// Indicates that the testJob is finished
		/// </summary>
		/// <returns>a bool, if the testjob is finished</returns>
		bool IsFinished();
	}
}