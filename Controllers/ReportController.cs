using DataAccess;
using DataAccess.POCO;
using DataAccess.Report;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [RedirectingActionReport]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReportController : ControllerBase
    {
        private IEndDayTradeRepository _endDayTradesModel;
        private IEndDayTradeTransactionRepository _endDayTradeTransactionsModel;
        private ISaleRepository _salesModel;
        private IRemittanceSaleRepository _remittanceSalesModel;
        private ISaleTransactionRepository _saleTransactionsModel;
        private IProductRepository _productsModel;
        private IRemittanceProductRepository _remittanceProductsModel;
        private IProductInventoryRepository _productInventoriesModel;
        private ICurrencyClosingBalanceRepository _closingBalancesModel;
        private ISettingRepository _settingsModel;
        private ICustomerParticularRepository _customerParticularsModel;
        private IAgentRepository _agentsModel;
        private int sgdDp = 2;
        private int rateDP = 8;

        public ReportController()
            : this(new EndDayTradeRepository(), new EndDayTradeTransactionRepository(), new SaleRepository(), new RemittanceSaleRepository(), new SaleTransactionRepository(), new ProductRepository(), new RemittanceProductRepository(), new ProductInventoryRepository(), new CurrencyClosingBalanceRepository(), new SettingRepository(), new CustomerParticularRepository(), new AgentRepository())
        {

        }

        public ReportController(IEndDayTradeRepository endDayTradesModel, IEndDayTradeTransactionRepository endDayTradeTransactionsModel, ISaleRepository salesModel, IRemittanceSaleRepository remittanceSalesModel, ISaleTransactionRepository saleTransactionsModel, IProductRepository productsModel, IRemittanceProductRepository remittanceProductModel, IProductInventoryRepository productInventoriesModel, ICurrencyClosingBalanceRepository closingBalancesModel, ISettingRepository settingsModel, ICustomerParticularRepository customerParticularsModel, IAgentRepository agentsModel)
        {
            _endDayTradesModel = endDayTradesModel;
            _endDayTradeTransactionsModel = endDayTradeTransactionsModel;
            _salesModel = salesModel;
            _saleTransactionsModel = saleTransactionsModel;
            _remittanceSalesModel = remittanceSalesModel;
            _productsModel = productsModel;
            _remittanceProductsModel = remittanceProductModel;
            _productInventoriesModel = productInventoriesModel;
            _closingBalancesModel = closingBalancesModel;
            _settingsModel = settingsModel;
            _customerParticularsModel = customerParticularsModel;
            _agentsModel = agentsModel;
            Product sgd = _productsModel.FindCurrencyCode("SGD");
            sgdDp = sgd.Decimal;
            ViewData["SGDDP"] = sgdDp;
            ViewData["RateDP"] = rateDP;
            ViewData["SGDFormat"] = GetDecimalFormat(sgdDp);
            ViewData["RateFormat"] = GetRateFormat(rateDP);
        }

        // GET: Report
        public ActionResult Index()
        {
            if (TempData["ReportName"] != null)
            {
                TempData.Remove("ReportName");
            }

            if (TempData["ReportDate"] != null)
            {
                TempData.Remove("ReportDate");
            }

            if (TempData["ReportFromDate"] != null)
            {
                TempData.Remove("ReportFromDate");
            }

            if (TempData["ReportToDate"] != null)
            {
                TempData.Remove("ReportToDate");
            }

            if (TempData["ReportCustomer"] != null)
            {
                TempData.Remove("ReportCustomer");
            }

            if (TempData["ReportMultipleCustomer"] != null)
            {
                TempData.Remove("ReportMultipleCustomer");
            }

            if (TempData["ReportProduct"] != null)
            {
                TempData.Remove("ReportProduct");
            }

            if (TempData["ReportStatus"] != null)
            {
                TempData.Remove("ReportStatus");
            }

            if (TempData["ReportSalesStatus"] != null)
            {
                TempData.Remove("ReportSalesStatus");
            }

            return RedirectToAction("Select");
        }

        //GET: Select
        public ActionResult Select()
        {
            //Check if today has closed end day trade
            DateTime current = DateTime.Now;

            DateTime currentActivation = DateTime.Now;

            IList<EndDayTrade> hasRecords = _endDayTradesModel.GetAll(currentActivation.ToString("dd/MM/yyyy"));

            if (hasRecords.Count == 0)
            {
                ViewData["HasClosedEndDayTrade"] = "Today's End of Day Trade has not closed!";
            }
            else
            {
                ViewData["HasClosedEndDayTrade"] = "Today's End of Day Trade has been last closed at " + hasRecords.OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault().CurrentActivationTime.ToString("dd/MM/yyyy HH:mm:ss") + "!";
            }

            ViewData["ReportName"] = "";
            if (TempData["ReportName"] != null)
            {
                ViewData["ReportName"] = TempData["ReportName"].ToString();
            }

            ViewData["ReportDate"] = "";
            if (TempData["ReportDate"] != null)
            {
                ViewData["ReportDate"] = TempData["ReportDate"].ToString();
            }

            ViewData["ReportFromDate"] = "";
            if (TempData["ReportFromDate"] != null)
            {
                ViewData["ReportFromDate"] = TempData["ReportFromDate"].ToString();
            }

            ViewData["ReportToDate"] = "";
            if (TempData["ReportToDate"] != null)
            {
                ViewData["ReportToDate"] = TempData["ReportToDate"].ToString();
            }

            ViewData["ReportCustomer"] = "";
            if (TempData["ReportCustomer"] != null)
            {
                ViewData["ReportCustomer"] = TempData["ReportCustomer"].ToString();
            }

            ViewData["ReportMultipleCustomer"] = "";
            if (TempData["ReportMultipleCustomer"] != null)
            {
                ViewData["ReportMultipleCustomer"] = TempData["ReportMultipleCustomer"].ToString();
            }

            ViewData["ReportProduct"] = "";
            if (TempData["ReportProduct"] != null)
            {
                ViewData["ReportProduct"] = TempData["ReportProduct"].ToString();
            }

            ViewData["ReportRemittanceProduct"] = "";
            if (TempData["ReportRemittanceProduct"] != null)
            {
                ViewData["ReportRemittanceProduct"] = TempData["ReportRemittanceProduct"].ToString();
            }

            ViewData["ReportStatus"] = "";
            if (TempData["ReportStatus"] != null)
            {
                ViewData["ReportStatus"] = TempData["ReportStatus"].ToString();
            }

            ViewData["ReportSalesStatus"] = "";
            if (TempData["ReportSalesStatus"] != null)
            {
                ViewData["ReportSalesStatus"] = TempData["ReportSalesStatus"].ToString();
            }

            ViewData["ReportRemittanceStatus"] = "";
            if (TempData["ReportRemittanceStatus"] != null)
            {
                ViewData["ReportRemittanceStatus"] = TempData["ReportRemittanceStatus"].ToString();
            }

            ViewData["TransactionType"] = "";
            if (TempData["TransactionType"] != null)
            {
                ViewData["TransactionType"] = TempData["TransactionType"].ToString();
            }

            ViewData["ReportAgents"] = "";
            if (TempData["ReportAgents"] != null)
            {
                ViewData["ReportAgents"] = TempData["ReportAgents"].ToString();
            }

            ViewData["ReportCountries"] = "";
            if (TempData["ReportCountries"] != null)
            {
                ViewData["ReportCountries"] = TempData["ReportCountries"].ToString();
            }

            ViewData["ReportCustomerType"] = "";
            if (TempData["ReportCustomerType"] != null)
            {
                ViewData["ReportCustomerType"] = TempData["ReportCustomerType"].ToString();
            }

            Dropdown[] reportNameDDL = ReportNameDDL();
            ViewData["ReportNameDropdown"] = new SelectList(reportNameDDL, "val", "name", ViewData["ReportName"].ToString());

            Dropdown[] customerDDL = CustomerDDL();
            ViewData["ReportCustomerDropdown"] = new SelectList(customerDDL, "val", "name", ViewData["ReportCustomer"].ToString());
            ViewData["ReportMultipleCustomerDropdown"] = new MultiSelectList(customerDDL, "val", "name", ViewData["ReportMultipleCustomer"].ToString().Split(','));

            Dropdown[] productDDL = ProductDDL();
            ViewData["ReportProductDropdown"] = new MultiSelectList(productDDL, "val", "name", ViewData["ReportProduct"].ToString().Split(','));

            Dropdown[] remittanceproductDDL = RemittanceProductDDL();
            ViewData["ReportRemittanceProductDropdown"] = new MultiSelectList(remittanceproductDDL, "val", "name", ViewData["ReportRemittanceProduct"].ToString().Split(','));

            Dropdown[] statusDDL = StatusDDL();
            ViewData["ReportStatusDropdown"] = new MultiSelectList(statusDDL, "val", "name", null, ViewData["ReportStatus"].ToString().Split(','));

            Dropdown[] salesStatusDDL = SalesStatusDDL();
            ViewData["ReportSalesStatusDropdown"] = new SelectList(salesStatusDDL, "val", "name", ViewData["ReportSalesStatus"].ToString());

            Dropdown[] remittanceStatusDDL = RemittanceStatusDDL();
            ViewData["ReportRemittanceStatusDropdown"] = new SelectList(remittanceStatusDDL, "val", "name", ViewData["ReportRemittanceStatus"].ToString());

            Dropdown[] transactionTypeDDL = TranscationTypeDDL();
            ViewData["ReportTranscationTypeDropdown"] = new MultiSelectList(transactionTypeDDL, "val", "name", ViewData["TransactionType"].ToString().Split(','));

            Dropdown[] agentsDDL = AgentDDL();
            ViewData["AgentsDropdown"] = new MultiSelectList(agentsDDL, "val", "name", ViewData["ReportAgents"].ToString().Split(','));

            Dropdown[] countriesDDL = CountryDDL();
            ViewData["CountriesDropdown"] = new MultiSelectList(countriesDDL, "val", "name", ViewData["ReportCountries"].ToString().Split(','));

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", ViewData["ReportCustomerType"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Select
        [HttpPost]
        public ActionResult Select(FormCollection form)
        {
            ViewData["ReportName"] = form["ReportName"].ToString();
            TempData["ReportName"] = form["ReportName"].ToString();

            ViewData["ReportDate"] = form["ReportDate"].ToString();
            TempData["ReportDate"] = form["ReportDate"].ToString();

            ViewData["ReportFromDate"] = form["ReportFromDate"].ToString();
            TempData["ReportFromDate"] = form["ReportFromDate"].ToString();

            ViewData["ReportToDate"] = form["ReportToDate"].ToString();
            TempData["ReportToDate"] = form["ReportToDate"].ToString();

            ViewData["ReportCustomer"] = form["ReportCustomer"].ToString();
            TempData["ReportCustomer"] = form["ReportCustomer"].ToString();

            ViewData["ReportMultipleCustomer"] = "";
            TempData["ReportMultipleCustomer"] = "";
            if (!string.IsNullOrEmpty(form["ReportMultipleCustomer"]))
            {
                ViewData["ReportMultipleCustomer"] = form["ReportMultipleCustomer"].ToString();
                TempData["ReportMultipleCustomer"] = form["ReportMultipleCustomer"].ToString();
            }

            ViewData["ReportProduct"] = "";
            TempData["ReportProduct"] = "";
            if (!string.IsNullOrEmpty(form["ReportProduct"]))
            {
                ViewData["ReportProduct"] = form["ReportProduct"].ToString();
                TempData["ReportProduct"] = form["ReportProduct"].ToString();
            }

            ViewData["ReportStatus"] = "";
            TempData["ReportStatus"] = "";
            if (!string.IsNullOrEmpty(form["ReportStatus"]))
            {
                ViewData["ReportStatus"] = form["ReportStatus"].ToString();
                TempData["ReportStatus"] = form["ReportStatus"].ToString();
            }

            ViewData["ReportSalesStatus"] = "";
            TempData["ReportSalesStatus"] = "";
            if (!string.IsNullOrEmpty(form["ReportSalesStatus"]))
            {
                ViewData["ReportSalesStatus"] = form["ReportSalesStatus"].ToString();
                TempData["ReportSalesStatus"] = form["ReportSalesStatus"].ToString();
            }

            ViewData["ReportRemittanceStatus"] = "";
            TempData["ReportRemittanceStatus"] = "";
            if (!string.IsNullOrEmpty(form["ReportRemittanceStatus"]))
            {
                ViewData["ReportRemittanceStatus"] = form["ReportRemittanceStatus"].ToString();
                TempData["ReportRemittanceStatus"] = form["ReportRemittanceStatus"].ToString();
            }

            ViewData["TransactionType"] = "";
            TempData["TransactionType"] = "";
            if (!string.IsNullOrEmpty(form["TransactionType"]))
            {
                ViewData["TransactionType"] = form["TransactionType"].ToString();
                TempData["TransactionType"] = form["TransactionType"].ToString();
            }

            ViewData["ReportAgents"] = "";
            TempData["ReportAgents"] = "";
            if (!string.IsNullOrEmpty(form["ReportAgents"]))
            {
                ViewData["ReportAgents"] = form["ReportAgents"].ToString();
                TempData["ReportAgents"] = form["ReportAgents"].ToString();
            }

            ViewData["ReportCountries"] = "";
            TempData["ReportCountries"] = "";
            if (!string.IsNullOrEmpty(form["ReportCountries"]))
            {
                ViewData["ReportCountries"] = form["ReportCountries"].ToString();
                TempData["ReportCountries"] = form["ReportCountries"].ToString();
            }

            ViewData["ReportCustomerType"] = "";
            TempData["ReportCustomerType"] = "";
            if (!string.IsNullOrEmpty(form["ReportCustomerType"]))
            {
                ViewData["ReportCustomerType"] = form["ReportCustomerType"].ToString();
                TempData["ReportCustomerType"] = form["ReportCustomerType"].ToString();
            }

            if (string.IsNullOrEmpty(form["ReportName"]))
            {
                ModelState.AddModelError("ReportName", "Report Name is required!");
            }
            else
            {
                List<string> reportNames = new List<string>();
                reportNames.Add("STS");
                reportNames.Add("PTS");
                reportNames.Add("BPB");
                reportNames.Add("PLS");
                reportNames.Add("STEC");
                reportNames.Add("STECB");
                reportNames.Add("GLPC");
                reportNames.Add("GLPCB");
                reportNames.Add("SWCD");
                reportNames.Add("OST");
                reportNames.Add("OPT");
                reportNames.Add("CustomerSummary");
                reportNames.Add("SMTS");
                reportNames.Add("STTS");

                if (!reportNames.Contains(form["ReportName"].ToString()))
                {
                    ModelState.AddModelError("ReportName", "Report Name is not valid!");
                }
            }

            if (form["ReportName"].ToString() != "OST" && form["ReportName"].ToString() != "OPT" && form["ReportName"].ToString() != "BPB")
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (string.IsNullOrEmpty(form["ReportFromDate"]))
                {
                    ModelState.AddModelError("ReportFromDate", "From Date is required!");
                }
                else
                {
                    try
                    {
                        fromDate = Convert.ToDateTime(form["ReportFromDate"]);
                    }
                    catch
                    {
                        ModelState.AddModelError("ReportFromDate", "From Date is invalid!");
                    }
                }

                if (string.IsNullOrEmpty(form["ReportToDate"]))
                {
                    ModelState.AddModelError("ReportToDate", "To Date is required!");
                }
                else
                {
                    try
                    {
                        toDate = Convert.ToDateTime(form["ReportToDate"]);
                    }
                    catch
                    {
                        ModelState.AddModelError("ReportToDate", "To Date is invalid!");
                    }
                }

                if (fromDate != null && toDate != null)
                {
                    if (fromDate > toDate)
                    {
                        ModelState.AddModelError("ReportFromDate", "From Date cannot be later than To Date!");
                        ModelState.AddModelError("ReportToDate", "To Date cannot be earlier than From Date!");
                    }
                }
            }
            else if (form["ReportName"].ToString() == "BPB")
            {
                DateTime? date = null;

                if (string.IsNullOrEmpty(form["ReportDate"]))
                {
                    ModelState.AddModelError("ReportDate", "Date is required!");
                }
                else
                {
                    try
                    {
                        date = Convert.ToDateTime(form["ReportDate"]);
                    }
                    catch
                    {
                        ModelState.AddModelError("ReportDate", "Date is invalid!");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                string report = form["ReportName"].ToString();
                return RedirectToAction(report);
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            //Check if today has closed end day trade
            DateTime current = DateTime.Now;

            DateTime currentActivation = DateTime.Now;

            IList<EndDayTrade> hasRecords = _endDayTradesModel.GetAll(currentActivation.ToString("dd/MM/yyyy"));

            if (hasRecords.Count == 0)
            {
                ViewData["HasClosedEndDayTrade"] = "Today's End of Day Trade has not closed!";
            }
            else
            {
                ViewData["HasClosedEndDayTrade"] = "Today's End of Day Trade has been last closed at " + hasRecords.OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault().CurrentActivationTime.ToString("dd/MM/yyyy HH:mm:ss") + "!";
            }

            Dropdown[] reportNameDDL = ReportNameDDL();
            ViewData["ReportNameDropdown"] = new SelectList(reportNameDDL, "val", "name", ViewData["ReportName"].ToString());

            Dropdown[] customerDDL = CustomerDDL();
            ViewData["ReportCustomerDropdown"] = new SelectList(customerDDL, "val", "name", ViewData["ReportCustomer"].ToString());
            ViewData["ReportMultipleCustomerDropdown"] = new MultiSelectList(customerDDL, "val", "name", ViewData["ReportMultipleCustomer"].ToString());

            Dropdown[] productDDL = ProductDDL();
            ViewData["ReportProductDropdown"] = new MultiSelectList(productDDL, "val", "name", ViewData["ReportProduct"].ToString().Split(','));

            Dropdown[] remittanceproductDDL = RemittanceProductDDL();
            ViewData["ReportRemittanceProductDropdown"] = new MultiSelectList(remittanceproductDDL, "val", "name", ViewData["ReportRemittanceProduct"].ToString().Split(','));

            Dropdown[] statusDDL = StatusDDL();
            ViewData["ReportStatusDropdown"] = new MultiSelectList(statusDDL, "val", "name", ViewData["ReportStatus"].ToString().Split(','));

            Dropdown[] salesStatusDDL = SalesStatusDDL();
            ViewData["ReportSalesStatusDropdown"] = new SelectList(salesStatusDDL, "val", "name", ViewData["ReportSalesStatus"].ToString());

            Dropdown[] remittanceStatusDDL = RemittanceStatusDDL();
            ViewData["ReportRemittanceStatusDropdown"] = new SelectList(remittanceStatusDDL, "val", "name", ViewData["ReportRemittanceStatus"].ToString());

            Dropdown[] transactionTypeDDL = TranscationTypeDDL();
            ViewData["ReportTranscationTypeDropdown"] = new MultiSelectList(transactionTypeDDL, "val", "name", ViewData["TransactionType"].ToString().Split(','));

            Dropdown[] agentsDDL = AgentDDL();
            ViewData["AgentsDropdown"] = new MultiSelectList(agentsDDL, "val", "name", ViewData["ReportAgents"].ToString().Split(','));

            Dropdown[] countriesDDL = CountryDDL();
            ViewData["CountriesDropdown"] = new MultiSelectList(countriesDDL, "val", "name", ViewData["ReportCountries"].ToString().Split(','));

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", ViewData["ReportCustomerType"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();

            return View();
        }

        //POST: CheckPendingDeleteSales
        [HttpPost]
        public string CheckPendingDeleteSales()
        {
            string result = "";

            try
            {
                EndDayTrade lastRecord = _endDayTradesModel.GetLastRecord();

                DateTime currentActivation = DateTime.Now;
                DateTime lastActivation = currentActivation.Date;

                List<string> acceptStatus = new List<string>() { "Completed", "Pending Delete GM Approval" };

                if (lastRecord != null)
                {
                    lastActivation = lastRecord.CurrentActivationTime;
                }
                else
                {
                    //Get Earliest Sales
                    Sale earliestSales = _salesModel.GetEarliestSales(acceptStatus);

                    if (earliestSales != null)
                    {
                        lastActivation = earliestSales.IssueDate;
                    }
                }

                List<Sale> pendingDeleteSales = _salesModel.GetPendingDeleteSales(lastActivation, currentActivation, "Buy");

                if (pendingDeleteSales.Count > 0)
                {
                    result = "{\"Result\":true,\"PendingDeleteSales\":true,\"Sales\":" + JsonConvert.SerializeObject(pendingDeleteSales.Select(e => e.MemoID).ToList()) + ",\"SaleIds\":\"" + String.Join("|", pendingDeleteSales.Select(e => e.ID).ToList()) + "\"}";
                }
                else
                {
                    result = "{\"Result\":true,\"PendingDeleteSales\":false}";
                }
            }
            catch (Exception e)
            {
                result = "{\"Result\":false,\"ErrorMessage\":\"" + e.Message + "\"}";
            }
            return result;
        }

        //GET: RevertPendingDeleteSales
        public ActionResult RevertPendingDeleteSales(string ids)
        {
            List<int> saleIds = ids.Split('|').Select(e => Convert.ToInt32(e)).ToList();

            foreach (int id in saleIds)
            {
                Sale sales = _salesModel.GetSingle(id);

                sales.Status = "Completed";

                bool result = _salesModel.Update(sales.ID, sales);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Sales";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Reverted Pending Delete Sale [" + sales.MemoID + "]";

                    bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                }
            }

            return RedirectToAction("CloseEndOfDayTrade");
        }

        //GET: CloseEndOfDayTrade
        public ActionResult CloseEndOfDayTrade(string execDate = null)
        {
            DateTime currentActivation = DateTime.Now;

            if (!string.IsNullOrEmpty(execDate))
            {
                currentActivation = Convert.ToDateTime(execDate);
            }

            IList<EndDayTrade> hasRecords = _endDayTradesModel.GetAll(currentActivation.ToString("dd/MM/yyyy"));

            int triggeringTime = 1;

            if (hasRecords.Count > 0)
            {
                bool hasDelete = false;

                //Delete current activated records
                foreach (EndDayTrade trade in hasRecords)
                {
                    triggeringTime = trade.TriggeringTime + 1;

                    bool result_delete = _endDayTradesModel.Delete(trade.ID);

                    if (result_delete)
                    {
                        if (!hasDelete)
                        {
                            hasDelete = true;
                        }
                    }
                }

                if (hasDelete)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "EndDayTrades";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted End of Day Trade";

                    bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                }
            }

            EndDayTrade lastRecord = _endDayTradesModel.GetLastRecord();

            DateTime lastActivation = currentActivation.Date;

            Product sgd = _productsModel.FindCurrencyCode("SGD");
            decimal closingBankAmount = sgd.ProductInventories[0].TotalInAccount;
            decimal closingCashAmount = sgd.ProductInventories[0].TotalInAccount;
            decimal openingBankAmount = sgd.ProductInventories[0].TotalInAccount;
            decimal openingCashAmount = sgd.ProductInventories[0].TotalInAccount;

            List<string> acceptStatus = new List<string>() { "Completed" };

            if (lastRecord != null)
            {
                lastActivation = lastRecord.CurrentActivationTime;
                openingBankAmount = lastRecord.ClosingBankAmount;
                openingCashAmount = lastRecord.ClosingCashAmount;
            }
            else
            {
                //Get Earliest Sales
                Sale earliestSales = _salesModel.GetEarliestSales(acceptStatus);

                if (earliestSales != null)
                {
                    lastActivation = earliestSales.IssueDate;
                }
            }

            IList<Product> products = _productsModel.GetAll();

            bool addTrade = false;
            bool addTradeTransactions = false;

            foreach (Product product in products)
            {
                List<string> desciptions = new List<string>();

                List<EndDayTradeTransaction> tradeTransactions = new List<EndDayTradeTransaction>();

                IList<SaleTransaction> transactions = _saleTransactionsModel.GetEndDayTradeTransactions(product.ID, lastActivation, currentActivation, "Buy", acceptStatus);

                decimal foreignCurrencyBal = product.ProductInventories[0].TotalInAccount;
                decimal averageRate = Convert.ToDecimal(product.BuyRate);
                decimal totalAmountForeign = 0;
                decimal totalAmountLocal = 0;

                if (transactions.Count > 0)
                {
                    string amountForeignFormat = GetDecimalFormat(product.Decimal);
                    string sgdFormat = GetDecimalFormat(sgdDp);

                    foreach (SaleTransaction transaction in transactions)
                    {
                        totalAmountForeign += transaction.AmountForeign;
                        totalAmountLocal += transaction.AmountLocal * product.Unit;
                        desciptions.Add(String.Format("{0}-{1}:{2}:{3}:{4}:{5}", transaction.Sales.MemoID, transaction.TransactionID, transaction.Products.Symbol + transaction.AmountForeign.ToString(amountForeignFormat), transaction.Rate.ToString(GetRateFormat(rateDP)), transaction.EncashmentRate != null ? Convert.ToDecimal(transaction.EncashmentRate).ToString(GetRateFormat(rateDP)) : "-", "SGD" + (transaction.AmountLocal * product.Unit).ToString(sgdFormat)));

                        tradeTransactions.Add(new EndDayTradeTransaction
                        {
                            SaleTransactionId = transaction.ID,
                            AmountForeign = transaction.AmountForeign,
                            Unit = transaction.Unit,
                            AmountLocal = transaction.AmountLocal
                        });
                    }
                }
                else
                {
                    desciptions.Add("No Transaction");
                }

                if (totalAmountForeign != 0)
                {
                    averageRate = totalAmountLocal / totalAmountForeign;
                }

                decimal closingBal = foreignCurrencyBal * averageRate;

                decimal openingForeignCurrencyBal = foreignCurrencyBal;
                decimal openingAveragePurchaseCost = averageRate;
                decimal openingBalanceAtAveragePurchase = closingBal;
                decimal openingProfitAmount = 0;
                decimal closingForeignCurrencyBal = foreignCurrencyBal;
                decimal closingAveragePurchaseCost = averageRate;
                decimal closingBalanceAtAveragePurchase = closingBal;

                EndDayTrade lastTrade = _endDayTradesModel.GetProductTrade(product.ID);

                if (lastTrade != null)
                {
                    openingForeignCurrencyBal = lastTrade.ClosingForeignCurrencyBalance;
                    openingAveragePurchaseCost = lastTrade.ClosingAveragePurchaseCost;
                    openingBalanceAtAveragePurchase = lastTrade.ClosingBalanceAtAveragePurchase;
                    openingProfitAmount = lastTrade.ClosingProfitAmount;
                }

                decimal closingProfitAmount = (openingForeignCurrencyBal * Convert.ToDecimal(sgd.BuyRate)) + closingBankAmount + closingCashAmount - product.ProductInventories[0].TotalInAccount * Convert.ToDecimal(sgd.BuyRate) - closingBal * Convert.ToDecimal(sgd.BuyRate);// Closing Balance formula

                EndDayTrade trade = new EndDayTrade();
                trade.LastActivationTime = lastActivation;
                trade.CurrentActivationTime = currentActivation;
                trade.TriggeringTime = triggeringTime;
                trade.CurrencyId = product.ID;
                trade.OpeningBankAmount = openingBankAmount;
                trade.OpeningCashAmount = openingCashAmount;
                trade.OpeningForeignCurrencyBalance = openingForeignCurrencyBal;
                trade.OpeningAveragePurchaseCost = openingAveragePurchaseCost;
                trade.OpeningBalanceAtAveragePurchase = openingBalanceAtAveragePurchase;
                trade.OpeningProfitAmount = openingProfitAmount;
                trade.ClosingBankAmount = closingBankAmount;
                trade.ClosingCashAmount = closingCashAmount;
                trade.ClosingForeignCurrencyBalance = closingForeignCurrencyBal;
                trade.ClosingAveragePurchaseCost = closingAveragePurchaseCost;
                trade.ClosingBalanceAtAveragePurchase = closingBalanceAtAveragePurchase;
                trade.ClosingProfitAmount = closingProfitAmount;
                trade.CurrentSGDBuyRate = Convert.ToDecimal(sgd.BuyRate);
                trade.Description = String.Join("|", desciptions);

                bool result = _endDayTradesModel.Add(trade);

                if (result)
                {
                    addTrade = true;

                    //Add Trade Transactions
                    if (tradeTransactions.Count > 0)
                    {
                        foreach (EndDayTradeTransaction tradeTransaction in tradeTransactions)
                        {
                            tradeTransaction.TradeId = trade.ID;

                            bool tradeTransaction_create_result = _endDayTradeTransactionsModel.Add(tradeTransaction);

                            if (tradeTransaction_create_result)
                            {
                                addTradeTransactions = true;
                            }
                        }
                    }
                }
            }

            if (addTrade)
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                string tableAffected = "EndDayTrades";
                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Closed End of Day Trade from " + lastActivation.ToString("dd/MM/yyyy HH:mm:ss") + " to " + currentActivation.ToString("dd/MM/yyyy HH:mm:ss");

                bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                if (addTradeTransactions)
                {
                    tableAffected = "EndDayTradeTransactions";
                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Closed End of Day Trade Transactions from " + lastActivation.ToString("dd/MM/yyyy HH:mm:ss") + " to " + currentActivation.ToString("dd/MM/yyyy HH:mm:ss");

                    bool log_tradeTransaction = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                }

                TempData.Add("Result", "success|Today's Sales have been successfully closed from " + lastActivation.ToString("dd/MM/yyyy HH:mm:ss") + " to " + currentActivation.ToString("dd/MM/yyyy HH:mm:ss") + "!");
            }
            else
            {
                TempData.Add("Result", "danger|No sales closed for today!");
            }

            return RedirectToAction("Select");
        }

        //GET: SetDateSession
        public void SetDateSession(string reportName, string reportCustomer, string reportProduct, string reportRemittanceProduct, string reportStatus, string reportSalesStatus, string reportRemittanceStatus, string reportDate, string reportFromDate, string reportToDate, string reportMultipleCustomer, string transactionType, string reportCountries, string reportCustomerType, string reportAgents)
        {
            TempData["ReportDate"] = reportDate;
            TempData["ReportFromDate"] = reportFromDate;
            TempData["ReportToDate"] = reportToDate;
            TempData["ReportCustomer"] = reportCustomer;
            TempData["ReportMultipleCustomer"] = !string.IsNullOrEmpty(reportMultipleCustomer) ? reportMultipleCustomer : "";
            TempData["ReportProduct"] = !string.IsNullOrEmpty(reportProduct) ? reportProduct : "";
            TempData["ReportRemittanceProduct"] = !string.IsNullOrEmpty(reportRemittanceProduct) ? reportRemittanceProduct : "";
            TempData["ReportStatus"] = !string.IsNullOrEmpty(reportStatus) ? reportStatus : "";
            TempData["ReportSalesStatus"] = !string.IsNullOrEmpty(reportSalesStatus) ? reportSalesStatus : "";
            TempData["ReportRemittanceStatus"] = !string.IsNullOrEmpty(reportRemittanceStatus) ? reportRemittanceStatus : "";
            TempData["TransactionType"] = !string.IsNullOrEmpty(transactionType) ? transactionType : "";
            TempData["ReportAgents"] = !string.IsNullOrEmpty(reportAgents) ? reportAgents : "";
            TempData["ReportCountries"] = !string.IsNullOrEmpty(reportCountries) ? reportCountries : "";
            TempData["ReportCustomerType"] = !string.IsNullOrEmpty(reportCustomerType) ? reportCustomerType : "";


            switch (reportName)
            {
                case "STS": ExportSTS(); break;
                case "PTS": ExportPTS(); break;
                case "BPB": ExportBPB(); break;
                case "SWCD": ExportSWCD(); break;
                case "OST": ExportOST(); break;
                case "OPT": ExportOPT(); break;
                case "CustomerSummary": ExportCustomerSummary(); break;
                case "SMTS": ExportSMTS(); break;
                case "SMTSS": ExportSMTSS(); break;
                case "STTS": ExportSTTS(); break;
                case "STTSS": ExportSTTSS(); break;
                case "RMTR": ExportRMTR(); break;
                case "RMTT": ExportRMTT(); break;
                default: break;
            }
        }

        //GET: STS
        public ActionResult STS(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportCustomer"].ToString();
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            TempData.Keep("ReportCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            if (!string.IsNullOrEmpty(reportSalesStatus))
            {
                acceptStatus.Add(reportSalesStatus);
                if (reportSalesStatus == "Completed")
                {
                    acceptStatus.Add("Pending Delete GM Approval");
                }
            }

            TempData.Keep("ReportSalesStatus");

            IPagedList<Sale> sales = _salesModel.GetLastApprovedSalesByDatePaged(fromDate, toDate, exceptionStatus, acceptStatus, page, pageSize, productIds, customerId, "Sell");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }
            ViewData["Sale"] = sales;

            //Get Grand Total on last page
            if (page == sales.PageCount)
            {
                decimal[] grandTotals = _salesModel.GetLastApprovedReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");

                ViewData["GrandTotal"] = grandTotals;
            }

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: STTS
        public ActionResult STTS(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var dt_fromDate = Convert.ToDateTime(fromDate + " 00:00:00");
            var dt_toDate = Convert.ToDateTime(toDate + " 23:59:59");
            TempData.Keep("ReportSalesStatus");


            var transactionSummaries = new List<TransactionSummary>();
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            if (productIds.Count > 0)
            {
                foreach (var data in getSaleList)
                {
                    data.SaleTransactions = data.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                }
                foreach (var data in getRemittanceList)
                {
                    data.RemittanceOders = data.RemittanceOders.Where(e => productIds.Contains(e.PayCurrency)).ToList();
                }
            }
            var finalCustomerList = new List<int>();
            if (getSaleList.Count > 0)
            {
                var getCustomerList = getSaleList.Select(s => s.CustomerParticularId).Distinct().ToList();
                finalCustomerList.AddRange(getCustomerList);
                getSaleList.OrderBy(e => e.CustomerParticularId).ThenByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                var getCustomerList2 = getRemittanceList.Select(s => s.CustomerParticularId).Distinct().ToList();
                finalCustomerList.AddRange(getCustomerList2);
                getRemittanceList.OrderBy(e => e.CustomerParticularId).ThenByDescending(e => e.LastApprovalOn).ToList();
            }
            finalCustomerList = finalCustomerList.Distinct().ToList();
            IPagedList<int> customerPage = finalCustomerList.ToPagedList(page, 2);

            ViewData["RemittanceList"] = getRemittanceList;
            ViewData["SaleList"] = getSaleList;
            ViewData["CustomerPage"] = customerPage;

            //Get Grand Total on last page
            //if (page == sales.PageCount)
            //{
            //    decimal[] grandTotals = _salesModel.GetLastApprovedReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");

            //    ViewData["GrandTotal"] = grandTotals;
            //}

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportSTS
        public void ExportSMTS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer) && reportCustomer != "null")
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
            {
                customerList.Add(customerId);
            }
            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //TempData.Keep("ReportSalesStatus");
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //    TempData.Keep("ReportSalesStatus");
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            if (getSaleList.Count > 0)
            {
                getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _productsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                foreach (var sale in getSaleList)
                {
                    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                }
                foreach (var remittance in getRemittanceList)
                {
                    remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.PayCurrencyDecimal.CurrencyCode)).ToList();
                }

            }

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = 0,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = 0,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }
            //IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            //foreach (Sale sale in sales)
            //{
            //    sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            //}

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales by Memo Transactions Type Summary Details");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES BY MEMO TRANSACTIONS TYPE SUMMARY DETAILS";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                //saleWS.Cells["A4:I4"].Merge = true;
                //saleWS.Cells[4, 1].Style.Font.Bold = true;
                //saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //saleWS.Cells[4, 1].Value = String.Format("TRANSACTION TYPE: {0}", !string.IsNullOrEmpty(transactionType) ? transactionType == "Pending" ? "Pending Only" : "Completed Only" : "All").ToUpper();

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "TRANSACTION TYPE";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "A/C NO";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "CONTACT PERSON";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "FOREIGN CURR / GET AMOUNT";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "TRANSACTION FEE";
                saleWS.Cells[6, 8].Style.Font.Bold = true;
                saleWS.Cells[6, 8].Value = "LOCAL CURR / PAY AMOUNT";
                saleWS.Cells[6, 9].Style.Font.Bold = true;
                saleWS.Cells[6, 9].Value = "PAYMENT MODE";
                saleWS.Cells[6, 10].Style.Font.Bold = true;
                saleWS.Cells[6, 10].Value = "REFERENCE NO";
                saleWS.Cells[6, 11].Style.Font.Bold = true;
                saleWS.Cells[6, 11].Value = "AMOUNT";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;

                List<string> distinctProduct = new List<string>();
                foreach (var date in filtersaleDate)
                {
                    var currentMonthSales = getSaleList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn).ToList();
                    var currentMonthRemittance = getRemittanceList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn).ToList();
                    foreach (Sale sale in currentMonthSales)
                    {
                        decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                        grandTotalForeign += sale.SaleTransactions.Sum(e => e.AmountForeign);
                        grandTotalLocal += sale.SaleTransactions.Sum(e => e.AmountLocal);
                        var getCurrencyID = sale.SaleTransactions.Select(e => e.CurrencyId).ToList();
                        foreach (var data in getCurrencyID)
                        {
                            var currencyCode = _productsModel.GetSingle(data).CurrencyCode;
                            distinctProduct.Add(currencyCode);
                        }
                        //distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 3].Value = sale.TransactionType;
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.CustomerCode;

                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Company_RegisteredName;
                        }
                        else
                        {
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Natural_Name;
                        }

                        saleWS.Cells[saleRow, 8].Value = totalSales.ToString(sgdFormat);

                        List<string> paymentModes = new List<string>();

                        if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                        {
                            paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                            foreach (string mode in paymentModes)
                            {
                                saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                                if (mode == "Cash")
                                {
                                    saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                                }
                                else if (mode == "Cheque 1")
                                {
                                    saleWS.Cells[saleRow, 9].Value = sale.Cheque1No;
                                    saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                                }
                                else if (mode == "Cheque 2")
                                {
                                    saleWS.Cells[saleRow, 9].Value = sale.Cheque2No;
                                    saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                                }
                                else if (mode == "Cheque 3")
                                {
                                    saleWS.Cells[saleRow, 9].Value = sale.Cheque3No;
                                    saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                                }
                                else
                                {
                                    saleWS.Cells[saleRow, 9].Value = sale.BankTransferNo;
                                    saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                                }

                                saleRow++;
                            }
                        }
                        else
                        {
                            saleRow++;
                        }

                        foreach (SaleTransaction transaction in sale.SaleTransactions)
                        {
                            saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode;
                            saleWS.Cells[saleRow, 2].Value = transaction.Products.Unit.ToString("#,##0");
                            saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                            saleWS.Cells[saleRow, 6].Value = transaction.AmountForeign.ToString(sgdFormat);
                            saleWS.Cells[saleRow, 8].Value = transaction.AmountLocal.ToString(sgdFormat);
                            saleRow++;
                        }

                        saleRow++;
                    }
                    foreach (Remittances sale in currentMonthRemittance)
                    {
                        decimal totalSales = 0.00M;
                        foreach (var transaction in sale.RemittanceOders)
                        {
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            decimal convertToSGD = 0;

                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                            }
                            else
                            {
                                var payAmount = transaction.PayAmount;
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                decimal? rate = 1 / payRate;
                                //if (getRate > 0)
                                //{
                                //    payRate = transaction.Rate * getRate;
                                //}

                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    //convertToSGD = convertToSGD / 1;
                                }

                            }
                            totalSales += convertToSGD;
                        }
                        grandTotalLocal += totalSales;
                        grandTotalForeign += sale.RemittanceOders.Sum(e => e.GetAmount);


                        distinctProduct.AddRange(sale.RemittanceOders.Select(e => e.GetCurrencyDecimal.CurrencyCode));

                        //distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 3].Value = "Remittance";
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.CustomerCode;

                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Company_RegisteredName;
                        }
                        else
                        {
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Natural_Name;
                        }

                        saleWS.Cells[saleRow, 8].Value = totalSales.ToString(sgdFormat);

                        List<string> paymentModes = new List<string>();

                        //if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                        //{
                        //    paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                        //    foreach (string mode in paymentModes)
                        //    {
                        //        saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                        //        if (mode == "Cash")
                        //        {
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                        //        }
                        //        else if (mode == "Cheque 1")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque1No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 2")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque2No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 3")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque3No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                        //        }
                        //        else
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.BankTransferNo;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                        //        }

                        //        saleRow++;
                        //    }
                        //}
                        //else
                        //{
                        saleRow++;
                        //}

                        foreach (RemittanceOrders transaction in sale.RemittanceOders)
                        {
                            var mode = "";
                            var depositAccount = "";
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var paymentID = Convert.ToInt32(transaction.PayPaymentType);
                                mode = context.PaymentModeLists.Where(e => e.ID == paymentID).FirstOrDefault().Name;
                                if (transaction.PayDepositAccount != 0)
                                {
                                    depositAccount = context.PayBankLists.Where(e => e.ID == transaction.PayDepositAccount).FirstOrDefault().BankName;
                                }
                            }
                            saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                            var payAmount = transaction.PayAmount;
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            decimal convertToSGD = 0;

                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                            }
                            else
                            {
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                decimal? rate = 1 / payRate;
                                //if (getRate > 0)
                                //{
                                //    payRate = transaction.Rate * getRate;
                                //}

                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    //convertToSGD = convertToSGD / 1;
                                }
                            }

                            if (transaction.PayPaymentType == "1")
                            {
                                saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat);
                            }
                            else if (transaction.PayPaymentType == "2")
                            {
                                saleWS.Cells[saleRow, 10].Value = transaction.ChequeNo;
                                saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                            }
                            else if (transaction.PayPaymentType == "3")
                            {
                                saleWS.Cells[saleRow, 10].Value = depositAccount + " " + transaction.BankTransferNo;
                                saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                            }

                            saleWS.Cells[saleRow, 1].Value = transaction.PayCurrencyDecimal.CurrencyCode;
                            saleWS.Cells[saleRow, 2].Value = transaction.PayAmount;
                            saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                            saleWS.Cells[saleRow, 5].Value = transaction.GetCurrencyDecimal.CurrencyCode;
                            saleWS.Cells[saleRow, 6].Value = transaction.GetAmount.ToString(GetDecimalFormat(2));
                            saleWS.Cells[saleRow, 7].Value = transaction.Fee.ToString(rateFormat);
                            saleWS.Cells[saleRow, 8].Value = convertToSGD.ToString(sgdFormat);
                            saleRow++;
                        }

                        saleRow++;
                    }

                }


                if (getSaleList.Count > 0 || getRemittanceList.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 5].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 6].Value = grandTotalForeign.ToString("#,##0.00######");
                    }
                    saleWS.Cells[saleRow, 8].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 8].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 8].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 8].Value = grandTotalLocal.ToString(sgdFormat);
                }
                saleRow++;
                saleRow++;

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-by-memo-transactions-type-summary-details-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportRMTR
        public void ExportRMTR()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            customerList.Add(0);

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportRemittanceProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            string reportCountries = TempData["ReportCountries"].ToString();
            List<int> countryIds = new List<int>();
            if (!string.IsNullOrEmpty(reportCountries))
            {
                countryIds = reportCountries.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
                countryIds.Add(0);

            TempData.Keep("ReportCountries");

            string reportAgents = TempData["ReportAgents"].ToString();
            List<int> agentIds = new List<int>();
            if (!string.IsNullOrEmpty(reportAgents))
            {
                agentIds = reportAgents.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportAgents");

            string reportCustomerType = TempData["ReportCustomerType"].ToString();
            TempData.Keep("ReportCustomerType");

            string reportStatus = "";
            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //TempData.Keep("ReportSalesStatus");
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //    TempData.Keep("ReportSalesStatus");
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            //var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList3(fromDate, toDate, productIds, transactionList, customerList, reportStatus, countryIds, reportCustomerType, agentIds);

            //if (getSaleList.Count > 0)
            //{
            //    getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            //}
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _remittanceProductsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                //foreach (var sale in getSaleList)
                //{
                //    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                //}
                if (!countryIds.Contains(0))
                {
                    foreach (var remittance in getRemittanceList)
                    {
                        remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.GetCurrencyDecimal.CurrencyCode) && countryIds.Contains(e.BeneficiaryBankCountry)).ToList();
                    }
                }
                else
                {
                    foreach (var remittance in getRemittanceList)
                    {
                        remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.GetCurrencyDecimal.CurrencyCode)).ToList();
                    }
                }

            }
            else
            {
                if (!countryIds.Contains(0))
                {
                    foreach (var remittance in getRemittanceList)
                    {

                        remittance.RemittanceOders = remittance.RemittanceOders.Where(e => countryIds.Contains(e.BeneficiaryBankCountry)).ToList();
                    }
                }
            }

            var saleDateList = new List<MonthlySalesDate>();
            //var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            //if (distinctDateList.Count > 0)
            //{
            //    foreach (var data in distinctDateList)
            //    {
            //        var model = new MonthlySalesDate()
            //        {
            //            CustomerParticularId = 0,
            //            LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
            //        };
            //        saleDateList.Add(model);
            //    }
            //}
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = 0,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }
            //IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            //foreach (Sale sale in sales)
            //{
            //    sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            //}

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Remittance Records");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "REMITTANCE RECORDS";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                //saleWS.Cells["A4:I4"].Merge = true;
                //saleWS.Cells[4, 1].Style.Font.Bold = true;
                //saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //saleWS.Cells[4, 1].Value = String.Format("TRANSACTION TYPE: {0}", !string.IsNullOrEmpty(transactionType) ? transactionType == "Pending" ? "Pending Only" : "Completed Only" : "All").ToUpper();

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "ISSUE DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "CUSTOMER";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "AGENT";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "ADDRESS / CONTACT NO";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "POSTAL CODE";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "PAY AMOUNT in SGD";
                saleWS.Cells[6, 8].Style.Font.Bold = true;
                saleWS.Cells[6, 8].Value = "CURRENT PAY RATE";
                saleWS.Cells[6, 9].Style.Font.Bold = true;
                saleWS.Cells[6, 9].Value = "GET AMOUNT";
                saleWS.Cells[6, 10].Style.Font.Bold = true;
                saleWS.Cells[6, 10].Value = "GET CURRENCY";
                saleWS.Cells[6, 11].Style.Font.Bold = true;
                saleWS.Cells[6, 11].Value = "BENEFICIARY's NAME";
                saleWS.Cells[6, 12].Style.Font.Bold = true;
                saleWS.Cells[6, 12].Value = "COUNTRY REMITTED";
                saleWS.Cells[6, 13].Style.Font.Bold = true;
                saleWS.Cells[6, 13].Value = "DATE REMITTED";
                saleWS.Cells[6, 14].Style.Font.Bold = true;
                saleWS.Cells[6, 14].Value = "DATE RECEIVED";
                saleWS.Cells[6, 15].Style.Font.Bold = true;
                saleWS.Cells[6, 15].Value = "LAST APPROVED DATE";
                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;

                List<string> distinctProduct = new List<string>();
                foreach (var date in filtersaleDate)
                {
                    //var currentMonthSales = getSaleList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn).ToList();
                    var currentMonthRemittance = getRemittanceList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn).ToList();
                    //grandTotalLocal = 0;
                    //grandTotalForeign = 0;

                    foreach (Remittances sale in currentMonthRemittance)
                    {
                        //string getAmount = "";
                        //string getCurrency = "";
                        decimal totalSales = 0.00M;
                        //double rowWidth = 26;
                        foreach (var transaction in sale.RemittanceOders)
                        {
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            decimal convertToSGD = 0;

                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                            }
                            else
                            {
                                var payAmount = transaction.PayAmount;
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                decimal? rate = 1 / payRate;
                                //if (getRate > 0)
                                //{
                                //    payRate = transaction.Rate * getRate;
                                //}

                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    //convertToSGD = convertToSGD / 1;
                                }

                            }
                            totalSales += convertToSGD;
                            //if (!string.IsNullOrEmpty(getAmount))
                            //    getAmount = transaction.GetAmount.ToString(GetDecimalFormat(transaction.GetCurrencyDecimal.ProductDecimal));
                            //else
                            //    getAmount = getAmount + "\n\n" + transaction.GetAmount.ToString(GetDecimalFormat(transaction.GetCurrencyDecimal.ProductDecimal));
                            //if (!string.IsNullOrEmpty(getCurrency))
                            //    getCurrency = transaction.GetCurrencyDecimal.CurrencyCode;
                            //else
                            //    getCurrency = getCurrency + "\n\n" + transaction.GetCurrencyDecimal.CurrencyCode;
                            //rowWidth = rowWidth += 10;
                        }
                        grandTotalLocal += totalSales;
                        grandTotalForeign += sale.RemittanceOders.Sum(e => e.GetAmount);


                        distinctProduct.AddRange(sale.RemittanceOders.Select(e => e.GetCurrencyDecimal.CurrencyCode));

                        //distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));
                        var getAgentName = "-";
                        if (sale.AgentId != 0)
                        {
                            getAgentName = sale.Agent.CompanyName;
                        }
                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.IssueDate.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 5].Value = sale.Address1 + " " + sale.Address2 + " " + sale.Address3 + " / " + sale.ContactNo;
                        saleWS.Cells[saleRow, 4].Value = getAgentName;

                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.Company_RegisteredName + "\n\n(" + sale.CustomerParticulars.CustomerType + ")";
                            saleWS.Cells[saleRow, 3].Style.WrapText = true;
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Company_PostalCode;
                        }
                        else
                        {
                            saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.Natural_Name + "\n\n(" + sale.CustomerParticulars.CustomerType + ")";
                            saleWS.Cells[saleRow, 3].Style.WrapText = true;
                            saleWS.Cells[saleRow, 6].Value = sale.CustomerParticulars.Mailing_PostalCode;
                        }
                        var approvalHistory = new List<ApprovalHistorys>();
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var approvalHistoryList = context.ApprovalHistorys.Where(e => e.ObjectId == sale.ID && e.Application == "RemittanceSale").ToList();
                            approvalHistory.AddRange(approvalHistoryList);
                        }
                        var checkfund = approvalHistory.Where(e => e.Description.Contains("Check Funds")).FirstOrDefault();
                        var DateRemitted = "";
                        if (checkfund != null)
                        {
                            DateRemitted = checkfund.DateTimeAction.ToString("dd-MM-yyyy");
                        }

                        var checkCompleted = approvalHistory.Where(e => e.Description.Contains("Completed")).FirstOrDefault();
                        var DateReceived = "";
                        if (checkCompleted != null)
                        {
                            DateReceived = checkCompleted.DateTimeAction.ToString("dd-MM-yyyy");
                        }
                        //saleWS.Cells[saleRow, 9].Value = getAmount;
                        //saleWS.Cells[saleRow, 9].Style.WrapText = true;
                        //saleWS.Cells[saleRow, 10].Value = getCurrency;
                        //saleWS.Cells[saleRow, 10].Style.WrapText = true;
                        //saleWS.Row(saleRow).Height = rowWidth;
                        //var getFirstRO = sale.RemittanceOders.FirstOrDefault();
                        //if (getFirstRO != null)
                        //    saleWS.Cells[saleRow, 8].Value = sale.RemittanceOders.FirstOrDefault().currentPayRate.ToString(GetDecimalFormat(12));
                        saleWS.Cells[saleRow, 7].Value = totalSales.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 7].Value = totalSales.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 13].Value = DateRemitted;
                        saleWS.Cells[saleRow, 14].Value = DateReceived;
                        saleWS.Cells[saleRow, 15].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");

                        List<string> paymentModes = new List<string>();

                        saleRow++;
                        //}

                        foreach (RemittanceOrders transaction in sale.RemittanceOders)
                        {
                            var payAmount = transaction.PayAmount;
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            decimal convertToSGD = 0;

                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                            }
                            else
                            {
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                //if (getRate > 0)
                                //{
                                //    payRate = transaction.Rate * getRate;
                                //}
                                decimal? rate = 1 / payRate;
                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    convertToSGD = convertToSGD / 1;
                                }
                            }

                            //if (transaction.PayPaymentType == "1")
                            //{
                            //    saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat);
                            //}
                            //else if (transaction.PayPaymentType == "2")
                            //{
                            //    saleWS.Cells[saleRow, 10].Value = transaction.ChequeNo;
                            //    saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                            //}
                            //else if (transaction.PayPaymentType == "3")
                            //{
                            //    saleWS.Cells[saleRow, 10].Value = depositAccount + " " + transaction.BankTransferNo;
                            //    saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                            //}

                            //saleWS.Cells[saleRow, 8].Value = transaction.PayCurrencyDecimal.CurrencyCode;
                            //saleWS.Cells[saleRow, 6].Value = transaction.PayAmount;
                            //saleWS.Cells[saleRow, 5].Value = transaction.Rate.ToString(rateFormat);
                            saleWS.Cells[saleRow, 10].Value = transaction.GetCurrencyDecimal.CurrencyCode;
                            saleWS.Cells[saleRow, 9].Value = transaction.GetAmount.ToString(GetDecimalFormat(2));
                            saleWS.Cells[saleRow, 8].Value = transaction.currentPayRate.ToString(GetDecimalFormat(12));
                            //saleWS.Cells[saleRow, 7].Value = transaction.Fee.ToString(rateFormat);
                            saleWS.Cells[saleRow, 7].Value = convertToSGD.ToString(sgdFormat);
                            saleWS.Cells[saleRow, 11].Value = transaction.BeneficiaryFullName;
                            string beneficiaryBankCountry = "";
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var getBankCountryName = context.Countries.Where(e => e.ID == transaction.BeneficiaryBankCountry).FirstOrDefault().Name;
                                if (getBankCountryName != "Others")
                                {
                                    beneficiaryBankCountry = getBankCountryName;
                                }
                                else
                                {
                                    beneficiaryBankCountry = transaction.BankCountryIfOthers;
                                }
                            }
                            saleWS.Cells[saleRow, 12].Value = beneficiaryBankCountry;
                            saleRow++;
                        }

                        saleRow++;
                    }

                }

                if (/*getSaleList.Count > 0 || */getRemittanceList.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 6].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 9].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 9].Value = grandTotalForeign.ToString("#,##0.00######");
                    }
                    saleWS.Cells[saleRow, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 7].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 7].Value = grandTotalLocal.ToString(sgdFormat);
                }
                saleRow++;
                saleRow++;

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col3 = saleWS.Column(3).Width;
                if (width_col3 < 22)
                {
                    saleWS.Column(3).Width = 22;
                    saleWS.Column(3).Style.WrapText = true;
                }
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col5 = saleWS.Column(5).Width;
                if (width_col5 > 32)
                {
                    saleWS.Column(5).Width = 32;
                    saleWS.Column(5).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }
                double width_col11 = saleWS.Column(11).Width;
                if (width_col11 > 15)
                {
                    saleWS.Column(8).Width = 15;
                    saleWS.Column(8).Style.WrapText = true;
                }
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=remittance-records-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportRMTT
        public void ExportRMTT()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (reportCustomer != "null")
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
                customerList.Add(0);

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportRemittanceProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportRemittanceProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");
            string reportStatus = TempData["ReportRemittanceStatus"].ToString();
            TempData.Keep("ReportRemittanceStatus");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryGetCurrencyList(fromDate, toDate, productIds, transactionList, customerList, reportStatus);
            if (string.IsNullOrEmpty(reportStatus))
            {
                reportStatus = "All Sales";
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _remittanceProductsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                //foreach (var sale in getSaleList)
                //{
                //    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                //}
                foreach (var remittance in getRemittanceList)
                {
                    remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.GetCurrencyDecimal.CurrencyCode)).ToList();
                }

            }

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = 0,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }
            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Remittance Transactions");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["I1:M1"].Merge = true;
                saleWS.Cells[1, 9].Style.Font.Bold = true;
                saleWS.Cells[1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 9].Value = exportDate;

                saleWS.Cells["A2:M2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "REMITTANCE TRANSACTIONS";
                saleWS.Cells["A3:M3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = reportStatus;
                saleWS.Cells["A4:M4"].Merge = true;
                saleWS.Cells[4, 1].Style.Font.Bold = true;
                saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[4, 1].Value = "FROM  " + fromDate + " TO " + toDate;

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "CUSTOMER NAME";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "GET AMOUNT";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "GET CURRENCY";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "REMITTANCE AMOUNT in SGD";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "COST OF REMITTANCE in SGD";
                saleWS.Cells[6, 8].Style.Font.Bold = true;
                saleWS.Cells[6, 8].Value = "REMITTANCE INCOME/GAIN";
                saleWS.Cells[6, 9].Style.Font.Bold = true;
                saleWS.Cells[6, 9].Value = "PAY CURRENCY";
                saleWS.Cells[6, 10].Style.Font.Bold = true;
                saleWS.Cells[6, 10].Value = "TRANSACTION FEE";
                saleWS.Cells[6, 11].Style.Font.Bold = true;
                saleWS.Cells[6, 11].Value = "AGENT FEE";
                saleWS.Cells[6, 12].Style.Font.Bold = true;
                saleWS.Cells[6, 12].Value = "CURRENT PAY RATE";
                saleWS.Cells[6, 13].Style.Font.Bold = true;
                saleWS.Cells[6, 13].Value = "REMARKS";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                decimal totalcostRemittance = 0;
                decimal totalGain = 0;
                decimal grandtotalTransactionFee = 0;
                decimal grandtotalAgentFee = 0;

                List<string> distinctProduct = new List<string>();
                foreach (var date in filtersaleDate)
                {
                    //grandTotalForeign = 0;
                    //grandTotalLocal = 0;
                    //totalcostRemittance = 0;
                    //totalGain = 0;
                    //grandtotalTransactionFee = 0;
                    var currentMonthRemittance = getRemittanceList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn).ToList();
                    double rowWidth = 26;
                    string getAmount = "";
                    string getCurrency = "";
                    string payCurrency = "";
                    foreach (Remittances sale in currentMonthRemittance)
                    {
                        getAmount = "";
                        getCurrency = "";
                        payCurrency = "";
                        rowWidth = 23;
                        decimal totalSales = 0.00M;
                        decimal totalTransactionFee = 0.00M;
                        int countRO = 1;
                        foreach (var transaction in sale.RemittanceOders)
                        {
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            decimal convertToSGD = 0;
                            decimal convertTransactionFeeToSGD = 0;

                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                                convertTransactionFeeToSGD = transaction.Fee;
                            }
                            else
                            {
                                var payAmount = transaction.PayAmount;
                                var transactionfee = transaction.Fee;
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                decimal? rate = 1 / payRate;
                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    convertToSGD = convertToSGD / 1;
                                    convertTransactionFeeToSGD = Convert.ToDecimal(transactionfee / rate);
                                }

                            }
                            totalSales += Convert.ToDecimal(convertToSGD.ToString(sgdFormat));
                            totalTransactionFee += Convert.ToDecimal(convertTransactionFeeToSGD.ToString(sgdFormat));

                            if (string.IsNullOrEmpty(getAmount))
                                getAmount = transaction.GetAmount.ToString(GetDecimalFormat(transaction.GetCurrencyDecimal.ProductDecimal));
                            else
                                getAmount = getAmount + "\n\n" + transaction.GetAmount.ToString(GetDecimalFormat(transaction.GetCurrencyDecimal.ProductDecimal));
                            if (string.IsNullOrEmpty(getCurrency))
                                getCurrency = transaction.GetCurrencyDecimal.CurrencyCode;
                            else
                                getCurrency = getCurrency + "\n\n" + transaction.GetCurrencyDecimal.CurrencyCode;
                            if (countRO != 1)
                                rowWidth = rowWidth += 20;
                            if (string.IsNullOrEmpty(payCurrency))
                                payCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            else
                                payCurrency = payCurrency + "\n\n" + transaction.PayCurrencyDecimal.CurrencyCode;
                            countRO++;
                        }
                        grandTotalLocal += totalSales;
                        grandtotalTransactionFee += totalTransactionFee;
                        grandTotalForeign += sale.RemittanceOders.Sum(e => e.GetAmount);
                        var profitRemittance = totalSales - sale.CostPrice;
                        totalcostRemittance += sale.CostPrice;
                        if (sale.AgentFee != null)
                            grandtotalAgentFee += Convert.ToDecimal(sale.AgentFee);
                        totalGain += profitRemittance;
                        distinctProduct.AddRange(sale.RemittanceOders.Select(e => e.GetCurrencyDecimal.CurrencyCode));
                        var customerName = "";
                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            customerName = /*sale.CustomerParticulars.CustomerCode + " - " + */sale.CustomerParticulars.Company_RegisteredName;
                        }
                        else
                        {
                            customerName = /*sale.CustomerParticulars.CustomerCode + " - " + */sale.CustomerParticulars.Natural_Name;
                        }
                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 3].Value = customerName;
                        saleWS.Cells[saleRow, 3].Style.WrapText = true;
                        saleWS.Cells[saleRow, 4].Value = getAmount;
                        saleWS.Cells[saleRow, 4].Style.WrapText = true;
                        saleWS.Cells[saleRow, 5].Value = getCurrency;
                        saleWS.Cells[saleRow, 5].Style.WrapText = true;
                        saleWS.Row(saleRow).Height = rowWidth;
                        saleWS.Cells[saleRow, 6].Value = totalSales.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 7].Value = sale.CostPrice.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 8].Value = profitRemittance.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 9].Value = payCurrency;
                        saleWS.Cells[saleRow, 9].Style.WrapText = true;
                        saleWS.Cells[saleRow, 10].Value = totalTransactionFee.ToString(sgdFormat);
                        decimal agentFee = 0;
                        if (sale.AgentFee != null)
                            agentFee = Convert.ToDecimal(sale.AgentFee);
                        saleWS.Cells[saleRow, 11].Value = agentFee.ToString(sgdFormat);
                        var getFirstRO = sale.RemittanceOders.FirstOrDefault();
                        if (getFirstRO != null)
                            saleWS.Cells[saleRow, 12].Value = sale.RemittanceOders.FirstOrDefault().currentPayRate.ToString(GetDecimalFormat(12));
                        List<string> paymentModes = new List<string>();

                        saleRow++;
                        //saleRow++;
                    }

                    //if (getRemittanceList.Count > 0)
                    //{
                    //    saleRow--;
                    //    saleWS.Cells[saleRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    //    saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 5].Value = "GRAND TOTAL";
                    //    saleWS.Cells[saleRow, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 7].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 7].Value = totalcostRemittance.ToString(sgdFormat);
                    //    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                    //    saleWS.Cells[saleRow, 8].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 8].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 8].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 8].Value = totalGain.ToString(sgdFormat);
                    //    saleWS.Cells[saleRow, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 9].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 9].Value = grandtotalTransactionFee.ToString(sgdFormat);
                    //    saleWS.Cells[saleRow, 10].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 10].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 10].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 10].Value = grandtotalAgentFee.ToString(sgdFormat);
                    //}
                    //saleRow++;
                    saleRow++;

                }

                //grand total
                saleWS.Cells[saleRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 5].Value = "GRAND TOTAL";
                saleWS.Cells[saleRow, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 7].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 7].Value = totalcostRemittance.ToString(sgdFormat);
                saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                saleWS.Cells[saleRow, 8].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 8].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 8].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 8].Value = totalGain.ToString(sgdFormat);
                saleWS.Cells[saleRow, 10].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 10].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 10].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 10].Value = grandtotalTransactionFee.ToString(sgdFormat);
                saleWS.Cells[saleRow, 11].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 11].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                saleWS.Cells[saleRow, 11].Style.Font.Bold = true;
                saleWS.Cells[saleRow, 11].Value = grandtotalAgentFee.ToString(sgdFormat);
                //grand total

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col6 = saleWS.Column(6).Width;
                if (width_col6 > 10)
                {
                    saleWS.Column(6).Width = 10;
                    saleWS.Column(6).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 11;
                    saleWS.Column(8).Style.WrapText = true;
                }
                double width_col9 = saleWS.Column(10).Width;
                if (width_col9 > 10)
                {
                    saleWS.Column(10).Width = 10;
                    saleWS.Column(10).Style.WrapText = true;
                }
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=remitance-transactions-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportSTS
        public void ExportSMTSS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer) && reportCustomer != "null")
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
            {
                customerList.Add(customerId);
            }

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //TempData.Keep("ReportSalesStatus");
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //    TempData.Keep("ReportSalesStatus");
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            if (getSaleList.Count > 0)
            {
                getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _productsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                foreach (var sale in getSaleList)
                {
                    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                }
                foreach (var remittance in getRemittanceList)
                {
                    remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.PayCurrencyDecimal.CurrencyCode)).ToList();
                }

            }

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }
            List<int> getCustomerList = new List<int>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (getCustomerList.Count > 0)
                    {
                        var check = getCustomerList.Where(e => e == data.CustomerParticularId).Count();
                        if (check < 1)
                        {
                            getCustomerList.Add(data.CustomerParticularId);
                        }
                    }
                    else
                    {
                        getCustomerList.Add(data.CustomerParticularId);
                    }
                }
            }
            //IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            //foreach (Sale sale in sales)
            //{
            //    sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            //}

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales by Memo Transactions Type Summary");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES BY MEMO TRANSACTIONS TYPE SUMMARY";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                //saleWS.Cells["A4:I4"].Merge = true;
                //saleWS.Cells[4, 1].Style.Font.Bold = true;
                //saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //saleWS.Cells[4, 1].Value = String.Format("TRANSACTION TYPE: {0}", !string.IsNullOrEmpty(transactionType) ? transactionType == "Pending" ? "Pending Only" : "Completed Only" : "All").ToUpper();

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "CUSTOMER";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "DATE";
                //saleWS.Cells[6, 3].Style.Font.Bold = true;
                //saleWS.Cells[6, 3].Value = "TRANSACTION TYPE";
                //saleWS.Cells[6, 4].Style.Font.Bold = true;
                //saleWS.Cells[6, 4].Value = "A/C NO";
                //saleWS.Cells[6, 5].Style.Font.Bold = true;
                //saleWS.Cells[6, 5].Value = "CONTACT PERSON";
                //saleWS.Cells[6, 6].Style.Font.Bold = true;
                //saleWS.Cells[6, 6].Value = "FOREIGN CURR / GET AMOUNT";
                //saleWS.Cells[6, 7].Style.Font.Bold = true;
                //saleWS.Cells[6, 7].Value = "TRANSACTION FEE";
                //saleWS.Cells[6, 8].Style.Font.Bold = true;
                //saleWS.Cells[6, 8].Value = "LOCAL CURR / PAY AMOUNT";
                //saleWS.Cells[6, 9].Style.Font.Bold = true;
                //saleWS.Cells[6, 9].Value = "PAYMENT MODE";
                //saleWS.Cells[6, 10].Style.Font.Bold = true;
                //saleWS.Cells[6, 10].Value = "REFERENCE NO";
                //saleWS.Cells[6, 11].Style.Font.Bold = true;
                //saleWS.Cells[6, 11].Value = "AMOUNT";

                int saleRow = 8;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                int col = 2;
                List<string> distinctProduct = new List<string>();
                foreach (var date in filtersaleDate)
                {
                    saleWS.Cells[7, col].Value = date.LastApprovalOn;

                    foreach (var customerID in getCustomerList)
                    {
                        var currentMonthSales = getSaleList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn && e.CustomerParticularId == customerID).ToList();
                        var currentMonthRemittance = getRemittanceList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn && e.CustomerParticularId == customerID).ToList();
                        var getCustomer = _customerParticularsModel.GetSingle(customerID);
                        var customerName = "";
                        if (getCustomer.CustomerType == "Corporate & Trading Company")
                        {
                            customerName = getCustomer.CustomerCode + " - " + getCustomer.Company_RegisteredName;
                        }
                        else
                        {
                            customerName = getCustomer.CustomerCode + " - " + getCustomer.Natural_Name;
                        }
                        saleWS.Cells[saleRow, 1].Value = customerName;
                        foreach (Sale sale in currentMonthSales)
                        {
                            decimal totalSales = sale.TotalAmountLocal;
                            grandTotalLocal += totalSales;
                            var getCurrencyID = sale.SaleTransactions.Select(e => e.CurrencyId).ToList();
                        }
                        foreach (Remittances sale in currentMonthRemittance)
                        {
                            decimal totalSales = 0.00M;
                            foreach (var transaction in sale.RemittanceOders)
                            {
                                var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                                decimal convertToSGD = 0;

                                if (getPayCurrency == "SGD")
                                {
                                    convertToSGD = transaction.PayAmount;
                                }
                                else
                                {
                                    var payAmount = transaction.PayAmount;
                                    decimal getRate = 0;
                                    //if (transaction.Fee > 0)
                                    //{
                                    //    getRate = 2 / transaction.Fee;
                                    //}
                                    decimal? payRate = transaction.currentPayRate;
                                    decimal? rate = 1 / payRate;
                                    //if (getRate > 0)
                                    //{
                                    //    payRate = transaction.Rate * getRate;
                                    //}

                                    if (payRate > 0)
                                    {
                                        convertToSGD = Convert.ToDecimal(payAmount / rate);
                                        //convertToSGD = convertToSGD / 1;
                                    }

                                }
                                totalSales += convertToSGD;
                            }
                            grandTotalLocal += totalSales;
                        }
                        saleWS.Cells[saleRow, col].Value = grandTotalLocal.ToString(sgdFormat);
                        saleRow++;
                        grandTotalLocal = 0;
                    }
                    saleRow = 8;
                    col++;
                }

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-by-memo-transactions-type-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportSTS
        public void ExportSTTS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer) && reportCustomer != "null")
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
            {
                customerList.Add(customerId);
            }

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            //if (!string.IsNullOrEmpty(reportProduct))
            //{
            //    productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            //}

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //TempData.Keep("ReportSalesStatus");
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //    TempData.Keep("ReportSalesStatus");
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            if (getSaleList.Count > 0)
            {
                getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _productsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                foreach (var sale in getSaleList)
                {
                    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                }
                foreach (var remittance in getRemittanceList)
                {
                    remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.PayCurrencyDecimal.CurrencyCode)).ToList();
                }

            }

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("dd-MM-yyyy")
                    };
                    saleDateList.Add(model);
                }
            }
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("dd-MM-yyyy")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }

            //IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            //foreach (Sale sale in sales)
            //{
            //    sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            //}

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales by Transactions Type Summary Details");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES BY TRANSACTION TYPE SUMMARY DETAILS";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                //saleWS.Cells["A4:I4"].Merge = true;
                //saleWS.Cells[4, 1].Style.Font.Bold = true;
                //saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //saleWS.Cells[4, 1].Value = "TRANSACTION TYPE: " + transactionType.ToUpper(); 

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "TRANSACTION TYPE";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "A/C NO";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "CONTACT PERSON";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "TOTAL FOREIGN CURR / GET AMOUNT";
                //saleWS.Cells[6, 7].Style.Font.Bold = true;
                //saleWS.Cells[6, 7].Value = "TRANSACTION FEE";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "LOCAL CURR / PAY AMOUNT";
                //saleWS.Cells[6, 8].Style.Font.Bold = true;
                //saleWS.Cells[6, 8].Value = "PAYMENT MODE";
                //saleWS.Cells[6, 10].Style.Font.Bold = true;
                //saleWS.Cells[6, 10].Value = "REFERENCE NO";
                //saleWS.Cells[6, 11].Style.Font.Bold = true;
                //saleWS.Cells[6, 11].Value = "AMOUNT";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;

                List<string> distinctProduct = new List<string>();
                foreach (var date in filtersaleDate)
                {
                    var currentMonthSales = getSaleList.Where(e => e.LastApprovalOn.ToString("dd-MM-yyyy") == date.LastApprovalOn).ToList();
                    currentMonthSales = currentMonthSales.OrderBy(e => e.CustomerParticularId).ThenBy(e => e.TransactionType).ToList();
                    var currentMonthRemittance = getRemittanceList.Where(e => e.LastApprovalOn.ToString("dd-MM-yyyy") == date.LastApprovalOn).ToList();
                    currentMonthRemittance = currentMonthRemittance.OrderBy(e => e.CustomerParticularId).ToList();
                    foreach (Sale sale in currentMonthSales)
                    {
                        decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                        grandTotalForeign += sale.TotalAmountForeign;
                        grandTotalLocal += sale.TotalAmountLocal;
                        var getCurrencyID = sale.SaleTransactions.Select(e => e.CurrencyId).ToList();
                        foreach (var data in getCurrencyID)
                        {
                            var currencyCode = _productsModel.GetSingle(data).CurrencyCode;
                            distinctProduct.Add(currencyCode);
                        }
                        //distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 3].Value = sale.TransactionType;
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.CustomerCode;

                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            saleWS.Cells[saleRow, 5].Value = sale.CustomerParticulars.Company_RegisteredName;
                        }
                        else
                        {
                            saleWS.Cells[saleRow, 5].Value = sale.CustomerParticulars.Natural_Name;
                        }

                        saleWS.Cells[saleRow, 6].Value = sale.TotalAmountForeign.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 7].Value = sale.TotalAmountLocal.ToString(sgdFormat);

                        List<string> paymentModes = new List<string>();

                        //if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                        //{
                        //    paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                        //    foreach (string mode in paymentModes)
                        //    {
                        //        saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                        //        if (mode == "Cash")
                        //        {
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                        //        }
                        //        else if (mode == "Cheque 1")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque1No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 2")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque2No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 3")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque3No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                        //        }
                        //        else
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.BankTransferNo;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                        //        }

                        //        saleRow++;
                        //    }
                        //}
                        saleRow++;

                        //foreach (SaleTransaction transaction in sale.SaleTransactions)
                        //{
                        //    saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode;
                        //    saleWS.Cells[saleRow, 2].Value = transaction.Products.Unit.ToString("#,##0");
                        //    saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        //    saleWS.Cells[saleRow, 6].Value = transaction.AmountForeign.ToString(sgdFormat);
                        //    saleWS.Cells[saleRow, 8].Value = transaction.AmountLocal.ToString(sgdFormat);
                        //    saleRow++;
                        //}

                        saleRow++;
                    }
                    foreach (Remittances sale in currentMonthRemittance)
                    {
                        decimal totalSales = 0.00M;
                        decimal convertToSGD = 0;

                        foreach (var transaction in sale.RemittanceOders)
                        {
                            var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                            if (getPayCurrency == "SGD")
                            {
                                convertToSGD = transaction.PayAmount;
                            }
                            else
                            {
                                var payAmount = transaction.PayAmount;
                                //decimal getRate = 0;
                                //if (transaction.Fee > 0)
                                //{
                                //    getRate = 2 / transaction.Fee;
                                //}
                                decimal? payRate = transaction.currentPayRate;
                                decimal? rate = 1 / payRate;
                                //if (getRate > 0)
                                //{
                                //    payRate = transaction.Rate * getRate;
                                //}
                                if (payRate > 0)
                                {
                                    convertToSGD = Convert.ToDecimal(payAmount / rate);
                                    //convertToSGD = convertToSGD / 1;
                                }
                            }

                            totalSales += convertToSGD;
                        }
                        grandTotalLocal += totalSales;
                        grandTotalForeign += sale.TotalGetAmount;


                        distinctProduct.AddRange(sale.RemittanceOders.Select(e => e.GetCurrencyDecimal.CurrencyCode));

                        //distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                        //Dump Data
                        saleWS.Cells[saleRow, 1].Value = sale.LastApprovalOn.ToString("dd-MM-yyyy");
                        saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                        saleWS.Cells[saleRow, 3].Value = "Remittance";
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.CustomerCode;

                        if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            saleWS.Cells[saleRow, 5].Value = sale.CustomerParticulars.Company_RegisteredName;
                        }
                        else
                        {
                            saleWS.Cells[saleRow, 5].Value = sale.CustomerParticulars.Natural_Name;
                        }

                        saleWS.Cells[saleRow, 6].Value = sale.TotalGetAmount.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 7].Value = totalSales.ToString(sgdFormat);

                        List<string> paymentModes = new List<string>();

                        //if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                        //{
                        //    paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                        //    foreach (string mode in paymentModes)
                        //    {
                        //        saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                        //        if (mode == "Cash")
                        //        {
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                        //        }
                        //        else if (mode == "Cheque 1")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque1No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 2")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque2No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                        //        }
                        //        else if (mode == "Cheque 3")
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.Cheque3No;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                        //        }
                        //        else
                        //        {
                        //            saleWS.Cells[saleRow, 9].Value = sale.BankTransferNo;
                        //            saleWS.Cells[saleRow, 10].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                        //        }

                        //        saleRow++;
                        //    }
                        //}
                        //else
                        //{
                        //saleRow++;
                        ////}

                        //foreach (RemittanceOrders transaction in sale.RemittanceOders)
                        //{
                        //    var mode = "";
                        //    var depositAccount = "";
                        //    using (var context = new DataAccess.GreatEastForex())
                        //    {
                        //        var paymentID = Convert.ToInt32(transaction.PayPaymentType);
                        //        mode = context.PaymentModeLists.Where(e => e.ID == paymentID).FirstOrDefault().Name;
                        //        if (transaction.PayDepositAccount != 0)
                        //        {
                        //            depositAccount = context.PayBankLists.Where(e => e.ID == transaction.PayDepositAccount).FirstOrDefault().BankName;
                        //        }
                        //    }
                        //    saleWS.Cells[saleRow, 9].Value = mode.ToUpper();

                        //    var payAmount = transaction.PayAmount;
                        //    decimal getRate = 0;
                        //    if (transaction.Fee > 0)
                        //    {
                        //        getRate = 2 / transaction.Fee;
                        //    }
                        //    decimal payRate = 0;
                        //    if (getRate > 0)
                        //    {
                        //        payRate = transaction.Rate * getRate;
                        //    }
                        //    decimal convertToSGD = 0;
                        //    if (payRate > 0)
                        //    {
                        //        convertToSGD = payAmount * payRate;
                        //        convertToSGD = convertToSGD / 1;
                        //    }

                        //    if (transaction.PayPaymentType == "1")
                        //    {
                        //        saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat);
                        //    }
                        //    else if (transaction.PayPaymentType == "2")
                        //    {
                        //        saleWS.Cells[saleRow, 10].Value = transaction.ChequeNo;
                        //        saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                        //    }
                        //    else if (transaction.PayPaymentType == "3")
                        //    {
                        //        saleWS.Cells[saleRow, 10].Value = depositAccount + " " + transaction.BankTransferNo;
                        //        saleWS.Cells[saleRow, 11].Value = Convert.ToDecimal(convertToSGD).ToString(sgdFormat); ;
                        //    }

                        //    saleWS.Cells[saleRow, 1].Value = transaction.PayCurrencyDecimal.CurrencyCode;
                        //    saleWS.Cells[saleRow, 2].Value = transaction.PayAmount;
                        //    saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        //    saleWS.Cells[saleRow, 5].Value = transaction.GetCurrencyDecimal.CurrencyCode;
                        //    saleWS.Cells[saleRow, 6].Value = transaction.GetAmount.ToString(GetDecimalFormat(2));
                        //    saleWS.Cells[saleRow, 7].Value = transaction.Fee.ToString(rateFormat);
                        //    saleWS.Cells[saleRow, 8].Value = convertToSGD.ToString(sgdFormat);
                        saleRow++;
                        //}

                        saleRow++;
                    }

                }
                if (getSaleList.Count > 0 || getRemittanceList.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 5].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 6].Value = grandTotalForeign.ToString("#,##0.00######") + " (" + distinctProduct.FirstOrDefault() + ")";
                    }
                    saleWS.Cells[saleRow, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 7].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 7].Value = grandTotalLocal.ToString(sgdFormat);
                }

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(6).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(7).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-by-transactions-type-summary-details" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportSTS
        public void ExportSTTSS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer) && reportCustomer != "null")
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }
            else
            {
                customerList.Add(customerId);
            }

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            //if (!string.IsNullOrEmpty(reportProduct))
            //{
            //    productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            //}

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //TempData.Keep("ReportSalesStatus");
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //    TempData.Keep("ReportSalesStatus");
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            if (getSaleList.Count > 0)
            {
                getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _productsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }

                foreach (var sale in getSaleList)
                {
                    sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                }
                foreach (var remittance in getRemittanceList)
                {
                    remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.PayCurrencyDecimal.CurrencyCode)).ToList();
                }

            }

            var saleCustomerList = new List<MonthlySalesDate>();
            var distinctCustomerList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctCustomerList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctCustomerList.Count > 0)
            {
                foreach (var data in distinctCustomerList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("dd-MM-yyyy")
                    };
                    saleCustomerList.Add(model);
                }
            }
            if (distinctCustomerList2.Count > 0)
            {
                foreach (var data in distinctCustomerList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("dd-MM-yyyy")
                    };
                    saleCustomerList.Add(model);
                }
            }
            var filtersaleCustomer = new List<MonthlySalesDate>();
            if (saleCustomerList.Count > 0)
            {
                foreach (var data in saleCustomerList)
                {
                    if (filtersaleCustomer.Count > 0)
                    {
                        var check = filtersaleCustomer.Where(e => e.CustomerParticularId == data.CustomerParticularId).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleCustomer.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleCustomer.Add(data);
                    }
                }
            }

            //IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            //foreach (Sale sale in sales)
            //{
            //    sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            //}

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales by Transactions Type Summary");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES BY TRANSACTION TYPE SUMMARY";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                //saleWS.Cells["A4:I4"].Merge = true;
                //saleWS.Cells[4, 1].Style.Font.Bold = true;
                //saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //saleWS.Cells[4, 1].Value = "TRANSACTION TYPE: " + transactionType.ToUpper(); 

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "CUSTOMER";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "BUY";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "SELL";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "ENCASHMENT";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "CROSS CURRENCY";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "REMITTANCE";
                //saleWS.Cells[6, 7].Style.Font.Bold = true;
                //saleWS.Cells[6, 7].Value = "TRANSACTION FEE";
                //saleWS.Cells[6, 7].Style.Font.Bold = true;
                //saleWS.Cells[6, 7].Value = "LOCAL CURR / PAY AMOUNT";
                //saleWS.Cells[6, 8].Style.Font.Bold = true;
                //saleWS.Cells[6, 8].Value = "PAYMENT MODE";
                //saleWS.Cells[6, 10].Style.Font.Bold = true;
                //saleWS.Cells[6, 10].Value = "REFERENCE NO";
                //saleWS.Cells[6, 11].Style.Font.Bold = true;
                //saleWS.Cells[6, 11].Value = "AMOUNT";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(12);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                List<string> distinctProduct = new List<string>();
                foreach (var customer in filtersaleCustomer)
                {
                    var currentSales = getSaleList.Where(e => e.CustomerParticularId == customer.CustomerParticularId).ToList();
                    currentSales = currentSales.OrderBy(e => e.CustomerParticularId).ThenBy(e => e.TransactionType).ToList();
                    var currentRemittance = getRemittanceList.Where(e => e.CustomerParticularId == customer.CustomerParticularId).ToList();
                    currentRemittance = currentRemittance.OrderBy(e => e.CustomerParticularId).ToList();

                    //Transaction Type filter
                    var buyList = currentSales.Where(e => e.TransactionType == "Buy").ToList();
                    var sellList = currentSales.Where(e => e.TransactionType == "Sell").ToList();
                    var encashmentList = currentSales.Where(e => e.TransactionType == "Encashment").ToList();
                    var crossCurrencyList = currentSales.Where(e => e.TransactionType == "Cross Currency").ToList();
                    if (buyList.Count > 0)
                    {
                        decimal customerTotal = 0;
                        foreach (var data in buyList)
                        {
                            decimal totalSales = data.SaleTransactions.Sum(e => e.AmountLocal);
                            customerTotal += totalSales;
                        }
                        saleWS.Cells[saleRow, 2].Value = customerTotal.ToString(sgdFormat);
                    }
                    if (sellList.Count > 0)
                    {
                        decimal customerTotal = 0;
                        foreach (var data in sellList)
                        {
                            decimal totalSales = data.SaleTransactions.Sum(e => e.AmountLocal);
                            customerTotal += totalSales;
                        }
                        saleWS.Cells[saleRow, 3].Value = customerTotal.ToString(sgdFormat);
                    }
                    if (encashmentList.Count > 0)
                    {
                        decimal customerTotal = 0;
                        foreach (var data in encashmentList)
                        {
                            decimal totalSales = data.SaleTransactions.Sum(e => e.AmountLocal);
                            customerTotal += totalSales;
                        }
                        saleWS.Cells[saleRow, 4].Value = customerTotal.ToString(sgdFormat);
                    }
                    if (crossCurrencyList.Count > 0)
                    {
                        decimal customerTotal = 0;
                        foreach (var data in crossCurrencyList)
                        {
                            decimal totalSales = data.SaleTransactions.Sum(e => e.AmountLocal);
                            customerTotal += totalSales;
                        }
                        saleWS.Cells[saleRow, 5].Value = customerTotal.ToString(sgdFormat);
                    }
                    if (currentRemittance.Count > 0)
                    {
                        decimal customerTotal = 0;
                        foreach (var data in currentRemittance)
                        {
                            decimal totalSales = 0.00M;
                            decimal convertToSGD = 0;

                            foreach (var transaction in data.RemittanceOders)
                            {
                                var getPayCurrency = transaction.PayCurrencyDecimal.CurrencyCode;
                                if (getPayCurrency == "SGD")
                                {
                                    convertToSGD = transaction.PayAmount;
                                }
                                else
                                {
                                    var payAmount = transaction.PayAmount;
                                    //decimal getRate = 0;
                                    //if (transaction.Fee > 0)
                                    //{
                                    //    getRate = 2 / transaction.Fee;
                                    //}
                                    decimal? payRate = transaction.currentPayRate;
                                    decimal? rate = 1 / payRate;
                                    //if (getRate > 0)
                                    //{
                                    //    payRate = transaction.Rate * getRate;
                                    //}
                                    if (payRate > 0)
                                    {
                                        convertToSGD = Convert.ToDecimal(payAmount / rate);
                                        //convertToSGD = convertToSGD / 1;
                                    }
                                }

                                totalSales += convertToSGD;
                            }
                            customerTotal += totalSales;
                        }
                        saleWS.Cells[saleRow, 6].Value = customerTotal.ToString(sgdFormat);
                    }
                    saleRow++;
                }
                if (getSaleList.Count > 0 || getRemittanceList.Count > 0)
                {
                    int row = 7;
                    var getCustomerList = _customerParticularsModel.GetAll().Where(e => filtersaleCustomer.Select(s => s.CustomerParticularId).Contains(e.ID)).ToList();
                    foreach (var data in getCustomerList)
                    {
                        var customerName = "";
                        if (data.CustomerType == "Corporate & Trading Company")
                        {
                            customerName = data.CustomerCode + " - " + data.Company_RegisteredName;
                        }
                        else
                        {
                            customerName = data.CustomerCode + " - " + data.Natural_Name;
                        }
                        saleWS.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        saleWS.Cells[row, 1].Style.Font.Bold = false;
                        saleWS.Cells[row, 1].Value = customerName;
                        row++;
                    }
                    //saleWS.Cells[saleRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    //saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                    //saleWS.Cells[saleRow, 5].Value = "GRAND TOTAL";
                    //if (distinctProduct.Distinct().Count() == 1)
                    //{
                    //    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    //    saleWS.Cells[saleRow, 6].Value = grandTotalForeign.ToString("#,##0.00######") + " (" + distinctProduct.FirstOrDefault() + ")";
                    //}
                    //saleWS.Cells[saleRow, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //saleWS.Cells[saleRow, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    //saleWS.Cells[saleRow, 7].Style.Font.Bold = true;
                    //saleWS.Cells[saleRow, 7].Value = grandTotalLocal.ToString(sgdFormat);
                }

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(6).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(7).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-by-transactions-type-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: SMTS
        public ActionResult SMTS(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportMultipleCustomer"].ToString();
            int customerId = 0;
            List<int> customerList = new List<int>();
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerList = reportCustomer.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportMultipleCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            string transactionType = TempData["TransactionType"].ToString();
            List<string> transactionList = new List<string>();
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactionList = transactionType.Split(',').ToList();
            }

            TempData.Keep("TransactionType");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            //string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            //if (!string.IsNullOrEmpty(reportSalesStatus))
            //{
            //    acceptStatus.Add(reportSalesStatus);
            //    if (reportSalesStatus == "Completed")
            //    {
            //        acceptStatus.Add("Pending Delete GM Approval");
            //    }
            //}
            var dt_fromDate = Convert.ToDateTime(fromDate + " 00:00:00");
            var dt_toDate = Convert.ToDateTime(toDate + " 23:59:59");
            TempData.Keep("ReportSalesStatus");


            var transactionSummaries = new List<TransactionSummary>();
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            var finalCustomerList = new List<int>();
            if (getSaleList.Count > 0)
            {
                var getCustomerList = getSaleList.Select(s => s.CustomerParticularId).Distinct().ToList();
                finalCustomerList.AddRange(getCustomerList);
                getSaleList.OrderBy(e => e.CustomerParticularId).ThenByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                var getCustomerList2 = getRemittanceList.Select(s => s.CustomerParticularId).Distinct().ToList();
                finalCustomerList.AddRange(getCustomerList2);
                getRemittanceList.OrderBy(e => e.CustomerParticularId).ThenByDescending(e => e.LastApprovalOn).ToList();
            }
            finalCustomerList = finalCustomerList.Distinct().ToList();
            IPagedList<int> customerPage = finalCustomerList.ToPagedList(page, 2);
            if (productIds.Count > 0)
            {
                List<string> productCurrencyCode = new List<string>();
                foreach (var productID in productIds)
                {
                    var getCurrencyCode = _productsModel.GetSingle(productID).CurrencyCode;
                    productCurrencyCode.Add(getCurrencyCode);
                }
                foreach (var data in customerPage)
                {
                    foreach (var sale in getSaleList.Where(e => e.CustomerParticularId == data).ToList())
                    {
                        sale.SaleTransactions = sale.SaleTransactions.Where(e => productIds.Contains(e.CurrencyId)).ToList();
                    }
                    foreach (var remittance in getRemittanceList.Where(e => e.CustomerParticularId == data).ToList())
                    {
                        remittance.RemittanceOders = remittance.RemittanceOders.Where(e => productCurrencyCode.Contains(e.PayCurrencyDecimal.CurrencyCode)).ToList();
                    }
                }
            }
            getSaleList = getSaleList.Where(e => customerPage.Contains(e.CustomerParticularId)).ToList();
            getRemittanceList = getRemittanceList.Where(e => customerPage.Contains(e.CustomerParticularId)).ToList();

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn && e.CustomerParticularId == data.CustomerParticularId).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }

            ViewData["SaleDateList"] = filtersaleDate;
            ViewData["RemittanceList"] = getRemittanceList;
            ViewData["SaleList"] = getSaleList;
            ViewData["CustomerPage"] = customerPage;

            //Get Grand Total on last page
            //if (page == sales.PageCount)
            //{
            //    decimal[] grandTotals = _salesModel.GetLastApprovedReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");

            //    ViewData["GrandTotal"] = grandTotals;
            //}

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportSTS
        public void ExportSTS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            string reportCustomer = TempData["ReportCustomer"].ToString();
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            TempData.Keep("ReportCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            TempData.Keep("ReportProduct");
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            TempData.Keep("ReportSalesStatus");
            if (!string.IsNullOrEmpty(reportSalesStatus))
            {
                reportSalesStatus = TempData["ReportSalesStatus"].ToString();
                TempData.Keep("ReportSalesStatus");
                acceptStatus.Add(reportSalesStatus);
                //if (reportSalesStatus == "Completed")
                //{
                //    acceptStatus.Add("Pending Delete GM Approval");
                //}
            }

            IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales Transactions Summary");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES TRANSACTIONS DAILY SUMMARY LISTING";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                saleWS.Cells["A4:I4"].Merge = true;
                saleWS.Cells[4, 1].Style.Font.Bold = true;
                saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[4, 1].Value = String.Format("Status: {0}", !string.IsNullOrEmpty(reportSalesStatus) ? reportSalesStatus == "Pending" ? "Pending Only" : "Completed Only" : "All").ToUpper();

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "A/C NO";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "CONTACT PERSON";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "FOREIGN CURR";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "LOCAL CURR";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "PAYMENT MODE";
                saleWS.Cells[6, 8].Style.Font.Bold = true;
                saleWS.Cells[6, 8].Value = "REFERENCE NO";
                saleWS.Cells[6, 9].Style.Font.Bold = true;
                saleWS.Cells[6, 9].Value = "AMOUNT";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(rateDP);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;

                List<int> distinctProduct = new List<int>();

                foreach (Sale sale in sales)
                {
                    decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                    grandTotalForeign += sale.SaleTransactions.Sum(e => e.AmountForeign);
                    grandTotalLocal += sale.SaleTransactions.Sum(e => e.AmountLocal);
                    distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                    //Dump Data
                    saleWS.Cells[saleRow, 1].Value = sale.IssueDate.ToString("dd-MM-yyyy");
                    saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                    saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.CustomerCode;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Natural_Name;
                    }

                    saleWS.Cells[saleRow, 6].Value = totalSales.ToString(sgdFormat);

                    List<string> paymentModes = new List<string>();

                    if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                    {
                        paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                        foreach (string mode in paymentModes)
                        {
                            saleWS.Cells[saleRow, 7].Value = mode.ToUpper();

                            if (mode == "Cash")
                            {
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                            }
                            else if (mode == "Cheque 1")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque1No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 2")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque2No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 3")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque3No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                            }
                            else
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.BankTransferNo;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                            }

                            saleRow++;
                        }
                    }
                    else
                    {
                        saleRow++;
                    }

                    foreach (SaleTransaction transaction in sale.SaleTransactions)
                    {
                        saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode;
                        saleWS.Cells[saleRow, 2].Value = transaction.Products.Unit.ToString("#,##0");
                        saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        saleWS.Cells[saleRow, 5].Value = transaction.AmountForeign.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 6].Value = transaction.AmountLocal.ToString(sgdFormat);
                        saleRow++;
                    }

                    saleRow++;
                }

                if (sales.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 4].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 4].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 5].Value = grandTotalForeign.ToString("#,##0.00######");
                    }
                    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                }
                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-transactions-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: PTS
        public ActionResult PTS(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportCustomer"].ToString();
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            TempData.Keep("ReportCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            if (!string.IsNullOrEmpty(reportSalesStatus))
            {
                acceptStatus.Add(reportSalesStatus);
                if (reportSalesStatus == "Completed")
                {
                    acceptStatus.Add("Pending Delete GM Approval");
                }
            }

            TempData.Keep("ReportSalesStatus");

            IPagedList<Sale> sales = _salesModel.GetLastApprovedSalesByDatePaged(fromDate, toDate, exceptionStatus, acceptStatus, page, pageSize, productIds, customerId, "Buy");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Buy" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }
            ViewData["Sale"] = sales;

            //Get Grand Total on last page
            if (page == sales.PageCount)
            {
                decimal[] grandTotals = _salesModel.GetLastApprovedReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Buy");

                ViewData["GrandTotal"] = grandTotals;
            }

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportPTS
        public void ExportPTS()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportCustomer"].ToString();
            TempData.Keep("ReportCustomer");
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            string reportProduct = TempData["ReportProduct"].ToString();
            TempData.Keep("ReportProduct");

            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

            List<string> acceptStatus = new List<string>();

            string reportSalesStatus = TempData["ReportSalesStatus"].ToString();
            TempData.Keep("ReportSalesStatus");

            if (!string.IsNullOrEmpty(reportSalesStatus))
            {
                acceptStatus.Add(reportSalesStatus);
                if (reportSalesStatus == "Completed")
                {
                    acceptStatus.Add("Pending Delete GM Approval");
                }
            }

            IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Buy");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Buy" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Purchase Transactions Summary");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:I1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:I2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "PURCHASE TRANSACTIONS DAILY SUMMARY LISTING";
                saleWS.Cells["A3:I3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;
                saleWS.Cells["A4:I4"].Merge = true;
                saleWS.Cells[4, 1].Style.Font.Bold = true;
                saleWS.Cells[4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[4, 1].Value = String.Format("Status: {0}", !string.IsNullOrEmpty(reportSalesStatus) ? reportSalesStatus == "Pending" ? "Pending Only" : "Completed Only" : "All").ToUpper();

                //set first row name
                saleWS.Cells[6, 1].Style.Font.Bold = true;
                saleWS.Cells[6, 1].Value = "DATE";
                saleWS.Cells[6, 2].Style.Font.Bold = true;
                saleWS.Cells[6, 2].Value = "MEMO ID";
                saleWS.Cells[6, 3].Style.Font.Bold = true;
                saleWS.Cells[6, 3].Value = "A/C NO";
                saleWS.Cells[6, 4].Style.Font.Bold = true;
                saleWS.Cells[6, 4].Value = "CONTACT PERSON";
                saleWS.Cells[6, 5].Style.Font.Bold = true;
                saleWS.Cells[6, 5].Value = "FOREIGN CURR";
                saleWS.Cells[6, 6].Style.Font.Bold = true;
                saleWS.Cells[6, 6].Value = "LOCAL CURR";
                saleWS.Cells[6, 7].Style.Font.Bold = true;
                saleWS.Cells[6, 7].Value = "PAYMENT MODE";
                saleWS.Cells[6, 8].Style.Font.Bold = true;
                saleWS.Cells[6, 8].Value = "REFERENCE NO";
                saleWS.Cells[6, 9].Style.Font.Bold = true;
                saleWS.Cells[6, 9].Value = "AMOUNT";

                int saleRow = 7;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(rateDP);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                List<int> distinctProduct = new List<int>();

                foreach (Sale sale in sales)
                {
                    decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                    grandTotalForeign += sale.SaleTransactions.Sum(e => e.AmountForeign);
                    grandTotalLocal += sale.SaleTransactions.Sum(e => e.AmountLocal);
                    distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                    //Dump Data
                    saleWS.Cells[saleRow, 1].Value = sale.IssueDate.ToString("dd-MM-yyyy");
                    saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                    saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.CustomerCode;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Natural_Name;
                    }

                    saleWS.Cells[saleRow, 6].Value = totalSales.ToString(sgdFormat);

                    if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                    {
                        string[] paymentModes = sale.LocalPaymentMode.Split(',');

                        foreach (string mode in paymentModes)
                        {
                            saleWS.Cells[saleRow, 7].Value = mode.ToUpper();

                            if (mode == "Cash")
                            {
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                            }
                            else if (mode == "Cheque 1")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque1No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 2")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque2No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 3")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque3No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                            }
                            else
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.BankTransferNo;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                            }

                            saleRow++;
                        }
                    }
                    else
                    {
                        saleRow++;
                    }

                    foreach (SaleTransaction transaction in sale.SaleTransactions)
                    {
                        saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode;
                        saleWS.Cells[saleRow, 2].Value = transaction.Products.Unit.ToString("#,##0");
                        saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        saleWS.Cells[saleRow, 5].Value = transaction.AmountForeign.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 6].Value = transaction.AmountLocal.ToString(sgdFormat);
                        saleRow++;
                    }

                    saleRow++;
                }

                if (sales.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 4].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 4].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 5].Value = grandTotalForeign.ToString("#,##0.00######");
                    }
                    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                }

                saleWS.PrinterSettings.PaperSize = ePaperSize.A4;
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 15)
                {
                    saleWS.Column(4).Width = 15;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=purchase-transactions-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: BPB
        public ActionResult BPB(int page = 1)
        {
            int pageSize = 1000;// Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string date = TempData["ReportDate"].ToString();
            ViewData["ReportDate"] = date;

            TempData.Keep("ReportDate");

            IList<Product> products = _productsModel.GetAll().Where(e => e.CurrencyCode != "SGD").ToList();

            List<CurrencyClosingBalance> list = new List<CurrencyClosingBalance>();

            foreach (Product product in products)
            {
                string amountForeignFormat = GetDecimalFormat(product.Decimal);

                decimal foreignCurrencyBal = product.ProductInventories[0].TotalInAccount;
                decimal averageRate = Convert.ToDecimal(product.BuyRate);

                //Take from end day of trade
                EndDayTrade productEndDayOfTrade = _endDayTradesModel.GetProductTrade(product.ID, date);

                if (productEndDayOfTrade != null)
                {
                    foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                    averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                }
                else
                {
                    productEndDayOfTrade = _endDayTradesModel.GetProductLastTrade(product.ID, date);

                    if (productEndDayOfTrade != null)
                    {
                        foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                        averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                    }
                }

                decimal closingBal = foreignCurrencyBal * averageRate / product.Unit;

                CurrencyClosingBalance bal = new CurrencyClosingBalance();
                bal.ProductId = product.ID;
                bal.Code = product.CurrencyCode;
                bal.Currency = product.CurrencyName;
                bal.AmountForeignFormat = amountForeignFormat;
                bal.ForeignCurrencyClosingBal = foreignCurrencyBal; // Renamed to Forex Closing Bal
                bal.AveragePurchaseCostOrLastBuyingRate = averageRate; // Renamed to Weighted Purchase Rate
                bal.ClosingBalAtAveragePurchaseOrLastBuying = closingBal; // Renamed to SGD Equivalent
                list.Add(bal);
            }

            IPagedList<CurrencyClosingBalance> closingBalances = _closingBalancesModel.GetPaged(list, page, pageSize);
            ViewData["CurrencyClosingBalance"] = closingBalances;

            if (page == closingBalances.PageCount)
            {
                ViewData["TotalStock"] = list.Sum(e => e.ClosingBalAtAveragePurchaseOrLastBuying).ToString(GetDecimalFormat(sgdDp));
            }

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportBPB
        public void ExportBPB()
        {
            string sgdFormat = GetDecimalFormat(sgdDp);
            string date = TempData["ReportDate"].ToString();
            ViewData["ReportDate"] = date;
            TempData.Keep("ReportDate");

            DateTime? closingStart = null;
            DateTime closingEnd = DateTime.Now;

            IList<Product> products = _productsModel.GetAll().Where(e => e.CurrencyCode != "SGD").ToList();

            List<CurrencyClosingBalance> list = new List<CurrencyClosingBalance>();

            foreach (Product product in products)
            {
                string amountForeignFormat = GetDecimalFormat(product.Decimal);

                decimal foreignCurrencyBal = product.ProductInventories[0].TotalInAccount;
                decimal averageRate = Convert.ToDecimal(product.BuyRate);

                string description = "No Transaction";

                //Take from end day of trade
                EndDayTrade productEndDayOfTrade = _endDayTradesModel.GetProductTrade(product.ID, date);

                if (productEndDayOfTrade != null)
                {
                    foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                    averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                    closingStart = productEndDayOfTrade.LastActivationTime;
                    closingEnd = productEndDayOfTrade.CurrentActivationTime;
                    description = productEndDayOfTrade.Description;
                }
                else
                {
                    productEndDayOfTrade = _endDayTradesModel.GetProductLastTrade(product.ID, date);

                    if (productEndDayOfTrade != null)
                    {
                        foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                        averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                        closingStart = productEndDayOfTrade.LastActivationTime;
                        closingEnd = productEndDayOfTrade.CurrentActivationTime;
                        description = productEndDayOfTrade.Description;
                    }
                }

                decimal closingBal = foreignCurrencyBal * averageRate / product.Unit;

                CurrencyClosingBalance bal = new CurrencyClosingBalance();
                bal.ProductId = product.ID;
                bal.Code = product.CurrencyCode;
                bal.Currency = product.CurrencyName;
                bal.AmountForeignFormat = amountForeignFormat;
                bal.ForeignCurrencyClosingBal = foreignCurrencyBal; // Renamed to Forex Closing Bal
                bal.AveragePurchaseCostOrLastBuyingRate = averageRate; // Renamed to Weighted Purchase Rate
                bal.ClosingBalAtAveragePurchaseOrLastBuying = closingBal; // Renamed to SGD Equivalent
                bal.Description = description;
                list.Add(bal);
            }

            IList<CurrencyClosingBalance> closingBalances = _closingBalancesModel.GetAll(list);

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet productWS = pck.Workbook.Worksheets.Add("Currency Closing Bal");

                //set header rows
                productWS.Cells["A1:B1"].Merge = true;
                productWS.Cells[1, 1].Style.Font.Bold = true;
                productWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                productWS.Cells[1, 1].Value = companyName;

                productWS.Cells["D1:E1"].Merge = true;
                productWS.Cells[1, 4].Style.Font.Bold = true;
                productWS.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                productWS.Cells[1, 4].Value = exportDate;

                productWS.Cells["A2:E2"].Merge = true;
                productWS.Cells[2, 1].Style.Font.Bold = true;
                productWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                productWS.Cells[2, 1].Value = "CURRENCY CLOSING BALANCE AT AVERAGE PURCHASE OR LAST BUYING RATE";
                productWS.Cells["A3:E3"].Merge = true;
                productWS.Cells[3, 1].Style.Font.Bold = true;
                productWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                productWS.Cells[3, 1].Value = "ON  " + date;

                //set first row name
                productWS.Cells[5, 1].Style.Font.Bold = true;
                productWS.Cells[5, 1].Value = "CODE";
                productWS.Cells[5, 2].Style.Font.Bold = true;
                productWS.Cells[5, 2].Value = "CURRENCY";
                productWS.Cells[5, 3].Style.Font.Bold = true;
                productWS.Cells[5, 3].Value = "FOREX CLOSING BAL";
                productWS.Cells[5, 4].Style.Font.Bold = true;
                productWS.Cells[5, 4].Value = "WEIGHTED PURCHASE RATE";
                productWS.Cells[5, 5].Style.Font.Bold = true;
                productWS.Cells[5, 5].Value = "SGD EQUIVALENT";

                //Create End of Day Trade Worksheet
                ExcelWorksheet eodtWs = pck.Workbook.Worksheets.Add("Breakdowns");

                //Set first row name
                eodtWs.Cells[1, 1].Style.Font.Bold = true;
                eodtWs.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                eodtWs.Cells[1, 1, 1, 6].Merge = true;
                eodtWs.Cells[1, 1].Value = "END OF DAY TRADE BREAKDOWNS";

                eodtWs.Cells[3, 1, 3, 6].Style.Font.Bold = true;
                eodtWs.Cells[3, 1].Value = "CODE";
                eodtWs.Cells[3, 2].Value = "REFERENCE NO.";
                eodtWs.Cells[3, 3].Value = "AMOUNT (FOREIGN)";
                eodtWs.Cells[3, 4].Value = "RATE";
                eodtWs.Cells[3, 5].Value = "ENCASHMENT RATE";
                eodtWs.Cells[3, 6].Value = "AMOUNT (LOCAL)";

                int productRow = 6;
                int eodtRow = 4;
                decimal totalInStock = 0;

                string rateFormat = GetRateFormat(rateDP);

                foreach (CurrencyClosingBalance bal in closingBalances)
                {
                    productWS.Cells[productRow, 1].Value = bal.Code;
                    productWS.Cells[productRow, 2].Value = bal.Currency;
                    productWS.Cells[productRow, 3].Value = bal.ForeignCurrencyClosingBal.ToString(bal.AmountForeignFormat);
                    productWS.Cells[productRow, 4].Value = bal.AveragePurchaseCostOrLastBuyingRate.ToString(rateFormat);
                    productWS.Cells[productRow, 5].Value = bal.ClosingBalAtAveragePurchaseOrLastBuying.ToString(sgdFormat);
                    productRow++;
                    totalInStock += bal.ClosingBalAtAveragePurchaseOrLastBuying;

                    eodtWs.Cells[eodtRow, 1].Value = bal.Code;

                    if (bal.Description == "No Transaction")
                    {
                        eodtWs.Cells[eodtRow, 2, eodtRow, 6].Merge = true;
                        eodtWs.Cells[eodtRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        eodtWs.Cells[eodtRow, 2].Style.Font.Italic = true;
                        eodtWs.Cells[eodtRow, 2].Value = bal.Description;
                        eodtRow++;
                    }
                    else
                    {
                        List<string> descriptions = bal.Description.Split('|').ToList();

                        foreach (string description in descriptions)
                        {
                            string[] desc = description.Split(':');

                            eodtWs.Cells[eodtRow, 2].Value = desc[0];
                            eodtWs.Cells[eodtRow, 3].Value = desc[1];
                            eodtWs.Cells[eodtRow, 4].Value = desc[2];
                            eodtWs.Cells[eodtRow, 5].Value = desc[3];
                            eodtWs.Cells[eodtRow, 6].Value = desc[4];
                            eodtRow++;
                        }
                    }
                }

                productWS.Cells[productRow, 4].Style.Font.Bold = true;
                productWS.Cells[productRow, 4].Value = "TOTAL STOCK IN S$";
                productWS.Cells[productRow, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                productWS.Cells[productRow, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                productWS.Cells[productRow, 5].Style.Font.Bold = true;
                productWS.Cells[productRow, 5].Value = totalInStock.ToString(sgdFormat);

                productWS.PrinterSettings.PaperSize = ePaperSize.A4;
                productWS.PrinterSettings.TopMargin = 0.35M;
                productWS.PrinterSettings.RightMargin = 0.35M;
                productWS.PrinterSettings.BottomMargin = 0.35M;
                productWS.PrinterSettings.LeftMargin = 0.35M;
                productWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                productWS.Cells[productWS.Dimension.Address].Style.Font.Size = 8;
                productWS.Cells[productWS.Dimension.Address].AutoFitColumns();

                productRow += 2;

                //Write Closing Period
                productWS.Cells[productRow, 1].Style.Font.Bold = true;
                productWS.Cells[productRow, 1].Style.Font.Size = 8;
                productWS.Cells[productRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                productWS.Cells[productRow, 1].Value = String.Format("Closing Period: {0} to {1}", closingStart != null ? Convert.ToDateTime(closingStart).ToString("dd/MM/yyyy HH:mm:ss") : "-", closingEnd.ToString("dd/MM/yyyy HH:mm:ss"));
                productWS.Cells[productRow, 1, productRow, 5].Merge = true;

                eodtWs.PrinterSettings.PaperSize = ePaperSize.A4;
                eodtWs.PrinterSettings.TopMargin = 0.35M;
                eodtWs.PrinterSettings.RightMargin = 0.35M;
                eodtWs.PrinterSettings.BottomMargin = 0.35M;
                eodtWs.PrinterSettings.LeftMargin = 0.35M;
                eodtWs.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                eodtWs.Cells[eodtWs.Dimension.Address].Style.Font.Size = 8;
                eodtWs.Cells[eodtWs.Dimension.Address].AutoFitColumns();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=currency-closing-balance-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: SWCD
        public ActionResult SWCD(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            //List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Pending GM Approval", "Pending GM Approval (Rejected)" };

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            IPagedList<Sale> sales = _salesModel.GetPaged(null, null, fromDate, toDate, page, pageSize);
            ViewData["Sale"] = sales;

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: CS
        public ActionResult CustomerSummary(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            //List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Pending GM Approval", "Pending GM Approval (Rejected)" };

            ViewData["ReportFromDate"] = fromDate;
            ViewData["ReportToDate"] = toDate;

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            var customers = _customerParticularsModel.GetAllCustomers(fromDate, toDate);
            customers = customers.Where(e => e.isKYCVerify == 1 && e.IsDeleted == "N").OrderBy(e => e.CreatedOn).ToList();
            var customerList = new List<CustomerSummary>();
            foreach (var customer in customers)
            {
                var model = new CustomerSummary()
                {
                    Date = customer.CreatedOn.Date.ToString("yyyy MMMM"),
                    CustomerAccount = customer.CustomerCode,
                    ID = customer.ID
                };
                customerList.Add(model);
            }
            var getDate = customerList.Select(s => s.Date).Distinct().ToList();
            IPagedList<string> datePageList = getDate.ToPagedList(page, pageSize);
            //ViewData["Sale"] = sales;
            ViewData["getDate"] = datePageList;
            ViewData["CustomerList"] = customerList;
            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportSWCD
        public void ExportSWCD()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            List<string> exceptionStatus = new List<string>();

            IList<Sale> sales = _salesModel.GetSaleLastApprovedDataByDate(fromDate, toDate, exceptionStatus, new List<string>(), new List<int>());
            sales = sales.Where(e => e.Status != "Cancelled").ToList();
            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd/MM/yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                string rateFormat = GetRateFormat(rateDP);
                string sgdFormat = GetDecimalFormat(sgdDp);

                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Sales with Customer Details");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["N1:O1"].Merge = true;
                saleWS.Cells[1, 14].Style.Font.Bold = true;
                saleWS.Cells[1, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 14].Value = exportDate;

                saleWS.Cells["A2:O2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "SALES WITH CUSTOMER DETAILS LISTING";
                saleWS.Cells["A3:O3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;

                //set first row name
                saleWS.Cells[5, 1].Style.Font.Bold = true;
                saleWS.Cells[5, 1].Value = "MEMO ID";
                saleWS.Cells[5, 2].Style.Font.Bold = true;
                saleWS.Cells[5, 2].Value = "ISSUE DATE";
                saleWS.Cells[5, 3].Style.Font.Bold = true;
                saleWS.Cells[5, 3].Value = "COLLECTION DATE/TIME";
                saleWS.Cells[5, 4].Style.Font.Bold = true;
                saleWS.Cells[5, 4].Value = "COMPANY / NAME";
                saleWS.Cells[5, 5].Style.Font.Bold = true;
                saleWS.Cells[5, 5].Value = "REGISTRATION NO. / NRIC / PASSPORT NO.";
                saleWS.Cells[5, 6].Style.Font.Bold = true;
                saleWS.Cells[5, 6].Value = "Authorized Person Name";
                saleWS.Cells[5, 7].Style.Font.Bold = true;
                saleWS.Cells[5, 7].Value = "Authorized Person IC/Passport";
                saleWS.Cells[5, 8].Style.Font.Bold = true;
                saleWS.Cells[5, 8].Value = "Authorized Person Nationality";
                saleWS.Cells[5, 9].Style.Font.Bold = true;
                saleWS.Cells[5, 9].Value = "CONTACT NO.";
                saleWS.Cells[5, 10].Style.Font.Bold = true;
                saleWS.Cells[5, 10].Value = "ADDRESS 1";
                saleWS.Cells[5, 11].Style.Font.Bold = true;
                saleWS.Cells[5, 11].Value = "ADDRESS 2";
                saleWS.Cells[5, 12].Style.Font.Bold = true;
                saleWS.Cells[5, 12].Value = "POSTAL CODE";
                saleWS.Cells[5, 13].Style.Font.Bold = true;
                saleWS.Cells[5, 13].Value = "TRANSACTION TYPE";
                saleWS.Cells[5, 14].Style.Font.Bold = true;
                saleWS.Cells[5, 14].Value = "TYPE";
                saleWS.Cells[5, 15].Style.Font.Bold = true;
                saleWS.Cells[5, 15].Value = "CURRENCY";
                saleWS.Cells[5, 16].Style.Font.Bold = true;
                saleWS.Cells[5, 16].Value = "EXCHANGE RATE";
                saleWS.Cells[5, 17].Style.Font.Bold = true;
                saleWS.Cells[5, 17].Value = "AMOUNT (FOREIGN)";
                saleWS.Cells[5, 18].Style.Font.Bold = true;
                saleWS.Cells[5, 18].Value = "AMOUNT (LOCAL)";

                int saleCount = 6;

                foreach (Sale sale in sales)
                {
                    foreach (SaleTransaction transaction in sale.SaleTransactions.OrderBy(e => e.ID))
                    {
                        CustomerParticular cp = sale.CustomerParticulars;
                        CustomerSourceOfFund sof = cp.SourceOfFunds[0];
                        CustomerActingAgent ag = cp.ActingAgents[0];
                        IList<CustomerAppointmentOfStaff> ca = cp.AppointmentOfStaffs;
                        CustomerDocumentCheckList dc = cp.DocumentCheckLists[0];
                        CustomerOther co = cp.Others[0];
                        IList<CustomerCustomRate> cr = cp.CustomRates;

                        string authPersonName = "-";
                        string authICPassport = "-";
                        string authNationality = "-";

                        //if have record
                        if (ca.Count > 0)
                        {
                            //get the first record details
                            authPersonName = ca[0].FullName;
                            authICPassport = ca[0].ICPassportNo;
                            authNationality = ca[0].Nationality;
                        }

                        //Dump Sales Data
                        saleWS.Cells[saleCount, 1, saleCount, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        saleWS.Cells[saleCount, 1].Value = sale.MemoID;
                        saleWS.Cells[saleCount, 2].Value = sale.IssueDate.ToString("dd/MM/yyyy");
                        saleWS.Cells[saleCount, 3].Value = String.Format("{0}" + Environment.NewLine + "{1}", Convert.ToDateTime(sale.CollectionDate).ToString("dd/MM/yyyy dddd"), sale.CollectionTime);
                        saleWS.Cells[saleCount, 4].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_RegisteredName : sale.CustomerParticulars.Natural_Name;
                        //saleWS.Cells[saleCount, 4].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_RegisteredName : sale.CustomerParticulars.Natural_Name;
                        saleWS.Cells[saleCount, 5].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_RegistrationNo : sale.CustomerParticulars.Natural_ICPassportNo;
                        saleWS.Cells[saleCount, 6].Value = authPersonName;
                        saleWS.Cells[saleCount, 7].Value = authICPassport;
                        saleWS.Cells[saleCount, 8].Value = authNationality;
                        saleWS.Cells[saleCount, 9].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? String.Format("Tel: {0}" + Environment.NewLine + "Fax: {1}", sale.CustomerParticulars.Company_TelNo, sale.CustomerParticulars.Company_FaxNo) : String.Format("(H): {0}" + Environment.NewLine + "(O): {1}" + Environment.NewLine + "(M): {2}", sale.CustomerParticulars.Natural_ContactNoH, sale.CustomerParticulars.Natural_ContactNoO, sale.CustomerParticulars.Natural_ContactNoM);
                        saleWS.Cells[saleCount, 10].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_BusinessAddress1 : sale.CustomerParticulars.Natural_PermanentAddress;
                        saleWS.Cells[saleCount, 11].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_BusinessAddress2 : null;
                        saleWS.Cells[saleCount, 12].Value = sale.CustomerParticulars.CustomerType == "Corporate & Trading Company" ? sale.CustomerParticulars.Company_PostalCode : null;
                        saleWS.Cells[saleCount, 13].Value = sale.TransactionType;
                        saleWS.Cells[saleCount, 14].Value = transaction.TransactionType;
                        saleWS.Cells[saleCount, 15].Value = transaction.Products.CurrencyCode;
                        saleWS.Cells[saleCount, 16].Value = transaction.Rate.ToString(rateFormat);
                        saleWS.Cells[saleCount, 17].Value = transaction.AmountForeign.ToString(GetDecimalFormat(transaction.Products.Decimal));
                        saleWS.Cells[saleCount, 18].Value = transaction.AmountLocal.ToString(sgdFormat);
                        saleCount++;
                    }
                }

                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                saleWS.Cells[saleWS.Dimension.Address].Style.WrapText = true;

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=sales-customer-details-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: ExportSWCD
        public void ExportCustomerSummary()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            List<int> productIds = new List<int>();
            List<string> transactionList = new List<string>();
            List<int> customerList = new List<int>();
            customerList.Add(0);
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);
            var getRemittanceList = _remittanceSalesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList);

            if (getSaleList.Count > 0)
            {
                getSaleList.OrderByDescending(e => e.LastApprovalOn).ThenBy(e => e.TransactionType).ToList();
            }
            if (getRemittanceList.Count > 0)
            {
                getRemittanceList.OrderByDescending(e => e.LastApprovalOn).ToList();
            }

            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            var distinctDateList2 = getRemittanceList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            if (distinctDateList2.Count > 0)
            {
                foreach (var data in distinctDateList2)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = data.CustomerParticularId,
                        LastApprovalOn = data.LastApprovalOn.ToString("yyyy MMMM")
                    };
                    saleDateList.Add(model);
                }
            }
            var filtersaleDate = new List<MonthlySalesDate>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (filtersaleDate.Count > 0)
                    {
                        var check = filtersaleDate.Where(e => e.LastApprovalOn == data.LastApprovalOn).FirstOrDefault();
                        if (check == null)
                        {
                            filtersaleDate.Add(data);
                        }
                    }
                    else
                    {
                        filtersaleDate.Add(data);
                    }
                }
            }
            List<int> getCustomerList = new List<int>();
            if (saleDateList.Count > 0)
            {
                foreach (var data in saleDateList)
                {
                    if (getCustomerList.Count > 0)
                    {
                        var check = getCustomerList.Where(e => e == data.CustomerParticularId).Count();
                        if (check < 1)
                        {
                            getCustomerList.Add(data.CustomerParticularId);
                        }
                    }
                    else
                    {
                        getCustomerList.Add(data.CustomerParticularId);
                    }
                }
            }
            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd/MM/yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                string rateFormat = GetRateFormat(12);
                string sgdFormat = GetDecimalFormat(sgdDp);

                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Customer Summary");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["N1:O1"].Merge = true;
                saleWS.Cells[1, 14].Style.Font.Bold = true;
                saleWS.Cells[1, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 14].Value = exportDate;

                saleWS.Cells["A2:O2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "Customer Summary";
                saleWS.Cells["A3:O3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Value = "FROM  " + fromDate + " TO " + toDate;

                //set first row name
                saleWS.Cells[5, 1].Style.Font.Bold = true;
                saleWS.Cells[5, 1].Value = "CUSTOMER";
                saleWS.Cells[5, 2].Style.Font.Bold = true;
                saleWS.Cells[5, 2].Value = "DATE";
                //saleWS.Cells[5, 3].Style.Font.Bold = true;
                //saleWS.Cells[5, 3].Value = "Total";

                int saleCount = 7;
                int col = 2;
                foreach (var date in filtersaleDate)
                {
                    saleWS.Cells[saleCount, 1, saleCount, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    saleWS.Cells[6, col].Value = date.LastApprovalOn;

                    foreach (var customer in getCustomerList)
                    {
                        var currentSalesCount = getSaleList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn && e.CustomerParticularId == customer).Count();
                        var currentRemittanceCount = getRemittanceList.Where(e => e.LastApprovalOn.ToString("yyyy MMMM") == date.LastApprovalOn && e.CustomerParticularId == customer).Count();
                        var totalSales = currentSalesCount + currentRemittanceCount;
                        var getCustomer = _customerParticularsModel.GetSingle(customer);
                        var customerName = "";
                        if (getCustomer != null)
                        {
                            if (getCustomer.CustomerType == "Corporate & Trading Company")
                            {
                                customerName = getCustomer.CustomerCode + " - " + getCustomer.Company_RegisteredName;
                            }
                            else
                            {
                                customerName = getCustomer.CustomerCode + " - " + getCustomer.Natural_Name;
                            }
                        }
                        saleWS.Cells[saleCount, 1].Value = customerName;
                        saleWS.Cells[saleCount, col].Value = totalSales;
                        saleCount++;
                    }
                    col++;
                    saleCount = 7;
                }

                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                saleWS.Cells[saleWS.Dimension.Address].Style.WrapText = true;

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=customer-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: OST
        public ActionResult OST(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");

            string reportCustomer = TempData["ReportCustomer"].ToString();

            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            TempData.Keep("ReportCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

            List<string> acceptStatus = new List<string>();

            string reportStatus = TempData["ReportStatus"].ToString();
            if (!string.IsNullOrEmpty(reportStatus))
            {
                acceptStatus = reportStatus.Split(',').ToList();
            }

            TempData.Keep("ReportStatus");

            IPagedList<Sale> sales = _salesModel.GetSalesByDatePaged(fromDate, toDate, exceptionStatus, acceptStatus, page, pageSize, productIds, customerId, "Sell");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }
            ViewData["Sale"] = sales;

            //Get Grand Total on last page
            if (page == sales.PageCount)
            {
                decimal[] grandTotals = _salesModel.GetReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");

                ViewData["GrandTotal"] = grandTotals;
            }

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportOST
        public void ExportOST()
        {
            string fromDate = TempData["ReportFromDate"].ToString();
            string toDate = TempData["ReportToDate"].ToString();

            string reportCustomer = TempData["ReportCustomer"].ToString();

            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            TempData.Keep("ReportCustomer");


            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            string reportProduct = TempData["ReportProduct"].ToString();
            TempData.Keep("ReportProduct");

            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

            List<string> acceptStatus = new List<string>();

            string reportStatus = TempData["ReportStatus"].ToString();
            TempData.Keep("ReportStatus");

            if (!string.IsNullOrEmpty(reportStatus))
            {
                acceptStatus = reportStatus.Split(',').ToList();
            }

            IList<Sale> sales = _salesModel.GetSaleDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Sell");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Sell" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Outstanding Sales Transactions");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:J1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:J2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "OUTSTANDING SALES TRANSACTIONS LISTING";
                saleWS.Cells["A3:J3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Style.WrapText = true;
                saleWS.Cells[3, 1].Value = String.Format("Status: {0}", acceptStatus.Count > 0 ? String.Join(", ", acceptStatus) : "All").ToUpper();

                //set first row name
                saleWS.Cells[5, 1].Style.Font.Bold = true;
                saleWS.Cells[5, 1].Value = "DATE";
                saleWS.Cells[5, 2].Style.Font.Bold = true;
                saleWS.Cells[5, 2].Value = "MEMO ID";
                saleWS.Cells[5, 3].Style.Font.Bold = true;
                saleWS.Cells[5, 3].Value = "A/C NO";
                saleWS.Cells[5, 4].Style.Font.Bold = true;
                saleWS.Cells[5, 4].Value = "CONTACT PERSON";
                saleWS.Cells[5, 5].Style.Font.Bold = true;
                saleWS.Cells[5, 5].Value = "FOREIGN CURR";
                saleWS.Cells[5, 6].Style.Font.Bold = true;
                saleWS.Cells[5, 6].Value = "LOCAL CURR";
                saleWS.Cells[5, 7].Style.Font.Bold = true;
                saleWS.Cells[5, 7].Value = "PAYMENT MODE";
                saleWS.Cells[5, 8].Style.Font.Bold = true;
                saleWS.Cells[5, 8].Value = "REFERENCE NO";
                saleWS.Cells[5, 9].Style.Font.Bold = true;
                saleWS.Cells[5, 9].Value = "AMOUNT";
                saleWS.Cells[5, 10].Style.Font.Bold = true;
                saleWS.Cells[5, 10].Value = "STATUS";

                int saleRow = 6;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(rateDP);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                List<int> distinctProduct = new List<int>();

                foreach (Sale sale in sales)
                {
                    decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                    grandTotalForeign += sale.SaleTransactions.Sum(e => e.AmountForeign);
                    grandTotalLocal += sale.SaleTransactions.Sum(e => e.AmountLocal);
                    distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                    //Dump Data
                    saleWS.Cells[saleRow, 1].Value = sale.IssueDate.ToString("dd-MM-yyyy");
                    saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                    saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.CustomerCode;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Natural_Name;
                    }

                    saleWS.Cells[saleRow, 6].Value = totalSales.ToString(sgdFormat);
                    saleWS.Cells[saleRow, 10].Value = sale.Status;

                    List<string> paymentModes = new List<string>();

                    if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                    {
                        paymentModes = sale.LocalPaymentMode.Split(',').ToList();

                        foreach (string mode in paymentModes)
                        {
                            saleWS.Cells[saleRow, 7].Value = mode.ToUpper();

                            if (mode == "Cash")
                            {
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                            }
                            else if (mode == "Cheque 1")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque1No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 2")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque2No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 3")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque3No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                            }
                            else
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.BankTransferNo;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                            }
                            saleRow++;
                        }
                    }
                    else
                    {
                        saleRow++;
                    }
                    var getForeignDecimal = sale.SaleTransactions.FirstOrDefault().Products.Decimal;
                    foreach (SaleTransaction transaction in sale.SaleTransactions)
                    {
                        saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode + "    N";
                        saleWS.Cells[saleRow, 2].Value = "1";
                        saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        saleWS.Cells[saleRow, 5].Value = transaction.AmountForeign.ToString(GetDecimalFormat(getForeignDecimal));
                        saleWS.Cells[saleRow, 6].Value = transaction.AmountLocal.ToString(sgdFormat);
                        saleRow++;
                    }

                    saleRow++;
                }

                if (sales.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 4].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 4].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        var getForeignDecimal = _productsModel.GetSingle(distinctProduct.FirstOrDefault()).Decimal;
                        saleWS.Cells[saleRow, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                        //saleWS.Cells[saleRow, 5].Value = grandTotalForeign.ToString("#,##0.00######");
                        saleWS.Cells[saleRow, 5].Value = grandTotalForeign.ToString(GetDecimalFormat(getForeignDecimal));
                    }
                    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                }

                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col2 = saleWS.Column(2).Width;
                if (width_col2 > 7)
                {
                    saleWS.Column(2).Width = 7;
                    saleWS.Column(2).Style.WrapText = true;
                }
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 12)
                {
                    saleWS.Column(4).Width = 12;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }
                double width_col10 = saleWS.Column(10).Width;
                if (width_col10 > 10)
                {
                    saleWS.Column(10).Width = 10;
                    saleWS.Column(10).Style.WrapText = true;
                }
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=outstanding-sales-transactions-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //GET: OPT
        public ActionResult OPT(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string fromDate = null;
            string toDate = null;

            string reportCustomer = TempData["ReportCustomer"].ToString();
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            TempData.Keep("ReportCustomer");

            string reportProduct = TempData["ReportProduct"].ToString();
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            TempData.Keep("ReportProduct");

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

            List<string> acceptStatus = new List<string>();

            string reportStatus = TempData["ReportStatus"].ToString();
            if (!string.IsNullOrEmpty(reportStatus))
            {
                acceptStatus = reportStatus.Split(',').ToList();
            }

            TempData.Keep("ReportStatus");

            IPagedList<Sale> sales = _salesModel.GetSalesByDatePaged(fromDate, toDate, exceptionStatus, acceptStatus, page, pageSize, productIds, customerId, "Buy");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Buy" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }
            ViewData["Sale"] = sales;

            //Get Grand Total on last page
            if (page == sales.PageCount)
            {
                decimal[] grandTotals = _salesModel.GetReportGrandTotal(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Buy");

                ViewData["GrandTotal"] = grandTotals;
            }

            ViewData["CompanyName"] = _settingsModel.GetCodeValue("COMPANY_NAME");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportOPT
        public void ExportOPT()
        {
            string fromDate = null;
            string toDate = null;

            string reportCustomer = TempData["ReportCustomer"].ToString();
            TempData.Keep("ReportCustomer");
            int customerId = 0;
            if (!string.IsNullOrEmpty(reportCustomer))
            {
                customerId = Convert.ToInt32(reportCustomer);
            }

            string reportProduct = TempData["ReportProduct"].ToString();
            TempData.Keep("ReportProduct");
            List<int> productIds = new List<int>();
            if (!string.IsNullOrEmpty(reportProduct))
            {
                productIds = reportProduct.Split(',').Select(e => Convert.ToInt32(e)).ToList();
            }

            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

            List<string> acceptStatus = new List<string>();

            string reportStatus = TempData["ReportStatus"].ToString();
            TempData.Keep("ReportStatus");
            if (!string.IsNullOrEmpty(reportStatus))
            {
                acceptStatus = reportStatus.Split(',').ToList();
            }

            IList<Sale> sales = _salesModel.GetSaleDataByDate(fromDate, toDate, exceptionStatus, acceptStatus, productIds, customerId, "Buy");
            foreach (Sale sale in sales)
            {
                sale.SaleTransactions = sale.SaleTransactions.Where(e => e.TransactionType == "Buy" && (productIds.Contains(e.CurrencyId) || productIds.Count == 0)).ToList();
            }

            string companyName = _settingsModel.GetCodeValue("COMPANY_NAME").ToUpper();
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Outstanding Purchase Transactions");

                //set header rows
                saleWS.Cells["A1:B1"].Merge = true;
                saleWS.Cells[1, 1].Style.Font.Bold = true;
                saleWS.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                saleWS.Cells[1, 1].Value = companyName;

                saleWS.Cells["G1:J1"].Merge = true;
                saleWS.Cells[1, 7].Style.Font.Bold = true;
                saleWS.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                saleWS.Cells[1, 7].Value = exportDate;

                saleWS.Cells["A2:J2"].Merge = true;
                saleWS.Cells[2, 1].Style.Font.Bold = true;
                saleWS.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[2, 1].Value = "OUTSTANDING PURCHASE TRANSACTIONS LISTING";

                saleWS.Cells["A3:J3"].Merge = true;
                saleWS.Cells[3, 1].Style.Font.Bold = true;
                saleWS.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                saleWS.Cells[3, 1].Style.WrapText = true;
                saleWS.Cells[3, 1].Value = String.Format("Status: {0}", acceptStatus.Count > 0 ? String.Join(", ", acceptStatus) : "All").ToUpper();

                //set first row name
                saleWS.Cells[5, 1].Style.Font.Bold = true;
                saleWS.Cells[5, 1].Value = "DATE";
                saleWS.Cells[5, 2].Style.Font.Bold = true;
                saleWS.Cells[5, 2].Value = "MEMO ID";
                saleWS.Cells[5, 3].Style.Font.Bold = true;
                saleWS.Cells[5, 3].Value = "A/C NO";
                saleWS.Cells[5, 4].Style.Font.Bold = true;
                saleWS.Cells[5, 4].Value = "CONTACT PERSON";
                saleWS.Cells[5, 5].Style.Font.Bold = true;
                saleWS.Cells[5, 5].Value = "FOREIGN CURR";
                saleWS.Cells[5, 6].Style.Font.Bold = true;
                saleWS.Cells[5, 6].Value = "LOCAL CURR";
                saleWS.Cells[5, 7].Style.Font.Bold = true;
                saleWS.Cells[5, 7].Value = "PAYMENT MODE";
                saleWS.Cells[5, 8].Style.Font.Bold = true;
                saleWS.Cells[5, 8].Value = "REFERENCE NO";
                saleWS.Cells[5, 9].Style.Font.Bold = true;
                saleWS.Cells[5, 9].Value = "AMOUNT";
                saleWS.Cells[5, 10].Style.Font.Bold = true;
                saleWS.Cells[5, 10].Value = "STATUS";

                int saleRow = 6;

                string sgdFormat = GetDecimalFormat(sgdDp);
                string rateFormat = GetRateFormat(rateDP);

                decimal grandTotalForeign = 0;
                decimal grandTotalLocal = 0;
                List<int> distinctProduct = new List<int>();

                foreach (Sale sale in sales)
                {
                    decimal totalSales = sale.SaleTransactions.Sum(e => e.AmountLocal);
                    grandTotalForeign += sale.SaleTransactions.Sum(e => e.AmountForeign);
                    grandTotalLocal += sale.SaleTransactions.Sum(e => e.AmountLocal);
                    distinctProduct.AddRange(sale.SaleTransactions.Select(e => e.CurrencyId));

                    //Dump Data
                    saleWS.Cells[saleRow, 1].Value = sale.IssueDate.ToString("dd-MM-yyyy");
                    saleWS.Cells[saleRow, 2].Value = sale.MemoID;
                    saleWS.Cells[saleRow, 3].Value = sale.CustomerParticulars.CustomerCode;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        saleWS.Cells[saleRow, 4].Value = sale.CustomerParticulars.Natural_Name;
                    }

                    saleWS.Cells[saleRow, 6].Value = totalSales.ToString(sgdFormat);
                    saleWS.Cells[saleRow, 10].Value = sale.Status;

                    if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                    {
                        string[] paymentModes = sale.LocalPaymentMode.Split(',');

                        foreach (string mode in paymentModes)
                        {
                            saleWS.Cells[saleRow, 7].Value = mode.ToUpper();

                            if (mode == "Cash")
                            {
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.CashAmount).ToString(sgdFormat);
                            }
                            else if (mode == "Cheque 1")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque1No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque1Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 2")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque2No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque2Amount).ToString(sgdFormat); ;
                            }
                            else if (mode == "Cheque 3")
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.Cheque3No;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.Cheque3Amount).ToString(sgdFormat); ;
                            }
                            else
                            {
                                saleWS.Cells[saleRow, 8].Value = sale.BankTransferNo;
                                saleWS.Cells[saleRow, 9].Value = Convert.ToDecimal(sale.BankTransferAmount).ToString(sgdFormat); ;
                            }

                            saleRow++;
                        }
                    }
                    else
                    {
                        saleRow++;
                    }

                    foreach (SaleTransaction transaction in sale.SaleTransactions)
                    {
                        saleWS.Cells[saleRow, 1].Value = transaction.Products.CurrencyCode + "    N";
                        saleWS.Cells[saleRow, 2].Value = "1";
                        saleWS.Cells[saleRow, 3].Value = transaction.Rate.ToString(rateFormat);
                        saleWS.Cells[saleRow, 5].Value = transaction.AmountForeign.ToString(sgdFormat);
                        saleWS.Cells[saleRow, 6].Value = transaction.AmountLocal.ToString(sgdFormat);
                        saleRow++;
                    }

                    saleRow++;
                }

                if (sales.Count > 0)
                {
                    saleRow--;
                    saleWS.Cells[saleRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    saleWS.Cells[saleRow, 4].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 4].Value = "GRAND TOTAL";
                    if (distinctProduct.Distinct().Count() == 1)
                    {
                        saleWS.Cells[saleRow, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                        saleWS.Cells[saleRow, 5].Style.Font.Bold = true;
                        saleWS.Cells[saleRow, 5].Value = grandTotalForeign.ToString("#,##0.00######");
                    }
                    saleWS.Cells[saleRow, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    saleWS.Cells[saleRow, 6].Style.Font.Bold = true;
                    saleWS.Cells[saleRow, 6].Value = grandTotalLocal.ToString(sgdFormat);
                }
                saleWS.PrinterSettings.TopMargin = 0.35M;
                saleWS.PrinterSettings.RightMargin = 0.35M;
                saleWS.PrinterSettings.BottomMargin = 0.35M;
                saleWS.PrinterSettings.LeftMargin = 0.35M;
                saleWS.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                saleWS.Cells[saleWS.Dimension.Address].Style.Font.Size = 8;
                saleWS.Cells[saleWS.Dimension.Address].AutoFitColumns();
                double width_col4 = saleWS.Column(4).Width;
                if (width_col4 > 12)
                {
                    saleWS.Column(4).Width = 12;
                    saleWS.Column(4).Style.WrapText = true;
                }
                double width_col7 = saleWS.Column(7).Width;
                if (width_col7 > 10)
                {
                    saleWS.Column(7).Width = 10;
                    saleWS.Column(7).Style.WrapText = true;
                }
                double width_col8 = saleWS.Column(8).Width;
                if (width_col8 > 10)
                {
                    saleWS.Column(8).Width = 10;
                    saleWS.Column(8).Style.WrapText = true;
                }
                double width_col10 = saleWS.Column(10).Width;
                if (width_col10 > 10)
                {
                    saleWS.Column(10).Width = 10;
                    saleWS.Column(10).Style.WrapText = true;
                }
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=outstanding-purchase-transactions-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //Report Name Dropdown
        public Dropdown[] ReportNameDDL()
        {
            List<string> reportNames = new List<string>
            {
                "Customer Summary|CustomerSummary",
                "Sales Transaction Summary|STS",
                "Sales by Memo Transaction Summary|SMTSS",
                "Sales by Memo Transaction Summary Details|SMTS",
                "Sales by Transaction Type Summary|STTSS",
                "Sales by Transaction Type Summary Details|STTS",
                "Purchase Transaction Summary|PTS",
                "Inventory Balance, Average Purchase and Last Buying Rate|BPB",
                "Sales With Customer Details|SWCD",
                "Outstanding Sales Transactions|OST",
                "Outstanding Purchase Transactions|OPT",
                "Remittance Records|RMTR",
                "Remittance Transactions|RMTT",
            };

            Dropdown[] ddl = new Dropdown[reportNames.Count];

            int count = 0;

            foreach (string reportName in reportNames)
            {
                string[] report = reportName.Split('|');

                ddl[count] = new Dropdown { name = report[0], val = report[1] };

                count++;
            }

            return ddl;
        }

        public Dropdown[] CustomerTypeDDL()
        {
            List<string> reportNames = new List<string>
            {
                "All|",
                "Corporate & Trading Company|Corporate",
                "Natural Person|Natural Person"
            };

            Dropdown[] ddl = new Dropdown[reportNames.Count];

            int count = 0;

            foreach (string reportName in reportNames)
            {
                string[] report = reportName.Split('|');

                ddl[count] = new Dropdown { name = report[0], val = report[1] };

                count++;
            }

            return ddl;
        }

        //Customer Dropdown
        public Dropdown[] CustomerDDL()
        {
            IList<CustomerParticular> customerParticulars = _customerParticularsModel.GetAllByStatus("Active").OrderByDescending(e => e.CustomerType == "Corporate & Trading Company").ThenBy(e => e.Company_RegisteredName).ThenBy(e => e.Natural_Name).ToList();

            Dropdown[] ddl = new Dropdown[customerParticulars.Count + 1];
            ddl[0] = new Dropdown { val = "0", name = "All Customers" };

            int count = 1;

            foreach (CustomerParticular customer in customerParticulars)
            {
                if (customer.CustomerType == "Corporate & Trading Company")
                {
                    if (customer.Others[0].CustomerProfile == "Incomplete")
                    {
                        ddl[count] = new Dropdown { name = customer.CustomerCode + " - " + customer.Company_RegisteredName + " (" + customer.Others[0].CustomerProfile + ")", val = customer.ID.ToString() };
                    }
                    else
                    {
                        ddl[count] = new Dropdown { name = customer.CustomerCode + " - " + customer.Company_RegisteredName, val = customer.ID.ToString() };
                    }
                }
                else
                {
                    if (customer.Others[0].CustomerProfile == "Incomplete")
                    {
                        ddl[count] = new Dropdown { name = customer.CustomerCode + " - " + customer.Natural_Name + " (" + customer.Others[0].CustomerProfile + ")", val = customer.ID.ToString() };
                    }
                    else
                    {
                        ddl[count] = new Dropdown { name = customer.CustomerCode + " - " + customer.Natural_Name, val = customer.ID.ToString() };
                    }
                }
                count++;
            }

            return ddl;
        }

        public Dropdown[] AgentDDL()
        {
            IList<Agents> agents = _agentsModel.GetAll().Where(e => e.IsDeleted == "N").OrderByDescending(e => e.ID).ToList();

            Dropdown[] ddl = new Dropdown[agents.Count + 1];
            ddl[0] = new Dropdown { val = "0", name = "All Agents" };

            int count = 1;

            foreach (Agents agent in agents)
            {
                ddl[count] = new Dropdown { name = agent.AgentId + " - " + agent.CompanyName, val = agent.ID.ToString() };
                count++;
            }

            return ddl;
        }

        public Dropdown[] CountryDDL()
        {
            List<Countries> countries = new List<Countries>();
            using (var context = new DataAccess.GreatEastForex())
            {
                var countryList = context.Countries.Where(e => e.IsDeleted == 0).OrderBy(e => e.ID).ToList();
                countries = countryList;
            }
            Dropdown[] ddl = new Dropdown[countries.Count + 1];
            ddl[0] = new Dropdown { val = "0", name = "All" };
            int count = 1;

            foreach (Countries country in countries)
            {
                ddl[count] = new Dropdown { val = country.ID.ToString(), name = country.Name };

                count++;
            }

            return ddl;
        }

        //Product Dropdown
        public Dropdown[] ProductDDL()
        {
            IList<Product> products = _productsModel.GetAll().OrderBy(e => e.CurrencyCode).ToList();

            Dropdown[] ddl = new Dropdown[products.Count];

            int count = 0;

            foreach (Product product in products)
            {
                ddl[count] = new Dropdown { val = product.ID.ToString(), name = product.CurrencyCode + " - " + product.CurrencyName };

                count++;
            }

            return ddl;
        }

        public Dropdown[] RemittanceProductDDL()
        {
            IList<RemittanceProducts> products = _remittanceProductsModel.GetAll().OrderBy(e => e.CurrencyCode).ToList();

            Dropdown[] ddl = new Dropdown[products.Count];

            int count = 0;

            foreach (RemittanceProducts product in products)
            {
                ddl[count] = new Dropdown { val = product.ID.ToString(), name = product.CurrencyCode + " - " + product.CurrencyName };

                count++;
            }

            return ddl;
        }

        //Status Dropdown
        public Dropdown[] StatusDDL(string check = null)
        {
            List<string> status = new List<string>
            {
                "Pending GM Approval|Pending GM Approval",
                "Pending GM Approval (Rejected)|Pending GM Approval (Rejected)",
                "Pending Accounts|Pending Accounts",
                "Pending Packing|Pending Packing",
                "Pending Delivery|Pending Delivery",
                "Pending Incoming Delivery|Pending Incoming Delivery",
                "Pending Cashier|Pending Cashier",
                "Pending Assign Delivery|Pending Assign Delivery",
            };

            Dropdown[] ddl = new Dropdown[status.Count];

            int count = 0;

            foreach (string stat in status)
            {
                string[] s = stat.Split('|');

                ddl[count] = new Dropdown { name = s[0], val = s[1] };

                count++;
            }

            return ddl;
        }

        //Status Dropdown
        public Dropdown[] SalesStatusDDL()
        {
            List<string> status = new List<string>
            {
                "All|",
                "Pending Only|Pending",
                "Completed Only|Completed"
            };

            Dropdown[] ddl = new Dropdown[status.Count];

            int count = 0;

            foreach (string stat in status)
            {
                string[] s = stat.Split('|');

                ddl[count] = new Dropdown { name = s[0], val = s[1] };

                count++;
            }

            return ddl;
        }

        //Transaction Type Dropdown
        public Dropdown[] TranscationTypeDDL()
        {
            List<string> status = new List<string>
            {
                "All|",
                "Buy|Buy",
                "Sell|Sell",
                "Encashment|Encashment",
                "Remittance|Remittance"
            };

            Dropdown[] ddl = new Dropdown[status.Count];

            int count = 0;

            foreach (string stat in status)
            {
                string[] s = stat.Split('|');

                ddl[count] = new Dropdown { name = s[0], val = s[1] };

                count++;
            }

            return ddl;
        }

        public Dropdown[] RemittanceStatusDDL()
        {
            List<string> status = new List<string>
            {
                "All Sales|",
                "All Completed|All Completed",
                "All Pending|All Pending",
                "Pending Accounts|Pending Accounts",
                "Pending Accounts(Check Funds)|Pending Accounts (Check Funds)",
                "Pending Accounts(Check Transactions)|Pending Accounts (Check Transactions)",
                "Pending Customer|Pending Customer",
                "Pending GM Approval|Pending GM Approval",
                "Pending GM Approval(Rejected)|Pending GM Approval (Rejected)"
            };

            Dropdown[] ddl = new Dropdown[status.Count];

            int count = 0;

            foreach (string stat in status)
            {
                string[] s = stat.Split('|');

                ddl[count] = new Dropdown { name = s[0], val = s[1] };

                count++;
            }

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

        public string GetRateFormat(int dp)
        {
            string format = "#,##0.";

            switch (dp)
            {
                case 1:
                    format += "#";
                    break;
                case 2:
                    format += "##";
                    break;
                case 3:
                    format += "###";
                    break;
                case 4:
                    format += "####";
                    break;
                case 5:
                    format += "#####";
                    break;
                case 6:
                    format += "######";
                    break;
                case 7:
                    format += "#######";
                    break;
                case 8:
                    format += "########";
                    break;
                case 9:
                    format += "#########";
                    break;
                case 10:
                    format += "##########";
                    break;
                case 11:
                    format += "###########";
                    break;
                case 12:
                    format += "############";
                    break;
                default:
                    format += "####";
                    break;
            }

            return format;
        }
    }

    public class RedirectingActionReport : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Access",
                    action = "Index"
                }));
            }
            else if (!(HttpContext.Current.Session["UserRole"].ToString().Contains("Finance") || HttpContext.Current.Session["UserRole"].ToString().Contains("General Manager") || HttpContext.Current.Session["UserRole"].ToString().Contains("Super Admin")))
            {
                filterContext.Controller.TempData.Add("Result", "danger|You have no access to this module!");

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Access",
                    action = "Logout"
                }));
            }
        }
    }
}