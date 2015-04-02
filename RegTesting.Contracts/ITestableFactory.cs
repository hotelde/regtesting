namespace RegTesting.Contracts
{
	/// <summary>
	/// The ITestableFactory
	/// </summary>
	public interface ITestableFactory
	{
		/// <summary>
		/// Create a Factory
		/// </summary>
		/// <param name="strAssemblyFile">the assemblyFile</param>
		/// <param name="strTypeName">the type to create</param>
		/// <param name="objConstructArgs">constructor arguments</param>
		/// <returns>A new class of type typeName or null in error case</returns>
		ITestable Create(string strAssemblyFile, string strTypeName, object[] objConstructArgs);
	}
}
