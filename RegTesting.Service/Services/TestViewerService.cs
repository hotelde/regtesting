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
		private readonly IResultRepository _objResultRepository;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ITestsuiteRepository _objTestsuiteRepository;
		private readonly ITesterRepository _objTesterRepository;
		private readonly ITestcaseRepository _objTestcaseRepository;
		private readonly IHistoryResultRepository _objHistoryResultRepository;

		/// <summary>
		/// Create a new TestViewer
		/// </summary>
		/// <param name="objResultRepository">the ResultRepository</param>
		/// <param name="objTestsystemRepository">the TestsystemRepository</param>
		/// <param name="objTestsuiteRepository">the TestsuiteRepository</param>
		/// <param name="objTesterRepository">the TesterRepository</param>
		/// <param name="objTestcaseRepository">the TestcaseRepository</param>
		/// <param name="objHistoryResultRepository">the HistoryResultRepository</param>
		public TestViewerService(IResultRepository objResultRepository, ITestsystemRepository objTestsystemRepository, ITestsuiteRepository objTestsuiteRepository,
			ITesterRepository objTesterRepository, ITestcaseRepository objTestcaseRepository, IHistoryResultRepository objHistoryResultRepository)
		{
			if (objResultRepository == null)
				throw new ArgumentNullException("objResultRepository");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
			if (objTestsuiteRepository == null)
				throw new ArgumentNullException("objTestsuiteRepository");
			if (objTesterRepository == null)
				throw new ArgumentNullException("objTesterRepository");
			if (objTestcaseRepository == null)
				throw new ArgumentNullException("objTestcaseRepository");
			if (objHistoryResultRepository == null)
				throw new ArgumentNullException("objHistoryResultRepository");

			_objResultRepository = objResultRepository;
			_objTestsystemRepository = objTestsystemRepository;
			_objTestsuiteRepository = objTestsuiteRepository;
			_objTesterRepository = objTesterRepository;
			_objTestcaseRepository = objTestcaseRepository;
			_objHistoryResultRepository = objHistoryResultRepository;
		}


		IList<TestsystemDto> ITestViewerService.GetTestsystems()
		{
			return Mapper.Map<IList<TestsystemDto>>(_objTestsystemRepository.GetAll());
		}

		IList<TestsuiteDto> ITestViewerService.GetTestSuites(int testsystem)
		{
			return Mapper.Map<IList<TestsuiteDto>>(_objTestsuiteRepository.GetAll()
				.Where(testsuite => testsuite.TestsystemFilter == null || testsuite.TestsystemFilter.ID == testsystem)
				.ToList());
		}

		IList<GroupedResult> ITestViewerService.GetResults(int intTestsystem, int intTestsuite, DateTime? datResultsSince)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			IList<Result> listOfResults = _objResultRepository.GetListOfResults(intTestsystem, objTestsuite.Browsers, objTestsuite.Testcases,
									  objTestsuite.Languages).ToList();

			List<GroupedResult> lstGroupedResults = new List<GroupedResult>();

			foreach (Testcase objTestcase in objTestsuite.Testcases)
			{
				GroupedResult objGroupedRes = CalculateGroupedResultsForTestcase(objTestcase.ID, listOfResults.Where(objResult => objResult.Testcase.ID == objTestcase.ID).ToList(), datResultsSince);
				if (objGroupedRes != null)
					lstGroupedResults.Add(objGroupedRes);
			}

			return lstGroupedResults;
		}


		TestcaseDetailsModel ITestViewerService.GetTestcaseDetails(int intTestsystem, int intTestsuite, int intTestcase)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			TestcaseDetailsModel objTestDetailsModel = new TestcaseDetailsModel();

			IList<Result> lstErrorResultsOfTestsuite = _objResultRepository.GetErrorResultsOfTestsuite(intTestsystem, objTestsuite.Browsers, objTestsuite.Testcases.Where(t => t.ID == intTestcase).ToList(), 
			                                                objTestsuite.Languages);

			IList<ErrorOccurrenceGroup> errorOccurenceGroups = ErrorGrouping.GetErrorOccurrenceGroups(lstErrorResultsOfTestsuite);
			if (errorOccurenceGroups.Any())
			{
				objTestDetailsModel.ErrorOccurrenceGroup = errorOccurenceGroups.First();
			}
			objTestDetailsModel.Testcase = _objTestcaseRepository.GetById(intTestcase);
			return objTestDetailsModel;
		}


		int ITestViewerService.GetTesterIDByName(string strName)
		{
			return _objTesterRepository.GetByName(strName).ID;
		}

		IList<ErrorOccurrenceGroup> ITestViewerService.GetCurrentErrorOccurrenceGroups(int intTestsystem, int intTestsuite)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			IList<Result> lstErrorResultsOfTestsuite = _objResultRepository.GetErrorResultsOfTestsuite(intTestsystem, objTestsuite.Browsers,
				objTestsuite.Testcases, objTestsuite.Languages);
			return ErrorGrouping.GetErrorOccurrenceGroups(lstErrorResultsOfTestsuite);
		}

		IList<ErrorOccurrenceGroup> ITestViewerService.GetHistoryErrorOccurrenceGroups(int intTestsystem, int intTestsuite, DateTime datFromDateTime,
		                                                  DateTime datToDateTime)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			IList<HistoryResult> lstHistoryErrorResultsOfTestsuite = _objHistoryResultRepository.GetListOfErrorHistoryResults(intTestsystem,
				objTestsuite.Browsers, objTestsuite.Testcases, objTestsuite.Languages, datFromDateTime, datToDateTime);
			return ErrorGrouping.GetHistoryErrorOccurrenceGroups(lstHistoryErrorResultsOfTestsuite);
		}

		IList<HistoryResult> ITestViewerService.GetResultsHistory(int intTestsystem, int intTestcase, int intBrowser, int intLanguage, int intTestsuite,
		                               int intMaxResults)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetById(intTestsuite);
			IList<Browser> lstBrowsers = intBrowser == -1 ? objTestsuite.Browsers : objTestsuite.Browsers.Where(t => t.ID == intBrowser).ToList();
			IList<Language> lstLanguages = intLanguage == -1 ? objTestsuite.Languages : objTestsuite.Languages.Where(t => t.ID == intLanguage).ToList();
			IList<Testcase> lstTestcases = intTestcase == -1 ? objTestsuite.Testcases : objTestsuite.Testcases.Where(t => t.ID == intTestcase).ToList();

			return _objHistoryResultRepository.GetListOfHistoryResults(intTestsystem, lstBrowsers, lstTestcases, lstLanguages, intMaxResults).ToList();
		}

		#region private members
		private GroupedResult CalculateGroupedResultsForTestcase(int intTestcaseID, List<Result> lstResults, DateTime? datResultsSince)
		{

			//Don't show not supported state...
			lstResults = lstResults.Where(t => t.ResultCode !=TestState.NotSupported).ToList();

			if (!lstResults.Any())
				return null;

			//If we have a date since filter, only use results if there are new ones...
			if (datResultsSince.HasValue && lstResults.Max(t => t.Testtime) + new TimeSpan(0, 0, 10) < datResultsSince.Value)
				return null;

			GroupedResult objGroupedResult = new GroupedResult { Testcase = intTestcaseID };

			var objGroupedResultsByResultCode = lstResults.GroupBy(result => result.ResultCode, result => result,
									 (key, elements) => new { ResultCode = key, Results = elements.ToList() });

			bool bolAllResultsAreEqual = objGroupedResultsByResultCode.Count() == 1;

			foreach (var result in objGroupedResultsByResultCode)
			{

				GroupedResultPart objGroupedResultPart = new GroupedResultPart { ResultCode = result.ResultCode };
				if (bolAllResultsAreEqual)
				{
					objGroupedResultPart.ResultLabel = (result.ResultCode).ToString();
				}
				else if (result.Results.Count == 1)
				{
					Result objSingleResult = result.Results[0];
					objGroupedResultPart.ResultLabel = objSingleResult.Browser.Name + " " + objSingleResult.Language.Languagecode + " " + ((TestState)result.ResultCode);
				}
				else
				{
					objGroupedResultPart.ResultLabel = result.Results.Count + "x " + (result.ResultCode);
				}
				objGroupedResult.GroupedResultParts.Add(objGroupedResultPart);
			}

			return objGroupedResult;
		}

	

		#endregion
	}
}
