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

		private readonly ChannelFactory<ISummaryService> _objHttpFactory;
		private readonly ISummaryService _objChannel;


		/// <summary>
		/// create a new StatusServiceWcfProxy
		/// </summary>
		public SummaryServiceWcfProxy()
		{
			if (_objChannel != null) return;
			_objHttpFactory =
			  new ChannelFactory<ISummaryService>("SummaryServiceEndpoint");
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

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllThorBranches()
		{
			return _objChannel.GetTestsystemSummaryForAllThorBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForAllSodaBranches()
		{
			return _objChannel.GetTestsystemSummaryForAllSodaBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForThorMainBranches()
		{
			return _objChannel.GetTestsystemSummaryForThorMainBranches();
		}

		IList<TestsystemSummary> ISummaryService.GetTestsystemSummaryForSodaMainBranches()
		{
			return _objChannel.GetTestsystemSummaryForSodaMainBranches();
		}
	}
}