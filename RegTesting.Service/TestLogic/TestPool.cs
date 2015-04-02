using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AutoMapper;
using NHibernate.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Service.Mail;
using RegTesting.Service.Tfs;

namespace RegTesting.Service.TestLogic
{
	/// <summary>
	/// The TestPool class
	/// </summary>
	public class TestPool : ITestPool
	{
		private readonly IResultRepository _objResultRepository;
		private readonly ITestFileLocker _objTestFileLocker;
		private readonly ITestJobRepository _objTestJobRepository;
		private readonly List<WorkItem> _lstWaitingWorkItems;
		private readonly List<WorkItem> _lstCurrentWorkItems;
		private readonly List<ITestJobManager> _lstCurrentTestJobManagers;
		private readonly IDictionary<string, ITestWorker> _dicTestWorkers;
		private readonly ITestJobFinishedMail _testJobFinishedMail;

		private delegate void AddTasksDelegate(ITestJobManager objTestJob, ICollection<WorkItem> objWorkItems);


		private readonly object _objLockWorkItems = new object();

		/// <summary>
		/// Create a new testPool
		/// </summary>
		/// <param name="objTestJobRepository">the testJobRepository</param>
		/// <param name="objResultRepository">the resultRepository</param>
		/// <param name="objTestFileLocker">the testFileLocker</param>
		/// <param name="testJobFinishedMail">mail notification on testsuite finished.</param>
		public TestPool(ITestJobRepository objTestJobRepository,
			IResultRepository objResultRepository, ITestFileLocker objTestFileLocker, ITestJobFinishedMail testJobFinishedMail)
		{
			if (objTestJobRepository == null)
				throw new ArgumentNullException("objTestJobRepository");
			if (objResultRepository == null)
				throw new ArgumentNullException("objResultRepository");
			if (objTestFileLocker == null)
				throw new ArgumentNullException("objTestFileLocker");
			
			_objResultRepository = objResultRepository;
			_objTestFileLocker = objTestFileLocker;
			_testJobFinishedMail = testJobFinishedMail;

			_lstWaitingWorkItems = new List<WorkItem>();
			_lstCurrentWorkItems = new List<WorkItem>();
			_lstCurrentTestJobManagers = new List<ITestJobManager>();
			_dicTestWorkers = new Dictionary<string, ITestWorker>();
			_objTestJobRepository = objTestJobRepository;
		}

		void ITestPool.AddTestJob(ITestJobManager objTestJob, ICollection<WorkItem> objWorkItems)
		{
			if (objTestJob == null)
				throw new ArgumentNullException("objTestJob");

			if (objWorkItems.Count == 0)
			{
				return;
			}

			AddTasksDelegate objAddTasksToWorkItemGroup = AddTestJobImpl;
			objAddTasksToWorkItemGroup.BeginInvoke(objTestJob, objWorkItems, null, null);
		}

		private void AddTestJobImpl(ITestJobManager objTestJobManager, ICollection<WorkItem> objWorkItems)
		{

	
			TestcaseProvider objTestcaseProvider;

			object objBranchSpecificFileLock = _objTestFileLocker.GetLock(objTestJobManager.TestJob.Testsystem.Name);
			lock (objBranchSpecificFileLock)
			{
				objTestcaseProvider =
					new TestcaseProvider(RegtestingServerConfiguration.Testsfolder + objTestJobManager.TestJob.Testsystem.Filename);
				objTestcaseProvider.CreateAppDomain();
			}

			_objTestJobRepository.Store(objTestJobManager.TestJob);

			lock (_objLockWorkItems)
			{
				List<WorkItem> lstAlreadyFoundWorkItems = new List<WorkItem>();
				List<Result> lstUpdatedResults = new List<Result>();

				foreach (WorkItem objWorkItem in objWorkItems)
				{

					ITestable objTestable = GetTestable(objWorkItem, objTestcaseProvider);
					if (objTestable==null)
					{
						lstUpdatedResults.Add(UpdateResultInfos(objWorkItem, objTestJobManager.TestJob, TestState.NotAvailable));
						continue;
					}

					if (!IsWorkItemSupported(objWorkItem, objTestable))
					{
						lstUpdatedResults.Add(UpdateResultInfos(objWorkItem, objTestJobManager.TestJob, TestState.NotSupported));
						continue;
					}

					WorkItem objExistingWorkItem = CheckForAlreadyQueyedWorkItems(objWorkItem);
					if (objExistingWorkItem != null)
					{
						objExistingWorkItem.AddTestJobManager(objTestJobManager);
						lstAlreadyFoundWorkItems.Add(objExistingWorkItem);
					}
					else
					{
						objTestJobManager.AddWorkItem(objWorkItem);
						lstUpdatedResults.Add(UpdateResultInfos(objWorkItem, objTestJobManager.TestJob, TestState.Pending));
					}
				}

				//If there is nothing to test, don't add a testsuite
				if (objTestJobManager.Count == 0 && 
					lstAlreadyFoundWorkItems.Count == 0)
					return;


				_lstCurrentTestJobManagers.Add(objTestJobManager);

				//Add new workItems to waiting list
				objTestJobManager.WorkItems.ForEach(_lstWaitingWorkItems.Add);

				//Add already found workItems back to
				lstAlreadyFoundWorkItems.ForEach(objTestJobManager.AddWorkItem);
				_objResultRepository.Store(lstUpdatedResults);
			}
			
		}

