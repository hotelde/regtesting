using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RegTesting.BuildTasks
{

	/// <summary>
	/// A Task to update the testcases
	/// </summary>
	public class CheckTestrunErrorCountTask : Task
	{

		/// <summary>
		/// called on execution of task. Checks the results file, if all tests passed.
		/// </summary>
		/// <returns>boolean with true in success case, else returns false</returns>
		public override bool Execute()
		{
			try
			{
				
				//Log.LogMessage(MessageImportance.Normal, "Reading UITest results file " + ResultFile);
	
				int failedTestsCount = ReadResultFile();
				if (failedTestsCount == 0)
				{
					Log.LogMessage(MessageImportance.Normal, "All UITests passed.");
				}
				else
				{
					Log.LogError(failedTestsCount + " UITests failed.");
					return false;
				}
				
			}
			catch(Exception exception)
			{
				Log.LogErrorFromException(exception);
				return false;
			}

			return true;
		}
		
		private int ReadResultFile()
		{
			string content = File.ReadAllText(ResultFile).Trim();
            var match = Regex.Match(content, "ResultSummary.*ResultSummary");
			var resultSummary = match.Value;
			var matchFailed = Regex.Match(resultSummary, "failed=\"\\d*\"");
			var failed = matchFailed.Value;
			var failedCount = Regex.Match(failed, "\\d+").Value;
            return int.Parse(failedCount);
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
