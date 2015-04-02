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
		private readonly IBrowserRepository _objBrowserRepository;
		private readonly ILanguageRepository _objLanguageRepository;
		private readonly ITesterRepository _objTesterRepository;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ITestcaseRepository _objTestcaseRepository;
		private readonly ITestsuiteRepository _objTestsuiteRepository;

		/// <summary>
		/// Create a new SettingsService
		/// </summary>
		/// <param name="objBrowserRepository">the BrowserRepository</param>
		/// <param name="objLanguageRepository">the LanguageRepository</param>
		/// <param name="objTesterRepository">the TesterRepository</param>
		/// <param name="objTestsystemRepository">the TestsystemRepository</param>
		/// <param name="objTestcaseRepository">the TestcaseRepository</param>
		/// <param name="objTestsuiteRepository">the TestsuiteRepository</param>
		public SettingsService(IBrowserRepository objBrowserRepository, ILanguageRepository objLanguageRepository, 
			ITesterRepository objTesterRepository, ITestsystemRepository objTestsystemRepository, ITestcaseRepository objTestcaseRepository, ITestsuiteRepository objTestsuiteRepository)
		{
			if (objBrowserRepository == null)
				throw new ArgumentNullException("objBrowserRepository");
			if (objLanguageRepository == null)
				throw new ArgumentNullException("objLanguageRepository");
			if (objTesterRepository == null)
				throw new ArgumentNullException("objTesterRepository");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
			if (objTestcaseRepository == null)
				throw new ArgumentNullException("objTestcaseRepository");
			if (objTestsuiteRepository == null)
				throw new ArgumentNullException("objTestsuiteRepository");

			_objBrowserRepository = objBrowserRepository;
			_objLanguageRepository = objLanguageRepository;
			_objTesterRepository = objTesterRepository;
			_objTestsystemRepository = objTestsystemRepository;
			_objTestcaseRepository = objTestcaseRepository;
			_objTestsuiteRepository = objTestsuiteRepository;
		}


		IEnumerable<BrowserDto> ISettingsService.GetBrowsers()
		{
			return Mapper.Map<IEnumerable<BrowserDto>>(_objBrowserRepository.GetAll());
		}

		BrowserDto ISettingsService.FindBrowserByID(int intID)
		{
			return Mapper.Map<BrowserDto>(_objBrowserRepository.GetById(intID));
		}

		void ISettingsService.StoreBrowser(BrowserDto objBrowser)
		{
			_objBrowserRepository.Store(Mapper.Map<Browser>(objBrowser));
		}

		void ISettingsService.DeleteBrowserByID(int intID)
		{
			_objBrowserRepository.RemoveById(intID);
		}


		IEnumerable<LanguageDto> ISettingsService.GetLanguages()
		{
			return Mapper.Map<IEnumerable<LanguageDto>>(_objLanguageRepository.GetAll());
		}

		LanguageDto ISettingsService.FindLanguageByID(int intID)
		{
			return Mapper.Map<LanguageDto>(_objLanguageRepository.GetById(intID));
		}


		void ISettingsService.StoreLanguage(LanguageDto objLanguage)
		{
			_objLanguageRepository.Store(Mapper.Map<Language>(objLanguage));
		}

		void ISettingsService.DeleteLanguageByID(int intID)
		{
			_objLanguageRepository.RemoveById(intID);
		}

		IEnumerable<TesterDto> ISettingsService.GetTesters()
		{
			return Mapper.Map<IEnumerable<TesterDto>>(_objTesterRepository.GetAll());
		}

		TesterDto ISettingsService.FindTesterByID(int intID)
		{
			return Mapper.Map<TesterDto>(_objTesterRepository.GetById(intID));
		}


		void ISettingsService.StoreTester(TesterDto objTester)
		{
			_objTesterRepository.Store(Mapper.Map<Tester>(objTester));
		}

		void ISettingsService.DeleteTesterByID(int intID)
		{
			_objTesterRepository.RemoveById(intID);
		}


		IEnumerable<TestsystemDto> ISettingsService.GetTestsystems()
		{
			return Mapper.Map<IEnumerable<TestsystemDto>>(_objTestsystemRepository.GetAll());
		}

		TestsystemDto ISettingsService.FindTestsystemByID(int intID)
		{
			return Mapper.Map<TestsystemDto>(_objTestsystemRepository.GetById(intID));
		}


		void ISettingsService.StoreTestsystem(TestsystemDto objTestsystem)
		{
			_objTestsystemRepository.Store(Mapper.Map<Testsystem>(objTestsystem));
		}

		void ISettingsService.DeleteTestsystemByID(int intID)
		{
			_objTestsystemRepository.RemoveById(intID);
		}


		IEnumerable<TestsuiteDto> ISettingsService.GetTestsuites()
		{
			return Mapper.Map<IEnumerable<TestsuiteDto>>(_objTestsuiteRepository.GetAll());
		}

		TestsuiteDto ISettingsService.FindTestsuiteByID(int intID)
		{
			return Mapper.Map<TestsuiteDto>(_objTestsuiteRepository.GetById(intID));
		}

		void ISettingsService.StoreTestsuite(TestsuiteDto objTestsuite)
		{
			if (objTestsuite.ID == 0)
			{
				_objTestsuiteRepository.Store(Mapper.Map<Testsuite>(objTestsuite));
			}
			else
			{
				Testsuite objTestsuiteCurrent = _objTestsuiteRepository.GetById(objTestsuite.ID);

				objTestsuiteCurrent.Name = objTestsuite.Name;
				objTestsuiteCurrent.Description = objTestsuite.Description;

				_objTestsuiteRepository.Store(objTestsuiteCurrent);
			}
	
		}

		void ISettingsService.DeleteTestsuiteByID(int intID)
		{
			_objTestsuiteRepository.RemoveById(intID);
		}



		IEnumerable<TestcaseDto> ISettingsService.GetTestcases()
		{
			return Mapper.Map<IEnumerable<TestcaseDto>>(_objTestcaseRepository.GetAll());
		}

		TestcaseDto ISettingsService.FindTestcaseByID(int intID)
		{
			return Mapper.Map<TestcaseDto>(_objTestcaseRepository.GetById(intID));
		}

		void ISettingsService.EditTestcase(TestcaseDto objTestcase)
		{
			_objTestcaseRepository.Store(Mapper.Map<Testcase>(objTestcase));
		}

		void ISettingsService.DeleteTestcaseByID(int intID)
		{
			_objTestcaseRepository.RemoveById(intID);
		}



		Error ISettingsService.GetError(int error)
		{
			throw new NotImplementedException();
		}

		void ISettingsService.EditError(Error error)
		{
			throw new NotImplementedException();
		}

		void ISettingsService.SetTestcasesForTestsuite(int intTestsuite, ICollection<int> colTestcases)
		{
			_objTestsuiteRepository.SetTestcasesForTestsuite(intTestsuite, colTestcases);
		}


		void ISettingsService.SetBrowsersForTestsuite(int intTestsuite, ICollection<int> colBrowsers)
		{
			_objTestsuiteRepository.SetBrowsersForTestsuite(intTestsuite, colBrowsers);
		}


		void ISettingsService.SetLanguagesForTestsuite(int intTestsuite, ICollection<int> colLanguages)
		{
			_objTestsuiteRepository.SetLanguagesForTestsuite(intTestsuite, colLanguages);
		}
	}
}
