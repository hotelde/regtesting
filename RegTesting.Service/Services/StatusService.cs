using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using NHibernate.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Cache;
using RegTesting.Service.Repositories;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// The statusService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class StatusService : IStatusService
	{
		private readonly ITestPool _testPool;
		private readonly ITestsystemRepository _testsystemRepository;

		/// <summary>
		/// Create a new statusService
		/// </summary>
		/// <param name="testPool">the testPool</param>
		/// <param name="testsystemRepository">the testsystemRepository</param>
		public StatusService(ITestPool testPool, ITestsystemRepository testsystemRepository)
		{
			if (testPool == null)
				throw new ArgumentNullException("testPool");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");

			_testPool = testPool;
			_testsystemRepository = testsystemRepository;
		}


		IList<TestJobDto> IStatusService.GetTestJobs()
		{
			return _testPool.GetTestJobs();

		}


		IList<TestJobDto> IStatusService.GetTestJobsForTestsystem(int testsystemId)
		{
			return _testPool.GetTestJobs(testsystemId);

		}

		IList<TestWorkerDto> IStatusService.GetTestWorkers()
		{
			return _testPool.GetTestWorker().Select(Mapper.Map<TestWorkerDto>).OrderBy(t => t.Name).ToList();
		}

		void IStatusService.PrioTestJob(int testjob)
		{
			_testPool.PrioTestJob(testjob);
		}

		void IStatusService.CancelTestJob(int testjob)
		{
			_testPool.CancelTestJob(testjob);
		}

		void IStatusService.RebootWorker(string node)
		{
			_testPool.GetTestWorker(node).RebootWorker();
		}

		void IStatusService.RebootAllWorker()
		{
			_testPool.GetTestWorker().ForEach(w => w.RebootWorker());
		}


		string IStatusService.GetMessage(int testsystemId)
		{
			Testsystem testsystem = _testsystemRepository.GetById(testsystemId);

			String deployname = Tfs.TfsBuildQuery.GetDeploymentName(testsystem);
			bool isDeploymentRunning = Tfs.TfsBuildQuery.IsDeploymentRunning(deployname);
			if (isDeploymentRunning)
			{
				return "<b>" + deployname + " is running</b>. Testing task starts after build is finished.";
			}
			return "";
		}

	}
}
