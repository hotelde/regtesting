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
		private readonly IErrorRepository _objErrorRepository;

		/// <summary>
		/// Create a new HistoryResultRepository
		/// </summary>
		/// <param name="objSession">the session</param>
		/// <param name="objErrorRepository">the errorRepository</param>
		public HistoryResultRepository(Func<ISession> objSession, IErrorRepository objErrorRepository)
			: base(objSession)
		{
			if (objErrorRepository == null)
				throw new ArgumentNullException("objErrorRepository");
			_objErrorRepository = objErrorRepository;
		}

		IList<HistoryResult> IHistoryResultRepository.GetHistoryResultsOfTestJob(int intTestJob, int intTestsystem)
		{
			return Session.Query<HistoryResult>()
				   .Where(result => result.TestJob.ID == intTestJob)
				   .Where(result => result.Testsystem.ID == intTestsystem)
				   .ToList();
		}

		IList<HistoryResult> IHistoryResultRepository.GetListOfHistoryResults(int intTestsystem, IEnumerable<Browser> lstBrowsers, IEnumerable<Testcase> lstTestcases, IEnumerable<Language> lstLanguages, int intMaxResults)
		{
			return GetHistoryResultListQuery(intTestsystem, lstBrowsers, lstTestcases, lstLanguages)
				.Take(intMaxResults)
				   .ToList();
		}


		private IQueryable<HistoryResult> GetHistoryResultListQuery(int intTestsystem, IEnumerable<Browser> lstBrowsers, IEnumerable<Testcase> lstTestcases, IEnumerable<Language> lstLanguages)
		{
			return Session.Query<HistoryResult>()
				.Where(result => lstBrowsers.Contains(result.Browser))
				.Where(result => lstLanguages.Contains(result.Language))
				.Where(result => lstTestcases.Contains(result.Testcase))
				.Where(result => result.Testsystem.ID == intTestsystem)
				.OrderByDescending(result => result.Testtime);
		}

		void IHistoryResultRepository.Store(HistoryResult objResult)
		{
			if (objResult.Error != null) objResult.Error = _objErrorRepository.GetOrStore(objResult.Error);
			((IRepository<HistoryResult>)this).Store(objResult);
		}

		IList<HistoryResult> IHistoryResultRepository.GetListOfErrorHistoryResults(int intTestsystem, IList<Browser> lstBrowsers, IList<Testcase> lstTestcases, IList<Language> lstLanguages,
			DateTime datFromDateTime, DateTime datToDateTime)
		{

			return GetHistoryResultListQuery(intTestsystem, lstBrowsers, lstTestcases, lstLanguages)
				.Where(
						r =>
							r.ResultCode == TestState.KnownError || r.ResultCode == TestState.Error || r.ResultCode == TestState.ErrorRepeat)
				.Where(r => r.Testtime >= datFromDateTime && r.Testtime <= datToDateTime)
				.ToList();

		}
	}

}
