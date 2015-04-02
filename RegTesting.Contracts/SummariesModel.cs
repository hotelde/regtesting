using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegTesting.Contracts
{
	/// <summary>
	/// Model for testsystem summaries
	/// </summary>
	public class SummariesModel
	{
		/// <summary>
		/// Summaries of thor branches
		/// </summary>
		public IList<TestsystemSummary> ThorSummaries { get; set; }

		/// <summary>
		/// Summaries of soda branches
		/// </summary>
		public IList<TestsystemSummary> SodaSummaries { get; set; }

	}
}
