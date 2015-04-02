
namespace RegTesting.Contracts
{

	/// <summary>
	/// The interface for a TypesLoaderFactory
	/// </summary>
	public interface ITypesLoaderFactory
	{
		/// <summary>
		/// Create a Factory
		/// </summary>
		/// <param name="strAssemblyFile">the Assembly</param>
		/// <param name="strTypeName">the type to create</param>
		/// <param name="objConstructArgs">constructor arguments</param>
		/// <returns>A new class of type typeName</returns>
		ITypesLoader Create(string strAssemblyFile, string strTypeName, object[] objConstructArgs);
	}
}
