using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// SummaryService Interface
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/summaryservice", ConfigurationName = "ISummaryService")]
	public interface ISummaryService
	{
		/// <summary>
		/// Get the testsystemsummaries for all branches
		/// </summary>
		/// <returns>a list of testsystemsummaries</returns>
		[OperationContract]
        IList<TestsystemSummary> GetLastTestsystemSummaries();

		/// <summary>
		/// Get the testsystemsummaries for the pinned branches
		/// </summary>
		/// <returns>a list of testsystemsummaries</returns>
		[OperationContract]
        IList<TestsystemSummary> GetPinnedTestsystemSummaries();

		/// <summary>
		/// Get the testsystemsummaries for the pinned branches
		/// </summary>
		/// <returns>a list of testsystemsummaries</returns>
		[OperationContract]
		IList<TestJobDto> GetTestJobs();


	}
}
