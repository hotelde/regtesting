using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using NHibernate.Linq;
using OpenQA.Selenium;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Cache;
using RegTesting.Service.Logging;
using RegTesting.Service.TestLogic;

namespace RegTesting.Service.Services
{

	/// <summary>
	/// The nodeService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class NodeService : INodeService
	{

		private readonly ITestPool _objTestPool;
		private readonly ITestFileLocker _objTestFileLocker;
		private readonly IResultRepository _objResultRepository;
		private readonly IHistoryResultRepository _objHistoryResultRepository;
		private readonly IBrowserRepository _objBrowserRepository;

		/// <summary>
		/// Create a new nodeService
		/// </summary>
		/// <param name="objTestPool">the testPool</param>
		/// <param name="objTestFileLocker">the testFileLocker</param>
		/// <param name="objResultRepository">the resultRepository</param>
		/// <param name="objHistoryResultRepository">the historyResultRepository</param>
		/// <param name="objBrowserRepository">the browserRepository</param>
		public NodeService(ITestPool objTestPool, ITestFileLocker objTestFileLocker, IResultRepository objResultRepository, IHistoryResultRepository objHistoryResultRepository, IBrowserRepository objBrowserRepository)
		{
			if (objTestPool == null)
				throw new ArgumentNullException("objTestPool");
			if (objTestFileLocker == null)
				throw new ArgumentNullException("objTestFileLocker");
			if (objResultRepository == null)
				throw new ArgumentNullException("objResultRepository");
			if (objHistoryResultRepository == null)
				throw new ArgumentNullException("objHistoryResultRepository");
			if (objBrowserRepository == null)
				throw new ArgumentNullException("objBrowserRepository");
			_objTestPool = objTestPool;
			_objTestFileLocker = objTestFileLocker;
			_objResultRepository = objResultRepository;
			_objHistoryResultRepository = objHistoryResultRepository;
			_objBrowserRepository = objBrowserRepository;
		}

		void INodeService.Register(string strNode, List<string> lstBrowsers)
		{
			ITestWorker objTestWorker = _objTestPool.GetTestWorker(strNode);
			if (objTestWorker == null)
			{
				objTestWorker = new NodeTestWorker(strNode);
				_objTestPool.RegisterTestWorker(objTestWorker);
			}
			else
			{

				if (objTestWorker.WorkItem != null)
				{
					_objTestPool.ReAddWorkItem(objTestWorker.WorkItem);
					objTestWorker.WorkItem = null;
				}
				objTestWorker.Browsers.Clear();
				objTestWorker.State = TestWorkerStatus.Ok;
			}
			foreach (string strBrowser in lstBrowsers)
			{
				Browser objBrowser = _objBrowserRepository.GetByName(strBrowser);
				if (objBrowser == null)
				{
					Logger.Log("Node " + strNode + " demands " + strBrowser + ". But browser is not available.");
					continue;
				}
				objTestWorker.Browsers.Add(objBrowser);

			}
		}

		WorkItemDto INodeService.GetWork(string strNode)
		{
			ITestWorker objTestWorker = _objTestPool.GetTestWorker(strNode);
			WorkItem objWorkItem = _objTestPool.GetWorkItem(objTestWorker);

			if (objWorkItem != null)
			{
				objWorkItem.RunCount++;
				objTestWorker.WorkItem = objWorkItem;
				objTestWorker.LastStart = DateTime.Now;
			}
			return Mapper.Map<WorkItemDto>(objWorkItem);
		}

		void INodeService.FinishedWork(string strNode, TestResult objTestResult)
		{
			ITestWorker objTestWorker = _objTestPool.GetTestWorker(strNode);
			HandleResult(objTestWorker, objTestResult);
		}

		byte[] INodeService.FetchDLL(string strNode, string strBranch)
		{

			object objBranchSpecificFileLock = _objTestFileLocker.GetLock(strNode);
			lock (objBranchSpecificFileLock)
			{
				using (FileStream objFileStream = new FileStream(RegtestingServerConfiguration.Testsfolder + strBranch + ".dll", FileMode.Open))
				{
					byte[] arrBuffer = new byte[52428800];
					int intSize = objFileStream.Read(arrBuffer, 0, 52428800);
					byte[] arrBufferShort = arrBuffer.Take(intSize).ToArray();
					return arrBufferShort;
				}
			}
		}

		private void HandleResult(ITestWorker objTestWorker, TestResult objTestResult)
		{
			if (objTestWorker.WorkItem != null &&
			    (objTestResult.TestState == TestState.Error || objTestResult.TestState == TestState.ErrorRepeat) &&
			    objTestWorker.WorkItem.RunCount < RegtestingServerConfiguration.MaxRunCount)
			{
				_objTestPool.ReAddWorkItem(objTestWorker.WorkItem);
				objTestWorker.WorkItem = null;
				return;
			}
			
			WorkItem objWorkItem = objTestWorker.WorkItem;
			objWorkItem.TestState = objTestResult.TestState;

			string strImagefile = null;
			if (!String.IsNullOrEmpty(objTestResult.Screenshot))
			{
				string strFolder = RegtestingServerConfiguration.Screenshotsfolder;
				string strSubFolder = DateTime.Now.Year + "-" + DateTime.Now.Month + "\\";
				string strFileName = "screen" + Helper.GetScreenshotString() + ".png";
				Directory.CreateDirectory(strFolder + strSubFolder);
				strImagefile = strSubFolder + strFileName;
				Screenshot objScreenshot = new Screenshot(objTestResult.Screenshot);
				objScreenshot.SaveAsFile(strFolder + strImagefile, ImageFormat.Png);
			}

			Result objResult = _objResultRepository.Get(objWorkItem.Testsystem, objWorkItem.Testcase, objWorkItem.Browser,
				objWorkItem.Language);
			objResult.ResultCode = objTestResult.TestState;
			objResult.DetailLog = CreateLog(objTestResult.Log);
			objResult.Error = objTestResult.Error;
			objResult.Testtime = DateTime.Now;
			objResult.ScreenshotFile = strImagefile;
			if (objResult.ResultCode == TestState.Error || objResult.ResultCode == TestState.ErrorRepeat ||
			    objResult.ResultCode == TestState.KnownError)
			{
				if (objResult.ErrorCount == null)
				{
					objResult.ErrorCount = 1;
					objResult.ErrorSince = objResult.Testtime;
				}
				else
				{
					objResult.ErrorCount = objResult.ErrorCount + 1;
				}
			}
			else
			{
				objResult.ErrorSince = null;
				objResult.ErrorCount = null;
			}
			_objResultRepository.Store(objResult);
			objWorkItem.Result = objResult;


			foreach (ITestJobManager objtestJobManager in objWorkItem.TestJobManagers)
			{
				HistoryResult objHistoryResult = Mapper.Map<HistoryResult>(objWorkItem);
				objHistoryResult.DetailLog = CreateLog(objTestResult.Log);
				objHistoryResult.ResultCode = objTestResult.TestState;
				objHistoryResult.Error = objTestResult.Error;
				objHistoryResult.Testtime = DateTime.Now;
				objHistoryResult.TestJob = objtestJobManager.TestJob;
				objHistoryResult.ScreenshotFile = strImagefile;
				_objHistoryResultRepository.Store(objHistoryResult);
			}
			lock (TestsystemSummariesCache.ThorCache.GetLock(objWorkItem.Testsystem.ID))
			{
				TestsystemSummariesCache.ThorCache.Set(objWorkItem.Testsystem.ID, null);
			}
			lock (TestsystemSummariesCache.SodaCache.GetLock(objWorkItem.Testsystem.ID))
			{
				TestsystemSummariesCache.SodaCache.Set(objWorkItem.Testsystem.ID, null);
			}
			objTestWorker.WorkItem = null;

			_objTestPool.WorkItemFinished(objWorkItem);

		}


		private string CreateLog(IEnumerable<string> lstLogEntries)
		{
			string log = "";

			if (lstLogEntries == null)
				return null;

			lstLogEntries.ForEach(t => log += t + "<br>");
			return log;
		}
	}
}
