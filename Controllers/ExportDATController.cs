using DataAccess;
using DataAccess.POCO;
using DataAccess.Report;
using GreatEastForex.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [RedirectingActionReport]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ExportDATController : ControllerBase
    {
        private ISaleRepository _salesModel;
        private ISaleTransactionRepository _saleTransactionsModel;
        private IEndDayTradeRepository _endDayTradesModel;
        private IProductRepository _productsModel;

        public ExportDATController()
            : this(new SaleRepository(), new SaleTransactionRepository(), new EndDayTradeRepository(), new ProductRepository())
        {

        }

        public ExportDATController(ISaleRepository salesModel, ISaleTransactionRepository saleTransactionsModel, IEndDayTradeRepository endDayTradesModel, IProductRepository productsModel)
        {
            _salesModel = salesModel;
            _saleTransactionsModel = saleTransactionsModel;
            _endDayTradesModel = endDayTradesModel;
            _productsModel = productsModel;
        }

        // GET: ExportDAT
        public ActionResult Index()
        {
            if (TempData["ReportName"] != null)
            {
                TempData.Remove("ReportName");
            }

            if (TempData["FromDate"] != null)
            {
                TempData.Remove("FromDate");
            }

            if (TempData["ToDate"] != null)
            {
                TempData.Remove("ToDate");
            }

            if (TempData["Date"] != null)
            {
                TempData.Remove("Date");
            }

            return RedirectToAction("Select");
        }

        //GET: Select
        public ActionResult Select()
        {
            ViewData["ReportName"] = "";
            if (TempData["ReportName"] != null)
            {
                ViewData["ReportName"] = TempData["ReportName"].ToString();
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

            ViewData["Date"] = "";
            if (TempData["Date"] != null)
            {
                ViewData["Date"] = TempData["Date"].ToString();
            }

            Dropdown[] reportNameDDL = ReportNameDDL();
            ViewData["ReportNameDropdown"] = new SelectList(reportNameDDL, "val", "name", ViewData["ReportName"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Select
        [HttpPost]
        public ActionResult Select(FormCollection form)
        {
            ViewData["ReportName"] = form["ReportName"].ToString();
            TempData["ReportName"] = form["ReportName"].ToString();

            ViewData["FromDate"] = form["FromDate"].ToString();
            TempData["FromDate"] = form["FromDate"].ToString();

            ViewData["ToDate"] = form["ToDate"].ToString();
            TempData["ToDate"] = form["ToDate"].ToString();

            ViewData["Date"] = form["Date"].ToString();
            TempData["Date"] = form["Date"].ToString();

            if (string.IsNullOrEmpty(form["ReportName"]))
            {
                ModelState.AddModelError("ReportName", "Report Name is required!");
            }
            else
            {
                List<string> reportNames = new List<string>();
                reportNames.Add("STR");
                reportNames.Add("BTR");
                reportNames.Add("CBR");

                if (!reportNames.Contains(form["ReportName"].ToString()))
                {
                    ModelState.AddModelError("ReportName", "Report Name is not valid!");
                }
                else
                {
                    if (form["ReportName"].ToString() == "STR" || form["ReportName"].ToString() == "BTR")
                    {
                        DateTime? fromDate = null;
                        DateTime? toDate = null;

                        if (string.IsNullOrEmpty(form["FromDate"]))
                        {
                            ModelState.AddModelError("FromDate", "From Date is required!");
                        }
                        else
                        {
                            try
                            {
                                fromDate = Convert.ToDateTime(form["FromDate"]);
                            }
                            catch
                            {
                                ModelState.AddModelError("FromDate", "From Date is not valid!");
                            }
                        }

                        if (string.IsNullOrEmpty(form["ToDate"]))
                        {
                            ModelState.AddModelError("ToDate", "To Date is required!");
                        }
                        else
                        {
                            try
                            {
                                toDate = Convert.ToDateTime(form["ToDate"]);
                            }
                            catch
                            {
                                ModelState.AddModelError("ToDate", "To Date is not valid!");
                            }
                        }

                        if (fromDate != null && toDate != null)
                        {
                            if (fromDate > toDate)
                            {
                                ModelState.AddModelError("FromDate", "From Date is later than To Date!");
                                ModelState.AddModelError("ToDate", "To Date is earlier thatn From Date!");
                            }
                        }
                    }
                    else if (form["ReportName"].ToString() == "CBR")
                    {
                        DateTime? date = null;

                        if (string.IsNullOrEmpty(form["Date"]))
                        {
                            ModelState.AddModelError("Date", "Date is required!");
                        }
                        else
                        {
                            try
                            {
                                date = Convert.ToDateTime(form["Date"]);
                            }
                            catch
                            {
                                ModelState.AddModelError("Date", "Date is not valid!");
                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                string report = form["ReportName"].ToString();

                switch (report)
                {
                    case "STR": ExportSTR(); break;
                    case "BTR": break;
                    case "CBR": break;
                    default: break;
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            Dropdown[] reportNameDDL = ReportNameDDL();
            ViewData["ReportNameDropdown"] = new SelectList(reportNameDDL, "val", "name", ViewData["ReportName"].ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: DownloadDAT
        public void DownloadDAT(string report, string fromDate, string toDate, string date)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            TempData["Date"] = date;

            switch (report)
            {
                case "STR": ExportSTR(); break;
                case "BTR": ExportBTR(); break;
                case "CBR": ExportCBR(); break;
                case "MJ": ExportManualJournal(); break;
                default: break;
            }
        }

        //GET: ExportSTR
        public void ExportSTR()
        {
            string fromDate = TempData["FromDate"].ToString();
            string toDate = TempData["ToDate"].ToString();

            IList<Sale> sales = _salesModel.GetSalesByDateRange(fromDate, toDate).Where(e => e.TransactionType != "Buy").ToList();

            StringBuilder message = new StringBuilder();

            foreach (Sale sale in sales)
            {
                decimal memoTotal = 0;

                foreach (SaleTransaction transaction in sale.SaleTransactions)
                {
                    if (transaction.TransactionType == "Sell")
                    {
                        memoTotal += transaction.AmountLocal;
                    }
                }

                string link = ">"; //LINK
                string itf = "ITF"; //"ITF"
                string memo_no = sale.MemoID; //MEMO_NO
                string trans_date = sale.LastApprovalOn.ToString("dd/MM/yyyy"); //TRANS_DATE
                string ac_no = sale.CustomerParticulars.CustomerCode; //AC_NO
                string contact_person = ""; //CONTACT_PERSON
                if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    contact_person = sale.CustomerParticulars.Company_RegisteredName;
                }
                else
                {
                    contact_person = sale.CustomerParticulars.Natural_Name;
                }
                if (sale.CustomerParticulars.Others[0].CustomerProfile == "Incomplete")
                {
                    contact_person += "(" + sale.CustomerParticulars.Others[0].CustomerProfile + ")";
                }
                string memo_total = memoTotal.ToString("#,##0.00"); //MEMO_TOTAL
                string[] pytModes = new string[16];
                List<string> paymentModes = new List<string>();

                if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                {
                    paymentModes = sale.LocalPaymentMode.Split(',').ToList();
                }
                else
                {
                    paymentModes.Add("Cash");
                }

                int pytModeCount = 1;

                foreach (string mode in paymentModes)
                {
                    if (mode == "Cash")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.CashBank) ? sale.CashBank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = ""; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.CashAmount != null ? Convert.ToDecimal(sale.CashAmount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 1")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque1Bank) ? sale.Cheque1Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque1No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque1Amount != null ? Convert.ToDecimal(sale.Cheque1Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 2")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque2Bank) ? sale.Cheque2Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque2No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque2Amount != null ? Convert.ToDecimal(sale.Cheque2Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 3")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque3Bank) ? sale.Cheque3Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque3No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque3Amount != null ? Convert.ToDecimal(sale.Cheque3Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.BankTransferBank) ? sale.BankTransferBank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.BankTransferNo; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.BankTransferAmount != null ? Convert.ToDecimal(sale.BankTransferAmount).ToString("#,##0.00") : ""; //PYT_AMT
                    }

                    pytModeCount++;
                }

                string pyt_mode = string.Join("~", pytModes); //Concatenated PYT_MODEs
                string memo_pyt = memoTotal.ToString("#,##0.00"); //MEMO_PYT
                string memo_bal = ""; //MEMO_BAL
                if (sale.MemoBalance != null)
                {
                    memo_bal = Convert.ToDecimal(sale.MemoBalance).ToString("#,##0.00");
                }

                message.AppendLine(
                    link + "~" +
                    itf + "~" +
                    memo_no + "~" +
                    trans_date + "~" +
                    ac_no + "~" +
                    contact_person + "~" +
                    memo_total + "~" +
                    pyt_mode + "~" +
                    memo_pyt + "~" +
                    memo_bal);
            }

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", "attachment;  filename=SELLTRAN.DAT");
            Response.Write(message.ToString());
        }

        //GET: ExportBTR
        public void ExportBTR()
        {
            string fromDate = TempData["FromDate"].ToString();
            string toDate = TempData["ToDate"].ToString();

            IList<Sale> sales = _salesModel.GetSalesByDateRange(fromDate, toDate).Where(e => e.TransactionType != "Sell").ToList();

            StringBuilder message = new StringBuilder();

            foreach (Sale sale in sales)
            {
                decimal memoTotal = 0;

                foreach (SaleTransaction transaction in sale.SaleTransactions)
                {
                    if (transaction.TransactionType == "Buy")
                    {
                        memoTotal += transaction.AmountLocal;
                    }
                }

                string link = ">"; //LINK
                string itf = "ITF"; //"ITF"
                string memo_no = sale.MemoID; //MEMO_NO
                string trans_date = sale.LastApprovalOn.ToString("dd/MM/yyyy"); //TRANS_DATE
                string ac_no = sale.CustomerParticulars.CustomerCode; //AC_NO
                string contact_person = ""; //CONTACT_PERSON
                if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    contact_person = sale.CustomerParticulars.Company_RegisteredName;
                }
                else
                {
                    contact_person = sale.CustomerParticulars.Natural_Name;
                }
                if (sale.CustomerParticulars.Others[0].CustomerProfile == "Incomplete")
                {
                    contact_person += "(" + sale.CustomerParticulars.Others[0].CustomerProfile + ")";
                }
                string memo_total = memoTotal.ToString("#,##0.00"); //tMEMO_TOTAL
                string[] pytModes = new string[16];
                List<string> paymentModes = new List<string>();

                if (!string.IsNullOrEmpty(sale.LocalPaymentMode))
                {
                    paymentModes = sale.LocalPaymentMode.Split(',').ToList();
                }
                else
                {
                    paymentModes.Add("Cash");
                }

                int pytModeCount = 1;

                foreach (string mode in paymentModes)
                {
                    if (mode == "Cash")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.CashBank) ? sale.CashBank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = ""; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.CashAmount != null ? Convert.ToDecimal(sale.CashAmount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 1")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque1Bank) ? sale.Cheque1Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque1No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque1Amount != null ? Convert.ToDecimal(sale.Cheque1Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 2")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque2Bank) ? sale.Cheque2Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque2No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque2Amount != null ? Convert.ToDecimal(sale.Cheque2Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else if (mode == "Cheque 3")
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.Cheque3Bank) ? sale.Cheque3Bank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.Cheque3No; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.Cheque3Amount != null ? Convert.ToDecimal(sale.Cheque3Amount).ToString("#,##0.00") : ""; //PYT_AMT
                    }
                    else
                    {
                        pytModes[(pytModeCount - 1) * 4] = mode.ToUpper(); //PYT_MODE
                        pytModes[(pytModeCount - 1) * 4 + 1] = !string.IsNullOrEmpty(sale.BankTransferBank) ? sale.BankTransferBank : "CASH"; //BANK
                        pytModes[(pytModeCount - 1) * 4 + 2] = sale.BankTransferNo; //PYT_REF
                        pytModes[(pytModeCount - 1) * 4 + 3] = sale.BankTransferAmount != null ? Convert.ToDecimal(sale.BankTransferAmount).ToString("#,##0.00") : ""; //PYT_AMT
                    }

                    pytModeCount++;
                }

                string pyt_mode = string.Join("~", pytModes); //Concatenated PYT_MODEs
                string memo_pyt = memoTotal.ToString("#,##0.00"); //MEMO_PYT
                string memo_bal = ""; //MEMO_BAL
                if (sale.MemoBalance != null)
                {
                    memo_bal = Convert.ToDecimal(sale.MemoBalance).ToString("#,##0.00");
                }

                message.AppendLine(
                    link + "~" +
                    itf + "~" +
                    memo_no + "~" +
                    trans_date + "~" +
                    ac_no + "~" +
                    contact_person + "~" +
                    memo_total + "~" +
                    pyt_mode + "~" +
                    memo_pyt + "~" +
                    memo_bal);
            }

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", "attachment;  filename=BUYTRAN.DAT");
            Response.Write(message.ToString());
        }

        //GET: ExportCBR
        public void ExportCBR()
        {
            string date = TempData["Date"].ToString(); //DATE-ENTRY RECORD_DATE

            IList<EndDayTrade> trades = _endDayTradesModel.GetAllDAT(date, date).Where(e => e.Products.CurrencyCode != "SGD").ToList();

            StringBuilder message = new StringBuilder();

            decimal totalStockIn = 0; //TOTAL_STOCK_IN_S$
            string not = "NOT"; //"NOT"

            foreach (EndDayTrade trade in trades)
            {
                totalStockIn += trade.ClosingForeignCurrencyBalance * trade.ClosingAveragePurchaseCost / trade.Products.Unit;
            }

            message.AppendLine(
                date + "~" +
                totalStockIn.ToString("0.00") + "~" +
                not);

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", "attachment;  filename=CLOSEBAL.DAT");
            Response.Write(message.ToString());
        }

        //GET: ExportRMTR
        public void ExportManualJournal()
        {
            string fromDate = TempData["FromDate"].ToString();
            string toDate = TempData["ToDate"].ToString();
            TempData.Keep("ReportFromDate");
            TempData.Keep("ReportToDate");
            List<int> customerList = new List<int>();
            customerList.Add(0);
            List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };
            List<int> productIds = new List<int>();
            List<string> acceptStatus = new List<string>();
            List<string> transactionList = new List<string>();
            //transactionList.Add("Buy");
            //transactionList.Add("Sell");
            var getSaleList = _salesModel.GetSalesSummaryList(fromDate, toDate, productIds, transactionList, customerList).Where(e => e.Status == "Completed").ToList();
            var saleDateList = new List<MonthlySalesDate>();
            var distinctDateList = getSaleList.Select(s => new { s.CustomerParticularId, s.LastApprovalOn }).Distinct().ToList();
            if (distinctDateList.Count > 0)
            {
                foreach (var data in distinctDateList)
                {
                    var model = new MonthlySalesDate()
                    {
                        CustomerParticularId = 0,
                        LastApprovalOn = data.LastApprovalOn.ToString("dd.MM.yyyy")
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
            string exportDate = DateTime.Now.ToString("dd-MM-yyyy");

            decimal totalSales = 0;
            decimal totalPurchases = 0;
            decimal totalDBSSales = 0;
            decimal totalDBSPurchases = 0;
            decimal totalOCBCSales = 0;
            decimal totalOCBCPurchases = 0;
            decimal totalUOBSales = 0;
            decimal totalUOBPurchases = 0;
            decimal totalCashSales = 0;
            decimal totalCashPurchases = 0;
            decimal totalMemoBalance = 0;

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create Worksheet
                ExcelWorksheet saleWS = pck.Workbook.Worksheets.Add("Xerox Export Data");

                //set header rows
                saleWS.Cells[1, 1].Value = "*Narration";
                saleWS.Cells[1, 2].Value = "*Date";
                saleWS.Cells[1, 3].Value = "Description";
                saleWS.Cells[1, 4].Value = "*AccountCode";
                saleWS.Cells[1, 5].Value = "*TaxRate";
                saleWS.Cells[1, 6].Value = "*Amount";
                saleWS.Cells[1, 7].Value = "TrackingName1";
                saleWS.Cells[1, 8].Value = "TrackingOption1";
                saleWS.Cells[1, 9].Value = "TrackingName2";
                saleWS.Cells[1, 10].Value = "TrackingOption2";
                var getAccountCode = AccountCodeDDL();
                int saleRow = 2;
                foreach (var date in filtersaleDate)
                {
                    var getCurrentSale = getSaleList.Where(e => e.LastApprovalOn.ToString("dd.MM.yyyy") == date.LastApprovalOn).ToList();

                    if (getCurrentSale.Count > 0)
                    {
                        foreach (var sale in getCurrentSale)
                        {
                            var getBuy = sale.SaleTransactions.Where(e => e.TransactionType == "Buy").ToList();
                            var getSell = sale.SaleTransactions.Where(e => e.TransactionType == "Sell").ToList();
                            totalSales += getSell.Sum(e => e.AmountLocal);
                            totalPurchases += getBuy.Sum(e => e.AmountLocal);
                            if (sale.TransactionType == "Buy" || sale.TransactionType == "Sell")
                            {
                                if (sale.LocalPaymentMode.Contains("Cheque 1") || sale.LocalPaymentMode.Contains("Cheque 2")
                                    || sale.LocalPaymentMode.Contains("Cheque 3") || sale.LocalPaymentMode.Contains("Bank Transfer"))
                                {
                                    if (sale.Cheque1Bank != null)
                                    {
                                        if (sale.Cheque1Bank == "DBS")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalDBSPurchases += Convert.ToDecimal(sale.Cheque1Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalDBSSales += Convert.ToDecimal(sale.Cheque1Amount);
                                        }
                                        else if (sale.Cheque1Bank == "OCBC")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque1Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque1Amount);
                                        }
                                        else if (sale.Cheque1Bank == "UOB")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalUOBPurchases += Convert.ToDecimal(sale.Cheque1Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalUOBSales += Convert.ToDecimal(sale.Cheque1Amount);
                                        }
                                    }
                                    else
                                    {
                                        if (sale.Cheque1Amount != null)
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque1Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque1Amount);
                                        }
                                    }

                                    if (sale.Cheque2Bank != null)
                                    {
                                        if (sale.Cheque2Bank == "DBS")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalDBSPurchases += Convert.ToDecimal(sale.Cheque2Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalDBSSales += Convert.ToDecimal(sale.Cheque2Amount);
                                        }
                                        else if (sale.Cheque2Bank == "OCBC")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque2Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque2Amount);

                                        }
                                        else if (sale.Cheque2Bank == "UOB")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalUOBPurchases += Convert.ToDecimal(sale.Cheque2Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalUOBSales += Convert.ToDecimal(sale.Cheque2Amount);

                                        }
                                    }
                                    else
                                    {
                                        if (sale.Cheque2Amount != null)
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque2Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque2Amount);
                                        }
                                    }
                                    if (sale.Cheque3Bank != null)
                                    {
                                        if (sale.Cheque3Bank == "DBS")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalDBSPurchases += Convert.ToDecimal(sale.Cheque3Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalDBSSales += Convert.ToDecimal(sale.Cheque3Amount);
                                        }
                                        else if (sale.Cheque3Bank == "OCBC")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque3Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque3Amount);
                                        }
                                        else if (sale.Cheque3Bank == "UOB")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalUOBPurchases += Convert.ToDecimal(sale.Cheque3Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalUOBSales += Convert.ToDecimal(sale.Cheque3Amount);
                                        }
                                    }
                                    else
                                    {
                                        if (sale.Cheque3Amount != null)
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.Cheque3Amount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.Cheque3Amount);
                                        }
                                    }
                                    if (sale.BankTransferBank != null)
                                    {
                                        if (sale.BankTransferBank == "DBS")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalDBSPurchases += Convert.ToDecimal(sale.BankTransferAmount);
                                            else if (sale.TransactionType == "Sell")
                                                totalDBSSales += Convert.ToDecimal(sale.BankTransferAmount);
                                        }
                                        else if (sale.BankTransferBank == "OCBC")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.BankTransferAmount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.BankTransferAmount);
                                        }
                                        else if (sale.BankTransferBank == "UOB")
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalUOBPurchases += Convert.ToDecimal(sale.BankTransferAmount);
                                            else if (sale.TransactionType == "Sell")
                                                totalUOBSales += Convert.ToDecimal(sale.BankTransferAmount);
                                        }
                                    }
                                    else
                                    {
                                        if (sale.BankTransferAmount != null)
                                        {
                                            if (sale.TransactionType == "Buy")
                                                totalOCBCPurchases += Convert.ToDecimal(sale.BankTransferAmount);
                                            else if (sale.TransactionType == "Sell")
                                                totalOCBCSales += Convert.ToDecimal(sale.BankTransferAmount);
                                        }
                                    }
                                }
                                if (sale.LocalPaymentMode.Contains("Cash"))
                                {
                                    if (sale.TransactionType == "Buy")
                                        totalCashPurchases += Convert.ToDecimal(sale.CashAmount);
                                    else if (sale.TransactionType == "Sell")
                                        totalCashSales += Convert.ToDecimal(sale.CashAmount);
                                }
                            }
                            else
                            {
                                if (getBuy.Count > 0)
                                {
                                    totalCashPurchases += Convert.ToDecimal(getBuy.Sum(e => e.AmountLocal));
                                }
                                if(getSell.Count > 0)
                                {
                                    totalCashSales += Convert.ToDecimal(getSell.Sum(e => e.AmountLocal));
                                }                                    
                            }
                            var bankCharge = sale.MemoBalance ?? 0;
                            if (bankCharge < 0)
                            {
                                bankCharge = bankCharge * -1;
                            }
                            totalMemoBalance += bankCharge;
                        }
                    }

                }

                IList<Product> products = _productsModel.GetAll().Where(e => e.CurrencyCode != "SGD").ToList();

                decimal ClosingStockInventories = 0;
                decimal ClosingStockCostSales = 0;
                decimal OpenStockInventories = 0;
                decimal OpenStockCostSales = 0;

                foreach (Product product in products)
                {
                    string amountForeignFormat = GetDecimalFormat(product.Decimal);

                    decimal foreignCurrencyBal = product.ProductInventories[0].TotalInAccount;
                    decimal foreignCurrencyBalOpen = product.ProductInventories[0].TotalInAccount;
                    decimal averageRate = Convert.ToDecimal(product.BuyRate);
                    decimal averageRateOpen = Convert.ToDecimal(product.BuyRate);
                    var toDateBeforeOneDay = Convert.ToDateTime(fromDate).AddDays(-1).ToString("dd-MM-yyyy");
                    //Take from end day of trade
                    EndDayTrade productEndDayOfTrade = _endDayTradesModel.GetProductTrade(product.ID, toDate);
                    EndDayTrade productEndDayOfTradeOpen = _endDayTradesModel.GetProductTrade(product.ID, toDateBeforeOneDay);

                    if (productEndDayOfTrade != null)
                    {
                        foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                        averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                    }
                    else
                    {
                        productEndDayOfTrade = _endDayTradesModel.GetProductLastTrade(product.ID, toDate);

                        if (productEndDayOfTrade != null)
                        {
                            foreignCurrencyBal = productEndDayOfTrade.ClosingForeignCurrencyBalance;
                            averageRate = productEndDayOfTrade.ClosingAveragePurchaseCost;
                        }
                    }
                    if (productEndDayOfTradeOpen != null)
                    {
                        foreignCurrencyBalOpen = productEndDayOfTradeOpen.ClosingForeignCurrencyBalance;
                        averageRateOpen = productEndDayOfTradeOpen.ClosingAveragePurchaseCost;
                    }
                    else
                    {
                        productEndDayOfTradeOpen = _endDayTradesModel.GetProductLastTrade(product.ID, toDateBeforeOneDay);

                        if (productEndDayOfTradeOpen != null)
                        {
                            foreignCurrencyBalOpen = productEndDayOfTradeOpen.ClosingForeignCurrencyBalance;
                            averageRateOpen = productEndDayOfTradeOpen.ClosingAveragePurchaseCost;
                        }
                    }

                    decimal closingBal = foreignCurrencyBal * averageRate / product.Unit;
                    decimal OpenBal = foreignCurrencyBalOpen * averageRateOpen / product.Unit;

                    //CurrencyClosingBalance bal = new CurrencyClosingBalance();
                    //bal.ProductId = product.ID;
                    //bal.Code = product.CurrencyCode;
                    //bal.Currency = product.CurrencyName;
                    //bal.AmountForeignFormat = amountForeignFormat;
                    //bal.ForeignCurrencyClosingBal = foreignCurrencyBal; // Renamed to Forex Closing Bal
                    //bal.AveragePurchaseCostOrLastBuyingRate = averageRate; // Renamed to Weighted Purchase Rate
                    //bal.ClosingBalAtAveragePurchaseOrLastBuying = closingBal; // Renamed to SGD Equivalent
                    ClosingStockInventories += closingBal;
                    ClosingStockCostSales += closingBal;
                    OpenStockInventories += OpenBal;
                    OpenStockCostSales += OpenBal;
                }
                var ClosingStockInventories2dp = ClosingStockInventories.ToString(GetDecimalFormat(2));
                var ClosingStockCostSales2dp = ClosingStockCostSales.ToString(GetDecimalFormat(2));
                var OpenStockInventories2dp = OpenStockInventories.ToString(GetDecimalFormat(2));
                var OpenStockCostSales2dp = OpenStockCostSales.ToString(GetDecimalFormat(2));
                var OpenCloseInventoriesBalance = Convert.ToDecimal(ClosingStockInventories2dp) - Convert.ToDecimal(OpenStockInventories2dp);
                var OpenCloseCostSalesBalance = Convert.ToDecimal(ClosingStockCostSales2dp) - Convert.ToDecimal(OpenStockCostSales2dp);
                foreach (var code in getAccountCode)
                {
                    saleWS.Cells[saleRow, 1].Value = "Transaction for *" + Convert.ToDateTime(fromDate).ToString("dd MMM yyyy") + " to *" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy");
                    saleWS.Cells[saleRow, 2].Value = Convert.ToDateTime(toDate).ToString("dd MMM yyyy");
                    saleWS.Cells[saleRow, 3].Value = "Transaction for *" + Convert.ToDateTime(fromDate).ToString("dd MMM yyyy") + " to *" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy");
                    saleWS.Cells[saleRow, 4].Value = Convert.ToInt32(code.Code);
                    saleWS.Cells[saleRow, 5].Value = "No Tax (0%)";
                    if (!string.IsNullOrEmpty(code.Description))
                    {
                        string finalValue = "0";
                        switch (code.Description)
                        {
                            case "1010":
                                if (totalSales > 0)
                                    finalValue = "-" + totalSales.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "1990":
                                if (totalPurchases > 0)
                                    finalValue = totalPurchases.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61001":
                                if (totalDBSSales > 0)
                                    finalValue = totalDBSSales.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61002":
                                if (totalDBSPurchases > 0)
                                    finalValue = "-" + totalDBSPurchases.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61061":
                                if (totalUOBSales > 0)
                                    finalValue = totalUOBSales.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61062":
                                if (totalUOBPurchases > 0)
                                    finalValue = "-" + totalUOBPurchases.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61011":
                                if (totalOCBCSales > 0)
                                    finalValue = totalOCBCSales.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61012":

                                if (totalOCBCPurchases > 0)
                                    finalValue = "-" + totalOCBCPurchases.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61201":
                                if (totalCashSales > 0)
                                    finalValue = totalCashSales.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "61202":
                                if (totalCashPurchases > 0)
                                    finalValue = "-" + totalCashPurchases.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "1995":
                                if (OpenCloseInventoriesBalance != 0)
                                    finalValue = (OpenCloseInventoriesBalance * -1).ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "6995":
                                if (OpenCloseCostSalesBalance != 0)
                                    finalValue = OpenCloseCostSalesBalance.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                            case "2340":
                                if (totalMemoBalance != 0)
                                    finalValue = totalMemoBalance.ToString(GetDecimalFormat(2));
                                else
                                    finalValue = "0.00";
                                saleWS.Cells[saleRow, 6].Value = Convert.ToDecimal(finalValue);
                                saleWS.Cells[saleRow, 6].Style.Numberformat.Format = "#,##0.00";
                                break;
                        }

                    }
                    saleRow++;
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
                Response.AddHeader("content-disposition", "attachment;  filename=xerox-export-data-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //Report Name Dropdown
        public Dropdown[] ReportNameDDL()
        {
            Dropdown[] ddl = new Dropdown[4];
            ddl[0] = new Dropdown { name = "Sell Transaction Report", val = "STR" };
            ddl[1] = new Dropdown { name = "Buy Transaction Report", val = "BTR" };
            ddl[2] = new Dropdown { name = "Closing Balance Report", val = "CBR" };
            ddl[3] = new Dropdown { name = "Xerox Export Data", val = "MJ" };
            return ddl;
        }

        //Account Code List
        public CountAccountCode[] AccountCodeDDL()
        {
            CountAccountCode[] ddl = new CountAccountCode[13];
            ddl[0] = new CountAccountCode { Name = "Sales", Code = "1010", Description = "1010" };
            ddl[1] = new CountAccountCode { Name = "Purchases", Code = "1990", Description = "1990" };
            ddl[2] = new CountAccountCode { Name = "DBS Sales", Code = "6100", Description = "61001" };
            ddl[3] = new CountAccountCode { Name = "DBS Purchase", Code = "6100", Description = "61002" };
            ddl[4] = new CountAccountCode { Name = "OCBC Sales", Code = "6101", Description = "61011" };
            ddl[5] = new CountAccountCode { Name = "OCBC Purchase", Code = "6101", Description = "61012" };
            ddl[6] = new CountAccountCode { Name = "UOB Sales", Code = "6106", Description = "61061" };
            ddl[7] = new CountAccountCode { Name = "UOB Purchase", Code = "6106", Description = "61062" };
            //ddl[6] = new CountAccountCode { Name = "Cash in hand", Code = "6110", Description = "6110" };
            //ddl[7] = new CountAccountCode { Name = "Cash in hand", Code = "6110", Description = "61102" };
            ddl[8] = new CountAccountCode { Name = "Cash Sales", Code = "6120", Description = "61201" };
            ddl[9] = new CountAccountCode { Name = "Cash Purchase", Code = "6120", Description = "61202" };
            ddl[10] = new CountAccountCode { Name = "Bank Charges", Code = "2340", Description = "2340" };
            ddl[11] = new CountAccountCode { Name = "Closing stock (Inventories)", Code = "6995", Description = "6995" };
            ddl[12] = new CountAccountCode { Name = "Closing stock (Cost of Sales)", Code = "1995", Description = "1995" };


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