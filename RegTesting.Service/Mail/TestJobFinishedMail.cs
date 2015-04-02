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
			List<ErrorOccurrenceGroup> lstErrorOccurrenceGroups = ErrorGrouping.GetErrorOccurrenceGroups(results);

			int intPercent = (100 * testJobManager.Passed) / (testJobManager.Count);
			string color;
			string strResultHeader;
			if (testJobManager.Passed == testJobManager.Count)
			{
				color = "#5cb85c";
				strResultHeader = "Success";
			}
			else
			{
				color = "#d9534f";
				strResultHeader = "Failed";
			}

			string strErrorOccurrences = "";

			if(lstErrorOccurrenceGroups.Count!=0) 
				strErrorOccurrences += "<h3>These tests have failed:</h3>";
			foreach (ErrorOccurrenceGroup objErrorOccurrenceGroup in lstErrorOccurrenceGroups)
			{
				strErrorOccurrences += "<div><b>" + objErrorOccurrenceGroup.Testcase.Name + "</b><br>(" + objErrorOccurrenceGroup.Testcase.Type + ")<br><ul>";

				foreach (ErrorOccurrence objErrorOccurrence in objErrorOccurrenceGroup.LstErrorOccurence)
				{
					strErrorOccurrences += "<li>" + objErrorOccurrence.Error.Message + " (" +  objErrorOccurrence.LstOccurence.Count +  "x)</li>";
				}
				strErrorOccurrences += "</ul></div><br>";
			}

			String strSubject = testJobManager.TestJob.Testsystem.Name + ", " + testJobManager.TestJob.Name + ": " + intPercent + "%";
			string strUrl = !string.IsNullOrEmpty(RegtestingServerConfiguration.Webportal) ? "<a href=\"" + RegtestingServerConfiguration.Webportal + "/testing?testsuite=" + testJobManager.TestJob.Testsuite.ID + "&testsystem=" + testJobManager.TestJob.Testsystem.ID  + "\">Show results page in browser</a>": "";
			String strBody = "<html><body>" +
							 "<div style=\"background-color:" + color + " ;padding:20px;font-size:1.7em;text-align:center;color:#ffffff;\"><b>" + testJobManager.TestJob.Testsystem.Name + "</b> - " + testJobManager.TestJob.Name  +   ": <b>" + strResultHeader + "</b></div>" 
							 + "<p>"

							 + "Started by: "  +testJobManager.TestJob.Tester.Name + " (" + testJobManager.TestJob.JobType + ")<br>"
							 + "Started at: " + testJobManager.TestJob.StartedAt + "<br>"
							 + testJobManager.Passed + " passed tests.<br>"
							 + testJobManager.Failured + " failed tests.<br>" 
							 + strUrl
							 + "</p><br>"
							 + strErrorOccurrences 
							 + "</body></html>";

			SendMail(testJobManager.TestJob.Tester.Mail, strSubject, strBody, true);
			if (testJobManager.TestJob.JobType == JobType.Buildtask)
			{
				SendMail(RegtestingServerConfiguration.NotifyAutomatedTestResultsMail, strSubject, strBody, true);
			}

		}
	}
}
