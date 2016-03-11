using System;
using System.Collections.Generic;
using System.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;

namespace RegTesting.SlimServer
{
	public static class TestPool
	{

		private static readonly object Lock = new object();
		private static readonly List<TestingJob> FinishedTestingJobs = new List<TestingJob>(); 
		private static readonly List<TestingJob> TestingJobs = new List<TestingJob>();


		public static void AddTestingJob(TestingJob job)
		{
			lock (Lock)
			{
				TestingJobs.Add(job);
            }
		}

		public static WorkItem GetWork(string nodename)
		{
			lock (Lock)
			{
				WorkItem work = null;
				
				if (TestingJobs.Count > 0)
				{
					var currentTestJob = TestingJobs[0];
					if (currentTestJob.WaitingWorkItems.Count == 0)
						return null;
					work = currentTestJob.WaitingWorkItems.First();
					currentTestJob.WaitingWorkItems.Remove(work);
					currentTestJob.CurrentWorkItems[nodename] = new WorkItemTask(work);
					Console.WriteLine("Worker " + nodename + " got a workitem.");
				};
				return work;
			}
		}

		public static void FinishedWorkItem(string nodeName, TestResult testResult)
		{
			lock (Lock)
			{
				var currentTestJob = TestingJobs[0];
				var workItemTask = currentTestJob.CurrentWorkItems[nodeName];
                var item = workItemTask.WorkItem;
				currentTestJob.ResultGenerator.AddTestResult(item.Testcase.Type, workItemTask.StartedAt, nodeName, testResult);
				currentTestJob.FinishedWorkItems.Add(item);
                currentTestJob.CurrentWorkItems.Remove(nodeName);
				Console.WriteLine(nodeName + " Finished WI: " + currentTestJob.FinishedWorkItems.Count + " Done. " + currentTestJob.CurrentWorkItems.Count + " Now. " + currentTestJob.WaitingWorkItems.Count + " Waiting.");

				if (currentTestJob.WaitingWorkItems.Count == 0 && currentTestJob.CurrentWorkItems.Count == 0)
				{
					TestingJobs.Remove(currentTestJob);
					FinishedTestingJobs.Add(currentTestJob);
					Console.WriteLine("Testjob " + currentTestJob.Guid + " finished.");

				}
			}
			
		}

		public static bool GetStatus(Guid guid)
		{
			lock (Lock)
			{
				return FinishedTestingJobs.FirstOrDefault(t => t.Guid.Equals(guid)) != null;
			}
		}

		public static string GetResultFile(Guid guid)
		{
			lock (Lock)
			{
				var testjob = FinishedTestingJobs.FirstOrDefault(t => t.Guid.Equals(guid));
				if (testjob != null)
				{
					FinishedTestingJobs.Remove(testjob);
					return testjob.ResultGenerator.GetResultFile();
				}
				return "";
			}
		}

		public static TestingJob GetCurrenTestingJob()
		{
			lock (Lock)
			{
				return TestingJobs[0];
			}
		}

		public static int GetFailedTestsCount(Guid testJobId)
		{
			lock (Lock)
			{
				var testjob = FinishedTestingJobs.FirstOrDefault(t => t.Guid.Equals(testJobId));
				if (testjob != null)
				{
					return testjob.ResultGenerator.GetFailedTestsCount();
				}
				return 0;
			}
		}
	}
}
