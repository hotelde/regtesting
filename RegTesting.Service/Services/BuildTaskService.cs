using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Enums;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Logging;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// The BuildTaskService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class BuildTaskService : IBuildTaskService
	{
		private readonly ITestFileLocker _objTestFileLocker;
		private readonly ITestsystemRepository _objTestsystemRepository;
		private readonly ITesterRepository _objTesterRepository;
		private readonly ITestsuiteRepository _objTestsuiteRepository;
		private readonly ITestcaseRepository _objTestcaseRepository;
		private readonly ITestPool _objTestPool;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objTestFileLocker">the testFileLocker</param>
		/// <param name="objTestsystemRepository">the testsystemRepository</param>
		/// <param name="objTesterRepository">the testerRepository</param>
		/// <param name="objTestsuiteRepository">the testsuiteRepository</param>
		/// <param name="objTestcaseRepository">the testcaseRepository</param>
		/// <param name="objTestPool">the testPool</param>
		public BuildTaskService(ITestFileLocker objTestFileLocker, ITestsystemRepository objTestsystemRepository,
			ITesterRepository objTesterRepository, ITestsuiteRepository objTestsuiteRepository, ITestcaseRepository objTestcaseRepository, ITestPool objTestPool)
		{
			if (objTestFileLocker == null)
				throw new ArgumentNullException("objTestFileLocker");
			if (objTestsystemRepository == null)
				throw new ArgumentNullException("objTestsystemRepository");
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
			_objTesterRepository = objTesterRepository;
			_objTestsuiteRepository = objTestsuiteRepository;
			_objTestcaseRepository = objTestcaseRepository;
			_objTestPool = objTestPool;
		}

		void IBuildTaskService.SendTestcaseFile(string strTestsystem, byte[] arrData)
		{
			object objLock = _objTestFileLocker.GetLock(strTestsystem);
			lock(objLock)
			{
				Testsystem objTestsystem = _objTestsystemRepository.GetByName(strTestsystem);
				objTestsystem.LastUpdated = DateTime.Now;
				_objTestsystemRepository.Store(objTestsystem);
				string strTestFile = RegtestingServerConfiguration.Testsfolder + objTestsystem.Filename;
				Directory.CreateDirectory(Path.GetDirectoryName(strTestFile));
				using (FileStream objFileStream = new FileStream(strTestFile, FileMode.Create, FileAccess.Write))
				{
					objFileStream.Write(arrData, 0, arrData.Length);
				}
				Logger.Log("UPDATE branch: " + strTestsystem);
				TestcaseProvider objTestcaseProvider = new TestcaseProvider(strTestFile);
				objTestcaseProvider.CreateAppDomain();
				foreach (string strTest in objTestcaseProvider.Types)
				{
					ITestable objTestable = objTestcaseProvider.GetTestableFromTypeName(strTest);
					if(objTestable==null) continue;
					
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
		
		void IBuildTaskService.AddRegTestTasks(string strTestsystem, string strReleaseManager, string strTestsuite)
		{
			Testsuite objTestsuite = _objTestsuiteRepository.GetByName(strTestsuite);
			Testsystem objTestsystem = _objTestsystemRepository.GetByName(strTestsystem);
			Tester objTester = _objTesterRepository.GetByName(strReleaseManager);

			TestJob objTestjob = new TestJob
			{
				Name = "Testsuite " + objTestsuite.Name,
				ResultCode = TestState.Pending,
				Testsuite = objTestsuite,
				Testsystem = objTestsystem,
				Tester = objTester,
				StartedAt = DateTime.Now,
				JobType = JobType.Buildtask
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
