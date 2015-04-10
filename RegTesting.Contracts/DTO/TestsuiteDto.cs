using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// A Testsuite
	/// </summary>
	[DataContract(IsReference = true)]
	[Serializable]
	public class TestsuiteDto
	{
		/// <summary>
		/// The id
		/// </summary>
		[DataMember]
		public virtual int ID { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		[DataMember]
		public virtual string Name { get; set; }

		/// <summary>
		/// The description for the testsuite
		/// </summary>
		[DataMember]
		public virtual string Description { get; set; }

		/// <summary>
		/// A filter for the testsystem. If set, the testsuite is only available on the linked testsystem.
		/// </summary>
		[DataMember]
		public virtual TestsystemDto TestsystemFilter { get; set; }

		/// <summary>
		/// The Testcases for the Testsuite
		/// </summary>
		[DataMember]
		public virtual IList<TestcaseDto> Testcases { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		[DataMember]
		public virtual IList<LanguageDto> Languages { get; set; }

		/// <summary>
		/// The Languages for the Testsuite
		/// </summary>
		[DataMember]
		public virtual IList<BrowserDto> Browsers { get; set; }




	}
}
