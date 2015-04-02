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
	/// The BrowserController
	/// </summary>
	[RegAuthorize]
    public class BrowserController : Controller
	{
		private readonly ISettingsService _objSettingsService;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objSettingsService">the settingsService</param>
		public BrowserController(ISettingsService objSettingsService)
		{
			_objSettingsService = objSettingsService;
		}


		//
        // GET: /Browser/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Browser
		/// </summary>
		/// <returns>A IndexView</returns>
        public ViewResult Index()
		{
			
			return View(Mapper.Map<IEnumerable<BrowserModel>>(_objSettingsService.GetBrowsers()));
        }

        //
        // GET: /Browser/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Browser</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map <BrowserModel>(_objSettingsService.FindBrowserByID(id)));
		}

		//
		// GET: /Browser/Create

		/// <summary>
		/// Create a new Browser
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Browser/Create

		/// <summary>
		/// Created a new Browser
		/// </summary>
		/// <param name="browserModel">New Browser</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Create(BrowserModel browserModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreBrowser(Mapper.Map<BrowserDto>(browserModel));
				return RedirectToAction("Index");
			}

			return View(browserModel);
		}

		//
		// GET: /Browser/Edit/5

		/// <summary>
		/// Edit a Browser
		/// </summary>
		/// <param name="id">ID of Browser</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<BrowserModel>(_objSettingsService.FindBrowserByID(id)));
		}

		//
		// POST: /Browser/Edit/5

		/// <summary>
		/// Edited a Browser
		/// </summary>
		/// <param name="browserModel">The edited Browser</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(BrowserModel browserModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreBrowser(Mapper.Map<BrowserDto>(browserModel));
				return RedirectToAction("Index");
			}
			return View(browserModel);
		}

		//
		// GET: /Browser/Delete/5

		/// <summary>
		/// Delete a Browser
		/// </summary>
		/// <param name="id">ID of Browser</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<BrowserModel>(_objSettingsService.FindBrowserByID(id)));
		}

		//
		// POST: /Browser/Delete/5

		/// <summary>
		/// Deleted a Browser
		/// </summary>
		/// <param name="id">ID of Browser</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_objSettingsService.DeleteBrowserByID(id);
			return RedirectToAction("Index");
		}

    }
}