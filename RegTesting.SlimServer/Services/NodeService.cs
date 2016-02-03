using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Services;

namespace RegTesting.SlimServer.Services
{

	/// <summary>
	/// The nodeService
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class NodeService : INodeService
	{

		void INodeService.Register(string node, List<string> browsers)
		{
			Console.WriteLine("Node connected: " + node);
		}

		WorkItem INodeService.GetWork(string nodeName)
		{
			WorkItem workItem = TestPool.GetWork(nodeName);
			return workItem;
		}

		void INodeService.FinishedWork(string nodeName, TestResult testResult)
		{
			TestPool.FinishedWorkItem(nodeName, testResult);
		}

		byte[] INodeService.FetchDLL(string nodeName, string branchName)
		{
            using (FileStream fileStream = new FileStream(TestPool.GetCurrenTestingJob().TestFile, FileMode.Open))
			{
				byte[] buffer = new byte[52428800];
				int size = fileStream.Read(buffer, 0, 52428800);
				byte[] bufferShort = buffer.Take(size).ToArray();
				return bufferShort;
			}
		}

	}
}
