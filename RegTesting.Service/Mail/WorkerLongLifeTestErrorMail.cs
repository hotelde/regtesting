using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{

	/// <summary>
	///  A mail for longlife tests WorkerErrors
	/// </summary>
	public class WorkerLongLifeTestErrorMail : AbstractMail
	{
		private readonly WorkItem _objWorkItem;
		private readonly string _objTestNode;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objWorkItem">The workItem causing the error </param>
		/// <param name="objTestNode">the corresponding testNode </param>
		public WorkerLongLifeTestErrorMail(WorkItem objWorkItem, string objTestNode)
		{
			_objWorkItem = objWorkItem;
			_objTestNode = objTestNode;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			String strSubject = "WARNUNG! Longlife test detected!" + _objTestNode + ".";
			String strBody = "Der Test " + _objWorkItem + " läuft seit mehr als " + RegtestingServerConfiguration.DefaultLongLifeTime + " Minuten. Möglicher Endlostest! Auf Node: " + _objTestNode + " .";

			SendMail(RegtestingServerConfiguration.Errormailadress, strSubject, strBody);
		}
	}
}
