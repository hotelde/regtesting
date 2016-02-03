using System;

namespace RegTesting.SlimServer.Logger
{

	/// <summary>
	/// The logger
	/// </summary>
	public static class Log
	{

		/// <summary>
		/// Log a entry
		/// </summary>
		/// <param name="logEntry">the entry</param>
		/// <param name="arg">the arguments</param>
		public static void AddEntry(string logEntry, params Object[] arg)
		{
			if (arg == null || arg.Length == 0)
				Console.Out.WriteLine(DateTime.Now + ": " + logEntry);
			else
				Console.Out.WriteLine(DateTime.Now + ": " + logEntry, arg);
		}
	}
}
