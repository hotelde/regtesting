using System.Collections.Generic;
using System.Diagnostics;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;

namespace RegTesting.Service.TestLogic
{
	/// <summary>
	/// A class for grouping error results by testcase and errors
	/// </summary>
	public class ErrorGrouping
	{

		/// <summary>
		/// Get the GetErrorOccurrenceGroups for a testsystem
		/// </summary>
		/// <param name="errorResults">the error results</param>
		/// <returns>A List of ErrorOccurrenceGroups</returns>
		public static List<ErrorOccurrenceGroup> GetErrorOccurrenceGroups(IList<Result> errorResults)
		{
			List<ErrorOccurrenceGroup> errorOccurrenceGroup = new List<ErrorOccurrenceGroup>();

			foreach (Result result in errorResults)
			{
				ErrorOccurrenceGroup occurrenceGroup = errorOccurrenceGroup.Find(objErrorOccurrenceGroupOther => objErrorOccurrenceGroupOther.Testcase.ID == result.Testcase.ID);
				if (occurrenceGroup == null)
				{
					occurrenceGroup = new ErrorOccurrenceGroup { Testcase = result.Testcase };
					errorOccurrenceGroup.Add(occurrenceGroup);
				}
				ErrorOccurrence errorOccurrence = occurrenceGroup.LstErrorOccurence.Find(objErrorOccurrenceOther => result.Error != null && objErrorOccurrenceOther.Error.ID == result.Error.ID);
				if (errorOccurrence == null)
				{
					Debug.Assert(result.Error != null, "objResult.Error != null");
					errorOccurrence = new ErrorOccurrence { Error = result.Error };
					occurrenceGroup.LstErrorOccurence.Add(errorOccurrence);
				}
				OccurrenceElement occurrenceElement = new OccurrenceElement
				{
					Browser = result.Browser,
					DateTime = result.Testtime,
					Language = result.Language,
					ScreenshotFile = result.ScreenshotFile,
					DetailLog = result.DetailLog,
					ErrorSince = result.ErrorSince,
					ErrorCount = result.ErrorCount


				};
				errorOccurrence.LstOccurence.Add(occurrenceElement);
			}
			return errorOccurrenceGroup;
		}
	
		/// <summary>
		/// Get the GetHistoryErrorOccurrenceGroups for a testsystem
		/// </summary>
		/// <param name="errorResults">the list of error results</param>
		/// <returns>A List of ErrorOccurrenceGroups</returns>
		public static List<ErrorOccurrenceGroup> GetHistoryErrorOccurrenceGroups(IList<HistoryResult> errorResults)
		{
			List<ErrorOccurrenceGroup> occurrenceGroups = new List<ErrorOccurrenceGroup>();

			foreach (HistoryResult result in errorResults)
			{
				ErrorOccurrenceGroup errorOccurrenceGroup = occurrenceGroups.Find(objErrorOccurrenceGroupOther => objErrorOccurrenceGroupOther.Testcase.ID == result.Testcase.ID);
				if (errorOccurrenceGroup == null)
				{
					errorOccurrenceGroup = new ErrorOccurrenceGroup { Testcase = result.Testcase };
					occurrenceGroups.Add(errorOccurrenceGroup);
				}
				ErrorOccurrence errorOccurrence = errorOccurrenceGroup.LstErrorOccurence.Find(errorOccurrenceOther => result.Error != null && errorOccurrenceOther.Error.ID == result.Error.ID);
				if (errorOccurrence == null)
				{
					Debug.Assert(result.Error != null, "objResult.Error != null");
					errorOccurrence = new ErrorOccurrence { Error = result.Error };
					errorOccurrenceGroup.LstErrorOccurence.Add(errorOccurrence);
				}
				OccurrenceElement occurrenceElement = new OccurrenceElement
				{
					Browser = result.Browser,
					DateTime = result.Testtime,
					Language = result.Language,
					ScreenshotFile = result.ScreenshotFile,
					DetailLog = result.DetailLog
				};
				errorOccurrence.LstOccurence.Add(occurrenceElement);
			}
			return occurrenceGroups;
		}
	}
}
