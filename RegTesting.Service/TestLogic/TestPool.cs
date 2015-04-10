using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using NHibernate.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Service.Mail;
using RegTesting.Service.Tfs;
using RegTesting.Tests.Core;

namespace RegTesting.Service.TestLogic
{
	/// <summary>
	/// The TestPool class
	/// </summary>
	public class TestPool : ITestPool
	{
		private readonly IResultRepository _resultRepository;
		private readonly ITestFileLocker _testFileLocker;
		private readonly ITestJobRepository _testJobRepository;
		private readonly List<WorkItem> _waitingWorkItems;
		private readonly List<WorkItem> _currentWorkItems;
		private readonly List<ITestJobManager> _currentTestJobManagers;
		private readonly IDictionary<string, ITestWorker> _testWorkers;
		private readonly ITestJobFinishedMail _testJobFinishedMail;

		private delegate void AddTasksDelegate(ITestJobManager testJob, ICollection<WorkItem> workItems);


		private readonly object _lockWorkItems = new object();

		/// <summary>
		/// Create a new testPool
		/// </summary>
		/// <param name="testJobRepository">the testJobRepository</param>
		/// <param name="resultRepository">the resultRepository</param>
		/// <param name="testFileLocker">the testFileLocker</param>
		/// <param name="testJobFinishedMail">mail notification on testsuite finished.</param>
		public TestPool(ITestJobRepository testJobRepository,
			IResultRepository resultRepository, ITestFileLocker testFileLocker, ITestJobFinishedMail testJobFinishedMail)
		{
			if (testJobRepository == null)
				throw new ArgumentNullException("testJobRepository");
			if (resultRepository == null)
				throw new ArgumentNullException("resultRepository");
			if (testFileLocker == null)
				throw new ArgumentNullException("testFileLocker");
			
			_resultRepository = resultRepository;
			_testFileLocker = testFileLocker;
			_testJobFinishedMail = testJobFinishedMail;

			_waitingWorkItems = new List<WorkItem>();
			_currentWorkItems = new List<WorkItem>();
			_currentTestJobManagers = new List<ITestJobManager>();
			_testWorkers = new Dictionary<string, ITestWorker>();
			_testJobRepository = testJobRepository;
		}

		void ITestPool.AddTestJob(ITestJobManager testJobManager, ICollection<WorkItem> workItems)
		{
			if (testJobManager == null)
				throw new ArgumentNullException("testJobManager");

			if (workItems.Count == 0)
			{
				return;
			}

			AddTasksDelegate addTasksToWorkItemGroup = AddTestJobImpl;
			addTasksToWorkItemGroup.BeginInvoke(testJobManager, workItems, null, null);
		}

		private void AddTestJobImpl(ITestJobManager testJobManager, ICollection<WorkItem> workItems)
		{

	
			TestcaseProvider testcaseProvider;

			object branchSpecificFileLock = _testFileLocker.GetLock(testJobManager.TestJob.Testsystem.Name);
			lock (branchSpecificFileLock)
			{
				testcaseProvider =
					new TestcaseProvider(RegtestingServerConfiguration.Testsfolder + testJobManager.TestJob.Testsystem.Filename);
				testcaseProvider.CreateAppDomain();
			}

			_testJobRepository.Store(testJobManager.TestJob);

			lock (_lockWorkItems)
			{
				List<WorkItem> alreadyFoundWorkItems = new List<WorkItem>();
				List<Result> updatedResults = new List<Result>();

				foreach (WorkItem workItem in workItems)
				{

					ITestable testable = GetTestable(workItem, testcaseProvider);
					if (testable==null)
					{
						updatedResults.Add(UpdateResultInfos(workItem, testJobManager.TestJob, TestState.NotAvailable));
						continue;
					}

					if (!IsWorkItemSupported(workItem, testable))
					{
						updatedResults.Add(UpdateResultInfos(workItem, testJobManager.TestJob, TestState.NotSupported));
						continue;
					}

					WorkItem existingWorkItem = CheckForAlreadyQueyedWorkItems(workItem);
					if (existingWorkItem != null)
					{
						existingWorkItem.AddTestJobManager(testJobManager);
						alreadyFoundWorkItems.Add(existingWorkItem);
					}
					else
					{
						testJobManager.AddWorkItem(workItem);
						updatedResults.Add(UpdateResultInfos(workItem, testJobManager.TestJob, TestState.Pending));
					}
				}

				//If there is nothing to test, don't add a testsuite
				if (testJobManager.Count == 0 && 
					alreadyFoundWorkItems.Count == 0)
					return;


				_currentTestJobManagers.Add(testJobManager);

				//Add new workItems to waiting list
				testJobManager.WorkItems.ForEach(_waitingWorkItems.Add);

				//Add already found workItems back to
				alreadyFoundWorkItems.ForEach(testJobManager.AddWorkItem);
				_resultRepository.Store(updatedResults);
			}
			
		}

