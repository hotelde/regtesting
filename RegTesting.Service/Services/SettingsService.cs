using System;
using System.Collections.Generic;
using AutoMapper;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// the settingsService
	/// </summary>
	public class SettingsService : ISettingsService
	{
		private readonly IBrowserRepository _browserRepository;
		private readonly ILanguageRepository _languageRepository;
		private readonly ITesterRepository _testerRepository;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ITestcaseRepository _testcaseRepository;
		private readonly ITestsuiteRepository _testsuiteRepository;

		/// <summary>
		/// Create a new SettingsService
		/// </summary>
		/// <param name="browserRepository">the BrowserRepository</param>
		/// <param name="languageRepository">the LanguageRepository</param>
		/// <param name="testerRepository">the TesterRepository</param>
		/// <param name="testsystemRepository">the TestsystemRepository</param>
		/// <param name="testcaseRepository">the TestcaseRepository</param>
		/// <param name="testsuiteRepository">the TestsuiteRepository</param>
		public SettingsService(IBrowserRepository browserRepository, ILanguageRepository languageRepository, 
			ITesterRepository testerRepository, ITestsystemRepository testsystemRepository, ITestcaseRepository testcaseRepository, ITestsuiteRepository testsuiteRepository)
		{
			if (browserRepository == null)
				throw new ArgumentNullException("browserRepository");
			if (languageRepository == null)
				throw new ArgumentNullException("languageRepository");
			if (testerRepository == null)
				throw new ArgumentNullException("testerRepository");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (testcaseRepository == null)
				throw new ArgumentNullException("testcaseRepository");
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");

			_browserRepository = browserRepository;
			_languageRepository = languageRepository;
			_testerRepository = testerRepository;
			_testsystemRepository = testsystemRepository;
			_testcaseRepository = testcaseRepository;
			_testsuiteRepository = testsuiteRepository;
		}


		IEnumerable<BrowserDto> ISettingsService.GetBrowsers()
		{
			return Mapper.Map<IEnumerable<BrowserDto>>(_browserRepository.GetAll());
		}

		BrowserDto ISettingsService.FindBrowserByID(int browserId)
		{
			return Mapper.Map<BrowserDto>(_browserRepository.GetById(browserId));
		}

		void ISettingsService.StoreBrowser(BrowserDto browserDto)
		{
			_browserRepository.Store(Mapper.Map<Browser>(browserDto));
		}

		void ISettingsService.DeleteBrowserByID(int browserId)
		{
			_browserRepository.RemoveById(browserId);
		}


		IEnumerable<LanguageDto> ISettingsService.GetLanguages()
		{
			return Mapper.Map<IEnumerable<LanguageDto>>(_languageRepository.GetAll());
		}

		LanguageDto ISettingsService.FindLanguageByID(int languageId)
		{
			return Mapper.Map<LanguageDto>(_languageRepository.GetById(languageId));
		}


		void ISettingsService.StoreLanguage(LanguageDto languageDto)
		{
			_languageRepository.Store(Mapper.Map<Language>(languageDto));
		}

		void ISettingsService.DeleteLanguageByID(int languageId)
		{
			_languageRepository.RemoveById(languageId);
		}

		IEnumerable<TesterDto> ISettingsService.GetTesters()
		{
			return Mapper.Map<IEnumerable<TesterDto>>(_testerRepository.GetAll());
		}

		TesterDto ISettingsService.FindTesterByID(int testerId)
		{
			return Mapper.Map<TesterDto>(_testerRepository.GetById(testerId));
		}


		void ISettingsService.StoreTester(TesterDto testerDto)
		{
			_testerRepository.Store(Mapper.Map<Tester>(testerDto));
		}

		void ISettingsService.DeleteTesterByID(int testerId)
		{
			_testerRepository.RemoveById(testerId);
		}


		IEnumerable<TestsystemDto> ISettingsService.GetTestsystems()
		{
			return Mapper.Map<IEnumerable<TestsystemDto>>(_testsystemRepository.GetAll());
		}

		TestsystemDto ISettingsService.FindTestsystemByID(int testsystemId)
		{
			return Mapper.Map<TestsystemDto>(_testsystemRepository.GetById(testsystemId));
		}


		void ISettingsService.StoreTestsystem(TestsystemDto testsystemDto)
		{
			_testsystemRepository.Store(Mapper.Map<Testsystem>(testsystemDto));
		}

		void ISettingsService.DeleteTestsystemByID(int testsystemId)
		{
			_testsystemRepository.RemoveById(testsystemId);
		}


		IEnumerable<TestsuiteDto> ISettingsService.GetTestsuites()
		{
			return Mapper.Map<IEnumerable<TestsuiteDto>>(_testsuiteRepository.GetAll());
		}

		TestsuiteDto ISettingsService.FindTestsuiteByID(int testsuiteId)
		{
			return Mapper.Map<TestsuiteDto>(_testsuiteRepository.GetById(testsuiteId));
		}

		void ISettingsService.StoreTestsuite(TestsuiteDto testsuiteDto)
		{
			if (testsuiteDto.ID == 0)
			{
				_testsuiteRepository.Store(Mapper.Map<Testsuite>(testsuiteDto));
			}
			else
			{
				Testsuite objTestsuiteCurrent = _testsuiteRepository.GetById(testsuiteDto.ID);

				objTestsuiteCurrent.Name = testsuiteDto.Name;
				objTestsuiteCurrent.Description = testsuiteDto.Description;

				_testsuiteRepository.Store(objTestsuiteCurrent);
			}
	
		}

		void ISettingsService.DeleteTestsuiteByID(int testsuiteId)
		{
			_testsuiteRepository.RemoveById(testsuiteId);
		}



		IEnumerable<TestcaseDto> ISettingsService.GetTestcases()
		{
			return Mapper.Map<IEnumerable<TestcaseDto>>(_testcaseRepository.GetAll());
		}

		TestcaseDto ISettingsService.FindTestcaseByID(int testcaseId)
		{
			return Mapper.Map<TestcaseDto>(_testcaseRepository.GetById(testcaseId));
		}

		void ISettingsService.EditTestcase(TestcaseDto testcaseDto)
		{
			_testcaseRepository.Store(Mapper.Map<Testcase>(testcaseDto));
		}

		void ISettingsService.DeleteTestcaseByID(int testcaseId)
		{
			_testcaseRepository.RemoveById(testcaseId);
		}



		Error ISettingsService.GetError(int error)
		{
			throw new NotImplementedException();
		}

		void ISettingsService.EditError(Error error)
		{
			throw new NotImplementedException();
		}

		void ISettingsService.SetTestcasesForTestsuite(int testsuiteId, ICollection<int> testcases)
		{
			_testsuiteRepository.SetTestcasesForTestsuite(testsuiteId, testcases);
		}


		void ISettingsService.SetBrowsersForTestsuite(int testsuiteId, ICollection<int> browsers)
		{
			_testsuiteRepository.SetBrowsersForTestsuite(testsuiteId, browsers);
		}


		void ISettingsService.SetLanguagesForTestsuite(int testsuiteId, ICollection<int> languages)
		{
			_testsuiteRepository.SetLanguagesForTestsuite(testsuiteId, languages);
		}
	}
}
