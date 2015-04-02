using System.Collections.Generic;
using RegTesting.Contracts.Domain;

namespace RegTesting.Mvc.Models
{
	/// <summary>
	/// A Model for the Testinfopopup
	/// </summary>
	public class TestinfoModel
	{
		/// <summary>
		/// LastUpdated of the corresponding testsystem
		/// </summary>
		public long LngLastUpdated { get; set; }

		/// <summary>
		/// The ResultHistory List
		/// </summary>
		public List<Result> LstResultHistory { get; set; }
		
		/// <summary>
		/// The current Result
		/// </summary>
		public Result ObjResult { get; set; }
 
	}
}