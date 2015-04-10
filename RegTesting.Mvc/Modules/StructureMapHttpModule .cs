using System;
using System.Web;
using StructureMap;

namespace RegTesting.Mvc.Modules
{
	internal class StructureMapHttpModule : IHttpModule
	{
		/// <summary>
		/// Initialization method for the <see cref="IHttpModule"/>.
		/// </summary>
		/// <param name="application">
		/// An instance of the <see cref="HttpApplication"/>.
		/// </param>
		public void Init(HttpApplication application)
		{
			application.EndRequest += Application_EndRequest;
		}

		/// <summary>
		/// The EndRequest-event.
		/// </summary>
		/// <param name="source">
		/// The source.
		/// </param>
		/// <param name="eventArgs">
		/// The event arguments.
		/// </param>
		private static void Application_EndRequest(object source, EventArgs eventArgs)
		{
			ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		public void Dispose()
		{
		}
	}

}