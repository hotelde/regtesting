﻿using System;
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
using RegTesting.Tests.Core;

namespace RegTesting.Service.Services
{
	/// <summary>
	/// The BuildTaskService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class BuildTaskService : IBuildTaskService
	{
		private readonly ITestFileLocker _testFileLocker;
		private readonly ITestsystemRepository _testsystemRepository;
		private readonly ITesterRepository _testerRepository;
		private readonly ITestsuiteRepository _testsuiteRepository;
		private readonly ITestcaseRepository _testcaseRepository;
		private readonly ITestPool _testPool;
		private readonly ITestJobRepository _testJobRepository;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="testFileLocker">the testFileLocker</param>
		/// <param name="testsystemRepository">the testsystemRepository</param>
		/// <param name="testerRepository">the testerRepository</param>
		/// <param name="testsuiteRepository">the testsuiteRepository</param>
		/// <param name="testcaseRepository">the testcaseRepository</param>
		/// <param name="testPool">the testPool</param>
		/// <param name="testJobRepository">the testJobRepository</param>
		public BuildTaskService(ITestFileLocker testFileLocker, ITestsystemRepository testsystemRepository,
			ITesterRepository testerRepository, ITestsuiteRepository testsuiteRepository, ITestcaseRepository testcaseRepository,
			ITestPool testPool, ITestJobRepository testJobRepository)
		{
			if (testFileLocker == null)
				throw new ArgumentNullException("testFileLocker");
			if (testsystemRepository == null)
				throw new ArgumentNullException("testsystemRepository");
			if (testerRepository == null)
				throw new ArgumentNullException("testerRepository");
			if (testsuiteRepository == null)
				throw new ArgumentNullException("testsuiteRepository");
			if (testcaseRepository == null)
				throw new ArgumentNullException("testcaseRepository");
			if (testPool == null)
				throw new ArgumentNullException("testPool");
			if (testJobRepository == null)
				throw new ArgumentNullException("testJobRepository");
			_testFileLocker = testFileLocker;
			_testsystemRepository = testsystemRepository;
			_testerRepository = testerRepository;
			_testsuiteRepository = testsuiteRepository;
			_testcaseRepository = testcaseRepository;
			_testPool = testPool;
			_testJobRepository = testJobRepository;
		}

		void IBuildTaskService.SendTestcaseFile(string testsystemName, byte[] data)
		{
			object _lock = _testFileLocker.GetLock(testsystemName);
			lock(_lock)
			{
				Testsystem testsystem = _testsystemRepository.GetByName(testsystemName);
				testsystem.LastUpdated = DateTime.Now;
				_testsystemRepository.Store(testsystem);
				string testFile = RegtestingServerConfiguration.Testsfolder + testsystem.Filename;
				Directory.CreateDirectory(Path.GetDirectoryName(testFile));
				using (FileStream fileStream = new FileStream(testFile, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(data, 0, data.Length);
				}
				Logger.Log("UPDATE branch: " + testsystemName);
				TestcaseProvider testcaseProvider = new TestcaseProvider(testFile);
				testcaseProvider.CreateAppDomain();
				foreach (string testcaseType in testcaseProvider.Types)
				{
					ITestable testable = testcaseProvider.GetTestableFromTypeName(testcaseType);
					if(testable==null) continue;
					
					Testcase testcase = _testcaseRepository.GetByType(testcaseType);
					string testableName = testable.GetName();
					if (testcase == null)
					{
						Logger.Log("New test: " + testableName);
						testcase = new Testcase { Name = testableName, Type = testcaseType };
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
		
		int IBuildTaskService.AddRegTestTasks(string testsystemName, string emailReceiver, string testsuiteName, string branch, string commitId, string commitMessage)
		{
			Testsuite testsuite = _testsuiteRepository.GetByName(testsuiteName);
			Testsystem testsystem = _testsystemRepository.GetByName(testsystemName);
			Tester tester = _testerRepository.GetByName(emailReceiver);
			string testjobname;
			if (!String.IsNullOrEmpty(commitId) && !String.IsNullOrEmpty(branch))
			{
				testjobname = "Commit " + commitId + " on " + ParseBranchName(branch);
			}
			else
			{
				testjobname = "Testsuite " + testsuiteName;
			}

			TestJob testjob = new TestJob
			{
				Name = testjobname,
				Description = commitMessage,
				ResultCode = TestState.Pending,
				Testsuite = testsuite,
				Testsystem = testsystem,
				Tester = tester,
				StartedAt = DateTime.Now,
				JobType = JobType.Buildtask
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
			return testjob.ID;
		}

		int IBuildTaskService.GetTestJobResult(int testJobId)
		{
			TestJob testjob = _testJobRepository.GetById(testJobId);
			if (testjob == null)
			{
				return (int) TestState.NotAvailable;
			}
			return (int) testjob.ResultCode;
		}

		private string ParseBranchName(string branchLine)
		{
			List<string> branchList = new List<string>();
			string[] branches = branchLine.Split(';');
			foreach (string branch in branches)
			{
				if (branch.StartsWith("remotes/origin/"))
				{
					branchList.Add(branch.Replace("remotes/origin/",""));
				}
			}


			return string.Join(", ", branchList);
		}
	}
}
