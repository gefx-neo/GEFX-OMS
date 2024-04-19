using DataAccess;
using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
	[HandleError]
	[RedirectingAction]
	[RedirectingActionWithSuperAdmin]
	[SessionState(SessionStateBehavior.ReadOnly)]
	public class ContactSupportSettingController : ControllerBase
    {
        // GET: ContactSupportSetting
        public ActionResult Index()
        {
			if (TempData["SearchKeyword"] != null)
			{
				TempData.Remove("SearchKeyword");
			}

			if (TempData["LogTable"] != null)
			{
				TempData.Remove("LogTable");
			}

			if (TempData["FromDate"] != null)
			{
				TempData.Remove("FromDate");
			}

			if (TempData["ToDate"] != null)
			{
				TempData.Remove("ToDate");
			}

			return RedirectToAction("Home");
		}

		public ActionResult Home()
		{
			ViewData["ContactSupportSetting"] = new CustomerPortal_SupportContactSetting();

			//SQL Command here
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
			{
				connection.Open();
				string commandText = commandText = @"SELECT * FROM [dbo].[CustomerPortal_SupportContactSetting]";

				using (SqlCommand command = new SqlCommand("SET ARITHABORT ON", connection))
				{
					command.ExecuteNonQuery();
					command.CommandText = commandText;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						CustomerPortal_SupportContactSetting setting = reader.Cast<IDataRecord>()
						.Select(x => new CustomerPortal_SupportContactSetting
						{
							ID = (int)x["ID"],
							Description = (x["Description"] == DBNull.Value) ? "" : (string)x["Description"],
							Image = (byte[])x["Image"],
							Email = (x["Email"] == DBNull.Value) ? "" : (string)x["Email"],
							Name = (x["Name"] == DBNull.Value) ? "" : (string)x["Name"],
							PhoneNo = (x["PhoneNo"] == DBNull.Value) ? "" : (string)x["PhoneNo"],

						}).FirstOrDefault();
						ViewData["ContactSupportSetting"] = setting;
					}
				}
				connection.Close();
			}
			
			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		public ActionResult Edit()
		{
			ViewData["ContactSupportSetting"] = new CustomerPortal_SupportContactSetting();

			//SQL Command here
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
			{
				connection.Open();
				string commandText = commandText = @"SELECT * FROM [dbo].[CustomerPortal_SupportContactSetting]";

				using (SqlCommand command = new SqlCommand("SET ARITHABORT ON", connection))
				{
					command.ExecuteNonQuery();
					command.CommandText = commandText;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						CustomerPortal_SupportContactSetting setting = reader.Cast<IDataRecord>()
						.Select(x => new CustomerPortal_SupportContactSetting
						{
							ID = (int)x["ID"],
							Description = (x["Description"] == DBNull.Value) ? "" : (string)x["Description"],
							Image = (byte[])x["Image"],
							Email = (x["Email"] == DBNull.Value) ? "" : (string)x["Email"],
							Name = (x["Name"] == DBNull.Value) ? "" : (string)x["Name"],
							PhoneNo = (x["PhoneNo"] == DBNull.Value) ? "" : (string)x["PhoneNo"],

						}).FirstOrDefault();
						ViewData["ContactSupportSetting"] = setting;
					}
				}
				connection.Close();
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		[HttpPost]
		public ActionResult Edit(FormCollection form, CustomerPortal_SupportContactSetting CSsetting)
		{
			ViewData["ContactSupportSetting"] = new CustomerPortal_SupportContactSetting();

			string Description = form["settings.Description"];
			string Image = form["Hidden_Image"];
			string Email = form["settings.Email"];
			string Name = form["settings.Name"];
			string PhoneNo = form["settings.PhoneNo"];
			ViewBag.UploadImageBefore = "No";
			//validation part
			if (string.IsNullOrEmpty(Description))
			{
				ModelState.AddModelError("settings.Description", "Description cannot be empty!");
			}

			if (string.IsNullOrEmpty(Image))
			{
				ModelState.AddModelError("settings.Image", "Image cannot be empty!");
			}
			else
			{
				//get the previous image record and compare
				//SQL Command here
				using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
				{
					connection.Open();
					string commandText = commandText = @"SELECT * FROM [dbo].[CustomerPortal_SupportContactSetting]";

					using (SqlCommand command = new SqlCommand("SET ARITHABORT ON", connection))
					{
						command.ExecuteNonQuery();
						command.CommandText = commandText;
						using (SqlDataReader reader = command.ExecuteReader())
						{
							CustomerPortal_SupportContactSetting setting = reader.Cast<IDataRecord>()
							.Select(x => new CustomerPortal_SupportContactSetting
							{
								ID = (int)x["ID"],
								Description = (x["Description"] == DBNull.Value) ? "" : (string)x["Description"],
								Image = (byte[])x["Image"],
								Email = (x["Email"] == DBNull.Value) ? "" : (string)x["Email"],
								Name = (x["Name"] == DBNull.Value) ? "" : (string)x["Name"],
								PhoneNo = (x["PhoneNo"] == DBNull.Value) ? "" : (string)x["PhoneNo"],
							}).FirstOrDefault();

							string getString = Convert.ToBase64String(setting.Image);

							if (getString != Image)
							{
								ViewBag.UploadImageBefore = "Yes";
							}
						}
					}
					connection.Close();
				}
			}

			if (string.IsNullOrEmpty(Email))
			{
				ModelState.AddModelError("settings.Email", "Email cannot be empty!");
			}
			else
			{
				//check format
				bool checkEmailFormat = FormValidationHelper.EmailValidation(Email);

				if (!checkEmailFormat)
				{
					ModelState.AddModelError("settings.Email", "Invalid Email Format!");
				}
			}

			if (string.IsNullOrEmpty(Name))
			{
				ModelState.AddModelError("settings.Name", "Name cannot be empty!");
			}

			if (string.IsNullOrEmpty(PhoneNo))
			{
				ModelState.AddModelError("settings.PhoneNo", "PhoneNo cannot be empty!");
			}

			if (ModelState.IsValid)
			{
				//save into db
				string commandText = "UPDATE [dbo].[CustomerPortal_SupportContactSetting] SET Description = @Desc, Image = @Image, Email = @Email, Name = @Name, PhoneNo = @PhoneNo Where ID = 1";
				using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
				{
					SqlCommand command = new SqlCommand(commandText, connection);
					
					command.Parameters.AddWithValue("@Desc", Description);
					command.Parameters.AddWithValue("@Image", Convert.FromBase64String(Image.Substring(Image.IndexOf(',') + 1)));
					command.Parameters.AddWithValue("@Email", Email);
					command.Parameters.AddWithValue("@Name", Name);
					command.Parameters.AddWithValue("@PhoneNo", PhoneNo);

					try
					{
						connection.Open();
						command.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
					connection.Close();

					TempData["Result"] = "success|Contact Support Details has been successfully updated!";
					return RedirectToAction("Home");
				}
			}
			else
			{
				//retrieve back the previous item
				//Image = Image.Replace('-', '+').Replace('_', '/').PadRight(4 * ((Image.Length + 3) / 4), '=');
				Image = Image.Substring(Image.IndexOf(',') + 1);
				var base64 = Convert.FromBase64String(Image);
				
				CustomerPortal_SupportContactSetting cs = new CustomerPortal_SupportContactSetting();
				cs.Description = Description;
				cs.Email = Email;
				cs.Image = base64;
				cs.Name = Name;
				cs.PhoneNo = PhoneNo;

				ViewData["ContactSupportSetting"] = cs;
				TempData["Result"] = "danger|Something went wrong in the form!";
				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
		}

		//POST: GetPreviousImage
		public ActionResult GetPreviousImage()
		{
			ViewData["ContactSupportSetting"] = new CustomerPortal_SupportContactSetting();

			//SQL Command here
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
			{
				connection.Open();
				string commandText = commandText = @"SELECT * FROM [dbo].[CustomerPortal_SupportContactSetting]";

				using (SqlCommand command = new SqlCommand("SET ARITHABORT ON", connection))
				{
					command.ExecuteNonQuery();
					command.CommandText = commandText;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						CustomerPortal_SupportContactSetting setting = reader.Cast<IDataRecord>()
						.Select(x => new CustomerPortal_SupportContactSetting
						{
							ID = (int)x["ID"],
							Description = (x["Description"] == DBNull.Value) ? "" : (string)x["Description"],
							Image = (byte[])x["Image"],
							Email = (x["Email"] == DBNull.Value) ? "" : (string)x["Email"],
							Name = (x["Name"] == DBNull.Value) ? "" : (string)x["Name"],
							PhoneNo = (x["PhoneNo"] == DBNull.Value) ? "" : (string)x["PhoneNo"],

						}).FirstOrDefault();
						ViewData["ContactSupportSetting"] = setting;
					}
				}
				connection.Close();
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		//POST: FileUploader
		[HttpPost]
		public void FileUploader()
		{
			string filesUploaded = "";

			try
			{
				if (Request.Files.Count > 0)
				{
					foreach (string key in Request.Files)
					{
						HttpPostedFileBase attachment = Request.Files[key];

						if (!string.IsNullOrEmpty(attachment.FileName))
						{
							string mimeType = attachment.ContentType;
							int fileLength = attachment.ContentLength;

							string[] allowedTypes = ConfigurationManager.AppSettings["AllowedFileTypes"].ToString().Split(',');

							if (allowedTypes.Contains(mimeType))
							{
								if (fileLength <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxFileSize"]) * 1024 * 1024)
								{
									string file = attachment.FileName.Substring(attachment.FileName.LastIndexOf(@"\") + 1, attachment.FileName.Length - (attachment.FileName.LastIndexOf(@"\") + 1));
									string fileName = Path.GetFileNameWithoutExtension(file);
									string newFileName = FileHelper.sanitiseFilename(fileName) + "_" + DateTime.Now.ToString("yyMMddHHmmss") + Path.GetExtension(file).ToLower();
									string path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

									if (!System.IO.File.Exists(path))
									{
										string oriPath = "";

										oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

										attachment.SaveAs(oriPath);

										//direct convert to base64 string
										byte[] buffer = System.IO.File.ReadAllBytes(oriPath);
										string getString = Convert.ToBase64String(buffer);
										filesUploaded = String.Format("data:image/gif;base64,{0}", getString);
										//filesUploaded += newFileName + ",";
									}
									else
									{
										Response.Write("{\"result\":\"error\",\"msg\":\"" + newFileName + " already exists.\"}");
										break;
									}
								}
								else
								{
									Response.Write("{\"result\":\"error\",\"msg\":\"File size exceeds 2MB.\"}");
									break;
								}
							}
							else
							{
								Response.Write("{\"result\":\"error\",\"msg\":\"Invalid file type.\"}");
								break;
							}
						}
						else
						{
							Response.Write("{\"result\":\"error\",\"msg\":\"Please select a file to upload.\"}");
							break;
						}
					}
				}
				else
				{
					Response.Write("{\"result\":\"error\",\"msg\":\"Please select a file to upload.\"}");
				}

				if (!string.IsNullOrEmpty(filesUploaded))
				{
					Response.Write("{\"result\":\"success\",\"msg\":\"" + filesUploaded + "\"}");
				}
			}
			catch
			{
				Response.Write("{\"result\":\"error\",\"msg\":\"An error occured while uploading file.\"}");
			}
		}
	}
}