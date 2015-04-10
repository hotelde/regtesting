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

		private readonly ChannelFactory<IStatusService> _httpFactory;
		private readonly IStatusService _channel;


		/// <summary>
		/// create a new StatusServiceWcfProxy
		/// </summary>
		public StatusServiceWcfProxy()
		{
			if (_channel != null) return;
			_httpFactory =
			  new ChannelFactory<IStatusService>("StatusServiceEndpoint");
			_channel = _httpFactory.CreateChannel();
		}


		/// <summary>
		/// Dispose method for WcfProxy class, disposes factory.
		/// </summary>
		public void Dispose()
		{
			if (_httpFactory != null)
			{
				_httpFactory.Close();
			}
		}

		IList<TestJobDto> IStatusService.GetTestJobs()
		{
			return _channel.GetTestJobs();
		}

		IList<TestJobDto> IStatusService.GetTestJobsForTestsystem(int testsystem)
		{
			return _channel.GetTestJobsForTestsystem(testsystem);
		}

		IList<TestWorkerDto> IStatusService.GetTestWorkers()
		{
			return _channel.GetTestWorkers();
		}

		void IStatusService.PrioTestJob(int testjob)
		{
			_channel.PrioTestJob(testjob);
		}

		void IStatusService.CancelTestJob(int testjob)
		{
			_channel.CancelTestJob(testjob);
		}

		void IStatusService.RebootWorker(string node)
		{
			_channel.RebootWorker(node);
		}

		void IStatusService.RebootAllWorker()
		{
			_channel.RebootAllWorker();
		}

		/// <summary>
		/// Get the message for a testsystem
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <returns>the message</returns>
		public string GetMessage(int testsystem)
		{
			return _channel.GetMessage(testsystem);
		}
	}
}