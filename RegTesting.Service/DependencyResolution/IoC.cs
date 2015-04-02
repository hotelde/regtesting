using NHibernate;
using RegTesting.Contracts;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Service.Mail;
using RegTesting.Service.Repositories;
using RegTesting.Service.Services;
using RegTesting.Service.TestLogic;
using StructureMap;

namespace RegTesting.Service.DependencyResolution {

	/// <summary>
	/// IoC class
	/// </summary>
    public static class IoC {
		/// <summary>
		/// Init StructureMap
		/// </summary>
		/// <returns>An IContainer</returns>
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {

							x.For<NHibernate.Cfg.Configuration>().Use(ApplicationContext.NHConfiguration);

							#region Session
							x.For<ISession>().HybridHttpOrThreadLocalScoped().Use(() => ObjectFactory.GetInstance<ISessionFactory>().OpenSession());
							x.For<ISessionFactory>().Singleton().Use(() => ApplicationContext.SessionFactory);
							#endregion

							#region Concrete Repository
							x.For<IBrowserRepository>().Singleton().Use(ObjectFactory.GetInstance<BrowserRepository>);
							x.For<ILanguageRepository>().Singleton().Use(ObjectFactory.GetInstance<LanguageRepository>);

							x.For<IHistoryResultRepository>().Singleton().Use(ObjectFactory.GetInstance<HistoryResultRepository>);

							x.For<IResultRepository>().Singleton().Use(ObjectFactory.GetInstance<ResultRepository>);

							x.For<IErrorRepository>().Singleton().Use(ObjectFactory.GetInstance<ErrorRepository>);

							x.For<ITestsystemRepository>().Singleton().Use(ObjectFactory.GetInstance<TestsystemRepository>);

							x.For<ITestsuiteRepository>().Singleton().Use(ObjectFactory.GetInstance<TestsuiteRepository>);

							x.For<ITestJobRepository>().Singleton().Use(ObjectFactory.GetInstance<TestJobRepository>);

							x.For<ITesterRepository>().Singleton().Use(ObjectFactory.GetInstance<TesterRepository>);

							x.For<ITestcaseRepository>().Singleton().Use(ObjectFactory.GetInstance<TestcaseRepository>);
							#endregion


							#region Services
							x.For<ITestService>().Use<TestService>();
							x.For<ITestViewerService>().Use<TestViewerService>();
							x.For<IStatusService>().Use<StatusService>();
							x.For<ISettingsService>().Use<SettingsService>();
							x.For<IBuildTaskService>().Use<BuildTaskService>();
							x.For<ILocalTestService>().Use<LocalTestService>();
							x.For<ISummaryService>().Use<SummaryService>();
							x.For<INodeService>().Use<NodeService>();
							#endregion
							

							#region TestPool
							x.For<ITestPool>().Singleton().Use(ObjectFactory.GetInstance<TestPool>);
	                        x.For<ITestJobFinishedMail>().Use<TestJobFinishedMail>();
							#endregion

							x.For<ITestFileLocker>().Singleton().Use(ObjectFactory.GetInstance<TestFileLocker>);
		
                        });





            return ObjectFactory.Container;
        }
    }
}