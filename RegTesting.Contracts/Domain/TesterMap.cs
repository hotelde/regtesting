using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// Mapping for Tester
	/// </summary>
	public class TesterMap : ClassMapping<Tester>
	{
		/// <summary>
		/// Create a mapping for Tester
		/// </summary>
		public TesterMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.Mail);
			Property(x => x.Name);
			Table("Testers");
		}
	}
}
