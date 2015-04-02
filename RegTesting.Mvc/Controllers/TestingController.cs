using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Filters;
using RegTesting.Mvc.Models;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// The TestingController
	/// </summary>
	[RegAuthorize]
	public class TestingController : Controller
	{
		private readonly ITestService _objTestService;
		private readonly ITestViewerService _objTestViewerService;
		private readonly IStatusService _objStatusService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objTestService">the TestService</param>
		/// <param name="objTestViewerService">the TestViewerService</param>
		/// <param name="objStatusService">the StatusService</param>
		public TestingController(ITestService objTestService, ITestViewerService objTestViewerService, IStatusService objStatusService)
		{
			if (objTestService == null)
				throw new ArgumentNullException("objTestService");
			if (objTestViewerService == null)
				throw new ArgumentNullException("objTestViewerService");
			if (objStatusService == null)
				throw new ArgumentNullException("objStatusService");

			_objTestService = objTestService;
			_objTestViewerService = objTestViewerService;
			_objStatusService = objStatusService;
		}


		/// <summary>
		/// Index function for Testing View.
		/// </summary>
		/// <param name="testsystem">ID of the Testsystem</param>
		/// <param name="testsuite">ID of the Testsuite</param>
		/// <returns>A Testing Index View</returns>
		public ActionResult Index(int testsystem = 0, int testsuite = 0)
		{
			if (testsystem ==0 && Session["testsystem"] != null) testsystem = (int)Session["testsystem"];
			if (testsuite == 0 && Session["testsuite"] != null) testsuite = (int)Session["testsuite"];

			IList<TestsystemModel> lstTestsystems = Mapper.Map <IList<TestsystemModel>>(_objTestViewerService.GetTestsystems());
			ViewBag.LstTestsystems = lstTestsystems;
			if (testsystem == 0 && lstTestsystems.Count > 0)
			{
				TestsystemModel objDev= lstTestsystems.FirstOrDefault(t => t.Name == "dev");
				testsystem = objDev != null ? objDev.ID : lstTestsystems[0].ID;
			}

			IList<TestsuiteModel> lstTestsuites = Mapper.Map<IList<TestsuiteModel>>(_objTestViewerService.GetTestSuites(testsystem));
			ViewBag.LstTestsuites = lstTestsuites;
			if ((testsuite == 0 || lstTestsuites.All(t => t.ID != testsuite)) && lstTestsuites.Count > 0)
			{
				TestsuiteModel objMain = lstTestsuites.FirstOrDefault(t => t.Name == RegtestingMvcConfiguration.ThorDefaulttestsuite);
				testsuite =objMain !=null ? objMain.ID : lstTestsuites[0].ID;
			}

			Session["testsystem"] = testsystem;
			Session["testsuite"] = testsuite;

			ViewBag.CurrentTestsuite = lstTestsuites.Single(t => t.ID == testsuite);
			ViewBag.CurrentTestsystem = lstTestsystems.Single(t => t.ID == testsystem);

			return View();
			

		}

		/// <summary>
		/// Rerun a given test on a given template for a given testsuite
		/// </summary>
		/// <param name="testcase">ID of Testcase</param>
		/// <returns>Returns a refreshtag, so wie can refresh our statustable</returns>
		public ActionResult RerunTest(int testcase)
		{
			if (Session["testsystem"] == null || Session["testsuite"] == null)
				return CreateResultJson(0, 0);

			int testsystem = (int)Session["testsystem"];
			int testsuite = (int)Session["testsuite"];
			int intTester = (int) Session["tester"];
			_objTestService.TestTestcaseOfTestsuite(intTester, testsystem, testsuite, testcase);
			return CreateResultJson(testsystem, testsuite);
			
		}

		/// <summary>
		/// Retest all tests, which result was an Error.
		/// </summary>
		/// <param name="testsystem">ID of testsystem</param>
		/// <param name="testsuite">ID of testsuite</param>
		/// <returns>A refreshed ResultJson</returns>
		public ActionResult RedoErrorTests(int testsystem, int testsuite)
		{
			_objTestService.TestFailedTestsOfTestsuite((int)Session["tester"], testsystem, testsuite);
			return CreateResultJson(testsystem, testsuite);
		}

		/// <summary>
		/// Creates a JSON objects of Results
		/// </summary>
		/// <param name="testsystem">ID of Testsystem</param>
		/// <param name="testsuite">ID of Testsuite</param>
		/// <param name="lngResultsSince">Add only results since that datetime (in ticks)</param>
		/// <returns>A ResultJson</returns>
		[NonAction]
		private JsonResult CreateResultJson(int testsystem, int testsuite, long lngResultsSince = 0)
		{
			long lngResultsUntil = DateTime.Now.Ticks;
			RefreshResultsModel objRefreshResultsModel = new RefreshResultsModel();

			IList<GroupedResult> lstResults = _objTestViewerService.GetResults(testsystem, testsuite, new DateTime(lngResultsSince));
	
			objRefreshResultsModel.Results = lstResults;
			objRefreshResultsModel.ResultsUntil = lngResultsUntil;
			return Json(objRefreshResultsModel, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		///  Refresh the testtable with new results
		/// </summary>
		/// <param name="testsystem">ID of testsystem</param>
		/// <param name="testsuite">ID of testsuite</param>
		/// <param name="resultsSince">Time in ticks, since when to get results</param>
		/// <returns>A ResultJson with updated testresults</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult RefreshResults(int testsystem, int testsuite, long resultsSince = 0)
		{
			return CreateResultJson(testsystem, testsuite, resultsSince);
		}

		/// <summary>
		/// Run all tests for a tester, a testsystem and a testsuite
		/// </summary>
		/// <param name="testsystem">ID of testsystem</param>
		/// <param name="testsuite">ID of testsuite</param>
		/// <returns>A ResultJson with updated testresults</returns>
		public ActionResult RunAllTests(int testsystem, int testsuite)
		{
			_objTestService.TestTestsuite((int)Session["tester"], testsystem, testsuite);

			return CreateResultJson(testsystem, testsuite);
		}

		/// <summary>
		/// Refresh the PartialView TestcaseDetails
		/// </summary>
		/// <param name="testsystem">current testsystem</param>
		/// <param name="testsuite">current testsuite</param>
		/// <param name="testcase">choosen testcase</param>
		/// <returns>A PartialView of type PartialTestcaseDetails</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetTestcaseDetails(int testsystem, int testsuite, int testcase)
		{
			TestcaseDetailsModel objTestcaseDetailsModel = _objTestViewerService.GetTestcaseDetails(testsystem, testsuite, testcase);
			return PartialView("PartialTestcaseDetails", objTestcaseDetailsModel);
		}

		/// <summary>
		/// 	Get IndexView of the Log
		/// </summary>
		/// <param name="toDate"> Show Logentries before toDate </param>
		/// <param name="testsystem"> ID of testsystem </param>
		/// <param name="fromDate"> Show Logentries after fromDate </param>
		/// <param name="testsuite"> ID of testsuite </param>
		/// <param name="history"> show history (history=1) or current results (history=0) </param>
		/// <returns> Index View </returns>
		public ActionResult Errorlog(string fromDate = "", string toDate = "", int testsystem = 0, int testsuite = 0, int history = 0)
		{
			if (testsystem == 0 && Session["testsystem"] != null) testsystem = (int)Session["testsystem"];
			if (testsuite == 0 && Session["testsuite"] != null) testsuite = (int)Session["testsuite"];
			Session["testsystem"] = testsystem;
			Session["testsuite"] = testsuite;

			IList<TestsystemModel> lstTestsystems = Mapper.Map<IList<TestsystemModel>>(_objTestViewerService.GetTestsystems());
			ViewBag.LstTestsystems = lstTestsystems;
			if (testsystem == 0 && lstTestsystems.Count > 0)
			{
				testsystem = lstTestsystems[0].ID;
			}

			IList<TestsuiteModel> lstTestsuites = Mapper.Map<IList<TestsuiteModel>>(_objTestViewerService.GetTestSuites(testsystem));
			ViewBag.LstTestsuites = lstTestsuites;
			if ((testsuite == 0 || lstTestsuites.All(t => t.ID != testsuite)) && lstTestsuites.Count > 0)
			{
				testsuite = lstTestsuites[0].ID;
			}

			IList<ErrorOccurrenceGroup> lstErrorOccurrenceGroup;

			DateTime datFromDateTime = fromDate == "" ? DateTime.Now : DateTime.Parse(fromDate + " 00:00:00");
			DateTime datToDateTime = toDate == "" ? DateTime.Now : DateTime.Parse(toDate + " 23:59:59");

			if (history == 0)
			{
				lstErrorOccurrenceGroup = _objTestViewerService.GetCurrentErrorOccurrenceGroups(testsystem, testsuite);
			}
			else
			{
				lstErrorOccurrenceGroup = _objTestViewerService.GetHistoryErrorOccurrenceGroups(testsystem, testsuite, datFromDateTime, datToDateTime);
			}

			ViewBag.DateFrom = datFromDateTime.ToString("dd.MM.yyyy");
			ViewBag.DateTo = datToDateTime.ToString("dd.MM.yyyy");

			ViewBag.CurrentTestsuite = lstTestsuites.Single(t => t.ID == testsuite);
			ViewBag.CurrentTestsystem = lstTestsystems.Single(t => t.ID == testsystem);

			ViewBag.ShowHistory = history;

			return View("Errorlog", lstErrorOccurrenceGroup);
		}




		/// <summary>
		/// Rerun the error tests of a testcase
		/// </summary>
		/// <param name="testsystem">ID of Testsystem</param>
		/// <param name="testcase">ID of Testcase</param>
		/// <param name="testsuite">ID of Testsuite</param>
		/// <returns>Returns a refreshtag, so wie can refresh our statustable</returns>
		public ActionResult RerunErrorsOfTest(int testcase)
		{
			if (Session["testsystem"] == null || Session["testsuite"] == null)
				return CreateResultJson(0, 0);

			int testsystem = (int) Session["testsystem"];
			int testsuite = (int) Session["testsuite"];
			int intTester = (int)Session["tester"];

			_objTestService.TestFailedTestsOfTestcaseOfTestsuite(intTester, testsystem,  testsuite, testcase);
			return CreateResultJson(testsystem, testsuite);
		}


		//
		// GET: /History/

		/// <summary>
		/// The History for a Testcasecombination.
		/// </summary>
		/// <param name="testsystem">ID of testsystem</param>
		/// <param name="testsuite">ID of testsuite</param>
		/// <param name="testcase">ID of testcase</param>
		/// <param name="browser">ID of browser</param>
		/// <param name="language">ID of language</param>
		/// <returns>A Testcase View</returns>
		public ActionResult Resulthistory(int testsystem = 0, int testcase = -1, int browser = -1, int language = -1, int testsuite = 0)
		{
			if (testsystem == 0 && Session["testsystem"] != null) testsystem = (int)Session["testsystem"];
			if (testsuite == 0 && Session["testsuite"] != null) testsuite = (int)Session["testsuite"];
			Session["testsystem"] = testsystem;
			Session["testsuite"] = testsuite;

			IList<TestsystemModel> lstTestsystems = Mapper.Map<IList<TestsystemModel>>(_objTestViewerService.GetTestsystems());
			ViewBag.LstTestsystems = lstTestsystems;
			if (testsystem == 0 && lstTestsystems.Count > 0)
			{
				testsystem = lstTestsystems[0].ID;
			}

			IList<TestsuiteModel> lstTestsuites = Mapper.Map<IList<TestsuiteModel>>(_objTestViewerService.GetTestSuites(testsystem));
			ViewBag.LstTestsuites = lstTestsuites;
			if ((testsuite == 0 || lstTestsuites.All(t => t.ID != testsuite)) && lstTestsuites.Count > 0)
			{
				testsuite = lstTestsuites[0].ID;
			}

			TestsuiteModel objCurrentTestsuite = lstTestsuites.Single(t => t.ID == testsuite);

			ViewBag.LstTestcases = objCurrentTestsuite.Testcases;
			if (testcase == 0 && objCurrentTestsuite.Testcases.Count > 0)
			{
				testcase = objCurrentTestsuite.Testcases[0].ID;
			}
			ViewBag.LstBrowsers = objCurrentTestsuite.Browsers;
			ViewBag.LstLanguages = objCurrentTestsuite.Languages;

			ViewBag.Testsystem = testsystem;
			ViewBag.Testsuite = testsuite;
			ViewBag.Testcase = testcase;
			ViewBag.Browser = browser;
			ViewBag.Language = language;
			//ViewBag.LastUpdated = _objTestViewer.GetLastUpdated(testsystem);

			ViewBag.CurrentTestsuite = objCurrentTestsuite;
			ViewBag.CurrentTestsystem = lstTestsystems.Single(t => t.ID == testsystem);

			IList<HistoryResult> objResultsHistory = _objTestViewerService.GetResultsHistory(testsystem, testcase, browser, language, testsuite, 200);
			return View(objResultsHistory);
		}

		/// <summary>
		/// Refresh  the testJobs for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>a actionResult with the testjobs partial</returns>
		public ActionResult RefreshTestJobs(int testsystem)
		{
			IList<TestJobModel> testJobModels = Mapper.Map<List<TestJobModel>>(_objStatusService.GetTestJobsForTestsystem(testsystem));
			return PartialView("TestJobsPartial", testJobModels);
		
		}

		/// <summary>
		/// Refresh the message for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>a actionResult with the message partial</returns>
		public ActionResult RefreshMessage(int testsystem)
		{
			return PartialView("PartialMessage",_objStatusService.GetMessage(testsystem));
		}
	}
}
