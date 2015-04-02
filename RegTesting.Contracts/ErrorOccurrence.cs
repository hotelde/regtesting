using System.Collections.Generic;
using System.Runtime.Serialization;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{

	/// <summary>
	/// A ErrorOccurence is an error and a list of OccurenceElements.
	/// </summary>
	[DataContract]
	public class ErrorOccurrence
	{
		/// <summary>
		/// The error, which occures
		/// </summary>
		[DataMember]
		public Error Error { get; set; }
		
		/// <summary>
		/// The OccurenceElements, each describing one occurence.
		/// </summary>
		[DataMember]
		public List<OccurrenceElement> LstOccurence { get; private set; }

		/// <summary>
		/// The Constructor, creates a new ErrorOccurrence
		/// </summary>
		public ErrorOccurrence()
		{
			LstOccurence = new List<OccurrenceElement>();
		}
	}
}
