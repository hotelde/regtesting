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

namespace RegTesting.Node
{
	class NodeLogic
	{
		/// <summary>
		/// The types for a testfile
		/// </summary>
		public string[] Types { get; set; }

		private readonly string _strServerAdr;
		private readonly string _strNodename;
		private TestcaseProvider _objTestcaseProvider;
		private readonly List<string> _lstBrowsers;
		private int _pollingIntervall;

		public NodeLogic(string strServerAdr, string strNodeName, List<string> lstBrowsers)
		{
			_strServerAdr = strServerAdr;
			_strNodename = strNodeName;
			_lstBrowsers = lstBrowsers;
			_pollingIntervall = NodeConfiguration.PollingIntervall;
		}

		public void Run()
		{
			Register();
			do
			{
				EnsureBrowserClosed();
				WorkItemDto objWorkItemDto = WaitForWorkItem();
				WorkItem objWorkItem = Mapper.Map<WorkItem>(objWorkItemDto);

				Console.WriteLine(@"Loading " + objWorkItem.Testsystem.Name);
				UpdateTestcases(objWorkItem.Testsystem);
				Console.WriteLine(@"Received" + objWorkItem.Testsystem.Name);
				TestResult objTestResult = HandleTest(objWorkItem);
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
			_objTestcaseProvider.Unload();
		}

		private void SendTestResultToServer(TestResult objTestResult)
		{
			Console.Out.WriteLine("Result: " + objTestResult.TestState);
			using (WcfClient objWcfClient = new WcfClient(_strServerAdr))
			{
				objWcfClient.FinishedWork(_strNodename, objTestResult);
			}

			Console.Out.WriteLine("Finished.");

		}

		private void UpdateTestcases(Testsystem objTestsystem)
		{
			const string strTestfile = @"LocalTests.dll";
			byte[] arrData;
			using (WcfClient objWcfClient = new WcfClient(_strServerAdr))
			{
				arrData = objWcfClient.FetchDll(_strNodename, objTestsystem.Name);
			}

			using (FileStream objFileStream = new FileStream(strTestfile, FileMode.Create, FileAccess.Write))
			{
				objFileStream.Write(arrData, 0, arrData.Length);
			}
			_objTestcaseProvider = new TestcaseProvider(strTestfile);
			_objTestcaseProvider.CreateAppDomain();
		}


		private ITestable LoadTestable(WorkItem objWorkItem)
		{
			return _objTestcaseProvider.GetTestableFromTypeName(objWorkItem.Testcase.Type);
		}

		private WorkItemDto WaitForWorkItem()
		{
			Console.Out.WriteLine("Wait for WorkItem");
			do
			{
				WorkItemDto objWorkItem = FetchWork();
				if (objWorkItem != null) return objWorkItem;
				Thread.Sleep(_pollingIntervall);

			} while (true);
		}

		private void Register()
		{
			Console.Out.WriteLine("Register at server...");
			using (WcfClient objWcfClient = new WcfClient(_strServerAdr))
			{
				objWcfClient.Register(_strNodename, _lstBrowsers);
			}


		}

		private WorkItemDto FetchWork()
		{
			using (WcfClient objWcfClient = new WcfClient(_strServerAdr))
			{
				return objWcfClient.GetWork(_strNodename);
			}
		}


		private TestResult HandleTest(WorkItem objWorkItem)
		{
			TestResult objTestResult = new TestResult();
			ITestable objTestable = null;
			List<string> lstLog = new List<string>();
			try
			{
				lstLog.Add("Test on " + _strNodename);

				/**1: Load Testclass **/
				Console.WriteLine(@"Testing {0} {1} ({2}/{3})", objWorkItem.Testcase.Name, objWorkItem.Browser.Name, objWorkItem.Testsystem.Name, objWorkItem.Language.Languagecode);
				objTestable = LoadTestable(objWorkItem);
				if (objTestable == null)
					return new TestResult { TestState = TestState.NotAvailable };

				/**2: Wait for branch get ready **/
				WaitOnWebExceptions(objWorkItem);

				/**3: Prepare Test **/
				objTestable.SetupTest(WebDriverInitStrategy.SeleniumLocal, objWorkItem.Browser, objWorkItem.Testsystem.Url,
									  objWorkItem.Language.Languagecode);

				/**4: Run Test **/
				objTestable.Test();

				objTestResult.TestState = TestState.Success;
			}
			catch (NotSupportedException objException)
			{
				Error objError = CreateErrorFromException(objException);
				objTestResult.TestState = TestState.NotSupported;
				objTestResult.Error = objError;
			}
			catch (TaskCanceledException objException)
			{
				Error objError = CreateErrorFromException(objException);
				objTestResult.TestState = TestState.Canceled;
				objTestResult.Error = objError;
			}
			catch (Exception objException)
			{
				ServerErrorModel serverException = null;
				try
				{
					if (objTestable != null)
						serverException = objTestable.CheckForServerError();
				}
				catch
				{
					//Error catching serverException
				}
				Error objError = CreateErrorFromException(objException);
				if (serverException != null)
				{
					objError.Type = serverException.Type;
					objError.Message = serverException.Message;
					objError.InnerException = serverException.InnerException;
					//objError.StackTrace = serverException.StackTrace; Keep error stacktrace.

				}
				objTestResult.TestState = TestState.Error;
				objTestResult.Error = objError;
				if (objTestable != null)
					objTestResult.Screenshot = objTestable.SaveScreenshot("");


			}
			finally
			{
				if (objTestable != null)
				{
					objTestable.TeardownTest();
					lstLog.AddRange(objTestable.GetLogLastTime());
				}

				objTestResult.Log = lstLog;
			}
			return objTestResult;
		}

		private Error CreateErrorFromException(Exception objException)
		{
			Error objError = new Error
			{
				Type = objException.GetType().ToString(),
				Message = objException.Message,
				StackTrace = objException.StackTrace ?? "",
				InnerException = (objException.InnerException != null ? objException.InnerException.ToString() : null),
			};
			return objError;
		}

		private void WaitOnWebExceptions(WorkItem objWorkItem)
		{
			for (int intTryCount = 0; intTryCount < 10; intTryCount++)
			{
				WebClient objWebClient = new WebClient();
				try
				{
					objWebClient.DownloadString("http://" + objWorkItem.Testsystem.Url);
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
