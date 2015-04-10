using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Filters;
using RegTesting.Mvc.Models;

namespace RegTesting.Mvc.Controllers
{

	/// <summary>
	/// The TestcaseController
	/// </summary>
	[RegAuthorize]
	public class TestcaseController : Controller
	{
		private readonly ISettingsService _settingsService;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="settingsService">the settingsService</param>
		public TestcaseController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}


		//
		// GET: /Testcase/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testcase
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<TestcaseModel>>(_settingsService.GetTestcases()));
		}

		//
		// GET: /Testcase/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Testcase</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map<TestcaseModel>(_settingsService.FindTestcaseByID(id)));
		}

		//
		// GET: /Testcase/Edit/5

		/// <summary>
		/// Edit a Testcase
		/// </summary>
		/// <param name="id">ID of Testcase</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<TestcaseModel>(_settingsService.FindTestcaseByID(id)));
		}

		//
		// POST: /Testcase/Edit/5

		/// <summary>
		/// Edited a Testcase
		/// </summary>
		/// <param name="TestcaseModel">The edited Testcase</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(TestcaseModel TestcaseModel)
		{
			if (ModelState.IsValid)
			{
				_settingsService.EditTestcase(Mapper.Map<TestcaseDto>(TestcaseModel));
				return RedirectToAction("Index");
			}
			return View(TestcaseModel);
		}

		//
		// GET: /Testcase/Delete/5

		/// <summary>
		/// Delete a Testcase
		/// </summary>
		/// <param name="id">ID of Testcase</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<TestcaseModel>(_settingsService.FindTestcaseByID(id)));
		}

		//
		// POST: /Testcase/Delete/5

		/// <summary>
		/// Deleted a Testcase
		/// </summary>
		/// <param name="id">ID of Testcase</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_settingsService.DeleteTestcaseByID(id);
			return RedirectToAction("Index");
		}

	}
}