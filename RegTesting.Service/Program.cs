using System;
using RegTesting.Contracts;
using RegTesting.Contracts.Services;
using RegTesting.Service.DependencyResolution;
using RegTesting.Service.Logging;
using RegTesting.Service.Mail;
using RegTesting.Service.TestLogic;
using RegTesting.Service.Tfs;
using RegTesting.Service.Wcf;
using StructureMap;

namespace RegTesting.Service
{
	/// <summary>
	/// The class for starting the ServiceHost for the testingService.
	/// </summary>
	public class Program
	{

		/// <summary>
		/// Mainfunction to start the Servicehost
		/// </summary>
		/// <param name="arrArgs">Currently we have no supported parameters</param>
		private static void Main(string[] arrArgs)
		{

			Logger.Log("Welcome to RegTesting Service V2!");

			//Init AutoMapper
			AutoMapperServerInit.CreateMappings();
			//Init NHibernate
			ApplicationContext.AppConfigure();
			//Init Structuremap
			IoC.Initialize();

			int port = RegtestingServerConfiguration.Port;

			Logger.Log("Loading Services on port " + port + ":");
			IHostService<ITestService> hostTestService = new HostWcfService<ITestService>(new Uri("http://localhost:" + port + "/TestService"));
			hostTestService.Init(ObjectFactory.GetInstance<ITestService>());

			IHostService<IStatusService> hostStatusService = new HostWcfService<IStatusService>(new Uri("http://localhost:" + port + "/StatusService"));
			hostStatusService.Init(ObjectFactory.GetInstance<IStatusService>());

			IHostService<IBuildTaskService> buildTaskService = new HostWcfService<IBuildTaskService>(new Uri("http://localhost:" + port + "/BuildTaskService"));
			buildTaskService.Init(ObjectFactory.GetInstance<IBuildTaskService>());

			IHostService<ILocalTestService> localTestService = new HostWcfService<ILocalTestService>(new Uri("http://localhost:" + port + "/LocalTestService"));
			localTestService.Init(ObjectFactory.GetInstance<ILocalTestService>());

			IHostService<ISummaryService> summaryService = new HostWcfService<ISummaryService>(new Uri("http://localhost:" + port + "/SummaryService"));
			summaryService.Init(ObjectFactory.GetInstance<ISummaryService>());

			IHostService<INodeService> nodeService = new HostWcfService<INodeService>(new Uri("http://localhost:" + port + "/NodeService"));
			nodeService.Init(ObjectFactory.GetInstance<INodeService>());

			new RecycleLonglifeWorkerBackgroundTask(ObjectFactory.GetInstance<ITestPool>());

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += UnhandledExceptionHandler;

			Logger.Log("Ready!");
			Console.ReadLine();
			
		}

		static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception objEx = (Exception)args.ExceptionObject;
			new UnhandledExceptionMail(objEx).Send();
		}

	}
}