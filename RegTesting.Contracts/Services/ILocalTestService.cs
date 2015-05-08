using System.Collections.Generic;
using System.ServiceModel;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	
	/// <summary>
	/// the LocalTestService
	/// </summary>
	[ServiceContract(Namespace = "http://hotelde.regtesting/localservice", ConfigurationName = "ILocalTestService")]
	public interface ILocalTestService
	{
		/// <summary>
		/// send a testcase file
		/// </summary>
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="data">the testfiledata</param>
		[OperationContract]
		void SendTestcaseFile(string testsystemName, byte[] data);

		/// <summary>
		/// Get languages
		/// </summary>
		/// <returns>a enumerable of languages</returns>
		[OperationContract]
		IEnumerable<LanguageDto> GetLanguages();

		/// <summary>
		/// Get browsers
		/// </summary>
		/// <returns>a enumerable of browsers</returns>
		[OperationContract]
		IEnumerable<BrowserDto> GetBrowsers();

		/// <summary>
		/// Get testsuites
		/// </summary>
		/// <returns>a enumerable of testsuites</returns>
		[OperationContract]
		IEnumerable<TestsuiteDto> GetTestsuites();

		/// <summary>
		/// add a local test task
		/// </summary>
		/// <param name="userName">the user</param>
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="testsystemUrl">the testurl</param>
		/// <param name="browsers">the browsers</param>
		/// <param name="testcases">the testcases</param>
		/// <param name="languages">the languages</param>
		[OperationContract]
		void AddLocalTestTasks(string userName, string testsystemName, string testsystemUrl, List<string> browsers, List<string> testcases, List<string> languages);

		/// <summary>
		/// start a testsuite via localtool
		/// </summary>
		/// <param name="testsystemName">the testsystem</param>
		/// <param name="userName">the releasemanager</param>
		/// <param name="testsuiteName">the testsuite</param>
		/// <param name="testsystemUrl">the testurl</param>
		[OperationContract]
		void AddTestsuiteTask(string userName, string testsystemName, string testsystemUrl, string testsuiteName);
	
	
	}
}
