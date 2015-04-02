using System;
using System.Collections.Generic;
using RegTesting.Contracts.Enums;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// A testJob
	/// </summary>
	public class TestJob
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
		/// The ResultCode
		/// </summary>
		public virtual TestState ResultCode { get; set; }

		/// <summary>
		/// The Testtime
		/// </summary>
		public virtual DateTime? StartedAt { get; set; }

		/// <summary>
		/// The TesttimeSt
		/// </summary>
		public virtual DateTime? FinishedAt { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public virtual Testsystem Testsystem { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual Testsuite Testsuite { get; set; }

		/// <summary>
		/// The Testsuite
		/// </summary>
		public virtual Tester Tester { get; set; }

		/// <summary>
		/// A flag if the testjob was started manually
		/// </summary>
		public virtual JobType JobType { get; set; }

	}
}
