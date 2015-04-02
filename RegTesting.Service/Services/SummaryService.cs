using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
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
		private readonly IResultRepository _objResultRepository;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ITestsuiteRepository _objTestsuiteRepository;

		/// <summary>
		/// Create a new SummaryService
		/// </summary>
		/// <param name="objResultRepository">the ResultRepository</param>
		/// <param name="objTestsystemRepository">the TestsystemRepository</param>
		/// <param name="objTestsuiteRepository">the TestsuiteRepository</param>
		public SummaryService(IResultRepository objResultRepository, ITestsystemRepository objTestsystemRepository, ITestsuiteRepository objTestsuiteRepository)
		{
			if (objResultRepository == null)
				throw new ArgumentNullException("objResultRepository");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
			if (objTestsuiteRepository == null)
				throw new ArgumentNullException("objTestsuiteRepository");

			_objResultRepository = objResultRepository;
			_objTestsystemRepository = objTestsystemRepository;
			_objTestsuiteRepository = objTestsuiteRepository;
		}




		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllThorBranches()
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetByName(RegtestingServerConfiguration.ThorDefaulttestsuite);
			return _objTestsystemRepository.GetAll()
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, objTestsuite, TestsystemSummariesCache.ThorCache))
				.OrderByDescending(objSummary => objSummary.LastChangeDate).Where(objSummary => DateTime.Now - objSummary.LastChangeDate < TimeSpan.FromDays(7)).ToList();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllSodaBranches()
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetByName(RegtestingServerConfiguration.SodaDefaulttestsuite);
			return _objTestsystemRepository.GetAll()
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, objTestsuite, TestsystemSummariesCache.SodaCache))
				.OrderByDescending(objSummary => objSummary.LastChangeDate).Where(objSummary => DateTime.Now - objSummary.LastChangeDate < TimeSpan.FromDays(7)).ToList();
		}


		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForThorMainBranches()
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetByName(RegtestingServerConfiguration.ThorDefaulttestsuite);
			IList<Testsystem> lstMainTestsystems = new List<Testsystem>
				{
					_objTestsystemRepository.GetByName("dev")
				};

			return lstMainTestsystems
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, objTestsuite, TestsystemSummariesCache.ThorCache))
				.ToList();
		}


		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForSodaMainBranches()
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetByName(RegtestingServerConfiguration.SodaDefaulttestsuite);
		
			IList<Testsystem> lstMainTestsystems = new List<Testsystem>
				{
					_objTestsystemRepository.GetByName("devsoda-uit")
				};

			return lstMainTestsystems
				.Select(objTestsystem => CreateTestsystemSummary(objTestsystem, objTestsuite, TestsystemSummariesCache.SodaCache))
				.ToList();
		}

		private TestsystemSummary CreateTestsystemSummary(Testsystem objTestsystem, Testsuite objTestsuite, TestsystemSummariesCache cache)
		{
			lock (cache.GetLock(objTestsystem.ID))
			{
				
				TestsystemSummary objCachedResult = cache.Get(objTestsystem.ID);

				if (objCachedResult != null)
					return objCachedResult;


				IList<Result> listOfResults = _objResultRepository.GetListOfResults(objTestsystem.ID, objTestsuite.Browsers, objTestsuite.Testcases,
													  objTestsuite.Languages);

				TestsystemSummary objTestsystemSummary = new TestsystemSummary
				{
					TestsuiteName = objTestsuite.Name,
					TestsuiteID = objTestsuite.ID,
					TestsystemName = objTestsystem.Name,
					TestsystemID = objTestsystem.ID,
				};

				if (listOfResults.Count != 0)
				{
					objTestsystemSummary.LastChangeDate = listOfResults.Min(t => t.Testtime);
					objTestsystemSummary.TestsystemStatus = listOfResults.Max(t => t.ResultCode);
				}
				else
				{
					objTestsystemSummary.LastChangeDate = DateTime.MinValue;
					objTestsystemSummary.TestsystemStatus = (int)TestState.NotSet;
				}

				cache.Set(objTestsystem.ID, objTestsystemSummary);
				return objTestsystemSummary;
			}
		}
	}
}
