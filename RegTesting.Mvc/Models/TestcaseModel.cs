using System.ComponentModel.DataAnnotations;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// The Testcase
	/// </summary>
	public class TestcaseModel
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
		/// If the test is activated
		/// </summary>
		public virtual bool Activated { get;set; }

		/// <summary>
		/// The Type
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// The Description
		/// </summary>
		[DataType(DataType.MultilineText)]
		public virtual string Description { get; set; }

	}
}
