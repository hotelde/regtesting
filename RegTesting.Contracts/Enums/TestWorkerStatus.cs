using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegTesting.Contracts.Enums
{
	/// <summary>
	/// The testWorkerStatus
	/// </summary>
	public enum TestWorkerStatus
	{
		Ok = 0, //The Worker works as expected.
		Rebooting = 1, //The Worker is deactivated due recent errors in tests.
		
		Pause = 3, //The worker was paused
		LongTest = 8 //The worker is testing for a long time - no answer yet
	}
}
