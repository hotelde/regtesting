namespace RegTesting.Tests.Core
{
	/// <summary>
	/// The ITestableFactory
	/// </summary>
	public interface ITestableFactory
	{
		/// <summary>
		/// Create a Factory
		/// </summary>
		/// <param name="assemblyFile">the assemblyFile</param>
		/// <param name="typeName">the type to create</param>
		/// <param name="constructArgs">constructor arguments</param>
		/// <returns>A new class of type typeName or null in error case</returns>
		ITestable Create(string assemblyFile, string typeName, object[] constructArgs);
	}
}
