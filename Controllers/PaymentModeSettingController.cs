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
	public class PaymentModeSettingController : ControllerBase
    {
		// GET: PaymentModeSetting
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
			ViewData["PaymentModeSetting"] = new PaymentModeLists();

			//SQL Command here
			using (var context = new DataAccess.GreatEastForex())
			{
				var setting = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();

				ViewData["PaymentModeSetting"] = setting;
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		public ActionResult Edit()
		{
			ViewData["PaymentModeSetting"] = new PaymentModeLists();

			//SQL Command here
			using (var context = new DataAccess.GreatEastForex())
			{
				var setting = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();

				ViewData["PaymentModeSetting"] = setting;
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(FormCollection form, PaymentModeLists setting)
		{
			ViewData["PaymentModeSetting"] = new PaymentModeLists();

			PaymentModeLists newPayment = new PaymentModeLists();
			List<PaymentModeLists> listofPayment = new List<PaymentModeLists>();

			List<string> GetKeyValue = form.AllKeys.Where(e => e.Contains("InstructionText_")).ToList();

			foreach (string order in GetKeyValue)
			{
				newPayment = new PaymentModeLists();
				string RowId = order.Split('_')[1];

				string checkItem = form["InstructionText_" + RowId];
				
				if (string.IsNullOrEmpty(checkItem))
				{
					ModelState.AddModelError("InstructionText_" + RowId, "Description cannot be empty!");
				}

				newPayment.ID = Convert.ToInt32(RowId);
				newPayment.Name = form["Name_" + RowId];
				newPayment.InstructionText = checkItem;

				listofPayment.Add(newPayment);

			}

			if (ModelState.IsValid)
			{
				using (var context = new DataAccess.GreatEastForex())
				{
					var some = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();

					foreach (string order in GetKeyValue)
					{
						int RowId = Convert.ToInt32(order.Split('_')[1]);

						string checkItem = form["InstructionText_" + RowId.ToString()];

						if (some.Where(e => e.ID == RowId).FirstOrDefault() != null)
						{
							some.Where(e => e.ID == RowId).FirstOrDefault().InstructionText = checkItem;
							some.Where(e => e.ID == RowId).FirstOrDefault().UpdatedOn = DateTime.Now;
						}
					}
					context.SaveChanges();

					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "PaymentModeSetting";
					string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Payment Mode Settings.";

					bool paymentmodesetting_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData["Result"] = "success|Payment Mode Details has been successfully updated!";
					return RedirectToAction("Home");
				}
			}
			else
			{
				ViewData["PaymentModeSetting"] = listofPayment;
				TempData["Result"] = "danger|Something went wrong in the form!";
				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
		}
	}
}