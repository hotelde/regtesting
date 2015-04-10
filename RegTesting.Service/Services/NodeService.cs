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

		private readonly ITestPool _testPool;
		private readonly ITestFileLocker _testFileLocker;
		private readonly IResultRepository _resultRepository;
		private readonly IHistoryResultRepository _historyResultRepository;
		private readonly IBrowserRepository _browserRepository;

		/// <summary>
		/// Create a new nodeService
		/// </summary>
		/// <param name="testPool">the testPool</param>
		/// <param name="testFileLocker">the testFileLocker</param>
		/// <param name="resultRepository">the resultRepository</param>
		/// <param name="historyResultRepository">the historyResultRepository</param>
		/// <param name="browserRepository">the browserRepository</param>
		public NodeService(ITestPool testPool, ITestFileLocker testFileLocker, IResultRepository resultRepository, IHistoryResultRepository historyResultRepository, IBrowserRepository browserRepository)
		{
			if (testPool == null)
				throw new ArgumentNullException("testPool");
			if (testFileLocker == null)
				throw new ArgumentNullException("testFileLocker");
			if (resultRepository == null)
				throw new ArgumentNullException("resultRepository");
			if (historyResultRepository == null)
				throw new ArgumentNullException("historyResultRepository");
			if (browserRepository == null)
				throw new ArgumentNullException("browserRepository");
			_testPool = testPool;
			_testFileLocker = testFileLocker;
			_resultRepository = resultRepository;
			_historyResultRepository = historyResultRepository;
			_browserRepository = browserRepository;
		}

		void INodeService.Register(string node, List<string> browsers)
		{
			ITestWorker testWorker = _testPool.GetTestWorker(node);
			if (testWorker == null)
			{
				testWorker = new NodeTestWorker(node);
				_testPool.RegisterTestWorker(testWorker);
			}
			else
			{

				if (testWorker.WorkItem != null)
				{
					_testPool.ReAddWorkItem(testWorker.WorkItem);
					testWorker.WorkItem = null;
				}
				testWorker.Browsers.Clear();
				testWorker.State = TestWorkerStatus.Ok;
			}
			foreach (string browserName in browsers)
			{
				Browser browser = _browserRepository.GetByName(browserName);
				if (browser == null)
				{
					Logger.Log("Node " + node + " demands " + browserName + ". But browser is not available.");
					continue;
				}
				testWorker.Browsers.Add(browser);

			}
		}

		WorkItemDto INodeService.GetWork(string nodeName)
		{
			ITestWorker testWorker = _testPool.GetTestWorker(nodeName);
			WorkItem workItem = _testPool.GetWorkItem(testWorker);

			if (workItem != null)
			{
				workItem.RunCount++;
				testWorker.WorkItem = workItem;
				testWorker.LastStart = DateTime.Now;
			}
			return Mapper.Map<WorkItemDto>(workItem);
		}

		void INodeService.FinishedWork(string nodeName, TestResult testResult)
		{
			ITestWorker testWorker = _testPool.GetTestWorker(nodeName);
			HandleResult(testWorker, testResult);
		}

		byte[] INodeService.FetchDLL(string nodeName, string branchName)
		{

			object branchSpecificFileLock = _testFileLocker.GetLock(nodeName);
			lock (branchSpecificFileLock)
			{
				using (FileStream fileStream = new FileStream(RegtestingServerConfiguration.Testsfolder + branchName + ".dll", FileMode.Open))
				{
					byte[] buffer = new byte[52428800];
					int size = fileStream.Read(buffer, 0, 52428800);
					byte[] bufferShort = buffer.Take(size).ToArray();
					return bufferShort;
				}
			}
		}

		private void HandleResult(ITestWorker testWorker, TestResult testResult)
		{
			if (testWorker.WorkItem != null &&
			    (testResult.TestState == TestState.Error || testResult.TestState == TestState.ErrorRepeat) &&
			    testWorker.WorkItem.RunCount < RegtestingServerConfiguration.MaxRunCount)
			{
				_testPool.ReAddWorkItem(testWorker.WorkItem);
				testWorker.WorkItem = null;
				return;
			}
			
			WorkItem workItem = testWorker.WorkItem;
			workItem.TestState = testResult.TestState;

			string imagefile = null;
			if (!String.IsNullOrEmpty(testResult.Screenshot))
			{
				string folder = RegtestingServerConfiguration.Screenshotsfolder;
				string subFolder = DateTime.Now.Year + "-" + DateTime.Now.Month + "\\";
				string fileName = "screen" + Helper.GetScreenshotString() + ".png";
				Directory.CreateDirectory(folder + subFolder);
				imagefile = subFolder + fileName;
				Screenshot screenshot = new Screenshot(testResult.Screenshot);
				screenshot.SaveAsFile(folder + imagefile, ImageFormat.Png);
			}

			Result result = _resultRepository.Get(workItem.Testsystem, workItem.Testcase, workItem.Browser,
				workItem.Language);
			result.ResultCode = testResult.TestState;
			result.DetailLog = CreateLog(testResult.Log);
			result.Error = testResult.Error;
			result.Testtime = DateTime.Now;
			result.ScreenshotFile = imagefile;
			if (result.ResultCode == TestState.Error || result.ResultCode == TestState.ErrorRepeat ||
			    result.ResultCode == TestState.KnownError)
			{
				if (result.ErrorCount == null)
				{
					result.ErrorCount = 1;
					result.ErrorSince = result.Testtime;
				}
				else
				{
					result.ErrorCount = result.ErrorCount + 1;
				}
			}
			else
			{
				result.ErrorSince = null;
				result.ErrorCount = null;
			}
			_resultRepository.Store(result);
			workItem.Result = result;


			foreach (ITestJobManager testJobManager in workItem.TestJobManagers)
			{
				HistoryResult historyResult = Mapper.Map<HistoryResult>(workItem);
				historyResult.DetailLog = CreateLog(testResult.Log);
				historyResult.ResultCode = testResult.TestState;
				historyResult.Error = testResult.Error;
				historyResult.Testtime = DateTime.Now;
				historyResult.TestJob = testJobManager.TestJob;
				historyResult.ScreenshotFile = imagefile;
				_historyResultRepository.Store(historyResult);
			}
			lock (TestsystemSummariesCache.ThorCache.GetLock(workItem.Testsystem.ID))
			{
				TestsystemSummariesCache.ThorCache.Set(workItem.Testsystem.ID, null);
			}
			lock (TestsystemSummariesCache.SodaCache.GetLock(workItem.Testsystem.ID))
			{
				TestsystemSummariesCache.SodaCache.Set(workItem.Testsystem.ID, null);
			}
			testWorker.WorkItem = null;

			_testPool.WorkItemFinished(workItem);

		}
		
		private string CreateLog(IEnumerable<string> logEntries)
		{
			string log = "";

			if (logEntries == null)
				return null;

			logEntries.ForEach(t => log += t + "<br>");
			return log;
		}
	}
}
