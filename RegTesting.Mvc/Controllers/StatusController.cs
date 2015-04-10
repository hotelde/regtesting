using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Filters;
using RegTesting.Mvc.Models;
using StructureMap;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// The StatusController
	/// </summary>
	[RegAuthorize]
	public class StatusController : Controller
	{

		private readonly IStatusService _statusService;

		/// <summary>
		/// Constructor
		/// </summary>
		public StatusController()
		{
			_statusService = ObjectFactory.GetInstance<IStatusService>();
		}




		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="statusService">the status service</param>
		public StatusController(IStatusService statusService)
		{
			_statusService = statusService;
		}

		//
		// GET: /Status/

		/// <summary>
		/// Get StatusView for viewing running and waiting tests in detail.
		/// </summary>
		/// <returns>Status View</returns>
		public ActionResult Index()
		{
			return RedirectToAction("TestJobs");
		}

		/// <summary>
		/// Get the WorkerView
		/// </summary>
		/// <returns>Worker View</returns>
		public ActionResult Worker()
		{
			return View( Mapper.Map<List<TestWorkerModel>>(_statusService.GetTestWorkers()));
		}

		/// <summary>
		/// Refresh the PartialView PartialStatus
		/// </summary>
		/// <returns>A PartialView of type PartialStatus</returns>
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult RefreshWorkerStatus()
		{
			return PartialView("PartialWorkerStatus", Mapper.Map<List<TestWorkerModel>>(_statusService.GetTestWorkers()));
		}

		/// <summary>
		/// Get StatusView for viewing running and waiting tests in detail.
		/// </summary>
		/// <returns>Status View</returns>
		public ActionResult TestJobs()
		{
			IList<TestJobModel> testJobModels = Mapper.Map<List<TestJobModel>>(_statusService.GetTestJobs());

			if (Request.IsAjaxRequest())
			{
				return PartialView("TestJobsPartial", testJobModels);
			}
			return View(testJobModels);
		}

		/// <summary>
		/// Cancel a WorkItemGroup
		/// </summary>
		/// <param name="testjob">ID of the testjob</param>
		/// <returns>Redirect to TestJobs View</returns>
		public ActionResult CancelWorkItemGroup(int testjob)
		{
			_statusService.CancelTestJob(testjob);
			return RedirectToAction("TestJobs");
		}

		/// <summary>
		/// priorize a testjob
		/// </summary>
		/// <param name="testjob">the testjobID</param>
		/// <returns>returns to the testJobs site</returns>
		public ActionResult PrioTestJob(int testjob)
		{
			_statusService.PrioTestJob(testjob);
			return RedirectToAction("TestJobs");
		}

		/// <summary>
		/// Reboot a worker by name
		/// </summary>
		/// <param name="node">the workerName</param>
		/// <returns>returns to the worker site</returns>
		public ActionResult RebootWorker(string node)
		{
			_statusService.RebootWorker(node);
			return RedirectToAction("Worker");

		}


		/// <summary>
		/// Reboot all workers
		/// </summary>
		/// <returns>returns to the worker site</returns>
		public ActionResult RebootAllWorker()
		{
			_statusService.RebootAllWorker();
			return RedirectToAction("Worker");

		}
	}
}
