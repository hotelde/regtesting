using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{

	/// <summary>
	/// TestcaseDetails
	/// </summary>
	public class TestcaseDetailsModel
	{
		/// <summary>
		/// The Testcase
		/// </summary>
		public Testcase Testcase { get; set; }

		/// <summary>
		/// The ErrorOccurrenceGroup
		/// </summary>
		public ErrorOccurrenceGroup ErrorOccurrenceGroup { get; set; }
	}
}
