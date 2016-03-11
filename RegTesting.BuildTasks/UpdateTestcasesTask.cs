using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RegTesting.BuildTasks
{

	/// <summary>
	/// A Task to update the testcases
	/// </summary>
	public class UpdateTestcasesTask : Task
	{

		/// <summary>
		/// called on execution of task. Sends the new testcases to the Testservice.
		/// </summary>
		/// <returns>boolean with true in success case, else returns false</returns>
		public override bool Execute()
		{
			try
			{
				
				Log.LogMessage(MessageImportance.Normal, "Starting UITests at " + EndpointAdress);
				using (WcfClient wcfClient = new WcfClient(EndpointAdress))
				{
					Guid testjob = wcfClient.SendFile(TestFile, TestUrl.ToLower() );
					Log.LogMessage(MessageImportance.Normal, "Waiting for Testresults. " + "Testjobid: " + testjob);
					wcfClient.WaitForTestJobResult(testjob);
					string resultFile = wcfClient.GetResultFile(testjob);
					WriteResultFile(resultFile);
				}
			}
			catch(Exception exception)
			{
				Log.LogErrorFromException(exception);
				return false;
			}

			return true;
		}
		
		private void WriteResultFile(string content)
		{
			File.WriteAllText(ResultFile, content);
		}

		/// <summary>
		/// EndpointAdress
		/// </summary>
		[Required]
		public String EndpointAdress
		{
			get;
			set;
		}

		/// <summary>
		/// Path to Resultfile
		/// </summary>
		[Required]
		public String ResultFile
		{
			get;
			set;
		}

		/// <summary>
		/// Path to Dllfile with Testcases
		/// </summary>
		[Required]
		public String TestFile
		{
			get;
			set;
		}

		/// <summary>
		/// Stage to update (gamma, beta, etc.)
		/// </summary>
		[Required]
		public String TestUrl
		{
			get;
			set;
		}
	}
}
