using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using AutoMapper;
using NHibernate.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Models;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// TestsystemSummaryController class
	/// </summary>
    public class TestjobsController : Controller
    {

		private readonly ISummaryService _summaryService;
		private readonly ITestViewerService _testViewerService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="summaryService">The summary service</param>
		/// <param name="testViewerService">The testViewer service</param>
		public TestjobsController(ISummaryService summaryService, ITestViewerService testViewerService)
		{
			if (summaryService == null)
				throw new ArgumentNullException("summaryService");
			if (testViewerService == null)
				throw new ArgumentNullException("testViewerService");
			_summaryService = summaryService;
			_testViewerService = testViewerService;
		}

		// 
        // GET: /TestsuiteSummary/
		/// <summary>
		/// Get the Testsummary
		/// </summary>
		/// <returns>View</returns>
		public ActionResult Index(int? testjob)
		{


			ViewBag.testjob = testjob;


			IList<TestJobModel> testJobModels = Mapper.Map<IList<TestJobModel>>(_summaryService.GetTestJobs());
			testJobModels.ForEach(ParseDescription);
			return View(testJobModels);


        }

		private void ParseDescription(TestJobModel testJobModel)
		{
			if (string.IsNullOrEmpty(testJobModel.Description))
				return;

			string[] descriptionParts = testJobModel.Description.Split(';');


			testJobModel.CommitMessage = "";
			for (int i = 0; i < descriptionParts.Length; i++)
			{
				var descriptionLine = descriptionParts[i];
				if (i == 0 && descriptionLine.StartsWith("commit "))
				{
					//skip full commit id
				}
				else if (descriptionLine.StartsWith("Author"))
				{
					testJobModel.CommitAuthor = descriptionLine.Replace("Author: ", "");
				}
				else if (descriptionLine.StartsWith("Date"))
				{
					testJobModel.CommitDate = descriptionLine.Replace("Date: ", "");
				}
				else if (descriptionLine.StartsWith("Merge: "))
				{
					testJobModel.CommitMergeInfo = descriptionLine.Replace("Merge: ", "");
				}
				else
				{
					testJobModel.CommitMessage +=  descriptionLine + "\n";
				}
			}
		}

		public ActionResult Details(int testjob)
		{
			return PartialView(_testViewerService.GetErrorOccurrenceGroupsForTestjob(testjob));
		}


    }
}
