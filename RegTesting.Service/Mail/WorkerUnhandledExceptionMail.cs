using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{
	/// <summary>
	/// A Mail for unhandled exceptions of workers
	/// </summary>
	public class WorkerUnhandledExceptionMail : AbstractMail
	{
		private readonly Exception _exception;

		/// <summary>
		/// Create a new WorkerUnhandledExceptionMail
		/// </summary>
		/// <param name="exception">The exception</param>
		public WorkerUnhandledExceptionMail(Exception exception)
		{
			_exception = exception;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			SendMail(RegtestingServerConfiguration.Errormailadress, "CRITICAL - Worker died!", "Critical Error: Worker died due of an uncatched error! " + _exception);
		}

	}
}
