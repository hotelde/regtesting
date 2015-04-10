using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// the errorRepository
	/// </summary>
	public class ErrorRepository : BaseRepository<Error>, IErrorRepository
	{
		/// <summary>
		/// Create a new errorRepository
		/// </summary>
		/// <param name="session">the session</param>
		public ErrorRepository(Func<ISession> session) : base(session)
		{
		}

		Error IErrorRepository.GetOrStore(Error error)
		{
			Error existingError = Session.Query<Error>()
				.FirstOrDefault(
					result => result.Message == error.Message &&
					result.InnerException == error.InnerException &&
					result.StackTrace == error.StackTrace &&
					result.Type == error.Type
				);
			if (existingError != null)
			{
				return existingError;
			}

			((IRepository<Error>) this).Store(error);
			return error;
		}

		Error IErrorRepository.GetByError(Error error)
		{
			return Session.Query<Error>()
				.SingleOrDefault(
				result => result.Message == error.Message &&
				result.InnerException == error.InnerException &&
				result.StackTrace == error.StackTrace &&
				result.Type == error.Type
	);
		}
	}
}
