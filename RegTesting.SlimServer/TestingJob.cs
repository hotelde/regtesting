using System;
using System.Collections.Generic;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;

namespace RegTesting.SlimServer
{
	public class TestingJob
	{
		public Guid Guid { get; set; }

		public List<WorkItem> WaitingWorkItems { get; set; }

		public Dictionary<string, WorkItemTask> CurrentWorkItems { get; set; }

		public List<WorkItem> FinishedWorkItems { get; set; }

		public TestState ResultCode { get; set; }

		public string TestFile { get; set; }

		public ResultGenerator ResultGenerator { get; set; }
	}
}
