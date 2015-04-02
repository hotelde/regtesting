using System;
using System.Collections.Generic;

namespace RegTesting.Mvc.Models
{

	/// <summary>
	/// A Browser
	/// </summary>
	public class BrowserModel : MarshalByRefObject
	{

		/// <summary>
		/// The ID
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The Name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The Browserstring
		/// </summary>
		public virtual string Browserstring { get; set; }

		/// <summary>
		/// The Versionsstring
		/// </summary>
		public virtual string Versionsstring { get; set; }


		/// <summary>
		/// The Browsers for the Testsuite
		/// </summary>
		public virtual IList<BrowserModel> BrowsersToTestsuite { get; set; }

	}
}
