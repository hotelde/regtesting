using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// Mapping for Testcase
	/// </summary>
	public class TestcaseMap : ClassMapping<Testcase>
	{
		/// <summary>
		/// Create a mapping for Testcase
		/// </summary>
		public TestcaseMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Activated);
			Property(x => x.Name);
			Property(x => x.Type);
			Property(x => x.Description, x => x.NotNullable(false));

			Table("Testcases");
		}
	}
}
