using RegTesting.Contracts;

namespace RegTesting.Service.Mail
{
	/// <summary>
	/// Interface for sending mails when testjobs are finished.
	/// </summary>
	public interface ITestJobFinishedMail
	{
		/// <summary>
		/// Send a mail when the testing job is done.
		/// </summary>
		/// <param name="testJobManager">A instance of a ITestJobManager.</param>
		void Send(ITestJobManager testJobManager);
	}
}
