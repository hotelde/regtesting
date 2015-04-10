using System;
using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{

	/// <summary>
	///  A mail for longlife tests WorkerErrors
	/// </summary>
	public class WorkerLongLifeTestErrorMail : AbstractMail
	{
		private readonly WorkItem _workItem;
		private readonly string _testNode;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="workItem">The workItem causing the error </param>
		/// <param name="testNode">the corresponding testNode </param>
		public WorkerLongLifeTestErrorMail(WorkItem workItem, string testNode)
		{
			_workItem = workItem;
			_testNode = testNode;
		}

		/// <summary>
		/// Send the mail
		/// </summary>
		public void Send()
		{
			String subject = "WARNUNG! Longlife test detected!" + _testNode + ".";
			String body = "Der Test " + _workItem + " läuft seit mehr als " + RegtestingServerConfiguration.DefaultLongLifeTime + " Minuten. Möglicher Endlostest! Auf Node: " + _testNode + " .";

			SendMail(RegtestingServerConfiguration.Errormailadress, subject, body);
		}
	}
}
