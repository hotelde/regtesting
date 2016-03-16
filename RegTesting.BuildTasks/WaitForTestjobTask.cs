using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RegTesting.BuildTasks
{

	/// <summary>
	/// A Task to wait for the testjob
	/// </summary>
	public class WaitForTestjobTask : Task
	{

		/// <summary>
		/// called on execution of task. Checks the results file, if all tests passed.
		/// </summary>
		/// <returns>boolean with true in success case, else returns false</returns>
		public override bool Execute()
		{
			try
			{

                string content = File.ReadAllText(TestjobFile).Trim();
                Guid testjob = new Guid(content);

                using (WcfClient wcfClient = new WcfClient(EndpointAdress))
                {
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
        /// Path to Testjobfile
        /// </summary>
        [Required]
        public String TestjobFile
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
		
	}
}
