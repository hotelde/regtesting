using System.Collections.Generic;
using RegTesting.Contracts;

namespace RegTesting.Service.Cache
{
	class TestsystemSummariesCache
	{
		public static TestsystemSummariesCache ThorCache = new TestsystemSummariesCache();
		public static TestsystemSummariesCache SodaCache = new TestsystemSummariesCache();

		private readonly Dictionary<int, TestsystemSummary> _dicTestsystemsummaries = new Dictionary<int, TestsystemSummary>();

		private readonly Dictionary<int, object> _dicTestsystemLocks = new Dictionary<int, object>();
		private readonly object _objLock = new object();

		public TestsystemSummary Get(int intTestsystem)
		{
			if (!_dicTestsystemsummaries.ContainsKey(intTestsystem))
				return null;

			return _dicTestsystemsummaries[intTestsystem];

		}

		public void Set(int intTestsystem, TestsystemSummary objValue)
		{

			_dicTestsystemsummaries[intTestsystem] = objValue;
			
		}

		public object GetLock(int intTestsystem)
		{
			lock (_objLock)
			{
				if (!_dicTestsystemLocks.ContainsKey(intTestsystem))
				{
					_dicTestsystemLocks[intTestsystem] = new object();
				}

				return _dicTestsystemLocks[intTestsystem];
			}

		}
	}
}
