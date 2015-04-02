using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RegTesting.Contracts.Domain;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// A testsystem
	/// </summary>
	public class TestsystemModel
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
		[DataType(DataType.MultilineText)]
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
		public virtual string Filename { get; set; }


		/// <summary>
		/// Create a Testsystem
		/// </summary>
		public TestsystemModel()
		{
			Testsuites = new List<Testsuite>();
		}
	}
}
