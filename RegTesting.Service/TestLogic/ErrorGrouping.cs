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
		/// <param name="lstErrorResults">the error results</param>
		/// <returns>A List of ErrorOccurrenceGroups</returns>
		public static List<ErrorOccurrenceGroup> GetErrorOccurrenceGroups(IList<Result> lstErrorResults)
		{
			List<ErrorOccurrenceGroup> lstErrorOccurrenceGroup = new List<ErrorOccurrenceGroup>();

			foreach (Result objResult in lstErrorResults)
			{
				ErrorOccurrenceGroup objErrorOccurrenceGroup = lstErrorOccurrenceGroup.Find(objErrorOccurrenceGroupOther => objErrorOccurrenceGroupOther.Testcase.ID == objResult.Testcase.ID);
				if (objErrorOccurrenceGroup == null)
				{
					objErrorOccurrenceGroup = new ErrorOccurrenceGroup { Testcase = objResult.Testcase };
					lstErrorOccurrenceGroup.Add(objErrorOccurrenceGroup);
				}
				ErrorOccurrence objErrorOccurrence = objErrorOccurrenceGroup.LstErrorOccurence.Find(objErrorOccurrenceOther => objResult.Error != null && objErrorOccurrenceOther.Error.ID == objResult.Error.ID);
				if (objErrorOccurrence == null)
				{
					Debug.Assert(objResult.Error != null, "objResult.Error != null");
					objErrorOccurrence = new ErrorOccurrence { Error = objResult.Error };
					objErrorOccurrenceGroup.LstErrorOccurence.Add(objErrorOccurrence);
				}
				OccurrenceElement objOccurrenceElement = new OccurrenceElement
				{
					Browser = objResult.Browser,
					DateTime = objResult.Testtime,
					Language = objResult.Language,
					ScreenshotFile = objResult.ScreenshotFile,
					DetailLog = objResult.DetailLog,
					ErrorSince = objResult.ErrorSince,
					ErrorCount = objResult.ErrorCount


				};
				objErrorOccurrence.LstOccurence.Add(objOccurrenceElement);
			}
			return lstErrorOccurrenceGroup;
		}
	
		/// <summary>
		/// Get the GetHistoryErrorOccurrenceGroups for a testsystem
		/// </summary>
		/// <param name="lstErrorResults">the list of error results</param>
		/// <returns>A List of ErrorOccurrenceGroups</returns>
		public static List<ErrorOccurrenceGroup> GetHistoryErrorOccurrenceGroups(IList<HistoryResult> lstErrorResults)
		{
			List<ErrorOccurrenceGroup> lstErrorOccurrenceGroup = new List<ErrorOccurrenceGroup>();

			foreach (HistoryResult objResult in lstErrorResults)
			{
				ErrorOccurrenceGroup objErrorOccurrenceGroup = lstErrorOccurrenceGroup.Find(objErrorOccurrenceGroupOther => objErrorOccurrenceGroupOther.Testcase.ID == objResult.Testcase.ID);
				if (objErrorOccurrenceGroup == null)
				{
					objErrorOccurrenceGroup = new ErrorOccurrenceGroup { Testcase = objResult.Testcase };
					lstErrorOccurrenceGroup.Add(objErrorOccurrenceGroup);
				}
				ErrorOccurrence objErrorOccurrence = objErrorOccurrenceGroup.LstErrorOccurence.Find(objErrorOccurrenceOther => objResult.Error != null && objErrorOccurrenceOther.Error.ID == objResult.Error.ID);
				if (objErrorOccurrence == null)
				{
					Debug.Assert(objResult.Error != null, "objResult.Error != null");
					objErrorOccurrence = new ErrorOccurrence { Error = objResult.Error };
					objErrorOccurrenceGroup.LstErrorOccurence.Add(objErrorOccurrence);
				}
				OccurrenceElement objOccurrenceElement = new OccurrenceElement
				{
					Browser = objResult.Browser,
					DateTime = objResult.Testtime,
					Language = objResult.Language,
					ScreenshotFile = objResult.ScreenshotFile,
					DetailLog = objResult.DetailLog
				};
				objErrorOccurrence.LstOccurence.Add(objOccurrenceElement);
			}
			return lstErrorOccurrenceGroup;
		}
	}
}
