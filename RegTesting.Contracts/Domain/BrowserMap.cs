using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// Mapping for Browser
	/// </summary>
	public class BrowserMap : ClassMapping<Browser>
	{
		/// <summary>
		/// Create a mapping for Browser
		/// </summary>
		public BrowserMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Browserstring);
			Property(x => x.Name);
			Property(x => x.Versionsstring, m =>m.NotNullable(false));
			//Bag(p => p.BrowsersToTestsuite, map => map.Key(k => k.Column("BrowsersToTestsuite")), ce => ce.OneToMany());
			Table("Browsers");
		}
	}
}
