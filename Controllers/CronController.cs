using DataAccess;
using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using DataAccess.Report;
using Ionic.Zip;

namespace GreatEastForex.Controllers
{
	public class CronController : Controller
	{
		private IEndDayTradeRepository _endDayTradesModel;
		private IEndDayTradeTransactionRepository _endDayTradeTransactionsModel;
		private ISaleRepository _salesModel;
		private ISaleTransactionRepository _saleTransactionsModel;
		private IProductRepository _productsModel;
		private IProductInventoryRepository _productInventoriesModel;
		private ICurrencyClosingBalanceRepository _closingBalancesModel;
		private ISettingRepository _settingsModel;
		private ICustomerParticularRepository _customerParticularsModel;
		private int sgdDp = 2;
		private int rateDP = 8;

		public CronController()
			: this(new EndDayTradeRepository(), new EndDayTradeTransactionRepository(), new SaleRepository(), new SaleTransactionRepository(), new ProductRepository(), new ProductInventoryRepository(), new CurrencyClosingBalanceRepository(), new SettingRepository(), new CustomerParticularRepository())
		{

		}

		public CronController(IEndDayTradeRepository endDayTradesModel, IEndDayTradeTransactionRepository endDayTradeTransactionsModel, ISaleRepository salesModel, ISaleTransactionRepository saleTransactionsModel, IProductRepository productsModel, IProductInventoryRepository productInventoriesModel, ICurrencyClosingBalanceRepository closingBalancesModel, ISettingRepository settingsModel, ICustomerParticularRepository customerParticularsModel)
		{
			_endDayTradesModel = endDayTradesModel;
			_endDayTradeTransactionsModel = endDayTradeTransactionsModel;
			_salesModel = salesModel;
			_saleTransactionsModel = saleTransactionsModel;
			_productsModel = productsModel;
			_productInventoriesModel = productInventoriesModel;
			_closingBalancesModel = closingBalancesModel;
			_settingsModel = settingsModel;
			_customerParticularsModel = customerParticularsModel;
			Product sgd = _productsModel.FindCurrencyCode("SGD");
			sgdDp = sgd.Decimal;
			ViewData["SGDDP"] = sgdDp;
			ViewData["RateDP"] = rateDP;
			ViewData["SGDFormat"] = GetDecimalFormat(sgdDp);
			ViewData["RateFormat"] = GetRateFormat(rateDP);
		}

		//Task: Daily Back-up All Report and save into server. (Zip format)
		//Cron: 12:01 Daily
		public void RunCron()
		{
			DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
			DateTime yesterday = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

			string checkpath = Server.MapPath("~/FileUploads/DailyBackup/");
			string zipToday = DateTime.Now.ToString("yyyyMMddHHmm");

			ExportBPB(today, yesterday, checkpath);
			ExportSTS(today, yesterday, checkpath);
			ExportPTS(today, yesterday, checkpath);
			ExportSWCD(today, yesterday, checkpath);
			ExportOST(today, yesterday, checkpath);
			ExportOPT(checkpath);

			ExportSTR(today, yesterday, checkpath);
			ExportBTR(today, yesterday, checkpath);
			ExportCBR(today, yesterday, checkpath);

			//Once Complete, add into zip and remove all the items
			//Check Folder have xlsx files or not.
			////Check Folder have DAT files or not.
			string[] files = Directory.GetFiles(Server.MapPath("~/FileUploads/DailyBackup/"), "*.xlsx");
			string[] datFiles = Directory.GetFiles(Server.MapPath("~/FileUploads/DailyBackup/"), "*.DAT");

			//if have excel files or dat files
			if (files.Length > 0 || datFiles.Length > 0)
			{
				using (ZipFile zip = new ZipFile())
				{
					foreach (string file in files)
					{
						// add this excel files into zip
						zip.AddFile(file,"DailyBackup_" + today.ToString("yyyyMMdd"));
					}

					foreach (string dat_file in datFiles)
					{
						// add this dat files into zip
						zip.AddFile(dat_file, "DailyBackup_" + today.ToString("yyyyMMdd"));
					}

					//Save the folder name as DailyBackupZip_DATE
					zip.Save(Server.MapPath("~/FileUploads/DailyBackup/") + "DailyBackupZip_" + zipToday + ".zip");
					zip.Dispose();
				}

				//if Today Zip file is exist, then remove all the excel files.
				if (Directory.GetFiles(Server.MapPath("~/FileUploads/DailyBackup/"), "DailyBackupZip_" + zipToday + ".zip").Length > 0 || Directory.GetFiles(Server.MapPath("~/FileUploads/DailyBackup/"), "DailyBackupZip_" + zipToday + ".DAT").Length > 0)
				{
					//remove all the excel files
					foreach (string filePath in files)
					{
						System.IO.File.Delete(filePath);
					}

					//remove all the DAT files
					foreach (string dat_file in datFiles)
					{
						System.IO.File.Delete(dat_file);
					}

					string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/DailyBackupEmailTemplate.html"));
					ListDictionary replacements = new ListDictionary();
					string urlText = Request.Url.GetLeftPart(UriPartial.Authority) + "/FileUploads/DailyBackup/" + "DailyBackupZip_" + zipToday + ".zip";
					replacements.Add("<%Url%>", urlText);
					replacements.Add("<%UrlText%>", urlText);

					//Send Email
					EmailHelper.sendEmailDailyBackup("Daily Backup - " + zipToday, body, replacements, ConfigurationManager.AppSettings["SystemEmailDailyBackupAddress"].ToString());
				}
			}
		}

