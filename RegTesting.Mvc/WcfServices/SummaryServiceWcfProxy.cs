using System;
using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;

namespace RegTesting.Mvc.WcfServices
{
	/// <summary>
	/// the wcf proxy for the statusService
	/// </summary>
	public class SummaryServiceWcfProxy : IDisposable, ISummaryService
	{

		private readonly ChannelFactory<ISummaryService> _httpFactory;
		private readonly ISummaryService _channel;


		/// <summary>
		/// create a new StatusServiceWcfProxy
		/// </summary>
		public SummaryServiceWcfProxy()
		{
			if (_channel != null) return;
			_httpFactory =
			  new ChannelFactory<ISummaryService>("SummaryServiceEndpoint");
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

		IList<TestsystemSummary> ISummaryService.GetLastTestsystemSummaries()
		{
			return _channel.GetLastTestsystemSummaries();
		}

		

		IList<TestsystemSummary> ISummaryService.GetPinnedTestsystemSummaries()
		{
			return  _channel.GetPinnedTestsystemSummaries();
		}


		IList<TestJobDto> ISummaryService.GetTestJobs()
		{
			return _channel.GetTestJobs();
		}
	}
}