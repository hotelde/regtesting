using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;

namespace RegTesting.Node
{
	class Program
	{

		public static void Main(string[] args)
		{
			if (args.Count()<2)
			{
				Console.Error.WriteLine("Wrong usage (missing arguments). Start with: NODENAME [BROWSER] [BROWSER]...");
				return;
			}
			string serviceAddress = NodeConfiguration.ServerAddress;
			string nodeName = args[0];

			Mapper.CreateMap<WorkItemDto, WorkItem>();
			Mapper.CreateMap<Browser, BrowserDto>().ReverseMap();
			Mapper.CreateMap<Language, LanguageDto>().ReverseMap();
			Mapper.CreateMap<Tester, TesterDto>().ReverseMap();
			Mapper.CreateMap<Testcase, TestcaseDto>().ReverseMap();
			Mapper.CreateMap<Testsystem, TestsystemDto>().ReverseMap();
			Mapper.CreateMap<Testsuite, TestsuiteDto>().ReverseMap();


			List<string> browsers = new List<string>();

			for (int i = 1; i < args.Length; i++)
			{
				browsers.Add(args[i]);
			}
			
			do
			{
				try
				{

					new NodeLogic(serviceAddress, nodeName, browsers).Run();
				}
				catch(Exception exception)
				{
					Console.WriteLine(exception.Message);
					Console.Out.WriteLine("Sleeping 10s.");
					Thread.Sleep(10000);
					Console.Out.WriteLine("Restarting...");
					//If there is a error in the node logic, start the node again.
				}
			} while (true);
			
		}
	}
}
