using System.Collections.Generic;
using RegTesting.Contracts.DTO;

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
		/// <param name="testJobManager">the testJob</param>
		/// <param name="workItems">the workItems</param>
		void AddTestJob(ITestJobManager testJobManager, ICollection<WorkItem> workItems);

		/// <summary>
		/// Get a workItem of the testpool
		/// </summary>
		/// <param name="testWorker">the testWorker which wants a workItem</param>
		/// <returns>a workItem</returns>
		WorkItem GetWorkItem(ITestWorker testWorker);

		/// <summary>
		/// A workItem is finished
		/// </summary>
		/// <param name="workItem">the workItem</param>
		void WorkItemFinished(WorkItem workItem);

		/// <summary>
		/// Get the testJobs
		/// </summary>
		/// <returns>a list of testJobs</returns>
		IList<TestJobManagerDto> GetTestJobs();

		/// <summary>
		/// Register a testworker to list
		/// </summary>
		/// <param name="testWorker">the testWorker</param>
		void RegisterTestWorker(ITestWorker testWorker);

		/// <summary>
		/// Remove a testworker from list
		/// </summary>
		/// <param name="testWorker">the testWorker</param>
		void RemoveTestWorker(ITestWorker testWorker);

		/// <summary>
		/// Get the testworker list
		/// </summary>
		/// <returns>a list of the current testworkers</returns>
		IList<ITestWorker> GetTestWorker();

		/// <summary>
		/// Get a testworker by name
		/// </summary>
		/// <param name="nodeName">the nodeName</param>
		/// <returns>a testworker by Name</returns>
		ITestWorker GetTestWorker(string nodeName);

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
		IList<TestJobManagerDto> GetTestJobs(int intTestsystem);

		/// <summary>
		/// Readd a workItem
		/// </summary>
		/// <param name="workItem">the workItem</param>
		void ReAddWorkItem(WorkItem workItem);
	}
}
