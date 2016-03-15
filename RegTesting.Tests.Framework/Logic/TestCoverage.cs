using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegTesting.Tests.Framework.Logic
{
	public static class TestCoverage
	{
		private static readonly Dictionary<string, bool> Coverage = new Dictionary<string, bool>();

		public static void AddEntry(string url, string element, string action)
		{
			string key = (url + "|" + element + "|" + action).ToLower();
			if (Coverage.ContainsKey(key))
				return;

			Coverage.Add(key,false);
		}

		public static void SetCovered(string url, string element, string action)
		{
			string key = (url + "|" + element + "|" + action).ToLower();
			if (!Coverage.ContainsKey(key))
			{
				AddEntry(url,element,action);
			}
			TestLog.Add(key);

			Coverage[key] = true;

		}

		public static Dictionary<string, bool> GetCoverage()
		{
			return Coverage;
		}
	}
}
