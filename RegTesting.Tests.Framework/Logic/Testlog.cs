using System;
using System.Collections.Generic;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// The testlog. Log every action of a testrun.
	/// </summary>
	public static class TestLog
	{

		private static List<string> entries = new List<string>();
		private static DateTime _startTime;



		/// <summary>
		/// Add a log entry
		/// </summary>
		/// <param name="entry">the logentry string</param>
		public static void Add(string entry)
		{
			DateTime datNow = DateTime.Now;
			if (entries.Count == 0) _startTime = datNow;
			entries.Add((datNow - _startTime).ToString(@"mm\:ss") + ": " + entry);
		}

		/// <summary>
		/// Add a log entry
		/// </summary>
		/// <param name="entry">the logentry string</param>
		public static void AddWithoutTime(string entry)
		{
			if (entries.Count == 0)
				_startTime = DateTime.Now; ;
			
			entries.Add(entry);
		}

		/// <summary>
		/// Get all logentries
		/// </summary>
		/// <returns>A list of string with a logentry per string</returns>
		public static List<string> Get()
		{
			return entries;
		}


		/// <summary>
		/// Reset the logEntries
		/// </summary>
		/// <returns>A list of string with a logentry per string</returns>
		public static List<string> GetAndDelete()
		{
			List<string> logEntries = Get();
			entries = new List<string>();
			return logEntries;

		}
	}
}
