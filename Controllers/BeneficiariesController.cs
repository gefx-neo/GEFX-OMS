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
	[RedirectingActionDealerOrSuperAdmin]
	public class BeneficiariesController : ControllerBase
    {
		private IBeneficiaryRepository _beneficiariesModel;
		private ICustomerParticularRepository _customersModel;

		public BeneficiariesController()
			: this(new BeneficiaryRepository(), new CustomerParticularRepository())
		{

		}

		public BeneficiariesController(IBeneficiaryRepository beneficiariesModel, ICustomerParticularRepository customersModel)
		{
			_beneficiariesModel = beneficiariesModel;
			_customersModel = customersModel;
		}

		// GET: Beneficiaries
		public ActionResult Index()
        {
			return RedirectToAction("Listing");
		}

		public ActionResult Listing(int page = 1)
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

			IPagedList<Beneficiaries> beneficiary = _beneficiariesModel.GetPagedBeneficiaries(ViewData["SearchKeyword"].ToString(), page, pageSize);
			ViewData["Beneficiaries"] = beneficiary;

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
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

			IPagedList<Beneficiaries> beneficiaries = _beneficiariesModel.GetPagedBeneficiaries(ViewData["SearchKeyword"].ToString(), page, pageSize);
			ViewData["Beneficiaries"] = beneficiaries;

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
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

			using (var context = new DataAccess.GreatEastForex())
			{
				ViewBag.Beneficiary = "active";
				Dropdown[] _CustomerDDL = CustomerDDL(0);
				ViewData["CustomerDropdown"] = new SelectList(_CustomerDDL, "val", "name");

				Dropdown[] _BankCountryDDL = CountryDDL();
				ViewData["BankCountryDropdown"] = new SelectList(_BankCountryDDL, "val", "name");

				Dropdown[] _PurposeOfPaymentDDL = PurposeOfPaymentDDL(99);//99 is because default dont load data first.

				ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name");

				Dropdown[] _SourceOfPaymentDDL = SourceOfPaymentDDL();
				ViewData["SourceOfPaymentDropdown"] = new SelectList(_SourceOfPaymentDDL, "val", "name");

				Dropdown[] _BusinessCategoryDDL = BusinessCategoryDDL();
				ViewData["BusinessCategoryDropdown"] = new SelectList(_BusinessCategoryDDL, "val", "name", "optGroup", null, null);

				Dropdown[] _NationalityDDL = NationalityDDL();
				ViewData["NationalityDropdown"] = new SelectList(_NationalityDDL, "val", "name");

				ViewData["Beneficiaries"] = new Beneficiaries();
				ViewData["BeneficiaryTypeIndividual"] = "";
				ViewData["BeneficiaryTypeBusiness"] = "";
				ViewData["BeneficiaryIsYourAccountNo"] = "";
				ViewData["BeneficiaryIsYourAccountYes"] = "";
				//ViewData["BeneficiaryBankTypeBankCode"] = "";
				//ViewData["BeneficiaryBankTypeSWIFT"] = "";

				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
		}

		[HttpPost]
		public ActionResult Create(Beneficiaries beneficiary, FormCollection form)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			using (var context = new DataAccess.GreatEastForex())
			{
				var checkID = form["beneficiary.CustomerParticularId"];

				if (form["beneficiary.CustomerParticularId"] == null && string.IsNullOrEmpty(form["beneficiary.CustomerParticularId"]))
				{
					ModelState.AddModelError("beneficiary.CustomerParticularId", "Please select customer.");
				}

				if (form["beneficiary.Type"] == null)
				{
					ModelState.AddModelError("beneficiary.Type", "Please select who do you want to send the money to.");
				}
				else
				{
					if (form["beneficiary.Type"] != "0" && form["beneficiary.Type"] != "1")
					{
						ModelState.AddModelError("beneficiary.Type", "Something went wrong in select who do you want to send the money to.");
					}
				}

				if (form["beneficiary.IsYourAccount"] == null)
				{
					ModelState.AddModelError("beneficiary.IsYourAccount", "Please select is this your account.");
				}
				else
				{
					//check integer to prevent user backend change the value
					if (form["beneficiary.IsYourAccount"] != "1" && form["beneficiary.IsYourAccount"] != "0")
					{
						ModelState.AddModelError("beneficiary.IsYourAccount", "Something went wrong in select this your account.");
					}
				}

				if (string.IsNullOrEmpty(beneficiary.BeneficiaryFriendlyName))
				{
					ModelState.AddModelError("beneficiary.BeneficiaryFriendlyName", "Please enter your beneficiary friendly name.");
				}
				else
				{
					//check have same friendly name or not
					int customerId = beneficiary.CustomerParticularId;
					string beneficiaryName = beneficiary.BeneficiaryFriendlyName;

					if (context.Beneficiaries.Where(e => e.CustomerParticularId == customerId && e.BeneficiaryFriendlyName == beneficiaryName).FirstOrDefault() != null)
					{
						ModelState.AddModelError("beneficiary.BeneficiaryFriendlyName", "Beneficiary Friendly Name must unique.");
					}
				}

				//if (form["beneficiary.BankType"] != null)
				//{
				//	if (form["beneficiary.BankType"] != "1" && form["beneficiary.BankType"] != "2")
				//	{
				//		ModelState.AddModelError("beneficiary.BankType", "Something went wrong in select Bank Type.");
				//	}
				//}

				//Check Bank Country
				if (form["beneficiary.BankCountry"] != null && !string.IsNullOrEmpty(form["beneficiary.BankCountry"]))
				{
					int CountryID = Convert.ToInt32(form["beneficiary.BankCountry"]);
					var CheckCountry = context.Countries.Where(e => e.ID == CountryID && e.IsDeleted == 0).FirstOrDefault();
					if (CheckCountry != null)
					{
						if (CheckCountry.Name == "Others")
						{
							if (string.IsNullOrEmpty(form["beneficiary.BankCountryIfOthers"]))
							{
								ModelState.AddModelError("beneficiary.BankCountryIfOthers", "Bank Country If Others cannot be empty.");
							}
							else
							{
								if (form["beneficiary.BankCountryIfOthers"].ToString().Length > 250)
								{
									ModelState.AddModelError("beneficiary.BankCountryIfOthers", "Bank Country If Others cannot more than 250 characters.");
								}
							}
						}
					}
				}

				if (form["beneficiary.Type"] != "0" && form["beneficiary.Type"] != "1")
				{
				}
				else
				{
					if (form["beneficiary.Type"] == "0")
					{
						//Check Nationality
						if (form["beneficiary.BeneficiaryNationality"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryNationality"]))
						{
							int NationalityID = Convert.ToInt32(form["beneficiary.BeneficiaryNationality"]);
							var Checknationality = context.Nationalities.Where(e => e.ID == NationalityID && e.IsDeleted == 0).FirstOrDefault();
							if (Checknationality != null)
							{
								if (Checknationality.Name == "Others")
								{
									if (string.IsNullOrEmpty(form["beneficiary.BeneficiaryNationalityIfOthers"]))
									{
										ModelState.AddModelError("beneficiary.BeneficiaryNationalityIfOthers", "Nationality If Others cannot be empty.");
									}
									else
									{
										if (form["beneficiary.BeneficiaryNationalityIfOthers"].ToString().Length > 250)
										{
											ModelState.AddModelError("beneficiary.BeneficiaryNationalityIfOthers", "Nationality If Others cannot more than 250 characters.");
										}
									}
								}
							}
						}
					}
					else if (form["beneficiary.Type"] == "1")
					{
						//Check Business Category
						if (form["beneficiary.BeneficiaryBusinessCategory"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryBusinessCategory"]))
						{
							int BusinessCategoryID = Convert.ToInt32(form["beneficiary.BeneficiaryBusinessCategory"]);
							var CheckBusinessCategory = context.BusinessCategoriesLists.Where(e => e.ID == BusinessCategoryID && e.IsDeleted == 0).FirstOrDefault();
							if (CheckBusinessCategory != null)
							{
								if (CheckBusinessCategory.Name == "Others")
								{
									if (string.IsNullOrEmpty(form["beneficiary.BeneficiaryBusinessCategoryIfOthers"]))
									{
										ModelState.AddModelError("beneficiary.BeneficiaryBusinessCategoryIfOthers", "Beneficiary's Business Category If Others cannot be empty.");
									}
									else
									{
										if (form["beneficiary.BeneficiaryBusinessCategoryIfOthers"].ToString().Length > 250)
										{
											ModelState.AddModelError("beneficiary.BeneficiaryBusinessCategoryIfOthers", "Beneficiary's Business Category If Others cannot more than 250 characters.");
										}
									}
								}
							}
						}
					}
				}

				//Check Purpose of Payment
				if (form["beneficiary.PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.PurposeOfPayment"]))
				{
					int PurposeOfPaymentID = Convert.ToInt32(form["beneficiary.PurposeOfPayment"]);
					var CheckPurposeOfPaymentID = context.PaymentLists.Where(e => e.ID == PurposeOfPaymentID && e.IsDeleted == 0).FirstOrDefault();
					if (CheckPurposeOfPaymentID != null)
					{
						if (CheckPurposeOfPaymentID.Name == "Others")
						{
							if (string.IsNullOrEmpty(form["beneficiary.PurposeOfPaymentIfOthers"]))
							{
								ModelState.AddModelError("beneficiary.PurposeOfPaymentIfOthers", "Purpose of Payment If Others cannot be empty.");
							}
							else
							{
								if (form["beneficiary.PurposeOfPaymentIfOthers"].ToString().Length > 250)
								{
									ModelState.AddModelError("beneficiary.PurposeOfPaymentIfOthers", "Purpose of Payment If Others cannot more than 250 characters.");
								}
							}
						}
					}
				}
				else
				{
					//beneficiary.PurposeOfPayment
					ModelState["beneficiary.PurposeOfPayment"].Errors.Clear();
				}

				//Check Source of Payment
				if (form["beneficiary.SourceOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.SourceOfPayment"]))
				{
					int SourceOfPaymentID = Convert.ToInt32(form["beneficiary.SourceOfPayment"]);
					var CheckSourceOfPaymentID = context.FundLists.Where(e => e.ID == SourceOfPaymentID && e.IsDeleted == 0).FirstOrDefault();
					if (CheckSourceOfPaymentID != null)
					{
						if (CheckSourceOfPaymentID.Name == "Others")
						{
							if (string.IsNullOrEmpty(form["beneficiary.SourceOfPaymentIfOthers"]))
							{
								ModelState.AddModelError("beneficiary.SourceOfPaymentIfOthers", "Source of Payment If Others cannot be empty.");
							}
							else
							{
								if (form["beneficiary.SourceOfPaymentIfOthers"].ToString().Length > 250)
								{
									ModelState.AddModelError("beneficiary.SourceOfPaymentIfOthers", "Source of Payment If Others cannot more than 250 characters.");
								}
							}
						}
					}
				}

				if (ModelState.IsValid)
				{
					//start add into beneficiary part.
					int purposeOfPayment = 0;
					int nationality = 0;
					int businessCategory = 0;
					int Country = 0;
					int sourceOfPayment = 0;

					if (form["beneficiary.PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.PurposeOfPayment"]))
					{
						purposeOfPayment = Convert.ToInt32(form["beneficiary.PurposeOfPayment"].ToString());
					}

					if (form["beneficiary.BeneficiaryNationality"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryNationality"]))
					{
						nationality = Convert.ToInt32(form["beneficiary.BeneficiaryNationality"].ToString());
					}

					if (form["beneficiary.BeneficiaryBusinessCategory"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryBusinessCategory"]))
					{
						businessCategory = Convert.ToInt32(form["beneficiary.BeneficiaryBusinessCategory"]);
					}

					if (form["beneficiary.BankCountry"] != null && !string.IsNullOrEmpty(form["beneficiary.BankCountry"]))
					{
						Country = Convert.ToInt32(form["beneficiary.BankCountry"]); ;
					}

					if (form["beneficiary.SourceOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.SourceOfPayment"]))
					{
						sourceOfPayment = Convert.ToInt32(form["beneficiary.SourceOfPayment"]);
					}

					Beneficiaries newRecord = new Beneficiaries();
					newRecord.CustomerParticularId = Convert.ToInt32(beneficiary.CustomerParticularId);
					newRecord.Type = Convert.ToInt32(form["beneficiary.Type"]);//0 = individual, 1 = business

					if (newRecord.Type == 0)
					{
						//individual
						newRecord.BeneficiaryNationality = nationality;
						newRecord.BeneficiaryNationalityIfOthers = form["beneficiary.BeneficiaryNationalityIfOthers"];
						newRecord.BeneficiaryCompanyRegistrationNo = null;
						newRecord.BeneficiaryBusinessCategory = 0;
						newRecord.BeneficiaryContactNo = null;
						newRecord.BeneficiaryBusinessCategoryIfOthers = null;
					}
					else
					{
						//business
						newRecord.BeneficiaryNationality = 0;//0 means dont have
						newRecord.BeneficiaryNationalityIfOthers = null;
						newRecord.BeneficiaryCompanyRegistrationNo = form["beneficiary.BeneficiaryCompanyRegistrationNo"];
						newRecord.BeneficiaryBusinessCategory = businessCategory;
						newRecord.BeneficiaryContactNo = form["beneficiary.BeneficiaryContactNo"];
						newRecord.BeneficiaryBusinessCategoryIfOthers = form["beneficiary.BeneficiaryBusinessCategoryIfOthers"];
					}

					newRecord.IsYourAccount = Convert.ToInt32(form["beneficiary.IsYourAccount"]);
					newRecord.BeneficiaryFriendlyName = form["beneficiary.BeneficiaryFriendlyName"];
					newRecord.BeneficiaryFullName = form["beneficiary.BeneficiaryFullName"];
					newRecord.BeneficiaryContactNoMain = form["beneficiary.BeneficiaryContactNoMain"];
					newRecord.BeneficiaryAddressMain = form["beneficiary.BeneficiaryAddressMain"];
					newRecord.BeneficiaryBankName = form["beneficiary.BeneficiaryBankName"];
					newRecord.IBANEuropeBSBAustralia = form["beneficiary.IBANEuropeBSBAustralia"];
					//newRecord.BankType = Convert.ToInt32(form["beneficiary.BankType"]);
					newRecord.BankCode = form["beneficiary.BankCode"];
					newRecord.BankAccountNo = form["beneficiary.BankAccountNo"];
					newRecord.BankCountry = Country;
					newRecord.BankCountryIfOthers = form["beneficiary.BankCountryIfOthers"];
					newRecord.BankAddress = form["beneficiary.BankAddress"];
					newRecord.PurposeOfPayment = purposeOfPayment;
					newRecord.SourceOfPayment = sourceOfPayment;
					newRecord.PaymentDetails = form["beneficiary.PaymentDetails"];
					//newRecord.BeneficiaryNationalityIfOthers = form["beneficiary.BeneficiaryNationalityIfOthers"];
					//newRecord.BeneficiaryBusinessCategoryIfOthers = form["beneficiary.BeneficiaryBusinessCategoryIfOthers"];
					newRecord.PurposeOfPaymentIfOthers = form["beneficiary.PurposeOfPaymentIfOthers"];
					newRecord.SourceOfPaymentIfOthers = form["beneficiary.SourceOfPaymentIfOthers"];
					newRecord.Status = "Active";
					newRecord.CreatedOn = DateTime.Now;
					newRecord.UpdatedOn = DateTime.Now;

					context.Beneficiaries.Add(newRecord);
					context.SaveChanges();

					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "Beneficiaries";
					string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Beneficiary";

					bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData["Result"] = "sucess|Beneficiary [" + newRecord.BeneficiaryFriendlyName + "] created successfully.";
					return RedirectToAction("Listing", "Beneficiaries");
				}
				else
				{
					ViewBag.Beneficiary = "active";
					Dropdown[] _CustomerDDL = CustomerDDL(beneficiary.CustomerParticularId);
					ViewData["CustomerDropdown"] = new SelectList(_CustomerDDL, "val", "name", beneficiary.CustomerParticularId);

					Dropdown[] _BankCountryDDL = CountryDDL();
					ViewData["BankCountryDropdown"] = new SelectList(_BankCountryDDL, "val", "name", beneficiary.BankCountry);

					Dropdown[] _PurposeOfPaymentDDL = PurposeOfPaymentDDL(beneficiary.Type);

					if (beneficiary.PurposeOfPayment == 0)
					{
						ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name");
					}
					else
					{
						ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name", beneficiary.PurposeOfPayment);
					}

					Dropdown[] _SourceOfPaymentDDL = SourceOfPaymentDDL();
					ViewData["SourceOfPaymentDropdown"] = new SelectList(_SourceOfPaymentDDL, "val", "name", beneficiary.SourceOfPayment);

					Dropdown[] _BusinessCategoryDDL = BusinessCategoryDDL();
					ViewData["BusinessCategoryDropdown"] = new SelectList(_BusinessCategoryDDL, "val", "name", "optGroup", beneficiary.BeneficiaryBusinessCategory);

					Dropdown[] _NationalityDDL = NationalityDDL();
					ViewData["NationalityDropdown"] = new SelectList(_NationalityDDL, "val", "name", beneficiary.BeneficiaryNationality);

					ViewData["Beneficiaries"] = beneficiary;
					ViewData["BeneficiaryTypeIndividual"] = "";
					ViewData["BeneficiaryTypeBusiness"] = "";
					ViewData["BeneficiaryIsYourAccountNo"] = "";
					ViewData["BeneficiaryIsYourAccountYes"] = "";
					//ViewData["BeneficiaryBankTypeBankCode"] = "";
					//ViewData["BeneficiaryBankTypeSWIFT"] = "";

					if (form["beneficiary.Type"] != null)
					{
						if (form["beneficiary.Type"] == "0")
						{
							ViewData["BeneficiaryTypeIndividual"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryTypeBusiness"] = "checked";
						}
					}

					if (form["beneficiary.IsYourAccount"] != null)
					{
						if (form["beneficiary.IsYourAccount"] == "0")
						{
							ViewData["BeneficiaryIsYourAccountNo"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryIsYourAccountYes"] = "checked";
						}
					}

					//if (form["beneficiary.BankType"] != null)
					//{
					//	if (form["beneficiary.BankType"] == "1")
					//	{
					//		ViewData["BeneficiaryBankTypeBankCode"] = "checked";
					//	}
					//	else if (form["beneficiary.BankType"] == "2")
					//	{
					//		ViewData["BeneficiaryBankTypeSWIFT"] = "checked";
					//	}
					//}

					ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();

					TempData["Result"] = "danger|Something went wrong!";
					return View();
				}
			}
		}

		public ActionResult Edit(long id)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			if (id > 0)
			{
				using (var context = new DataAccess.GreatEastForex())
				{
					var getBeneficiaryData = context.Beneficiaries.Where(e => e.ID == id).FirstOrDefault();

					if (getBeneficiaryData != null)
					{
						ViewBag.Beneficiary = "active";
						Dropdown[] _CustomerDDL = CustomerDDL(getBeneficiaryData.CustomerParticularId);
						ViewData["CustomerDropdown"] = new SelectList(_CustomerDDL, "val", "name", getBeneficiaryData.CustomerParticularId);

						Dropdown[] _BankCountryDDL = CountryDDL();
						ViewData["BankCountryDropdown"] = new SelectList(_BankCountryDDL, "val", "name", getBeneficiaryData.BankCountry);

						Dropdown[] _PurposeOfPaymentDDL = PurposeOfPaymentDDL(getBeneficiaryData.Type);

						if (getBeneficiaryData.PurposeOfPayment == 0)
						{
							ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name");
						}
						else
						{
							ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name", getBeneficiaryData.PurposeOfPayment);
						}

						Dropdown[] _SourceOfPaymentDDL = SourceOfPaymentDDL();
						ViewData["SourceOfPaymentDropdown"] = new SelectList(_SourceOfPaymentDDL, "val", "name", getBeneficiaryData.SourceOfPayment);

						Dropdown[] _BusinessCategoryDDL = BusinessCategoryDDL();
						ViewData["BusinessCategoryDropdown"] = new SelectList(_BusinessCategoryDDL, "val", "name", "optGroup", getBeneficiaryData.BeneficiaryBusinessCategory);

						Dropdown[] _NationalityDDL = NationalityDDL();
						ViewData["NationalityDropdown"] = new SelectList(_NationalityDDL, "val", "name", getBeneficiaryData.BeneficiaryNationality);

						ViewData["Beneficiaries"] = getBeneficiaryData;
						ViewData["BeneficiaryID"] = id;
						ViewData["BeneficiaryTypeIndividual"] = "";
						ViewData["BeneficiaryTypeBusiness"] = "";
						ViewData["BeneficiaryIsYourAccountNo"] = "";
						ViewData["BeneficiaryIsYourAccountYes"] = "";
						//ViewData["BeneficiaryBankTypeBankCode"] = "";
						//ViewData["BeneficiaryBankTypeSWIFT"] = "";

						if (getBeneficiaryData.Type == 0)
						{
							ViewData["BeneficiaryTypeIndividual"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryTypeBusiness"] = "checked";
						}

						if (getBeneficiaryData.IsYourAccount == 0)
						{
							ViewData["BeneficiaryIsYourAccountNo"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryIsYourAccountYes"] = "checked";
						}

						//if (getBeneficiaryData.BankType == 1)
						//{
						//	ViewData["BeneficiaryBankTypeBankCode"] = "checked";
						//}
						//else if (getBeneficiaryData.BankType == 2)
						//{
						//	ViewData["BeneficiaryBankTypeSWIFT"] = "checked";
						//}

						
						ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
						return View();
					}
					else
					{
						TempData["Result"] = "danger|Beneficiary record not found!";
						return RedirectToAction("Listing", "Beneficiaries");
					}
				}
			}
			else
			{
				TempData["Result"] = "danger|Something went wrong!";
				return RedirectToAction("Listing", "Beneficiaries");
			}
		}

		[HttpPost]
		public ActionResult Edit(long id, Beneficiaries beneficiary, FormCollection form)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			if (id > 0)
			{
				using (var context = new DataAccess.GreatEastForex())
				{
					if (form["beneficiary.CustomerParticularId"] == null && string.IsNullOrEmpty(form["beneficiary.CustomerParticularId"]))
					{
						ModelState.AddModelError("beneficiary.CustomerParticularId", "Please select customer.");
					}

					if (form["beneficiary.Type"] == null)
					{
						ModelState.AddModelError("beneficiary.Type", "Please select who do you want to send the money to.");
					}
					else
					{
						if (form["beneficiary.Type"] != "0" && form["beneficiary.Type"] != "1")
						{
							ModelState.AddModelError("beneficiary.Type", "Something went wrong in select who do you want to send the money to.");
						}
					}

					if (form["beneficiary.IsYourAccount"] == null)
					{
						ModelState.AddModelError("beneficiary.IsYourAccount", "Please select is this your account.");
					}
					else
					{
						//check integer to prevent user backend change the value
						if (form["beneficiary.IsYourAccount"] != "1" && form["beneficiary.IsYourAccount"] != "0")
						{
							ModelState.AddModelError("beneficiary.IsYourAccount", "Something went wrong in select this your account.");
						}
					}

					if (string.IsNullOrEmpty(beneficiary.BeneficiaryFriendlyName))
					{
						ModelState.AddModelError("beneficiary.BeneficiaryFriendlyName", "Please enter your beneficiary friendly name.");
					}
					else
					{
						//check have same friendly name or not
						int customerId = beneficiary.CustomerParticularId;
						string beneficiaryName = beneficiary.BeneficiaryFriendlyName;

						if (context.Beneficiaries.Where(e => e.CustomerParticularId == customerId && e.BeneficiaryFriendlyName == beneficiaryName && e.ID != id).FirstOrDefault() != null)
						{
							ModelState.AddModelError("beneficiary.BeneficiaryFriendlyName", "Beneficiary Friendly Name must unique.");
						}
					}

					//if (form["beneficiary.BankType"] != null)
					//{
					//	if (form["beneficiary.BankType"] != "1" && form["beneficiary.BankType"] != "2")
					//	{
					//		ModelState.AddModelError("beneficiary.BankType", "Something went wrong in select Bank Type.");
					//	}
					//}

					//Check Bank Country
					if (form["beneficiary.Country"] != null && !string.IsNullOrEmpty(form["beneficiary.Country"]))
					{
						int CountryID = Convert.ToInt32(form["beneficiary.Country"]);
						var CheckCountry = context.Countries.Where(e => e.ID == CountryID && e.IsDeleted == 0).FirstOrDefault();
						if (CheckCountry != null)
						{
							if (CheckCountry.Name == "Others")
							{
								if (string.IsNullOrEmpty(form["beneficiary.BankCountryIfOthers"]))
								{
									ModelState.AddModelError("beneficiary.BankCountryIfOthers", "Bank Country If Others cannot be empty.");
								}
								else
								{
									if (form["beneficiary.BankCountryIfOthers"].ToString().Length > 250)
									{
										ModelState.AddModelError("beneficiary.BankCountryIfOthers", "Bank Country If Others cannot more than 250 characters.");
									}
								}
							}
						}
					}

					if (form["beneficiary.Type"] != "0" && form["beneficiary.Type"] != "1")
					{
					}
					else
					{
						if (form["beneficiary.Type"] == "0")
						{
							//Check Nationality
							if (form["beneficiary.Nationality"] != null && !string.IsNullOrEmpty(form["beneficiary.Nationality"]))
							{
								int NationalityID = Convert.ToInt32(form["beneficiary.Nationality"]);
								var Checknationality = context.Nationalities.Where(e => e.ID == NationalityID && e.IsDeleted == 0).FirstOrDefault();
								if (Checknationality != null)
								{
									if (Checknationality.Name == "Others")
									{
										if (string.IsNullOrEmpty(form["beneficiary.BeneficiaryNationalityIfOthers"]))
										{
											ModelState.AddModelError("beneficiary.BeneficiaryNationalityIfOthers", "Nationality If Others cannot be empty.");
										}
										else
										{
											if (form["beneficiary.BeneficiaryNationalityIfOthers"].ToString().Length > 250)
											{
												ModelState.AddModelError("beneficiary.BeneficiaryNationalityIfOthers", "Nationality If Others cannot more than 250 characters.");
											}
										}
									}
								}
							}
						}
						else if (form["beneficiary.Type"] == "1")
						{
							//Check Business Category
							if (form["beneficiary.BusinessCategory"] != null && !string.IsNullOrEmpty(form["beneficiary.BusinessCategory"]))
							{
								int BusinessCategoryID = Convert.ToInt32(form["beneficiary.BusinessCategory"]);
								var CheckBusinessCategory = context.BusinessCategoriesLists.Where(e => e.ID == BusinessCategoryID && e.IsDeleted == 0).FirstOrDefault();
								if (CheckBusinessCategory != null)
								{
									if (CheckBusinessCategory.Name == "Others")
									{
										if (string.IsNullOrEmpty(form["beneficiary.BeneficiaryBusinessCategoryIfOthers"]))
										{
											ModelState.AddModelError("beneficiary.BeneficiaryBusinessCategoryIfOthers", "Beneficiary's Business Category If Others cannot be empty.");
										}
										else
										{
											if (form["beneficiary.BeneficiaryBusinessCategoryIfOthers"].ToString().Length > 250)
											{
												ModelState.AddModelError("beneficiary.BeneficiaryBusinessCategoryIfOthers", "Beneficiary's Business Category If Others cannot more than 250 characters.");
											}
										}
									}
								}
							}
						}
					}

					//Check Purpose of Payment
					if (form["beneficiary.PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.PurposeOfPayment"]))
					{
						int PurposeOfPaymentID = Convert.ToInt32(form["beneficiary.PurposeOfPayment"]);
						var CheckPurposeOfPaymentID = context.PaymentLists.Where(e => e.ID == PurposeOfPaymentID && e.IsDeleted == 0).FirstOrDefault();
						if (CheckPurposeOfPaymentID != null)
						{
							if (CheckPurposeOfPaymentID.Name == "Others")
							{
								if (string.IsNullOrEmpty(form["beneficiary.PurposeOfPaymentIfOthers"]))
								{
									ModelState.AddModelError("beneficiary.PurposeOfPaymentIfOthers", "Purpose of Payment If Others cannot be empty.");
								}
								else
								{
									if (form["beneficiary.PurposeOfPaymentIfOthers"].ToString().Length > 250)
									{
										ModelState.AddModelError("beneficiary.PurposeOfPaymentIfOthers", "Purpose of Payment If Others cannot more than 250 characters.");
									}
								}
							}
						}
					}
					else
					{
						//beneficiary.PurposeOfPayment
						ModelState["beneficiary.PurposeOfPayment"].Errors.Clear();
					}

					//Check Source of Payment
					if (form["beneficiary.SourceOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.SourceOfPayment"]))
					{
						int SourceOfPaymentID = Convert.ToInt32(form["beneficiary.SourceOfPayment"]);
						var CheckSourceOfPaymentID = context.FundLists.Where(e => e.ID == SourceOfPaymentID && e.IsDeleted == 0).FirstOrDefault();
						if (CheckSourceOfPaymentID != null)
						{
							if (CheckSourceOfPaymentID.Name == "Others")
							{
								if (string.IsNullOrEmpty(form["beneficiary.SourceOfPaymentIfOthers"]))
								{
									ModelState.AddModelError("beneficiary.SourceOfPaymentIfOthers", "Source of Payment If Others cannot be empty.");
								}
								else
								{
									if (form["beneficiary.SourceOfPaymentIfOthers"].ToString().Length > 250)
									{
										ModelState.AddModelError("beneficiary.SourceOfPaymentIfOthers", "Source of Payment If Others cannot more than 250 characters.");
									}
								}
							}
						}
					}

					if (ModelState.IsValid)
					{
						//start add into beneficiary part.
						int purposeOfPayment = 0;
						int nationality = 0;
						int businessCategory = 0;
						int Country = 0;
						int sourceOfPayment = 0;

						if (form["beneficiary.PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.PurposeOfPayment"]))
						{
							purposeOfPayment = Convert.ToInt32(form["beneficiary.PurposeOfPayment"].ToString());
						}

						if (form["beneficiary.BeneficiaryNationality"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryNationality"]))
						{
							nationality = Convert.ToInt32(form["beneficiary.BeneficiaryNationality"].ToString());
						}

						if (form["beneficiary.BeneficiaryBusinessCategory"] != null && !string.IsNullOrEmpty(form["beneficiary.BeneficiaryBusinessCategory"]))
						{
							businessCategory = Convert.ToInt32(form["beneficiary.BeneficiaryBusinessCategory"]);
						}

						if (form["beneficiary.BankCountry"] != null && !string.IsNullOrEmpty(form["beneficiary.BankCountry"]))
						{
							Country = Convert.ToInt32(form["beneficiary.BankCountry"]); ;
						}

						if (form["beneficiary.SourceOfPayment"] != null && !string.IsNullOrEmpty(form["beneficiary.SourceOfPayment"]))
						{
							sourceOfPayment = Convert.ToInt32(form["beneficiary.SourceOfPayment"]);
						}

						Beneficiaries updateRecord = context.Beneficiaries.Where(e => e.ID == id).FirstOrDefault();
						updateRecord.CustomerParticularId = Convert.ToInt32(beneficiary.CustomerParticularId);
						updateRecord.Type = Convert.ToInt32(form["beneficiary.Type"]);//0 = individual, 1 = business

						if (updateRecord.Type == 0)
						{
							//individual
							updateRecord.BeneficiaryNationality = nationality;
							updateRecord.BeneficiaryNationalityIfOthers = form["beneficiary.BeneficiaryNationalityIfOthers"];
							updateRecord.BeneficiaryCompanyRegistrationNo = null;
							updateRecord.BeneficiaryBusinessCategory = 0;
							updateRecord.BeneficiaryContactNo = null;
							updateRecord.BeneficiaryBusinessCategoryIfOthers = null;
						}
						else
						{
							//business
							updateRecord.BeneficiaryNationality = 0;//0 means dont have
							updateRecord.BeneficiaryCompanyRegistrationNo = form["beneficiary.BeneficiaryCompanyRegistrationNo"];
							updateRecord.BeneficiaryBusinessCategory = businessCategory;
							updateRecord.BeneficiaryContactNo = form["beneficiary.BeneficiaryContactNo"];
							updateRecord.BeneficiaryNationalityIfOthers = null;
							updateRecord.BeneficiaryBusinessCategoryIfOthers = form["beneficiary.BeneficiaryBusinessCategoryIfOthers"];
						}

						updateRecord.IsYourAccount = Convert.ToInt32(form["beneficiary.IsYourAccount"]);
						updateRecord.BeneficiaryFriendlyName = form["beneficiary.BeneficiaryFriendlyName"];
						updateRecord.BeneficiaryFullName = form["beneficiary.BeneficiaryFullName"];
						updateRecord.BeneficiaryContactNoMain = form["beneficiary.BeneficiaryContactNoMain"];
						updateRecord.BeneficiaryAddressMain = form["beneficiary.BeneficiaryAddressMain"];
						updateRecord.BeneficiaryBankName = form["beneficiary.BeneficiaryBankName"];
						updateRecord.IBANEuropeBSBAustralia = form["beneficiary.IBANEuropeBSBAustralia"];
						//updateRecord.BankType = Convert.ToInt32(form["beneficiary.BankType"]);
						updateRecord.BankCode = form["beneficiary.BankCode"];
						updateRecord.BankAccountNo = form["beneficiary.BankAccountNo"];
						updateRecord.BankCountry = Country;
						updateRecord.BankCountryIfOthers = form["beneficiary.BankCountryIfOthers"];
						updateRecord.BankAddress = form["beneficiary.BankAddress"];
						updateRecord.PurposeOfPayment = purposeOfPayment;
						updateRecord.SourceOfPayment = sourceOfPayment;
						updateRecord.PaymentDetails = form["beneficiary.PaymentDetails"];
						//updateRecord.BeneficiaryNationalityIfOthers = form["beneficiary.BeneficiaryNationalityIfOthers"];
						//updateRecord.BeneficiaryBusinessCategoryIfOthers = form["beneficiary.BeneficiaryBusinessCategoryIfOthers"];
						updateRecord.PurposeOfPaymentIfOthers = form["beneficiary.PurposeOfPaymentIfOthers"];
						updateRecord.SourceOfPaymentIfOthers = form["beneficiary.SourceOfPaymentIfOthers"];
						updateRecord.UpdatedOn = DateTime.Now;
						context.SaveChanges();

						TempData["Result"] = "sucess|Beneficiary [" + updateRecord.BeneficiaryFriendlyName  + "] updated successfully.";
						return RedirectToAction("Listing", "Beneficiaries");
					}
					else
					{
						ViewBag.Beneficiary = "active";
						Dropdown[] _CustomerDDL = CustomerDDL(beneficiary.CustomerParticularId);
						ViewData["CustomerDropdown"] = new SelectList(_CustomerDDL, "val", "name", beneficiary.CustomerParticularId);

						Dropdown[] _BankCountryDDL = CountryDDL();
						ViewData["BankCountryDropdown"] = new SelectList(_BankCountryDDL, "val", "name", beneficiary.BankCountry);

						Dropdown[] _PurposeOfPaymentDDL = PurposeOfPaymentDDL(beneficiary.Type);

						if (beneficiary.PurposeOfPayment == 0)
						{
							ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name");
						}
						else
						{
							ViewData["PurposeOfPaymentDropdown"] = new SelectList(_PurposeOfPaymentDDL, "val", "name", beneficiary.PurposeOfPayment);
						}

						Dropdown[] _SourceOfPaymentDDL = SourceOfPaymentDDL();
						ViewData["SourceOfPaymentDropdown"] = new SelectList(_SourceOfPaymentDDL, "val", "name", beneficiary.SourceOfPayment);

						Dropdown[] _BusinessCategoryDDL = BusinessCategoryDDL();
						ViewData["BusinessCategoryDropdown"] = new SelectList(_BusinessCategoryDDL, "val", "name", "optGroup", beneficiary.BeneficiaryBusinessCategory);

						Dropdown[] _NationalityDDL = NationalityDDL();
						ViewData["NationalityDropdown"] = new SelectList(_NationalityDDL, "val", "name", beneficiary.BeneficiaryNationality);

						ViewData["Beneficiaries"] = beneficiary;
						ViewData["BeneficiaryID"] = id;
						ViewData["BeneficiaryTypeIndividual"] = "";
						ViewData["BeneficiaryTypeBusiness"] = "";
						ViewData["BeneficiaryIsYourAccountNo"] = "";
						ViewData["BeneficiaryIsYourAccountYes"] = "";
						//ViewData["BeneficiaryBankTypeBankCode"] = "";
						//ViewData["BeneficiaryBankTypeSWIFT"] = "";

						if (beneficiary.Type == 0)
						{
							ViewData["BeneficiaryTypeIndividual"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryTypeBusiness"] = "checked";
						}

						if (beneficiary.IsYourAccount == 0)
						{
							ViewData["BeneficiaryIsYourAccountNo"] = "checked";
						}
						else
						{
							ViewData["BeneficiaryIsYourAccountYes"] = "checked";
						}

						//if (beneficiary.BankType == 1)
						//{
						//	ViewData["BeneficiaryBankTypeBankCode"] = "checked";
						//}
						//else if (beneficiary.BankType == 2)
						//{
						//	ViewData["BeneficiaryBankTypeSWIFT"] = "checked";
						//}

						ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();

						TempData["Result"] = "danger|Something went wrong!";
						return View();
					}
				}
			}
			else
			{
				TempData["Result"] = "danger|Something went wrong!";
				return RedirectToAction("Listing", "Beneficiaries");
			}
		}

		//GET: Delete
		public ActionResult Delete(int id)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData["Page"]);
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				Beneficiaries beneficiaries = _beneficiariesModel.GetSingleBeneficiaries(id);

				if (beneficiaries != null)
				{
					bool result = _beneficiariesModel.DeleteBeneficiaries(id);

					if (result)
					{
						int userid = Convert.ToInt32(Session["UserId"]);
						string tableAffected = "Beneficiaries";
						string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Beneficiary [" + beneficiaries.BeneficiaryFriendlyName + "]";

						bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

						TempData.Add("Result", "success|" + beneficiaries.BeneficiaryFriendlyName + " has been successfully deleted!");
					}
					else
					{
						TempData.Add("Result", "danger|An error occured while deleting beneficiary record!");
					}
				}
				else
				{
					TempData.Add("Result", "danger|Agent beneficiary not found!");
				}

				return RedirectToAction("Listing", new { @page = page });
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to delete this record!");
				return RedirectToAction("Index", "TaskList");
			}
		}

		//Customer Dropdown
		public Dropdown[] CustomerDDL(int id)
		{
			if (id > 0)
			{
				IList<CustomerParticular> GetAllActiveCustomer = _customersModel.GetAllByStatus("Active");

				var CheckCustomerIsInside = GetAllActiveCustomer.Where(e => e.ID == id).FirstOrDefault();

				if (CheckCustomerIsInside == null)
				{
					CustomerParticular GetSelectedCustomer = _customersModel.GetSingle3(id);

					if (GetSelectedCustomer != null)
					{
						GetAllActiveCustomer.Add(GetSelectedCustomer);
					}
				}

				Dropdown[] ddl = new Dropdown[GetAllActiveCustomer.Count];

				int count = 0;
				foreach (var item in GetAllActiveCustomer)
				{
					ddl[count] = new Dropdown { name = item.CustomerCode, val = item.ID.ToString() };
					count++;
				}

				return ddl;
			}
			else
			{
				IList<CustomerParticular> GetAllActiveCustomer = _customersModel.GetAllByStatus("Active");

				Dropdown[] ddl = new Dropdown[GetAllActiveCustomer.Count];

				int count = 0;
				foreach (var item in GetAllActiveCustomer)
				{
					ddl[count] = new Dropdown { name = item.CustomerCode, val = item.ID.ToString() };
					count++;
				}

				return ddl;
			}
		}

		//Country Dropdown
		public Dropdown[] CountryDDL()
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var getAllCountry = context.Countries.Where(e => e.IsDeleted == 0).ToList();

				if (getAllCountry.Count > 0)
				{
					Dropdown[] ddl = new Dropdown[getAllCountry.Count];

					int count = 0;
					foreach (var item in getAllCountry)
					{
						ddl[count] = new Dropdown { name = item.Name, val = item.ID.ToString() };
						count++;
					}
					return ddl;
				}
				else
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
			}
		}

		//Purpose of Payment Dropdown
		public Dropdown[] PurposeOfPaymentDDL(int id)
		{
			using (var context = new DataAccess.GreatEastForex())
			{

				if (id == 99)
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
				else
				{
					if (id >= 0)
					{
						var getAllPurposeOfPayment = context.PaymentLists.Where(e => e.IsDeleted == 0 && e.Type == id || e.Type == 3).ToList();

						if (getAllPurposeOfPayment.Count > 0)
						{
							Dropdown[] ddl = new Dropdown[getAllPurposeOfPayment.Count];

							int count = 0;
							foreach (var item in getAllPurposeOfPayment)
							{
								ddl[count] = new Dropdown { name = item.Name, val = item.ID.ToString() };
								count++;
							}
							return ddl;
						}
						else
						{
							Dropdown[] ddl = new Dropdown[0];
							return ddl;
						}
					}
					else
					{
						Dropdown[] ddl = new Dropdown[0];
						return ddl;
					}
				}
			}
		}

		//Source of Payment Dropdown
		public Dropdown[] SourceOfPaymentDDL()
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var getAllSourceOfPayment = context.FundLists.Where(e => e.IsDeleted == 0).ToList();

				if (getAllSourceOfPayment.Count > 0)
				{
					Dropdown[] ddl = new Dropdown[getAllSourceOfPayment.Count];

					int count = 0;
					foreach (var item in getAllSourceOfPayment)
					{
						ddl[count] = new Dropdown { name = item.Name, val = item.ID.ToString() };
						count++;
					}
					return ddl;
				}
				else
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
			}
		}

		//Business Category Dropdown
		public Dropdown[] BusinessCategoryDDL()
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var getAllBusinessCategory = context.BusinessCategoriesLists.Where(e => e.IsDeleted == 0).ToList();

				if (getAllBusinessCategory.Count > 0)
				{
					Dropdown[] ddl = new Dropdown[getAllBusinessCategory.Count];

					int count = 0;
					foreach (var item in getAllBusinessCategory)
					{
						ddl[count] = new Dropdown { name = item.Name, val = item.ID.ToString(), optGroup = item.Headers };
						count++;
					}
					return ddl;
				}
				else
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
			}
		}

		//Nationality Dropdown
		public Dropdown[] NationalityDDL()
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var getAllNationality = context.Nationalities.Where(e => e.IsDeleted == 0).ToList();

				if (getAllNationality.Count > 0)
				{
					Dropdown[] ddl = new Dropdown[getAllNationality.Count];

					int count = 0;
					foreach (var item in getAllNationality)
					{
						ddl[count] = new Dropdown { name = item.Name, val = item.ID.ToString() };
						count++;
					}
					return ddl;
				}
				else
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
			}
		}

		//Ajax call and get the different type of the payment list
		[HttpPost]
		public ActionResult GetPurposeOfPaymentList(int beneficiaryType)
		{
			response data = new response();
			data.success = false;

			if (beneficiaryType == 0 || beneficiaryType == 1)
			{
				using (var context = new DataAccess.GreatEastForex())
				{
					//0 = individual
					//1 = business
					data.paymentList = context.PaymentLists.Where(e => e.IsDeleted == 0 && e.Type == beneficiaryType || e.Type == 3).ToList();
					//if (beneficiaryType == "individual")
					//{
						
					//}
					//else
					//{
					//	data.paymentList = context.PaymentLists.Where(e => e.IsDeleted == 0 && e.Type == 1).ToList();
					//}
				}
				data.success = true;
			}

			return Json(new { res = data }, JsonRequestBehavior.AllowGet);
		}

		//This is Json Return
		public class response
		{
			public bool success { get; set; }
			public List<PaymentLists> paymentList { get; set; }
			public string error { get; set; }
		}
	}
}