		private WorkItem CheckForAlreadyQueyedWorkItems(WorkItem objWorkItem)
		{
			//Search in waiting workItems for a matching workitem
			WorkItem objExistingWorkItem = GetMatchingWorkItem(_lstWaitingWorkItems, objWorkItem);
			if (objExistingWorkItem != null)
				return objExistingWorkItem;

			//Search in current workItems for a matching workitem
			objExistingWorkItem = GetMatchingWorkItem(_lstCurrentWorkItems, objWorkItem);
			return objExistingWorkItem;
		}

		private WorkItem GetMatchingWorkItem(IEnumerable<WorkItem> colWorkItems, WorkItem objWorkItem)
		{
			return colWorkItems.FirstOrDefault(t => t.Browser.ID == objWorkItem.Browser.ID &&
															 t.Testcase.ID == objWorkItem.Testcase.ID &&
															 t.Language.ID == objWorkItem.Language.ID &&
															 t.Testsystem.ID == objWorkItem.Testsystem.ID
															 && t.IsCanceled == false);
		}

		private Result UpdateResultInfos(WorkItem objWorkItem, TestJob objTestJob, TestState enmTestState)
		{
			Result objResult = _objResultRepository.Get(objWorkItem.Testsystem, objWorkItem.Testcase,
				objWorkItem.Browser, objWorkItem.Language);

			objResult.Tester = objWorkItem.Tester;
			objResult.Testtime = DateTime.Now;
			objResult.TestJob = objTestJob;
			objResult.ResultCode = enmTestState;
			objResult.Error = null;
			return objResult;
		}

		private ITestable GetTestable(WorkItem objWorkItem, TestcaseProvider objTestcaseProvider)
		{

			object objBranchSpecificFileLock = _objTestFileLocker.GetLock(objWorkItem.Testsystem.Name);
			lock (objBranchSpecificFileLock)
			{
				ITestable objTest = objTestcaseProvider.GetTestableFromTypeName(objWorkItem.Testcase.Type);
				return objTest;
			}

			
		}


		private bool IsWorkItemSupported(WorkItem objWorkItem, ITestable objTestcase)
		{

			string[] arrSupportedLanguages = objTestcase.GetSupportedLanguages();
			if (arrSupportedLanguages != null &&
				!arrSupportedLanguages.Contains(objWorkItem.Language.Languagecode, StringComparer.InvariantCultureIgnoreCase))
				return false;

			string[] arrSupportedBrowsers = objTestcase.GetSupportedBrowsers();
			if (arrSupportedBrowsers != null &&
				!arrSupportedBrowsers.Contains(objWorkItem.Browser.Name, StringComparer.InvariantCultureIgnoreCase))
				return false;
			
			return true;
		}

		WorkItem ITestPool.GetWorkItem(ITestWorker objTestWorker)
		{
			lock (_objLockWorkItems)
			{
				foreach (WorkItem objWorkItem in _lstWaitingWorkItems)
				{
					if (objTestWorker.Browsers.Any(t=>t.ID == objWorkItem.Browser.ID))
					{
						//If Deployment is running, don't test on the stage!
						if (TfsBuildQuery.IsDeploymentRunning(objWorkItem.Testsystem))
							continue;

						_lstWaitingWorkItems.Remove(objWorkItem);
						_lstCurrentWorkItems.Add(objWorkItem);
						return objWorkItem;
					}
				}
			}
			return null;
		}

		void ITestPool.WorkItemFinished(WorkItem objWorkItem)
		{
			if (objWorkItem == null)
				throw new ArgumentNullException("objWorkItem");


			lock (_objLockWorkItems)
			{
				_lstCurrentWorkItems.Remove(objWorkItem);
				if (!objWorkItem.IsCanceled) {
					foreach (ITestJobManager objTestJob in objWorkItem.TestJobManagers)
					{
						CheckTestJobFinished(objTestJob);
					}
				}

			}

		}

