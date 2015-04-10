using System;
using RegTesting.Contracts;


namespace RegTesting.Service.Mail
{
	/// <summary>
	/// A Mail for unhandled exceptions
	/// </summary>
	public class UnhandledExceptionMail : AbstractMail
	{
		private readonly Exception _exception;

		/// <summary>
		/// Create a new UnhandledExceptionMail
		/// </summary>
		/// <param name="exception">The exception</param>
		public UnhandledExceptionMail(Exception exception)
		{
			_exception = exception;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			SendMail(RegtestingServerConfiguration.Errormailadress, "CRITICAL - UnhandledException!", "UnhandledException:\n" + _exception);
		}

	}
}
