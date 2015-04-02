using System;
using RegTesting.Contracts.Enums;

namespace RegTesting.Contracts.DTO
{
	/// <summary>
	/// The TestJobManager
	/// </summary>
	public class TestJobDto
	{
		/// <summary>
		/// The ID
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// the name of the testJob
		/// </summary>
		public virtual string TestJobName { get; set; }

		/// <summary>
		/// The ResultCode
		/// </summary>
		public virtual int TestJobResultCode { get; set; }

		/// <summary>
		/// The Testtime
		/// </summary>
		public virtual DateTime? TestJobStartedAt { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public virtual string TestJobTestsystemName { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual string TestJobTestsuiteName { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual string TestJobTesterName { get; set; }

		/// <summary>
		/// The jobtype
		/// </summary>
		public virtual JobType TestJobJobType { get; set; }

		/// <summary>
		/// The Count of workitems
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The count of failured workItems
		/// </summary>
		public int Failured { get; set; }

		/// <summary>
		/// The count of passed workItems
		/// </summary>
		public int Passed { get; set; }

		/// <summary>
		/// The count of finished workItems
		/// </summary>
		public int Finished { get; set; }

		/// <summary>
		/// The canceled workItems
		/// </summary>
		public int Canceled { get; set; }

		/// <summary>
		/// flag if the testJob is canceled 
		/// </summary>
		public bool IsCanceled { get; set; }

	}
}
