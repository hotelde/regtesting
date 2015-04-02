using System.Collections.Generic;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;

namespace RegTesting.Contracts.Services
{
	/// <summary>
	/// the SettingsService
	/// </summary>
	public interface ISettingsService
	{
		/// <summary>
		/// Get all browsers
		/// </summary>
		/// <returns>a enumerable of browsers</returns>
		IEnumerable<BrowserDto> GetBrowsers();
		/// <summary>
		/// find a browser by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a browserDto</returns>
		BrowserDto FindBrowserByID(int intID);

		/// <summary>
		/// edit a browser
		/// </summary>
		/// <param name="objBrowser">a browserDto</param>
		void StoreBrowser(BrowserDto objBrowser);
		/// <summary>
		/// delete a browser by id
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteBrowserByID(int intID);

		/// <summary>
		/// get all languages
		/// </summary>
		/// <returns>a enumerable of languages</returns>
		IEnumerable<LanguageDto> GetLanguages();
		/// <summary>
		/// find a language by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a languageDto</returns>
		LanguageDto FindLanguageByID(int intID);

		/// <summary>
		/// edit a language
		/// </summary>
		/// <param name="objLanguage">the languageDto</param>
		void StoreLanguage(LanguageDto objLanguage);
		/// <summary>
		/// delete a language by id
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteLanguageByID(int intID);

		/// <summary>
		/// get all testers
		/// </summary>
		/// <returns>a enumerable of testers</returns>
		IEnumerable<TesterDto> GetTesters();
		/// <summary>
		/// find a tester by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a testerDto</returns>
		TesterDto FindTesterByID(int intID);

		/// <summary>
		/// edit a tester
		/// </summary>
		/// <param name="objTester">the testerDto</param>
		void StoreTester(TesterDto objTester);
		/// <summary>
		/// delete a tester by id
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteTesterByID(int intID);

		/// <summary>
		/// get all testsystems
		/// </summary>
		/// <returns>a enumerable of testsystems</returns>
		IEnumerable<TestsystemDto> GetTestsystems();
		/// <summary>
		/// find a testsystem by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a testsystemDto</returns>
		TestsystemDto FindTestsystemByID(int intID);

		/// <summary>
		/// edit a testsystem
		/// </summary>
		/// <param name="objTestsystem">the testsystemDto</param>
		void StoreTestsystem(TestsystemDto objTestsystem);
		/// <summary>
		/// delete a testsystem by id
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteTestsystemByID(int intID);

		/// <summary>
		/// get all testcases
		/// </summary>
		/// <returns>a enumerable of testcases</returns>
		IEnumerable<TestcaseDto> GetTestcases();
		/// <summary>
		/// find a testcase by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a testcaseDto</returns>
		TestcaseDto FindTestcaseByID(int intID);
		/// <summary>
		/// edit a testcase
		/// </summary>
		/// <param name="objTestcase">the testcaseDto</param>
		void EditTestcase(TestcaseDto objTestcase);
		/// <summary>
		/// delete a testcase by id
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteTestcaseByID(int intID);

		/// <summary>
		/// get all testsuites
		/// </summary>
		/// <returns>a enumerable of testsuites</returns>
		IEnumerable<TestsuiteDto> GetTestsuites();
		/// <summary>
		/// find a testsuite by id
		/// </summary>
		/// <param name="intID">the id</param>
		/// <returns>a testsuiteDto</returns>
		TestsuiteDto FindTestsuiteByID(int intID);

		/// <summary>
		/// edit a testsuite
		/// </summary>
		/// <param name="objTestsuite">the testsuiteDto</param>
		void StoreTestsuite(TestsuiteDto objTestsuite);
		/// <summary>
		/// delete a testsuite
		/// </summary>
		/// <param name="intID">the id</param>
		void DeleteTestsuiteByID(int intID);

		/// <summary>
		/// get an error
		/// </summary>
		/// <param name="error">the error</param>
		/// <returns>a error</returns>
		Error GetError(int error);
		/// <summary>
		/// edit an error
		/// </summary>
		/// <param name="error">the error</param>
		void EditError(Error error);
	
		/// <summary>
		/// set the testcases for a  testsuite
		/// </summary>
		/// <param name="testsuite">the testsuite</param>
		/// <param name="testcases">the testcases</param>
		void SetTestcasesForTestsuite(int testsuite, ICollection<int> testcases);
		/// <summary>
		/// set the browsers for a testsuite
		/// </summary>
		/// <param name="testsuite">the testsuite</param>
		/// <param name="browsers">the browsers</param>
		void SetBrowsersForTestsuite(int testsuite, ICollection<int> browsers);
		/// <summary>
		/// set the languages for a testsuite
		/// </summary>
		/// <param name="testsuite">the testsuite</param>
		/// <param name="languages">the languages</param>
		void SetLanguagesForTestsuite(int testsuite, ICollection<int> languages);
	}
}
