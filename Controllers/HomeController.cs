using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace GreatEastForex.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

		public string Test()
		{
			string sourceFile = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + "1.jpg";
			string destinationFile = System.IO.Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), "1.jpg");

			if (System.IO.File.Exists(sourceFile))
			{
				//This is when Shared folder have the file, then move the file to local folder.
				System.IO.File.Move(sourceFile, destinationFile);
				return "true";
			}
			else
			{
				//This is when Shared folder does not have the file, then do your own logic.
				return "false";
			}
		}

        //public void testSMTP()
        //{
        //    string smtpHostType = "";
        //    string smtpHost = "mail205.livehostsupport.com";
        //    int smtpPort = 587;
        //    string useremail = "customerservice@greateastforex.com";
        //    string password = "p@ssw0rd188";
        //    string to = "kianchi@thedottsolutions.com";
        //    string from = ConfigurationManager.AppSettings["SystemEmailAddress"].ToString();

        //    using (MailMessage mm = new MailMessage(from, to))
        //    {
        //        mm.Subject = "Test Point SMTP";
        //        mm.Body = "Successful!";
        //        //if (model.Attachment.ContentLength > 0)
        //        //{
        //        //    string fileName = Path.GetFileName(model.Attachment.FileName);
        //        //    mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
        //        //}
        //        mm.IsBodyHtml = false;

        //        //smtpHostType = smtpHostType.ToUpper();
        //        //if(smtpHostType == "GMAIL")
        //        //{
        //        //    smtpHost = "smtp.gmail.com";
        //        //    smtpPort = 587;
        //        //}

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Send(mm);
        //    }
        //}
    }
}