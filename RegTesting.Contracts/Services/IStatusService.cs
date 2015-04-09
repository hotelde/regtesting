using System;
using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the testManager for starting test tasks.
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/statusservice", ConfigurationName = "IStatusService")]
    public interface IStatusService
    {

		/// <summary>
		/// Get the current testJobs
		/// </summary>
		/// <returns>Get all testJobs</returns>
		[OperationContract]
		IList<TestJobManagerDto> GetTestJobs();

		/// <summary>
		/// Get the current testJobs of a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>Get all testJobs</returns>
		[OperationContract]
		IList<TestJobManagerDto> GetTestJobsForTestsystem(int testsystem);

		/// <summary>
		/// Get the current testworkers
		/// </summary>
		/// <returns>a list of all testWorkers</returns>
		[OperationContract]
		IList<TestWorkerDto> GetTestWorkers();

		/// <summary>
		/// Priorize a testjob
		/// </summary>
		/// <param name="testjob">the testJobID</param>
		[OperationContract]
		void PrioTestJob(int testjob);

		/// <summary>
		/// Cancel a testjob
		/// </summary>
		/// <param name="testjob">the testJobID</param>
		[OperationContract]
		void CancelTestJob(int testjob);

		/// <summary>
		/// Reboot a worker
		/// </summary>
		/// <param name="node">the node</param>
		[OperationContract]
		void RebootWorker(string node);

		/// <summary>
		/// Reboot all worker
		/// </summary>
		[OperationContract]
		void RebootAllWorker();

		/// <summary>
		/// Get a message for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>the message</returns>
		[OperationContract]
		String GetMessage(int testsystem);

    }

}
