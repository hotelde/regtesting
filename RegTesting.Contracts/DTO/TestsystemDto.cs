using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RegTesting.Contracts.Domain;

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
		/// LastUpdated - When was the last time a file of a testsuite was updated?
		/// </summary>
		[DataMember]
		public virtual DateTime LastUpdated { get; set; }

		/// <summary>
		/// The name of a testsystem
		/// </summary>
		[DataMember]
		public virtual string Name { get; set; }

		/// <summary>
		/// The Testsuites specific for this Testsystem
		/// </summary>
		[DataMember]
		public virtual IList<TestsuiteDto> Testsuites { get; set; }

		/// <summary>
		/// The filename of the testcases Assembly
		/// </summary>
		[DataMember]
		public virtual string Filename { get; set; }


		/// <summary>
		/// Create a Testsystem
		/// </summary>
		public TestsystemDto()
		{
			Testsuites = new List<TestsuiteDto>();
		}
	}
}
