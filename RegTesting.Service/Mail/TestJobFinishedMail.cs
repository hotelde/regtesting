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

			int percent = (100 * testJobManager.Passed) / (testJobManager.Count);
			string color;
			string resultHeader;
			if (testJobManager.Passed == testJobManager.Count)
			{
				color = "#5cb85c";
				resultHeader = "Success";
			}
			else
			{
				color = "#d9534f";
				resultHeader = "Failed";
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

			String subject = testJobManager.TestJob.Testsystem.Name + ", " + testJobManager.TestJob.Name + ": " + percent + "%";
			string url = !string.IsNullOrEmpty(RegtestingServerConfiguration.Webportal) ? "<a href=\"" + RegtestingServerConfiguration.Webportal + "/testing?testsuite=" + testJobManager.TestJob.Testsuite.ID + "&testsystem=" + testJobManager.TestJob.Testsystem.ID  + "\">Show results page in browser</a>": "";
			String body = "<html><body>" +
							 "<div style=\"background-color:" + color + " ;padding:20px;font-size:1.7em;text-align:center;color:#ffffff;\"><b>" + testJobManager.TestJob.Testsystem.Name + "</b> - " + testJobManager.TestJob.Name  +   ": <b>" + resultHeader + "</b></div>" 
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
