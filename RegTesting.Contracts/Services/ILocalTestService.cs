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
		/// <param name="strTestsystem">the testsystem</param>
		/// <param name="arrData">the testfiledata</param>
		[OperationContract]
		void SendTestcaseFile(string strTestsystem, byte[] arrData);

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
		/// add a local test task
		/// </summary>
		/// <param name="strUser">the user</param>
		/// <param name="strTestsystemName">the testsystem</param>
		/// <param name="strTestsystemUrl">the testurl</param>
		/// <param name="lstBrowser">the browsers</param>
		/// <param name="lstTestcases">the testcases</param>
		/// <param name="lstLanguages">the languages</param>
		[OperationContract]
		void AddLocalTestTasks(string strUser, string strTestsystemName, string strTestsystemUrl, List<string> lstBrowser, List<string> lstTestcases, List<string> lstLanguages);
	}
}
