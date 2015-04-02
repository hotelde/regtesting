using System;
using System.Web;
using System.Web.Mvc;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.Extensions;

namespace RegTesting.Mvc.Filters
{

	/// <summary>
	/// A Attribute for Authorize
	/// </summary>
	public class RegAuthorizeAttribute : AuthorizeAttribute, IIoCFilter
	{
		private readonly ITestViewerService _objTestViewerService;

		/// <summary>
		/// Default constructor for global.asax
		/// </summary>
		public RegAuthorizeAttribute()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objTestViewerService">the test viewer</param>
		public RegAuthorizeAttribute(ITestViewerService objTestViewerService) :this()
		{
			if (objTestViewerService == null)
				throw new ArgumentNullException("objTestViewerService");
			_objTestViewerService = objTestViewerService;
		}

		/// <summary>
		/// Add the ID of the current tester while authorizing.
		/// </summary>
		/// <param name="httpContext">the current httpContext</param>
		/// <returns>boolean, if the authorize was successful</returns>
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool bolAuthorized = base.AuthorizeCore(httpContext);
			if (!bolAuthorized)
			{
				return false;
			}
			if (httpContext.Session != null)
			{
				if (httpContext.Session["tester"] == null)
				{

					httpContext.Session["tester"] = _objTestViewerService.GetTesterIDByName(httpContext.User.Identity.GetLogin());
				}
			}
			return true;
		}
	}
}