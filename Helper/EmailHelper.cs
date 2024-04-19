using DataAccess.POCO;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace GreatEastForex.Helper
{
    public class EmailHelper
    {
		//
		// Helper: Send Email

		public static bool sendEmailDailyBackup(string subject, string body, ListDictionary replacements, string recipient)
		{
			try
			{
				MailDefinition md = new MailDefinition();
				md.From = ConfigurationManager.AppSettings["SystemEmailAddress"].ToString();
				md.IsBodyHtml = true;
				md.Subject = subject;

				MailMessage mm = md.CreateMailMessage(recipient, replacements, body, new System.Web.UI.Control());
				mm.From = new MailAddress(ConfigurationManager.AppSettings["SystemEmailAddress"].ToString(), ConfigurationManager.AppSettings["EmailDisplayName"].ToString());

				SmtpClient smtp = new SmtpClient();
				//smtp.Send(mm);

				return true;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public static bool sendEmail(string subject, string body, ListDictionary replacements, string recipient, int userTriggeringId, string emailType, int? saleId = null)
        {
			EmailLog emailLog = new EmailLog()
			{
				UserTriggeringId = userTriggeringId,
				EmailType = emailType,
				SaleId = saleId,
				ReceiverEmail = recipient,
				CcEmail = null,
				BccEmail = null,
				Subject = subject,
				Attachments = null,
				Timestamp = DateTime.Now
			};

			try
            {
				MailDefinition md = new MailDefinition();
				md.From = ConfigurationManager.AppSettings["SystemEmailAddress"].ToString();
				md.IsBodyHtml = true;
				md.Subject = subject;

				MailMessage mm = md.CreateMailMessage(recipient, replacements, body, new System.Web.UI.Control());
				
				mm.From = new MailAddress(ConfigurationManager.AppSettings["SystemEmailAddress"].ToString(), ConfigurationManager.AppSettings["EmailDisplayName"].ToString());
				mm.ReplyToList.Add(ConfigurationManager.AppSettings["EmailReplyTo"].ToString());

				emailLog.EmailContent = mm.Body;

				SmtpClient smtp = new SmtpClient();
				//smtp.Send(mm);

				emailLog.Status = "Success";
				emailLog.Remarks = "Email has been successfully sent to " + recipient;

				EmailLogRepository _emailLogsModel = new EmailLogRepository();
				bool result = _emailLogsModel.Add(emailLog);

				return true;
            }
            catch (Exception e)
            {
				emailLog.Status = "Fail";
				emailLog.Remarks = "An error occured while sending email. " + e.Message;

				EmailLogRepository _emailLogsModel = new EmailLogRepository();
				bool result = _emailLogsModel.Add(emailLog);

				throw;
            }
        }

        public static bool sendEmail(string subject, string body, ListDictionary replacements, string recipient, string cc, string bcc, byte[] attachment, string fileName, int userTriggeringId, string emailType, int? saleId = null)
        {

			EmailLog emailLog = new EmailLog()
			{
				UserTriggeringId = userTriggeringId,
				EmailType = emailType,
				SaleId = saleId,
				ReceiverEmail = recipient,
				CcEmail = cc,
				BccEmail = bcc,
				Subject = subject,
				Attachments = fileName,
				Timestamp = DateTime.Now
			};

			try
            {
				MailDefinition md = new MailDefinition();
				md.From = ConfigurationManager.AppSettings["SystemEmailAddress"].ToString();
				md.IsBodyHtml = true;
				md.Subject = subject;

				MailMessage mm = md.CreateMailMessage(recipient, replacements, body, new System.Web.UI.Control());
				mm.From = new MailAddress(ConfigurationManager.AppSettings["SystemEmailAddress"].ToString(), ConfigurationManager.AppSettings["EmailDisplayName"].ToString());
				if (!string.IsNullOrEmpty(cc))
				{
					mm.CC.Add(cc);
				}
				if (!string.IsNullOrEmpty(bcc))
				{
					mm.Bcc.Add(bcc);
				}
				mm.ReplyToList.Add(ConfigurationManager.AppSettings["EmailReplyTo"].ToString());

				MemoryStream stream = new MemoryStream(attachment);
				Attachment attc = new Attachment(stream, fileName);
				mm.Attachments.Add(attc);

				emailLog.EmailContent = mm.Body;

				SmtpClient smtp = new SmtpClient();
				//smtp.Send(mm);

				emailLog.Status = "Success";
				emailLog.Remarks = "Email has been successfully sent to " + recipient;

				EmailLogRepository _emailLogsModel = new EmailLogRepository();
				bool result = _emailLogsModel.Add(emailLog);

				return true;
            }
            catch (Exception e)
			{
				emailLog.Status = "Fail";
				emailLog.Remarks = "An error occured while sending email. " + e.Message;

				EmailLogRepository _emailLogsModel = new EmailLogRepository();
				bool result = _emailLogsModel.Add(emailLog);

				throw;
            }
        }

        //Test Send Multiple To Email
        //public static bool sendEmail(string subject, string body, ListDictionary replacements, string recipient)
        //{

        //    try
        //    {
        //        MailDefinition md = new MailDefinition();
        //        md.From = ConfigurationManager.AppSettings["SystemEmailAddress"].ToString();
        //        md.IsBodyHtml = true;
        //        md.Subject = subject;

        //        MailMessage mm = new MailMessage();
        //        MailAddress fromMail = new MailAddress(md.From);
        //        mm.From = fromMail;
        //        mm.Subject = subject;

        //        foreach (var address in recipient.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            mm.To.Add(address);
        //        }

        //        mm = md.CreateMailMessage(recipient, replacements, body, new System.Web.UI.Control());


        //        mm.From = new MailAddress(ConfigurationManager.AppSettings["SystemEmailAddress"].ToString(), ConfigurationManager.AppSettings["EmailDisplayName"].ToString());

        //        //mm.ReplyToList.Add(ConfigurationManager.AppSettings["EmailReplyTo"].ToString());

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Send(mm);

        //        return true;
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}
    }
}