		//GET: ExportSTS
		public void ExportSTS(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");
			int customerId = 0;

			List<int> productIds = new List<int>();

			List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

			List<string> acceptStatus = new List<string>();

			string reportSalesStatus = "";

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "sales-transactions-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		//GET: ExportPTS
		public void ExportPTS(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");

			int customerId = 0;

			List<int> productIds = new List<int>();

			List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected" };

			List<string> acceptStatus = new List<string>();

			string reportSalesStatus = "";

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "purchase-transactions-summary-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		public void ExportBPB(DateTime today, DateTime yesterday, string checkpath)
		{
			//get today and tomorrow assignment records.

			string sgdFormat = GetDecimalFormat(sgdDp);
			string date = yesterday.ToString("yyyy-MM-dd");

			DateTime? closingStart = yesterday;
			DateTime closingEnd = yesterday;

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "currency-closing-balance-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		//GET: ExportOPT
		public void ExportOPT(string checkpath)
		{
			string fromDate = null;
			string toDate = null;

			int customerId = 0;

			List<int> productIds = new List<int>();

			List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

			List<string> acceptStatus = new List<string>();

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "outstanding-purchase-transactions-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		//GET: ExportSWCD
		public void ExportSWCD(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "sales-customer-details-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		//GET: ExportOST
		public void ExportOST(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");
			int customerId = 0;

			List<int> productIds = new List<int>();
			List<string> exceptionStatus = new List<string>() { "Cancelled", "Rejected", "Completed", "Pending Delete GM Approval" };

			List<string> acceptStatus = new List<string>();

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

				byte[] bin = pck.GetAsByteArray();

				string filePath = checkpath + "outstanding-sales-transactions-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
				System.IO.File.WriteAllBytes(filePath, bin);
			}
		}

		//GET: ExportSTR
		public void ExportSTR(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");

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

			string filePath = checkpath + "SELLTRAN-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".DAT";

			System.IO.File.WriteAllText(filePath, message.ToString());
		}

		//GET: ExportBTR
		public void ExportBTR(DateTime today, DateTime yesterday, string checkpath)
		{
			string fromDate = yesterday.ToString("yyyy-MM-dd");
			string toDate = yesterday.ToString("yyyy-MM-dd");

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

			string filePath = checkpath + "BUYTRAN-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".DAT";

			System.IO.File.WriteAllText(filePath, message.ToString());
		}

		//GET: ExportCBR
		public void ExportCBR(DateTime today, DateTime yesterday, string checkpath)
		{
			string date = yesterday.ToString("yyyy-MM-dd"); ; //DATE-ENTRY RECORD_DATE

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

			string filePath = checkpath + "CLOSEBAL-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".DAT";

			System.IO.File.WriteAllText(filePath, message.ToString());
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
				default:
					format += "####";
					break;
			}

			return format;
		}
	}
}