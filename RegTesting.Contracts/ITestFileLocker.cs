using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RegTesting.Contracts
{
	/// <summary>
	/// Interface for a testFileLocker
	/// </summary>
	public interface ITestFileLocker
	{
		/// <summary>
		/// Get the lock specific for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>the lock</returns>
		object GetLock(string testsystem);

	}


}
