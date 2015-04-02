using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{

	/// <summary>
	/// Mapping for Result
	/// </summary>
	public class ResultMap  : ClassMapping<Result>
	{
		/// <summary>
		/// Create a mapping for Result
		/// </summary>
		public ResultMap()
		{

			Id(x => x.ID, map => map.Generator(Generators.Identity));

			ManyToOne(x => x.Browser, map =>  map.Column("Browser"));

			ManyToOne(x => x.Language, map => map.Column("Language"));

			ManyToOne(x => x.Testcase, map => map.Column("Testcase"));

			ManyToOne(x => x.Testsystem, map => map.Column("Testsystem"));

			ManyToOne(x => x.Tester, map => map.Column("Tester"));

			ManyToOne(x => x.Error, map => map.Column("Error"));

			ManyToOne(x => x.TestJob, map => map.Column("TestJob"));

			Property(x => x.DetailLog, x => x.Type(NHibernateUtil.StringClob));

			Property(x => x.Testtime);
			Property(x => x.ErrorSince);
			Property(x => x.ErrorCount);
			Property(x => x.ScreenshotFile);
			Property(x => x.Runtime);
			Property(x => x.ResultCode);

			Table("Results");
		}
	}
}
