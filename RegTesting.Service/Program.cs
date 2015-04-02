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
			IHostService<ITestService> objHostTestService = new HostWcfService<ITestService>(new Uri("http://localhost:" + port + "/TestService"));
			objHostTestService.Init(ObjectFactory.GetInstance<ITestService>());

			IHostService<IStatusService> objHostStatusService = new HostWcfService<IStatusService>(new Uri("http://localhost:" + port + "/StatusService"));
			objHostStatusService.Init(ObjectFactory.GetInstance<IStatusService>());

			IHostService<IBuildTaskService> objBuildTaskService = new HostWcfService<IBuildTaskService>(new Uri("http://localhost:" + port + "/BuildTaskService"));
			objBuildTaskService.Init(ObjectFactory.GetInstance<IBuildTaskService>());

			IHostService<ILocalTestService> objLocalTestService = new HostWcfService<ILocalTestService>(new Uri("http://localhost:" + port + "/LocalTestService"));
			objLocalTestService.Init(ObjectFactory.GetInstance<ILocalTestService>());

			IHostService<ISummaryService> objSummaryService = new HostWcfService<ISummaryService>(new Uri("http://localhost:" + port + "/SummaryService"));
			objSummaryService.Init(ObjectFactory.GetInstance<ISummaryService>());

			IHostService<INodeService> objNodeService = new HostWcfService<INodeService>(new Uri("http://localhost:" + port + "/NodeService"));
			objNodeService.Init(ObjectFactory.GetInstance<INodeService>());

			new RecycleLonglifeWorkerBackgroundTask(ObjectFactory.GetInstance<ITestPool>());

			AppDomain objCurrentDomain = AppDomain.CurrentDomain;
			objCurrentDomain.UnhandledException += UnhandledExceptionHandler;

			Logger.Log("Ready!");
			Console.ReadLine();
		}

		static void UnhandledExceptionHandler(object objSender, UnhandledExceptionEventArgs objArgs)
		{
			Exception objEx = (Exception)objArgs.ExceptionObject;
			new UnhandledExceptionMail(objEx).Send();
		}

	}
}