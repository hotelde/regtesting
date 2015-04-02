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

		public static void Main(string[] arrArgs)
		{
			if (arrArgs.Count()<2)
			{
				Console.Error.WriteLine("Wrong usage (missing arguments). Start with: NODENAME [BROWSER] [BROWSER]...");
				return;
			}
			string strServiceAddress = NodeConfiguration.ServerAddress;
			string strNodeName = arrArgs[0];

			Mapper.CreateMap<WorkItemDto, WorkItem>();
			Mapper.CreateMap<Browser, BrowserDto>().ReverseMap();
			Mapper.CreateMap<Language, LanguageDto>().ReverseMap();
			Mapper.CreateMap<Tester, TesterDto>().ReverseMap();
			Mapper.CreateMap<Testcase, TestcaseDto>().ReverseMap();
			Mapper.CreateMap<Testsystem, TestsystemDto>().ReverseMap();
			Mapper.CreateMap<Testsuite, TestsuiteDto>().ReverseMap();


			List<string> lstBrowsers = new List<string>();

			for (int intIndex = 1; intIndex < arrArgs.Length; intIndex++)
			{
				lstBrowsers.Add(arrArgs[intIndex]);
			}
			
			do
			{
				try
				{

					new NodeLogic(strServiceAddress, strNodeName, lstBrowsers).Run();
				}
				catch(Exception objException)
				{
					Console.WriteLine(objException.Message);
					Console.Out.WriteLine("Sleeping 10s.");
					Thread.Sleep(10000);
					Console.Out.WriteLine("Restarting...");
					//If there is a error in the node logic, start the node again.
				}
			} while (true);
			
		}
	}
}
