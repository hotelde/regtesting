using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RegTesting.Contracts;
using RegTesting.Contracts.Services;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// TestsystemSummaryController class
	/// </summary>
	public class TestsystemSummaryController : Controller
	{

		private readonly ISummaryService _summaryService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="summaryService">regtesting service</param>
		public TestsystemSummaryController(ISummaryService summaryService)
		{
			_summaryService = summaryService;
		}

		// 
		// GET: /TestsuiteSummary/
		/// <summary>
		/// Get the Testsummary
		/// </summary>
		/// <returns>View</returns>
		public ActionResult Index()
		{
			return View(_summaryService.GetPinnedTestsystemSummaries());
		}

		/// <summary>
		/// Get the branchSummariesTable
		/// </summary>
		/// <returns>a partial view with the summary table</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetLastBranchSummariesTable()
		{
			return PartialView("BranchSummariesTablePartial", _summaryService.GetLastTestsystemSummaries().Where(t=>!t.TestsystemName.StartsWith("local",StringComparison.InvariantCultureIgnoreCase)).ToList());
		}

		/// <summary>
		/// Get the pinned branch summaries
		/// </summary>
		/// <returns>a partial view with the summaries of the pinned branches</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetPinnedBranchSummaries()
		{
            return PartialView("MainBranchSummariesPartial", _summaryService.GetPinnedTestsystemSummaries());
		}

	}
}
