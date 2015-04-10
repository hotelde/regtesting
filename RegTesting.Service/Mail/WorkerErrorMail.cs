using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{
	/// <summary>
	///  A Mail for WorkerErrors
	/// </summary>
	public class WorkerErrorMail : AbstractMail
	{
		private readonly Exception _exception;
		private readonly WorkItem _workItem;
		private readonly Uri _testNode;

		/// <summary>
		/// Create a new WorkerErrorMail
		/// </summary>
		/// <param name="workItem">The workItem causing the error </param>
		/// <param name="exception">The exception</param>
		/// <param name="testNode">the corresponding testNode </param>
		public WorkerErrorMail(WorkItem workItem, Exception exception, Uri testNode)
		{
			_workItem = workItem;
			_exception = exception;
			_testNode = testNode;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			String subject = "WARNUNG! " + _testNode + "  wegen Fehler deaktiviert.";
			String body = "Der Testworker " + _testNode + " wurde wegen Fehlern deaktivert.\nWorkItem: " + _workItem + "\nFehler: " + _exception;

			SendMail(RegtestingServerConfiguration.Errormailadress, subject, body);
		}

	}
}
