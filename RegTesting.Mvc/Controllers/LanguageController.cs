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
	/// The LanguageController
	/// </summary>
	[RegAuthorize]
	public class LanguageController : Controller
	{
		private readonly ISettingsService _objSettingsService;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objSettingsService">the settingsService</param>
		public LanguageController(ISettingsService objSettingsService)
		{
			_objSettingsService = objSettingsService;
		}


		//
		// GET: /Language/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Language
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<LanguageModel>>(_objSettingsService.GetLanguages()));
		}

		//
		// GET: /Language/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Language</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map<LanguageModel>(_objSettingsService.FindLanguageByID(id)));
		}

		//
		// GET: /Language/Create

		/// <summary>
		/// Create a new Language
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Language/Create

		/// <summary>
		/// Created a new Language
		/// </summary>
		/// <param name="LanguageModel">New Language</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Create(LanguageModel LanguageModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreLanguage(Mapper.Map<LanguageDto>(LanguageModel));
				return RedirectToAction("Index");
			}

			return View(LanguageModel);
		}

		//
		// GET: /Language/Edit/5

		/// <summary>
		/// Edit a Language
		/// </summary>
		/// <param name="id">ID of Language</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<LanguageModel>(_objSettingsService.FindLanguageByID(id)));
		}

		//
		// POST: /Language/Edit/5

		/// <summary>
		/// Edited a Language
		/// </summary>
		/// <param name="LanguageModel">The edited Language</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(LanguageModel LanguageModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreLanguage(Mapper.Map<LanguageDto>(LanguageModel));
				return RedirectToAction("Index");
			}
			return View(LanguageModel);
		}

		//
		// GET: /Language/Delete/5

		/// <summary>
		/// Delete a Language
		/// </summary>
		/// <param name="id">ID of Language</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<LanguageModel>(_objSettingsService.FindLanguageByID(id)));
		}

		//
		// POST: /Language/Delete/5

		/// <summary>
		/// Deleted a Language
		/// </summary>
		/// <param name="id">ID of Language</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_objSettingsService.DeleteLanguageByID(id);
			return RedirectToAction("Index");
		}

	}
}