using System.Collections.Generic;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// A Testpool
	/// </summary>
	public interface ITestPool
	{
		/// <summary>
		/// Add workItems to the testpool
		/// </summary>
		/// <param name="objTestJob">the testJob</param>
		/// <param name="objWorkItems">the workItems</param>
		void AddTestJob(ITestJobManager objTestJob, ICollection<WorkItem> objWorkItems);

		/// <summary>
		/// Get a workItem of the testpool
		/// </summary>
		/// <param name="objTestWorker">the testWorker which wants a workItem</param>
		/// <returns>a workItem</returns>
		WorkItem GetWorkItem(ITestWorker objTestWorker);

		/// <summary>
		/// A workItem is finished
		/// </summary>
		/// <param name="objWorkItem">the workItem</param>
		void WorkItemFinished(WorkItem objWorkItem);

		/// <summary>
		/// Get the testJobs
		/// </summary>
		/// <returns>a list of testJobs</returns>
		IList<TestJobDto> GetTestJobs();

		/// <summary>
		/// Register a testworker to list
		/// </summary>
		/// <param name="objTestWorker">the testWorker</param>
		void RegisterTestWorker(ITestWorker objTestWorker);

		/// <summary>
		/// Remove a testworker from list
		/// </summary>
		/// <param name="objTestWorker">the testWorker</param>
		void RemoveTestWorker(ITestWorker objTestWorker);

		/// <summary>
		/// Get the testworker list
		/// </summary>
		/// <returns>a list of the current testworkers</returns>
		IList<ITestWorker> GetTestWorker();

		/// <summary>
		/// Get a testworker by name
		/// </summary>
		/// <param name="strNode">the nodeName</param>
		/// <returns>a testworker by Name</returns>
		ITestWorker GetTestWorker(string strNode);

		/// <summary>
		/// priorize a testjob
		/// </summary>
		/// <param name="testjob">the testjobID</param>
		void PrioTestJob(int testjob);

		/// <summary>
		/// cancel a testjob
		/// </summary>
		/// <param name="testjob">the testjobID</param>
		void CancelTestJob(int testjob);

		/// <summary>
		///  Get the testJobs for a testsystem
		/// </summary>
		/// <param name="intTestsystem">the testsystem</param>
		/// <returns>a list of testJobs</returns>
		IList<TestJobDto> GetTestJobs(int intTestsystem);

		/// <summary>
		/// Readd a workItem
		/// </summary>
		/// <param name="workItem">the workItem</param>
		void ReAddWorkItem(WorkItem workItem);
	}
}
