using System;
using RegTesting.Contracts.DTO;

namespace RegTesting.SlimServer
{
	public class WorkItemTask
	{
		public WorkItem WorkItem { get; set; }

		public DateTime StartedAt { get; set; }

		public WorkItemTask(WorkItem workItem)
		{
			WorkItem = workItem;
			StartedAt = DateTime.Now;
		}
	}
}
