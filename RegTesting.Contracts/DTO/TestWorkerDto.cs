using System.Collections.Generic;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;

namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// the TestWorkerDto
	/// </summary>
	public class TestWorkerDto
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
		public string WorkItemLanguageName { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public string WorkItemTestsystemName { get; set; }

		/// <summary>
		/// The LanguageCode
		/// </summary>
		public string WorkItemLanguageLanguagecode { get; set; }

		/// <summary>
		/// The TestcaseName
		/// </summary>
		public string WorkItemTestcaseName { get; set; }

		/// <summary>
		/// The testruntime
		/// </summary>
		public string TestruntimeString { get; set; }

	}
}
