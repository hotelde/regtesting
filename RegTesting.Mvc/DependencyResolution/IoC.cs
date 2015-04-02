// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using NHibernate;
using RegTesting.Contracts;
using RegTesting.Contracts.Repositories;
using RegTesting.Contracts.Services;
using RegTesting.Mvc.WcfServices;
using RegTesting.Service;
using RegTesting.Service.Mail;
using RegTesting.Service.Repositories;
using RegTesting.Service.Services;
using RegTesting.Service.TestLogic;
using StructureMap;
namespace RegTesting.Mvc.DependencyResolution {

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
							x.For<ITestService>().HybridHttpOrThreadLocalScoped().Use<TestServiceWcfProxy>();
							x.For<IStatusService>().HybridHttpOrThreadLocalScoped().Use<StatusServiceWcfProxy>();
							x.For<ISummaryService>().HybridHttpOrThreadLocalScoped().Use<SummaryServiceWcfProxy>();
							x.For<ITestViewerService>().Use<TestViewerService>();
							x.For<ISettingsService>().Use<SettingsService>();

							#endregion
							

							#region TestPool
							x.For<ITestPool>().Singleton().Use(ObjectFactory.GetInstance<TestPool>);
							x.For<ITestJobFinishedMail>().Use<TestJobFinishedMail>();
							#endregion
		
						});





			return ObjectFactory.Container;
		}
	}
}