		private WorkItem CheckForAlreadyQueyedWorkItems(WorkItem workItem)
		{
			//Search in waiting workItems for a matching workitem
			WorkItem existingWorkItem = GetMatchingWorkItem(_waitingWorkItems, workItem);
			if (existingWorkItem != null)
				return existingWorkItem;

			//Search in current workItems for a matching workitem
			existingWorkItem = GetMatchingWorkItem(_currentWorkItems, workItem);
			return existingWorkItem;
		}

		private WorkItem GetMatchingWorkItem(IEnumerable<WorkItem> workItems, WorkItem workItem)
		{
			return workItems.FirstOrDefault(t => t.Browser.ID == workItem.Browser.ID &&
															 t.Testcase.ID == workItem.Testcase.ID &&
															 t.Language.ID == workItem.Language.ID &&
															 t.Testsystem.ID == workItem.Testsystem.ID
															 && t.IsCanceled == false);
		}

		private Result UpdateResultInfos(WorkItem workItem, TestJob testJob, TestState testState)
		{
			Result result = _resultRepository.Get(workItem.Testsystem, workItem.Testcase,
				workItem.Browser, workItem.Language);

			result.Tester = workItem.Tester;
			result.Testtime = DateTime.Now;
			result.TestJob = testJob;
			result.ResultCode = testState;
			result.Error = null;
			return result;
		}

		private ITestable GetTestable(WorkItem workItem, TestcaseProvider testcaseProvider)
		{

			object branchSpecificFileLock = _testFileLocker.GetLock(workItem.Testsystem.Name);
			lock (branchSpecificFileLock)
			{
				ITestable testable = testcaseProvider.GetTestableFromTypeName(workItem.Testcase.Type);
				return testable;
			}

			
		}


		private bool IsWorkItemSupported(WorkItem workItem, ITestable testable)
		{

			string[] supportedLanguages = testable.GetSupportedLanguages();
			if (supportedLanguages != null &&
				!supportedLanguages.Contains(workItem.Language.Languagecode, StringComparer.InvariantCultureIgnoreCase))
				return false;

			string[] supportedBrowsers = testable.GetSupportedBrowsers();
			if (supportedBrowsers != null &&
				!supportedBrowsers.Contains(workItem.Browser.Name, StringComparer.InvariantCultureIgnoreCase))
				return false;
			
			return true;
		}

		WorkItem ITestPool.GetWorkItem(ITestWorker testWorker)
		{
			lock (_lockWorkItems)
			{
				foreach (WorkItem workItem in _waitingWorkItems)
				{
					if (testWorker.Browsers.Any(t=>t.ID == workItem.Browser.ID))
					{
						//If Deployment is running, don't test on the stage!
						if (TfsBuildQuery.IsDeploymentRunning(workItem.Testsystem))
							continue;

						_waitingWorkItems.Remove(workItem);
						_currentWorkItems.Add(workItem);
						return workItem;
					}
				}
			}
			return null;
		}

		void ITestPool.WorkItemFinished(WorkItem workItem)
		{
			if (workItem == null)
				throw new ArgumentNullException("workItem");


			lock (_lockWorkItems)
			{
				_currentWorkItems.Remove(workItem);
				if (!workItem.IsCanceled) {
					foreach (ITestJobManager testJobManager in workItem.TestJobManagers)
					{
						CheckTestJobFinished(testJobManager);
					}
				}

			}

		}

