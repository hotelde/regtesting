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
		private readonly ITestPool _objTestPool;
		private readonly ITestsystemRepository _objTestsystemRepository;

		/// <summary>
		/// Create a new statusService
		/// </summary>
		/// <param name="objTestPool">the testPool</param>
		/// <param name="objTestsystemRepository">the testsystemRepository</param>
		public StatusService(ITestPool objTestPool, ITestsystemRepository objTestsystemRepository)
		{
			if (objTestPool == null)
				throw new ArgumentNullException("objTestPool");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");

			_objTestPool = objTestPool;
			_objTestsystemRepository = objTestsystemRepository;
		}


		IList<TestJobDto> IStatusService.GetTestJobs()
		{
			return _objTestPool.GetTestJobs();

		}


		IList<TestJobDto> IStatusService.GetTestJobsForTestsystem(int intTestsystem)
		{
			return _objTestPool.GetTestJobs(intTestsystem);

		}

		IList<TestWorkerDto> IStatusService.GetTestWorkers()
		{
			return _objTestPool.GetTestWorker().Select(Mapper.Map<TestWorkerDto>).OrderBy(t => t.Name).ToList();
		}

		void IStatusService.PrioTestJob(int testjob)
		{
			_objTestPool.PrioTestJob(testjob);
		}

		void IStatusService.CancelTestJob(int testjob)
		{
			_objTestPool.CancelTestJob(testjob);
		}

		void IStatusService.RebootWorker(string node)
		{
			_objTestPool.GetTestWorker(node).RebootWorker();
		}

		void IStatusService.RebootAllWorker()
		{
			_objTestPool.GetTestWorker().ForEach(w => w.RebootWorker());
		}


		string IStatusService.GetMessage(int testsystem)
		{
			Testsystem objTestsystem = _objTestsystemRepository.GetById(testsystem);

			String strDeployname = Tfs.TfsBuildQuery.GetDeploymentName(objTestsystem);
			bool bolIsDeploymentRunning = Tfs.TfsBuildQuery.IsDeploymentRunning(strDeployname);
			if (bolIsDeploymentRunning)
			{
				return "<b>" + strDeployname + " is running</b>. Testing task starts after build is finished.";
			}
			return "";
		}

	}
}
