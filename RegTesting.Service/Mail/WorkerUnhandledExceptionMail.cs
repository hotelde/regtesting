using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{
	/// <summary>
	/// A Mail for unhandled exceptions of workers
	/// </summary>
	public class WorkerUnhandledExceptionMail : AbstractMail
	{
		private readonly Exception _objException;

		/// <summary>
		/// Create a new WorkerUnhandledExceptionMail
		/// </summary>
		/// <param name="objException">The exception</param>
		public WorkerUnhandledExceptionMail(Exception objException)
		{
			_objException = objException;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			SendMail(RegtestingServerConfiguration.Errormailadress, "CRITICAL - Worker died!", "Critical Error: Worker died due of an uncatched error! " + _objException);
		}

	}
}
