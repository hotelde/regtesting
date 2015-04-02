using System.Collections.Generic;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// A Testsuite
	/// </summary>
	public class CreateTestsuiteModel
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
		public virtual string Description { get; set; }

		/// <summary>
		/// A id for an testsuite to copy the settings from
		/// </summary>
		public virtual int CopyTestsuiteSettings { get; set; }





	}
}
