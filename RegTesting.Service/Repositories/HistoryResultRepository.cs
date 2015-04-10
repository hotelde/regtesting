using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.Repositories;

namespace RegTesting.Service.Repositories
{
	/// <summary>
	/// The HistoryResultRepository
	/// </summary>
	public class HistoryResultRepository : BaseRepository<HistoryResult>, IHistoryResultRepository
	{
		private readonly IErrorRepository _errorRepository;

		/// <summary>
		/// Create a new HistoryResultRepository
		/// </summary>
		/// <param name="session">the session</param>
		/// <param name="errorRepository">the errorRepository</param>
		public HistoryResultRepository(Func<ISession> session, IErrorRepository errorRepository)
			: base(session)
		{
			if (errorRepository == null)
				throw new ArgumentNullException("errorRepository");
			_errorRepository = errorRepository;
		}

		IList<HistoryResult> IHistoryResultRepository.GetHistoryResultsOfTestJob(int testJobId, int testsystemId)
		{
			return Session.Query<HistoryResult>()
				   .Where(result => result.TestJob.ID == testJobId)
				   .Where(result => result.Testsystem.ID == testsystemId)
				   .ToList();
		}

		IList<HistoryResult> IHistoryResultRepository.GetListOfHistoryResults(int testsystemId, IEnumerable<Browser> browsers, IEnumerable<Testcase> testcases, IEnumerable<Language> languages, int maxResults)
		{
			return GetHistoryResultListQuery(testsystemId, browsers, testcases, languages)
				.Take(maxResults)
				   .ToList();
		}


		private IQueryable<HistoryResult> GetHistoryResultListQuery(int testsystemId, IEnumerable<Browser> browsers, IEnumerable<Testcase> testcases, IEnumerable<Language> languages)
		{
			return Session.Query<HistoryResult>()
				.Where(result => browsers.Contains(result.Browser))
				.Where(result => languages.Contains(result.Language))
				.Where(result => testcases.Contains(result.Testcase))
				.Where(result => result.Testsystem.ID == testsystemId)
				.OrderByDescending(result => result.Testtime);
		}

		void IHistoryResultRepository.Store(HistoryResult historyResult)
		{
			if (historyResult.Error != null) historyResult.Error = _errorRepository.GetOrStore(historyResult.Error);
			((IRepository<HistoryResult>)this).Store(historyResult);
		}

		IList<HistoryResult> IHistoryResultRepository.GetListOfErrorHistoryResults(int testsystemId, IList<Browser> browsers, IList<Testcase> testcases, IList<Language> languages,
			DateTime dateFrom, DateTime dateTo)
		{

			return GetHistoryResultListQuery(testsystemId, browsers, testcases, languages)
				.Where(
						r =>
							r.ResultCode == TestState.KnownError || r.ResultCode == TestState.Error || r.ResultCode == TestState.ErrorRepeat)
				.Where(r => r.Testtime >= dateFrom && r.Testtime <= dateTo)
				.ToList();

		}
	}

}
