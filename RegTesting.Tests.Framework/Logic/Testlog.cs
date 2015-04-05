using System;
using System.Collections.Generic;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// The testlog. Log every action of a testrun.
	/// </summary>
	public static class TestLog
	{

		private static List<string> _lstEntries = new List<string>();
		private static DateTime _datStartTime;



		/// <summary>
		/// Add a log entry
		/// </summary>
		/// <param name="strEntry">the logentry string</param>
		public static void Add(string strEntry)
		{
			DateTime datNow = DateTime.Now;
			if (_lstEntries.Count == 0) _datStartTime = datNow;
			_lstEntries.Add((datNow - _datStartTime).ToString(@"mm\:ss") + ": " + strEntry);
		}

		/// <summary>
		/// Add a log entry
		/// </summary>
		/// <param name="strEntry">the logentry string</param>
		public static void AddWithoutTime(string strEntry)
		{
			if (_lstEntries.Count == 0)
				_datStartTime = DateTime.Now; ;
			
			_lstEntries.Add(strEntry);
		}

		/// <summary>
		/// Get all logentries
		/// </summary>
		/// <returns>A list of string with a logentry per string</returns>
		public static List<string> Get()
		{
			return _lstEntries;
		}


		/// <summary>
		/// Reset the logEntries
		/// </summary>
		/// <returns>A list of string with a logentry per string</returns>
		public static List<string> GetAndDelete()
		{
			List<string> lstLogEntries = Get();
			_lstEntries = new List<string>();
			return lstLogEntries;

		}
	}
}
