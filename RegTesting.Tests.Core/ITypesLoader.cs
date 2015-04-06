namespace RegTesting.Tests.Core
{
	/// <summary>
	/// Interface for the TypesLoader
	/// </summary>
	public interface ITypesLoader
	{
		/// <summary>
		/// Get all Types of the Testcases
		/// </summary>
		/// <returns>String array with all Typenames</returns>
        string[] GetTypes(string loadAssembly);
	}
}
