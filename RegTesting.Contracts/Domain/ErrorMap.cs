using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace RegTesting.Contracts.Domain
{
	/// <summary>
	/// Mapping for ErrorMap
	/// </summary>
	public class ErrorMap : ClassMapping<Error>
	{
		/// <summary>
		/// Create a Mapping for ErrorMap
		/// </summary>
		public ErrorMap()
		{
			Id(x => x.ID, map => map.Generator(Generators.Identity));
			Property(x => x.InnerException);
			Property(x => x.Message);
			Property(x => x.StackTrace);
			Property(x => x.TfsWorkItemID);
			Property(x => x.Type);
			Table("Errors");
		}
	}
}
