using RegTesting.Contracts;
using RegTesting.Contracts.Enums;

namespace RegTesting.Mvc.Helper
{
	/// <summary>
	/// Mapping of different status to status strings
	/// </summary>
	public static class StatusHelper
	{


		/// <summary>
		/// Get the status string for a teststate (int)
		/// </summary>
		/// <param name="enmTestState">the status</param>
		/// <returns>a string, describing the status</returns>
		public static string GetStatusString(TestState enmTestState)
		{

			switch (enmTestState)
			{
				case TestState.Pending:
				case TestState.Running:
				case TestState.Canceled:
					return "default";
				case TestState.KnownError:
					return "warning";
				case TestState.Success:
					return "success";
				case TestState.ErrorRepeat:
				case TestState.Error:
					return "danger";
				default:
					return "default";
			}
		}

		/// <summary>
		/// Get the testworker status string
		/// </summary>
		/// <param name="enmState">the workerState</param>
		/// <returns>a string, describing the testworker status</returns>
		public static string GetTestWorkerStatusString(TestWorkerStatus enmState)
		{
			switch (enmState)
			{
				case TestWorkerStatus.Ok:
					return "success";
				case TestWorkerStatus.Pause:
					return "default";
				case TestWorkerStatus.Rebooting:
					return "warning";
				case TestWorkerStatus.LongTest:
					return "primary";
				default:
					return "default";
			}
		}

	}
}