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

				
		private readonly ISettingsService _settingsService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="settingsService">regtesting service</param>
		public TestsuiteController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}



		//
		// GET: /Testsuite/

		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testsuite
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Index()
		{

			return View(Mapper.Map<IEnumerable<TestsuiteModel>>(_settingsService.GetTestsuites()));
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
			return View(Mapper.Map<TestsuiteModel>(_settingsService.FindTestsuiteByID(id)));
		}

		//
		// GET: /Testsuite/Create

		/// <summary>
		/// Create a new Testsuite
		/// </summary>
		/// <returns>A CreateView</returns>
		public ActionResult Create()
		{
			IList<TestsuiteModel> lstTestsuites = Mapper.Map<IList<TestsuiteModel>>(_settingsService.GetTestsuites().Where(t=>!t.Name.StartsWith("Local ")));
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
				TestsuiteDto testsuite = Mapper.Map<TestsuiteDto>(createTestsuiteModel);
				if (createTestsuiteModel.CopyTestsuiteSettings != 0)
				{
					try
					{
						TestsuiteDto sourceTestsuite = _settingsService.FindTestsuiteByID(createTestsuiteModel.CopyTestsuiteSettings);
						testsuite.Browsers = sourceTestsuite.Browsers;
						testsuite.Languages = sourceTestsuite.Languages;
						testsuite.Testcases = sourceTestsuite.Testcases;
					}
					catch
					{
					}
				}
				_settingsService.StoreTestsuite(testsuite);

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
			return View(Mapper.Map<TestsuiteModel>(_settingsService.FindTestsuiteByID(id)));
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
				_settingsService.StoreTestsuite(Mapper.Map<TestsuiteDto>(testsuiteModel));
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
			return View(Mapper.Map<TestsuiteModel>(_settingsService.FindTestsuiteByID(id)));
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
			_settingsService.DeleteTestsuiteByID(id);
			return RedirectToAction("Index");
		}


		/// <summary>
		/// Edit the Testcases
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A EditTestcasesView</returns>
		public ActionResult EditTestcases(int id)
		{

			IEnumerable<TestcaseModel> testcases = Mapper.Map<IEnumerable<TestcaseModel>>(_settingsService.GetTestcases());
			ViewBag.LstTestcases = testcases;
			ViewBag.Testsuite = id;
			IList<int> testcasesInTestsuite = _settingsService.FindTestsuiteByID(id).Testcases.Select(t => t.ID).ToList();
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
			_settingsService.SetTestcasesForTestsuite(testsuite, testcases);
			return RedirectToAction("Edit", new { id = testsuite });
		}

		/// <summary>
		/// Edit the Browsers
		/// </summary>
		/// <param name="id">ID of Testsuite</param>
		/// <returns>A EditBrowsersView</returns>
		public ActionResult EditBrowsers(int id)
		{
			IEnumerable<BrowserModel> browser = Mapper.Map<IEnumerable<BrowserModel>>(_settingsService.GetBrowsers());
			ViewBag.LstBrowsers = browser;
			ViewBag.Testsuite = id;
			IList<int> browsersInTestsuite = _settingsService.FindTestsuiteByID(id).Browsers.Select(t=>t.ID).ToList();
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
			_settingsService.SetBrowsersForTestsuite(testsuite, browsers);
			return RedirectToAction("Edit", new { id = testsuite });
		}

		/// <summary>
		/// Edit the Languages
		/// </summary>
		/// <param name="id">ID of Languages</param>
		/// <returns>A EditLanguagesView</returns>
		public ActionResult EditLanguages(int id)
		{
			IEnumerable<LanguageModel> languages = Mapper.Map<IEnumerable<LanguageModel>>(_settingsService.GetLanguages());
			ViewBag.LstLanguages = languages;
			ViewBag.Testsuite = id;
			IList<int> languagesInTestsuite = _settingsService.FindTestsuiteByID(id).Languages.Select(t => t.ID).ToList();
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
			_settingsService.SetLanguagesForTestsuite(testsuite, languages);
			return RedirectToAction("Edit", new { id = testsuite });
		}


		/// <summary>
		///  The IndexView: Managing, Creating and Editing Testsuite
		/// </summary>
		/// <returns>A IndexView</returns>
		public ViewResult Overview()
		{
			IList<TestcaseModel> testcases = Mapper.Map<IList<TestcaseModel>>(_settingsService.GetTestcases());
			ViewBag.LstTestcases = testcases;
			IList<BrowserModel> browsers = Mapper.Map<IList<BrowserModel>>(_settingsService.GetBrowsers());
			ViewBag.LstBrowsers = browsers;
			IList<LanguageModel> languages = Mapper.Map<IList<LanguageModel>>(_settingsService.GetLanguages());
			ViewBag.LstLanguages = languages;
			return View(Mapper.Map<IEnumerable<TestsuiteModel>>(_settingsService.GetTestsuites()));
		}
		
	}
}