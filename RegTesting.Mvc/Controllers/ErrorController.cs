using RegTesting.Contracts.Domain;
using System.Web.Mvc;
using RegTesting.Contracts.Services;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// A ErrorController
	/// </summary>
	public class ErrorController : Controller
	{

		
		private readonly ISettingsService _settingsService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="settingsService">regtesting service</param>
		public ErrorController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}




		//
		// GET: /Error/

		/// <summary>
		/// The IndexView of the ErrorController
		/// </summary>
		/// <returns>A Index View</returns>
		public ActionResult Index()
		{
			return View();
		}



		/// <summary>
		/// Edit an error
		/// </summary>
		/// <param name="error">the error to edit</param>
		/// <returns>a Edit error view</returns>
		public ActionResult Edit(int error)
		{
			Error error;

			error = _settingsService.GetError(error);
			
			return View("Edit", error);
		}


		/// <summary>
		/// Save the edit of a error
		/// </summary>
		/// <param name="error">Error to save</param>
		/// <returns>The TestingLog View</returns>
		[HttpPost]
		public ActionResult Edit(Error error)
		{
			if (ModelState.IsValid)
			{
				_settingsService.EditError(error);
				return RedirectToAction("Index","Testing");
			}
			return View(error);
		}

	}
}
