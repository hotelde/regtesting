using System;
using System.Collections.Generic;
using System.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Service.Logging;
using RegTesting.Service.TestLogic;

namespace RegTesting.Service.Mail
{
	/// <summary>
	/// Class to send no mail on Test-Job succeess.
	/// </summary>
	public class FakeTestJobFinishedMail: ITestJobFinishedMail
	{
		/// <summary>
		/// Fake implementation of send email. Logs on Console that no email is sent.
		/// </summary>
		/// <param name="testJobManager">A instance of a TestJobManager.</param>
		public void Send(ITestJobManager testJobManager)
		{
			Logger.Log("Test job finished. No success-mail send. Mailing-Service is running in test configuration.");
			
			List<Result> results = testJobManager.WorkItems.Select(t => t.Result).Where(t => t != null && t.Error != null).ToList();
			List<ErrorOccurrenceGroup> lstErrorOccurrenceGroups = ErrorGrouping.GetErrorOccurrenceGroups(results);
			Logger.Log(testJobManager.Passed + " passed tests. " + testJobManager.Failured + " failed tests. ErrorOccurences from GetErrorOccurrenceGroups(): " + lstErrorOccurrenceGroups.Count);
		}
	}
}
