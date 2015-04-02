using System.Collections.Generic;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// A Testsuite
	/// </summary>
	public class Testsuite
	{
		/// <summary>
		/// The id
		/// </summary>
		public virtual int ID { get; protected set; }

		/// <summary>
		/// The name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The description for the testsuite
		/// </summary>
		public virtual string Description { get; set; }

		/// <summary>
		/// A filter for the testsystem. If set, the testsuite is only available on the linked testsystem.
		/// </summary>
		public virtual Testsystem TestsystemFilter { get; set; }

		/// <summary>
		/// The Testcases for the Testsuite
		/// </summary>
		public virtual IList<Testcase> Testcases { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		public virtual IList<Language> Languages { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		public virtual IList<Browser> Browsers { get; set; }




	}
}
