using System;
using System.Runtime.Serialization;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// A OccurrenceElement contains a dateTime, a browser and a language.
	/// </summary>
	[DataContract]
	public class OccurrenceElement
	{

		/// <summary>
		/// The screenshotfile of the occurrence
		/// </summary>
		[DataMember]
		public string ScreenshotFile;

		/// <summary>
		/// The dateTime of the occurrence
		/// </summary>
		[DataMember]
		public DateTime DateTime { get; set; }

		/// <summary>
		/// The browser of the occurrence
		/// </summary>
		[DataMember]
		public Browser Browser { get; set; }

		/// <summary>
		/// The language of the occurrence
		/// </summary>
		[DataMember]
		public Language Language { get; set; }

		/// <summary>
		/// The detailLog of the occurrence
		/// </summary>
		[DataMember]
		public string DetailLog { get; set; }

		/// <summary>
		/// Since when occurs an error?
		/// </summary>
		[DataMember]
		public DateTime? ErrorSince { get; set; }

		/// <summary>
		/// How many failures since first failure?
		/// </summary>
		[DataMember]
		public int? ErrorCount { get; set; }
	}
}
