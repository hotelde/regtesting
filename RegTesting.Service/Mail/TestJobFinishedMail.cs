using System;
using System.Collections.Generic;
using System.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;
using RegTesting.Service.TestLogic;

namespace RegTesting.Service.Mail
{

	/// <summary>
	/// A Mail for finished workItemGroups. It sends results to the tester.
	/// </summary>
	public class TestJobFinishedMail : AbstractMail, ITestJobFinishedMail
	{
		/// <summary>
		/// Send the mail
		/// </summary>
		void ITestJobFinishedMail.Send(ITestJobManager testJobManager)
		{
			if (testJobManager == null)
				throw new ArgumentNullException("testJobManager");


			
			if (String.IsNullOrEmpty(testJobManager.TestJob.Tester.Mail)) return;

			List<Result> results = testJobManager.WorkItems.Select(t => t.Result).Where(t=>t!= null && t.Error!=null).ToList();
			List<ErrorOccurrenceGroup> errorOccurrenceGroups = ErrorGrouping.GetErrorOccurrenceGroups(results);

			string backgroundcolor;
			string bordercolor;
			string resultHeaderStatus;

			if (testJobManager.Passed == testJobManager.Count)
			{
				backgroundcolor = "#B6DAB8";
				bordercolor = "#5CB85C";
				resultHeaderStatus = "succeeded";
			}
			else
			{
				backgroundcolor = "#DF8A8A";
				bordercolor = "#D9534F";
				resultHeaderStatus = "failed";
			}
			string errorOccurrences = "";

			if(errorOccurrenceGroups.Count!=0) 
				errorOccurrences += "<h3>These tests have failed:</h3>";
			foreach (ErrorOccurrenceGroup errorOccurrenceGroup in errorOccurrenceGroups)
			{
				errorOccurrences += "<div><b>" + errorOccurrenceGroup.Testcase.Name + "</b><br>(" + errorOccurrenceGroup.Testcase.Type + ")<br><ul>";

				foreach (ErrorOccurrence errorOccurrence in errorOccurrenceGroup.LstErrorOccurence)
				{
					errorOccurrences += "<li>" + errorOccurrence.Error.Message + " (" +  errorOccurrence.LstOccurence.Count +  "x)</li>";
				}
				errorOccurrences += "</ul></div><br>";
			}

			String subject = "UI-Tests " + resultHeaderStatus + " - " +  testJobManager.TestJob.Name;
			String header;
			if (!String.IsNullOrEmpty(testJobManager.TestJob.Description))
			{
				header = testJobManager.TestJob.Description.Replace(";", "<br>");
			}
			else
			{
				header = testJobManager.TestJob.Name;
			}


			string url = !string.IsNullOrEmpty(RegtestingServerConfiguration.Webportal) ? "<a href=\"" + RegtestingServerConfiguration.Webportal + "?testjob=" + testJobManager.TestJob.ID + "\">Show results in browser</a>": "";
			String body = "<html><body>" +
							 "<div style=\"border-left: 10px solid;border-color: " + bordercolor +";background-color: " + backgroundcolor + ";font-size:1.2em;\">" + header + "</div>" 
							 + "<p>"

							 
							 + "Started by: "  +testJobManager.TestJob.Tester.Name + " (" + testJobManager.TestJob.JobType + ")<br>"
							 + "Started at: " + testJobManager.TestJob.StartedAt + "<br>"
							 + testJobManager.Passed + " passed tests.<br>"
							 + testJobManager.Failured + " failed tests.<br>" 
							 + url
							 + "</p><br>"
							 + errorOccurrences 
							 + "</body></html>";

			SendMail(testJobManager.TestJob.Tester.Mail, subject, body, true);
			if (testJobManager.TestJob.JobType == JobType.Buildtask)
			{
				SendMail(RegtestingServerConfiguration.NotifyAutomatedTestResultsMail, subject, body, true);
			}

		}
	}
}
