using System;
using System.ServiceModel;
using RegTesting.Contracts.Services;

namespace RegTesting.Mvc.WcfServices
{
	/// <summary>
	/// the wcf proxy for the testService
	/// </summary>
	public class TestServiceWcfProxy : IDisposable, ITestService
	{

		private readonly ChannelFactory<ITestService> _httpFactory;
		private readonly ITestService _channel;

		/// <summary>
		/// create a new TestServiceWcfProxy
		/// </summary>
		public TestServiceWcfProxy()
		{
			if (_channel != null) return;
			_httpFactory =
			  new ChannelFactory<ITestService>("TestServiceEndpoint");
			_channel = _httpFactory.CreateChannel();
		}

		void ITestService.TestTestsuite(int testerId, int testsystemId, int testsuiteId)
		{
			_channel.TestTestsuite(testerId, testsystemId, testsuiteId);
		}

		void ITestService.TestTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId)
		{
			_channel.TestTestcaseOfTestsuite(testerId, testsystemId, testsuiteId, testcaseId);
		}

		void ITestService.TestFailedTestsOfTestsuite(int testerId, int testsystemId, int testsuiteId)
		{
			_channel.TestFailedTestsOfTestsuite(testerId, testsystemId, testsuiteId);
		}

		void ITestService.TestFailedTestsOfTestcaseOfTestsuite(int testerId, int testsystemId, int testsuiteId, int testcaseId)
		{
			_channel.TestFailedTestsOfTestcaseOfTestsuite(testerId, testsystemId, testsuiteId, testcaseId);
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
	}
}