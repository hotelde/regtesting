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

		private readonly ChannelFactory<ITestService> _objHttpFactory;
		private readonly ITestService _objChannel;

		/// <summary>
		/// create a new TestServiceWcfProxy
		/// </summary>
		public TestServiceWcfProxy()
		{
			if (_objChannel != null) return;
			_objHttpFactory =
			  new ChannelFactory<ITestService>("TestServiceEndpoint");
			_objChannel = _objHttpFactory.CreateChannel();
		}

		void ITestService.TestTestsuite(int intTester, int intTestsystem, int intTestsuite)
		{
			_objChannel.TestTestsuite(intTester, intTestsystem, intTestsuite);
		}

		void ITestService.TestTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase)
		{
			_objChannel.TestTestcaseOfTestsuite(intTester, intTestsystem, intTestsuite, intTestcase);
		}

		void ITestService.TestFailedTestsOfTestsuite(int intTester, int intTestsystem, int intTestsuite)
		{
			_objChannel.TestFailedTestsOfTestsuite(intTester, intTestsystem, intTestsuite);
		}

		void ITestService.TestFailedTestsOfTestcaseOfTestsuite(int intTester, int intTestsystem, int intTestsuite, int intTestcase)
		{
			_objChannel.TestFailedTestsOfTestcaseOfTestsuite(intTester, intTestsystem, intTestsuite, intTestcase);
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
	}
}