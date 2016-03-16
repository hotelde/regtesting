using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RegTesting.BuildTasks
{

	/// <summary>
	/// A Task to update the testcases
	/// </summary>
	public class StartTestjobTask : Task
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
					Guid testjob = wcfClient.SendFile(TestFile, TestUrl.ToLower());
                    Log.LogMessage(MessageImportance.Normal, "Started testjob with id " + testjob);
                    WriteTestjobFile(testjob.ToString());

				}
			}
			catch(Exception exception)
			{
				Log.LogErrorFromException(exception);
				return false;
			}

			return true;
		}

        private void WriteTestjobFile(string testjob)
        {
            File.WriteAllText(TestjobFile, testjob);
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
		public String TestjobFile
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