		private void CheckTestJobFinished(ITestJobManager testJobManager)
		{
			if (!testJobManager.IsFinished())
				return;

			if (IsEmailNecessary(testJobManager))
				 _testJobFinishedMail.Send(testJobManager);

			_currentTestJobManagers.Remove(testJobManager);
		}

		private bool IsEmailNecessary(ITestJobManager testJobManager)
		{
			return testJobManager.Count != 0 &&
				!testJobManager.IsCanceled &&
				!(testJobManager.Failured == 0 && testJobManager.TestJob.JobType == JobType.Buildtask);

		}

		IList<TestJobDto> ITestPool.GetTestJobs()
		{
			return Mapper.Map<List<ITestJobManager>, List<TestJobDto>>(_currentTestJobManagers.ToList());
		}

		void ITestPool.RegisterTestWorker(ITestWorker testWorker)
		{
			if (testWorker == null)
				throw new ArgumentNullException("testWorker");

			_testWorkers.Add(testWorker.Name, testWorker);
		}

		void ITestPool.RemoveTestWorker(ITestWorker testWorker)
		{
			throw new NotImplementedException();
		}

		IList<ITestWorker> ITestPool.GetTestWorker()
		{
			return _testWorkers.Select(t=>t.Value).ToList();
		}

		ITestWorker ITestPool.GetTestWorker(string nodeName)
		{
			if (!_testWorkers.ContainsKey(nodeName))
			{
				return null;
			}
			return _testWorkers[nodeName];
		}

		void ITestPool.PrioTestJob(int testjob)
		{
			lock (_lockWorkItems)
			{
				List<WorkItem> toPriorize = new List<WorkItem>();
				ITestJobManager testJobManager = _currentTestJobManagers.SingleOrDefault(t => t.ID == testjob);
				if (testJobManager == null)
					return;

				for (int waitingTasksIter = _waitingWorkItems.Count - 1; waitingTasksIter > -1; waitingTasksIter--)
				{
					WorkItem waitingWorkItem = _waitingWorkItems[waitingTasksIter];
					if (!waitingWorkItem.TestJobManagers.Contains(testJobManager))
						continue;

					_waitingWorkItems.Remove(waitingWorkItem);
					toPriorize.Add(waitingWorkItem);
				}
				_waitingWorkItems.InsertRange(0, toPriorize);
				_currentTestJobManagers.Remove(testJobManager);
				_currentTestJobManagers.Insert(0, testJobManager);
			}
		}

		void ITestPool.CancelTestJob(int testjob)
		{
			new Thread(() => CancelTestJobImpl(testjob)).Start();
		}

		IList<TestJobDto> ITestPool.GetTestJobs(int intTestsystem)
		{
			return Mapper.Map<List<ITestJobManager>, List<TestJobDto>>(_currentTestJobManagers.Where(t=>t.TestJob.Testsystem.ID==intTestsystem).ToList());

		}

		void ITestPool.ReAddWorkItem(WorkItem workItem)
		{
			lock (_lockWorkItems)
			{
				_currentWorkItems.Remove(workItem);
				_waitingWorkItems.Insert(0,workItem);
			}
		}

		private void CancelTestJobImpl(int testjob)
		{
			lock (_lockWorkItems)
			{
				ITestJobManager testJobManager = _currentTestJobManagers.SingleOrDefault(t => t.ID == testjob);
				if (testJobManager == null)
					return;

				testJobManager.IsCanceled = true;

				foreach (WorkItem workItem in testJobManager.WorkItems.Where(objWorkItem =>
					!objWorkItem.IsCanceled &&
					objWorkItem.TestJobManagers.All(t => t.IsCanceled)))
				{
					CancelWorkItem(workItem);
				}
				CheckTestJobFinished(testJobManager);
			}
		}

		private void CancelWorkItem(WorkItem workItem)
		{
			workItem.IsCanceled = true;
			workItem.TestState = TestState.Canceled;
			if (_waitingWorkItems.Contains(workItem))
				_waitingWorkItems.Remove(workItem);
		}
	}
}