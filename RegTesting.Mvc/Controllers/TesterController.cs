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
	/// The TesterController
	/// </summary>
	[RegAuthorize]
	public class TesterController : Controller
	{
		private readonly ISettingsService _settingsService;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="settingsService">the settingsService</param>
		public TesterController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}


		//
		// GET: /Tester/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Tester
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<TesterModel>>(_settingsService.GetTesters()));
		}

		//
		// GET: /Tester/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Tester</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map<TesterModel>(_settingsService.FindTesterByID(id)));
		}

		//
		// GET: /Tester/Create

		/// <summary>
		/// Create a new Tester
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Tester/Create

		/// <summary>
		/// Created a new Tester
		/// </summary>
		/// <param name="TesterModel">New Tester</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Create(TesterModel TesterModel)
		{
			if (ModelState.IsValid)
			{
				_settingsService.StoreTester(Mapper.Map<TesterDto>(TesterModel));
				return RedirectToAction("Index");
			}

			return View(TesterModel);
		}

		//
		// GET: /Tester/Edit/5

		/// <summary>
		/// Edit a Tester
		/// </summary>
		/// <param name="id">ID of Tester</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<TesterModel>(_settingsService.FindTesterByID(id)));
		}

		//
		// POST: /Tester/Edit/5

		/// <summary>
		/// Edited a Tester
		/// </summary>
		/// <param name="TesterModel">The edited Tester</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(TesterModel TesterModel)
		{
			if (ModelState.IsValid)
			{
				_settingsService.StoreTester(Mapper.Map<TesterDto>(TesterModel));
				return RedirectToAction("Index");
			}
			return View(TesterModel);
		}

		//
		// GET: /Tester/Delete/5

		/// <summary>
		/// Delete a Tester
		/// </summary>
		/// <param name="id">ID of Tester</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<TesterModel>(_settingsService.FindTesterByID(id)));
		}

		//
		// POST: /Tester/Delete/5

		/// <summary>
		/// Deleted a Tester
		/// </summary>
		/// <param name="id">ID of Tester</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_settingsService.DeleteTesterByID(id);
			return RedirectToAction("Index");
		}

	}
}