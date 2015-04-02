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

		private readonly ISummaryService _objSummaryService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objSummaryService">regtesting service</param>
		public TestsystemSummaryController(ISummaryService objSummaryService)
		{
			_objSummaryService = objSummaryService;
		}

        // 
        // GET: /TestsuiteSummary/
		/// <summary>
		/// Get the Testsummary
		/// </summary>
		/// <returns>View</returns>
        public ActionResult Index()
        {
			
			Dictionary<string, IList<TestsystemSummary>> summaries = new Dictionary<string, IList<TestsystemSummary>>();
			
			summaries.Add("THOR", _objSummaryService.GetTestsystemSummaryForThorMainBranches());
			summaries.Add("SODA", _objSummaryService.GetTestsystemSummaryForSodaMainBranches());

			return View(summaries);
        }

		/// <summary>
		/// Get the branchSummariesTable
		/// </summary>
		/// <returns>a partial view with the summary table</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetThorBranchSummariesTable()
		{
			return PartialView("BranchSummariesTablePartial", _objSummaryService.GetTestsystemSummaryForAllThorBranches().Where(t=>!t.TestsystemName.StartsWith("local",StringComparison.InvariantCultureIgnoreCase)).ToList());
		}

		/// <summary>
		/// Get the branchSummariesTable
		/// </summary>
		/// <returns>a partial view with the summary table</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetSodaBranchSummariesTable()
		{
			return PartialView("BranchSummariesTablePartial", _objSummaryService.GetTestsystemSummaryForAllSodaBranches().Where(t => !t.TestsystemName.StartsWith("local", StringComparison.InvariantCultureIgnoreCase)).ToList());
		}

		/// <summary>
		/// Get the thor main branch summaries
		/// </summary>
		/// <returns>a partial view with the summaries of the main branches</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetThorMainBranchSummaries()
		{
			return PartialView("MainBranchSummariesPartial", _objSummaryService.GetTestsystemSummaryForThorMainBranches());
		}

		/// <summary>
		/// Get the thor main branch summaries
		/// </summary>
		/// <returns>a partial view with the summaries of the main branches</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetSodaMainBranchSummaries()
		{
			return PartialView("MainBranchSummariesPartial", _objSummaryService.GetTestsystemSummaryForSodaMainBranches());
		}
    }
}
