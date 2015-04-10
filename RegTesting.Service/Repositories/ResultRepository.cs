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
	/// ResultRepository class
	/// </summary>
	public class ResultRepository : BaseRepository<Result>, IResultRepository
	{
		private readonly IErrorRepository _errorRepository;

		private readonly object _lock = new object();

		/// <summary>
		/// Create a new ResultRepository
		/// </summary>
		/// <param name="session">the session</param>
		/// <param name="errorRepository">the errorRepository</param>
		public ResultRepository(Func<ISession> session, IErrorRepository errorRepository)
			: base(session)
		{
			if (errorRepository == null)
				throw new ArgumentNullException("errorRepository");

			_errorRepository = errorRepository;
		}

		/// <summary>
		/// Get the Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="testsystemId">the Testsystem to get</param>
		/// <param name="browsers">the list of browsers</param>
		/// <param name="testcases">the list of testcases</param>
		/// <param name="languages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> IResultRepository.GetListOfResults(int testsystemId, IList<Browser> browsers,
			IList<Testcase> testcases, IList<Language> languages)
		{
			return GetResultListQuery(testsystemId, browsers, testcases, languages).ToList();
		}

		private IQueryable<Result> GetResultListQuery(int testsystemId, IList<Browser> browsers,
			IList<Testcase> testcases, IList<Language> languages)
		{
			return Session.Query<Result>()
				.Where(result => browsers.Contains(result.Browser))
				.Where(result => languages.Contains(result.Language))
				.Where(result => testcases.Contains(result.Testcase))
				.Where(result => result.Testsystem.ID == testsystemId)
				.OrderByDescending(result => result.Testtime);

		}

		/// <summary>
		/// Get the error Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="testsystemId">the Testsystem to get</param>
		/// <param name="browsers">the list of browsers</param>
		/// <param name="testcases">the list of testcases</param>
		/// <param name="languages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> IResultRepository.GetErrorResultsOfTestsuite(int testsystemId, IList<Browser> browsers,
			IList<Testcase> testcases, IList<Language> languages)
		{
			return
				GetResultListQuery(testsystemId, browsers, testcases, languages)
					.Where(
						r =>
							r.ResultCode == TestState.KnownError || r.ResultCode == TestState.Error || r.ResultCode == TestState.ErrorRepeat)
					.ToList();
		}


		/// <summary>
		/// Get a single Result
		/// </summary>
		/// <param name="testsystem">the testsystem</param>
		/// <param name="testcase">the testcase</param>
		/// <param name="browser">the browser</param>
		/// <param name="language">the language</param>
		/// <returns>a result</returns>
		Result IResultRepository.Get(Testsystem testsystem, Testcase testcase, Browser browser, Language language)
		{
			Result result =(
				from results in Session.Query<Result>()
				where results.Testsystem.ID == testsystem.ID &&
				      results.Testcase.ID == testcase.ID &&
				      results.Browser.ID == browser.ID &&
				      results.Language.ID == language.ID
				select results).FirstOrDefault();
			return result ?? new Result
			{
				Browser = browser,
				Language = language,
				Testsystem = testsystem,
				Testcase = testcase,
				ResultCode = TestState.NotSet
			};
		}


		void IResultRepository.Store(Result result)
		{
			if (result.Error != null) result.Error = _errorRepository.GetOrStore(result.Error);
			((IRepository<Result>)this).Store(result);
		}

	}
}