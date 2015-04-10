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
		/// <param name="receiver">The receiver</param>
		/// <param name="subject">The subject</param>
		/// <param name="body">The Body</param>
		/// <param name="isHmtl">is it html?</param>
		/// <param name="sender">The Sender, default is regtesting@hotel.de </param>
		protected void SendMail(string receiver, string subject, string body, bool isHmtl = false, string sender = null)
		{

			if (String.IsNullOrEmpty(receiver)) return;

			if (String.IsNullOrEmpty(sender))
				sender = RegtestingServerConfiguration.SenderEmail;

			try
			{
				//create the mail message
				MailMessage mailMessage = new MailMessage
				{
					From = new MailAddress(sender)
				};

				//set the addresses
				mailMessage.To.Add(receiver);

				//set the content
				mailMessage.Subject = subject;
				mailMessage.Body = body;
				mailMessage.IsBodyHtml = isHmtl;

				//send the message
				SmtpClient smtpClient = new SmtpClient(RegtestingServerConfiguration.SmtpServer) { UseDefaultCredentials = true };
				smtpClient.Send(mailMessage);
			}
			catch (Exception exception)
			{
				Logger.Log("ERROR Sending mail: " + exception);
			}

		}
	}
}
