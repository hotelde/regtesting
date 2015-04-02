using System.Web.Mvc;

namespace RegTesting.Mvc.Filters
{
	/// <summary>
	/// Marker Interface for <see cref="Filter"/>.
	/// When filter is marked with that interface the filter will
	/// be created via IoC and constructor injection is possible.
	/// </summary>
	public interface IIoCFilter
	{
	}
}
