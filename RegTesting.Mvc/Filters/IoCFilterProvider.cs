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

		private readonly IFilterProvider _filterProvider;
		private readonly IDependencyResolver _dependencyResolver;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="IoCFilterProvider{T}"/> class.
		/// </summary>
		/// <param name="filterProvider">The Provider to decorate.</param>
		/// <param name="dependencyResolver">The Resolver used to create new instances of Filter.</param>
		public IoCFilterProvider(IFilterProvider filterProvider, IDependencyResolver dependencyResolver = null)
		{
			_filterProvider = filterProvider;
			_dependencyResolver = dependencyResolver ?? DependencyResolver.Current;
		}

		#endregion

		#region Public Methods

		IEnumerable<Filter> IFilterProvider.GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			return _filterProvider.GetFilters(controllerContext, actionDescriptor).Select(objFilter => (objFilter.Instance is TFilter) ? new Filter(_dependencyResolver.GetService(objFilter.Instance.GetType()), objFilter.Scope, objFilter.Order) : objFilter);
		}

		/// <summary>
		/// Decorates all filter within the <paramref name="filterProviders"/>.
		/// </summary>
		/// <param name="filterProviders">The list of filter providers. optional. If not set, the default <see cref="FilterProviders.Providers"/> will be used.</param>
		/// <param name="dependencyResolver">Optional dependency resolver. IF not set, the default <see cref="DependencyResolver.Current"/> will be used.</param>
		public static void ApplyToAllFilters(IList<IFilterProvider> filterProviders = null, IDependencyResolver dependencyResolver = null)
		{
			filterProviders = filterProviders ?? FilterProviders.Providers;

			for (int i = 0; i < filterProviders.Count; i++)
				filterProviders[i] = new IoCFilterProvider<TFilter>(filterProviders[i], dependencyResolver ?? DependencyResolver.Current);
		}

		#endregion

	}
}