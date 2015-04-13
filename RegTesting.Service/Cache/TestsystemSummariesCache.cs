using System.Collections.Generic;
using RegTesting.Contracts;

namespace RegTesting.Service.Cache
{
	class TestsystemSummariesCache
	{
		public static TestsystemSummariesCache Cache = new TestsystemSummariesCache();

		private readonly Dictionary<int, TestsystemSummary> testSystemSummaries = new Dictionary<int, TestsystemSummary>();
		private readonly Dictionary<int, object> testsystemLocks = new Dictionary<int, object>();
		private readonly object _lock = new object();

		public TestsystemSummary Get(int testsystemIndex)
		{
			if (!testSystemSummaries.ContainsKey(testsystemIndex))
				return null;

			return testSystemSummaries[testsystemIndex];

		}

		public void Set(int testsystemIndex, TestsystemSummary testsystemSummary)
		{

			testSystemSummaries[testsystemIndex] = testsystemSummary;
			
		}

		public object GetLock(int testsystemIndex)
		{
			lock (_lock)
			{
				if (!testsystemLocks.ContainsKey(testsystemIndex))
				{
					testsystemLocks[testsystemIndex] = new object();
				}

				return testsystemLocks[testsystemIndex];
			}

		}
	}
}
