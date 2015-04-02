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
		/// <param name="objApplication">
		/// An instance of the <see cref="HttpApplication"/>.
		/// </param>
		public void Init(HttpApplication objApplication)
		{
			objApplication.EndRequest += Application_EndRequest;
		}

		/// <summary>
		/// The EndRequest-event.
		/// </summary>
		/// <param name="objSource">
		/// The source.
		/// </param>
		/// <param name="objEventArgs">
		/// The event arguments.
		/// </param>
		private static void Application_EndRequest(object objSource, EventArgs objEventArgs)
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