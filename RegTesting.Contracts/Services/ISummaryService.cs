using System.Collections.Generic;
using System.ServiceModel;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// SummaryService Interface
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/summaryservice", ConfigurationName = "ISummaryService")]
	public interface ISummaryService
	{
		/// <summary>
		/// Get the testsystemsummarys for thor branches
		/// </summary>
		/// <returns>a list of testsystemsummarys</returns>
		[OperationContract]
		IList<TestsystemSummary> GetTestsystemSummaryForAllThorBranches();

		/// <summary>
		/// Get the testsystemsummarys for soda branches
		/// </summary>
		/// <returns>a list of testsystemsummarys</returns>
		[OperationContract]
		IList<TestsystemSummary> GetTestsystemSummaryForAllSodaBranches();

		/// <summary>
		/// Get the testsystemsummarys for the THOR main branches
		/// </summary>
		/// <returns>a list of testsystemsummarys</returns>
		[OperationContract]
		IList<TestsystemSummary> GetTestsystemSummaryForThorMainBranches();


		/// <summary>
		/// Get the testsystemsummarys for the SODA main branches
		/// </summary>
		/// <returns>a list of testsystemsummarys</returns>
		[OperationContract]
		IList<TestsystemSummary> GetTestsystemSummaryForSodaMainBranches();



	}
}
