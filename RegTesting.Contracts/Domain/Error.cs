namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// A Error
	/// </summary>
	public class Error
	{
		/// <summary>
		/// The ID
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The Type
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// The Message
		/// </summary>
		public virtual string Message { get; set; }

		/// <summary>
		/// The StackTrace
		/// </summary>
		public virtual string StackTrace { get; set; }

		/// <summary>
		/// The InnerException
		/// </summary>
		public virtual string InnerException { get; set; }

	}
}
