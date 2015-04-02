using System;
using System.Web.Mvc;
using System.Web.Routing;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.App_Start;
using RegTesting.Mvc.Filters;
using RegTesting.Service;
using StructureMap;

namespace RegTesting.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

	/// <summary>
	/// Defines our Mvcapplication
	/// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {

		/// <summary>
		/// Registers global filters
		/// </summary>
		/// <param name="filters">filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

		/// <summary>
		/// Register routes
		/// </summary>
		/// <param name="routes">routes</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "Log", // Route name
                "log/{action}", // URL with parameters
                new { controller = "TestingLog", action = "Index" } // Parameter defaults
             );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "TestsystemSummary", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
		/// <summary>
		/// Starts the application
		/// </summary>
        protected void Application_Start()
		{
			ApplicationContext.AppConfigure();
			StructuremapMvc.Start();
			AutoMapperInit.CreateMappings();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

			IoCFilterProvider<IIoCFilter>.ApplyToAllFilters();

			ObjectFactory.GetInstance<ITestService>();
		}

		/// <summary>
		/// Callback for end of requests
		/// </summary>
		/// <param name="sender">the sender</param>
		/// <param name="e">the eventArgs</param>
		protected void Application_EndRequest(object sender, EventArgs e)
		{
			//Dispose all httpScoped objects.
			ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
		}

    }
}