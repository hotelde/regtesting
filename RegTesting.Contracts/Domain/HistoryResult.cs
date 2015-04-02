using System;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// A Result
	/// </summary>
	public class HistoryResult
	{
		/// <summary>
		/// The ID
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The Tester
		/// </summary>
		public virtual Tester Tester { get; set; }

		/// <summary>
		/// The Testsystem
		/// </summary>
		public virtual Testsystem Testsystem { get; set; }

		/// <summary>
		/// The Testcase
		/// </summary>
		public virtual Testcase Testcase { get; set; }

		/// <summary>
		/// The Browser
		/// </summary>
		public virtual Browser Browser { get; set; }

		/// <summary>
		/// The Language
		/// </summary>
		public virtual Language Language { get; set; }

		/// <summary>
		/// The ResultCode
		/// </summary>
		public virtual TestState ResultCode { get; set; }

		/// <summary>
		/// The Testtime
		/// </summary>
		public virtual DateTime Testtime { get; set; }

		/// <summary>
		/// The Error
		/// </summary>
		public virtual Error Error { get; set; }

		/// <summary>
		/// The ScreenshotFile
		/// </summary>
		public virtual string ScreenshotFile { get; set; }

		/// <summary>
		/// The DetailLog
		/// </summary>
		public virtual string DetailLog { get; set; }

		/// <summary>
		/// The Runtime
		/// </summary>
		public virtual int Runtime { get; set; }

		/// <summary>
		/// ErrorCount
		/// </summary>
		public virtual TestJob TestJob { get; set; }
	}
}
