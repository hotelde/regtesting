using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;

namespace RegTesting.SlimServer
{
	public class ResultGenerator
	{

		private readonly Guid _testRunID;
		private readonly Guid _testSettingsID;
		private readonly Guid _resultsNotInListId;
		private readonly Guid _allResultsId;
		private DateTime _startTime;

		private int _totalTests = 0;
		private int _passedTests = 0;
		private int _failedTests = 0;

		private readonly StringBuilder _results = new StringBuilder();
		private readonly StringBuilder _testDefinitions = new StringBuilder();
		private readonly StringBuilder _testEntries = new StringBuilder();


		public ResultGenerator()
		{
			_startTime = DateTime.Now;
			_testRunID = Guid.NewGuid();
			_testSettingsID = Guid.NewGuid();
			_resultsNotInListId = Guid.NewGuid();
			_allResultsId = Guid.NewGuid();
		}

		public void AddTestResult(string testName, DateTime testStartTime, string node, TestResult testResult)
		{
			bool success = testResult.TestState != TestState.Error;

			if (success)
			{
				_passedTests++;
			}
			else
			{
				_failedTests++;
			}
			_totalTests++;

			var exceutionId = Guid.NewGuid();
			var testEndTime = DateTime.Now;
			var testDuration = testEndTime.Subtract(testStartTime);
			var testTypeId = StringToGuid(testName);
			var testId = StringToGuid(testName + "test");
			string outcome = success ? "Passed" : "Failed";
			string log = string.Join(Environment.NewLine, testResult.Log);
			if (!string.IsNullOrEmpty(log))
			{
				log = log + Environment.NewLine + Environment.NewLine;
			}
			string errorOutput = success || testResult.Error == null ? "": 
				"<Output>"+
				"<ErrorInfo>"+
				  "<Message>" + HttpUtility.HtmlEncode(testResult.Error.Message) + "</Message>"+
				  "<StackTrace>"+ HttpUtility.HtmlEncode(log + testResult.Error.StackTrace)  + "</StackTrace>"+
				  "</ErrorInfo>"+ " </Output> ";

			_results.Append(
				"<UnitTestResult executionId = \"" + exceutionId + "\" testId = \"" + testId + "\" " +
				"testName = \"" + testName + "\" computerName=\"" + node + "\" duration=\"" + testDuration + "\"" +
				" startTime=\"" + testStartTime.ToString("s", CultureInfo.InvariantCulture) +  "\" endTime=\"" + testEndTime.ToString("s", CultureInfo.InvariantCulture) + "\"" +
				" testType=\"" + testTypeId + "\" outcome=\"" + outcome + "\" " +
				"testListId=\"" + _resultsNotInListId + "\">" + errorOutput +  "</UnitTestResult>"
			);

			_testDefinitions.Append(
				"<UnitTest name=\"" + testName + "\" id=\"" + testId + "\" >" +
				"<Execution id= \"" + exceutionId + "\" />" +
				"<TestMethod codeBase=\"" + "cb:" + testName + "\" name=\"DE Chrome " + testName + "\" className=\"" + testName +
				"\"/>" +
				"</UnitTest>"
			);

			_testEntries.Append(
				"<TestEntry testId=\""+ testId + "\" executionId=\""+ exceutionId +"\" testListId=\"" + _resultsNotInListId + "\" />"
			);
		}
		public string GetResultFile()
		{
			return GetStart() + GetResultsBlock() + GetTestDefinitionsBlock() + GetTestEntriesBlock() + GetEnd();
		}


		private string GetResultsBlock()
		{
			return "<Results>" + _results + "</Results>";
        }
		private string GetTestDefinitionsBlock()
		{
			return "<TestDefinitions>" + _testDefinitions + "</TestDefinitions>";
		}

		private string GetTestEntriesBlock()
		{
			return "<TestEntries>" + _testEntries + "</TestEntries>";
		}

		static Guid StringToGuid(string value)
		{
			// Create a new instance of the MD5CryptoServiceProvider object.
			MD5 md5Hasher = MD5.Create();
			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
			return new Guid(data);
		}

		private string GetStart()
		{
			return
				"<TestRun id=\"" + _testRunID + "\" name=\"TestRun " + _startTime.ToString("s", CultureInfo.InvariantCulture) +
				"\" runUser=\"HOTELDEOFFICE\tfs_access\" xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2010\">" +
				"<Times creation=\"" + _startTime.ToString("s", CultureInfo.InvariantCulture) + "\" queuing = \"" + _startTime.ToString("s", CultureInfo.InvariantCulture) +"\" start=\"" + _startTime.ToString("s", CultureInfo.InvariantCulture) + "\" finish=\"" + DateTime.Now.ToString("s", CultureInfo.InvariantCulture) + "\"/>" +
				"<TestSettings name=\"UiTests Run\" id=\"" + _testSettingsID +"\"/>" +
				"<TestLists >" +
				"<TestList name=\"Results Not in a List\" id =\"" + _resultsNotInListId +"\"/>" +
				"<TestList name=\"All Loaded Results\" id=\"" + _allResultsId + "\"/>" +
				"</TestLists>";

		}
		private string GetEnd()
		{
			return
				"<ResultSummary outcome=\"Completed\">"+
				"<Counters total=\""+ _totalTests + "\" executed=\""+ _totalTests + "\" passed=\"" + _passedTests + "\" failed=\"" + _failedTests +
				"\" error=\"0\" timeout=\"0\" aborted=\"0\" inconclusive=\"0\" passedButRunAborted=\"0\" notRunnable=\"0\" notExecuted=\"0\" " +
				"disconnected=\"0\" warning=\"0\" completed=\"0\" inProgress=\"0\" pending=\"0\" />" +
				"</ResultSummary>"+
				"</TestRun> ";
		}

		public int GetFailedTestsCount()
		{
			return _failedTests;
		}
	}
}
