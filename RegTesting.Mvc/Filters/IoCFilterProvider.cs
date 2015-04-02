using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RegTesting.Mvc.Filters
{
	/// <summary>
	/// <see cref="IFilterProvider"/> decorates the 
	/// <see cref="FilterProviders.Providers"/> and calls
	/// the Dependecy Resolver to create a new filter instance when 
	/// the Filter, containing in the filter collection 
	/// is of type <typeparamref name="TFilter"/>.
	///  </summary>
	/// <typeparam name="TFilter">The marker type of IoC Filter.</typeparam>
	public class IoCFilterProvider<TFilter> : IFilterProvider
	{
		#region Non Public Members

		private readonly IFilterProvider _objFilterProvider;
		private readonly IDependencyResolver _objDependencyResolver;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="IoCFilterProvider{T}"/> class.
		/// </summary>
		/// <param name="objFilterProvider">The Provider to decorate.</param>
		/// <param name="objDependencyResolver">The Resolver used to create new instances of Filter.</param>
		public IoCFilterProvider(IFilterProvider objFilterProvider, IDependencyResolver objDependencyResolver = null)
		{
			_objFilterProvider = objFilterProvider;
			_objDependencyResolver = objDependencyResolver ?? DependencyResolver.Current;
		}

		#endregion

		#region Public Methods

		IEnumerable<Filter> IFilterProvider.GetFilters(ControllerContext objControllerContext, ActionDescriptor objActionDescriptor)
		{
			return _objFilterProvider.GetFilters(objControllerContext, objActionDescriptor).Select(objFilter => (objFilter.Instance is TFilter) ? new Filter(_objDependencyResolver.GetService(objFilter.Instance.GetType()), objFilter.Scope, objFilter.Order) : objFilter);
		}

		/// <summary>
		/// Decorates all filter within the <paramref name="lstFilterProviders"/>.
		/// </summary>
		/// <param name="lstFilterProviders">The list of filter providers. optional. If not set, the default <see cref="FilterProviders.Providers"/> will be used.</param>
		/// <param name="objDependencyResolver">Optional dependency resolver. IF not set, the default <see cref="DependencyResolver.Current"/> will be used.</param>
		public static void ApplyToAllFilters(IList<IFilterProvider> lstFilterProviders = null, IDependencyResolver objDependencyResolver = null)
		{
			lstFilterProviders = lstFilterProviders ?? FilterProviders.Providers;

			for (int i = 0; i < lstFilterProviders.Count; i++)
				lstFilterProviders[i] = new IoCFilterProvider<TFilter>(lstFilterProviders[i], objDependencyResolver ?? DependencyResolver.Current);
		}

		#endregion

	}
}