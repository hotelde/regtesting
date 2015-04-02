using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;

namespace RegTesting.Service.Services
{

	/// <summary>
	/// The TestManager
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class TestService : ITestService
	{
		private readonly ITestPool _objTestPool;
		private readonly ITestsuiteRepository _objTestsuiteRepository;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ITesterRepository _objTesterRepository;
		private readonly IResultRepository _objResultRepository;

		/// <summary>
		/// Creates a new TestManager
		/// </summary>
		/// <param name="objTestPool">the testPool</param>
		/// <param name="objTestsuiteRepository">the testsuiteRepository</param>
		/// <param name="objTestsystemRepository">the testsystemRepository</param>
		/// <param name="objTesterRepository">the testerRepository</param>
		/// <param name="objResultRepository">the resultRepository</param>
		public TestService(ITestPool objTestPool, ITestsuiteRepository objTestsuiteRepository,
			ITestsystemRepository objTestsystemRepository, ITesterRepository objTesterRepository,
			IResultRepository objResultRepository)
		{
			if (objTestsuiteRepository == null)
				throw new ArgumentNullException("objTestsuiteRepository");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
			if (objTesterRepository == null)
				throw new ArgumentNullException("objTesterRepository");
			if (objResultRepository == null)
				throw new ArgumentNullException("objResultRepository");

			_objTestPool = objTestPool;
			_objTestsuiteRepository = objTestsuiteRepository;
			_objTestsystemRepository = objTestsystemRepository;
			_objTesterRepository = objTesterRepository;
			_objResultRepository = objResultRepository;
		}

		void ITestService.TestTestsuite(int intTester, int intTestsystem, int intTestsuite)
		{
			Testsuite objTestsuite  = _objTestsuiteRepository.GetById(intTestsuite);
			Testsystem objTestsystem = _objTestsystemRepository.GetById(intTestsystem);
			Tester objTester = _objTesterRepository.GetById(intTester);

			TestJob objTestjob = CreateTestJob("Testsuite " + objTestsuite.Name, objTestsuite, objTestsystem, objTester);

			ITestJobManager objTestJobManager = new TestJobManager(objTestjob);

			ICollection<WorkItem> lstWorkItems = (from objTestcase in objTestsuite.Testcases
			                               from objBrowser in objTestsuite.Browsers
			                               from objLanguage in objTestsuite.Languages
												  select new WorkItem(objTestJobManager)
				                               {
					                               Testcase = objTestcase,
												   Browser = objBrowser,
												   Language = objLanguage,
												   Testsystem = objTestsystem,
												   Tester = objTester 
				                               }).ToList();

			_objTestPool.AddTestJob(objTestJobManager, lstWorkItems);
		}

		void ITestService.TestTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			Testsystem objTestsystem = _objTestsystemRepository.GetById(intTestsystem);
			Tester objTester = _objTesterRepository.GetById(intTester);
			Testcase objTestcase = objTestsuite.Testcases.Single(t => t.ID == intTestcase);

			TestJob objTestjob = CreateTestJob("Testcase " + objTestcase.Name + "(Testsuite " + objTestsuite.Name + ")", objTestsuite, objTestsystem, objTester);
			ITestJobManager objTestJobManager = new TestJobManager(objTestjob);

			ICollection<WorkItem> lstWorkItems = (from objBrowser in objTestsuite.Browsers
										   from objLanguage in objTestsuite.Languages
												  select new WorkItem(objTestJobManager)
										   {
											   Testcase = objTestcase,
											   Browser = objBrowser,
											   Language = objLanguage,
											   Testsystem = objTestsystem,
											   Tester = objTester 

										   }).ToList();

			_objTestPool.AddTestJob(objTestJobManager, lstWorkItems);
		}


		void ITestService.TestFailedTestsOfTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			Testcase objTestcase = objTestsuite.Testcases.Single(t => t.ID == intTestcase);

			IList<Result> lstErrorResults = _objResultRepository.GetErrorResultsOfTestsuite(intTestsystem, objTestsuite.Browsers, new List<Testcase>{ objTestcase}, 
				objTestsuite.Languages);
			Testsystem objTestsystem = _objTestsystemRepository.GetById(intTestsystem);
			Tester objTester = _objTesterRepository.GetById(intTester);

			TestJob objTestjob = CreateTestJob("Repeat failed of " + "Testcase " + objTestcase.Name + "(Testsuite " + objTestsuite.Name + ")", objTestsuite, objTestsystem, objTester);

			ITestJobManager objTestJobManager = new TestJobManager(objTestjob);

			ICollection<WorkItem> lstWorkItems = (from result in lstErrorResults
												  select new WorkItem(objTestJobManager)
												  {
													  Testcase = result.Testcase,
													  Browser = result.Browser,
													  Language = result.Language,
													  Testsystem = objTestsystem,
													  Tester = objTester
												  }).ToList();
			_objTestPool.AddTestJob(objTestJobManager, lstWorkItems);
		}


		void ITestService.TestFailedTestsOfTestsuite(int intTester, int intTestsystem, int intTestsuite)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			IList<Result> lstErrorResults = _objResultRepository.GetErrorResultsOfTestsuite(intTestsystem, objTestsuite.Browsers, objTestsuite.Testcases,
				objTestsuite.Languages);
			Testsystem objTestsystem = _objTestsystemRepository.GetById(intTestsystem);
			Tester objTester = _objTesterRepository.GetById(intTester);

			TestJob objTestjob = CreateTestJob("Repeat failed of " + objTestsuite.Name, objTestsuite, objTestsystem, objTester);

			ITestJobManager objTestJobManager = new TestJobManager(objTestjob);

			ICollection<WorkItem> lstWorkItems = (from result in lstErrorResults
												  select new WorkItem(objTestJobManager)
												  {
													  Testcase = result.Testcase,
													  Browser = result.Browser,
													  Language = result.Language,
													  Testsystem = objTestsystem,
													  Tester = objTester
												  }).ToList();

			_objTestPool.AddTestJob(objTestJobManager, lstWorkItems);
		}

		private static TestJob CreateTestJob(string strName, Testsuite objTestsuite, Testsystem objTestsystem, Tester objTester)
		{
			TestJob objTestjob = new TestJob
			{
				Name = strName,
				ResultCode = TestState.Pending,
				Testsuite = objTestsuite,
				Testsystem = objTestsystem,
				Tester = objTester,
				StartedAt = DateTime.Now,
				JobType = JobType.Testportal
			};
			return objTestjob;
		}


	}
}
