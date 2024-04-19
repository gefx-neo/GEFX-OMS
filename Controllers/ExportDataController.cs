using DataAccess.POCO;
using GreatEastForex.Models;
using OfficeOpenXml;
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
    [RedirectingAction]
    [RedirectingActionWithSuperAdmin]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ExportDataController : ControllerBase
    {
        private ICustomerParticularRepository _customerParticularsModel;
        private IProductRepository _productsModel;
        private ISaleRepository _salesModel;
        private IUserRepository _usersModel;
        private int sgdDp = 2;
        private int rateDP = 6;

        public ExportDataController()
            : this(new CustomerParticularRepository(), new ProductRepository(), new SaleRepository(), new UserRepository())
        {

        }

        public ExportDataController(ICustomerParticularRepository customerParticularsModel, IProductRepository productsModel, ISaleRepository salesModel, IUserRepository usersModel)
        {
            _customerParticularsModel = customerParticularsModel;
            _productsModel = productsModel;
            _salesModel = salesModel;
            _usersModel = usersModel;
            Product sgd = _productsModel.FindCurrencyCode("SGD");
            sgdDp = sgd.Decimal;
            ViewData["SGDDP"] = GetDecimalFormat(sgdDp);
            ViewData["RateDP"] = GetDecimalFormat(rateDP);
        }

        // GET: ExportData
        public ActionResult Index()
        {
            if (TempData["TableName"] != null)
            {
                TempData.Remove("TableName");
            }

            if (TempData["FromDate"] != null)
            {
                TempData.Remove("FromDate");
            }

            if (TempData["ToDate"] != null)
            {
                TempData.Remove("ToDate");
            }

            return RedirectToAction("Options");
        }

        //GET: Options
        public ActionResult Options()
        {
            ViewData["TableName"] = "";
            if (TempData["TableName"] != null)
            {
                ViewData["TableName"] = TempData["TableName"].ToString();
            }

            ViewData["FromDate"] = "";
            if (TempData["FromDate"] != null)
            {
                ViewData["FromDate"] = TempData["FromDate"].ToString();
            }

            ViewData["ToDate"] = "";
            if (TempData["ToDate"] != null)
            {
                ViewData["ToDate"] = TempData["ToDate"].ToString();
            }

            Dropdown[] tableNameDDL = TableNameDDL();
            ViewData["TableNameDropdown"] = new SelectList(tableNameDDL, "val", "name", ViewData["TableName"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Options
        [HttpPost]
        public ActionResult Options(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["TableName"]))
            {
                ModelState.AddModelError("TableName", "Table Name is required!");
            }

            if (!string.IsNullOrEmpty(form["FromDate"]) && !string.IsNullOrEmpty(form["ToDate"]))
            {
                DateTime fromDate = Convert.ToDateTime(form["fromDate"]);
                DateTime toDate = Convert.ToDateTime(form["ToDate"]);

                if (fromDate > toDate)
                {
                    ModelState.AddModelError("FromDate", "From Date cannot be later than To Date!");
                    ModelState.AddModelError("ToDate", "To Date cannot be earlier than From Date!");
                }
            }

            if (ModelState.IsValid)
            {
                string tableName = form["TableName"].ToString();
                TempData["TableName"] = tableName;

                string fromDate = form["FromDate"].ToString();
                TempData["FromDate"] = fromDate;

                string toDate = form["ToDate"].ToString();
                TempData["ToDate"] = toDate;

                if (TempData["PageSize"] != null)
                {
                    TempData.Remove("PageSize");
                }

                switch (tableName)
                {
                    case "Customers": return RedirectToAction("Customer");
                    case "Product/Inventory": return RedirectToAction("ProductInventory");
                    case "Sales": return RedirectToAction("Sale");
                    case "Users": return RedirectToAction("User");
                    default: TempData.Add("Result", "danger|Table Name not found!"); break;
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            ViewData["TableName"] = form["TableName"].ToString();
            TempData["TableName"] = form["TableName"].ToString();

            ViewData["FromDate"] = form["FromDate"].ToString();
            TempData["FromDate"] = form["FromDate"].ToString();

            ViewData["ToDate"] = form["ToDate"].ToString();
            TempData["ToDate"] = form["ToDate"].ToString();

            Dropdown[] tableNameDDL = TableNameDDL();
            ViewData["TableNameDropdown"] = new SelectList(tableNameDDL, "val", "name", ViewData["TableName"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Customer Data
        public ActionResult Customer(int page = 1)
        {
            int pageSize = 40;
            if (TempData["PageSize"] != null)
            {
                pageSize = Convert.ToInt32(TempData["PageSize"]);
            }

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            TempData["Page"] = page;
            ViewData["Page"] = page;

            string from = "";

            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            else
            {
                TempData["FromDate"] = "";
            }

            ViewData["FromDate"] = from;
            TempData.Keep("FromDate");

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            else
            {
                TempData["ToDate"] = "";
            }
            ViewData["ToDate"] = to;
            TempData.Keep("ToDate");

            IPagedList<CustomerParticular> customers = _customerParticularsModel.GetCustomerDataPaged(from, to, page, pageSize);
			ViewData["AllCustomerParticular"] = new List<CustomerParticular>();
			ViewData["AllSearchTags"] = new List<SearchTags>();

			using (var context = new DataAccess.GreatEastForex())
			{
				ViewData["AllCustomerParticular"] = context.CustomerParticulars.ToList();
				ViewData["AllSearchTags"] = context.SearchTags.ToList();
			}

			ViewData["CustomerParticular"] = customers;
            ViewData["CustomerParticularCompany"] = customers.Where(e => e.CustomerType == "Corporate & Trading Company").ToList();
            ViewData["CustomerParticularNatural"] = customers.Where(e => e.CustomerType == "Natural Person").ToList();

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Customer Data
        [HttpPost]
        public ActionResult Customer(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(form["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            ViewData["FromDate"] = from;
            TempData.Keep("FromDate");

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            ViewData["ToDate"] = to;
            TempData.Keep("ToDate");

            IPagedList<CustomerParticular> customers = _customerParticularsModel.GetCustomerDataPaged(from, to, page, pageSize);
            ViewData["CustomerParticular"] = customers;

			ViewData["AllCustomerParticular"] = new List<CustomerParticular>();
			ViewData["AllSearchTags"] = new List<SearchTags>();

			using (var context = new DataAccess.GreatEastForex())
			{
				ViewData["AllCustomerParticular"] = context.CustomerParticulars.ToList();
				ViewData["AllSearchTags"] = context.SearchTags.ToList();
			}

			ViewData["CustomerParticularCompany"] = customers.Where(e=>e.CustomerType == "Corporate & Trading Company").ToList();
            ViewData["CustomerParticularNatural"] = customers.Where(e => e.CustomerType == "Natural Person").ToList();

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Export Customer Excel
        public void ExportCustomerExcel(string fromDate, string toDate)
        {
            IList<CustomerParticular> customers = _customerParticularsModel.GetAllCustomers(fromDate, toDate);

            IList<CustomerParticular> customerCompany = customers.Where(e => e.CustomerType == "Corporate & Trading Company").ToList();
            IList<CustomerParticular> customerNatural = customers.Where(e => e.CustomerType == "Natural Person").ToList();

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Corporate & Trading Company
                //Create Customer Particulars Worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Customer Particulars");

                //set first row name
                ws.Cells["A1:AP1"].Merge = true;
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                ws.Cells[1, 1].Value = "Corporate & Trading Company";
                ws.Cells[2, 1].Value = "Customer Code";
                ws.Cells[2, 2].Value = "Customer Type";
                ws.Cells[2, 3].Value = "Registered Name";
                //ws.Cells[2, 4].Value = "Registered Address";
                ws.Cells[2, 4].Value = "Business Address 1";
                ws.Cells[2, 5].Value = "Business Address 2";
                ws.Cells[2, 6].Value = "Postal Code";
                ws.Cells[2, 7].Value = "Contact Name";
                ws.Cells[2, 8].Value = "Tel No";
                ws.Cells[2, 9].Value = "Fax No";
                ws.Cells[2, 10].Value = "Email";
				ws.Cells[2, 11].Value = "Sales Remarks";

				ws.Cells[2, 12].Value = "Place of Registration";
                ws.Cells[2, 13].Value = "Date of Registration";
                ws.Cells[2, 14].Value = "Type of Entity";
                ws.Cells[2, 15].Value = "Type of Entity If Others";
                ws.Cells[2, 16].Value = "Purpose and Intended";
                ws.Cells[2, 17].Value = "Source of Funds";
                ws.Cells[2, 18].Value = "Source of Funds If Others";
                ws.Cells[2, 19].Value = "Is the benficial owner or has the beneficial owner ever been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[2, 20].Value = "Is the benficial owner or has the beneficial owner ever been a parent/ step-parent /step-child, adopted child/ spouse/ sibling/ step-sibling/ adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[2, 21].Value = "Is the beneficial owner or has the beneficial owner ever been closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[2, 22].Value = "Agent Acting";
                ws.Cells[2, 23].Value = "Customer Type";
                ws.Cells[2, 24].Value = "Address";
                ws.Cells[2, 25].Value = "Place of Registration";
                ws.Cells[2, 26].Value = "Registration No";
                ws.Cells[2, 27].Value = "Date of Registration";
                ws.Cells[2, 28].Value = "Relationship between Agent(s) and Client";
                ws.Cells[2, 29].Value = "Basis of Authority";
                ws.Cells[2, 30].Value = "Account Opening form";
                ws.Cells[2, 31].Value = "Photocopy of IC (Authorised Trading Person)";
                ws.Cells[2, 32].Value = "Photocopy of IC (Director)";
                ws.Cells[2, 33].Value = "Company business profile from ACRA";
                ws.Cells[2, 34].Value = "Approval By";
                ws.Cells[2, 35].Value = "Screening Results";
                ws.Cells[2, 36].Value = "Grading";
                ws.Cells[2, 37].Value = "Next Review Date";
                ws.Cells[2, 38].Value = "Acra Expiry";

				ws.Cells[2, 39].Value = "Bank Account No.";
				ws.Cells[2, 40].Value = "GM Approval Above";
                ws.Cells[2, 41].Value = "Status";
                ws.Cells[2, 42].Value = "Created On";
                ws.Cells[2, 43].Value = "Updated On";
                ws.Cells[2, 44].Value = "Is Deleted";

                //Create Appointments of Staff Worksheet
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Customer Appointments");

                //set first row name
                ws1.Cells[1, 1].Value = "Customer Code";
                ws1.Cells[1, 2].Value = "Full Name";
                ws1.Cells[1, 3].Value = "IC/Passport No";
                ws1.Cells[1, 4].Value = "Nationality";
                ws1.Cells[1, 5].Value = "Job Title";
                ws1.Cells[1, 6].Value = "Specimen Signature";

                //Create Sanctions and PEP Screening Report Worksheet
                ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Sanctions & PEP Screening Report");
                ws3.Cells[1, 1].Value = "Customer Code";
                ws3.Cells[1, 2].Value = "Date";
                ws3.Cells[1, 3].Value = "Date of ACRA";
                ws3.Cells[1, 4].Value = "Screened By";
                ws3.Cells[1, 5].Value = "Screening of all Directors, Secretary and Shareholders on Acra";
                ws3.Cells[1, 6].Value = "Screening of all Authorized Persons in Account Opening Form and/or Update Form";
                ws3.Cells[1, 7].Value = "Remarks";

                //Create Custom Rates Worksheet
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Customer Custom Rates");

                //set first row name
                ws2.Cells[1, 1].Value = "Customer Code";
                ws2.Cells[1, 2].Value = "Currency Code";
                ws2.Cells[1, 3].Value = "Buy Rate Adjustment";
                ws2.Cells[1, 4].Value = "Sell Rate Adjustment";
                ws2.Cells[1, 5].Value = "Encashment Rate";

                int particularRow = 3;
                int appointmentRow = 2;
                int screeningReportRow = 2;
                int customRateRow = 2;

                foreach (CustomerParticular customer in customerCompany)
                {
                    //Dump Customer Particulars Data
                    ws.Cells[particularRow, 1].Value = customer.CustomerCode;
                    ws.Cells[particularRow, 2].Value = customer.CustomerType;
                    ws.Cells[particularRow, 3].Value = customer.Company_RegisteredName;
                    //ws.Cells[particularRow, 4].Value = customer.Company_RegisteredAddress;
                    ws.Cells[particularRow, 4].Value = customer.Company_BusinessAddress1;
                    ws.Cells[particularRow, 5].Value = customer.Company_BusinessAddress2;
                    ws.Cells[particularRow, 6].Value = customer.Company_PostalCode;
                    ws.Cells[particularRow, 7].Value = customer.Company_ContactName;
                    ws.Cells[particularRow, 8].Value = customer.Company_TelNo;
                    ws.Cells[particularRow, 9].Value = customer.Company_FaxNo;
                    ws.Cells[particularRow, 10].Value = customer.Company_Email;
					ws.Cells[particularRow, 11].Value = "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].SalesRemarks != null)
						{
							ws.Cells[particularRow, 11].Value = customer.Others[0].SalesRemarks;
						}
					}

					ws.Cells[particularRow, 12].Value = customer.Company_PlaceOfRegistration;
                    ws.Cells[particularRow, 13].Value = Convert.ToDateTime(customer.Company_DateOfRegistration).ToString("dd/MM/yyyy");
                    ws.Cells[particularRow, 14].Value = customer.Company_TypeOfEntity;
                    ws.Cells[particularRow, 15].Value = customer.Company_TypeOfEntityIfOthers;
                    ws.Cells[particularRow, 16].Value = customer.Company_PurposeAndIntended;
                    ws.Cells[particularRow, 17].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_SourceOfFund : "";
                    ws.Cells[particularRow, 18].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_SourceOfFundIfOthers : "";
                    ws.Cells[particularRow, 19].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_1 : "";
                    ws.Cells[particularRow, 20].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_2 : "";
                    ws.Cells[particularRow, 21].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_3 : "";
                    ws.Cells[particularRow, 22].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].ActingAgent : "";
                    ws.Cells[particularRow, 23].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_CustomerType : "";
                    ws.Cells[particularRow, 24].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_Address : "";
                    ws.Cells[particularRow, 25].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_PlaceOfRegistration : "";
                    ws.Cells[particularRow, 26].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_RegistrationNo : "";

                    if(customer.ActingAgents.Count > 0)
                    {
                        if (customer.ActingAgents[0].Company_DateOfRegistration != null)
                        {
                            ws.Cells[particularRow, 27].Value = Convert.ToDateTime(customer.ActingAgents[0].Company_DateOfRegistration).ToString("dd/MM/yyyy");
                        }
                    }

                    ws.Cells[particularRow, 28].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Relationship : "";
                    ws.Cells[particularRow, 29].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].BasisOfAuthority : "";
                    ws.Cells[particularRow, 30].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_AccountOpeningForm : "";
                    ws.Cells[particularRow, 31].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_ICWithAuthorizedTradingPersons : "";
                    ws.Cells[particularRow, 32].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_ICWithDirectors : "";
                    ws.Cells[particularRow, 33].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_BusinessProfileFromAcra : "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].ApprovalBy != 0)
						{
							ws.Cells[particularRow, 34].Value = customer.Others[0].Users.Name;
						}
						else
						{
							ws.Cells[particularRow, 34].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 34].Value = "";
					}

                    //ws.Cells[particularRow, 34].Value = customer.Others.Count > 0 ? customer.Others[0].Users.Name : "";
                    ws.Cells[particularRow, 35].Value = customer.Others.Count > 0 ? customer.Others[0].ScreeningResults : "";
                    ws.Cells[particularRow, 36].Value = customer.Others.Count > 0 ? customer.Others[0].Grading : "";

                    if(customer.Others.Count > 0)
                    {
                        if (customer.Others[0].NextReviewDate != null)
                        {
                            ws.Cells[particularRow, 37].Value = Convert.ToDateTime(customer.Others[0].NextReviewDate).ToString("dd/MM/yyyy");
                        }
                        if (customer.Others[0].AcraExpiry != null)
                        {
                            ws.Cells[particularRow, 38].Value = Convert.ToDateTime(customer.Others[0].AcraExpiry).ToString("dd/MM/yyyy");
                        }
                    }

					if (customer.Others.Count > 0)
					{
						if (!string.IsNullOrEmpty(customer.Others[0].BankAccountNo))
						{
							ws.Cells[particularRow, 39].Value = customer.Others[0].BankAccountNo.Replace("|", ",");
						}
						else
						{
							ws.Cells[particularRow, 39].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 39].Value = "";
					}

					//ws.Cells[particularRow, 39].Value = customer.Others.Count > 0 ? customer.Others[0].BankAccountNo.Replace("|",",") : "";
					ws.Cells[particularRow, 40].Value = customer.Others.Count > 0 ? customer.Others[0].GMApprovalAbove : 0;
                    ws.Cells[particularRow, 41].Value = customer.Others.Count > 0 ? customer.Others[0].Status : "";
                    ws.Cells[particularRow, 42].Value = customer.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[particularRow, 43].Value = customer.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[particularRow, 44].Value = customer.IsDeleted;
                    particularRow++;

                    foreach (CustomerAppointmentOfStaff appointment in customer.AppointmentOfStaffs)
                    {
                        //Dump Customer Appointment Data
                        ws1.Cells[appointmentRow, 1].Value = appointment.CustomerParticulars.CustomerCode;
                        ws1.Cells[appointmentRow, 2].Value = appointment.FullName;
                        ws1.Cells[appointmentRow, 3].Value = appointment.ICPassportNo;
                        ws1.Cells[appointmentRow, 4].Value = appointment.Nationality;
                        ws1.Cells[appointmentRow, 5].Value = appointment.JobTitle;
                        ws1.Cells[appointmentRow, 6].Value = appointment.SpecimenSignature;
                        appointmentRow++;
                    }

                    foreach (CustomerScreeningReport screeningReport in customer.PEPScreeningReports)
                    {
                        //Dump Customer Sanctions and PEP Screening Report
                        ws3.Cells[screeningReportRow, 1].Value = customer.CustomerCode;
                        ws3.Cells[screeningReportRow, 2].Value = screeningReport.Date.ToString("dd/MM/yyyy");
                        ws3.Cells[screeningReportRow, 3].Value = screeningReport.DateOfAcra.ToString("dd/MM/yyyy");
                        ws3.Cells[screeningReportRow, 4].Value = screeningReport.ScreenedBy;
                        ws3.Cells[screeningReportRow, 5].Value = screeningReport.ScreeningReport_1;
                        ws3.Cells[screeningReportRow, 6].Value = screeningReport.ScreeningReport_2;
                        ws3.Cells[screeningReportRow, 7].Value = screeningReport.Remarks;
                        screeningReportRow++;
                    }

                    foreach (CustomerCustomRate rate in customer.CustomRates)
                    {
                        //Dump Customer Custom Rate Data
                        ws2.Cells[customRateRow, 1].Value = rate.CustomerParticulars.CustomerCode;
                        ws2.Cells[customRateRow, 2].Value = rate.Products.CurrencyCode;

						if (rate.BuyRate != null && rate.BuyRate != 0)
						{
							ws2.Cells[customRateRow, 3].Value = rate.BuyRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 3].Value = "";
						}

						if (rate.SellRate != null && rate.SellRate != 0)
						{
							ws2.Cells[customRateRow, 4].Value = rate.SellRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 4].Value = "";
						}

						//ws2.Cells[customRateRow, 3].Value = rate.BuyRate;
						//ws2.Cells[customRateRow, 4].Value = rate.SellRate;

						if (rate.EncashmentRate != null && rate.EncashmentRate != 0)
						{
							ws2.Cells[customRateRow, 5].Value = rate.EncashmentRate.Value.ToString("#,##0.########");
						}
						else
						{
							ws2.Cells[customRateRow, 5].Value = "";
						}

                       
                        customRateRow++;
                    }
                }

                //Natural Person
                particularRow++;//Add a blank line

                //set first row name
                ws.Cells["A" + particularRow + ":AU" + particularRow].Merge = true;
                ws.Cells[particularRow, 1].Style.Font.Bold = true;
                ws.Cells[particularRow, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[particularRow, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                ws.Cells[particularRow, 1].Value = "Natural Person";

                particularRow++;

                ws.Cells[particularRow, 1].Value = "Customer Code";
                ws.Cells[particularRow, 2].Value = "Customer Type";
                ws.Cells[particularRow, 3].Value = "Name";
                ws.Cells[particularRow, 4].Value = "Permanent Address";
                ws.Cells[particularRow, 5].Value = "Mailing Address";
                ws.Cells[particularRow, 6].Value = "Nationality";
                ws.Cells[particularRow, 7].Value = "IC/Passport";
                ws.Cells[particularRow, 8].Value = "Date of Birth";
                ws.Cells[particularRow, 9].Value = "Contact No (H)";
                ws.Cells[particularRow, 10].Value = "Contact No (O)";
                ws.Cells[particularRow, 11].Value = "Contact No (M)";
                ws.Cells[particularRow, 12].Value = "Email";
				ws.Cells[particularRow, 13].Value = "Sales Remarks";


				ws.Cells[particularRow, 14].Value = "Employee Type";
                ws.Cells[particularRow, 15].Value = "Name of Employer";
                ws.Cells[particularRow, 16].Value = "Job Title";
                ws.Cells[particularRow, 17].Value = "Registered Address of Employer";
                ws.Cells[particularRow, 18].Value = "Name of Business";
                ws.Cells[particularRow, 19].Value = "Business Registration No";
                ws.Cells[particularRow, 20].Value = "Registered Business Address";
                ws.Cells[particularRow, 21].Value = "Principal Place of Business";
                ws.Cells[particularRow, 22].Value = "Source of Funds";
                ws.Cells[particularRow, 23].Value = "Source of Funds If Others";
                ws.Cells[particularRow, 24].Value = "Annual Income (SGD)";
                ws.Cells[particularRow, 25].Value = "Are you or have you ever been entrusted with prominent figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[particularRow, 26].Value = "Are you or have you ever been a parent/ step-parent /step-child, adopted child/ spouse/ sibling/ step-sibling/ adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[particularRow, 27].Value = "Are you closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation? the beneficial owner or has the beneficial owner ever been closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
                ws.Cells[particularRow, 28].Value = "Agent Acting";
                ws.Cells[particularRow, 29].Value = "Name";
                ws.Cells[particularRow, 30].Value = "Permanent Address";
                ws.Cells[particularRow, 31].Value = "Nationality";
                ws.Cells[particularRow, 32].Value = "IC/Passport No";
                ws.Cells[particularRow, 33].Value = "Date of Birth";
                ws.Cells[particularRow, 34].Value = "Relationship between Agent(s) and Client";
                ws.Cells[particularRow, 35].Value = "Basis of Authority";
                ws.Cells[particularRow, 36].Value = "Photocopy of IC";
                ws.Cells[particularRow, 37].Value = "Business/Name Card";
                ws.Cells[particularRow, 38].Value = "Signed KYC Form";
                ws.Cells[particularRow, 39].Value = "Approval By";
                ws.Cells[particularRow, 40].Value = "Screening Results";
                ws.Cells[particularRow, 41].Value = "Grading";
                ws.Cells[particularRow, 42].Value = "Next Review Date";
                ws.Cells[particularRow, 43].Value = "Acra Expiry";
				ws.Cells[particularRow, 44].Value = "Bank Account No.";

				ws.Cells[particularRow, 45].Value = "GM Approval Above";
                ws.Cells[particularRow, 46].Value = "Status";
                ws.Cells[particularRow, 47].Value = "Created On";
                ws.Cells[particularRow, 48].Value = "Updated On";
                ws.Cells[particularRow, 49].Value = "Is Deleted";

                particularRow++;

                foreach (CustomerParticular customer in customerNatural)
                {
                    //Dump Customer Particulars Data
                    ws.Cells[particularRow, 1].Value = customer.CustomerCode;
                    ws.Cells[particularRow, 2].Value = customer.CustomerType;
                    ws.Cells[particularRow, 3].Value = customer.Natural_Name;
                    ws.Cells[particularRow, 4].Value = customer.Natural_PermanentAddress;
                    ws.Cells[particularRow, 5].Value = customer.Natural_MailingAddress;
                    ws.Cells[particularRow, 6].Value = customer.Natural_Nationality;
                    ws.Cells[particularRow, 7].Value = customer.Natural_ICPassportNo;
                    ws.Cells[particularRow, 8].Value = Convert.ToDateTime(customer.Natural_DOB).ToString("dd/MM/yyyy");
                    ws.Cells[particularRow, 9].Value = customer.Natural_ContactNoH;
                    ws.Cells[particularRow, 10].Value = customer.Natural_ContactNoO;
                    ws.Cells[particularRow, 11].Value = customer.Natural_ContactNoM;
                    ws.Cells[particularRow, 12].Value = customer.Natural_Email;
					ws.Cells[particularRow, 13].Value = "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].SalesRemarks != null)
						{
							ws.Cells[particularRow, 13].Value = customer.Others[0].SalesRemarks;
						}
					}

					ws.Cells[particularRow, 14].Value = customer.Natural_EmploymentType;
                    ws.Cells[particularRow, 15].Value = customer.Natural_EmployedEmployerName;
                    ws.Cells[particularRow, 16].Value = customer.Natural_EmployedJobTitle;
                    ws.Cells[particularRow, 17].Value = customer.Natural_EmployedRegisteredAddress;
                    ws.Cells[particularRow, 18].Value = customer.Natural_SelfEmployedBusinessName;
                    ws.Cells[particularRow, 19].Value = customer.Natural_SelfEmployedRegistrationNo;
                    ws.Cells[particularRow, 20].Value = customer.Natural_SelfEmployedBusinessAddress;
                    ws.Cells[particularRow, 21].Value = customer.Natural_SelfEmployedBusinessPrincipalPlace;
                    ws.Cells[particularRow, 22].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_SourceOfFund : "";
                    ws.Cells[particularRow, 23].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_SourceOfFundIfOthers : "";
                    ws.Cells[particularRow, 24].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_AnnualIncome : "";
                    ws.Cells[particularRow, 25].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_1 : "";
                    ws.Cells[particularRow, 26].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_2 : "";
                    ws.Cells[particularRow, 27].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_3 : "";
                    ws.Cells[particularRow, 28].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].ActingAgent : "";
                    ws.Cells[particularRow, 29].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_Name : "";
                    ws.Cells[particularRow, 30].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_PermanentAddress : "";
                    ws.Cells[particularRow, 31].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_Nationality : "";
                    ws.Cells[particularRow, 32].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_ICPassportNo : "";

                    if(customer.ActingAgents.Count > 0)
                    {
                        if (customer.ActingAgents[0].Natural_DOB != null)
                        {
                            ws.Cells[particularRow, 33].Value = Convert.ToDateTime(customer.ActingAgents[0].Natural_DOB).ToString("dd/MM/yyyy");
                        }
                    }

                    ws.Cells[particularRow, 34].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Relationship : "";
                    ws.Cells[particularRow, 35].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].BasisOfAuthority : "";
                    ws.Cells[particularRow, 36].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_ICOfCustomer : "";
                    ws.Cells[particularRow, 37].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_BusinessNameCard : "";
                    ws.Cells[particularRow, 38].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_KYCForm : "";
					ws.Cells[particularRow, 39].Value = "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].Users != null)
						{
							ws.Cells[particularRow, 39].Value = customer.Others[0].Users.Name;
						}
					}

                    //ws.Cells[particularRow, 39].Value = customer.Others.Count > 0 ? customer.Others[0].Users.Name : "";
                    ws.Cells[particularRow, 40].Value = customer.Others.Count > 0 ? customer.Others[0].ScreeningResults : "";
                    ws.Cells[particularRow, 41].Value = customer.Others.Count > 0 ? customer.Others[0].Grading : "";

                    if(customer.Others.Count > 0)
                    {
                        if (customer.Others[0].NextReviewDate != null)
                        {
                            ws.Cells[particularRow, 42].Value = customer.Others[0].NextReviewDate;
                        }
                        if (customer.Others[0].AcraExpiry != null)
                        {
                            ws.Cells[particularRow, 43].Value = customer.Others[0].AcraExpiry;
                        }

                    }

					if (customer.Others.Count > 0)
					{
						if (!string.IsNullOrEmpty(customer.Others[0].BankAccountNo))
						{
							ws.Cells[particularRow, 44].Value = customer.Others[0].BankAccountNo.Replace("|", ",");
						}
						else
						{
							ws.Cells[particularRow, 44].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 44].Value = "";
					}

					//ws.Cells[particularRow, 44].Value = customer.Others.Count > 0 ? customer.Others[0].BankAccountNo.Replace("|", ",") : "";
					ws.Cells[particularRow, 45].Value = customer.Others.Count > 0 ? customer.Others[0].GMApprovalAbove : 0;
                    ws.Cells[particularRow, 46].Value = customer.Others.Count > 0 ? customer.Others[0].Status : "";
                    ws.Cells[particularRow, 47].Value = customer.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[particularRow, 48].Value = customer.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[particularRow, 49].Value = customer.IsDeleted;
                    particularRow++;

                    foreach (CustomerScreeningReport screeningReport in customer.PEPScreeningReports)
                    {
                        //Dump Customer Sanctions and PEP Screening Report
                        ws3.Cells[screeningReportRow, 1].Value = customer.CustomerCode;
                        ws3.Cells[screeningReportRow, 2].Value = screeningReport.Date.ToString("dd/MM/yyyy");
                        ws3.Cells[screeningReportRow, 3].Value = screeningReport.DateOfAcra.ToString("dd/MM/yyyy");
                        ws3.Cells[screeningReportRow, 4].Value = screeningReport.ScreenedBy;
                        ws3.Cells[screeningReportRow, 5].Value = screeningReport.ScreeningReport_1;
                        ws3.Cells[screeningReportRow, 6].Value = screeningReport.ScreeningReport_2;
                        ws3.Cells[screeningReportRow, 7].Value = screeningReport.Remarks;
                        screeningReportRow++;
                    }

                    foreach (CustomerCustomRate rate in customer.CustomRates)
                    {
                        //Dump Custome Custom Rate Data
                        ws2.Cells[customRateRow, 1].Value = rate.CustomerParticulars.CustomerCode;
                        ws2.Cells[customRateRow, 2].Value = rate.Products.CurrencyCode;
						//ws2.Cells[customRateRow, 3].Value = rate.BuyRate;
						//ws2.Cells[customRateRow, 4].Value = rate.SellRate;

						if (rate.BuyRate != null && rate.BuyRate != 0)
						{
							ws2.Cells[customRateRow, 3].Value = rate.BuyRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 3].Value = "";
						}

						if (rate.SellRate != null && rate.SellRate != 0)
						{
							ws2.Cells[customRateRow, 4].Value = rate.SellRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 4].Value = "";
						}

						if (rate.EncashmentRate != null && rate.EncashmentRate != 0)
						{
							ws2.Cells[customRateRow, 5].Value = rate.EncashmentRate.Value.ToString("#,##0.########");
						}
						else
						{
							ws2.Cells[customRateRow, 5].Value = "";
						}

                        customRateRow++;
                    }
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws1.Cells[ws1.Dimension.Address].AutoFitColumns();
                ws2.Cells[ws2.Dimension.Address].AutoFitColumns();
                ws3.Cells[ws3.Dimension.Address].AutoFitColumns();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=customers-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

		//GET: Export Customer Excel2
		public void ExportCustomerExcel2(string fromDate, string toDate)
		{
			IList<CustomerParticular> customers = _customerParticularsModel.GetAllCustomers(fromDate, toDate);

			IList<CustomerParticular> customerCompany = customers.Where(e => e.CustomerType == "Corporate & Trading Company").ToList();
			IList<CustomerParticular> customerNatural = customers.Where(e => e.CustomerType == "Natural Person").ToList();

			var CountryList = new List<Countries>();
			var CountryCodeList = new List<CountryCodeLists>();
			var AllCustomer = new List<CustomerParticular>();
			var AllSearchTags = new List<SearchTags>();

			using (var context = new DataAccess.GreatEastForex())
			{
				CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
				CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
				AllCustomer = context.CustomerParticulars.ToList();
				AllSearchTags = context.SearchTags.ToList();
			}

			using (ExcelPackage pck = new ExcelPackage())
			{
				//Corporate & Trading Company
				//Create Customer Particulars Worksheet
				ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Customer Particulars");

				//set first row name
				ws.Cells["A1:AP1"].Merge = true;
				ws.Cells[1, 1].Style.Font.Bold = true;
				ws.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				ws.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				ws.Cells[1, 1].Value = "Corporate & Trading Company";
				ws.Cells[2, 1].Value = "Customer Code";
				ws.Cells[2, 2].Value = "Customer Type";
				ws.Cells[2, 3].Value = "Registered Name";
				//ws.Cells[2, 4].Value = "Registered Address";
				ws.Cells[2, 4].Value = "Business Address 1";
				ws.Cells[2, 5].Value = "Business Address 2";
				ws.Cells[2, 6].Value = "Business Address 3";
				ws.Cells[2, 7].Value = "Business Postal Code";
				ws.Cells[2, 8].Value = "Shipping Address 1";
				ws.Cells[2, 9].Value = "Shipping Address 2";
				ws.Cells[2, 10].Value = "Shipping Address 3";
				ws.Cells[2, 11].Value = "Shipping Postal Code";
				ws.Cells[2, 12].Value = "Contact Name";
				ws.Cells[2, 13].Value = "Tel No";
				ws.Cells[2, 14].Value = "Fax No";
				ws.Cells[2, 15].Value = "Email";
				ws.Cells[2, 16].Value = "Contact No.(H)";
				ws.Cells[2, 17].Value = "Contact No.(O)";
				ws.Cells[2, 18].Value = "Contact No.(M)";
				ws.Cells[2, 19].Value = "Company IC/Passport";
				ws.Cells[2, 20].Value = "Company Job Title";
				ws.Cells[2, 21].Value = "Company Nationality";
				ws.Cells[2, 22].Value = "Company Country";
				ws.Cells[2, 23].Value = "Company Country Code";
				ws.Cells[2, 24].Value = "Is Main Account";
				ws.Cells[2, 25].Value = "Main Account";
				ws.Cells[2, 26].Value = "Customer Title";
				ws.Cells[2, 27].Value = "Customer Surname";
				ws.Cells[2, 28].Value = "Customer Given Name";
				ws.Cells[2, 29].Value = "Customer D.O.B.";
				ws.Cells[2, 30].Value = "Customer Account Validated";
				ws.Cells[2, 31].Value = "Is KYC Verify";
				ws.Cells[2, 32].Value = "Has Customer Account";
				ws.Cells[2, 33].Value = "Enable Transaction Type";
				ws.Cells[2, 34].Value = "Search Tag";
				ws.Cells[2, 35].Value = "Sales Remarks";

				ws.Cells[2, 36].Value = "Place of Registration";
				ws.Cells[2, 37].Value = "Date of Registration";
				ws.Cells[2, 38].Value = "Type of Entity";
				ws.Cells[2, 39].Value = "Type of Entity If Others";
				ws.Cells[2, 40].Value = "Purpose and Intended";
				ws.Cells[2, 41].Value = "Source of Funds";
				ws.Cells[2, 42].Value = "Source of Funds If Others";
				ws.Cells[2, 43].Value = "Is the benficial owner or has the beneficial owner ever been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[2, 44].Value = "Is the benficial owner or has the beneficial owner ever been a parent/ step-parent /step-child, adopted child/ spouse/ sibling/ step-sibling/ adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[2, 45].Value = "Is the beneficial owner or has the beneficial owner ever been closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[2, 46].Value = "Name the service(s) you are most likely to use (tick all that apply)";
				ws.Cells[2, 47].Value = "Purpose of intended transactions";
				ws.Cells[2, 48].Value = "Where did you hear about us?";
				ws.Cells[2, 49].Value = "Agent Acting";
				ws.Cells[2, 50].Value = "Customer Type";
				ws.Cells[2, 51].Value = "Address";
				ws.Cells[2, 52].Value = "Place of Registration";
				ws.Cells[2, 53].Value = "Registration No";
				ws.Cells[2, 54].Value = "Date of Registration";
				ws.Cells[2, 55].Value = "Relationship between Agent(s) and Client";
				ws.Cells[2, 56].Value = "Basis of Authority";
				ws.Cells[2, 57].Value = "Account Opening form";
				ws.Cells[2, 58].Value = "Photocopy of IC (Authorised Trading Person)";
				ws.Cells[2, 59].Value = "Photocopy of IC (Director)";
				ws.Cells[2, 60].Value = "Company business profile from ACRA";
				ws.Cells[2, 61].Value = "Company business Selfie Passport Working Pass";
				ws.Cells[2, 62].Value = "Company business Selfie Photo ID";
				ws.Cells[2, 63].Value = "Approval By";
				ws.Cells[2, 64].Value = "Screening Results";
				ws.Cells[2, 65].Value = "Grading";
				ws.Cells[2, 66].Value = "Next Review Date";
				ws.Cells[2, 67].Value = "Acra Expiry";

				ws.Cells[2, 68].Value = "Bank Account No.";
				ws.Cells[2, 69].Value = "GM Approval Above";
				ws.Cells[2, 70].Value = "Status";
				ws.Cells[2, 71].Value = "Created On";
				ws.Cells[2, 72].Value = "Updated On";
				ws.Cells[2, 73].Value = "Is Deleted";

				//Create Appointments of Staff Worksheet
				ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Customer Appointments");

				//set first row name
				ws1.Cells[1, 1].Value = "Customer Code";
				ws1.Cells[1, 2].Value = "Full Name";
				ws1.Cells[1, 3].Value = "IC/Passport No";
				ws1.Cells[1, 4].Value = "Nationality";
				ws1.Cells[1, 5].Value = "Job Title";
				ws1.Cells[1, 6].Value = "Specimen Signature";

				//Create Sanctions and PEP Screening Report Worksheet
				ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Sanctions & PEP Screening Report");
				ws3.Cells[1, 1].Value = "Customer Code";
				ws3.Cells[1, 2].Value = "Date";
				ws3.Cells[1, 3].Value = "Date of ACRA";
				ws3.Cells[1, 4].Value = "Screened By";
				ws3.Cells[1, 5].Value = "Screening of all Directors, Secretary and Shareholders on Acra";
				ws3.Cells[1, 6].Value = "Screening of all Authorized Persons in Account Opening Form and/or Update Form";
				ws3.Cells[1, 7].Value = "Remarks";

				//Create Custom Rates Worksheet
				ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Customer Custom Rates");

				//set first row name
				ws2.Cells[1, 1].Value = "Customer Code";
				ws2.Cells[1, 2].Value = "Currency Code";
				ws2.Cells[1, 3].Value = "Buy Rate Adjustment";
				ws2.Cells[1, 4].Value = "Sell Rate Adjustment";
				ws2.Cells[1, 5].Value = "Encashment Rate";

				//Create Remittance Custom Rates Worksheet
				ExcelWorksheet ws4 = pck.Workbook.Worksheets.Add("Customer Remittance Custom Rates");

				//set first row name
				ws4.Cells[1, 1].Value = "Customer Code";
				ws4.Cells[1, 2].Value = "Currency Code";
				ws4.Cells[1, 3].Value = "Buy Rate Adjustment";
				ws4.Cells[1, 4].Value = "Sell Rate Adjustment";
				ws4.Cells[1, 5].Value = "Transaction Fee";

				//Create Customer Activity Logs
				ExcelWorksheet ws5 = pck.Workbook.Worksheets.Add("Customer Activity Logs");

				//set first row name
				ws5.Cells[1, 1].Value = "Customer Code";
				ws5.Cells[1, 2].Value = "Title";
				ws5.Cells[1, 3].Value = "Date Time";
				ws5.Cells[1, 4].Value = "Note";

				int particularRow = 3;
				int appointmentRow = 2;
				int screeningReportRow = 2;
				int customRateRow = 2;
				int remittanceCustomRateRow = 2;
				int activityLogRow = 2;

				foreach (CustomerParticular customer in customerCompany)
				{
					//Dump Customer Particulars Data
					ws.Cells[particularRow, 1].Value = customer.CustomerCode;
					ws.Cells[particularRow, 2].Value = customer.CustomerType;
					ws.Cells[particularRow, 3].Value = customer.Company_RegisteredName;
					//ws.Cells[particularRow, 4].Value = customer.Company_RegisteredAddress;
					ws.Cells[particularRow, 4].Value = customer.Company_BusinessAddress1;
					ws.Cells[particularRow, 5].Value = customer.Company_BusinessAddress2;
					ws.Cells[particularRow, 6].Value = customer.Company_BusinessAddress3;
					ws.Cells[particularRow, 7].Value = customer.Company_PostalCode;
					ws.Cells[particularRow, 8].Value = customer.Shipping_Address1;
					ws.Cells[particularRow, 9].Value = customer.Shipping_Address2;
					ws.Cells[particularRow, 10].Value = customer.Shipping_Address3;
					ws.Cells[particularRow, 11].Value = customer.Shipping_PostalCode;
					ws.Cells[particularRow, 12].Value = customer.Company_ContactName;
					ws.Cells[particularRow, 13].Value = customer.Company_TelNo;
					ws.Cells[particularRow, 14].Value = customer.Company_FaxNo;
					ws.Cells[particularRow, 15].Value = customer.Company_Email;
					ws.Cells[particularRow, 16].Value = customer.Company_ContactNoH;
					ws.Cells[particularRow, 17].Value = customer.Company_ContactNoO;
					ws.Cells[particularRow, 18].Value = customer.Company_ContactNoM;
					ws.Cells[particularRow, 19].Value = customer.Company_ICPassport;
					ws.Cells[particularRow, 20].Value = customer.Company_JobTitle;
					ws.Cells[particularRow, 21].Value = customer.Company_Nationality;
					ws.Cells[particularRow, 22].Value = "";

					if (customer.Company_Country != 0)
					{
						if (CountryList.Where(e => e.ID == customer.Company_Country).FirstOrDefault() != null)
						{
							ws.Cells[particularRow, 22].Value = CountryList.Where(e => e.ID == customer.Company_Country).FirstOrDefault().Name;
						}
					}

					ws.Cells[particularRow, 23].Value = "";

					if (customer.Company_CountryCode != 0)
					{
						if (CountryList.Where(e => e.ID == customer.Company_CountryCode).FirstOrDefault() != null)
						{
							ws.Cells[particularRow, 23].Value = CountryCodeList.Where(e => e.ID == customer.Company_CountryCode).FirstOrDefault().Name + " " + CountryCodeList.Where(e => e.ID == customer.Company_CountryCode).FirstOrDefault().Code;
						}
					}

					ws.Cells[particularRow, 24].Value = "No";

					if (customer.IsSubAccount == 0)
					{
						ws.Cells[particularRow, 24].Value = "Yes";
					}

					if (customer.IsSubAccount == 0)
					{
						ws.Cells[particularRow, 25].Value = "-";
					}
					else
					{
						if (AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault() != null)
						{
							// (string.IsNullOrEmpty(AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName) ? AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Natural_Name : AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName)
							ws.Cells[particularRow, 25].Value = (string.IsNullOrEmpty(AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName) ? AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Natural_Name : AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName);
						}
						else
						{
							ws.Cells[particularRow, 25].Value = "-";
						}
					}

					ws.Cells[particularRow, 26].Value = customer.Customer_Title;
					ws.Cells[particularRow, 27].Value = customer.Surname;
					ws.Cells[particularRow, 28].Value = customer.GivenName;
					ws.Cells[particularRow, 29].Value = "";

					if (customer.DOB != null)
					{
						ws.Cells[particularRow, 29].Value = Convert.ToDateTime(customer.DOB).ToString("dd/MM/yyyy");
					}

					ws.Cells[particularRow, 30].Value = "No";

					if (customer.isVerify == 1)
					{
						ws.Cells[particularRow, 30].Value = "Yes";
					}

					ws.Cells[particularRow, 31].Value = "No";

					if (customer.isKYCVerify == 1)
					{
						ws.Cells[particularRow, 31].Value = "Yes";
					}
					
					ws.Cells[particularRow, 32].Value = "No";

					if (customer.hasCustomerAccount == 1)
					{
						ws.Cells[particularRow, 32].Value = "Yes";
					}

					ws.Cells[particularRow, 33].Value = customer.EnableTransactionType;

					ws.Cells[particularRow, 34].Value = "";

					if (!string.IsNullOrEmpty(customer.SearchTags))
					{
						string GetSearchTags = customer.SearchTags.Replace("-", "");
						string[] SplitSearchTags = GetSearchTags.Split(',');
						List<String> searchTagList = new List<String>();
						int SplitID = 0;
						foreach (var _SplitSearchTags in SplitSearchTags)
						{
							SplitID = Convert.ToInt32(_SplitSearchTags);

							if (AllSearchTags.Where(e => e.ID == SplitID).FirstOrDefault() != null)
							{
								searchTagList.Add(AllSearchTags.Where(e => e.ID == SplitID).FirstOrDefault().TagName);
							}
						}

						if (searchTagList.Count > 0)
						{
							ws.Cells[particularRow, 34].Value = String.Join(",", searchTagList);
						}
					}

					ws.Cells[particularRow, 35].Value = "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].SalesRemarks != null)
						{
							ws.Cells[particularRow, 35].Value = customer.Others[0].SalesRemarks;
						}
					}

					ws.Cells[particularRow, 36].Value = customer.Company_PlaceOfRegistration;
					ws.Cells[particularRow, 37].Value = Convert.ToDateTime(customer.Company_DateOfRegistration).ToString("dd/MM/yyyy");
					ws.Cells[particularRow, 38].Value = customer.Company_TypeOfEntity;
					ws.Cells[particularRow, 39].Value = customer.Company_TypeOfEntityIfOthers;
					ws.Cells[particularRow, 40].Value = customer.Company_PurposeAndIntended;
					ws.Cells[particularRow, 41].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_SourceOfFund : "";
					ws.Cells[particularRow, 42].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_SourceOfFundIfOthers : "";
					ws.Cells[particularRow, 43].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_1 : "";
					ws.Cells[particularRow, 44].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_2 : "";
					ws.Cells[particularRow, 45].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PoliticallyExposedIndividuals_3 : "";
					ws.Cells[particularRow, 46].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_ServiceLikeToUse : "";
					ws.Cells[particularRow, 47].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_PurposeOfIntendedTransactions : "";
					ws.Cells[particularRow, 48].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Company_HearAboutUs : "";

					ws.Cells[particularRow, 49].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].ActingAgent : "";
					ws.Cells[particularRow, 50].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_CustomerType : "";
					ws.Cells[particularRow, 51].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_Address : "";
					ws.Cells[particularRow, 52].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_PlaceOfRegistration : "";
					ws.Cells[particularRow, 53].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Company_RegistrationNo : "";

					if (customer.ActingAgents.Count > 0)
					{
						if (customer.ActingAgents[0].Company_DateOfRegistration != null)
						{
							ws.Cells[particularRow, 54].Value = Convert.ToDateTime(customer.ActingAgents[0].Company_DateOfRegistration).ToString("dd/MM/yyyy");
						}
					}

					ws.Cells[particularRow, 55].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Relationship : "";
					ws.Cells[particularRow, 56].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].BasisOfAuthority : "";
					ws.Cells[particularRow, 57].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_AccountOpeningForm : "";
					ws.Cells[particularRow, 58].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_ICWithAuthorizedTradingPersons : "";
					ws.Cells[particularRow, 59].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_ICWithDirectors : "";
					ws.Cells[particularRow, 60].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_BusinessProfileFromAcra : "";
					ws.Cells[particularRow, 61].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_SelfiePassporWorkingPass : "";
					ws.Cells[particularRow, 62].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Company_SelfiePhotoID : "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].ApprovalBy != 0)
						{
							ws.Cells[particularRow, 63].Value = customer.Others[0].Users.Name;
						}
						else
						{
							ws.Cells[particularRow, 63].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 63].Value = "";
					}

					//ws.Cells[particularRow, 34].Value = customer.Others.Count > 0 ? customer.Others[0].Users.Name : "";
					ws.Cells[particularRow, 64].Value = customer.Others.Count > 0 ? customer.Others[0].ScreeningResults : "";
					ws.Cells[particularRow, 65].Value = customer.Others.Count > 0 ? customer.Others[0].Grading : "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].NextReviewDate != null)
						{
							ws.Cells[particularRow, 66].Value = Convert.ToDateTime(customer.Others[0].NextReviewDate).ToString("dd/MM/yyyy");
						}
						if (customer.Others[0].AcraExpiry != null)
						{
							ws.Cells[particularRow, 67].Value = Convert.ToDateTime(customer.Others[0].AcraExpiry).ToString("dd/MM/yyyy");
						}
					}

					if (customer.Others.Count > 0)
					{
						if (!string.IsNullOrEmpty(customer.Others[0].BankAccountNo))
						{
							ws.Cells[particularRow, 68].Value = customer.Others[0].BankAccountNo.Replace("|", ",");
						}
						else
						{
							ws.Cells[particularRow, 68].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 68].Value = "";
					}

					//ws.Cells[particularRow, 39].Value = customer.Others.Count > 0 ? customer.Others[0].BankAccountNo.Replace("|",",") : "";
					ws.Cells[particularRow, 69].Value = customer.Others.Count > 0 ? customer.Others[0].GMApprovalAbove : 0;
					ws.Cells[particularRow, 70].Value = customer.Others.Count > 0 ? customer.Others[0].Status : "";
					ws.Cells[particularRow, 71].Value = customer.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
					ws.Cells[particularRow, 72].Value = customer.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
					ws.Cells[particularRow, 73].Value = customer.IsDeleted;
					particularRow++;

					foreach (CustomerAppointmentOfStaff appointment in customer.AppointmentOfStaffs)
					{
						//Dump Customer Appointment Data
						ws1.Cells[appointmentRow, 1].Value = appointment.CustomerParticulars.CustomerCode;
						ws1.Cells[appointmentRow, 2].Value = appointment.FullName;
						ws1.Cells[appointmentRow, 3].Value = appointment.ICPassportNo;
						ws1.Cells[appointmentRow, 4].Value = appointment.Nationality;
						ws1.Cells[appointmentRow, 5].Value = appointment.JobTitle;
						ws1.Cells[appointmentRow, 6].Value = appointment.SpecimenSignature;
						appointmentRow++;
					}

					foreach (CustomerScreeningReport screeningReport in customer.PEPScreeningReports)
					{
						//Dump Customer Sanctions and PEP Screening Report
						ws3.Cells[screeningReportRow, 1].Value = customer.CustomerCode;
						ws3.Cells[screeningReportRow, 2].Value = screeningReport.Date.ToString("dd/MM/yyyy");
						ws3.Cells[screeningReportRow, 3].Value = screeningReport.DateOfAcra.ToString("dd/MM/yyyy");
						ws3.Cells[screeningReportRow, 4].Value = screeningReport.ScreenedBy;
						ws3.Cells[screeningReportRow, 5].Value = screeningReport.ScreeningReport_1;
						ws3.Cells[screeningReportRow, 6].Value = screeningReport.ScreeningReport_2;
						ws3.Cells[screeningReportRow, 7].Value = screeningReport.Remarks;
						screeningReportRow++;
					}

					foreach (CustomerCustomRate rate in customer.CustomRates)
					{
						//Dump Customer Custom Rate Data
						ws2.Cells[customRateRow, 1].Value = rate.CustomerParticulars.CustomerCode;
						ws2.Cells[customRateRow, 2].Value = rate.Products.CurrencyCode;

						if (rate.BuyRate != null && rate.BuyRate != 0)
						{
							ws2.Cells[customRateRow, 3].Value = rate.BuyRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 3].Value = "";
						}

						if (rate.SellRate != null && rate.SellRate != 0)
						{
							ws2.Cells[customRateRow, 4].Value = rate.SellRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 4].Value = "";
						}

						//ws2.Cells[customRateRow, 3].Value = rate.BuyRate;
						//ws2.Cells[customRateRow, 4].Value = rate.SellRate;

						if (rate.EncashmentRate != null && rate.EncashmentRate != 0)
						{
							ws2.Cells[customRateRow, 5].Value = rate.EncashmentRate.Value.ToString("#,##0.########");
						}
						else
						{
							ws2.Cells[customRateRow, 5].Value = "";
						}


						customRateRow++;
					}

					foreach (CustomerRemittanceProductCustomRate Rrate in customer.CustomerRemittanceProductCustomRates)
					{
						//Dump Customer Remittance Custom Rate Data
						ws4.Cells[remittanceCustomRateRow, 1].Value = Rrate.CustomerParticulars.CustomerCode;
						ws4.Cells[remittanceCustomRateRow, 2].Value = Rrate.RemittanceProduct.CurrencyCode;

						if (Rrate.PayRateAdjustment != null && Rrate.PayRateAdjustment != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 3].Value = Rrate.PayRateAdjustment.Value;
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 3].Value = "";
						}

						if (Rrate.GetRateAdjustment != null && Rrate.GetRateAdjustment != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 4].Value = Rrate.GetRateAdjustment.Value;
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 4].Value = "";
						}

						if (Rrate.Fee != null && Rrate.Fee != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 5].Value = Rrate.Fee.Value.ToString("#,##0.########");
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 5].Value = "";
						}

						remittanceCustomRateRow++;
					}

					foreach (CustomerActivityLog activityLogs in customer.ActivityLogs)
					{
						//Dump Customer Activity Logs
						ws5.Cells[activityLogRow, 1].Value = customer.CustomerCode;
						ws5.Cells[activityLogRow, 2].Value = activityLogs.Title;
						ws5.Cells[activityLogRow, 3].Value = activityLogs.ActivityLog_DateTime.ToString("dd/MM/yyyy");
						ws5.Cells[activityLogRow, 4].Value = activityLogs.ActivityLog_Note;

						activityLogRow++;
					}
				}

				//Natural Person
				particularRow++;//Add a blank line

				//set first row name
				ws.Cells["A" + particularRow + ":AU" + particularRow].Merge = true;
				ws.Cells[particularRow, 1].Style.Font.Bold = true;
				ws.Cells[particularRow, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				ws.Cells[particularRow, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				ws.Cells[particularRow, 1].Value = "Natural Person";

				particularRow++;

				ws.Cells[particularRow, 1].Value = "Customer Code";
				ws.Cells[particularRow, 2].Value = "Customer Type";
				ws.Cells[particularRow, 3].Value = "Name";
				ws.Cells[particularRow, 4].Value = "Permanent Address 1";
				ws.Cells[particularRow, 5].Value = "Permanent Address 2";
				ws.Cells[particularRow, 6].Value = "Permanent Address 3";
				ws.Cells[particularRow, 7].Value = "Permanent Postal Code";
				ws.Cells[particularRow, 8].Value = "Mailing Address 1";
				ws.Cells[particularRow, 9].Value = "Mailing Address 2";
				ws.Cells[particularRow, 10].Value = "Mailing Address 3";
				ws.Cells[particularRow, 11].Value = "Mailing Postal Code";
				ws.Cells[particularRow, 12].Value = "Nationality";
				ws.Cells[particularRow, 13].Value = "IC/Passport";
				ws.Cells[particularRow, 14].Value = "Date of Birth";
				ws.Cells[particularRow, 15].Value = "Contact No (H)";
				ws.Cells[particularRow, 16].Value = "Contact No (O)";
				ws.Cells[particularRow, 17].Value = "Contact No (M)";
				ws.Cells[particularRow, 18].Value = "Email";
				ws.Cells[particularRow, 19].Value = "Is Main Account";
				ws.Cells[particularRow, 20].Value = "Main Account";
				ws.Cells[particularRow, 21].Value = "Customer Title";
				ws.Cells[particularRow, 22].Value = "Customer Surname";
				ws.Cells[particularRow, 23].Value = "Customer Given Name";
				ws.Cells[particularRow, 24].Value = "Customer Account Validated";
				ws.Cells[particularRow, 25].Value = "Is KYC Verify";
				ws.Cells[particularRow, 26].Value = "Has Customer Account";
				ws.Cells[particularRow, 27].Value = "Enable Transaction Type";
				ws.Cells[particularRow, 28].Value = "Search Tag";
				ws.Cells[particularRow, 29].Value = "Sales Remarks";


				ws.Cells[particularRow, 30].Value = "Employee Type";
				ws.Cells[particularRow, 31].Value = "Name of Employer";
				ws.Cells[particularRow, 32].Value = "Job Title";
				ws.Cells[particularRow, 33].Value = "Registered Address 1 of Employer";
				ws.Cells[particularRow, 34].Value = "Registered Address 2 of Employer";
				ws.Cells[particularRow, 35].Value = "Registered Address 3 of Employer";
				ws.Cells[particularRow, 36].Value = "Name of Business";
				ws.Cells[particularRow, 37].Value = "Business Registration No";
				ws.Cells[particularRow, 38].Value = "Registered Business Address";
				ws.Cells[particularRow, 39].Value = "Principal Place of Business";
				ws.Cells[particularRow, 40].Value = "Source of Funds";
				ws.Cells[particularRow, 41].Value = "Source of Funds If Others";
				ws.Cells[particularRow, 42].Value = "Annual Income (SGD)";
				ws.Cells[particularRow, 43].Value = "Are you or have you ever been entrusted with prominent figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[particularRow, 44].Value = "Are you or have you ever been a parent/ step-parent /step-child, adopted child/ spouse/ sibling/ step-sibling/ adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[particularRow, 45].Value = "Are you closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation? the beneficial owner or has the beneficial owner ever been closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation?";
				ws.Cells[particularRow, 46].Value = "Name the service(s) you are most likely to use (tick all that apply)";
				ws.Cells[particularRow, 47].Value = "Purpose of intended transactions";
				ws.Cells[particularRow, 48].Value = "Where did you hear about us?";
				ws.Cells[particularRow, 49].Value = "Agent Acting";
				ws.Cells[particularRow, 50].Value = "Name";
				ws.Cells[particularRow, 51].Value = "Permanent Address";
				ws.Cells[particularRow, 52].Value = "Nationality";
				ws.Cells[particularRow, 53].Value = "IC/Passport No";
				ws.Cells[particularRow, 54].Value = "Date of Birth";
				ws.Cells[particularRow, 55].Value = "Relationship between Agent(s) and Client";
				ws.Cells[particularRow, 56].Value = "Basis of Authority";
				ws.Cells[particularRow, 57].Value = "Photocopy of IC";
				ws.Cells[particularRow, 58].Value = "Business/Name Card";
				ws.Cells[particularRow, 59].Value = "Signed KYC Form";
				ws.Cells[particularRow, 60].Value = "Selfie Photo ID";
				ws.Cells[particularRow, 61].Value = "Approval By";
				ws.Cells[particularRow, 62].Value = "Screening Results";
				ws.Cells[particularRow, 63].Value = "Grading";
				ws.Cells[particularRow, 64].Value = "Next Review Date";
				ws.Cells[particularRow, 65].Value = "Acra Expiry";
				ws.Cells[particularRow, 66].Value = "Bank Account No.";

				ws.Cells[particularRow, 67].Value = "GM Approval Above";
				ws.Cells[particularRow, 68].Value = "Status";
				ws.Cells[particularRow, 69].Value = "Created On";
				ws.Cells[particularRow, 70].Value = "Updated On";
				ws.Cells[particularRow, 71].Value = "Is Deleted";

				particularRow++;

				foreach (CustomerParticular customer in customerNatural)
				{
					//Dump Customer Particulars Data
					ws.Cells[particularRow, 1].Value = customer.CustomerCode;
					ws.Cells[particularRow, 2].Value = customer.CustomerType;
					ws.Cells[particularRow, 3].Value = customer.Natural_Name;
					ws.Cells[particularRow, 4].Value = customer.Natural_PermanentAddress;
					ws.Cells[particularRow, 5].Value = customer.Natural_PermanentAddress2;
					ws.Cells[particularRow, 6].Value = customer.Natural_PermanentAddress3;
					ws.Cells[particularRow, 7].Value = customer.Natural_PermanentPostalCode;
					ws.Cells[particularRow, 8].Value = customer.Natural_MailingAddress;
					ws.Cells[particularRow, 9].Value = customer.Natural_MailingAddress2;
					ws.Cells[particularRow, 10].Value = customer.Natural_MailingAddress3;
					ws.Cells[particularRow, 11].Value = customer.Mailing_PostalCode;
					ws.Cells[particularRow, 12].Value = customer.Natural_Nationality;
					ws.Cells[particularRow, 13].Value = customer.Natural_ICPassportNo;
					ws.Cells[particularRow, 14].Value = Convert.ToDateTime(customer.Natural_DOB).ToString("dd/MM/yyyy");
					ws.Cells[particularRow, 15].Value = customer.Natural_ContactNoH;
					ws.Cells[particularRow, 16].Value = customer.Natural_ContactNoO;
					ws.Cells[particularRow, 17].Value = customer.Natural_ContactNoM;
					ws.Cells[particularRow, 18].Value = customer.Natural_Email;

					ws.Cells[particularRow, 19].Value = "No";

					if (customer.IsSubAccount == 0)
					{
						ws.Cells[particularRow, 19].Value = "Yes";
					}

					if (customer.IsSubAccount == 0)
					{
						ws.Cells[particularRow, 20].Value = "-";
					}
					else
					{
						if (AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault() != null)
						{
							// (string.IsNullOrEmpty(AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName) ? AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Natural_Name : AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName)
							ws.Cells[particularRow, 20].Value = (string.IsNullOrEmpty(AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName) ? AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Natural_Name : AllCustomer.Where(e => e.ID == customer.IsSubAccount).FirstOrDefault().Company_RegisteredName);
						}
						else
						{
							ws.Cells[particularRow, 20].Value = "-";
						}
					}

					ws.Cells[particularRow, 21].Value = customer.Customer_Title;
					ws.Cells[particularRow, 22].Value = customer.Surname;
					ws.Cells[particularRow, 23].Value = customer.GivenName;

					ws.Cells[particularRow, 24].Value = "No";

					if (customer.isVerify == 1)
					{
						ws.Cells[particularRow, 24].Value = "Yes";
					}

					ws.Cells[particularRow, 25].Value = "No";

					if (customer.isKYCVerify == 1)
					{
						ws.Cells[particularRow, 25].Value = "Yes";
					}

					ws.Cells[particularRow, 26].Value = "No";

					if (customer.hasCustomerAccount == 1)
					{
						ws.Cells[particularRow, 26].Value = "Yes";
					}

					ws.Cells[particularRow, 27].Value = customer.EnableTransactionType;

					ws.Cells[particularRow, 28].Value = "";

					if (!string.IsNullOrEmpty(customer.SearchTags))
					{
						string GetSearchTags = customer.SearchTags.Replace("-", "");
						string[] SplitSearchTags = GetSearchTags.Split(',');
						List<String> searchTagList = new List<String>();
						int SplitID = 0;
						foreach (var _SplitSearchTags in SplitSearchTags)
						{
							SplitID = Convert.ToInt32(_SplitSearchTags);

							if (AllSearchTags.Where(e => e.ID == SplitID).FirstOrDefault() != null)
							{
								searchTagList.Add(AllSearchTags.Where(e => e.ID == SplitID).FirstOrDefault().TagName);
							}
						}

						if (searchTagList.Count > 0)
						{
							ws.Cells[particularRow, 28].Value = String.Join(",", searchTagList);
						}
					}

					ws.Cells[particularRow, 29].Value = "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].SalesRemarks != null)
						{
							ws.Cells[particularRow, 29].Value = customer.Others[0].SalesRemarks;
						}
					}

					ws.Cells[particularRow, 30].Value = customer.Natural_EmploymentType;
					ws.Cells[particularRow, 31].Value = customer.Natural_EmployedEmployerName;
					ws.Cells[particularRow, 32].Value = customer.Natural_EmployedJobTitle;
					ws.Cells[particularRow, 33].Value = customer.Natural_EmployedRegisteredAddress;
					ws.Cells[particularRow, 34].Value = customer.Natural_EmployedRegisteredAddress2;
					ws.Cells[particularRow, 35].Value = customer.Natural_EmployedRegisteredAddress3;
					ws.Cells[particularRow, 36].Value = customer.Natural_SelfEmployedBusinessName;
					ws.Cells[particularRow, 37].Value = customer.Natural_SelfEmployedRegistrationNo;
					ws.Cells[particularRow, 38].Value = customer.Natural_SelfEmployedBusinessAddress;
					ws.Cells[particularRow, 39].Value = customer.Natural_SelfEmployedBusinessPrincipalPlace;
					ws.Cells[particularRow, 40].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_SourceOfFund : "";
					ws.Cells[particularRow, 41].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_SourceOfFundIfOthers : "";
					ws.Cells[particularRow, 42].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_AnnualIncome : "";
					ws.Cells[particularRow, 43].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_1 : "";
					ws.Cells[particularRow, 44].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_2 : "";
					ws.Cells[particularRow, 45].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PoliticallyExposedIndividuals_3 : "";
					ws.Cells[particularRow, 46].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_ServiceLikeToUse : "";
					ws.Cells[particularRow, 47].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_PurposeOfIntendedTransactions : "";
					ws.Cells[particularRow, 48].Value = customer.SourceOfFunds.Count > 0 ? customer.SourceOfFunds[0].Natural_HearAboutUs : "";
					ws.Cells[particularRow, 49].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].ActingAgent : "";
					ws.Cells[particularRow, 50].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_Name : "";
					ws.Cells[particularRow, 51].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_PermanentAddress : "";
					ws.Cells[particularRow, 52].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_Nationality : "";
					ws.Cells[particularRow, 53].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Natural_ICPassportNo : "";

					if (customer.ActingAgents.Count > 0)
					{
						if (customer.ActingAgents[0].Natural_DOB != null)
						{
							ws.Cells[particularRow, 54].Value = Convert.ToDateTime(customer.ActingAgents[0].Natural_DOB).ToString("dd/MM/yyyy");
						}
					}

					ws.Cells[particularRow, 55].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].Relationship : "";
					ws.Cells[particularRow, 56].Value = customer.ActingAgents.Count > 0 ? customer.ActingAgents[0].BasisOfAuthority : "";
					ws.Cells[particularRow, 57].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_ICOfCustomer : "";
					ws.Cells[particularRow, 58].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_BusinessNameCard : "";
					ws.Cells[particularRow, 59].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_KYCForm : "";
					ws.Cells[particularRow, 60].Value = customer.DocumentCheckLists.Count > 0 ? customer.DocumentCheckLists[0].Natural_SelfiePhotoID : "";
					ws.Cells[particularRow, 61].Value = customer.Others.Count > 0 ? customer.Others[0].Users.Name : "";
					ws.Cells[particularRow, 62].Value = customer.Others.Count > 0 ? customer.Others[0].ScreeningResults : "";
					ws.Cells[particularRow, 63].Value = customer.Others.Count > 0 ? customer.Others[0].Grading : "";

					if (customer.Others.Count > 0)
					{
						if (customer.Others[0].NextReviewDate != null)
						{
							ws.Cells[particularRow, 64].Value = customer.Others[0].NextReviewDate;
						}
						if (customer.Others[0].AcraExpiry != null)
						{
							ws.Cells[particularRow, 65].Value = customer.Others[0].AcraExpiry;
						}

					}

					if (customer.Others.Count > 0)
					{
						if (!string.IsNullOrEmpty(customer.Others[0].BankAccountNo))
						{
							ws.Cells[particularRow, 66].Value = customer.Others[0].BankAccountNo.Replace("|", ",");
						}
						else
						{
							ws.Cells[particularRow, 66].Value = "";
						}
					}
					else
					{
						ws.Cells[particularRow, 66].Value = "";
					}

					//ws.Cells[particularRow, 44].Value = customer.Others.Count > 0 ? customer.Others[0].BankAccountNo.Replace("|", ",") : "";
					ws.Cells[particularRow, 67].Value = customer.Others.Count > 0 ? customer.Others[0].GMApprovalAbove : 0;
					ws.Cells[particularRow, 68].Value = customer.Others.Count > 0 ? customer.Others[0].Status : "";
					ws.Cells[particularRow, 69].Value = customer.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
					ws.Cells[particularRow, 70].Value = customer.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
					ws.Cells[particularRow, 71].Value = customer.IsDeleted;
					particularRow++;

					foreach (CustomerScreeningReport screeningReport in customer.PEPScreeningReports)
					{
						//Dump Customer Sanctions and PEP Screening Report
						ws3.Cells[screeningReportRow, 1].Value = customer.CustomerCode;
						ws3.Cells[screeningReportRow, 2].Value = screeningReport.Date.ToString("dd/MM/yyyy");
						ws3.Cells[screeningReportRow, 3].Value = screeningReport.DateOfAcra.ToString("dd/MM/yyyy");
						ws3.Cells[screeningReportRow, 4].Value = screeningReport.ScreenedBy;
						ws3.Cells[screeningReportRow, 5].Value = screeningReport.ScreeningReport_1;
						ws3.Cells[screeningReportRow, 6].Value = screeningReport.ScreeningReport_2;
						ws3.Cells[screeningReportRow, 7].Value = screeningReport.Remarks;
						screeningReportRow++;
					}

					foreach (CustomerCustomRate rate in customer.CustomRates)
					{
						//Dump Custome Custom Rate Data
						ws2.Cells[customRateRow, 1].Value = rate.CustomerParticulars.CustomerCode;
						ws2.Cells[customRateRow, 2].Value = rate.Products.CurrencyCode;
						//ws2.Cells[customRateRow, 3].Value = rate.BuyRate;
						//ws2.Cells[customRateRow, 4].Value = rate.SellRate;

						if (rate.BuyRate != null && rate.BuyRate != 0)
						{
							ws2.Cells[customRateRow, 3].Value = rate.BuyRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 3].Value = "";
						}

						if (rate.SellRate != null && rate.SellRate != 0)
						{
							ws2.Cells[customRateRow, 4].Value = rate.SellRate.Value;
						}
						else
						{
							ws2.Cells[customRateRow, 4].Value = "";
						}

						if (rate.EncashmentRate != null && rate.EncashmentRate != 0)
						{
							ws2.Cells[customRateRow, 5].Value = rate.EncashmentRate.Value.ToString("#,##0.########");
						}
						else
						{
							ws2.Cells[customRateRow, 5].Value = "";
						}

						customRateRow++;
					}

					foreach (CustomerRemittanceProductCustomRate Rrate in customer.CustomerRemittanceProductCustomRates)
					{
						//Dump Customer Remittance Custom Rate Data
						ws4.Cells[remittanceCustomRateRow, 1].Value = Rrate.CustomerParticulars.CustomerCode;
						ws4.Cells[remittanceCustomRateRow, 2].Value = Rrate.RemittanceProduct.CurrencyCode;

						if (Rrate.PayRateAdjustment != null && Rrate.PayRateAdjustment != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 3].Value = Rrate.PayRateAdjustment.Value;
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 3].Value = "";
						}

						if (Rrate.GetRateAdjustment != null && Rrate.GetRateAdjustment != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 4].Value = Rrate.GetRateAdjustment.Value;
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 4].Value = "";
						}

						if (Rrate.Fee != null && Rrate.Fee != 0)
						{
							ws4.Cells[remittanceCustomRateRow, 5].Value = Rrate.Fee.Value.ToString("#,##0.########");
						}
						else
						{
							ws4.Cells[remittanceCustomRateRow, 5].Value = "";
						}

						remittanceCustomRateRow++;
					}

					foreach (CustomerActivityLog activityLogs in customer.ActivityLogs)
					{
						//Dump Customer Activity Logs
						ws5.Cells[activityLogRow, 1].Value = customer.CustomerCode;
						ws5.Cells[activityLogRow, 2].Value = activityLogs.Title;
						ws5.Cells[activityLogRow, 3].Value = activityLogs.ActivityLog_DateTime.ToString("dd/MM/yyyy");
						ws5.Cells[activityLogRow, 4].Value = activityLogs.ActivityLog_Note;

						activityLogRow++;
					}
				}

				ws.Cells[ws.Dimension.Address].AutoFitColumns();
				ws1.Cells[ws1.Dimension.Address].AutoFitColumns();
				ws2.Cells[ws2.Dimension.Address].AutoFitColumns();
				ws3.Cells[ws3.Dimension.Address].AutoFitColumns();
				ws4.Cells[ws4.Dimension.Address].AutoFitColumns();

				//Write it back to the client
				Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				Response.AddHeader("content-disposition", "attachment;  filename=customers-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
				Response.BinaryWrite(pck.GetAsByteArray());
			}
		}

		//GET: ProductInventory Data
		public ActionResult ProductInventory(int page = 1)
        {
            int pageSize = 40;
            if (TempData["PageSize"] != null)
            {
                pageSize = Convert.ToInt32(TempData["PageSize"]);
            }

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            TempData["Page"] = page;
            ViewData["Page"] = page;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            else
            {
                TempData["FromDate"] = "";
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            else
            {
                TempData["ToDate"] = "";
            }
            ViewData["ToDate"] = to;

            IPagedList<Product> products = _productsModel.GetProductDataPaged(from, to, page, pageSize);
            ViewData["Product"] = products;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: ProductInventory Data
        [HttpPost]
        public ActionResult ProductInventory(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(form["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            ViewData["ToDate"] = to;

            IPagedList<Product> products = _productsModel.GetProductDataPaged(from, to, page, pageSize);
            ViewData["Product"] = products;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Export Product Inventory Data
        public void ExportProductInventoryExcel(string fromDate, string toDate)
        {
            IList<Product> products = _productsModel.GetAllProducts(fromDate, toDate);

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Products Worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Products");

                //set first row name
                ws.Cells[1, 1].Value = "Currency Code";
                ws.Cells[1, 2].Value = "Currency Name";
                ws.Cells[1, 3].Value = "Buy Rate";
                ws.Cells[1, 4].Value = "Sell Rate";
                ws.Cells[1, 5].Value = "Decimal";
                ws.Cells[1, 6].Value = "Symbol";
                ws.Cells[1, 7].Value = "Acceptable Range";
                ws.Cells[1, 8].Value = "Unit";
                ws.Cells[1, 9].Value = "Payment Mode Allowed";
                ws.Cells[1, 10].Value = "Transaction Type Allowed";
                ws.Cells[1, 11].Value = "Encashment Rate";
                ws.Cells[1, 12].Value = "Total In Account";
                ws.Cells[1, 13].Value = "Status";
                ws.Cells[1, 14].Value = "Created On";
                ws.Cells[1, 15].Value = "Updated On";
                ws.Cells[1, 16].Value = "Is Deleted";

                //Create Product Denominations Worksheet
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Product Denominations");

                //set first row name
                ws1.Cells[1, 1].Value = "Currency Code";
                ws1.Cells[1, 2].Value = "Denomination Value";

                //Create Products Worksheet
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Product Inventory Logs");

                //set first row name
                ws2.Cells[1, 1].Value = "Currency Code";
                ws2.Cells[1, 2].Value = "Type";
                ws2.Cells[1, 3].Value = "Amount";
                ws2.Cells[1, 4].Value = "Description";

                int productRow = 2;
                int denominationRow = 2;
                int inventoryRow = 2;

                foreach (Product product in products)
                {
                    //Dump Products Data
                    ws.Cells[productRow, 1].Value = product.CurrencyCode;
                    ws.Cells[productRow, 2].Value = product.CurrencyName;
                    ws.Cells[productRow, 3].Value = product.BuyRate;
                    ws.Cells[productRow, 4].Value = product.SellRate;
                    ws.Cells[productRow, 5].Value = product.Decimal;
                    ws.Cells[productRow, 6].Value = product.Symbol;
                    ws.Cells[productRow, 7].Value = product.AcceptableRange;
                    ws.Cells[productRow, 8].Value = product.Unit;
                    ws.Cells[productRow, 9].Value = product.PaymentModeAllowed;
                    ws.Cells[productRow, 10].Value = product.TransactionTypeAllowed;
                    ws.Cells[productRow, 11].Value = product.EncashmentRate.ToString("#,##0.########");
                    ws.Cells[productRow, 12].Value = product.ProductInventories[0].TotalInAccount;
                    ws.Cells[productRow, 13].Value = product.Status;
                    ws.Cells[productRow, 14].Value = product.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[productRow, 15].Value = product.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[productRow, 16].Value = product.IsDeleted;
                    productRow++;

                    product.ProductDenominations = product.ProductDenominations.OrderByDescending(e => e.DenominationValue).ToList();

                    foreach (ProductDenomination denomination in product.ProductDenominations)
                    {
                        ws1.Cells[denominationRow, 1].Value = denomination.Products.CurrencyCode;
                        if (denomination.DenominationValue == 0)
                        {
                            ws1.Cells[denominationRow, 2].Value = "coins";
                        }
                        else
                        {
                            ws1.Cells[denominationRow, 2].Value = denomination.DenominationValue;
                        }
                        denominationRow++;
                    }

                    foreach (Inventory inventory in product.Inventories)
                    {
                        ws2.Cells[inventoryRow, 1].Value = inventory.Products.CurrencyCode;
                        ws2.Cells[inventoryRow, 2].Value = inventory.Type;
                        ws2.Cells[inventoryRow, 3].Value = inventory.Amount;
                        ws2.Cells[inventoryRow, 4].Value = inventory.Description;
                        inventoryRow++;
                    }
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws1.Cells[ws1.Dimension.Address].AutoFitColumns();
                ws2.Cells[ws2.Dimension.Address].AutoFitColumns();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=productinventories-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ViewInventoryLogs
        public ActionResult ViewInventoryLogs(int id)
        {
            Product products = _productsModel.GetSingle(id);

            string dp = "#,##0";
            if (products.Decimal == 1)
            {
                dp += ".0";
            }
            else if (products.Decimal == 2)
            {
                dp += ".00";
            }
            else if (products.Decimal == 3)
            {
                dp += ".000";
            }
            else if (products.Decimal == 4)
            {
                dp += ".0000";
            }
            ViewData["DecimalPlaces"] = dp;
            ViewData["Symbol"] = products.Symbol;
            ViewData["CurrencyCode"] = products.CurrencyCode;

            ViewData["InventoryLogs"] = products.Inventories;

            return View();
        }

        //GET: Sale Data
        public ActionResult Sale(int page = 1)
        {
            int pageSize = 40;
            if (TempData["PageSize"] != null)
            {
                pageSize = Convert.ToInt32(TempData["PageSize"]);
            }

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            TempData["Page"] = page;
            ViewData["Page"] = page;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            else
            {
                TempData["FromDate"] = "";
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            else
            {
                TempData["ToDate"] = "";
            }
            ViewData["ToDate"] = to;

            IPagedList<Sale> sales = _salesModel.GetSaleDataPaged(from, to, page, pageSize);
            ViewData["Sale"] = sales;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Sale Data
        [HttpPost]
        public ActionResult Sale(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(form["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            ViewData["ToDate"] = to;

            IPagedList<Sale> sales = _salesModel.GetSaleDataPaged(from, to, page, pageSize);
            ViewData["Sale"] = sales;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Export Sale Excel
        public void ExportSaleExcel(string fromDate, string toDate)
        {
            IList<Sale> sales = _salesModel.GetAllSales(fromDate, toDate);

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Sales Worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sales");

                //set first row name
                ws.Cells[1, 1].Value = "Memo ID";
                ws.Cells[1, 2].Value = "Customer/Company Name";
                ws.Cells[1, 3].Value = "Address";
                ws.Cells[1, 4].Value = "Postal Code";
                ws.Cells[1, 5].Value = "Contact No.";
                ws.Cells[1, 6].Value = "Customer Appointment";
                ws.Cells[1, 7].Value = "Registration No. (Place of Registration) / IC/Passport No. (Nationality)";
                ws.Cells[1, 8].Value = "Issue Date";
                ws.Cells[1, 9].Value = "Collection Date";
                ws.Cells[1, 10].Value = "Collection Time";
                ws.Cells[1, 11].Value = "Created By";
                ws.Cells[1, 12].Value = "Urgent";
                ws.Cells[1, 13].Value = "Bag No";
                ws.Cells[1, 14].Value = "Transaction Type";
                ws.Cells[1, 15].Value = "Local Payment Mode";
                ws.Cells[1, 16].Value = "Cash Amount";
                ws.Cells[1, 17].Value = "Cheque 1 No";
                ws.Cells[1, 18].Value = "Cheque 1 Amount";
                ws.Cells[1, 19].Value = "Cheque 2 No";
                ws.Cells[1, 20].Value = "Cheque 2 Amount";
                ws.Cells[1, 21].Value = "Cheque 3 No";
                ws.Cells[1, 22].Value = "Cheque 3 Amount";
                ws.Cells[1, 23].Value = "Bank Transfer No";
                ws.Cells[1, 24].Value = "Bank Transfer Amount";
                ws.Cells[1, 25].Value = "Remarks";
                ws.Cells[1, 26].Value = "Total Amount Local";
                ws.Cells[1, 27].Value = "Total Amount Foreign";
                ws.Cells[1, 28].Value = "Status";
                ws.Cells[1, 29].Value = "Delivery Confirmation";
                ws.Cells[1, 30].Value = "Disapproved Reason";
                ws.Cells[1, 31].Value = "Created On";
                ws.Cells[1, 32].Value = "Updated On";
                ws.Cells[1, 33].Value = "Is Deleted";

                //Create Sale Transaction Worksheet
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Sale Transactions");

                //set first row name
                ws1.Cells[1, 1].Value = "Memo ID";
                ws1.Cells[1, 2].Value = "Transaction ID";
                ws1.Cells[1, 3].Value = "Transaction Type";
                ws1.Cells[1, 4].Value = "Currency";
                ws1.Cells[1, 5].Value = "Rate";
                ws1.Cells[1, 6].Value = "Encashment Rate";
                ws1.Cells[1, 7].Value = "Unit";
                ws1.Cells[1, 8].Value = "Amount Foreign";
                ws1.Cells[1, 9].Value = "Amount Local";
                ws1.Cells[1, 10].Value = "Payment Mode";
                ws1.Cells[1, 11].Value = "Cheque No";
                ws1.Cells[1, 12].Value = "Bank Transfer No";
                ws1.Cells[1, 13].Value = "Vessel Name";

                //Create Sale Transaction Denominations Worksheet
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Sale Transaction Denominations");

                //set first row name
                ws2.Cells[1, 1].Value = "Transaction ID";
                ws2.Cells[1, 2].Value = "Denomination";
                ws2.Cells[1, 3].Value = "Pieces";
                ws2.Cells[1, 4].Value = "Amount Foreign";

                int saleRow = 2;
                int transactionRow = 2;
                int denominationRow = 2;

                foreach (Sale sale in sales)
                {
                    //Dump Sales Data
                    ws.Cells[saleRow, 1].Value = sale.MemoID;
                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        ws.Cells[saleRow, 2].Value = sale.CustomerParticulars.Company_RegisteredName;
                        ws.Cells[saleRow, 3].Value = sale.CustomerParticulars.Company_BusinessAddress1 + "\r\n" + sale.CustomerParticulars.Company_BusinessAddress2;
                        ws.Cells[saleRow, 4].Value = sale.CustomerParticulars.Company_PostalCode;
                        ws.Cells[saleRow, 5].Value = sale.CustomerParticulars.Company_TelNo;
                        ws.Cells[saleRow, 7].Value = sale.CustomerParticulars.Company_RegistrationNo + " (" + sale.CustomerParticulars.Company_PlaceOfRegistration + ")";
                    }
                    else
                    {
                        ws.Cells[saleRow, 2].Value = sale.CustomerParticulars.Natural_Name;
                        ws.Cells[saleRow, 3].Value = sale.CustomerParticulars.Natural_PermanentAddress;
                        ws.Cells[saleRow, 4].Value = "-";
                        ws.Cells[saleRow, 5].Value = sale.CustomerParticulars.Natural_ContactNoM;
                        ws.Cells[saleRow, 7].Value = sale.CustomerParticulars.Natural_ICPassportNo + " (" + sale.CustomerParticulars.Natural_Nationality + ")";
                    }
                    if (sale.CustomerParticulars.AppointmentOfStaffs.Count > 0)
                    {
                        ws.Cells[saleRow, 6].Value = sale.CustomerParticulars.AppointmentOfStaffs.FirstOrDefault().FullName;
                    }
                    else
                    {
                        ws.Cells[saleRow, 6].Value = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    ws.Cells[saleRow, 8].Value = sale.IssueDate.ToString("dd/MM/yyyy");
                    ws.Cells[saleRow, 9].Value = Convert.ToDateTime(sale.CollectionDate).ToString("dd/MM/yyyy");
                    ws.Cells[saleRow, 10].Value = sale.CollectionTime;
                    ws.Cells[saleRow, 11].Value = sale.Users.Name;
                    ws.Cells[saleRow, 12].Value = sale.Urgent;
                    ws.Cells[saleRow, 13].Value = sale.BagNo;
                    ws.Cells[saleRow, 14].Value = sale.TransactionType.Replace("Deposit", "Swap");
                    ws.Cells[saleRow, 15].Value = sale.LocalPaymentMode;
                    ws.Cells[saleRow, 16].Value = sale.CashAmount;
                    ws.Cells[saleRow, 17].Value = sale.Cheque1No;
                    ws.Cells[saleRow, 18].Value = sale.Cheque1Amount;
                    ws.Cells[saleRow, 19].Value = sale.Cheque2No;
                    ws.Cells[saleRow, 20].Value = sale.Cheque2Amount;
                    ws.Cells[saleRow, 21].Value = sale.Cheque3No;
                    ws.Cells[saleRow, 22].Value = sale.Cheque3Amount;
                    ws.Cells[saleRow, 23].Value = sale.BankTransferNo;
                    ws.Cells[saleRow, 24].Value = sale.BankTransferAmount;
                    ws.Cells[saleRow, 25].Value = sale.Remarks;
                    ws.Cells[saleRow, 26].Value = sale.TotalAmountLocal;
                    ws.Cells[saleRow, 27].Value = sale.TotalAmountForeign;
                    ws.Cells[saleRow, 28].Value = sale.Status;
                    ws.Cells[saleRow, 29].Value = sale.DeliveryConfirmation;
                    ws.Cells[saleRow, 30].Value = sale.DisapprovedReason;
                    ws.Cells[saleRow, 31].Value = sale.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[saleRow, 32].Value = sale.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[saleRow, 33].Value = sale.IsDeleted;
                    saleRow++;

                    foreach (SaleTransaction transaction in sale.SaleTransactions)
                    {
                        //Dump Sale Transaction Data
                        ws1.Cells[transactionRow, 1].Value = transaction.Sales.MemoID;
                        ws1.Cells[transactionRow, 2].Value = transaction.TransactionID;
                        ws1.Cells[transactionRow, 3].Value = transaction.TransactionType;
                        ws1.Cells[transactionRow, 4].Value = transaction.Products.CurrencyCode;
                        ws1.Cells[transactionRow, 5].Value = transaction.Rate;
                        ws1.Cells[transactionRow, 6].Value = transaction.EncashmentRate;
                        ws1.Cells[transactionRow, 7].Value = transaction.Unit;
                        ws1.Cells[transactionRow, 8].Value = transaction.AmountForeign;
                        ws1.Cells[transactionRow, 9].Value = transaction.AmountLocal;
                        ws1.Cells[transactionRow, 10].Value = transaction.PaymentMode;
                        ws1.Cells[transactionRow, 11].Value = transaction.ChequeNo;
                        ws1.Cells[transactionRow, 12].Value = transaction.BankTransferNo;
                        ws1.Cells[transactionRow, 13].Value = transaction.VesselName;
                        transactionRow++;

                        foreach (SaleTransactionDenomination denomination in transaction.SaleTransactionDenominations)
                        {
                            //Dump Sale Transaction Denomination Data
                            ws2.Cells[denominationRow, 1].Value = denomination.SaleTransactions.Sales.MemoID + "-" + denomination.SaleTransactions.TransactionID;
                            if (denomination.Denomination == 0)
                            {
                                ws2.Cells[denominationRow, 2].Value = "coins";
                                ws2.Cells[denominationRow, 3].Value = "-";
                            }
                            else
                            {
                                ws2.Cells[denominationRow, 2].Value = denomination.Denomination;
                                ws2.Cells[denominationRow, 3].Value = denomination.Pieces;
                            }
                            ws2.Cells[denominationRow, 4].Value = denomination.AmountForeign;
                            denominationRow++;
                        }
                    }
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws1.Cells[ws1.Dimension.Address].AutoFitColumns();
                ws2.Cells[ws2.Dimension.Address].AutoFitColumns();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: User Data
        public ActionResult User(int page = 1)
        {
            int pageSize = 40;
            if (TempData["PageSize"] != null)
            {
                pageSize = Convert.ToInt32(TempData["PageSize"]);
            }

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            TempData["Page"] = page;
            ViewData["Page"] = page;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            else
            {
                TempData["FromDate"] = "";
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            else
            {
                TempData["ToDate"] = "";
            }
            ViewData["ToDate"] = to;

            IPagedList<User> users = _usersModel.GetUserDataPaged(from, to, page, pageSize);
            ViewData["User"] = users;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: User Data
        [HttpPost]
        public ActionResult User(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(form["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string from = "";
            if (TempData["FromDate"] != null)
            {
                from = TempData["FromDate"].ToString();
            }
            ViewData["FromDate"] = from;

            string to = "";
            if (TempData["ToDate"] != null)
            {
                to = TempData["ToDate"].ToString();
            }
            ViewData["ToDate"] = to;

            IPagedList<User> users = _usersModel.GetUserDataPaged(from, to, page, pageSize);
            ViewData["User"] = users;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Export User Excel
        public void ExportUserExcel(string fromDate, string toDate)
        {
            IList<User> users = _usersModel.GetAllUsers(fromDate, toDate);

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Users Worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Users");

                //set first row name
                ws.Cells[1, 1].Value = "NRIC";
                ws.Cells[1, 2].Value = "Name";
                ws.Cells[1, 3].Value = "Email";
                ws.Cells[1, 4].Value = "Role";
                ws.Cells[1, 5].Value = "Status";
                ws.Cells[1, 6].Value = "Created On";
                ws.Cells[1, 7].Value = "Updated On";
                ws.Cells[1, 8].Value = "Is Deleted";
                ws.Cells[1, 9].Value = "Last Login";
                ws.Cells[1, 10].Value = "Reset Password Token";

                int userRow = 2;

                foreach (User user in users)
                {
                    //Dump Users Data
                    ws.Cells[userRow, 1].Value = user.NRIC;
                    ws.Cells[userRow, 2].Value = user.Name;
                    ws.Cells[userRow, 3].Value = user.Email;
                    ws.Cells[userRow, 4].Value = user.Role;
                    ws.Cells[userRow, 5].Value = user.Status;
                    ws.Cells[userRow, 6].Value = user.CreatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[userRow, 7].Value = user.UpdatedOn.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[userRow, 8].Value = user.IsDeleted;
                    ws.Cells[userRow, 9].Value = user.LastLogin.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[userRow, 10].Value = user.ResetPasswordToken;
                    userRow++;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=users-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //Table Name Dropdown
        public Dropdown[] TableNameDDL()
        {
            List<string> tableNames = new List<string>();
            tableNames.Add("Sales");
            tableNames.Add("Customers");
            tableNames.Add("Users");
            tableNames.Add("Product/Inventory");

            tableNames = tableNames.OrderBy(e => e).ToList();

            Dropdown[] ddl = new Dropdown[tableNames.Count];

            int count = 0;
            foreach (string name in tableNames)
            {
                ddl[count] = new Dropdown { name = name, val = name };
                count++;
            }

            return ddl;
        }

        //PageSize Dropdown
        public Dropdown[] PageSizeDDL()
        {
            Dropdown[] ddl = new Dropdown[5];
            ddl[0] = new Dropdown { name = "20", val = "20" };
            ddl[1] = new Dropdown { name = "40", val = "40" };
            ddl[2] = new Dropdown { name = "60", val = "60" };
            ddl[3] = new Dropdown { name = "80", val = "80" };
            ddl[4] = new Dropdown { name = "100", val = "100" };
            return ddl;
        }

        public string GetDecimalFormat(int dp)
        {
            string format = "#,##0";

            if (dp > 0)
            {
                format += ".";

                for (int i = 1; i <= dp; i++)
                {
                    format += "0";
                }
            }

            return format;
        }
    }
}