using System;
using System.Collections.Generic;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;

namespace RegTesting.Contracts
{

	/// <summary>
	/// the interface for the testWorker
	/// </summary>
	public interface ITestWorker
	{

		/// <summary>
		/// the supported Systems
		/// </summary>
		IList<Browser> Browsers { get; }


		/// <summary>
		/// the supported Systems
		/// </summary>
		IList<string> SupportedBrowsers { get; }

		/// <summary>
		/// the Name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// get the state
		/// </summary>
		/// <returns>the state</returns>
		TestWorkerStatus State { get; set; }

		/// <summary>
		/// get the workItem
		/// </summary>
		/// <returns>the current WorkItem</returns>
		WorkItem WorkItem { get; set; }

		/// <summary>
		/// the testRuntime in Seconds
		/// </summary>
		int Testruntime { get; }

		/// <summary>
		/// The testruntime
		/// </summary>
		string TestruntimeString { get; }

		/// <summary>
		/// The datetime of the last workitem started
		/// </summary>
		DateTime LastStart { get; set; }

		/// <summary>
		/// cancel the test
		/// </summary>
		void CancelTest();

		/// <summary>
		/// reboot the worker
		/// </summary>
		void RebootWorker();


	}
}
