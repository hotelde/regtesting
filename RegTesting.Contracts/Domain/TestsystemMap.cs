using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// Classmapping for Testsystem
	/// </summary>
	public class TestsystemMap : ClassMapping<Testsystem>
	{
		/// <summary>
		/// Mapping of Testsystem
		/// </summary>
		public TestsystemMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Description);
			Property(x => x.Name);
			Property(x => x.Url);
			Property(x => x.LastUpdated);
			Bag(p => p.Testsuites, map => map.Key(k => k.Column("TestsystemFilter")), ce => ce.OneToMany());

			Table("Testsystems");
		}
	}
}
