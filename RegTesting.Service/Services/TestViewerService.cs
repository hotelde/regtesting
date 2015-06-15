using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.TestLogic;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// The TestViewer class
	/// </summary>
	public class TestViewerService : ITestViewerService
	{
		private readonly IResultRepository _resultRepository;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ITestsuiteRepository _testsuiteRepository;
		private readonly ITesterRepository _testerRepository;
		private readonly ITestcaseRepository _testcaseRepository;
		private readonly IHistoryResultRepository _historyResultRepository;

		/// <summary>
		/// Create a new TestViewer
		/// </summary>
		/// <param name="resultRepository">the ResultRepository</param>
		/// <param name="testsystemRepository">the TestsystemRepository</param>
		/// <param name="testsuiteRepository">the TestsuiteRepository</param>
		/// <param name="testerRepository">the TesterRepository</param>
		/// <param name="testcaseRepository">the TestcaseRepository</param>
		/// <param name="historyResultRepository">the HistoryResultRepository</param>
		public TestViewerService(IResultRepository resultRepository, ITestsystemRepository testsystemRepository, ITestsuiteRepository testsuiteRepository,
			ITesterRepository testerRepository, ITestcaseRepository testcaseRepository, IHistoryResultRepository historyResultRepository)
		{
			if (resultRepository == null)
				throw new ArgumentNullException("resultRepository");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");
			if (testerRepository == null)
				throw new ArgumentNullException("testerRepository");
			if (testcaseRepository == null)
				throw new ArgumentNullException("testcaseRepository");
			if (historyResultRepository == null)
				throw new ArgumentNullException("historyResultRepository");

			_resultRepository = resultRepository;
			_testsystemRepository = testsystemRepository;
			_testsuiteRepository = testsuiteRepository;
			_testerRepository = testerRepository;
			_testcaseRepository = testcaseRepository;
			_historyResultRepository = historyResultRepository;
		}


		IList<TestsystemDto> ITestViewerService.GetTestsystems()
		{
			return Mapper.Map<IList<TestsystemDto>>(_testsystemRepository.GetAll());
		}

		IList<TestsuiteDto> ITestViewerService.GetTestSuites(int testsystem)
		{
			return Mapper.Map<IList<TestsuiteDto>>(_testsuiteRepository.GetAll()
				.Where(testsuite => testsuite.TestsystemFilter == null || testsuite.TestsystemFilter.ID == testsystem)
				.ToList());
		}

		IList<GroupedResult> ITestViewerService.GetResults(int testsystemIndex, int testsuiteId, DateTime? resultsSince)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			IList<Result> results = _resultRepository.GetListOfResults(testsystemIndex, testsuite.Browsers, testsuite.Testcases,
									  testsuite.Languages).ToList();

			List<GroupedResult> groupedResults = new List<GroupedResult>();

			foreach (Testcase testcase in testsuite.Testcases)
			{
				GroupedResult groupedResult = CalculateGroupedResultsForTestcase(testcase.ID, results.Where(result => result.Testcase.ID == testcase.ID).ToList(), resultsSince);
				if (groupedResult != null)
					groupedResults.Add(groupedResult);
			}

			return groupedResults;
		}


		TestcaseDetailsModel ITestViewerService.GetTestcaseDetails(int testsystemIndex, int testsuiteId, int testcaseId)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			TestcaseDetailsModel testcaseDetails = new TestcaseDetailsModel();

			IList<Result> errorResultsOfTestsuite = _resultRepository.GetErrorResultsOfTestsuite(testsystemIndex, testsuite.Browsers, testsuite.Testcases.Where(t => t.ID == testcaseId).ToList(), 
			                                                testsuite.Languages);

			IList<ErrorOccurrenceGroup> errorOccurenceGroups = ErrorGrouping.GetErrorOccurrenceGroups(errorResultsOfTestsuite);
			if (errorOccurenceGroups.Any())
			{
				testcaseDetails.ErrorOccurrenceGroup = errorOccurenceGroups.First();
			}
			testcaseDetails.Testcase = _testcaseRepository.GetById(testcaseId);
			return testcaseDetails;
		}


		int ITestViewerService.GetTesterIDByName(string name)
		{
			return _testerRepository.GetByName(name).ID;
		}

		IList<ErrorOccurrenceGroup> ITestViewerService.GetCurrentErrorOccurrenceGroups(int testsystemIndex, int testsuiteId)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			IList<Result> errorResultsOfTestsuite = _resultRepository.GetErrorResultsOfTestsuite(testsystemIndex, testsuite.Browsers,
				testsuite.Testcases, testsuite.Languages);
			return ErrorGrouping.GetErrorOccurrenceGroups(errorResultsOfTestsuite);
		}

		IList<ErrorOccurrenceGroup> ITestViewerService.GetHistoryErrorOccurrenceGroups(int testsystemIndex, int testsuiteId, DateTime fromDate, DateTime toDate)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			IList<HistoryResult> historyErrorResultsOfTestsuite = _historyResultRepository.GetListOfErrorHistoryResults(testsystemIndex,
				testsuite.Browsers, testsuite.Testcases, testsuite.Languages, fromDate, toDate);
			return ErrorGrouping.GetHistoryErrorOccurrenceGroups(historyErrorResultsOfTestsuite);
		}

		IList<ErrorOccurrenceGroup> ITestViewerService.GetErrorOccurrenceGroupsForTestjob(int testjobId)
		{
			IList<HistoryResult> historyErrorResultsOfTestsuite = _historyResultRepository.GetListOfErrorHistoryResults(testjobId);
			return ErrorGrouping.GetHistoryErrorOccurrenceGroups(historyErrorResultsOfTestsuite);
		}


		IList<HistoryResult> ITestViewerService.GetResultsHistory(int testsystemIndex, int testcaseId, int browserId, int languageId, int testsuiteId,
		                               int maxResults)
		{
			Testsuite testsuite = _testsuiteRepository.GetById(testsuiteId);
			IList<Browser> browsers = browserId == -1 ? testsuite.Browsers : testsuite.Browsers.Where(t => t.ID == browserId).ToList();
			IList<Language> languages = languageId == -1 ? testsuite.Languages : testsuite.Languages.Where(t => t.ID == languageId).ToList();
			IList<Testcase> testcases = testcaseId == -1 ? testsuite.Testcases : testsuite.Testcases.Where(t => t.ID == testcaseId).ToList();

			return _historyResultRepository.GetListOfHistoryResults(testsystemIndex, browsers, testcases, languages, maxResults).ToList();
		}

		#region private members
		private GroupedResult CalculateGroupedResultsForTestcase(int intTestcaseID, List<Result> results, DateTime? resultsSince)
		{

			//Don't show not supported state...
			results = results.Where(t => t.ResultCode !=TestState.NotSupported).ToList();

			if (!results.Any())
				return null;

			//If we have a date since filter, only use results if there are new ones...
			if (resultsSince.HasValue && results.Max(t => t.Testtime) + new TimeSpan(0, 0, 10) < resultsSince.Value)
				return null;

			GroupedResult groupedResult = new GroupedResult { Testcase = intTestcaseID };

			var groupedResultsByResultCode = results.GroupBy(result => result.ResultCode, result => result,
									 (key, elements) => new { ResultCode = key, Results = elements.ToList() });

			bool allResultsAreEqual = groupedResultsByResultCode.Count() == 1;

			foreach (var result in groupedResultsByResultCode)
			{

				GroupedResultPart groupedResultPart = new GroupedResultPart { ResultCode = result.ResultCode };
				if (allResultsAreEqual)
				{
					groupedResultPart.ResultLabel = (result.ResultCode).ToString();
				}
				else if (result.Results.Count == 1)
				{
					Result singleResult = result.Results[0];
					groupedResultPart.ResultLabel = singleResult.Browser.Name + " " + singleResult.Language.Languagecode + " " + ((TestState)result.ResultCode);
				}
				else
				{
					groupedResultPart.ResultLabel = result.Results.Count + "x " + (result.ResultCode);
				}
				groupedResult.GroupedResultParts.Add(groupedResultPart);
			}

			return groupedResult;
		}

	

		#endregion
	}
}
