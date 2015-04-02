using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using RegTesting.Contracts.Attributes;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
	/// <summary>
	/// Template for a testcase. For most testcases you should implement the Testcase class.
	/// </summary>
	[ServiceContract]
	public interface ITestable
	{
		/// <summary>
		/// Initialize the test with a driver and a baseurl. Then start this specific test with Test().
		/// </summary>
		/// <param name="objWebDriverInitStrategy">the Webdriver init strategy</param>
		/// <param name="objTemplate">Browser to Test on </param>
		/// <param name="strBaseurl">The baseurl to test on<example>http://gamma</example></param>
		/// <param name="strLanguagecode">Languagecode for Test<example>DE</example></param>
		/// <param name="intTimeOut">a test timeout</param>
		/// <returns>Currently returning a empty string, later on we can contain errormessages etc.</returns>
		[OperationContract]
		void SetupTest(WebDriverInitStrategy objWebDriverInitStrategy, Browser objTemplate, string strBaseurl, string strLanguagecode, int intTimeOut = 0);

		/// <summary>
		/// Our mainfunction of the Testclasses, here comes the logic of the testcases.
		/// </summary>
		[OperationContract]
		void Test();

		/// <summary>
		/// The cleaningfunction. Here we close our driverconnections etc.
		/// </summary>
		/// <returns>A Exception or null else</returns>
		[OperationContract]
		void TeardownTest();

		/// <summary>
		/// Returns the name of the testcase.
		/// </summary>
		/// <returns>A string with the name of the testcase.</returns>
		[OperationContract]
		String GetName();

		/// <summary>
		/// The id of the testcase, set on executiontime.
		/// </summary>
		[DataMember]
		int ID { get; set; }

		/// <summary>
		/// Save a Screenshot
		/// </summary>
		/// <param name="strFolder">The folder, where to save screenshot</param>
		/// <returns>A string with the filename to the screenshot</returns>
		string SaveScreenshot(string strFolder);

		/// <summary>
		/// Implementation of Dispose 
		/// </summary>
		void Dispose();

		/// <summary>
		/// Get the testing log.
		/// </summary>
		/// <returns>The log entries as list of string</returns>
		[OperationContract]
		List<string> GetLogLastTime();

		/// <summary>
		/// Get the testing log.
		/// </summary>
		/// <returns>The log entries as list of string</returns>
		[OperationContract]
		List<string> GetLog();

		/// <summary>
		/// Cancel the current test
		/// </summary>
		[OperationContract]
		void CancelTest();

		/// <summary>
		/// Get the supported Languages
		/// </summary>
		/// <returns>an array with all supported languages or null if default (every language is supported)</returns>
		[OperationContract]
		string[] GetSupportedLanguages();

		/// <summary>
		/// Get the supported Browsers
		/// </summary>
		/// <returns>an array with all supported browsers or null if default (every browser is supported)</returns>
		[OperationContract]
		string[] GetSupportedBrowsers();

		/// <summary>
		/// Check if there is a server error
		/// </summary>
		/// <returns>a string with the error name</returns>
		[OperationContract]
		ServerErrorModel CheckForServerError();
	}

}