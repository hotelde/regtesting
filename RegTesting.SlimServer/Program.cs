using System;
using RegTesting.Contracts.Services;
using RegTesting.SlimServer.Logger;
using RegTesting.SlimServer.Wcf;
using StructureMap;

namespace RegTesting.SlimServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.AddEntry("Welcome to RegTesting Service V2!");

			//Init AutoMapper
			//AutoMapperServerInit.CreateMappings();

			//Init Structuremap
			IoC.Initialize();

			int port = RegtestingServerConfiguration.Port;

			Log.AddEntry("Loading Services on port " + port + ":");
			IHostService<ISlimServerService> hostTestService = new HostWcfService<ISlimServerService>(new Uri("http://localhost:" + port + "/SlimServerService"));
			hostTestService.Init(ObjectFactory.GetInstance<ISlimServerService>());

			IHostService<INodeService> nodeService = new HostWcfService<INodeService>(new Uri("http://localhost:" + port + "/NodeService"));
			nodeService.Init(ObjectFactory.GetInstance<INodeService>());

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += UnhandledExceptionHandler;

			Log.AddEntry("Ready!");
			Console.ReadLine();
		}


		static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception objEx = (Exception)args.ExceptionObject;
			Console.Error.Write(objEx.ToString());
		}
	}
}
