using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Tests.Core;
using Browser = RegTesting.Tests.Core.Browser;

namespace RegTesting.Node
{
	class NodeLogic
	{
		/// <summary>
		/// The types for a testfile
		/// </summary>
		public string[] Types { get; set; }

		private readonly string _serverAdr;
		private readonly string _nodename;
		private TestcaseProvider _testcaseProvider;
		private readonly List<string> _browsers;
		private int _pollingIntervall;

		public NodeLogic(string serverAdr, string nodeName, List<string> browsers)
		{
			_serverAdr = serverAdr;
			_nodename = nodeName;
			_browsers = browsers;
			_pollingIntervall = NodeConfiguration.PollingIntervall;
		}

		public void Run()
		{
			Register();
			do
			{
				EnsureBrowserClosed();
				WorkItem workItem = WaitForWorkItem();

				Console.WriteLine(@"Loading " + workItem.Testsystem.Name);
				UpdateTestcases(workItem.Testsystem);
				Console.WriteLine(@"Received" + workItem.Testsystem.Name);
				TestResult objTestResult = HandleTest(workItem);
				SendTestResultToServer(objTestResult);
				UnloadTestcases();
			} while (true);

		}

		private void EnsureBrowserClosed()
		{
			CloseProcesses("iexplore", "firefox");
		}

		private void CloseProcesses(params string[] processNames)
		{
			foreach (Process[] processes in processNames.Select(Process.GetProcessesByName))
			{
				KillAll(processes);
			}
		}

		private void KillAll(IEnumerable<Process> processes)
		{
			foreach (Process process in processes)
			{
				try
				{
					process.Kill();
					process.WaitForExit(1000 * 15);
				}
				catch (Exception)
				{
					/*Could not close Process - But at least we tried*/
				}
			}
		}
		private void UnloadTestcases()
		{
			_testcaseProvider.Unload();
		}

		private void SendTestResultToServer(TestResult testResult)
		{
			Console.Out.WriteLine("Result: " + testResult.TestState);
			using (WcfClient objWcfClient = new WcfClient(_serverAdr))
			{
				objWcfClient.FinishedWork(_nodename, testResult);
			}

			Console.Out.WriteLine("Finished.");

		}

		private void UpdateTestcases(TestsystemDto testsystem)
		{
			const string testfile = @"LocalTests.dll";
			byte[] data;
			using (WcfClient wcfClient = new WcfClient(_serverAdr))
			{
				data = wcfClient.FetchDll(_nodename, testsystem.Name);
			}

			using (FileStream fileStream = new FileStream(testfile, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(data, 0, data.Length);
			}
			_testcaseProvider = new TestcaseProvider(testfile);
			_testcaseProvider.CreateAppDomain();
		}


		private ITestable LoadTestable(WorkItem workItem)
		{
			return _testcaseProvider.GetTestableFromTypeName(workItem.Testcase.Type);
		}

		private WorkItem WaitForWorkItem()
		{
			Console.Out.WriteLine("Wait for WorkItem");
			do
			{
				WorkItem workItem = FetchWork();
				if (workItem != null) return workItem;
				Thread.Sleep(_pollingIntervall);

			} while (true);
		}

		private void Register()
		{
			Console.Out.WriteLine("Register at server...");
			using (WcfClient wcfClient = new WcfClient(_serverAdr))
			{
				wcfClient.Register(_nodename, _browsers);
			}
		}

		private WorkItem FetchWork()
		{
			using (WcfClient wcfClient = new WcfClient(_serverAdr))
			{
				return wcfClient.GetWork(_nodename);
			}
		}

		private TestResult HandleTest(WorkItem workItem)
		{
			TestResult testResult = new TestResult();
			ITestable testable = null;
			List<string> log = new List<string>();
			try
			{
				log.Add("Test on " + _nodename);

				/**1: Load Testclass **/
				Console.WriteLine(@"Testing {0} {1} ({2}/{3})", workItem.Testcase.Name, workItem.Browser.Name, workItem.Testsystem.Name, workItem.Language.Languagecode);
				testable = LoadTestable(workItem);
				if (testable == null)
					return new TestResult { TestState = TestState.NotAvailable };

				/**2: Wait for branch get ready **/
				WaitOnWebExceptions(workItem);

				/**3: Prepare Test **/
				Browser browser = new Browser()
				{
					Browserstring = workItem.Browser.Browserstring,
					Versionsstring = workItem.Browser.Versionsstring
				};
				testable.SetupTest(WebDriverInitStrategy.SeleniumLocal, browser, workItem.Testsystem.Url,
									  workItem.Language.Languagecode);

				/**4: Run Test **/
				testable.Test();

				testResult.TestState = TestState.Success;
			}
			catch (NotSupportedException notSupportedException)
			{
				Error error = CreateErrorFromException(notSupportedException);
				testResult.TestState = TestState.NotSupported;
				testResult.Error = error;
			}
			catch (TaskCanceledException taskCanceledException)
			{
				Error error = CreateErrorFromException(taskCanceledException);
				testResult.TestState = TestState.Canceled;
				testResult.Error = error;
			}
			catch (Exception exception)
			{
				ServerErrorModel serverException = null;
				try
				{
					if (testable != null)
						serverException = testable.CheckForServerError();
				}
				catch
				{
					//Error catching serverException
				}
				Error error = CreateErrorFromException(exception);
				if (serverException != null)
				{
					error.Type = serverException.Type;
					error.Message = serverException.Message;
					error.InnerException = serverException.InnerException;
					//objError.StackTrace = serverException.StackTrace; Keep error stacktrace.

				}
				testResult.TestState = TestState.Error;
				testResult.Error = error;
				if (testable != null)
					testResult.Screenshot = testable.SaveScreenshot("");


			}
			finally
			{
				if (testable != null)
				{
					testable.TeardownTest();
					log.AddRange(testable.GetLogLastTime());
				}

				testResult.Log = log;
			}
			return testResult;
		}

		private Error CreateErrorFromException(Exception exception)
		{
			Error error = new Error
			{
				Type = exception.GetType().ToString(),
				Message = exception.Message,
				StackTrace = exception.StackTrace ?? "",
				InnerException = (exception.InnerException != null ? exception.InnerException.ToString() : null),
			};
			return error;
		}

		private void WaitOnWebExceptions(WorkItem workItem)
		{
			for (int intTryCount = 0; intTryCount < 10; intTryCount++)
			{
				WebClient webClient = new WebClient();
				try
				{
					webClient.DownloadString("http://" + workItem.Testsystem.Url);
					break;
				}
				catch
				{
					//Catched an exception. Waiting for retry...
					Thread.Sleep(10000);
				}

			}

		}
	}
}
