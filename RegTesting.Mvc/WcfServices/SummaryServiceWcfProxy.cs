using System;
using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts;
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

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllThorBranches()
		{
			return _channel.GetTestsystemSummaryForAllThorBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllSodaBranches()
		{
			return _channel.GetTestsystemSummaryForAllSodaBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForThorMainBranches()
		{
			return _channel.GetTestsystemSummaryForThorMainBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForSodaMainBranches()
		{
			return _channel.GetTestsystemSummaryForSodaMainBranches();
		}
	}
}