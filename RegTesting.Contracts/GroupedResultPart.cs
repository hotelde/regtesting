
namespace RegTesting.Contracts
{

	/// <summary>
	/// GroupedResult
	/// </summary>
	public class GroupedResultPart
	{
		/// <summary>
		/// The Browser
		/// </summary>
		public int Browser { get; set; }

		/// <summary>
		/// The Language
		/// </summary>
		public int Language { get; set; }

		/// <summary>
		/// The ResultCode
		/// </summary>
		public TestState ResultCode { get; set; }

		/// <summary>
		/// The ResultLabel
		/// </summary>
		public string ResultLabel { get; set; } 
	}
}
