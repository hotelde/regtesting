namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// The Testcase
	/// </summary>
	public class TestcaseDto
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
		public virtual string Description { get; set; }

	}
}
