using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Cache;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// SummaryService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class SummaryService : ISummaryService
	{
		private readonly IResultRepository _resultRepository;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ITestsuiteRepository _testsuiteRepository;
		private readonly ITestJobRepository _testJobRepository;

		/// <summary>
		/// Create a new SummaryService
		/// </summary>
		/// <param name="resultRepository">the ResultRepository</param>
		/// <param name="testsystemRepository">the TestsystemRepository</param>
		/// <param name="testsuiteRepository">the TestsuiteRepository</param>
		/// <param name="testJobRepository">the TestJobRepository</param>
		public SummaryService(IResultRepository resultRepository, ITestsystemRepository testsystemRepository, ITestsuiteRepository testsuiteRepository, ITestJobRepository testJobRepository)
		{
			if (resultRepository == null)
				throw new ArgumentNullException("resultRepository");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");
			if (testJobRepository == null)
				throw new ArgumentNullException("testJobRepository");

			_resultRepository = resultRepository;
			_testsystemRepository = testsystemRepository;
			_testsuiteRepository = testsuiteRepository;
			_testJobRepository = testJobRepository;
		}




        IList<TestsystemSummary> ISummaryService.GetLastTestsystemSummaries()
		{
			Testsuite testsuite = _testsuiteRepository.GetByName(RegtestingServerConfiguration.Defaulttestsuite);
			return _testsystemRepository.GetAll()
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, testsuite, TestsystemSummariesCache.Cache))
				.OrderByDescending(objSummary => objSummary.LastChangeDate).Where(objSummary => DateTime.Now - objSummary.LastChangeDate < TimeSpan.FromDays(7)).ToList();
		}

        IList<TestsystemSummary> ISummaryService.GetPinnedTestsystemSummaries()
		{
			Testsuite testsuite = _testsuiteRepository.GetByName(RegtestingServerConfiguration.Defaulttestsuite);
	        IList<Testsystem> mainTestsystems =
		        RegtestingServerConfiguration.PinnedBranches.Select(t => _testsystemRepository.GetByName(t)).ToList();
		

			return mainTestsystems
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, testsuite, TestsystemSummariesCache.Cache))
				.ToList();
		}

		IList<TestJobDto> ISummaryService.GetTestJobs()
		{
			return Mapper.Map<List<TestJob>, List<TestJobDto>>(_testJobRepository.GetAll().Where(job => DateTime.Now - job.StartedAt < TimeSpan.FromDays(5)).OrderByDescending(t => t.StartedAt).ToList());
		}

		private TestsystemSummary CreateTestsystemSummary(Testsystem testsystem, Testsuite testsuite, TestsystemSummariesCache cache)
		{
			lock (cache.GetLock(testsystem.ID))
			{
				
				TestsystemSummary cachedResult = cache.Get(testsystem.ID);

				if (cachedResult != null)
					return cachedResult;


				IList<Result> results = _resultRepository.GetListOfResults(testsystem.ID, testsuite.Browsers, testsuite.Testcases,
													  testsuite.Languages);

				TestsystemSummary testsystemSummary = new TestsystemSummary
				{
					TestsuiteName = testsuite.Name,
					TestsuiteID = testsuite.ID,
					TestsystemName = testsystem.Name,
					TestsystemID = testsystem.ID,
				};

				if (results.Count != 0)
				{
					testsystemSummary.LastChangeDate = results.Min(t => t.Testtime);
					testsystemSummary.TestsystemStatus = results.Max(t => t.ResultCode);
				}
				else
				{
					testsystemSummary.LastChangeDate = DateTime.MinValue;
					testsystemSummary.TestsystemStatus = (int)TestState.NotSet;
				}

				cache.Set(testsystem.ID, testsystemSummary);
				return testsystemSummary;
			}
		}

       
    }
}
