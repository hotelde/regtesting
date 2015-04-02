using System.Collections.Generic;
using System.Runtime.Serialization;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{

	/// <summary>
	/// A ErrorOccurenceGroup is a testcase and a list of ErrorOccurences, each for a different error (occured one or more times).
	/// </summary>
	[DataContract]
	public class ErrorOccurrenceGroup
	{
		/// <summary>
		/// The testcase, which throwed an error.
		/// </summary>
		[DataMember]
		public Testcase Testcase { get; set; }

		/// <summary>
		/// The list of ErrorOccurences
		/// </summary>
		[DataMember]
		public List<ErrorOccurrence> LstErrorOccurence { get; private set; }

		/// <summary>
		/// Constructor for the ErrorOccurrenceGroup
		/// </summary>
		public ErrorOccurrenceGroup()
		{
			LstErrorOccurence = new List<ErrorOccurrence>();
		}
	}
}
