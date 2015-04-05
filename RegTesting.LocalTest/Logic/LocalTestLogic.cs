using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RegTesting.Contracts;
using RegTesting.LocalTest.Properties;
using RegTesting.Tests.Core;

namespace RegTesting.LocalTest.Logic
{
	class LocalTestLogic
	{
		private ITestable _objTestable;

		public string TestHeader { get; private set; }
		public List<string> LogEntries { get { return _objTestable.GetLog(); }}

		private bool _bolCanceled;

		/// <summary>
		/// Load all Testcases from
		/// </summary>
		/// <returns></returns>
		internal List<string> GetTestcases()
		{
			string[] arrTypes = new TypesLoader().GetTypes();
			List<string> lstTestcasesTypes = arrTypes.ToList();
			lstTestcasesTypes.Sort();
			return lstTestcasesTypes;
		}


		/// <summary>
		/// Start Tests
		/// </summary>
		/// <param name="strTestsystem">string with the testsystem</param>
		/// <param name="lstBrowsers">the browsers</param>
		/// <param name="lstTestcases"></param>
		/// <param name="lstLanguages"></param>
		internal void TestLocal(string strTestsystem, List<string> lstBrowsers, List<string> lstTestcases, List<string> lstLanguages)
		{
			_bolCanceled = false;
			foreach (string strTestcase in lstTestcases)
			{
				foreach (string strBrowser in lstBrowsers)
				{
					Browser objBrowser = GetBrowser(strBrowser);
					foreach (string strLanguage in lstLanguages)
					{
						if (_bolCanceled) return;
						InitializeTest(strTestcase, strTestsystem, strLanguage, objBrowser);
					}
				}
			}
		}

		private Browser GetBrowser(string strBrowser)
		{
			return new Browser { Browserstring = strBrowser };
		}


		/// <summary>
		/// Start a Test and throw exception in errorcase
		/// </summary>
		/// <param name="strTypeName">typename of testcase to load</param>
		/// <param name="strTestsystem">string with the testsystem</param>
		/// <param name="strLanguage">string with the language</param>
		/// <param name="objBrowser"></param>
		internal void InitializeTest(string strTypeName, string strTestsystem, string strLanguage, Browser objBrowser)
		{
			TestHeader = String.Format("Testcase: {0} ({1}, {2}) on {3}", strTypeName, strLanguage, objBrowser, strTestsystem);

			_objTestable = GetTestable(strTypeName);
			_objTestable.GetLogLastTime();
			_objTestable.SetupTest(WebDriverInitStrategy.SeleniumLocal, objBrowser, strTestsystem, strLanguage);
			try
			{
				_objTestable.Test();
			}
			catch (TaskCanceledException)
			{
				//Test is canceled. Do normal teardown
			}

			_objTestable.TeardownTest();
		}


		internal void CancelTests()
		{
			_bolCanceled = true;
			_objTestable.CancelTest();
		}


		/// <summary>
		/// Create a testable from a TypeName
		/// </summary>
		/// <param name="strTypeName">TypeName of Testcase</param>
		/// <returns>An ITestable with the created testcase</returns>
		private ITestable GetTestable(string strTypeName)
		{
			Assembly objAssembly = AppDomain.CurrentDomain.GetAssemblies().Single(objAssemb => objAssemb.GetName().Name == "RegTesting.Tests");
			Type objType = objAssembly.GetType(strTypeName);
			if (typeof(ITestable).IsAssignableFrom(objType) && !objType.IsAbstract)
			{
				ITestable objTestable = (ITestable)AppDomain.CurrentDomain.CreateInstance("RegTesting.Tests", strTypeName).Unwrap();
				return objTestable;
			}
			return null;
		}

		public string GetAppSetting(string strKey)
		{
			try
			{

				return (string) Settings.Default[strKey];
			}
			catch
			{
				//No appsettingsfile found.
				return "";
			}

		}

		public void SetAppSetting(string strKey, string strValue)
		{
			Settings.Default[strKey] = strValue;
			Settings.Default.Save();

		}

	}
}
