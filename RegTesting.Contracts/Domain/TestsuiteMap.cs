using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// The ClassMapping for a Testsuite
	/// </summary>
	public class TestsuiteMap : ClassMapping<Testsuite>
	{
		/// <summary>
		/// Mapping of a Testsuite
		/// </summary>
		public TestsuiteMap()
		  {
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Description);
			Property(x => x.Name);
			ManyToOne(x => x.TestsystemFilter, map =>
			{
				map.Column("TestsystemFilter");
				map.Class(typeof(Testsystem));
			});

			Bag(x => x.Testcases, collectionMapping =>
				{
					collectionMapping.Table("TestsuiteTestcase");
					collectionMapping.Cascade(Cascade.None);
					collectionMapping.Key(k => k.Column("Testsuite_ID"));
				},
				map => map.ManyToMany(p => p.Column("Testcase_ID"))
			);

			Bag(x => x.Languages, collectionMapping =>
			{
				collectionMapping.Table("TestsuiteLanguage");
				collectionMapping.Cascade(Cascade.None);
				collectionMapping.Key(k => k.Column("Testsuite_ID"));
			},
				map => map.ManyToMany(p => p.Column("Language_ID"))
			);

			Bag(x => x.Browsers, collectionMapping =>
			{
				collectionMapping.Table("TestsuiteBrowser");
				collectionMapping.Cascade(Cascade.None);
				collectionMapping.Key(k => k.Column("Testsuite_ID"));
			},
				map => map.ManyToMany(p => p.Column("Browser_ID"))
			);

			Table("Testsuites");

		  }

	}
}
