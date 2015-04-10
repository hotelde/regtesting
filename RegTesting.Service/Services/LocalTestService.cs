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
using RegTesting.Tests.Core;

namespace RegTesting.Service.Services
{

	/// <summary>
	/// the LocalTestService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class LocalTestService : ILocalTestService
	{
		private readonly ITestFileLocker _testFileLocker;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ILanguageRepository _languageRepository;
		private readonly IBrowserRepository _browserRepository;
		private readonly ITesterRepository _testerRepository;
		private readonly ITestsuiteRepository _testsuiteRepository;
		private readonly ITestcaseRepository _testcaseRepository;
		private readonly ITestPool _testPool;

		/// <summary>
		/// Local Test Service constructor
		/// </summary>
		/// <param name="testFileLocker">the testFileLocker</param>
		/// <param name="testsystemRepository">the testSystemRepository</param>
		/// <param name="languageRepository">the languageRepository</param>
		/// <param name="browserRepository">the browserRepository</param>
		/// <param name="testerRepository">the testerRepository</param>
		/// <param name="testsuiteRepository">the testsuiteRepository</param>
		/// <param name="testcaseRepository">the testcaseRepository</param>
		/// <param name="testPool">the testPool</param>
		public LocalTestService(ITestFileLocker testFileLocker, ITestsystemRepository testsystemRepository,
			ILanguageRepository languageRepository, IBrowserRepository browserRepository, ITesterRepository testerRepository,
			ITestsuiteRepository testsuiteRepository, ITestcaseRepository testcaseRepository, ITestPool testPool)
		{
			if (testFileLocker == null)
				throw new ArgumentNullException("testFileLocker");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (languageRepository == null)
				throw new ArgumentNullException("languageRepository");
			if (browserRepository == null)
				throw new ArgumentNullException("browserRepository");
			if (testerRepository == null)
				throw new ArgumentNullException("testerRepository");
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");
			if (testcaseRepository == null)
				throw new ArgumentNullException("testcaseRepository");
			if (testPool == null)
				throw new ArgumentNullException("testPool");

			_testFileLocker = testFileLocker;
			_testsystemRepository = testsystemRepository;
			_languageRepository = languageRepository;
			_browserRepository = browserRepository;
			_testerRepository = testerRepository;
			_testsuiteRepository = testsuiteRepository;
			_testcaseRepository = testcaseRepository;
			_testPool = testPool;
		}

		void ILocalTestService.SendTestcaseFile(string testsystemName, byte[] data)
		{
			object _lock = _testFileLocker.GetLock(testsystemName);
			lock (_lock)
			{
				Testsystem testsystem = _testsystemRepository.GetByName(testsystemName);
				string testFile = RegtestingServerConfiguration.Testsfolder + testsystem.Filename;
				Directory.CreateDirectory(Path.GetDirectoryName(testFile));
				using (FileStream fileStream = new FileStream(testFile, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(data, 0, data.Length);
				}

				TestcaseProvider testcaseProvider = new TestcaseProvider(testFile);
				testcaseProvider.CreateAppDomain();
				foreach (string test in testcaseProvider.Types)
				{
					ITestable testable = testcaseProvider.GetTestableFromTypeName(test);
					if (testable == null) continue;

					Testcase testcase = _testcaseRepository.GetByType(test);
					string testableName = testable.GetName();
					if (testcase == null)
					{
						Logger.Log("New test: " + testableName);
						testcase = new Testcase { Activated = true, Name = testableName, Type = test };
						_testcaseRepository.Store(testcase);
					}
					else if (!testcase.Name.Equals(testableName))
					{
						Logger.Log("Renamed test: " + testcase.Name + " to " + testableName);
						testcase.Name = testableName;
						_testcaseRepository.Store(testcase);

					}

				}
				testcaseProvider.Unload();
			}
			
		}

		IEnumerable<LanguageDto> ILocalTestService.GetLanguages()
		{
			return Mapper.Map<IEnumerable<LanguageDto>>(_languageRepository.GetAll());

		}

		IEnumerable<BrowserDto> ILocalTestService.GetBrowsers()
		{
			return Mapper.Map<IEnumerable<BrowserDto>>(_browserRepository.GetAll());
		}

		void ILocalTestService.AddLocalTestTasks(string userName, string testsystemName, string testsystemUrl, List<string> browsers,
			List<string> testcases, List<string> languages)
		{
			
			Testsystem testsystem = _testsystemRepository.GetByName(testsystemName);
			Tester tester = _testerRepository.GetByName(userName);

			testsystem.Url = testsystemUrl;
			_testsystemRepository.Store(testsystem);

			Testsuite testsuite = new Testsuite { Description = "", Name = "Local " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), TestsystemFilter = testsystem };
			_testsuiteRepository.Store(testsuite);

			int testsuiteId = testsuite.ID;
			List<int> testcaseIds = testcases.Select(t => _testcaseRepository.GetByType(t).ID).ToList();
			List<int> browserIds = browsers.Select(t => _browserRepository.GetByName(t).ID).ToList();
			List<int> languageIds = languages.Select(t => _languageRepository.GetByLanguageCode(t).ID).ToList();


			_testsuiteRepository.SetTestcasesForTestsuite(testsuiteId, testcaseIds);
			_testsuiteRepository.SetBrowsersForTestsuite(testsuiteId, browserIds);
			_testsuiteRepository.SetLanguagesForTestsuite(testsuiteId, languageIds);


			TestJob testjob = new TestJob
			{
				Name = "Testsuite " + testsuite.Name,
				ResultCode = TestState.Pending,
				Testsuite = testsuite,
				Testsystem = testsystem,
				Tester = tester,
				StartedAt = DateTime.Now,
				JobType = JobType.Localtesttool
			};

			ITestJobManager testJobManager = new TestJobManager(testjob);

			ICollection<WorkItem> workItems = (from testcase in testsuite.Testcases
												  from browser in testsuite.Browsers
												  from language in testsuite.Languages
												  select new WorkItem(testJobManager)
												  {
													  Testcase = testcase,
													  Browser = browser,
													  Language = language,
													  Testsystem = testsystem,
													  Tester = tester
												  }).ToList();

			_testPool.AddTestJob(testJobManager, workItems);
		}
	}
}
