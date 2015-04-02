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
	/// The TestsystemController
	/// </summary>
	[RegAuthorize]
	public class TestsystemController : Controller
	{
		private readonly ISettingsService _objSettingsService;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objSettingsService">the settingsService</param>
		public TestsystemController(ISettingsService objSettingsService)
		{
			_objSettingsService = objSettingsService;
		}


		//
		// GET: /Testsystem/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testsystem
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<TestsystemModel>>(_objSettingsService.GetTestsystems()));
		}

		//
		// GET: /Testsystem/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Testsystem</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map<TestsystemModel>(_objSettingsService.FindTestsystemByID(id)));
		}

		//
		// GET: /Testsystem/Create

		/// <summary>
		/// Create a new Testsystem
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Testsystem/Create

		/// <summary>
		/// Created a new Testsystem
		/// </summary>
		/// <param name="TestsystemModel">New Testsystem</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Create(TestsystemModel TestsystemModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreTestsystem(Mapper.Map<TestsystemDto>(TestsystemModel));
				return RedirectToAction("Index");
			}

			return View(TestsystemModel);
		}

		//
		// GET: /Testsystem/Edit/5

		/// <summary>
		/// Edit a Testsystem
		/// </summary>
		/// <param name="id">ID of Testsystem</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<TestsystemModel>(_objSettingsService.FindTestsystemByID(id)));
		}

		//
		// POST: /Testsystem/Edit/5

		/// <summary>
		/// Edited a Testsystem
		/// </summary>
		/// <param name="TestsystemModel">The edited Testsystem</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(TestsystemModel TestsystemModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreTestsystem(Mapper.Map<TestsystemDto>(TestsystemModel));
				return RedirectToAction("Index");
			}
			return View(TestsystemModel);
		}

		//
		// GET: /Testsystem/Delete/5

		/// <summary>
		/// Delete a Testsystem
		/// </summary>
		/// <param name="id">ID of Testsystem</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<TestsystemModel>(_objSettingsService.FindTestsystemByID(id)));
		}

		//
		// POST: /Testsystem/Delete/5

		/// <summary>
		/// Deleted a Testsystem
		/// </summary>
		/// <param name="id">ID of Testsystem</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_objSettingsService.DeleteTestsystemByID(id);
			return RedirectToAction("Index");
		}

	}
}