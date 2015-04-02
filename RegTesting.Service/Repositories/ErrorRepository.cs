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
		/// <param name="objSession">the session</param>
		public ErrorRepository(Func<ISession> objSession) : base(objSession)
		{
		}

		Error IErrorRepository.GetOrStore(Error objError)
		{
			Error objExistingError = Session.Query<Error>()
				.FirstOrDefault(
					result => result.Message == objError.Message &&
					result.InnerException == objError.InnerException &&
					result.StackTrace == objError.StackTrace &&
					result.Type == objError.Type
				);
			if (objExistingError != null)
			{
				return objExistingError;
			}

			((IRepository<Error>) this).Store(objError);
			return objError;
		}

		Error IErrorRepository.GetByError(Error objError)
		{
			return Session.Query<Error>()
				.SingleOrDefault(
				result => result.Message == objError.Message &&
				result.InnerException == objError.InnerException &&
				result.StackTrace == objError.StackTrace &&
				result.Type == objError.Type
	);
		}
	}
}
