using System.Collections.Generic;

namespace RegTesting.Contracts
{

	/// <summary>
	/// The groupedResult
	/// </summary>
	public class GroupedResult
	{
		/// <summary>
		/// The testcase id
		/// </summary>
		public int Testcase { get; set; }

		/// <summary>
		/// List of grouped results
		/// </summary>
		public List<GroupedResultPart> GroupedResultParts = new List<GroupedResultPart>();



	}
}
	