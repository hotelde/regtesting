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
		private readonly IErrorRepository _objErrorRepository;

		private readonly object _objLock = new object();

		/// <summary>
		/// Create a new ResultRepository
		/// </summary>
		/// <param name="objSession">the session</param>
		/// <param name="objErrorRepository">the errorRepository</param>
		public ResultRepository(Func<ISession> objSession, IErrorRepository objErrorRepository)
			: base(objSession)
		{
			if (objErrorRepository == null)
				throw new ArgumentNullException("objErrorRepository");

			_objErrorRepository = objErrorRepository;
		}

		/// <summary>
		/// Get the Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="intTestsystem">the Testsystem to get</param>
		/// <param name="lstBrowser">the list of browsers</param>
		/// <param name="lstTestcase">the list of testcases</param>
		/// <param name="lstLanguages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> IResultRepository.GetListOfResults(int intTestsystem, IList<Browser> lstBrowser,
			IList<Testcase> lstTestcase, IList<Language> lstLanguages)
		{
			return GetResultListQuery(intTestsystem, lstBrowser, lstTestcase, lstLanguages).ToList();
		}

		private IQueryable<Result> GetResultListQuery(int intTestsystem, IList<Browser> lstBrowser,
			IList<Testcase> lstTestcase, IList<Language> lstLanguages)
		{
			return Session.Query<Result>()
				.Where(result => lstBrowser.Contains(result.Browser))
				.Where(result => lstLanguages.Contains(result.Language))
				.Where(result => lstTestcase.Contains(result.Testcase))
				.Where(result => result.Testsystem.ID == intTestsystem)
				.OrderByDescending(result => result.Testtime);

		}

		/// <summary>
		/// Get the error Results filtered by testsystem, browsers, testcases and languages
		/// </summary>
		/// <param name="intTestsystem">the Testsystem to get</param>
		/// <param name="lstBrowser">the list of browsers</param>
		/// <param name="lstTestcase">the list of testcases</param>
		/// <param name="lstLanguages">the list of languages</param>
		/// <returns>A list of results</returns>
		IList<Result> IResultRepository.GetErrorResultsOfTestsuite(int intTestsystem, IList<Browser> lstBrowser,
			IList<Testcase> lstTestcase, IList<Language> lstLanguages)
		{
			return
				GetResultListQuery(intTestsystem, lstBrowser, lstTestcase, lstLanguages)
					.Where(
						r =>
							r.ResultCode == TestState.KnownError || r.ResultCode == TestState.Error || r.ResultCode == TestState.ErrorRepeat)
					.ToList();
		}


		/// <summary>
		/// Get a single Result
		/// </summary>
		/// <param name="objTestsystem">the testsystem</param>
		/// <param name="objTestcase">the testcase</param>
		/// <param name="objBrowser">the browser</param>
		/// <param name="objLanguage">the language</param>
		/// <returns>a result</returns>
		Result IResultRepository.Get(Testsystem objTestsystem, Testcase objTestcase, Browser objBrowser, Language objLanguage)
		{
			Result objResult =(
				from results in Session.Query<Result>()
				where results.Testsystem.ID == objTestsystem.ID &&
				      results.Testcase.ID == objTestcase.ID &&
				      results.Browser.ID == objBrowser.ID &&
				      results.Language.ID == objLanguage.ID
				select results).FirstOrDefault();
			return objResult ?? new Result
			{
				Browser = objBrowser,
				Language = objLanguage,
				Testsystem = objTestsystem,
				Testcase = objTestcase,
				ResultCode = TestState.NotSet
			};
		}


		void IResultRepository.Store(Result objResult)
		{
			if (objResult.Error != null) objResult.Error = _objErrorRepository.GetOrStore(objResult.Error);
			((IRepository<Result>)this).Store(objResult);
		}

	}
}