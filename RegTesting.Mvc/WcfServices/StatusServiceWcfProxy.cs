using System;
using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;

namespace RegTesting.Mvc.WcfServices
{
	/// <summary>
	/// the wcf proxy for the statusService
	/// </summary>
	public class StatusServiceWcfProxy : IDisposable, IStatusService
	{

		private readonly ChannelFactory<IStatusService> _objHttpFactory;
		private readonly IStatusService _objChannel;


		/// <summary>
		/// create a new StatusServiceWcfProxy
		/// </summary>
		public StatusServiceWcfProxy()
		{
			if (_objChannel != null) return;
			_objHttpFactory =
			  new ChannelFactory<IStatusService>("StatusServiceEndpoint");
			_objChannel = _objHttpFactory.CreateChannel();
		}


		/// <summary>
		/// Dispose method for WcfProxy class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_objHttpFactory != null)
			{
				_objHttpFactory.Close();
			}
		}

		IList<TestJobDto> IStatusService.GetTestJobs()
		{
			return _objChannel.GetTestJobs();
		}

		IList<TestJobDto> IStatusService.GetTestJobsForTestsystem(int testsystem)
		{
			return _objChannel.GetTestJobsForTestsystem(testsystem);
		}

		IList<TestWorkerDto> IStatusService.GetTestWorkers()
		{
			return _objChannel.GetTestWorkers();
		}

		void IStatusService.PrioTestJob(int testjob)
		{
			_objChannel.PrioTestJob(testjob);
		}

		void IStatusService.CancelTestJob(int testjob)
		{
			_objChannel.CancelTestJob(testjob);
		}

		void IStatusService.RebootWorker(string node)
		{
			_objChannel.RebootWorker(node);
		}

		void IStatusService.RebootAllWorker()
		{
			_objChannel.RebootAllWorker();
		}

		/// <summary>
		/// Get the message for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>the message</returns>
		public string GetMessage(int testsystem)
		{
			return _objChannel.GetMessage(testsystem);
		}
	}
}