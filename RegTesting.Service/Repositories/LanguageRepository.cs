using System;
using NHibernate;
using NHibernate.Criterion;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// TestsuiteRepository
	/// </summary>
	public class LanguageRepository : BaseRepository<Language>, ILanguageRepository
	{

		/// <summary>
		/// Create a TestsuiteRepository
		/// </summary>
		/// <param name="session">the session</param>
		public LanguageRepository(Func<ISession> session)
			: base(session)
		{
		}


		Language ILanguageRepository.GetByLanguageCode(string languageCode)
		{
			return Session
				.CreateCriteria(typeof(Language))
				.Add(Restrictions.Eq("Languagecode", languageCode))
				.UniqueResult<Language>();
		}
	}
}
