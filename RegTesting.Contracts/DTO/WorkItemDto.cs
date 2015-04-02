using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// the workItemDto
	/// </summary>
	public class WorkItemDto
	{
		/// <summary>
		/// The Browser
		/// </summary>
		public BrowserDto Browser { get; set; }

		/// <summary>
		/// The language
		/// </summary>
		public LanguageDto Language { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public TestsystemDto Testsystem { get; set; }

		/// <summary>
		/// The Testcase
		/// </summary>
		public TestcaseDto Testcase { get; set; }

	}
}
