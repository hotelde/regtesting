using System;
using RegTesting.Contracts;


namespace RegTesting.Service.Mail
{
	/// <summary>
	/// A Mail for unhandled exceptions
	/// </summary>
	public class UnhandledExceptionMail : AbstractMail
	{
		private readonly Exception _objException;

		/// <summary>
		/// Create a new UnhandledExceptionMail
		/// </summary>
		/// <param name="objException">The exception</param>
		public UnhandledExceptionMail(Exception objException)
		{
			_objException = objException;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			SendMail(RegtestingServerConfiguration.Errormailadress, "CRITICAL - UnhandledException!", "UnhandledException:\n" + _objException);
		}

	}
}
