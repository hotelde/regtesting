using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegTesting.Service.Logging
{

	/// <summary>
	/// The logger
	/// </summary>
	public static class Logger
	{

		/// <summary>
		/// Log a entry
		/// </summary>
		/// <param name="strLogEntry">the entry</param>
		/// <param name="arg">the arguments</param>
		public static void Log(string strLogEntry, params Object[] arg)
		{
			if (arg == null || arg.Length == 0)
				Console.Out.WriteLine(DateTime.Now + ": " + strLogEntry);
			else
				Console.Out.WriteLine(DateTime.Now + ": " + strLogEntry, arg);
		}
	}
}
