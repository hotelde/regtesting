using System;
using System.Net.Mail;
using RegTesting.Service.Logging;

namespace RegTesting.Service.Mail
{

	/// <summary>
	/// A abstract mail.
	/// </summary>
	public abstract class AbstractMail
	{
		/// <summary>
		/// Send a mail.
		/// </summary>
		/// <param name="strReceiver">The receiver</param>
		/// <param name="strSubject">The subject</param>
		/// <param name="strBody">The Body</param>
		/// <param name="bolIsHmtl">is it html?</param>
		/// <param name="strFrom">The Sender, default is regtesting@hotel.de </param>
		protected void SendMail(string strReceiver, string strSubject, string strBody, bool bolIsHmtl = false, string strFrom = null)
		{

			if (String.IsNullOrEmpty(strReceiver)) return;

			if (String.IsNullOrEmpty(strFrom))
				strFrom = RegtestingServerConfiguration.SenderEmail;

			try
			{
				//create the mail message
				MailMessage objMailMessage = new MailMessage
				{
					From = new MailAddress(strFrom)
				};

				//set the addresses
				objMailMessage.To.Add(strReceiver);

				//set the content
				objMailMessage.Subject = strSubject;
				objMailMessage.Body = strBody;
				objMailMessage.IsBodyHtml = bolIsHmtl;

				//send the message
				SmtpClient objSmtpClient = new SmtpClient(RegtestingServerConfiguration.SmtpServer) { UseDefaultCredentials = true };
				objSmtpClient.Send(objMailMessage);
			}
			catch (Exception objException)
			{
				Logger.Log("ERROR Sending mail: " + objException);
			}

		}
	}
}
