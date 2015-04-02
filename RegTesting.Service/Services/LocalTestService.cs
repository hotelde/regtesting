using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Logging;

namespace RegTesting.Service.Services
{

	/// <summary>
	/// the LocalTestService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class LocalTestService : ILocalTestService
	{
		private readonly ITestFileLocker _objTestFileLocker;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ILanguageRepository _objLanguageRepository;
		private readonly IBrowserRepository _objBrowserRepository;
		private readonly ITesterRepository _objTesterRepository;
		private readonly ITestsuiteRepository _objTestsuiteRepository;
		private readonly ITestcaseRepository _objTestcaseRepository;
		private readonly ITestPool _objTestPool;

		/// <summary>
		/// Local Test Service constructor
		/// </summary>
		/// <param name="objTestFileLocker">the testFileLocker</param>
		/// <param name="objTestsystemRepository">the testSystemRepository</param>
		/// <param name="objLanguageRepository">the languageRepository</param>
		/// <param name="objBrowserRepository">the browserRepository</param>
		/// <param name="objTesterRepository">the testerRepository</param>
		/// <param name="objTestsuiteRepository">the testsuiteRepository</param>
		/// <param name="objTestcaseRepository">the testcaseRepository</param>
		/// <param name="objTestPool">the testPool</param>
		public LocalTestService(ITestFileLocker objTestFileLocker, ITestsystemRepository objTestsystemRepository,
			ILanguageRepository objLanguageRepository, IBrowserRepository objBrowserRepository, ITesterRepository objTesterRepository,
			ITestsuiteRepository objTestsuiteRepository, ITestcaseRepository objTestcaseRepository, ITestPool objTestPool)
		{
			if (objTestFileLocker == null)
				throw new ArgumentNullException("objTestFileLocker");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
			if (objLanguageRepository == null)
				throw new ArgumentNullException("objLanguageRepository");
			if (objBrowserRepository == null)
				throw new ArgumentNullException("objBrowserRepository");
			if (objTesterRepository == null)
				throw new ArgumentNullException("objTesterRepository");
			if (objTestsuiteRepository == null)
				throw new ArgumentNullException("objTestsuiteRepository");
			if (objTestcaseRepository == null)
				throw new ArgumentNullException("objTestcaseRepository");
			if (objTestPool == null)
				throw new ArgumentNullException("objTestPool");

			_objTestFileLocker = objTestFileLocker;
			_objTestsystemRepository = objTestsystemRepository;
			_objLanguageRepository = objLanguageRepository;
			_objBrowserRepository = objBrowserRepository;
			_objTesterRepository = objTesterRepository;
			_objTestsuiteRepository = objTestsuiteRepository;
			_objTestcaseRepository = objTestcaseRepository;
			_objTestPool = objTestPool;
		}

		void ILocalTestService.SendTestcaseFile(string strTestsystem, byte[] arrData)
		{
			object objLock = _objTestFileLocker.GetLock(strTestsystem);
			lock (objLock)
			{
				Testsystem objTestsystem = _objTestsystemRepository.GetByName(strTestsystem);
				string strTestFile = RegtestingServerConfiguration.Testsfolder + objTestsystem.Filename;
				Directory.CreateDirectory(Path.GetDirectoryName(strTestFile));
				using (FileStream objFileStream = new FileStream(strTestFile, FileMode.Create, FileAccess.Write))
				{
					objFileStream.Write(arrData, 0, arrData.Length);
				}

				TestcaseProvider objTestcaseProvider = new TestcaseProvider(strTestFile);
				objTestcaseProvider.CreateAppDomain();
				foreach (string strTest in objTestcaseProvider.Types)
				{
					ITestable objTestable = objTestcaseProvider.GetTestableFromTypeName(strTest);
					if (objTestable == null) continue;

					Testcase objTestcase = _objTestcaseRepository.GetByType(strTest);
					string strTestableName = objTestable.GetName();
					if (objTestcase == null)
					{
						Logger.Log("New test: " + strTestableName);
						objTestcase = new Testcase { Activated = true, Name = strTestableName, Type = strTest };
						_objTestcaseRepository.Store(objTestcase);
					}
					else if (!objTestcase.Name.Equals(strTestableName))
					{
						Logger.Log("Renamed test: " + objTestcase.Name + " to " + strTestableName);
						objTestcase.Name = strTestableName;
						_objTestcaseRepository.Store(objTestcase);

					}

				}
				objTestcaseProvider.Unload();
			}




		}

		IEnumerable<LanguageDto> ILocalTestService.GetLanguages()
		{
			return Mapper.Map<IEnumerable<LanguageDto>>(_objLanguageRepository.GetAll());

		}

		IEnumerable<BrowserDto> ILocalTestService.GetBrowsers()
		{
			return Mapper.Map<IEnumerable<BrowserDto>>(_objBrowserRepository.GetAll());
		}

		void ILocalTestService.AddLocalTestTasks(string strUser, string strTestsystemName, string strTestsystemUrl, List<string> lstBrowser,
			List<string> lstTestcases, List<string> lstLanguages)
		{
			
			Testsystem objTestsystem = _objTestsystemRepository.GetByName(strTestsystemName);
			Tester objTester = _objTesterRepository.GetByName(strUser);

			objTestsystem.Url = strTestsystemUrl;
			_objTestsystemRepository.Store(objTestsystem);

			Testsuite objTestsuite = new Testsuite { Description = "", Name = "Local " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), TestsystemFilter = objTestsystem };
			_objTestsuiteRepository.Store(objTestsuite);

			int intTestsuite = objTestsuite.ID;
			List<int> lstTestcaseIds = lstTestcases.Select(t => _objTestcaseRepository.GetByType(t).ID).ToList();
			List<int> lstBrowserIds = lstBrowser.Select(t => _objBrowserRepository.GetByName(t).ID).ToList();
			List<int> lstLanguageIds = lstLanguages.Select(t => _objLanguageRepository.GetByLanguageCode(t).ID).ToList();


			_objTestsuiteRepository.SetTestcasesForTestsuite(intTestsuite, lstTestcaseIds);
			_objTestsuiteRepository.SetBrowsersForTestsuite(intTestsuite, lstBrowserIds);
			_objTestsuiteRepository.SetLanguagesForTestsuite(intTestsuite, lstLanguageIds);


			TestJob objTestjob = new TestJob
			{
				Name = "Testsuite " + objTestsuite.Name,
				ResultCode = TestState.Pending,
				Testsuite = objTestsuite,
				Testsystem = objTestsystem,
				Tester = objTester,
				StartedAt = DateTime.Now,
				JobType = JobType.Localtesttool
			};

			ITestJobManager objTestJobManager = new TestJobManager(objTestjob);

			ICollection<WorkItem> lstWorkItems = (from objTestcase in objTestsuite.Testcases
												  from objBrowser in objTestsuite.Browsers
												  from objLanguage in objTestsuite.Languages
												  select new WorkItem(objTestJobManager)
												  {
													  Testcase = objTestcase,
													  Browser = objBrowser,
													  Language = objLanguage,
													  Testsystem = objTestsystem,
													  Tester = objTester
												  }).ToList();

			_objTestPool.AddTestJob(objTestJobManager, lstWorkItems);
		}
	}
}
