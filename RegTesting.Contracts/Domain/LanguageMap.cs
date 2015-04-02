using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// The Mapping for Language
	/// </summary>
	public class LanguageMap : ClassMapping<Language>
	{

		/// <summary>
		/// Create a mapping for Language
		/// </summary>
		public LanguageMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Languagecode);
			Property(x => x.Name);
			Table("Languages");
		}
	}
}
