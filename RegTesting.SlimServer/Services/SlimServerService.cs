using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;
using RegTesting.Tests.Core;

namespace RegTesting.SlimServer.Services
{
	/// <summary>
	/// The SlimServerService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class SlimServerService : ISlimServerService
	{
		Guid ISlimServerService.AddTestJob(string testsystemName, byte[] data)
		{
			var jobGuid = Guid.NewGuid();
			string testFile = RegtestingServerConfiguration.Testsfolder + jobGuid + ".dll";;
			Directory.CreateDirectory(Path.GetDirectoryName(testFile));
			using (FileStream fileStream = new FileStream(testFile, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(data, 0, data.Length);
			}
            TestcaseProvider testcaseProvider = new TestcaseProvider(testFile);
			testcaseProvider.CreateAppDomain();


			List<WorkItem> items = new List<WorkItem>();
			foreach (string testcaseType in testcaseProvider.Types)
			{
				ITestable testable = testcaseProvider.GetTestableFromTypeName(testcaseType);
				if (testable == null) continue;
				WorkItem workItem = new WorkItem
				{
					Browser = new BrowserDto
					{
						Browserstring = "phantomjs",
						Name = "phantomjs"
					},
					Language = new LanguageDto
					{
						Name = "Deutsch",
						Languagecode = "DE"
					},
					Testcase = new TestcaseDto
					{
						Name = testable.GetName(),
						Type = testcaseType
					},
					Testsystem = new TestsystemDto
					{
						Filename = jobGuid + ".dll",
						Name = testsystemName,
						Url = testsystemName
					}
				};
				items.Add(workItem);
			}
			testcaseProvider.Unload();

			TestingJob testingJob = new TestingJob
			{
				Guid = jobGuid,
				ResultCode = TestState.Pending,
				WaitingWorkItems = items,
				TestFile = testFile,
				CurrentWorkItems = new Dictionary<string, WorkItemTask>(),
				FinishedWorkItems = new List<WorkItem>(),
				ResultGenerator = new ResultGenerator()
			};

			TestPool.AddTestingJob(testingJob);
			Console.WriteLine("Added Job "  + jobGuid + "(" + testcaseProvider.Types.Count() + " tests) to Testpool.");
			return jobGuid;
		}

		bool ISlimServerService.IsTestJobFinished(Guid testJobId)
		{
            return TestPool.GetStatus(testJobId);
		}

		string ISlimServerService.GetResultFile(Guid testJobId)
		{
			return TestPool.GetResultFile(testJobId);
		}
	}
}
