using System.Web.Mvc;
using RegTesting.Mvc.Filters;

namespace RegTesting.Mvc.Controllers
{
	/// <summary>
	/// Settingscontroller
	/// </summary>
	[RegAuthorize]
	public class SettingsController : Controller
	{


		//
		// GET: /Settings/
		/// <summary>
		/// Get SettingsView for settings.
		/// </summary>
		/// <returns>Settings View</returns>
		public ActionResult Index()
		{
			return View();
		}

	}
}
