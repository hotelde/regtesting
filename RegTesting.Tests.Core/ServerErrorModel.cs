using System.Runtime.Serialization;

namespace RegTesting.Tests.Core
{

	/// <summary>
	/// A servererror
	/// </summary>
	[DataContract]
	public class ServerErrorModel
	{

		/// <summary>
		/// The Type
		/// </summary>
		[DataMember]
		public virtual string Type { get; set; }

		/// <summary>
		/// The Message
		/// </summary>
		[DataMember]
		public virtual string Message { get; set; }

		/// <summary>
		/// The StackTrace
		/// </summary>
		[DataMember]
		public virtual string StackTrace { get; set; }

		/// <summary>
		/// The InnerException
		/// </summary>
		[DataMember]
		public virtual string InnerException { get; set; }
	}
}
