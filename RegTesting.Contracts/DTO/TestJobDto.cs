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
		public virtual string Name { get; set; }

		/// <summary>
		/// the description of the testJob
		/// </summary>
		public virtual string Description {get; set; }

		/// <summary>
		/// The ResultCode
		/// </summary>
		public virtual int ResultCode { get; set; }

		/// <summary>
		/// The Testtime
		/// </summary>
		public virtual DateTime? StartedAt { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public virtual string TestsystemName { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual string TestsuiteName { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual string TesterName { get; set; }

		/// <summary>
		/// The jobtype
		/// </summary>
		public virtual JobType JobType { get; set; }

	}
}
