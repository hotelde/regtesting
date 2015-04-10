using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;
using RegTesting.Service.Logging;

namespace RegTesting.Service.TestLogic
{
	/// <summary>
	/// The TestWorker
	/// </summary>
	public class NodeTestWorker :ITestWorker
	{
		/// <summary>
		/// The workItem currently tested
		/// </summary>
		public WorkItem WorkItem { get; set; }



		/// <summary>
		/// the supported Systems
		/// </summary>
		public IList<Browser> Browsers { get; private set; }

		/// <summary>
		/// the supported Systems as string list
		/// </summary>
		public IList<string> SupportedBrowsers {get { return Browsers.Select(t => t.Name).ToList(); } }


		/// <summary>
		/// The name of the node
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The state of the worker
		/// </summary>
		public TestWorkerStatus State { get; set; }

		/// <summary>
		/// The datetime of the last workitem started
		/// </summary>
		public DateTime LastStart { get; set; }

		/// <summary>
		/// The current Testruntime in seconds
		/// </summary>
		public int Testruntime
		{
			get
			{
				TimeSpan timeSpan = DateTime.Now - LastStart;
				TimeSpan timeSpanTrimmed = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				return (int)timeSpanTrimmed.TotalSeconds;
			}
		}

		/// <summary>
		/// The testruntime as string
		/// </summary>
		public string TestruntimeString {
			get
			{
				TimeSpan timeSpan = DateTime.Now - LastStart;
				TimeSpan timeSpanTrimmed = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				return timeSpanTrimmed.ToString("c");
			}
		}

		/// <summary>
		/// Create a new NodeTestWorker
		/// </summary>
		/// <param name="nodeName">the nodeName</param>
		public NodeTestWorker(string nodeName)
		{
			Name = nodeName;
			Browsers = new List<Browser>();
			State = TestWorkerStatus.Ok;
			LastStart = DateTime.Now;
		}

		void ITestWorker.CancelTest()
		{
		}

		void ITestWorker.RebootWorker()
		{

			if (!Name.ToLower().StartsWith("xd-its-brows-"))
			{
				Logger.Log("Invalid worker reboot: " + Name);
				return;
			}

			State = TestWorkerStatus.Rebooting;
			LastStart = DateTime.Now;

			Process process = new Process {StartInfo = {FileName = "shutdown", Arguments = "-m " + Name + " -r -f -t 0"}};

			Logger.Log("Rebooting worker: " + Name);
			process.Start();
		}

	}
}
