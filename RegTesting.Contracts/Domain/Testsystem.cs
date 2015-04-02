using System;
using System.Collections.Generic;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// A testsystem
	/// </summary>
	public class Testsystem
	{

		/// <summary>
		/// The id
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The url
		/// </summary>
		public virtual string Url { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		public virtual string Description { get; set; }

		/// <summary>
		/// LastUpdated - When was the last time a file of a testsuite was updated?
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }

		/// <summary>
		/// The name of a testsystem
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The Testsuites specific for this Testsystem
		/// </summary>
		public virtual IList<Testsuite> Testsuites { get; set; }

		/// <summary>
		/// The filename of the testcases Assembly
		/// </summary>
		public virtual string Filename { get { return Name.Replace('/', '\\').Replace(':', '-') + ".dll"; } }


		/// <summary>
		/// Create a Testsystem
		/// </summary>
		public Testsystem()
		{
			Testsuites = new List<Testsuite>();
		}
	}
}