		private void CheckTestJobFinished(ITestJobManager objTestJob)
		{
			if (!objTestJob.IsFinished())
				return;

			if (IsEmailNecessary(objTestJob))
				 _testJobFinishedMail.Send(objTestJob);

			_lstCurrentTestJobManagers.Remove(objTestJob);
		}

		private bool IsEmailNecessary(ITestJobManager objTestJob)
		{
			return objTestJob.Count != 0 &&
				!objTestJob.IsCanceled &&
				!(objTestJob.Failured == 0 && objTestJob.TestJob.JobType == JobType.Buildtask);

		}

		IList<TestJobDto> ITestPool.GetTestJobs()
		{
			return Mapper.Map<List<ITestJobManager>, List<TestJobDto>>(_lstCurrentTestJobManagers.ToList());
		}

		void ITestPool.RegisterTestWorker(ITestWorker objTestWorker)
		{
			if (objTestWorker == null)
				throw new ArgumentNullException("objTestWorker");

			_dicTestWorkers.Add(objTestWorker.Name, objTestWorker);
		}

		void ITestPool.RemoveTestWorker(ITestWorker objTestWorker)
		{
			throw new NotImplementedException();
		}

		IList<ITestWorker> ITestPool.GetTestWorker()
		{
			return _dicTestWorkers.Select(t=>t.Value).ToList();
		}

		ITestWorker ITestPool.GetTestWorker(string strNode)
		{
			if (!_dicTestWorkers.ContainsKey(strNode))
			{
				return null;
			}
			return _dicTestWorkers[strNode];
		}

		void ITestPool.PrioTestJob(int testjob)
		{
			lock (_objLockWorkItems)
			{
				List<WorkItem> lstToPriorize = new List<WorkItem>();
				ITestJobManager objTestJobManager = _lstCurrentTestJobManagers.SingleOrDefault(t => t.ID == testjob);
				if (objTestJobManager == null)
					return;

				for (int intWaitingTasksIter = _lstWaitingWorkItems.Count - 1; intWaitingTasksIter > -1; intWaitingTasksIter--)
				{
					WorkItem objWorkItem = _lstWaitingWorkItems[intWaitingTasksIter];
					if (!objWorkItem.TestJobManagers.Contains(objTestJobManager))
						continue;

					_lstWaitingWorkItems.Remove(objWorkItem);
					lstToPriorize.Add(objWorkItem);
				}
				_lstWaitingWorkItems.InsertRange(0, lstToPriorize);
				_lstCurrentTestJobManagers.Remove(objTestJobManager);
				_lstCurrentTestJobManagers.Insert(0, objTestJobManager);
			}
		}

		void ITestPool.CancelTestJob(int testjob)
		{
			new Thread(() => CancelTestJobImpl(testjob)).Start();
		}

		IList<TestJobDto> ITestPool.GetTestJobs(int intTestsystem)
		{
			return Mapper.Map<List<ITestJobManager>, List<TestJobDto>>(_lstCurrentTestJobManagers.Where(t=>t.TestJob.Testsystem.ID==intTestsystem).ToList());

		}

		void ITestPool.ReAddWorkItem(WorkItem objWorkItem)
		{
			lock (_objLockWorkItems)
			{
				_lstCurrentWorkItems.Remove(objWorkItem);
				_lstWaitingWorkItems.Insert(0,objWorkItem);
			}
		}

		private void CancelTestJobImpl(int testjob)
		{
			lock (_objLockWorkItems)
			{
				ITestJobManager objTestJobManager = _lstCurrentTestJobManagers.SingleOrDefault(t => t.ID == testjob);
				if (objTestJobManager == null)
					return;

				objTestJobManager.IsCanceled = true;

				foreach (WorkItem objWorkItem in objTestJobManager.WorkItems.Where(objWorkItem =>
					!objWorkItem.IsCanceled &&
					objWorkItem.TestJobManagers.All(t => t.IsCanceled)))
				{
					CancelWorkItem(objWorkItem);
				}
				CheckTestJobFinished(objTestJobManager);
			}
		}

		private void CancelWorkItem(WorkItem objWorkItem)
		{
			objWorkItem.IsCanceled = true;
			objWorkItem.TestState = TestState.Canceled;
			if (_lstWaitingWorkItems.Contains(objWorkItem))
				_lstWaitingWorkItems.Remove(objWorkItem);
		}
	}
}