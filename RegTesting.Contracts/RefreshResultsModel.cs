using System.Collections.Generic;

namespace RegTesting.Contracts
{
	/// <summary>
	/// The RefreshResultsModel
	/// </summary>
	public class RefreshResultsModel
	{
		/// <summary>
		/// The Results to update
		/// </summary>
		public IList<GroupedResult> Results { get; set; }

		/// <summary>
		/// the time in ticks, until when results are included.
		/// </summary>
		public long ResultsUntil { get; set; }
	}
}
