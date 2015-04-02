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
		/// <param name="objSession">the session</param>
		public LanguageRepository(Func<ISession> objSession)
			: base(objSession)
		{
		}


		Language ILanguageRepository.GetByLanguageCode(string strCode)
		{
			return Session
				.CreateCriteria(typeof(Language))
				.Add(Restrictions.Eq("Languagecode", strCode))
				.UniqueResult<Language>();
		}
	}
}
