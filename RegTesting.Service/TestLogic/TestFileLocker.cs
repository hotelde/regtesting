using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RegTesting.Contracts;

namespace RegTesting.Service.TestLogic
{
	/// <summary>
	/// The testFileLocker
	/// </summary>
	public class TestFileLocker : ITestFileLocker
	{
		private volatile ConcurrentDictionary<string, object> _dicLocks = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		object ITestFileLocker.GetLock(string strTestsystem)
		{
			return _dicLocks.GetOrAdd(strTestsystem, (strName) => new object());
		}

	}
}
