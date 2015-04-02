using System.Collections.Generic;
using RegTesting.Contracts.Enums;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// the testWorkerModel
	/// </summary>
	public class TestWorkerModel
	{
		/// <summary>
		/// the supported Systems
		/// </summary>
		public IList<string> SupportedBrowsers { get; set; }

		/// <summary>
		/// the nodeHost
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// the state
		/// </summary>
		public TestWorkerStatus State { get; set; }

		/// <summary>
		/// The Browser
		/// </summary>
		public string WorkItemBrowserName { get; set; }

		/// <summary>
		/// The language
		/// </summary>
		public string WorkItemLanguageLanguagecode { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public string WorkItemTestsystemName { get; set; }

		/// <summary>
		/// The Testcase
		/// </summary>
		public string WorkItemTestcaseName { get; set; }

		/// <summary>
		/// The testruntime
		/// </summary>
		public string TestruntimeString { get; set; }
	}
}
