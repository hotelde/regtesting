using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{
	/// <summary>
	///  A Mail for WorkerErrors
	/// </summary>
	public class WorkerErrorMail : AbstractMail
	{
		private readonly Exception _objException;
		private readonly WorkItem _objWorkItem;
		private readonly Uri _objTestNode;

		/// <summary>
		/// Create a new WorkerErrorMail
		/// </summary>
		/// <param name="objWorkItem">The workItem causing the error </param>
		/// <param name="objException">The exception</param>
		/// <param name="objTestNode">the corresponding testNode </param>
		public WorkerErrorMail(WorkItem objWorkItem, Exception objException, Uri objTestNode)
		{
			_objWorkItem = objWorkItem;
			_objException = objException;
			_objTestNode = objTestNode;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			String strSubject = "WARNUNG! " + _objTestNode + "  wegen Fehler deaktiviert.";
			String strBody = "Der Testworker " + _objTestNode + " wurde wegen Fehlern deaktivert.\nWorkItem: " + _objWorkItem + "\nFehler: " + _objException;

			SendMail(RegtestingServerConfiguration.Errormailadress, strSubject, strBody);
		}

	}
}
