
namespace RegTesting.Tests.Core
{

	/// <summary>
	/// The interface for a TypesLoaderFactory
	/// </summary>
	public interface ITypesLoaderFactory
	{
		/// <summary>
		/// Create a Factory
		/// </summary>
		/// <param name="assemblyFile">the Assembly</param>
		/// <param name="typeName">the type to create</param>
		/// <param name="constructArgs">constructor arguments</param>
		/// <returns>A new class of type typeName</returns>
		ITypesLoader Create(string assemblyFile, string typeName, object[] constructArgs);
	}
}
