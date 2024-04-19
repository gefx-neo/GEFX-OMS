using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
	[HandleError]
	[RedirectingAction]
	public class RemittanceProductController : ControllerBase
    {
		private IRemittanceProductRepository _remittanceProductsModel;

		public RemittanceProductController()
			: this(new RemittanceProductRepository())
		{

		}

		public RemittanceProductController(IRemittanceProductRepository remittanceProductsModel)
		{
			_remittanceProductsModel = remittanceProductsModel;
		}

		// GET: RemittanceProduct
		public ActionResult Index()
        {
			return RedirectToAction("Listing");
		}

		public ActionResult Create()
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Sales Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				Dropdown[] statusDDL = StatusDDL();
				ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name");

				using (var context = new DataAccess.GreatEastForex())
				{
					ViewData["PaymentModeCheckBox"] = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();
				}

				ViewData["IsCheck"] = "";

				Dropdown[] productDecimalDDL = ProductDecimalDDL();
				ViewData["ProductDecimalDropdown"] = new SelectList(productDecimalDDL, "val", "name");

				Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
				ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name");

				Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
				ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name");

				var products = new RemittanceProducts();
				products.AcceptableRange = 5;
				products.ProductSymbol = "$";

				ViewData["PaymentModeAllowedCashCheckbox"] = "";
				ViewData["PaymentModeAllowedChequeCheckbox"] = "";
				ViewData["PaymentModeAllowedBankTransferCheckbox"] = "";
				ViewData["TransactionTypeAllowedPayCheckbox"] = "";
				ViewData["TransactionTypeAllowedGetCheckbox"] = "";
				ViewData["Product"] = products;

				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to access this page!");
				return RedirectToAction("Index","TaskList");
			}
		}

		[HttpPost]
		public ActionResult Create(RemittanceProducts products, FormCollection form, string[] flags)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData["Page"]);
				ViewData["Page"] = page;
			}

			if (!string.IsNullOrEmpty(products.CurrencyCode))
			{
				if (string.IsNullOrEmpty(products.CurrencyCode.Trim()))
				{
					ModelState.AddModelError("products.CurrencyCode", "Currency Code cannot be empty!");
				}
				else
				{
					if (products.CurrencyCode.Length != 3)
					{
						ModelState.AddModelError("products.CurrencyCode", "Currency Code must exactly 3 characters!");
					}
					else
					{
						RemittanceProducts checkCurrencyCode = _remittanceProductsModel.FindCurrencyCode(products.CurrencyCode);

						if (checkCurrencyCode != null)
						{
							ModelState.AddModelError("products.CurrencyCode", "Currency Code already existed!");
						}
					}
				}
			}
			else
			{
				ModelState.AddModelError("products.CurrencyCode", "Currency Code cannot be empty!");
			}

			if (string.IsNullOrEmpty(products.CurrencyName) || string.IsNullOrEmpty(products.CurrencyName.Trim()))
			{
				ModelState.AddModelError("products.CurrencyName", "Currency Name cannot be empty!");
			}

			if (string.IsNullOrEmpty(products.ProductSymbol) || string.IsNullOrEmpty(products.ProductSymbol.Trim()))
			{
				ModelState.AddModelError("products.ProductSymbol", "Symbol cannot be empty!");
			}
			else
			{
				if (products.ProductSymbol.Length > 3)
				{
					ModelState.AddModelError("products.ProductSymbol", "Symbol must less than equal to 3 characters!");
				}
			}

			if (products.BuyRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment is not valid!");
				}
				else
				{
					if (products.BuyRateAdjustment > 999)
					{
						ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (products.SellRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment is not valid!");
				}
				else
				{
					if (products.SellRateAdjustment > 999)
					{
						ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (ModelState.IsValid)
			{
				if (flags != null)
				{
					products.PaymentModeAllowed = string.Join(",", flags);
				}

				if (!string.IsNullOrEmpty(form["customerParticulars.TransactionTypeAllowed"]))
				{
					products.TransactionTypeAllowed = form["customerParticulars.TransactionTypeAllowed"];
				}

				//if (!products.GetRate.HasValue)
				//{
				//	products.GetRate = 0;
				//}

				//if (!products.PayRate.HasValue)
				//{
				//	products.PayRate = 0;
				//}

				//products.AutomatedGetRate = 0;
				//products.AutomatedPayRate = 0;
				products.IsBaseProduct = 0;

				bool result = _remittanceProductsModel.Add(products);

				if (result)
				{
					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "RemittanceProducts";
					string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Remittance Product";

					bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully created!");

					return RedirectToAction("Listing", new { @page = page });
				}
				else
				{
					TempData.Add("Result", "danger|An error occured while saving remittance product record!");
				}
			}
			else
			{
				TempData.Add("Result", "danger|There is something wrong in the form!");
			}

			using (var context = new DataAccess.GreatEastForex())
			{
				ViewData["PaymentModeCheckBox"] = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();
			}

			ViewData["IsCheck"] = flags;

			Dropdown[] statusDDL = StatusDDL();
			ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

			Dropdown[] productDecimalDDL = ProductDecimalDDL();
			ViewData["ProductDecimalDropdown"] = new SelectList(productDecimalDDL, "val", "name", products.ProductDecimal);

			Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
			ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

			Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
			ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

			ViewData["PaymentModeAllowedCashCheckbox"] = "";
			ViewData["PaymentModeAllowedChequeCheckbox"] = "";
			ViewData["PaymentModeAllowedBankTransferCheckbox"] = "";
			ViewData["TransactionTypeAllowedPayCheckbox"] = "";
			ViewData["TransactionTypeAllowedGetCheckbox"] = "";

			if (!string.IsNullOrEmpty(form["customerParticulars.PaymentModeAllowed"]))
			{
				string[] type = form["customerParticulars.PaymentModeAllowed"].ToString().Split(',');

				if (Array.IndexOf(type, "Cash") >= 0)
				{
					ViewData["PaymentModeAllowedCashCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Cheque") >= 0)
				{
					ViewData["PaymentModeAllowedChequeCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Bank Transfer") >= 0)
				{
					ViewData["PaymentModeAllowedBankTransferCheckbox"] = "checked";
				}
			}

			if (!string.IsNullOrEmpty(form["customerParticulars.TransactionTypeAllowed"]))
			{
				string[] type = form["customerParticulars.TransactionTypeAllowed"].ToString().Split(',');

				if (Array.IndexOf(type, "Pay") >= 0)
				{
					ViewData["TransactionTypeAllowedPayCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Get") >= 0)
				{
					ViewData["TransactionTypeAllowedGetCheckbox"] = "checked";
				}
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		public ActionResult Edit(int id)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');
			ViewData["ViewOnly"] = "";

			if (Array.IndexOf(userRoleList, "Finance") >= 0)
			{
				ViewData["ViewOnly"] = "Yes";
			}

			if (Array.IndexOf(userRoleList, "Sales Admin") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				RemittanceProducts products = _remittanceProductsModel.GetSingle(id);

				if (products != null)
				{
					ViewData["ProductID"] = products.ID;
					Dropdown[] statusDDL = StatusDDL();
					ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

					Dropdown[] productDecimalDDL = ProductDecimalDDL();
					ViewData["ProductDecimalDropdown"] = new SelectList(productDecimalDDL, "val", "name", products.ProductDecimal);

					Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
					ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

					Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
					ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

					ViewData["PaymentModeAllowedCashCheckbox"] = "";
					ViewData["PaymentModeAllowedChequeCheckbox"] = "";
					ViewData["PaymentModeAllowedBankTransferCheckbox"] = "";
					ViewData["TransactionTypeAllowedPayCheckbox"] = "";
					ViewData["TransactionTypeAllowedGetCheckbox"] = "";

					if (!string.IsNullOrEmpty(products.PaymentModeAllowed))
					{
						string[] type = products.PaymentModeAllowed.ToString().Split(',');

						if (Array.IndexOf(type, "Cash") >= 0)
						{
							ViewData["PaymentModeAllowedCashCheckbox"] = "checked";
						}

						if (Array.IndexOf(type, "Cheque") >= 0)
						{
							ViewData["PaymentModeAllowedChequeCheckbox"] = "checked";
						}

						if (Array.IndexOf(type, "Bank Transfer") >= 0)
						{
							ViewData["PaymentModeAllowedBankTransferCheckbox"] = "checked";
						}
					}

					if (!string.IsNullOrEmpty(products.TransactionTypeAllowed))
					{
						string[] type = products.TransactionTypeAllowed.ToString().Split(',');

						if (Array.IndexOf(type, "Pay") >= 0)
						{
							ViewData["TransactionTypeAllowedPayCheckbox"] = "checked";
						}

						if (Array.IndexOf(type, "Get") >= 0)
						{
							ViewData["TransactionTypeAllowedGetCheckbox"] = "checked";
						}
					}

					using (var context = new DataAccess.GreatEastForex())
					{
						ViewData["PaymentModeCheckBox"] = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();
					}

					ViewData["IsCheck"] = new string[] { };

					if (!string.IsNullOrEmpty(products.PaymentModeAllowed))
					{
						ViewData["IsCheck"] = products.PaymentModeAllowed.Split(',');
					}

					ViewData["Product"] = products;
					ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
					return View();
				}

				TempData.Add("Result", "danger|Remittance Product record not found!");

				return RedirectToAction("Listing", new { @page = page });
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to access this page!");
				return RedirectToAction("Index", "TaskList");
			}
		}

		[HttpPost]
		public ActionResult Edit(int id, RemittanceProducts products, FormCollection form, string[] flags)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData["Page"]);
				ViewData["Page"] = page;
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');
			ViewData["ViewOnly"] = "";

			if (Array.IndexOf(userRoleList, "Finance") >= 0)
			{
				ViewData["ViewOnly"] = "Yes";
			}

			RemittanceProducts oldData = _remittanceProductsModel.GetSingle(id);

			if (!string.IsNullOrEmpty(products.CurrencyCode))
			{
				if (string.IsNullOrEmpty(products.CurrencyCode.Trim()))
				{
					ModelState.AddModelError("products.CurrencyCode", "Currency Code cannot be empty!");
				}
				else
				{
					if (products.CurrencyCode.Length != 3)
					{
						ModelState.AddModelError("products.CurrencyCode", "Currency Code must exactly 3 characters!");
					}
					else
					{
						RemittanceProducts checkCurrencyCode = _remittanceProductsModel.FindCurrencyCodeNotOwnSelf(id, products.CurrencyCode);

						if (checkCurrencyCode != null)
						{
							ModelState.AddModelError("products.CurrencyCode", "Currency Code already existed!");
						}
					}
				}
			}
			else
			{
				ModelState.AddModelError("products.CurrencyCode", "Currency Code cannot be empty!");
			}

			if (string.IsNullOrEmpty(products.CurrencyName) || string.IsNullOrEmpty(products.CurrencyName.Trim()))
			{
				ModelState.AddModelError("products.CurrencyName", "Currency Name cannot be empty!");
			}

			if (string.IsNullOrEmpty(products.ProductSymbol) || string.IsNullOrEmpty(products.ProductSymbol.Trim()))
			{
				ModelState.AddModelError("products.ProductSymbol", "Symbol cannot be empty!");
			}
			else
			{
				if (products.ProductSymbol.Length > 3)
				{
					ModelState.AddModelError("products.ProductSymbol", "Symbol must less than equal to 3 characters!");
				}
			}

			if (products.BuyRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment is not valid!");
				}
				else
				{
					if (products.BuyRateAdjustment > 999)
					{
						ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (products.SellRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment is not valid!");
				}
				else
				{
					if (products.SellRateAdjustment > 999)
					{
						ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (ModelState.IsValid)
			{
				if (flags != null)
				{
					products.PaymentModeAllowed = string.Join(",", flags);
				}

				if (!string.IsNullOrEmpty(form["customerParticulars.PaymentModeAllowed"]))
				{
					products.PaymentModeAllowed = form["customerParticulars.PaymentModeAllowed"];
				}

				if (!string.IsNullOrEmpty(form["customerParticulars.TransactionTypeAllowed"]))
				{
					products.TransactionTypeAllowed = form["customerParticulars.TransactionTypeAllowed"];
				}

				bool result = _remittanceProductsModel.Update(id, products);

				if (result)
				{
					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "RemittanceProducts";
					string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Remittance Product";

					bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully updated!");

					return RedirectToAction("Listing", new { @page = page });
				}
				else
				{
					TempData.Add("Result", "danger|An error occured while saving remittance product record!");
				}
			}
			else
			{
				TempData.Add("Result", "danger|There is something wrong in the form!");
			}

			ViewData["ProductID"] = products.ID;
			ViewData["Product"] = products;

			using (var context = new DataAccess.GreatEastForex())
			{
				ViewData["PaymentModeCheckBox"] = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();
			}

			ViewData["IsCheck"] = flags;

			Dropdown[] statusDDL = StatusDDL();
			ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

			Dropdown[] productDecimalDDL = ProductDecimalDDL();
			ViewData["ProductDecimalDropdown"] = new SelectList(productDecimalDDL, "val", "name", products.ProductDecimal);

			Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
			ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

			Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
			ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

			ViewData["PaymentModeAllowedCashCheckbox"] = "";
			ViewData["PaymentModeAllowedChequeCheckbox"] = "";
			ViewData["PaymentModeAllowedBankTransferCheckbox"] = "";
			ViewData["TransactionTypeAllowedPayCheckbox"] = "";
			ViewData["TransactionTypeAllowedGetCheckbox"] = "";

			if (!string.IsNullOrEmpty(form["customerParticulars.PaymentModeAllowed"]))
			{
				string[] type = form["customerParticulars.PaymentModeAllowed"].ToString().Split(',');

				if (Array.IndexOf(type, "Cash") >= 0)
				{
					ViewData["PaymentModeAllowedCashCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Cheque") >= 0)
				{
					ViewData["PaymentModeAllowedChequeCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Bank Transfer") >= 0)
				{
					ViewData["PaymentModeAllowedBankTransferCheckbox"] = "checked";
				}
			}

			if (!string.IsNullOrEmpty(form["customerParticulars.TransactionTypeAllowed"]))
			{
				string[] type = form["customerParticulars.TransactionTypeAllowed"].ToString().Split(',');

				if (Array.IndexOf(type, "Pay") >= 0)
				{
					ViewData["TransactionTypeAllowedPayCheckbox"] = "checked";
				}

				if (Array.IndexOf(type, "Get") >= 0)
				{
					ViewData["TransactionTypeAllowedGetCheckbox"] = "checked";
				}
			}

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		//GET: Delete
		public ActionResult Delete(int id)
		{
			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Sales Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				int page = 1;

				if (TempData["Page"] != null)
				{
					page = Convert.ToInt32(TempData["Page"]);
				}

				RemittanceProducts products = _remittanceProductsModel.GetSingle(id);

				if (products != null)
				{
					bool result = _remittanceProductsModel.Delete(id);

					if (result)
					{
						int userid = Convert.ToInt32(Session["UserId"]);
						string tableAffected = "RemittanceProducts";
						string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Remittance Product";

						bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

						TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully deleted!");
					}
					else
					{
						TempData.Add("Result", "danger|An error occured while deleting remittance product record!");
					}
				}
				else
				{
					TempData.Add("Result", "danger|Remittance Product record not found!");
				}

				return RedirectToAction("Listing", new { @page = page });
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to delete this record!");
				return RedirectToAction("Index", "TaskList");
			}
		}

		public ActionResult Listing(int page = 1)
		{
			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Sales Admin") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

				TempData["Page"] = page;
				ViewData["Page"] = page;

				TempData["PageSize"] = pageSize;
				ViewData["PageSize"] = pageSize;

				ViewData["SearchKeyword"] = "";

				if (TempData["SearchKeyword"] != null)
				{
					ViewData["SearchKeyword"] = TempData["SearchKeyword"];
				}

				IPagedList<RemittanceProducts> products = _remittanceProductsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
				ViewData["Products"] = products;

				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to access this page!");
				return RedirectToAction("Index", "TaskList");
			}
		}

		//POST: Listing
		[HttpPost]
		public ActionResult Listing(FormCollection form)
		{
			int page = 1;
			int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

			TempData["Page"] = page;
			ViewData["Page"] = page;

			TempData["PageSize"] = pageSize;
			ViewData["PageSize"] = pageSize;

			ViewData["SearchKeyword"] = form["SearchKeyword"].Trim();
			TempData["SearchKeyword"] = form["SearchKeyword"].Trim();

			IPagedList<RemittanceProducts> products = _remittanceProductsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
			ViewData["Products"] = products;

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		//Status Dropdown
		public Dropdown[] StatusDDL()
		{
			Dropdown[] ddl = new Dropdown[2];
			ddl[0] = new Dropdown { name = "Active", val = "Active" };
			ddl[1] = new Dropdown { name = "Suspended", val = "Suspended" };
			return ddl;
		}

		//Product Decimal Dropdown
		public Dropdown[] ProductDecimalDDL()
		{
			Dropdown[] ddl = new Dropdown[4];
			ddl[0] = new Dropdown { name = "1", val = "1" };
			ddl[1] = new Dropdown { name = "2", val = "2" };
			ddl[2] = new Dropdown { name = "3", val = "3" };
			ddl[3] = new Dropdown { name = "4", val = "4" };
			return ddl;
		}

		//Guarantee Rate Dropdown
		public Dropdown[] GuaranteeRateDDL()
		{
			Dropdown[] ddl = new Dropdown[2];
			ddl[0] = new Dropdown { name = "No", val = "0" };
			ddl[1] = new Dropdown { name = "Yes", val = "1" };
			return ddl;
		}

		//Popular Currency Dropdown
		public Dropdown[] PopularCurrencyDDL()
		{
			Dropdown[] ddl = new Dropdown[2];
			ddl[0] = new Dropdown { name = "No", val = "0" };
			ddl[1] = new Dropdown { name = "Yes", val = "1" };
			return ddl;
		}
	}
}