using System;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// Model for a testsystem summary
	/// </summary>
	public class TestsystemSummary
	{
		/// <summary>
		/// The Testsuite
		/// </summary>
		public string TestsuiteName { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public int TestsuiteID { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public string TestsystemName { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public int TestsystemID { get; set; }

		/// <summary>
		/// The Date when the testsystem changed
		/// </summary>
		public DateTime LastChangeDate { get; set; }

		/// <summary>
		/// The Status of the TestsystemStatus
		/// </summary>
		public TestState TestsystemStatus { get; set; }

	}
}