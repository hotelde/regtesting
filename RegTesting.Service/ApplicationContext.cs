using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;

namespace RegTesting.Service
{
	/// <summary>
	/// The ApplicationContext
	/// </summary>
	public class ApplicationContext
	{
		/// <summary>
		/// The NHibernateConfiguration
		/// </summary>
		public static Configuration NHConfiguration { get; set; }

		/// <summary>
		/// The SessionFactory
		/// </summary>
		public static ISessionFactory SessionFactory { get; set; }

		/// <summary>
		/// Configure NHibernate
		/// </summary>
		public static void AppConfigure()
		{
			NHConfiguration = ConfigureNHibernate();
			SessionFactory = NHConfiguration.BuildSessionFactory();
		}

		/// <summary>
		/// Configure NHibernate
		/// </summary>
		private static Configuration ConfigureNHibernate()
		{
			Configuration configure = new Configuration();
			configure.SessionFactoryName("SessionFactory");

			configure.DataBaseIntegration(db =>
				{
					db.Dialect<MsSql2008Dialect>();
					db.Driver<SqlClientDriver>();
					db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
					db.IsolationLevel = IsolationLevel.ReadCommitted;

					db.ConnectionStringName = RegtestingServerConfiguration.DefaultConnectionString;
					//db.Timeout = 10;

					//For testing
					//db.LogFormattedSql = true;
					//db.LogSqlInConsole = true;
					//db.AutoCommentSql = true;
				});

			HbmMapping objHbmMapping = GetMappings();
			configure.AddDeserializedMapping(objHbmMapping,"NHMapping");
			SchemaMetadataUpdater.QuoteTableAndColumns(configure);

			return configure;
		}

		/// <summary>
		/// Get Mappings for NHibernate
		/// </summary>
		/// <returns>The Mappings</returns>
		protected static HbmMapping GetMappings()
		{
			ModelMapper objModelMapper = new ModelMapper();

			objModelMapper.AddMapping<TestsuiteMap>();
			objModelMapper.AddMapping<TestsystemMap>();
			objModelMapper.AddMapping<ResultMap>();
			objModelMapper.AddMapping<BrowserMap>();
			objModelMapper.AddMapping<LanguageMap>();
			objModelMapper.AddMapping<TestcaseMap>();
			objModelMapper.AddMapping<ErrorMap>();
			objModelMapper.AddMapping<TesterMap>();
			objModelMapper.AddMapping<HistoryResultMap>();
			objModelMapper.AddMapping<TestJobMap>();

			HbmMapping objHbmMapping = objModelMapper.CompileMappingFor(new[] { typeof(Testsuite), typeof(Testsystem),
				typeof(Result), typeof(Browser), typeof(Language),  typeof(Testcase),  typeof(Error), typeof(Tester),
				typeof(TestJob),typeof(HistoryResult)});
			return objHbmMapping;
		}

	}
}