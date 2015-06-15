using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{

	/// <summary>
	/// The mapping for a TestJob
	/// </summary>
	public class TestJobMap : ClassMapping<TestJob>
	{
		/// <summary>
		/// Create a mapping for Tester
		/// </summary>
		public TestJobMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Name);
			Property(x => x.Description);
			Property(x => x.ResultCode);
			Property(x => x.StartedAt);
			Property(x => x.FinishedAt);

			ManyToOne(x => x.Testsuite, map => map.Column("Testsuite"));
			ManyToOne(x => x.Testsystem, map => map.Column("Testsystem"));

			Table("TestJobs");
		}
	}
}
