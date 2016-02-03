using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// A testsystem
	/// </summary>
	[DataContract(IsReference = true)]
	[Serializable]
	public class TestsystemDto
	{

		/// <summary>
		/// The id
		/// </summary>
		[DataMember]
		public virtual int ID { get; set; }

		/// <summary>
		/// The url
		/// </summary>
		[DataMember]
		public virtual string Url { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		[DataMember]
		public virtual string Description { get; set; }

		/// <summary>
		/// The name of a testsystem
		/// </summary>
		[DataMember]
		public virtual string Name { get; set; }

		/// <summary>
		/// The filename of the testcases Assembly
		/// </summary>
		[DataMember]
		public virtual string Filename { get; set; }

	}
}
