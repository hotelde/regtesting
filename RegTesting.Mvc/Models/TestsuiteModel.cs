using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RegTesting.Contracts.Domain;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// A Testsuite
	/// </summary>
	public class TestsuiteModel
	{
		/// <summary>
		/// The id
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The description for the testsuite
		/// </summary>
		[DataType(DataType.MultilineText)]
		public virtual string Description { get; set; }

		/// <summary>
		/// A filter for the testsystem. If set, the testsuite is only available on the linked testsystem.
		/// </summary>
		public virtual TestsystemModel TestsystemFilter { get; set; }

		/// <summary>
		/// The Testcases for the Testsuite
		/// </summary>
		public virtual IList<TestcaseModel> Testcases { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		public virtual IList<LanguageModel> Languages { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		public virtual IList<BrowserModel> Browsers { get; set; }




	}
}
