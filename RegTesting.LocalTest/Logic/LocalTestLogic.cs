using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RegTesting.LocalTest.Properties;
using RegTesting.Tests.Core;

namespace RegTesting.LocalTest.Logic
{
	class LocalTestLogic
	{
		private ITestable _testable;

		private TestcaseProvider _testcaseProvider = null;

		public string TestHeader { get; private set; }
		public List<string> LogEntries { 
			get {
				if(_testable != null)
					return _testable.GetLog();
				return new List<string>();
			}
		}


		public string FinishedScreenshot;

		public string CurrentScreenshot
		{
			get
			{
				if (!_finished)
					return _testable?.SaveScreenshot(string.Empty);

				return FinishedScreenshot;
			}
		}


		private bool _finished;

		private bool _canceled;



		internal void LoadTestFile(string filename)
		{
			if (_testcaseProvider != null)
			{
				_testcaseProvider.Unload();
				_testcaseProvider = null;
			}
			_testcaseProvider = new TestcaseProvider(filename);
			_testcaseProvider.CreateAppDomain();
		}


		/// <summary>
		/// Load all Testcases from
		/// </summary>
		/// <returns></returns>
		internal List<string> GetTestcases()
		{

			string[] types = _testcaseProvider != null ? _testcaseProvider.LoadTypes() : new string[0];
			List<string> testcasesTypes = types.ToList();
			testcasesTypes.Sort();
			return testcasesTypes;
		}


		/// <summary>
		/// Start Tests
		/// </summary>
		/// <param name="testsystem">string with the testsystem</param>
		/// <param name="browsers">the browsers</param>
		/// <param name="testcases"></param>
		/// <param name="languages"></param>
		internal void TestLocal(string testsystem, List<string> browsers, List<string> testcases, List<string> languages)
		{
			_canceled = false;
			foreach (string testcase in testcases)
			{
				foreach (string browserName in browsers)
				{
					Browser browser = GetBrowser(browserName);
					foreach (string language in languages)
					{
						if (_canceled) return;
						InitializeTest(testcase, testsystem, language, browser);
					}
				}
			}
		}

		private Browser GetBrowser(string browserName)
		{
			return new Browser { Browserstring = browserName };
		}


		/// <summary>
		/// Start a Test and throw exception in errorcase
		/// </summary>
		/// <param name="typeName">typename of testcase to load</param>
		/// <param name="testsystem">string with the testsystem</param>
		/// <param name="language">string with the language</param>
		/// <param name="browser"></param>
		internal void InitializeTest(string typeName, string testsystem, string language, Browser browser)
		{
			TestHeader = String.Format("Testcase: {0} ({1}, {2}) on {3}", typeName, language, browser, testsystem);

			_testable = _testcaseProvider.GetTestableFromTypeName(typeName);
			_testable.GetLogLastTime();
			_testable.SetupTest(WebDriverInitStrategy.SeleniumLocal, browser, testsystem, language);
			try
			{
				_testable.Test();
			}
			catch (TaskCanceledException)
			{
				//Test is canceled. Do normal teardown
			}

			FinishedScreenshot = CurrentScreenshot;
			_finished = true;
			_testable.TeardownTest();
		}


		internal void CancelTests()
		{
			_canceled = true;
			_testable.CancelTest();
		}

		public string GetAppSetting(string key)
		{
			try
			{

				return (string) Settings.Default[key];
			}
			catch
			{
				//No appsettingsfile found.
				return "";
			}

		}

		public void SetAppSetting(string key, string value)
		{
			Settings.Default[key] = value;
			Settings.Default.Save();

		}

	}
}
