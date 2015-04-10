using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RegTesting.Contracts;
using RegTesting.Contracts.Enums;

namespace RegTesting.Service.TestLogic
{
	class RecycleLonglifeWorkerBackgroundTask
	{
		private readonly ITestPool _testPool;
		private bool _canceled = false;
		private readonly int _recycleTime;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="testPool">the testPool</param>
		public RecycleLonglifeWorkerBackgroundTask(ITestPool testPool)
		{
			if (testPool == null)
				throw new ArgumentNullException("testPool");
			_testPool = testPool;
			_recycleTime = RegtestingServerConfiguration.RecycleTime;
			Run();
		}

		private void Run()
		{
			Task recycleTask = Task.Factory.StartNew(() =>
			{
				while (!_canceled)
				{
					Thread.Sleep(5000);
					foreach (ITestWorker testWorker in _testPool.GetTestWorker().Where(t=>t.WorkItem!=null))
					{
						if (testWorker.Testruntime > _recycleTime && testWorker.State == TestWorkerStatus.Ok)
						{
							testWorker.RebootWorker();
						}
					}
				}
			});
		}
	}
}
