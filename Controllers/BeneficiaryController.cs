using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GreatEastForex.Helper;
using DataAccess;
using GreatEastForex.Models;
using DataAccess.POCO;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    public class BeneficiaryController : ControllerBase
    {
        // GET: Beneficiary
        public ActionResult Index()
        {
            return RedirectToAction("Listing");
        }

        public ActionResult Listing()
        {
            ViewBag.Beneficiary = "active";

            //load Default Bootpag Number Default
            //1. BeneficiaryTotal
            int BeneficiaryTotal = 0;
            int userid = Convert.ToInt32(Session["CustomerId"]);

            using (var context = new DataAccess.GreatEastForex())
            {
                var checkValidate = context.CustomerParticulars.Where(e => e.ID == userid).FirstOrDefault();

                if (checkValidate.isKYCVerify == 0)
                {
                    TempData.Add("Result", "danger|Your KYC Profile is not yet verify!");
                    return RedirectToAction("Home", "Customer");
                }

                BeneficiaryTotal = context.Beneficiaries.Where(e => e.CustomerParticularId == userid).Count();
            }

            int totalPage = PaginationList(BeneficiaryTotal);

            ViewBag.BeneficiaryTotal = totalPage;
            ViewBag.BeneficiaryPage = 1;

            return View();
        }

        public ActionResult PartialListing(int page = 1, int pageTaken = 0, string keyword = "")
        {
            //Get skip take query
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            ViewData["BeneficiaryData"] = new List<BeneficiaryList>();
            long mainCustomerID = Convert.ToInt64(Session["CustomerId"]);
            int BeneficiaryTotal = 0;

            using (var context = new DataAccess.GreatEastForex())
            {
                var param1 = new SqlParameter();
                param1.ParameterName = "@id";
                param1.SqlDbType = SqlDbType.NVarChar;
                param1.SqlValue = mainCustomerID;

                var record = context.BeneficiaryLists.SqlQuery("dbo.BeneficiaryListingSearch @id", param1).ToList();

                if (!string.IsNullOrEmpty(keyword))
                {
                    record = record.Where(e => (e.BeneficiaryFullName ?? "").Contains(keyword) || (e.BeneficiaryFriendlyName ?? "").Contains(keyword)).ToList();
                }

                BeneficiaryTotal = record.Count();
                record = record.OrderByDescending(e => e.ID).Skip(pageTaken).Take(pageSize).ToList();

                //This is when that page is no record, then will go to previous page and show new record.
                if (record.Count == 0 && pageTaken >= pageSize)
                {
                    var param2 = new SqlParameter();
                    param2.ParameterName = "@id";
                    param2.SqlDbType = SqlDbType.NVarChar;
                    param2.SqlValue = mainCustomerID;

                    record = context.BeneficiaryLists.SqlQuery("dbo.BeneficiaryListingSearch @id", param2).ToList();

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        record = record.Where(e => (e.BeneficiaryFullName ?? "").Contains(keyword) || (e.BeneficiaryFriendlyName ?? "").Contains(keyword)).ToList();
                    }
                    page = page - 1;
                    pageTaken = (page - 1) * pageSize;
                    BeneficiaryTotal = record.Count();
                    record = record.OrderByDescending(e => e.ID).Skip(pageTaken).Take(pageSize).ToList();
                }
                ViewData["BeneficiaryData"] = record;
                int totalPage = PaginationList(BeneficiaryTotal);
                ViewBag.BeneficiaryTotal = totalPage;
                ViewBag.PageTaken = pageTaken;
                ViewBag.BeneficiaryPage = page;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, int currentPage = 1)
        {
            BeneficiaryDeleteReturn data = new BeneficiaryDeleteReturn();
            data.success = false;

            //Get skip take query
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            ViewData["BeneficiaryData"] = new Beneficiaries();
            int mainCustomerID = Convert.ToInt32(Session["CustomerId"]);
            int getCurrentPage = currentPage;
            data.currentPage = getCurrentPage;

            using (var context = new DataAccess.GreatEastForex())
            {
                var removeBeneficiary = context.Beneficiaries.Where(e => e.ID == id && e.CustomerParticularId == mainCustomerID).FirstOrDefault();
                context.Beneficiaries.Remove(removeBeneficiary);
                context.SaveChanges();
                data.success = true;
            }

            return Json(new { res = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Beneficiary = "active";
            ViewBag.NationalityList = new List<Nationalities>();
            ViewBag.BusinessCategoriesList = new List<BusinessCategoriesLists>();
            ViewBag.CountryList = new List<Countries>();
            ViewBag.PaymentList = new List<PaymentLists>();
            ViewBag.FundList = new List<FundLists>();
            ViewData["BeneficiaryFriendlyName"] = "";
            ViewData["BeneficiaryFullName"] = "";
            ViewData["BeneficiaryCompanyRegistrationNo"] = "";
            ViewData["BeneficiaryContactNo"] = "";
            ViewData["BankCode"] = "";
            ViewData["BankAccountNo"] = "";
            ViewData["BankAddress"] = "";
            ViewData["PaymentDetails"] = "";
            int userid = Convert.ToInt32(Session["CustomerId"]);

            using (var context = new DataAccess.GreatEastForex())
            {
                var checkValidate = context.CustomerParticulars.Where(e => e.ID == userid).FirstOrDefault();
                if (checkValidate != null)
                {
                    if (checkValidate.isKYCVerify == 0)
                    {
                        TempData.Add("Result", "danger|Your KYC Profile is not yet verify!");
                        return RedirectToAction("Home", "Customer");
                    }
                }


                ViewBag.NationalityList = context.Nationalities.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.BusinessCategoriesList = context.BusinessCategoriesLists.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.FundList = context.FundLists.Where(e => e.IsDeleted == 0).ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            if (form["beneficiary-destination"] == null)
            {
                ModelState.AddModelError("beneficiary-destination", "Please select who do you want to send the money to!");
            }
            else
            {
                if (form["beneficiary-destination"] != "individual" && form["beneficiary-destination"] != "business")
                {
                    ModelState.AddModelError("beneficiary-destination", "Something went wrong in select who do you want to send the money to!");
                }
            }

            if (form["beneficiary-youraccount"] == null)
            {
                ModelState.AddModelError("beneficiary-youraccount", "Please select is this your account!");
            }
            else
            {
                //check integer to prevent user backend change the value
                if (form["beneficiary-youraccount"] != "1" && form["beneficiary-youraccount"] != "0")
                {
                    ModelState.AddModelError("beneficiary-youraccount", "Something went wrong in select this your account!");
                }
            }

            if (string.IsNullOrEmpty(form["BeneficiaryFriendlyName"].Trim()) || form["BeneficiaryFriendlyName"].Trim() == null)
            {
                ModelState.AddModelError("BeneficiaryFriendlyName", "Please enter your beneficiary friendly name!");
            }
            else
            {
                //check have same friendly name or not
                using (var context = new DataAccess.GreatEastForex())
                {
                    int customerId = Convert.ToInt32(Session["CustomerId"]);
                    string beneficiaryName = form["BeneficiaryFriendlyName"];

                    if (context.Beneficiaries.Where(e => e.CustomerParticularId == customerId && e.BeneficiaryFriendlyName == beneficiaryName).FirstOrDefault() != null)
                    {
                        ModelState.AddModelError("BeneficiaryFriendlyName", "Beneficiary Friendly Name must unique!");
                    }
                }
            }

            if (form["BankType"] != null)
            {
                if (form["BankType"] != "1" && form["BankType"] != "2")
                {
                    ModelState.AddModelError("BankType", "Something went wrong in select Bank Type!");
                }
            }

            if (ModelState.IsValid)
            {
                //start add into beneficiary part.
                using (var context = new DataAccess.GreatEastForex())
                {
                    int purposeOfPayment = 0;
                    int nationality = 0;
                    int businessCategory = 0;
                    int Country = 0;
                    int sourceOfPayment = 0;

                    if (form["PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["PurposeOfPayment"]))
                    {
                        purposeOfPayment = Convert.ToInt32(form["PurposeOfPayment"].ToString());
                    }

                    if (form["Nationality"] != null && !string.IsNullOrEmpty(form["Nationality"]))
                    {
                        nationality = Convert.ToInt32(form["Nationality"].ToString());
                    }

                    if (form["BusinessCategory"] != null && !string.IsNullOrEmpty(form["BusinessCategory"]))
                    {
                        businessCategory = Convert.ToInt32(form["BusinessCategory"]);
                    }

                    if (form["Country"] != null && !string.IsNullOrEmpty(form["Country"]))
                    {
                        Country = Convert.ToInt32(form["Country"]); ;
                    }

                    if (form["SourceOfPayment"] != null && !string.IsNullOrEmpty(form["SourceOfPayment"]))
                    {
                        sourceOfPayment = Convert.ToInt32(form["SourceOfPayment"]);
                    }

                    Beneficiaries newRecord = new Beneficiaries();
                    newRecord.CustomerParticularId = Convert.ToInt32(Session["CustomerId"]);
                    newRecord.Type = (form["beneficiary-destination"] == "individual") ? 0 : 1;//0 = individual, 1 = business

                    if (newRecord.Type == 0)
                    {
                        //individual
                        newRecord.BeneficiaryNationality = nationality;
                        newRecord.BeneficiaryCompanyRegistrationNo = null;
                        newRecord.BeneficiaryBusinessCategory = 0;
                        newRecord.BeneficiaryContactNo = null;
                    }
                    else
                    {
                        //business
                        newRecord.BeneficiaryNationality = 0;//0 means dont have
                        newRecord.BeneficiaryCompanyRegistrationNo = form["BeneficiaryCompanyRegistrationNo"];
                        newRecord.BeneficiaryBusinessCategory = businessCategory;
                        newRecord.BeneficiaryContactNo = form["BeneficiaryContactNo"];
                    }

                    newRecord.IsYourAccount = Convert.ToInt32(form["beneficiary-youraccount"]);
                    newRecord.BeneficiaryFriendlyName = form["BeneficiaryFriendlyName"];
                    newRecord.BeneficiaryFullName = form["BeneficiaryFullName"];
                    newRecord.BankType = Convert.ToInt32(form["BankType"]);
                    newRecord.BankCode = form["BankCode"];
                    newRecord.BankAccountNo = form["BankAccountNo"];
                    newRecord.BankCountry = Country;
                    newRecord.BankAddress = form["BankAddress"];
                    newRecord.PurposeOfPayment = purposeOfPayment;
                    newRecord.SourceOfPayment = sourceOfPayment;
                    newRecord.PaymentDetails = form["PaymentDetails"];
                    newRecord.Status = "Active";
                    newRecord.CreatedOn = DateTime.Now;
                    newRecord.UpdatedOn = newRecord.CreatedOn;

                    context.Beneficiaries.Add(newRecord);
                    context.SaveChanges();

                    return RedirectToAction("SuccessPage", "Beneficiary");
                }
            }
            else
            {
                TempData["Result"] = "danger|Something went wrong in the form!";
            }

            ViewBag.Beneficiary = "active";
            ViewBag.NationalityList = new List<Nationalities>();
            ViewBag.BusinessCategoriesList = new List<BusinessCategoriesLists>();
            ViewBag.CountryList = new List<Countries>();
            ViewBag.PaymentList = new List<PaymentLists>();
            ViewBag.FundList = new List<FundLists>();
            ViewData["BeneficiaryFriendlyName"] = form["BeneficiaryFriendlyName"];
            ViewData["BeneficiaryFullName"] = form["BeneficiaryFullName"];
            ViewData["BeneficiaryCompanyRegistrationNo"] = form["BeneficiaryCompanyRegistrationNo"];
            ViewData["BeneficiaryContactNo"] = form["BeneficiaryContactNo"];
            ViewData["BankCode"] = form["BankCode"];
            ViewData["BankAccountNo"] = form["BankAccountNo"];
            ViewData["BankAddress"] = form["BankAddress"];
            ViewData["PaymentDetails"] = form["PaymentDetails"];
            ViewData["BankType"] = form["BankType"];
            ViewData["BeneficiaryDestination"] = form["beneficiary-destination"];
            ViewData["BeneficiaryAccount"] = form["beneficiary-youraccount"];
            ViewData["NationalitySelected"] = form["Nationality"];
            ViewData["BusinessCategorySelected"] = form["BusinessCategory"];
            ViewData["BankCountrySelected"] = form["Country"];
            ViewData["PurposeOfPaymentSelected"] = form["PurposeOfPayment"];
            ViewData["SourceOfPaymentSelected"] = form["SourceOfPayment"];

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.NationalityList = context.Nationalities.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.BusinessCategoriesList = context.BusinessCategoriesLists.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.FundList = context.FundLists.Where(e => e.IsDeleted == 0).ToList();
            }
            return View();
        }

        public ActionResult Edit(long id)
        {
            if (id > 0)
            {
                using (var context = new DataAccess.GreatEastForex())
                {
                    int userid = Convert.ToInt32(Session["CustomerId"]);
                    var checkValidate = context.CustomerParticulars.Where(e => e.ID == userid).FirstOrDefault();

                    if (checkValidate.isKYCVerify == 0)
                    {
                        TempData.Add("Result", "danger|Your KYC Profile is not yet verify!");
                        return RedirectToAction("Home", "Customer");
                    }

                    var getBeneficiaryData = context.Beneficiaries.Where(e => e.ID == id).FirstOrDefault();

                    if (getBeneficiaryData != null)
                    {
                        if (getBeneficiaryData.CustomerParticularId == Convert.ToInt32(Session["CustomerId"]))
                        {
                            ViewBag.Beneficiary = "active";
                            ViewBag.NationalityList = new List<Nationalities>();
                            ViewBag.BusinessCategoriesList = new List<BusinessCategoriesLists>();
                            ViewBag.CountryList = new List<Countries>();
                            ViewBag.PaymentList = new List<PaymentLists>();
                            ViewBag.FundList = new List<FundLists>();

                            ViewData["BeneficiaryFriendlyName"] = getBeneficiaryData.BeneficiaryFriendlyName;
                            ViewData["BeneficiaryFullName"] = getBeneficiaryData.BeneficiaryFullName;
                            ViewData["BeneficiaryCompanyRegistrationNo"] = (!string.IsNullOrEmpty(getBeneficiaryData.BeneficiaryCompanyRegistrationNo) ? getBeneficiaryData.BeneficiaryCompanyRegistrationNo : "");
                            ViewData["BeneficiaryContactNo"] = (!string.IsNullOrEmpty(getBeneficiaryData.BeneficiaryContactNo) ? getBeneficiaryData.BeneficiaryContactNo : "");
                            ViewData["BankCode"] = getBeneficiaryData.BankCode;
                            ViewData["BankAccountNo"] = getBeneficiaryData.BankAccountNo;
                            ViewData["BankAddress"] = getBeneficiaryData.BankAddress;
                            ViewData["PaymentDetails"] = getBeneficiaryData.PaymentDetails;
                            ViewData["BankType"] = getBeneficiaryData.BankType;
                            ViewData["BeneficiaryDestination"] = (getBeneficiaryData.Type == 0 ? "individual" : "business");
                            ViewData["BeneficiaryAccount"] = getBeneficiaryData.IsYourAccount;
                            ViewData["NationalitySelected"] = getBeneficiaryData.BeneficiaryNationality;
                            ViewData["BusinessCategorySelected"] = getBeneficiaryData.BeneficiaryBusinessCategory;
                            ViewData["BankCountrySelected"] = getBeneficiaryData.BankCountry;
                            ViewData["PurposeOfPaymentSelected"] = getBeneficiaryData.PurposeOfPayment;
                            ViewData["SourceOfPaymentSelected"] = getBeneficiaryData.SourceOfPayment;

                            ViewBag.NationalityList = context.Nationalities.Where(e => e.IsDeleted == 0).ToList();
                            ViewBag.BusinessCategoriesList = context.BusinessCategoriesLists.Where(e => e.IsDeleted == 0).ToList();
                            ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                            ViewBag.FundList = context.FundLists.Where(e => e.IsDeleted == 0).ToList();

                            return View();
                        }
                        else
                        {
                            TempData["Result"] = "danger|You are not allow to edit this beneficiary!";
                            return RedirectToAction("Listing", "Beneficiary");
                        }
                    }
                    else
                    {
                        TempData["Result"] = "danger|Beneficiary record not found!";
                        return RedirectToAction("Listing", "Beneficiary");
                    }
                }
            }
            else
            {
                TempData["Result"] = "danger|Something went wrong!";
                return RedirectToAction("Listing", "Beneficiary");
            }
        }

        [HttpPost]
        public ActionResult Edit(long id, FormCollection form)
        {
            if (form["beneficiary-destination"] == null)
            {
                ModelState.AddModelError("beneficiary-destination", "Please select who do you want to send the money to!");
            }
            else
            {
                if (form["beneficiary-destination"] != "individual" && form["beneficiary-destination"] != "business")
                {
                    ModelState.AddModelError("beneficiary-destination", "Something went wrong in select who do you want to send the money to!");
                }
            }

            if (form["beneficiary-youraccount"] == null)
            {
                ModelState.AddModelError("beneficiary-youraccount", "Please select is this your account!");
            }
            else
            {
                //check integer to prevent user backend change the value
                if (form["beneficiary-youraccount"] != "1" && form["beneficiary-youraccount"] != "0")
                {
                    ModelState.AddModelError("beneficiary-youraccount", "Something went wrong in select this your account!");
                }
            }

            if (string.IsNullOrEmpty(form["BeneficiaryFriendlyName"].Trim()) || form["BeneficiaryFriendlyName"].Trim() == null)
            {
                ModelState.AddModelError("BeneficiaryFriendlyName", "Please enter your beneficiary friendly name!");
            }
            else
            {
                //check have same friendly name or not
                using (var context = new DataAccess.GreatEastForex())
                {
                    int customerId = Convert.ToInt32(Session["CustomerId"]);
                    string beneficiaryName = form["BeneficiaryFriendlyName"];

                    if (context.Beneficiaries.Where(e => e.CustomerParticularId == customerId && e.BeneficiaryFriendlyName == beneficiaryName && e.ID != id).FirstOrDefault() != null)
                    {
                        ModelState.AddModelError("BeneficiaryFriendlyName", "Beneficiary Friendly Name must unique!");
                    }
                }
            }

            if (form["BankType"] != null)
            {
                if (form["BankType"] != "1" && form["BankType"] != "2")
                {
                    ModelState.AddModelError("BankType", "Something went wrong in select Bank Type!");
                }
            }

            if (ModelState.IsValid)
            {
                //start add into beneficiary part.
                using (var context = new DataAccess.GreatEastForex())
                {
                    int purposeOfPayment = 0;
                    int nationality = 0;
                    int businessCategory = 0;
                    int Country = 0;
                    int sourceOfPayment = 0;

                    if (form["PurposeOfPayment"] != null && !string.IsNullOrEmpty(form["PurposeOfPayment"]))
                    {
                        purposeOfPayment = Convert.ToInt32(form["PurposeOfPayment"].ToString());
                    }

                    if (form["Nationality"] != null && !string.IsNullOrEmpty(form["Nationality"]))
                    {
                        nationality = Convert.ToInt32(form["Nationality"].ToString());
                    }

                    if (form["BusinessCategory"] != null && !string.IsNullOrEmpty(form["BusinessCategory"]))
                    {
                        businessCategory = Convert.ToInt32(form["BusinessCategory"]);
                    }

                    if (form["Country"] != null && !string.IsNullOrEmpty(form["Country"]))
                    {
                        Country = Convert.ToInt32(form["Country"]); ;
                    }

                    if (form["SourceOfPayment"] != null && !string.IsNullOrEmpty(form["SourceOfPayment"]))
                    {
                        sourceOfPayment = Convert.ToInt32(form["SourceOfPayment"]);
                    }

                    Beneficiaries updateRecord = context.Beneficiaries.Where(e => e.ID == id).FirstOrDefault();
                    updateRecord.CustomerParticularId = Convert.ToInt32(Session["CustomerId"]);
                    updateRecord.Type = (form["beneficiary-destination"] == "individual") ? 0 : 1;//0 = individual, 1 = business

                    if (updateRecord.Type == 0)
                    {
                        //individual
                        updateRecord.BeneficiaryNationality = nationality;
                        updateRecord.BeneficiaryCompanyRegistrationNo = null;
                        updateRecord.BeneficiaryBusinessCategory = 0;
                        updateRecord.BeneficiaryContactNo = null;
                    }
                    else
                    {
                        //business
                        updateRecord.BeneficiaryNationality = 0;//0 means dont have
                        updateRecord.BeneficiaryCompanyRegistrationNo = form["BeneficiaryCompanyRegistrationNo"];
                        updateRecord.BeneficiaryBusinessCategory = businessCategory;
                        updateRecord.BeneficiaryContactNo = form["BeneficiaryContactNo"];
                    }

                    updateRecord.IsYourAccount = Convert.ToInt32(form["beneficiary-youraccount"]);
                    updateRecord.BeneficiaryFriendlyName = form["BeneficiaryFriendlyName"];
                    updateRecord.BeneficiaryFullName = form["BeneficiaryFullName"];
                    updateRecord.BankType = Convert.ToInt32(form["BankType"]);
                    updateRecord.BankCode = form["BankCode"];
                    updateRecord.BankAccountNo = form["BankAccountNo"];
                    updateRecord.BankCountry = Country;
                    updateRecord.BankAddress = form["BankAddress"];
                    updateRecord.PurposeOfPayment = purposeOfPayment;
                    updateRecord.SourceOfPayment = sourceOfPayment;
                    updateRecord.PaymentDetails = form["PaymentDetails"];
                    updateRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();

                    return RedirectToAction("UpdateSuccessPage", "Beneficiary");
                }
            }
            else
            {
                TempData["Result"] = "danger|Something went wrong in the form!";
            }

            ViewBag.Beneficiary = "active";
            ViewBag.NationalityList = new List<Nationalities>();
            ViewBag.BusinessCategoriesList = new List<BusinessCategoriesLists>();
            ViewBag.CountryList = new List<Countries>();
            ViewBag.PaymentList = new List<PaymentLists>();
            ViewBag.FundList = new List<FundLists>();
            ViewData["BeneficiaryFriendlyName"] = form["BeneficiaryFriendlyName"];
            ViewData["BeneficiaryFullName"] = form["BeneficiaryFullName"];
            ViewData["BeneficiaryCompanyRegistrationNo"] = form["BeneficiaryCompanyRegistrationNo"];
            ViewData["BeneficiaryContactNo"] = form["BeneficiaryContactNo"];
            ViewData["BankCode"] = form["BankCode"];
            ViewData["BankAccountNo"] = form["BankAccountNo"];
            ViewData["BankAddress"] = form["BankAddress"];
            ViewData["PaymentDetails"] = form["PaymentDetails"];
            ViewData["BankType"] = form["BankType"];
            ViewData["BeneficiaryDestination"] = form["beneficiary-destination"];
            ViewData["BeneficiaryAccount"] = form["beneficiary-youraccount"];
            ViewData["NationalitySelected"] = form["Nationality"];
            ViewData["BusinessCategorySelected"] = form["BusinessCategory"];
            ViewData["BankCountrySelected"] = form["Country"];
            ViewData["PurposeOfPaymentSelected"] = form["PurposeOfPayment"];
            ViewData["SourceOfPaymentSelected"] = form["SourceOfPayment"];

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.NationalityList = context.Nationalities.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.BusinessCategoriesList = context.BusinessCategoriesLists.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.FundList = context.FundLists.Where(e => e.IsDeleted == 0).ToList();
            }
            return View();
        }

        public ActionResult SuccessPage()
        {
            ViewBag.Beneficiary = "active";
            return View();
        }

        public ActionResult UpdateSuccessPage()
        {
            ViewBag.Beneficiary = "active";
            return View();
        }

        //Ajax call and get the different type of the payment list
        [HttpPost]
        public ActionResult GetPurposeOfPaymentList(string beneficiaryType)
        {
            response data = new response();
            data.success = false;

            if (beneficiaryType == "individual" || beneficiaryType == "business")
            {
                using (var context = new DataAccess.GreatEastForex())
                {
                    //0 = individual
                    //1 = business
                    if (beneficiaryType == "individual")
                    {
                        data.paymentList = context.PaymentLists.Where(e => e.IsDeleted == 0 && e.Type == 0).ToList();
                    }
                    else
                    {
                        data.paymentList = context.PaymentLists.Where(e => e.IsDeleted == 0 && e.Type == 1).ToList();
                    }
                }
                data.success = true;
            }

            return Json(new { res = data }, JsonRequestBehavior.AllowGet);
        }

        //Ajax call and get the Beneficiary Record selected from Remittance module.
        [HttpPost]
        public ActionResult GetBeneficiaryRecord(int id)
        {
            responseBeneficiary data = new responseBeneficiary();
            data.success = false;

            if (id > 0)
            {
                using (var context = new DataAccess.GreatEastForex())
                {
                    data.beneficiaryRecord = context.Beneficiaries.Where(e => e.ID == id).FirstOrDefault();
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

        //This is Json Return
        public class responseBeneficiary
        {
            public bool success { get; set; }
            public Beneficiaries beneficiaryRecord { get; set; }
            public string error { get; set; }
        }

        //This is Return Delete Json
        public class BeneficiaryDeleteReturn
        {
            public bool success { get; set; }
            public int currentPage { get; set; }

            public string keyword { get; set; }
            public string error { get; set; }
        }

        public int PaginationList(int Count)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int totalPage = 0;
            var pageCount = Count;
            int i = (int)pageCount % pageSize;

            if (i > 0)
            {
                totalPage = (int)pageCount / pageSize + 1;
            }
            else
            {
                totalPage = (int)pageCount / pageSize;
            }

            ViewBag.totalPage = totalPage;

            return totalPage;
        }
    }
}