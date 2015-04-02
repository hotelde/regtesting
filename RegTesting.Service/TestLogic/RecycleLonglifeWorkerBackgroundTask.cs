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
		private readonly ITestPool _objTestPool;
		private bool _bolCanceled = false;
		private readonly int _intRecycleTime;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objTestPool">the testPool</param>
		public RecycleLonglifeWorkerBackgroundTask(ITestPool objTestPool)
		{
			if (objTestPool == null)
				throw new ArgumentNullException("objTestPool");
			_objTestPool = objTestPool;
			_intRecycleTime = RegtestingServerConfiguration.RecycleTime;
			Run();
		}

		private void Run()
		{
			Task objRecycleTask = Task.Factory.StartNew(() =>
			{
				while (!_bolCanceled)
				{
					Thread.Sleep(5000);
					foreach (ITestWorker objTestWorker in _objTestPool.GetTestWorker().Where(t=>t.WorkItem!=null))
					{
						if (objTestWorker.Testruntime > _intRecycleTime && objTestWorker.State == TestWorkerStatus.Ok)
						{
							objTestWorker.RebootWorker();
						}
					}
				}
			});
		}
	}
}
