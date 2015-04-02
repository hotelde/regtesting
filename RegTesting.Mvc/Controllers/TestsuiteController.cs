using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Filters;
using RegTesting.Mvc.Models;

namespace RegTesting.Mvc.Controllers
{ 
	/// <summary>
	/// The TestsuiteController
	/// </summary>
	[RegAuthorize]
	public class TestsuiteController : Controller
	{

				
		private readonly ISettingsService _objSettingsService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objSettingsService">regtesting service</param>
		public TestsuiteController(ISettingsService objSettingsService)
		{
			_objSettingsService = objSettingsService;
		}



		//
		// GET: /Testsuite/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testsuite
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<TestsuiteModel>>(_objSettingsService.GetTestsuites()));
		}

		//
		// GET: /Testsuite/Details/5

		/// <summary>
		/// The DetailsView
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A DetailsView</returns>
		public ViewResult Details(int id)
		{
			return View(Mapper.Map<TestsuiteModel>(_objSettingsService.FindTestsuiteByID(id)));
		}

		//
		// GET: /Testsuite/Create

		/// <summary>
		/// Create a new Testsuite
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			IList<TestsuiteModel> lstTestsuites = Mapper.Map<IList<TestsuiteModel>>(_objSettingsService.GetTestsuites().Where(t=>!t.Name.StartsWith("Local ")));
			ViewBag.LstTestsuites = lstTestsuites;
			return View();
		}

		//
		// POST: /Testsuite/Create

		/// <summary>
		/// Created a new Testsuite
		/// </summary>
		/// <param name="createTestsuiteModel">New Testsuite</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Create(CreateTestsuiteModel createTestsuiteModel)
		{
			if (ModelState.IsValid)
			{
				TestsuiteDto objTestsuite = Mapper.Map<TestsuiteDto>(createTestsuiteModel);
				if (createTestsuiteModel.CopyTestsuiteSettings != 0)
				{
					try
					{
						TestsuiteDto objSourceTestsuite = _objSettingsService.FindTestsuiteByID(createTestsuiteModel.CopyTestsuiteSettings);
						objTestsuite.Browsers = objSourceTestsuite.Browsers;
						objTestsuite.Languages = objSourceTestsuite.Languages;
						objTestsuite.Testcases = objSourceTestsuite.Testcases;
					}
					catch
					{
					}
				}
				_objSettingsService.StoreTestsuite(objTestsuite);

				return RedirectToAction("Index");
			}

			return View(createTestsuiteModel);
		}

		//
		// GET: /Testsuite/Edit/5

		/// <summary>
		/// Edit a Testsuite
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A EditView</returns>
		public ActionResult Edit(int id)
		{
			return View(Mapper.Map<TestsuiteModel>(_objSettingsService.FindTestsuiteByID(id)));
		}

		//
		// POST: /Testsuite/Edit/5

		/// <summary>
		/// Edited a Testsuite
		/// </summary>
		/// <param name="testsuiteModel">The edited Testsuite</param>
		/// <returns>The IndexView</returns>
		[HttpPost]
		public ActionResult Edit(TestsuiteModel testsuiteModel)
		{
			if (ModelState.IsValid)
			{
				_objSettingsService.StoreTestsuite(Mapper.Map<TestsuiteDto>(testsuiteModel));
				return RedirectToAction("Index");
			}
			return View(testsuiteModel);
		}

		//
		// GET: /Testsuite/Delete/5

		/// <summary>
		/// Delete a Testsuite
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A DeleteView</returns>
		public ActionResult Delete(int id)
		{
			return View(Mapper.Map<TestsuiteModel>(_objSettingsService.FindTestsuiteByID(id)));
		}

		//
		// POST: /Testsuite/Delete/5

		/// <summary>
		/// Deleted a Testsuite
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>The IndexView</returns>
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			_objSettingsService.DeleteTestsuiteByID(id);
			return RedirectToAction("Index");
		}


		/// <summary>
		/// Edit the Testcases
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A EditTestcasesView</returns>
		public ActionResult EditTestcases(int id)
		{

			IEnumerable<TestcaseModel> lstTestcases = Mapper.Map<IEnumerable<TestcaseModel>>(_objSettingsService.GetTestcases());
			ViewBag.LstTestcases = lstTestcases;
			ViewBag.Testsuite = id;
			IList<int> testcasesInTestsuite = _objSettingsService.FindTestsuiteByID(id).Testcases.Select(t => t.ID).ToList();
			return View(testcasesInTestsuite);
			
		}

		/// <summary>
		/// Testcases have changed
		/// </summary>
		/// <param name="testsuite">ID of testsuite</param>
		/// <param name="testcases">IDs of testcases</param>
		/// <returns>Redirect to EditView</returns>
		public ActionResult TestcasesChanged(int testsuite, ICollection<int> testcases)
		{
			_objSettingsService.SetTestcasesForTestsuite(testsuite, testcases);
			return RedirectToAction("Edit", new { id = testsuite });
		}

		/// <summary>
		/// Edit the Browsers
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A EditBrowsersView</returns>
		public ActionResult EditBrowsers(int id)
		{
			IEnumerable<BrowserModel> lstBrowser = Mapper.Map<IEnumerable<BrowserModel>>(_objSettingsService.GetBrowsers());
			ViewBag.LstBrowsers = lstBrowser;
			ViewBag.Testsuite = id;
			IList<int> browsersInTestsuite = _objSettingsService.FindTestsuiteByID(id).Browsers.Select(t=>t.ID).ToList();
			return View(browsersInTestsuite);
		}

		/// <summary>
		/// Browsers have changed
		/// </summary>
		/// <param name="testsuite">ID of Testsuite</param>
		/// <param name="browsers">IDs of Browsers</param>
		/// <returns>Redirect to EditView</returns>
		public ActionResult BrowsersChanged(int testsuite, ICollection<int> browsers)
		{
			_objSettingsService.SetBrowsersForTestsuite(testsuite, browsers);
			return RedirectToAction("Edit", new { id = testsuite });
		}

		/// <summary>
		/// Edit the Languages
		/// </summary>
		/// <param name="id">ID of Languages</param>
		/// <returns>A EditLanguagesView</returns>
		public ActionResult EditLanguages(int id)
		{
			IEnumerable<LanguageModel> lstLanguages = Mapper.Map<IEnumerable<LanguageModel>>(_objSettingsService.GetLanguages());
			ViewBag.LstLanguages = lstLanguages;
			ViewBag.Testsuite = id;
			IList<int> languagesInTestsuite = _objSettingsService.FindTestsuiteByID(id).Languages.Select(t => t.ID).ToList();
			return View(languagesInTestsuite);
		}

		/// <summary>
		/// Languages have changed
		/// </summary>
		/// <param name="testsuite">ID of Testsuite</param>
		/// <param name="languages">IDs of Languages</param>
		/// <returns>Redirect to EditView</returns>
		public ActionResult LanguagesChanged(int testsuite, ICollection<int> languages)
		{
			_objSettingsService.SetLanguagesForTestsuite(testsuite, languages);
			return RedirectToAction("Edit", new { id = testsuite });
		}


		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testsuite
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Overview()
		{
			IList<TestcaseModel> lstTestcases = Mapper.Map<IList<TestcaseModel>>(_objSettingsService.GetTestcases());
			ViewBag.LstTestcases = lstTestcases;
			IList<BrowserModel> lstBrowsers = Mapper.Map<IList<BrowserModel>>(_objSettingsService.GetBrowsers());
			ViewBag.LstBrowsers = lstBrowsers;
			IList<LanguageModel> lstLanguages = Mapper.Map<IList<LanguageModel>>(_objSettingsService.GetLanguages());
			ViewBag.LstLanguages = lstLanguages;
			return View(Mapper.Map<IEnumerable<TestsuiteModel>>(_objSettingsService.GetTestsuites()));
		}
		
	}
}