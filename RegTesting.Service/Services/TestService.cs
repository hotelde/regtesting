using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly ITestPool _testPool;
		private readonly ITestsuiteRepository _testsuiteRepository;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ITesterRepository _testerRepository;
		private readonly IResultRepository _resultRepository;

		/// <summary>
		/// Creates a new TestManager
		/// </summary>
		/// <param name="testPool">the testPool</param>
		/// <param name="testsuiteRepository">the testsuiteRepository</param>
		/// <param name="testsystemRepository">the testsystemRepository</param>
		/// <param name="testerRepository">the testerRepository</param>
		/// <param name="resultRepository">the resultRepository</param>
		public TestService(ITestPool testPool, ITestsuiteRepository testsuiteRepository,
			ITestsystemRepository testsystemRepository, ITesterRepository testerRepository,
			IResultRepository resultRepository)
		{
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (testerRepository == null)
				throw new ArgumentNullException("testerRepository");
			if (resultRepository == null)
				throw new ArgumentNullException("resultRepository");

			_testPool = testPool;
			_testsuiteRepository = testsuiteRepository;
			_testsystemRepository = testsystemRepository;
			_testerRepository = testerRepository;
			_resultRepository = resultRepository;
		}

		void ITestService.TestTestsuite(int testerId, int testsystemId, int testsuiteId)
		{
			Testsuite testsuite  = _testsuiteRepository.GetById(testsuiteId);
			Testsystem testsystem = _testsystemRepository.GetById(testsystemId);
			Tester tester = _testerRepository.GetById(testerId);

			TestJob testJob = CreateTestJob("Testsuite " + testsuite.Name, testsuite, testsystem, tester);

			ITestJobManager testJobManager = new TestJobManager(testJob);

			ICollection<WorkItem> workItems = (from testcase in testsuite.Testcases
			                               from browser in testsuite.Browsers
			                               from language in testsuite.Languages
												  select new WorkItem(testJobManager)
				                               {
					                               Testcase = testcase,
												   Browser = browser,
												   Language = language,
												   Testsystem = testsystem,
												   Tester = tester 
				                               }).ToList();

			_testPool.AddTestJob(testJobManager, workItems);
		}

		void ITestService.TestTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			Testsystem testsystem = _testsystemRepository.GetById(testsystemId);
			Tester tester = _testerRepository.GetById(testerId);
			Testcase testcase = testsuite.Testcases.Single(t => t.ID == testcaseId);

			TestJob testJob = CreateTestJob("Testcase " + testcase.Name + "(Testsuite " + testsuite.Name + ")", testsuite, testsystem, tester);
			ITestJobManager testJobManager = new TestJobManager(testJob);

			ICollection<WorkItem> workItems = (from browser in testsuite.Browsers
										   from language in testsuite.Languages
												  select new WorkItem(testJobManager)
										   {
											   Testcase = testcase,
											   Browser = browser,
											   Language = language,
											   Testsystem = testsystem,
											   Tester = tester 

										   }).ToList();

			_testPool.AddTestJob(testJobManager, workItems);
		}


		void ITestService.TestFailedTestsOfTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			Testcase testcase = testsuite.Testcases.Single(t => t.ID == testcaseId);

			IList<Result> errorResults = _resultRepository.GetErrorResultsOfTestsuite(testsystemId, testsuite.Browsers, new List<Testcase>{ testcase}, 
				testsuite.Languages);
			Testsystem testsystem = _testsystemRepository.GetById(testsystemId);
			Tester tester = _testerRepository.GetById(testerId);

			TestJob testjob = CreateTestJob("Repeat failed of " + "Testcase " + testcase.Name + "(Testsuite " + testsuite.Name + ")", testsuite, testsystem, tester);

			ITestJobManager testjobManager = new TestJobManager(testjob);

			ICollection<WorkItem> workItems = (from result in errorResults
												  select new WorkItem(testjobManager)
												  {
													  Testcase = result.Testcase,
													  Browser = result.Browser,
													  Language = result.Language,
													  Testsystem = testsystem,
													  Tester = tester
												  }).ToList();
			_testPool.AddTestJob(testjobManager, workItems);
		}


		void ITestService.TestFailedTestsOfTestsuite(int testerId, int testsystemId, int testsuiteId)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			IList<Result> errorResults = _resultRepository.GetErrorResultsOfTestsuite(testsystemId, testsuite.Browsers, testsuite.Testcases,
				testsuite.Languages);
			Testsystem testsystem = _testsystemRepository.GetById(testsystemId);
			Tester tester = _testerRepository.GetById(testerId);

			TestJob testjob = CreateTestJob("Repeat failed of " + testsuite.Name, testsuite, testsystem, tester);

			ITestJobManager testjobManager = new TestJobManager(testjob);

			ICollection<WorkItem> workItems = (from result in errorResults
												  select new WorkItem(testjobManager)
												  {
													  Testcase = result.Testcase,
													  Browser = result.Browser,
													  Language = result.Language,
													  Testsystem = testsystem,
													  Tester = tester
												  }).ToList();

			_testPool.AddTestJob(testjobManager, workItems);
		}

		private static TestJob CreateTestJob(string name, Testsuite testsuite, Testsystem testsystem, Tester tester)
		{
			TestJob testjob = new TestJob
			{
				Name = name,
				ResultCode = TestState.Pending,
				Testsuite = testsuite,
				Testsystem = testsystem,
				Tester = tester,
				StartedAt = DateTime.Now,
				JobType = JobType.Testportal
			};
			return testjob;
		}


	}
}
