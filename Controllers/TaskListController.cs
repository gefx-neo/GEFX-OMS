using DataAccess;
using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

//using System.Net;
//using System.Net.Mail;
//using System.Collections.Specialized;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TaskListController : ControllerBase
    {
        private ICustomerParticularRepository _customerParticularsModel;
        private ISaleRepository _salesModel;
        private IRemittanceSaleRepository _remittancesalesModel;
        private ISaleTransactionRepository _saleTransactionsModel;
        private IRemittanceOrderRepository _remittanceordersModel;
        private ISaleTransactionDenominationRepository _saleTransactionDenominationsModel;
        private IProductRepository _productsModel;
        private IProductInventoryRepository _productInventoriesModel;
        private IRemittanceProductRepository _remittanceproductsModel;
        private IProductDenominationRepository _productDenominationsModel;
        private IInventoryRepository _inventoriesModel;
        private ITaskListRepository _taskListsModel;
        private IEndDayTradeRepository _endDayTradesModel;
        private IEndDayTradeTransactionRepository _endDayTradeTransactionsModel;
        private ISettingRepository _settingsModel;
        private IUserRepository _usersModel;
        private IApprovalHistorysRepository _approvalHistorysModel;
        private int sgdDp = 2;
        private int rateDP = 6;
		public static readonly object LockObject = new object();

		public TaskListController()
            : this(new CustomerParticularRepository(), new SaleRepository(), new RemittanceSaleRepository(), new SaleTransactionRepository(), new RemittanceOrderRepository(), new SaleTransactionDenominationRepository(), new ProductRepository(), new RemittanceProductRepository(), new ProductInventoryRepository(), new ProductDenominationRepository(), new InventoryRepository(), new TaskListRepository(), new EndDayTradeRepository(), new EndDayTradeTransactionRepository(), new SettingRepository(), new UserRepository(), new ApprovalHistorysRepository())
        {

        }

        public TaskListController(ICustomerParticularRepository customerParticularsModel, ISaleRepository salesModel, IRemittanceSaleRepository remittancesalesModel, ISaleTransactionRepository saleTransactionsModel, IRemittanceOrderRepository remittanceOrder, ISaleTransactionDenominationRepository saleTransactionDenominationsModel, IProductRepository productsModel, IRemittanceProductRepository remittanceProduct, IProductInventoryRepository productInventoriesModel, IProductDenominationRepository productDenominationsModel, IInventoryRepository inventoriesModel, ITaskListRepository taskListsModel, IEndDayTradeRepository endDayTradesModel, IEndDayTradeTransactionRepository endDayTradeTransactionsModel, ISettingRepository settingsModel, IUserRepository usersModel, IApprovalHistorysRepository approvalHistorysModel)
        {
            _customerParticularsModel = customerParticularsModel;
            _salesModel = salesModel;
            _remittancesalesModel = remittancesalesModel;
            _saleTransactionsModel = saleTransactionsModel;
            _remittanceordersModel = remittanceOrder;
            _saleTransactionDenominationsModel = saleTransactionDenominationsModel;
            _productsModel = productsModel;
            _remittanceproductsModel = remittanceProduct;
            _productInventoriesModel = productInventoriesModel;
            _productDenominationsModel = productDenominationsModel;
            _inventoriesModel = inventoriesModel;
            _taskListsModel = taskListsModel;
            _endDayTradesModel = endDayTradesModel;
            _endDayTradeTransactionsModel = endDayTradeTransactionsModel;
            _settingsModel = settingsModel;
            _usersModel = usersModel;
            _approvalHistorysModel = approvalHistorysModel;
            Product sgd = _productsModel.FindCurrencyCode("SGD");
            sgdDp = sgd.Decimal;
            ViewData["SGDDP"] = GetDecimalFormat(sgdDp);
            ViewData["RateDP"] = GetDecimalFormat(rateDP);
        }

        //// GET: TestEmail
        //public ActionResult Test()
        //{
        //    return View();
        //}

        //// POST: TestEmail
        //[HttpPost]
        //public ActionResult Test(FormCollection form)
        //{
        //    string multipleTo = null;

        //    string[] multiplearray = null;

        //    bool checkError = false;

        //    if(!string.IsNullOrEmpty(form["MultipleTo"]))
        //    {

        //        multipleTo = form["MultipleTo"].ToString();

        //        string[] splitcheck = Array.ConvertAll(multipleTo.Split(','), p => p.Trim());

        //        foreach(var splitchecks in splitcheck)
        //        {

        //            if (string.IsNullOrEmpty(splitchecks))
        //            {
        //                ModelState.AddModelError("MultipleTo", "Email cannot be null");
        //                checkError = true;
        //            }
        //            else
        //            {
        //                bool checkEmailFormat = FormValidationHelper.EmailValidation(splitchecks);

        //                if (!checkEmailFormat)
        //                {
        //                    ModelState.AddModelError("MultipleTo", "Invalid Email Format");
        //                    checkError = true;
        //                }
        //            }

        //        }

        //        if(!checkError)
        //        {
        //            multiplearray = splitcheck;
        //        }

        //    }
        //    else
        //    {
        //        ModelState.AddModelError("MultipleTo", "Email cannot blank");
        //    }

        //    if (ModelState.IsValid)
        //    {

        //        ListDictionary replacements = new ListDictionary();
        //        replacements.Add("<%Name%>", "Test Name");
        //        replacements.Add("<%MemoID%>", "Test Memo ID 001");
        //        replacements.Add("<%EmailType%>", "Receipt");
        //        replacements.Add("<%Message%>", "Test Message");
        //        replacements.Add("<%AcknowledgeMessage%>", "");

        //        string subject = "Test Multiple Email";
        //        string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/Memo.html"));

        //        string recipient = multipleTo;

        //        bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient);

        //        if (sent)
        //        {
        //            //updateResult = "{\"result\":\"success\", \"msg\":\"Sale Memo has been sent to " + customerName.Replace("\"", "\\\"") + "!\"}";
        //        }
        //        else
        //        {
        //            //updateResult = "{\"result\":\"error\", \"msg\":\"Failed to send Sale Memo!\"}";
        //        }
        //    }


        //    return View();
        //}

        // GET: TaskList
        public ActionResult Index()
        {
            if (TempData["SearchKeyword"] != null)
            {
                TempData.Remove("SearchKeyword");
            }

            return RedirectToAction("Listing");
        }

        //GET: Listing
        public ActionResult Listing(int page = 1)
        {
            //List<TaskList> taskLists = new List<TaskList>();

            ////if ops exe, then assign the user id just only view own order.
            //string isOpsExec = "No";
            //string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

            ////Sale Task List
            //List<string> status = new List<string>();
            //IList<Sale> finalSales = null;

            //if (Session["UserRole"].ToString().Contains("Finance"))
            //{
            //    status.Add("Pending Accounts");
            //}

            //if (Session["UserRole"].ToString().Contains("Inventory"))
            //{
            //    status.Add("Pending Packing");
            //}

            //if (Session["UserRole"].ToString().Contains("Ops Exec"))
            //{
            //    //status.Add("Pending Assign Delivery");
            //    status.Add("Pending Delivery by ");
            //    status.Add("Pending Incoming Delivery by ");
            //    isOpsExec = "Yes";
            //}

            //if (Session["UserRole"].ToString().Contains("Ops Manager"))
            //{
            //    status.Add("Pending Delivery by ");
            //    status.Add("Pending Incoming Delivery by ");
            //    isOpsManager = "Yes";
            //}

            //if (Session["UserRole"].ToString().Contains("Cashier"))
            //{
            //    status.Add("Pending Cashier");
            //}

            //if (Session["UserRole"].ToString().Contains("General Manager"))
            //{
            //    status.Add("Pending GM Approval");
            //    status.Add("Pending GM Approval (Rejected)");
            //    status.Add("Pending Delete GM Approval");
            //}

            ////Update Here
            ////first to check have Exe Ops Or not.
            ////if have, grab the Exec Ops data first
            ////Then loop others and add into the sales range.

            ////1. Check is Exec Ops or not.

            //IList<Sale> sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));

            //if(isOpsExec == "Yes")
            //{
            //    //sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]), isOpsExec).Where(e => status.Contains(e.Status) && e.PendingDeliveryById == Convert.ToInt32(Session["UserId"])).ToList();

            //    status.Remove("Pending Delivery by ");
            //    status.Remove("Pending Incoming Delivery by ");

            //    finalSales = sales;
            //}

            //if(isOpsManager == "Yes")
            //{
            //    status.Add("Pending Delivery by ");
            //    //sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]), isOpsExec).Where(e => status.Contains(e.Status) && e.PendingDeliveryById == Convert.ToInt32(Session["UserId"])).ToList();
            //}

            ////now this one is new status, without the ops exec
            ////sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));

            //if(sales.Count > 0)
            //{
            //    finalSales = sales.Distinct().ToList();
            //}
            //else
            //{
            //    finalSales = sales.Distinct().ToList();
            //}

            ////End Here

            //IList<TaskList> saleLists = new List<TaskList>();

            //string sgdFormat = GetDecimalFormat(sgdDp);

            ////For Collection Time Sequence
            //Dictionary<string, int> collectionTimes = new Dictionary<string, int>();
            //collectionTimes.Add("9am to 10am", 1);
            //collectionTimes.Add("10am to 12pm", 2);
            //collectionTimes.Add("2pm to 3pm", 3);
            //collectionTimes.Add("3pm to 5pm", 4);

            //foreach (Sale sale in finalSales)
            //{
            //    TaskList list = new TaskList();
            //    list.SaleTransactions = new List<SaleTransaction>();
            //    list.ID = sale.ID;
            //    list.Task = "Sale";
            //    list.ReferenceNo = sale.MemoID;
            //    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
            //    {
            //        list.Name = sale.CustomerParticulars.Company_RegisteredName;
            //    }
            //    else
            //    {
            //        list.Name = sale.CustomerParticulars.Natural_Name;
            //    }
            //    list.CollectionDate = sale.CollectionDate;
            //    list.CollectionTime = new string[] { collectionTimes[sale.CollectionTime].ToString(), sale.CollectionTime };
            //    list.TransactionType = sale.TransactionType;

            //    if(sale.SaleTransactions.Count > 0)
            //    {
            //        list.Type = sale.SaleTransactions[0].Products.CurrencyCode;
            //    }
            //    else
            //    {
            //        list.Type = "";
            //    }


            //    list.Vessel = "Vessel ABC";

            //    if(sale.SaleTransactions.Count > 0)
            //    {
            //        list.Amount = sale.SaleTransactions[0].Products.Symbol + sale.SaleTransactions[0].AmountForeign.ToString("#,##0.########");
            //    }
            //    else
            //    {
            //        list.Amount = "";
            //    }

            //    list.Status = sale.Status;
            //    list.Urgent = sale.Urgent;
            //    if (sale.Urgent == "Urgent")
            //    {
            //        list.UrgentClass = "bg-red-100";
            //    }
            //    else
            //    {
            //        list.UrgentClass = "";
            //    }
            //    list.SaleTransactions.AddRange(sale.SaleTransactions);
            //    saleLists.Add(list);
            //}

            //saleLists = _taskListsModel.Sorting(saleLists);
            //taskLists.AddRange(saleLists);

            //if (Session["UserRole"].ToString().Contains("General Manager"))
            //{
            //    //Customer Task List
            //    IList<CustomerParticular> customerParticulars = _customerParticularsModel.GetAllByStatus("Pending Approval");
            //    IList<TaskList> customerLists = new List<TaskList>();

            //    foreach (CustomerParticular customer in customerParticulars)
            //    {
            //        TaskList list = new TaskList();
            //        list.ID = customer.ID;
            //        list.Task = "Customer";
            //        list.ReferenceNo = customer.CustomerCode;
            //        if (customer.CustomerType == "Corporate & Trading Company")
            //        {
            //            list.Name = customer.Company_RegisteredName;
            //        }
            //        else
            //        {
            //            list.Name = customer.Natural_Name;
            //        }
            //        list.CollectionTime = new string[2] { "0", "-" };
            //        list.TransactionType = "-";
            //        list.Type = "-";
            //        list.Vessel = "-";
            //        list.Amount = "-";
            //        list.Status = customer.Others[0].Status;
            //        customerLists.Add(list);
            //    }

            //    customerLists = _taskListsModel.Sorting(customerLists);
            //    taskLists.AddRange(customerLists);
            //}

            //ViewData["TaskLists"] = taskLists;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //Partial View: ViewTaskList
        public ActionResult ViewTaskListPartial()
        {
            List<TaskList> taskLists = new List<TaskList>();

            //if ops exe, then assign the user id just only view own order.
            string isOpsExec = "No";
            string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

            //Sale Task List
            List<string> status = new List<string>();
            IList<Sale> finalSales = null;
            IList<Remittances> finalRemittanceSales = null;

            //New Update
            string userRole = Session["UserRole"].ToString();
            string[] userRoleList = userRole.Split(',');

            if (Array.IndexOf(userRoleList, "Finance") >= 0)
            {
                status.Add("Pending Accounts");
                status.Add("Pending Accounts (Check Funds)");
                status.Add("Pending Accounts (Check Transaction)");
            }

            if (Array.IndexOf(userRoleList, "Dealer") >= 0)
            {
                status.Add("Pending Dealer");
                status.Add("Pending Customer");
            }

            if (Array.IndexOf(userRoleList, "Inventory") >= 0)
            {
                status.Add("Pending Packing");
            }

            if (Array.IndexOf(userRoleList, "Ops Exec") >= 0)
            {
                //status.Add("Pending Assign Delivery");
                status.Add("Pending Delivery by ");
                status.Add("Pending Incoming Delivery by ");
                isOpsExec = "Yes";
            }

            if (Array.IndexOf(userRoleList, "Ops Manager") >= 0)
            {
                status.Add("Pending Delivery by ");
                status.Add("Pending Incoming Delivery by ");
                isOpsManager = "Yes";
            }

            if (Array.IndexOf(userRoleList, "Cashier") >= 0)
            {
                status.Add("Pending Cashier");
            }

            if (Array.IndexOf(userRoleList, "General Manager") >= 0)
            {
                status.Add("Pending GM Approval");
                status.Add("Pending GM Approval (Rejected)");
                status.Add("Pending Delete GM Approval");
            }

            IList<Sale> getSales = new List<Sale>();
            IList<Remittances> getRemittanceSales = new List<Remittances>();
            int userid = Convert.ToInt32(Session["UserId"].ToString());

            if (status.Count > 0)
            {
                //check is junior dealer or is Customer Viewer or not
                if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0)
                {
					if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
					{
						//this is multiple role
						getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						//if (userRoleList.Length > 2)
						//{
						//	//this is multiple role
						//	getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						//	getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						//}
					}
					else
					{
						//this is multiple role
						getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
					}
                }
                else if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
                {
					if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0)
					{
						if (userRoleList.Length > 2)
						{
							//this is multiple role
							getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
							getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						}
						else
						{
							//this is multiple role
							getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
							getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						}
					}
					else
					{
						//this is multiple role
						getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
						getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
					}
                }
                else
                {
                    //this is not junior dealer
                    getSales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
                    getRemittanceSales = _remittancesalesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));
                }

                if (isOpsExec == "Yes")
                {
                    //sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]), isOpsExec).Where(e => status.Contains(e.Status) && e.PendingDeliveryById == Convert.ToInt32(Session["UserId"])).ToList();

                    status.Remove("Pending Delivery by ");
                    status.Remove("Pending Incoming Delivery by ");

                    finalSales = getSales;
                    finalRemittanceSales = getRemittanceSales;
                }

                if (isOpsManager == "Yes")
                {
                    status.Add("Pending Delivery by ");
                    //sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]), isOpsExec).Where(e => status.Contains(e.Status) && e.PendingDeliveryById == Convert.ToInt32(Session["UserId"])).ToList();
                }

                //now this one is new status, without the ops exec
                //sales = _salesModel.GetUserSales(status, Convert.ToInt32(Session["UserId"]));

                if (getSales.Count > 0)
                {
                    finalSales = getSales.Distinct().ToList();
                }
                else
                {
                    finalSales = getSales.Distinct().ToList();

                }
                if (getRemittanceSales.Count > 0)
                {
                    finalRemittanceSales = getRemittanceSales.Distinct().ToList();
                }
                else
                {
                    if (finalRemittanceSales != null)
                    {
                        finalRemittanceSales = getRemittanceSales.Distinct().ToList();
                    }
                    else
                    {
                        finalRemittanceSales = new List<Remittances>();
                    }
                }
                //End Here

                IList<TaskList> saleLists = new List<TaskList>();
                IList<TaskList> remittancesaleLists = new List<TaskList>();

                string sgdFormat = GetDecimalFormat(sgdDp);

                //For Collection Time Sequence
                //Dictionary<string, int> collectionTimes = new Dictionary<string, int>();
                //collectionTimes.Add("9am to 10am", 1);
                //collectionTimes.Add("10am to 12pm", 2);
                //collectionTimes.Add("2pm to 3pm", 3);
                //collectionTimes.Add("3pm to 5pm", 4);

                foreach (Sale sale in finalSales)
                {
                    TaskList list = new TaskList();
                    list.SaleTransactions = new List<SaleTransaction>();
                    list.ID = sale.ID;
                    list.Task = "Sale";
                    list.ReferenceNo = sale.MemoID;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        list.Name = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        list.Name = sale.CustomerParticulars.Natural_Name;
                    }

                    list.CollectionDate = sale.CollectionDate;
                    list.CollectionTime = sale.CollectionTime;//new string[] { collectionTimes[sale.CollectionTime].ToString(), sale.CollectionTime };

					list.TransactionType = sale.TransactionType;

                    if (sale.SaleTransactions.Count > 0)
                    {
                        list.Type = sale.SaleTransactions[0].Products.CurrencyCode;
                    }
                    else
                    {
                        list.Type = "";
                    }


                    list.Vessel = "Vessel ABC";

                    if (sale.SaleTransactions.Count > 0)
                    {
                        list.Amount = sale.SaleTransactions[0].Products.Symbol + sale.SaleTransactions[0].AmountForeign.ToString("#,##0.########");
                    }
                    else
                    {
                        list.Amount = "";
                    }

                    list.Status = sale.Status;
                    list.Urgent = sale.Urgent;
                    if (sale.Urgent == "Urgent")
                    {
                        list.UrgentClass = "bg-red-100";
                    }
                    else
                    {
                        list.UrgentClass = "";
                    }
                    list.SaleTransactions.AddRange(sale.SaleTransactions);
                    saleLists.Add(list);
                }
                foreach (Remittances sale in finalRemittanceSales)
                {
                    TaskList list = new TaskList();
                    list.RemittanceOrders = new List<RemittanceOrders>();
                    list.ID = sale.ID;
                    list.Task = "RemittanceSale";
                    list.ReferenceNo = sale.MemoID;

                    if (sale.CustomerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        list.Name = sale.CustomerParticulars.Company_RegisteredName;
                    }
                    else
                    {
                        list.Name = sale.CustomerParticulars.Natural_Name;
                    }

                    //list.CollectionDate = sale.CollectionDate;
                    //list.CollectionTime = new string[] { collectionTimes[sale.CollectionTime].ToString(), sale.CollectionTime };
                    list.TransactionType = "Remittance";

                    if (sale.RemittanceOders.Count > 0)
                    {
                        var currencyCode = _remittanceproductsModel.GetSingle(sale.RemittanceOders[0].PayCurrency).CurrencyCode;
                        list.Type = currencyCode;
                    }
                    else
                    {
                        list.Type = "";
                    }


                    list.Vessel = "";

                    if (sale.RemittanceOders.Count > 0)
                    {
                        var getSymbol = _remittanceproductsModel.GetSingle(sale.RemittanceOders[0].PayCurrency).ProductSymbol;
                        list.Amount = getSymbol + sale.TotalPayAmount.ToString("#,##0.########");
                    }
                    else
                    {
                        list.Amount = "";
                    }

                    list.Status = sale.Status;

                    if (sale.IsUrgent == 1)
                    {
                        list.Urgent = "Urgent";
                        list.UrgentClass = "bg-red-100";
                    }
                    else
                    {
                        list.Urgent = "Non-Urgent";
                        list.UrgentClass = "";
                    }
                    list.RemittanceOrders.AddRange(sale.RemittanceOders);
                    remittancesaleLists.Add(list);
                }

                saleLists = _taskListsModel.Sorting(saleLists);
                remittancesaleLists = _taskListsModel.Sorting2(remittancesaleLists);
                taskLists.AddRange(saleLists);
                taskLists.AddRange(remittancesaleLists);

                if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                {
                    //Customer Task List
                    IList<CustomerParticular> customerParticulars = _customerParticularsModel.GetAllByStatus("Pending Approval");
                    IList<TaskList> customerLists = new List<TaskList>();

                    if (customerParticulars.Count > 0)
                    {
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            foreach (CustomerParticular customer in customerParticulars)
                            {
                                TaskList list = new TaskList();

								var getTempData = context.Temp_CustomerParticulars.Where(e => e.Customer_MainID == customer.ID).FirstOrDefault();

								if (getTempData != null)
								{
									if (customer.CustomerType == "Corporate & Trading Company")
									{
										list.Name = getTempData.Company_RegisteredName;
									}
									else
									{
										list.Name = getTempData.Natural_Name;
									}
								}
								else
								{
									if (customer.CustomerType == "Corporate & Trading Company")
									{
										list.Name = customer.Company_RegisteredName;
									}
									else
									{
										list.Name = customer.Natural_Name;
									}
								}

								list.ID = customer.ID;
                                list.Task = "Customer";
                                list.ReferenceNo = customer.CustomerCode;
								//if (customer.CustomerType == "Corporate & Trading Company")
								//{
								//	list.Name = customer.Company_RegisteredName;
								//}
								//else
								//{
								//	list.Name = customer.Natural_Name;
								//}
								list.CollectionTime = "-";
                                list.TransactionType = "-";
                                list.Type = "-";
                                list.Vessel = "-";
                                list.Amount = "-";
                                list.Status = customer.Others[0].Status;
                                customerLists.Add(list);
                            }
                        }
                    }

                    customerLists = _taskListsModel.Sorting(customerLists);
                    taskLists.AddRange(customerLists);
                }
            }

            //End Update


            //Update Here
            //first to check have Exe Ops Or not.
            //if have, grab the Exec Ops data first
            //Then loop others and add into the sales range.

            //1. Check is Exec Ops or not.
            ViewData["TaskLists"] = taskLists;
            return View();
        }

        //GET: UpdateSale
        public ActionResult UpdateSale(int id)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            string sgdFormat = GetDecimalFormat(sgdDp);
            string rateFormat = GetDecimalFormat(rateDP);

            if (sales != null)
            {
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;
                ViewData["TransactionType"] = sales.TransactionType;
                ViewData["SaleRemarks"] = "";
                if (!string.IsNullOrEmpty(sales.Remarks))
                {
                    ViewData["SaleRemarks"] = sales.Remarks;
                }
                string userRole = Session["UserRole"].ToString();
                string[] userRoleList = userRole.Split(',');

                if (sales.Status == "Pending Accounts")
                {
                    //if (Array.IndexOf(userRoleList, "Finance") >= 0)
                    if (Array.IndexOf(userRoleList, "Finance") >= 0)
                    {
                        ViewData["CollectionDate"] = Convert.ToDateTime(sales.CollectionDate).ToString("dd/MM/yyyy dddd");

						//Dropdown[] collectionTimeDDL = CollectionTimeDDL();
						//ViewData["CollectionTimeDropdown"] = new SelectList(collectionTimeDDL, "val", "name", sales.CollectionTime);
						ViewData["CollectionTime"] = "";

						if (!string.IsNullOrEmpty(sales.CollectionTime))
						{
							ViewData["CollectionTime"] = sales.CollectionTime;
						}

						ViewData["BankReferenceNumber"] = "";
                        if (!string.IsNullOrEmpty(sales.BankTransferNo))
                        {
                            ViewData["BankReferenceNumber"] = sales.BankTransferNo;
                        }

                        if (!string.IsNullOrEmpty(sales.LocalPaymentMode))
                        {
                            if (sales.LocalPaymentMode.Contains("Bank Transfer"))
                            {
                                ViewData["BankTransfer"] = true;
                            }
                        }

                        //ViewData["Remarks"] = !string.IsNullOrEmpty(sales.Remarks) ? sales.Remarks : "";

                        return View("PendingAccounts");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending Packing")
                {
                    //if (Array.IndexOf(userRoleList, "Inventory") >= 0)
                    if (Array.IndexOf(userRoleList, "Inventory") >= 0)
                    {
                        ViewData["CollectionDate"] = Convert.ToDateTime(sales.CollectionDate).ToString("dd/MM/yyyy dddd");

						//Dropdown[] collectionTimeDDL = CollectionTimeDDL();
						//ViewData["CollectionTimeDropdown"] = new SelectList(collectionTimeDDL, "val", "name", sales.CollectionTime);
						ViewData["CollectionTime"] = "";
						if (!string.IsNullOrEmpty(sales.CollectionTime))
						{
							ViewData["CollectionTime"] = sales.CollectionTime;
						}

						ViewData["BagNumber"] = "";
                        if (!string.IsNullOrEmpty(sales.BagNo))
                        {
                            ViewData["BagNumber"] = sales.BagNo;
                        }
                        ViewData["BankReferenceNumber"] = "";
                        if (!string.IsNullOrEmpty(sales.BankTransferNo))
                        {
                            ViewData["BankReferenceNumber"] = sales.BankTransferNo;
                        }
                        if (!string.IsNullOrEmpty(sales.LocalPaymentMode))
                        {
                            if (sales.LocalPaymentMode.Contains("Bank Transfer"))
                            {
                                ViewData["BankTransfer"] = true;
                            }
                        }
                        ViewData["CustomerRemarks"] = "";
                        if (!string.IsNullOrEmpty(sales.CustomerRemarks))
                        {
                            ViewData["CustomerRemarks"] = sales.CustomerRemarks;
                        }

                        ViewData["SaleRemarks"] = "";
                        if (!string.IsNullOrEmpty(sales.Remarks))
                        {
                            ViewData["SaleRemarks"] = sales.Remarks;
                        }

                        ViewData["SaleTransactionsData"] = sales.SaleTransactions;

                        ViewData["SaleTransactionType"] = sales.TransactionType;

                        return View("PendingPacking");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending Assign Delivery")
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                    TempData.Add("Result", "danger|Memo ID not ready!");
                }
                else if (sales.Status.Contains("Pending Delivery by "))
                {
                    //if (Array.IndexOf(userRoleList, "Ops Exec") >= 0)
                    if (Array.IndexOf(userRoleList, "Ops Exec") >= 0 && sales.PendingDeliveryById == Convert.ToInt32(Session["UserId"]))
                    {
                        return View("PendingDelivery");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status.Contains("Pending Incoming Delivery by "))
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                    TempData.Add("Result", "danger|Memo ID not ready!");
                }
                else if (sales.Status == "Pending Cashier")
                {

                    //if (Array.IndexOf(userRoleList, "Cashier") >= 0)
                    if (Array.IndexOf(userRoleList, "Cashier") >= 0)
                    {
                        ViewData["DisabledCheckbox"] = "";

                        ViewData["LocalPaymentModeCashCheckbox"] = "";
                        ViewData["LocalPaymentModeCheque1Checkbox"] = "";
                        ViewData["LocalPaymentModeCheque2Checkbox"] = "";
                        ViewData["LocalPaymentModeCheque3Checkbox"] = "";
                        ViewData["LocalPaymentModeBankTransferCheckbox"] = "";
                        ViewData["PendingChequeLog"] = 0;
                        ViewData["PendingBankTransferLog"] = 0;

                        ViewData["PendingLocalCheque1Log"] = 0;
                        ViewData["PendingLocalCheque2Log"] = 0;
                        ViewData["PendingLocalCheque3Log"] = 0;
                        ViewData["PendingLocalBankTransferLog"] = 0;

                        ViewData["TotalLocalPaymentAmount"] = "";
                        decimal totalAmount = 0;

                        if (sales.TransactionType == "Buy" || sales.TransactionType == "Sell")
                        {
                            if (!string.IsNullOrEmpty(sales.LocalPaymentMode))
                            {
                                if (sales.LocalPaymentMode.Contains("Cash"))
                                {
                                    ViewData["LocalPaymentModeCashCheckbox"] = "checked";
                                    totalAmount += Convert.ToDecimal(sales.CashAmount);
                                }

                                if (sales.LocalPaymentMode.Contains("Cheque 1"))
                                {
                                    ViewData["LocalPaymentModeCheque1Checkbox"] = "checked";
                                    totalAmount += Convert.ToDecimal(sales.Cheque1Amount);
                                }

                                if (sales.LocalPaymentMode.Contains("Cheque 2"))
                                {
                                    ViewData["LocalPaymentModeCheque2Checkbox"] = "checked";
                                    totalAmount += Convert.ToDecimal(sales.Cheque2Amount);
                                }

                                if (sales.LocalPaymentMode.Contains("Cheque 3"))
                                {
                                    ViewData["LocalPaymentModeCheque3Checkbox"] = "checked";
                                    totalAmount += Convert.ToDecimal(sales.Cheque3Amount);
                                }

                                if (sales.LocalPaymentMode.Contains("Bank Transfer"))
                                {
                                    ViewData["LocalPaymentModeBankTransferCheckbox"] = "checked";
                                    totalAmount += Convert.ToDecimal(sales.BankTransferAmount);
                                }

                                ViewData["TotalLocalPaymentAmount"] = totalAmount.ToString(sgdFormat);
                            }
                        }
                        else
                        {
                            ViewData["DisabledCheckbox"] = "disabled";
                        }

                        ViewData["CashAmount"] = "";
                        if (sales.CashAmount != null)
                        {
                            ViewData["CashAmount"] = Convert.ToDecimal(sales.CashAmount).ToString(sgdFormat);
                        }

                        ViewData["Cheque1No"] = "";
                        if (!string.IsNullOrEmpty(sales.Cheque1No))
                        {
                            ViewData["Cheque1No"] = sales.Cheque1No;
                        }

                        ViewData["Cheque2No"] = "";
                        if (!string.IsNullOrEmpty(sales.Cheque2No))
                        {
                            ViewData["Cheque2No"] = sales.Cheque2No;
                        }

                        ViewData["Cheque3No"] = "";
                        if (!string.IsNullOrEmpty(sales.Cheque3No))
                        {
                            ViewData["Cheque3No"] = sales.Cheque3No;
                        }

                        ViewData["Cheque1Amount"] = "";
                        if (sales.Cheque1Amount != null)
                        {
                            ViewData["Cheque1Amount"] = Convert.ToDecimal(sales.Cheque1Amount).ToString(sgdFormat);
                        }

                        ViewData["Cheque2Amount"] = "";
                        if (sales.Cheque2Amount != null)
                        {
                            ViewData["Cheque2Amount"] = Convert.ToDecimal(sales.Cheque2Amount).ToString(sgdFormat);
                        }

                        ViewData["Cheque3Amount"] = "";
                        if (sales.Cheque3Amount != null)
                        {
                            ViewData["Cheque3Amount"] = Convert.ToDecimal(sales.Cheque3Amount).ToString(sgdFormat);
                        }

                        ViewData["BankTransferNo"] = "";
                        if (!string.IsNullOrEmpty(sales.BankTransferNo))
                        {
                            ViewData["BankTransferNo"] = sales.BankTransferNo;
                        }

                        ViewData["BankTranferAmount"] = "";
                        if (sales.BankTransferAmount != null)
                        {
                            ViewData["BankTranferAmount"] = Convert.ToDecimal(sales.BankTransferAmount).ToString(sgdFormat);
                        }

                        Dropdown[] cashBankDDL = LocalPaymentBankDDL(sales.CashBank);
                        ViewData["CashBankDropdown"] = new SelectList(cashBankDDL, "val", "name", sales.CashBank);

                        Dropdown[] cheque1BankDDL = LocalPaymentBankDDL(sales.Cheque1Bank);
                        ViewData["Cheque1BankDropdown"] = new SelectList(cheque1BankDDL, "val", "name", sales.Cheque1Bank);

                        Dropdown[] cheque2BankDDL = LocalPaymentBankDDL(sales.Cheque2Bank);
                        ViewData["Cheque2BankDropdown"] = new SelectList(cheque2BankDDL, "val", "name", sales.Cheque2Bank);

                        Dropdown[] cheque3BankDDL = LocalPaymentBankDDL(sales.Cheque3Bank);
                        ViewData["Cheque3BankDropdown"] = new SelectList(cheque3BankDDL, "val", "name", sales.Cheque3Bank);

                        Dropdown[] bankTransferBankDDL = LocalPaymentBankDDL(sales.BankTransferBank);
                        ViewData["BankTransferBankDropdown"] = new SelectList(bankTransferBankDDL, "val", "name", sales.BankTransferBank);

                        ViewData["MemoBalance"] = "";
                        if (sales.MemoBalance != null)
                        {
                            ViewData["MemoBalance"] = Convert.ToDecimal(sales.MemoBalance).ToString(sgdFormat);
                        }

                        return View("PendingCashier");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending GM Approval (Rejected)")
                {
                    //if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        ViewData["Sales"] = sales;

                        return View("Disapproved");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending Delete GM Approval")
                {
                    //if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        ViewData["Sales"] = sales;

                        return View("GMDeleteSale");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                    TempData.Add("Result", "danger|Memo ID not ready!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //POST: AddProductDenominations
        [HttpPost]
        public string AddProductDenominations(int tid, int pid)
        {
            string result = "";

            try
            {
                SaleTransaction transaction = _saleTransactionsModel.GetSingle(tid);

                if (transaction != null)
                {
                    Sale sales = _salesModel.GetSingle(transaction.SaleId);

                    if (sales != null)
                    {
                        if (transaction.CurrencyId == pid)
                        {
                            if ((sales.TransactionType == "Encashment" && transaction.TransactionType == "Sell") || (sales.TransactionType == "Deposit" && transaction.TransactionType == "Buy") || sales.TransactionType == "Buy" || sales.TransactionType == "Sell" || sales.TransactionType == "Cross Currency")
                            {
                                if (transaction.SaleTransactionDenominations.Count == 0)
                                {
                                    IList<ProductDenomination> productDenos = _productDenominationsModel.GetProductDenomination(transaction.CurrencyId);

                                    transaction.SaleTransactionDenominations = productDenos.Select(e => new SaleTransactionDenomination
                                    {
                                        ID = e.ID * -1,
                                        SaleTransactionId = transaction.ID,
                                        Denomination = e.DenominationValue,
                                        Pieces = 0,
                                        AmountForeign = 0
                                    }).ToList();
                                }
                            }

                            ViewData["SaleTransaction"] = transaction;

                            string view = "";

                            using (var sw = new StringWriter())
                            {
                                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "AddProductDenominations");
                                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                                viewResult.View.Render(viewContext, sw);
                                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                                view = sw.GetStringBuilder().ToString();
                            }

                            view = JsonConvert.SerializeObject(view.Replace("\r\n", ""));

                            result = "{\"Result\":true,\"View\":" + view + "}";
                        }
                        else
                        {
                            result = "{\"Result\":false,\"ErrorMessage\":\"Sale Transaction record not found!\"}";
                        }
                    }
                    else
                    {
                        result = "{\"Result\":false,\"ErrorMessage\":\"Sale record not found!\"}";
                    }
                }
                else
                {
                    result = "{\"Result\":false,\"ErrorMessage\":\"Sale Transaction record not found!\"}";
                }
            }
            catch (Exception e)
            {
                result = "{\"Result\":false,\"ErrorMessage\":\"" + e.Message + "\"}";
            }

            return result;
        }

        //POST: PendingAccounts
        [HttpPost]
        public ActionResult PendingAccounts(int id, FormCollection form)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending Accounts")
                {
                    List<string[]> modelErrors = new List<string[]>();

                    if (string.IsNullOrEmpty(form["CollectionDate"]))
                    {
                        string[] error = new string[2];
                        error[0] = "CollectionDate";
                        error[1] = "Collection Date is required!";
                        modelErrors.Add(error);
                    }
                    else
                    {
                        try
                        {
                            sales.CollectionDate = Convert.ToDateTime(form["CollectionDate"]);
                        }
                        catch
                        {
                            string[] error = new string[2];
                            error[0] = "CollectionDate";
                            error[1] = "Collection Date is not valid!";
                            modelErrors.Add(error);
                        }
                    }

					if (string.IsNullOrEmpty(form["CollectionTime"]))
					{
						string[] error = new string[2];
						error[0] = "CollectionTime";
						error[1] = "Collection Time is required.";
						modelErrors.Add(error);
					}
					else
					{
						if (form["CollectionTime"].ToString().Length > 40)
						{
							string[] error = new string[2];
							error[0] = "CollectionTime";
							error[1] = "Collection Time cannot more than 40 characters.";
							modelErrors.Add(error);
						}
					}

                    if (!string.IsNullOrEmpty(form["Bank_Reference_Number"]))
                    {
                        sales.BankTransferNo = form["Bank_Reference_Number"].ToString();
                    }

                    if (modelErrors.Count == 0)
                    {
                        sales.CollectionTime = form["CollectionTime"];
                        sales.Remarks = form["Remarks"];
                        sales.Status = "Pending Packing";
                        sales.LastApprovalOn = DateTime.Now;

                        bool result = _salesModel.Update(sales.ID, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //POST: PendingPacking
        [HttpPost]
        public ActionResult PendingPacking(int id, FormCollection form)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending Packing")
                {
                    List<string> denominationKeys = form.AllKeys.Where(e => e.Contains("Denomination_Value_Denomination_")).ToList();

                    List<string[]> modelErrors = new List<string[]>();

                    if (string.IsNullOrEmpty(form["CollectionDate"]))
                    {
                        string[] error = new string[2];
                        error[0] = "CollectionDate";
                        error[1] = "Collection Date is required!";
                        modelErrors.Add(error);
                    }
                    else
                    {
                        try
                        {
                            sales.CollectionDate = Convert.ToDateTime(form["CollectionDate"]);
                        }
                        catch
                        {
                            string[] error = new string[2];
                            error[0] = "CollectionDate";
                            error[1] = "Collection Date is not valid!";
                            modelErrors.Add(error);
                        }
                    }

					if (string.IsNullOrEmpty(form["CollectionTime"]))
					{
						string[] error = new string[2];
						error[0] = "CollectionTime";
						error[1] = "Collection Time is required.";
						modelErrors.Add(error);
					}
					else
					{
						if (form["CollectionTime"].ToString().Length > 40)
						{
							string[] error = new string[2];
							error[0] = "CollectionTime";
							error[1] = "Collection Time cannot more than 40 characters.";
							modelErrors.Add(error);
						}
					}

                    foreach (string denoKey in denominationKeys)
                    {
                        string[] key = denoKey.Split('_');
                        string rowId = key[3];
                        string denoId = key[4];

                        if (string.IsNullOrEmpty(form[denoKey]))
                        {
                            string[] error = new string[2];
                            error[0] = denoKey;
                            error[1] = "Denomination Value is required!";
                            modelErrors.Add(error);
                        }

                        if (!string.IsNullOrEmpty(form["Denomination_Value_Pieces_" + rowId + "_" + denoId]))
                        {
                            bool checkPieces = FormValidationHelper.IntegerValidation(form["Denomination_Value_Pieces_" + rowId + "_" + denoId].ToString().Replace(",", ""));

                            if (!checkPieces)
                            {
                                string[] error = new string[2];
                                error[0] = "Denomination_Value_Pieces_" + rowId + "_" + denoId;
                                error[1] = "Pieces is not valid!";
                                modelErrors.Add(error);
                            }
                        }

                        if (!string.IsNullOrEmpty(form["Denomination_Value_AmountForeign_" + rowId + "_" + denoId]))
                        {
                            bool checkAmountForeign = FormValidationHelper.NonNegativeAmountValidation(form["Denomination_Value_AmountForeign_" + rowId + "_" + denoId].ToString());

                            if (!checkAmountForeign)
                            {
                                string[] error = new string[2];
                                error[0] = "Denomination_Value_AmountForeign_" + rowId + "_" + denoId;
                                error[1] = "Amount (Foreign) is not valid!";
                                modelErrors.Add(error);
                            }
                        }
                    }

                    if (denominationKeys.Count > 0)
                    {
                        List<string> totalAmountKeys = form.AllKeys.Where(e => e.Contains("Denomination_Total_Calculated_Foreign_")).ToList();

                        foreach (string totalKey in totalAmountKeys)
                        {
                            string[] key = totalKey.Split('_');
                            string rowId = key[4];

                            List<string> hasDenominations = form.AllKeys.Where(e => e.Contains("Denomination_Value_Denomination_" + rowId + "_")).ToList();

                            if (hasDenominations.Count > 0)
                            {
                                decimal totalCalculatedForeign = Convert.ToDecimal(form["Denomination_Total_Calculated_Foreign_" + rowId]);
                                //decimal totalCalculatedLocal = Convert.ToDecimal(form["Denomination_Total_Calculated_Local_" + rowId]);
                                decimal totalOrderForeign = Convert.ToDecimal(form["Denomination_Total_Order_Foreign_" + rowId]);
                                //decimal totalOrderLocal = Convert.ToDecimal(form["Denomination_Total_Order_Local_" + rowId]);

                                if (Math.Abs(totalCalculatedForeign - totalOrderForeign) > Convert.ToDecimal(0.01))
                                {
                                    string[] error = new string[2];
                                    error[0] = "Denomination_Total_Calculated_Foreign_" + rowId;
                                    error[1] = "Total Calculated Amount (Foreign) and Total Order Amount (Foreign) not equal!";
                                    modelErrors.Add(error);
                                }

                                //if (Math.Abs(totalCalculatedLocal - totalOrderLocal) > Convert.ToDecimal(0.01))
                                //{
                                //    string[] error = new string[2];
                                //    error[0] = "Denomination_Total_Calculated_Local_" + rowId;
                                //    error[1] = "Total Calculated Amount (Local) and Total Order Amount (Local) not equal!";
                                //    modelErrors.Add(error);
                                //}
                            }
                        }
                    }

                    if (modelErrors.Count == 0)
                    {
                        sales.CollectionTime = form["CollectionTime"];

                        if (!string.IsNullOrEmpty(form["Bag_Number"]))
                        {
                            sales.BagNo = form["Bag_Number"].ToString();
                        }

                        if (!string.IsNullOrEmpty(form["Bank_Reference_Number"]))
                        {
                            sales.BankTransferNo = form["Bank_Reference_Number"].ToString();
                        }

                        sales.Remarks = form["Sale_Remarks"];

                        if (sales.RequireDelivery == "Yes")
                        {
                            sales.Status = "Pending Assign Delivery";
                        }
                        else
                        {
                            sales.Status = "Pending Cashier";
                        }
                        sales.LastApprovalOn = DateTime.Now;

                        bool result = _salesModel.Update(id, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]");
                            }

                            bool del_denomination = false;
                            bool add_denomination = false;

                            foreach (SaleTransaction transaction in sales.SaleTransactions.OrderBy(e => e.ID))
                            {
                                //Remove old transaction denominations
                                if (transaction.SaleTransactionDenominations.Count > 0)
                                {
                                    foreach (SaleTransactionDenomination denomination in transaction.SaleTransactionDenominations)
                                    {
                                        bool delete_denomination = _saleTransactionDenominationsModel.Delete(denomination.ID);

                                        if (delete_denomination)
                                        {
                                            if (!del_denomination)
                                            {
                                                del_denomination = true;
                                            }
                                        }
                                    }
                                }

                                //Add new transaction denomination
                                if (denominationKeys.Count > 0)
                                {
                                    foreach (string denoKey in denominationKeys)
                                    {
                                        string[] keys = denoKey.Split('_');
                                        string rowId = keys[3];
                                        string denoId = keys[4];
                                        if (Convert.ToInt32(rowId) == transaction.ID)
                                        {
                                            SaleTransactionDenomination denomination = new SaleTransactionDenomination();
                                            denomination.SaleTransactionId = transaction.ID;
                                            denomination.Denomination = Convert.ToInt32(form[denoKey]);
                                            denomination.Pieces = 0;
                                            if (!string.IsNullOrEmpty(form["Denomination_Value_Pieces_" + rowId + "_" + denoId]))
                                            {
                                                denomination.Pieces = Convert.ToInt32(form["Denomination_Value_Pieces_" + rowId + "_" + denoId].ToString().Replace(",", ""));
                                            }
                                            denomination.AmountForeign = 0;
                                            if (!string.IsNullOrEmpty(form["Denomination_Value_AmountForeign_" + rowId + "_" + denoId]))
                                            {
                                                denomination.AmountForeign = Convert.ToDecimal(form["Denomination_Value_AmountForeign_" + rowId + "_" + denoId]);
                                            }

                                            bool result_denomination = _saleTransactionDenominationsModel.Add(denomination);

                                            if (result_denomination)
                                            {
                                                if (!add_denomination)
                                                {
                                                    add_denomination = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (del_denomination)
                            {
                                userid = Convert.ToInt32(Session["UserId"]);
                                tableAffected = "SaleTransactionDenominations";
                                description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted Sale Transaction Denominations";

                                bool denomination_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                            }

                            if (add_denomination)
                            {
                                userid = Convert.ToInt32(Session["UserId"]);
                                tableAffected = "SaleTransactionDenominations";
                                description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Sale Transaction Denominations";

                                bool denomination_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //POST: PendingDelivery
        [HttpPost]
        public ActionResult PendingDelivery(int id, FormCollection form)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status.Contains("Pending Delivery by "))
                {
                    List<string[]> modelErrors = new List<string[]>();

                    if (string.IsNullOrEmpty(form["DeliveryConfirmation"]))
                    {
                        string[] error = new string[2];
                        error[0] = "DeliveryConfirmation";
                        error[1] = "Upload Confirmation is required!";
                        modelErrors.Add(error);
                    }

                    if (modelErrors.Count == 0)
                    {
                        sales.DeliveryConfirmation = form["DeliveryConfirmation"].ToString();

                        int pendingDeliveryID = 0;

                        if (sales.PendingDeliveryById != null)
                        {
                            pendingDeliveryID = Convert.ToInt32(sales.PendingDeliveryById);
                        }

                        User getPendingDeliveryID = _usersModel.GetSingle(pendingDeliveryID);

                        if (getPendingDeliveryID != null)
                        {
                            sales.Status = "Pending Incoming Delivery by " + getPendingDeliveryID.Name;
                        }
                        else
                        {
                            sales.Status = "Pending Incoming Delivery by ";
                        }

                        sales.LastApprovalOn = DateTime.Now;

                        bool result = _salesModel.Update(sales.ID, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]");
                            }

                            //Move Upload Confirmation Files
                            string[] confirmationFiles = sales.DeliveryConfirmation.Split(',');

                            foreach (string file in confirmationFiles)
                            {
                                string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["DeliveryConfirmationFolder"].ToString()), file);

                                System.IO.File.Move(sourceFile, destinationFile);
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //POST: PendingCashier
        [HttpPost]
        public ActionResult PendingCashier(int id, FormCollection form)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                ViewData["PendingChequeLog"] = form["PendingChequeLog"].ToString();
                ViewData["PendingBankTransferLog"] = form["PendingBankTransferLog"].ToString();

                ViewData["PendingLocalCheque1Log"] = form["PendingLocalCheque1Log"].ToString();
                ViewData["PendingLocalCheque2Log"] = form["PendingLocalCheque2Log"].ToString();
                ViewData["PendingLocalCheque3Log"] = form["PendingLocalCheque3Log"].ToString();
                ViewData["PendingLocalBankTransferLog"] = form["PendingLocalBankTransferLog"].ToString();


                if (sales.Status == "Pending Cashier")
                {
                    List<string[]> modelErrors = new List<string[]>();

                    if (sales.TransactionType == "Buy" || sales.TransactionType == "Sell")
                    {
                        decimal totalAmount = sales.TotalAmountLocal;

                        if (string.IsNullOrEmpty(form["sales.LocalPaymentMode"]))
                        {
                            string[] error = new string[2];
                            error[0] = "LocalPaymentMode";
                            error[1] = "Payment Mode is required!";
                            modelErrors.Add(error);
                        }
                        else
                        {
                            string[] paymentModes = form["sales.LocalPaymentMode"].ToString().Split(',');

                            if (paymentModes.Length > 4)
                            {
                                string[] error = new string[2];
                                error[0] = "LocalPaymentMode";
                                error[1] = "Maximum FOUR Payment Modes only!";
                                modelErrors.Add(error);
                            }

                            totalAmount = 0;

                            if (form["sales.LocalPaymentMode"].ToString().Contains("Cash"))
                            {
                                if (string.IsNullOrEmpty(form["CashAmount"]))
                                {
                                    string[] error = new string[2];
                                    error[0] = "CashAmount";
                                    error[1] = "Cash Amount is required!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CashAmount"]);

                                    if (!checkAmount)
                                    {
                                        string[] error = new string[2];
                                        error[0] = "CashAmount";
                                        error[1] = "Cash Amount is not valid!";
                                        modelErrors.Add(error);
                                    }
                                    else
                                    {
                                        totalAmount += Convert.ToDecimal(form["CashAmount"]);
                                    }
                                }
                            }

                            if (form["sales.LocalPaymentMode"].ToString().Contains("Cheque 1"))
                            {
                                if (string.IsNullOrEmpty(form["Cheque1Amount"]))
                                {
                                    string[] error = new string[2];
                                    error[0] = "Cheque1Amount";
                                    error[1] = "Cheque 1 Amount is required!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["Cheque1Amount"]);

                                    if (!checkAmount)
                                    {
                                        string[] error = new string[2];
                                        error[0] = "Cheque1Amount";
                                        error[1] = "Cheque 1 Amount is not valid!";
                                        modelErrors.Add(error);
                                    }
                                    else
                                    {
                                        totalAmount += Convert.ToDecimal(form["Cheque1Amount"]);
                                    }
                                }
                            }

                            if (form["sales.LocalPaymentMode"].ToString().Contains("Cheque 2"))
                            {
                                if (string.IsNullOrEmpty(form["Cheque2Amount"]))
                                {
                                    string[] error = new string[2];
                                    error[0] = "Cheque2Amount";
                                    error[1] = "Cheque 2 Amount is required!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["Cheque2Amount"]);

                                    if (!checkAmount)
                                    {
                                        string[] error = new string[2];
                                        error[0] = "Cheque2Amount";
                                        error[1] = "Cheque 2 Amount is not valid!";
                                        modelErrors.Add(error);
                                    }
                                    else
                                    {
                                        totalAmount += Convert.ToDecimal(form["Cheque2Amount"]);
                                    }
                                }
                            }

                            if (form["sales.LocalPaymentMode"].ToString().Contains("Cheque 3"))
                            {
                                if (string.IsNullOrEmpty(form["Cheque3Amount"]))
                                {
                                    string[] error = new string[2];
                                    error[0] = "Cheque3Amount";
                                    error[1] = "Cheque 3 Amount is required!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["Cheque3Amount"]);

                                    if (!checkAmount)
                                    {
                                        string[] error = new string[2];
                                        error[0] = "Cheque3Amount";
                                        error[1] = "Cheque 3 Amount is not valid!";
                                        modelErrors.Add(error);
                                    }
                                    else
                                    {
                                        totalAmount += Convert.ToDecimal(form["Cheque3Amount"]);
                                    }
                                }
                            }

                            if (form["sales.LocalPaymentMode"].ToString().Contains("Bank Transfer"))
                            {
                                if (string.IsNullOrEmpty(form["BankTranferAmount"]))
                                {
                                    string[] error = new string[2];
                                    error[0] = "BankTranferAmount";
                                    error[1] = "Bank Tranfer Amount is required!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["BankTranferAmount"]);

                                    if (!checkAmount)
                                    {
                                        string[] error = new string[2];
                                        error[0] = "BankTranferAmount";
                                        error[1] = "Bank Tranfer Amount is not valid!";
                                        modelErrors.Add(error);
                                    }
                                    else
                                    {
                                        totalAmount += Convert.ToDecimal(form["BankTranferAmount"]);
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(form["MemoBalance"]))
                            {
                                string[] error = new string[2];
                                error[0] = "MemoBalance";
                                error[1] = "Memo Balance is required!";
                                modelErrors.Add(error);
                            }
                            else
                            {
                                bool checkAmount = FormValidationHelper.AmountValidation(form["MemoBalance"]);

                                if (!checkAmount)
                                {
                                    string[] error = new string[2];
                                    error[0] = "MemoBalance";
                                    error[1] = "Memo Balance is not valid!";
                                    modelErrors.Add(error);
                                }
                                else
                                {
                                    totalAmount += Convert.ToDecimal(form["MemoBalance"]);
                                }
                            }

                            if (totalAmount != sales.TotalAmountLocal)
                            {
                                string[] error = new string[2];
                                error[0] = "LocalPaymentMode";
                                error[1] = "Local Payment Mode Amount and Total Amount (Local) not equal!";
                                modelErrors.Add(error);
                            }
                        }
                    }
                    else
                    {
                        sales.LocalPaymentMode = null;
                        sales.CashAmount = null;
                        sales.Cheque1Amount = null;
                        sales.Cheque1No = null;
                        sales.Cheque2Amount = null;
                        sales.Cheque2No = null;
                        sales.Cheque3Amount = null;
                        sales.Cheque3No = null;
                        sales.BankTransferAmount = null;
                        sales.BankTransferNo = null;
                        sales.MemoBalance = null;
                    }

					lock (LockObject)
					{
						if (modelErrors.Count == 0)
						{
							sales.LocalPaymentMode = null;
							if (!string.IsNullOrEmpty(form["sales.LocalPaymentMode"]))
							{
								sales.LocalPaymentMode = form["sales.LocalPaymentMode"].ToString();
							}

							sales.CashAmount = null;
							if (!string.IsNullOrEmpty(form["CashAmount"]))
							{
								sales.CashAmount = Convert.ToDecimal(form["CashAmount"]);
							}

							sales.Cheque1No = null;
							if (!string.IsNullOrEmpty(form["Cheque1No"]))
							{
								sales.Cheque1No = form["Cheque1No"].ToString();
								ViewData["PendingLocalCheque1Log"] = "1";
							}

							sales.Cheque2No = null;
							if (!string.IsNullOrEmpty(form["Cheque2No"]))
							{
								sales.Cheque2No = form["Cheque2No"].ToString();
								ViewData["PendingLocalCheque2Log"] = "1";
							}

							sales.Cheque3No = null;
							if (!string.IsNullOrEmpty(form["Cheque3No"]))
							{
								sales.Cheque3No = form["Cheque3No"].ToString();
								ViewData["PendingLocalCheque3Log"] = "1";
							}

							sales.Cheque1Amount = null;
							if (!string.IsNullOrEmpty(form["Cheque1Amount"]))
							{
								sales.Cheque1Amount = Convert.ToDecimal(form["Cheque1Amount"]);
							}

							sales.Cheque2Amount = null;
							if (!string.IsNullOrEmpty(form["Cheque2Amount"]))
							{
								sales.Cheque2Amount = Convert.ToDecimal(form["Cheque2Amount"]);
							}

							sales.Cheque3Amount = null;
							if (!string.IsNullOrEmpty(form["Cheque3Amount"]))
							{
								sales.Cheque3Amount = Convert.ToDecimal(form["Cheque3Amount"]);
							}

							sales.BankTransferNo = null;
							if (!string.IsNullOrEmpty(form["BankTransferNo"]))
							{
								sales.BankTransferNo = form["BankTransferNo"].ToString();
								ViewData["PendingLocalBankTransferLog"] = "1";
							}

							sales.BankTransferAmount = null;
							if (!string.IsNullOrEmpty(form["BankTranferAmount"]))
							{
								sales.BankTransferAmount = Convert.ToDecimal(form["BankTranferAmount"]);
							}

							sales.CashBank = null;
							if (!string.IsNullOrEmpty(form["CashBank"]))
							{
								sales.CashBank = form["CashBank"].ToString();
							}

							sales.Cheque1Bank = null;
							if (!string.IsNullOrEmpty(form["Cheque1Bank"]))
							{
								sales.Cheque1Bank = form["Cheque1Bank"].ToString();
							}

							sales.Cheque2Bank = null;
							if (!string.IsNullOrEmpty(form["Cheque2Bank"]))
							{
								sales.Cheque2Bank = form["Cheque2Bank"].ToString();
							}

							sales.Cheque3Bank = null;
							if (!string.IsNullOrEmpty(form["Cheque3Bank"]))
							{
								sales.Cheque3Bank = form["Cheque3Bank"].ToString();
							}

							sales.BankTransferBank = null;
							if (!string.IsNullOrEmpty(form["BankTransferBank"]))
							{
								sales.BankTransferBank = form["BankTransferBank"].ToString();
							}

							sales.MemoBalance = null;
							if (!string.IsNullOrEmpty(form["MemoBalance"]))
							{
								sales.MemoBalance = Convert.ToDecimal(form["MemoBalance"]);
							}

							sales.Status = "Completed";
							sales.LastApprovalOn = DateTime.Now;

							//For rollback
							Sale oldData = _salesModel.GetSingle(id);

							Sale rollBack_Sale = new Sale()
							{
								ID = oldData.ID,
								MemoID = oldData.MemoID,
								CustomerParticularId = oldData.CustomerParticularId,
								IssueDate = oldData.IssueDate,
								CollectionDate = oldData.CollectionDate,
								CollectionTime = oldData.CollectionTime,
								CreatedBy = oldData.CreatedBy,
								Urgent = oldData.Urgent,
								RequireDelivery = oldData.RequireDelivery,
								BagNo = oldData.BagNo,
								Remarks = oldData.Remarks,
								CustomerRemarks = oldData.CustomerRemarks,
								TransactionType = oldData.TransactionType,
								LocalPaymentMode = oldData.LocalPaymentMode,
								CashAmount = oldData.CashAmount,
								CashBank = oldData.CashBank,
								Cheque1No = oldData.Cheque1No,
								Cheque1Amount = oldData.Cheque1Amount,
								Cheque1Bank = oldData.Cheque1Bank,
								Cheque2No = oldData.Cheque2No,
								Cheque2Amount = oldData.Cheque2Amount,
								Cheque2Bank = oldData.Cheque2Bank,
								Cheque3No = oldData.Cheque3No,
								Cheque3Amount = oldData.Cheque3Amount,
								Cheque3Bank = oldData.Cheque3Bank,
								BankTransferNo = oldData.BankTransferNo,
								BankTransferAmount = oldData.BankTransferAmount,
								BankTransferBank = oldData.BankTransferBank,
								MemoBalance = oldData.MemoBalance,
								TotalAmountLocal = oldData.TotalAmountLocal,
								TotalAmountForeign = oldData.TotalAmountForeign,
								Status = oldData.Status,
								LastApprovalOn = oldData.LastApprovalOn,
								PendingDeliveryById = oldData.PendingDeliveryById,
								DeliveryConfirmation = oldData.DeliveryConfirmation,
								DisapprovedReason = oldData.DisapprovedReason
							};

							Dictionary<int, ProductInventory> rollBack_ProductInventories = new Dictionary<int, ProductInventory>();
							List<int> rollBack_Inventories = new List<int>();

							try
							{
								bool result = _salesModel.Update(sales.ID, sales);
								DateTime CheckUpdateTime = sales.LastApprovalOn;

								if (result)
								{
									var CheckStatus = _salesModel.GetSingle(id);
									var CheckError = false;

									if (CheckStatus != null)
									{
										if (CheckUpdateTime != CheckStatus.LastApprovalOn)
										{
											CheckError = true;
										}
									}

									if (!CheckError)
									{
										//Product Inventory transaction
										bool add_productInventory = false;
										bool deduct_productInventory = false;

										bool add_inventory = false;
										bool deduct_inventory = false;

										List<TotalInAccountModel> ListOfTotalInAccount = new List<TotalInAccountModel>();
										TotalInAccountModel SingleItem = new TotalInAccountModel();

										List<Inventory> ListOfInventory = new List<Inventory>();
										Inventory SingleInventory = new Inventory();

										IList<SaleTransaction> saleTransactions = _saleTransactionsModel.GetSaleTransactions(id);

										foreach (SaleTransaction transaction in saleTransactions)
										{
											if (transaction.TransactionType == "Buy")
											{
												Product addProducts = _productsModel.GetSingle(transaction.CurrencyId);

												if (!rollBack_ProductInventories.ContainsKey(addProducts.ProductInventories[0].ID))
												{
													rollBack_ProductInventories.Add(addProducts.ProductInventories[0].ID, new ProductInventory()
													{
														ID = addProducts.ProductInventories[0].ID,
														ProductId = addProducts.ProductInventories[0].ProductId,
														TotalInAccount = addProducts.ProductInventories[0].TotalInAccount,
														CreatedOn = addProducts.ProductInventories[0].CreatedOn,
														UpdatedOn = addProducts.ProductInventories[0].UpdatedOn,
														IsDeleted = addProducts.ProductInventories[0].IsDeleted
													});
												}

												//addProducts.ProductInventories[0].TotalInAccount += transaction.AmountForeign;
												//bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

												SingleItem = new TotalInAccountModel();

												SingleItem.id = addProducts.ProductInventories[0].ID;
												SingleItem.Amount = transaction.AmountForeign;
												SingleItem.TransactionType = "plus";

												ListOfTotalInAccount.Add(SingleItem);

												SingleInventory = new Inventory();
												SingleInventory.ProductId = addProducts.ID;
												SingleInventory.Type = "Add";
												SingleInventory.Amount = transaction.AmountForeign;
												SingleInventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";

												ListOfInventory.Add(SingleInventory);

												//if (result_addProductInventory)
												//{
												//	Inventory inventory = new Inventory();
												//	inventory.ProductId = addProducts.ID;
												//	inventory.Type = "Add";
												//	inventory.Amount = transaction.AmountForeign;
												//	inventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";
												//	bool result_inventory = _inventoriesModel.Add(inventory);

												//	if (result_inventory)
												//	{
												//		rollBack_Inventories.Add(inventory.ID);

												//		if (!add_inventory)
												//		{
												//			add_inventory = true;
												//		}
												//	}

												//	if (!add_productInventory)
												//	{
												//		add_productInventory = true;
												//	}
												//}

												Product deductProducts = _productsModel.FindCurrencyCode("SGD");

												if (!rollBack_ProductInventories.ContainsKey(deductProducts.ProductInventories[0].ID))
												{
													rollBack_ProductInventories.Add(deductProducts.ProductInventories[0].ID, new ProductInventory()
													{
														ID = deductProducts.ProductInventories[0].ID,
														ProductId = deductProducts.ProductInventories[0].ProductId,
														TotalInAccount = deductProducts.ProductInventories[0].TotalInAccount,
														CreatedOn = deductProducts.ProductInventories[0].CreatedOn,
														UpdatedOn = deductProducts.ProductInventories[0].UpdatedOn,
														IsDeleted = deductProducts.ProductInventories[0].IsDeleted
													});
												}

												SingleItem = new TotalInAccountModel();

												SingleItem.id = deductProducts.ProductInventories[0].ID;
												SingleItem.Amount = transaction.AmountLocal;
												SingleItem.TransactionType = "minus";

												ListOfTotalInAccount.Add(SingleItem);

												SingleInventory = new Inventory();
												SingleInventory.ProductId = deductProducts.ID;
												SingleInventory.Type = "Deduct";
												SingleInventory.Amount = transaction.AmountLocal;
												SingleInventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";

												ListOfInventory.Add(SingleInventory);

												//deductProducts.ProductInventories[0].TotalInAccount -= transaction.AmountLocal;
												//bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

												//if (result_deductProductInventory)
												//{
												//	Inventory inventory = new Inventory();
												//	inventory.ProductId = deductProducts.ID;
												//	inventory.Type = "Deduct";
												//	inventory.Amount = transaction.AmountLocal;
												//	inventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";
												//	bool result_inventory = _inventoriesModel.Add(inventory);

												//	if (result_inventory)
												//	{
												//		rollBack_Inventories.Add(inventory.ID);

												//		if (!deduct_inventory)
												//		{
												//			deduct_inventory = true;
												//		}
												//	}

												//	if (!deduct_productInventory)
												//	{
												//		deduct_productInventory = true;
												//	}
												//}
											}
											else
											{
												Product deductProducts = _productsModel.GetSingle(transaction.CurrencyId);

												if (!rollBack_ProductInventories.ContainsKey(deductProducts.ProductInventories[0].ID))
												{
													rollBack_ProductInventories.Add(deductProducts.ProductInventories[0].ID, new ProductInventory()
													{
														ID = deductProducts.ProductInventories[0].ID,
														ProductId = deductProducts.ProductInventories[0].ProductId,
														TotalInAccount = deductProducts.ProductInventories[0].TotalInAccount,
														CreatedOn = deductProducts.ProductInventories[0].CreatedOn,
														UpdatedOn = deductProducts.ProductInventories[0].UpdatedOn,
														IsDeleted = deductProducts.ProductInventories[0].IsDeleted
													});
												}

												SingleItem = new TotalInAccountModel();
												SingleItem.id = deductProducts.ProductInventories[0].ID;
												SingleItem.Amount = transaction.AmountForeign;
												SingleItem.TransactionType = "minus";

												ListOfTotalInAccount.Add(SingleItem);

												SingleInventory = new Inventory();
												SingleInventory.ProductId = deductProducts.ID;
												SingleInventory.Type = "Deduct";
												SingleInventory.Amount = transaction.AmountForeign;
												SingleInventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";

												ListOfInventory.Add(SingleInventory);

												//deductProducts.ProductInventories[0].TotalInAccount -= transaction.AmountForeign;
												//bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

												//if (result_deductProductInventory)
												//{
												//	Inventory inventory = new Inventory();
												//	inventory.ProductId = deductProducts.ID;
												//	inventory.Type = "Deduct";
												//	inventory.Amount = transaction.AmountForeign;
												//	inventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";
												//	bool result_inventory = _inventoriesModel.Add(inventory);

												//	if (result_inventory)
												//	{
												//		rollBack_Inventories.Add(inventory.ID);

												//		if (!deduct_inventory)
												//		{
												//			deduct_inventory = true;
												//		}
												//	}

												//	if (!deduct_productInventory)
												//	{
												//		deduct_productInventory = true;
												//	}
												//}

												Product addProducts = _productsModel.FindCurrencyCode("SGD");

												if (!rollBack_ProductInventories.ContainsKey(addProducts.ProductInventories[0].ID))
												{
													rollBack_ProductInventories.Add(addProducts.ProductInventories[0].ID, new ProductInventory()
													{
														ID = addProducts.ProductInventories[0].ID,
														ProductId = addProducts.ProductInventories[0].ProductId,
														TotalInAccount = addProducts.ProductInventories[0].TotalInAccount,
														CreatedOn = addProducts.ProductInventories[0].CreatedOn,
														UpdatedOn = addProducts.ProductInventories[0].UpdatedOn,
														IsDeleted = addProducts.ProductInventories[0].IsDeleted
													});
												}

												SingleItem = new TotalInAccountModel();
												SingleItem.id = addProducts.ProductInventories[0].ID;
												SingleItem.Amount = transaction.AmountLocal;
												SingleItem.TransactionType = "plus";

												ListOfTotalInAccount.Add(SingleItem);

												//addProducts.ProductInventories[0].TotalInAccount += transaction.AmountLocal;
												//bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

												SingleInventory = new Inventory();
												SingleInventory.ProductId = addProducts.ID;
												SingleInventory.Type = "Add";
												SingleInventory.Amount = transaction.AmountLocal;
												SingleInventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";

												ListOfInventory.Add(SingleInventory);

												//if (result_addProductInventory)
												//{
												//	Inventory inventory = new Inventory();
												//	inventory.ProductId = addProducts.ID;
												//	inventory.Type = "Add";
												//	inventory.Amount = transaction.AmountLocal;
												//	inventory.Description = "Sale Transaction [MemoID: " + sales.MemoID + "]";
												//	bool result_inventory = _inventoriesModel.Add(inventory);

												//	if (result_inventory)
												//	{
												//		rollBack_Inventories.Add(inventory.ID);

												//		if (!add_inventory)
												//		{
												//			add_inventory = true;
												//		}
												//	}

												//	if (!add_productInventory)
												//	{
												//		add_productInventory = true;
												//	}
												//}
											}
										}

										var GetSalesStatus = _salesModel.GetSingle(sales.ID);

										if (GetSalesStatus != null)
										{
											if (CheckUpdateTime == GetSalesStatus.LastApprovalOn)
											{
												foreach (TotalInAccountModel total in ListOfTotalInAccount)
												{
													SingleInventory = new Inventory();
													if (total.TransactionType == "plus")
													{
														bool result_addProductInventory = _productInventoriesModel.Update2(total.id, total.Amount, total.TransactionType);

														if (result_addProductInventory)
														{
															add_productInventory = true;
															var ProductFinalAmount = _productInventoriesModel.GetSingle(total.id).TotalInAccount;
															if (ListOfInventory.Count > 0)
															{
																if (ListOfInventory.Where(e => e.ProductId == total.id && e.Type == "Add" && e.Amount == total.Amount && e.ID == 0).FirstOrDefault() != null)
																{
																	SingleInventory = ListOfInventory.Where(e => e.ProductId == total.id && e.Type == "Add" && e.Amount == total.Amount && e.ID == 0).FirstOrDefault();
																	SingleInventory.Description = SingleInventory.Description + " [After Bal: " + ProductFinalAmount + "]";
																	bool result_inventory = _inventoriesModel.Add(SingleInventory);

																	if (result_inventory)
																	{
																		add_inventory = true;
																	}
																}
															}
														}
													}
													else if (total.TransactionType == "minus")
													{
														bool result_deductProductInventory = _productInventoriesModel.Update2(total.id, total.Amount, total.TransactionType);

														if (result_deductProductInventory)
														{
															deduct_productInventory = true;
															var ProductFinalAmount = _productInventoriesModel.GetSingle(total.id).TotalInAccount;
															if (ListOfInventory.Count > 0)
															{
																if (ListOfInventory.Where(e => e.ProductId == total.id && e.Type == "Deduct" && e.Amount == total.Amount && e.ID == 0).FirstOrDefault() != null)
																{
																	SingleInventory = ListOfInventory.Where(e => e.ProductId == total.id && e.Type == "Deduct" && e.Amount == total.Amount && e.ID == 0).FirstOrDefault();
																	SingleInventory.Description = SingleInventory.Description + " [After Bal: " + ProductFinalAmount + "]";
																	bool result_inventory = _inventoriesModel.Add(SingleInventory);

																	if (result_inventory)
																	{
																		deduct_inventory = true;
																	}
																}
															}
														}
													}
												}

												int userid = Convert.ToInt32(Session["UserId"]);
												string tableAffected = "Sales";
												string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Sale [" + sales.MemoID + "].";

												bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

												if (sale_log)
												{
													//Add Approval History
													bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Sale [" + sales.MemoID + "]");

													if (sales.TransactionType == "Buy")
													{
														//Check the hidden Value
														if (ViewData["PendingChequeLog"].ToString() == "1")
														{
															bool chequelog = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Cheque (Transaction Row) [" + sales.MemoID + "]");
														}

														//Check the hidden Value
														if (ViewData["PendingBankTransferLog"].ToString() == "1")
														{
															bool banktransferlog = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Bank Transfer (Transaction Row) [" + sales.MemoID + "]");
														}
													}

													if (sales.TransactionType == "Sell")
													{
														//Check the hidden Value
														if (ViewData["PendingLocalCheque1Log"].ToString() == "1")
														{
															bool localcheque1log = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Cheque 1 (Local Payment) [" + sales.MemoID + "]");
														}

														//Check the hidden Value
														if (ViewData["PendingLocalCheque2Log"].ToString() == "1")
														{
															bool localcheque2log = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Cheque 2 (Local Payment) [" + sales.MemoID + "]");
														}

														//Check the hidden Value
														if (ViewData["PendingLocalCheque3Log"].ToString() == "1")
														{
															bool localcheque3log = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Cheque 3 (Local Payment) [" + sales.MemoID + "]");
														}

														//Check the hidden Value
														if (ViewData["PendingLocalBankTransferLog"].ToString() == "1")
														{
															bool localbanktransferlog = _approvalHistorysModel.Add("Sale_TransactionItem", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Checked Transaction Bank Transfer (Local Payment) [" + sales.MemoID + "]");
														}
													}
												}

												if (add_productInventory)
												{
													userid = Convert.ToInt32(Session["UserId"]);
													tableAffected = "ProductInventories";
													description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Product Inventories [" + sales.MemoID + "]";

													bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
												}

												if (add_inventory)
												{
													userid = Convert.ToInt32(Session["UserId"]);
													tableAffected = "Inventories";
													description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Inventories [" + sales.MemoID + "]";

													bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
												}

												if (deduct_productInventory)
												{
													userid = Convert.ToInt32(Session["UserId"]);
													tableAffected = "ProductInventories";
													description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Product Inventories [" + sales.MemoID + "]";

													bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
												}

												if (deduct_inventory)
												{
													userid = Convert.ToInt32(Session["UserId"]);
													tableAffected = "Inventories";
													description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Inventories [" + sales.MemoID + "]";

													bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
												}

												TempData["Result"] = "success|" + sales.MemoID + " has been successfully updated!";
												//TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
												updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";
											}
											else
											{
												updateResult = "{\"result\":\"error\", \"msg\":\"This record is been updated by someone.\"}";
											}
										}
										else
										{
											updateResult = "{\"result\":\"error\", \"msg\":\"Sales record not found.\"}";
										}
									}
									else
									{
										updateResult = "{\"result\":\"error\", \"msg\":\"This record is been updated by someone.\"}";
									}
								}
								else
								{
									updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
								}
							}
							catch (DbUpdateException e)
							{
								if (e.ToString().Contains("was deadlocked on lock resources with another process and has been chosen as the deadlock victim"))
								{
									//Roll back sale
									_salesModel.Update(rollBack_Sale.ID, rollBack_Sale);

									//Roll back product inventories
									foreach (int key in rollBack_ProductInventories.Keys)
									{
										_productInventoriesModel.Update(key, rollBack_ProductInventories[key]);
									}

									//Roll back Inventories
									foreach (int key in rollBack_Inventories)
									{
										_inventoriesModel.Delete(key);
									}
								}

								updateResult = "{\"result\":\"error\", \"msg\":\"An error occured while updating sales! Please resubmit again!\"}";
							}
							catch (Exception e)
							{
								//Roll back sale
								_salesModel.Update(rollBack_Sale.ID, rollBack_Sale);

								//Roll back product inventories
								foreach (int key in rollBack_ProductInventories.Keys)
								{
									_productInventoriesModel.Update(key, rollBack_ProductInventories[key]);
								}

								//Roll back Inventories
								foreach (int key in rollBack_Inventories)
								{
									_inventoriesModel.Delete(key);
								}

								updateResult = "{\"result\":\"error\", \"msg\":\"An error occured while updating sales! Please resubmit again!\"}";
							}
						}
						else
						{
							string msg = "";

							foreach (string[] error in modelErrors)
							{
								msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
							}

							updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
						}
					}
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //GET: DisapproveSale
        public ActionResult DisapproveSale(int id)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;

                //Exception Status for Disapproval
                List<string> exceptionStatus = new List<string>();
                exceptionStatus.Add("Pending Assign Delivery");
                exceptionStatus.Add("Pending Incoming Delivery");
                //exceptionStatus.Add("Pending Cashier");
                exceptionStatus.Add("Completed");
                exceptionStatus.Add("Pending Delete GM Approval");
                exceptionStatus.Add("Pending GM Approval (Rejected)");
                exceptionStatus.Add("Cancelled");

                if (!exceptionStatus.Contains(sales.Status))
                {
                    Dropdown[] reasonDDL = ReasonDDL();
                    ViewData["ReasonDropdown"] = new SelectList(reasonDDL, "val", "name");

                    return View();
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Disapproval for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|Disapproval for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //POST: DisapproveSale
        [HttpPost]
        public ActionResult DisapproveSale(int id, FormCollection form)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;

                //Exception Status for Disapproval
                List<string> exceptionStatus = new List<string>();
                exceptionStatus.Add("Pending Assign Delivery");
                exceptionStatus.Add("Pending Incoming Delivery");
                //exceptionStatus.Add("Pending Cashier");
                exceptionStatus.Add("Completed");
                exceptionStatus.Add("Pending Delete GM Approval");
                exceptionStatus.Add("Pending GM Approval (Rejected)");
                exceptionStatus.Add("Cancelled");

                if (!exceptionStatus.Contains(sales.Status))
                {
                    List<string[]> modelErrors = new List<string[]>();

                    if (string.IsNullOrEmpty(form["Reason"]))
                    {
                        string[] error = new string[2];
                        error[0] = "Reason";
                        error[1] = "Reason is required!";
                        modelErrors.Add(error);
                    }

                    if (string.IsNullOrEmpty(form["Remarks"]))
                    {
                        string[] error = new string[2];
                        error[0] = "Remarks";
                        error[1] = "Remarks is required!";
                        modelErrors.Add(error);
                    }

                    if (modelErrors.Count == 0)
                    {
                        sales.DisapprovedReason = form["Reason"].ToString();
                        sales.Remarks += "    [Disapproved Remarks: " + form["Remarks"].Trim() + "]";

                        bool result = _salesModel.DisapproveSale(id, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Disapproved Sale [" + sales.MemoID + "].";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Disapprove", "[" + Session["Username"].ToString() + "] Disapproved Sale [" + sales.MemoID + "].");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been disapproved!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been disapproved!\"}";
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while disapproving sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Disapproval for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|Disapproval for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult);
        }

        //GET: UpdateDisapprovedSale
        public ActionResult UpdateDisapprovedSale(int id, string status)
        {
            Sale sales = _salesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending GM Approval (Rejected)")
                {
                    bool result = false;

                    if (status == "Repack")
                    {
                        result = _salesModel.UpdateStatus(id, "Pending Packing");
                    }
                    else
                    {
                        result = _salesModel.UpdateStatus(id, "Cancelled");
                    }

                    if (result)
                    {
                        if (status == "Repack")
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Repacked Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Repack", "[" + Session["Username"].ToString() + "] Repacked Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been sent to repack!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been sent to repack!\"}";
                        }
                        else
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Cancelled Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Cancel", "[" + Session["Username"].ToString() + "] Cancelled Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been cancelled!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been cancelled!\"}";
                        }
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Error while disapproving sale record!\"}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"" + status + " for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|" + status + " for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //GET: GMApprovalSale
        public ActionResult GMApprovalSale(int id, string approval)
        {
            Sale sales = _salesModel.GetSingle(id);

            if (sales != null)
            {
                if (sales.Status == "Pending GM Approval")
                {
                    if (approval == "approve")
                    {
                        bool result = _salesModel.UpdateStatus(id, "Pending Accounts");

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Sale [" + sales.MemoID + "].";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Approve", "[" + Session["Username"].ToString() + "] Approved Sale [" + sales.MemoID + "].");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully approved!");
                        }
                        else
                        {
                            TempData.Add("Result", "error|Error while approving sale record!");
                        }
                    }
                    else
                    {
                        bool result = _salesModel.UpdateStatus(id, "Rejected");

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Rejected Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Reject", "[" + Session["Username"].ToString() + "] Rejected Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully rejected!");
                        }
                        else
                        {
                            TempData.Add("Result", "error|Error while rejecting sale record!");
                        }
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|Memo ID no need approval!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return RedirectToAction("Listing");
        }

        //GET: ValidateAmount
        public void ValidateAmount(string amount, int transactionId)
        {
            bool result = FormValidationHelper.NonNegativeAmountValidation(amount);

            if (result)
            {
                SaleTransaction transaction = _saleTransactionsModel.GetSingle(transactionId);

                string amt = FormValidationHelper.AmountFormatter(Convert.ToDecimal(amount), sgdDp);

                Response.Write("{\"result\":\"true\", \"msg\":\"" + amt + "\"}");
            }
            else
            {
                Response.Write("{\"result\":\"false\", \"msg\":\"'" + amount + "' is not a valid Amount!\"}");
            }
        }

        //GET: ValidatePieces
        public void ValidatePieces(string pieces)
        {
            bool result = FormValidationHelper.IntegerValidation(pieces);

            if (result)
            {
                string pc = FormValidationHelper.AmountFormatter(Convert.ToDecimal(pieces), 0);
                Response.Write("{\"result\":\"true\", \"msg\":\"" + pc + "\"}");
            }
            else
            {
                Response.Write("{\"result\":\"false\", \"msg\":\"'" + pieces + "' is not a valid Pieces!\"}");
            }
        }

        //GET: FormatAmount
        public string FormatAmount(decimal amount, int dp)
        {
            string amt = FormValidationHelper.AmountFormatter(amount, sgdDp);

            string result = "{\"Result\":true,\"Amount\":\"" + amt + "\"}";

            return result;
        }

        //GET: CalculateDenominationAmountLocal
        public void CalculateDenominationAmountLocal(decimal amount, int transactionId)
        {
            SaleTransaction transaction = _saleTransactionsModel.GetSingle(transactionId);

            decimal rate = transaction.Rate;

            if (transaction.TransactionType == "Buy")
            {
                if (transaction.Sales.TransactionType == "Encashment" || transaction.Sales.TransactionType == "Deposit")
                {
                    if (transaction.EncashmentRate > 0)
                    {
                        rate /= Convert.ToDecimal(transaction.EncashmentRate);
                    }
                }

                amount *= rate;
            }
            else
            {
                if (transaction.Sales.TransactionType == "Encashment" || transaction.Sales.TransactionType == "Deposit")
                {
                    rate *= Convert.ToDecimal(transaction.EncashmentRate);
                }

                amount /= rate;
            }

            string amt = FormValidationHelper.AmountFormatter(amount, sgdDp);

            Response.Write("{\"result\":\"true\", \"msg\":\"" + amt + "\"}");
        }

        //GET: CalculateDenominationAmountForeign
        public void CalculateDenominationAmountForeign(int pieces, int transactionId, int denominationId)
        {
            SaleTransaction transaction = _saleTransactionsModel.GetSingle(transactionId);

            int denominationVal = 0;

            if (denominationId > 0)
            {
                SaleTransactionDenomination denomination = _saleTransactionDenominationsModel.GetSingle(denominationId);
                denominationVal = denomination.Denomination;
            }
            else
            {
                denominationId *= -1;
                ProductDenomination denomination = _productDenominationsModel.GetSingle(denominationId);
                denominationVal = denomination.DenominationValue;
            }


            decimal amount = pieces * denominationVal;

            string amt = FormValidationHelper.AmountFormatter(amount, sgdDp);

            Response.Write("{\"result\":\"true\", \"msg\":\"" + amt + "\"}");
        }

        //GET: GMDeleteSaleApproval
        public ActionResult GMDeleteSaleApproval(int id, string d)
        {
            Sale sales = _salesModel.GetSingle(id);

            if (sales != null)
            {
                string userRole = Session["UserRole"].ToString();
                string[] userRoleList = userRole.Split(',');

                if (sales.Status == "Pending Delete GM Approval")
                {
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        if (d == "approve")
                        {
                            sales.Status = "Cancelled";

                            bool result = _salesModel.Update(sales.ID, sales);

                            if (result)
                            {
                                int userid = Convert.ToInt32(Session["UserId"]);
                                string tableAffected = "Sales";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Pending Delete Sale [" + sales.MemoID + "]";

                                bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                if (sale_log)
                                {
                                    //Add Approval History
                                    bool approval_history = _approvalHistorysModel.Add("Sale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Cancel", "[" + Session["Username"].ToString() + "] Approved Pending Delete Sale [" + sales.MemoID + "]");
                                }

                                //Update Each Related Parts
                                #region Return amounts to Product
                                bool add_productInventory = false;
                                bool deduct_productInventory = false;

                                bool add_inventory = false;
                                bool deduct_inventory = false;

                                Dictionary<string, decimal> transactAmount = new Dictionary<string, decimal>();

                                foreach (SaleTransaction transaction in sales.SaleTransactions)
                                {
                                    if (transaction.TransactionType == "Buy")
                                    {
                                        Product addProducts = _productsModel.GetSingle(transaction.CurrencyId);
                                        addProducts.ProductInventories[0].TotalInAccount -= transaction.AmountForeign;
                                        if (transactAmount.ContainsKey(addProducts.CurrencyCode))
                                        {
                                            transactAmount[addProducts.CurrencyCode] -= transaction.AmountForeign;
                                        }
                                        else
                                        {
                                            transactAmount.Add(addProducts.CurrencyCode, transaction.AmountForeign * -1);
                                        }
                                        bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

                                        if (result_addProductInventory)
                                        {
                                            Inventory inventory = new Inventory();
                                            inventory.ProductId = addProducts.ID;
                                            inventory.Type = "Deduct";
                                            inventory.Amount = transaction.AmountForeign;
                                            inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                            bool result_inventory = _inventoriesModel.Add(inventory);

                                            if (result_inventory)
                                            {
                                                if (!add_inventory)
                                                {
                                                    add_inventory = true;
                                                }
                                            }

                                            if (!add_productInventory)
                                            {
                                                add_productInventory = true;
                                            }
                                        }

                                        Product deductProducts = _productsModel.FindCurrencyCode("SGD");
                                        deductProducts.ProductInventories[0].TotalInAccount += transaction.AmountLocal;
                                        if (transactAmount.ContainsKey(deductProducts.CurrencyCode))
                                        {
                                            transactAmount[deductProducts.CurrencyCode] += transaction.AmountLocal;
                                        }
                                        else
                                        {
                                            transactAmount.Add(deductProducts.CurrencyCode, transaction.AmountLocal);
                                        }
                                        bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

                                        if (result_deductProductInventory)
                                        {
                                            Inventory inventory = new Inventory();
                                            inventory.ProductId = deductProducts.ID;
                                            inventory.Type = "Add";
                                            inventory.Amount = transaction.AmountLocal;
                                            inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                            bool result_inventory = _inventoriesModel.Add(inventory);

                                            if (result_inventory)
                                            {
                                                if (!deduct_inventory)
                                                {
                                                    deduct_inventory = true;
                                                }
                                            }

                                            if (!deduct_productInventory)
                                            {
                                                deduct_productInventory = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Product deductProducts = _productsModel.GetSingle(transaction.CurrencyId);
                                        deductProducts.ProductInventories[0].TotalInAccount += transaction.AmountForeign;
                                        if (transactAmount.ContainsKey(deductProducts.CurrencyCode))
                                        {
                                            transactAmount[deductProducts.CurrencyCode] += transaction.AmountForeign;
                                        }
                                        else
                                        {
                                            transactAmount.Add(deductProducts.CurrencyCode, transaction.AmountForeign);
                                        }
                                        bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

                                        if (result_deductProductInventory)
                                        {
                                            Inventory inventory = new Inventory();
                                            inventory.ProductId = deductProducts.ID;
                                            inventory.Type = "Add";
                                            inventory.Amount = transaction.AmountForeign;
                                            inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                            bool result_inventory = _inventoriesModel.Add(inventory);

                                            if (result_inventory)
                                            {
                                                if (!deduct_inventory)
                                                {
                                                    deduct_inventory = true;
                                                }
                                            }

                                            if (!deduct_productInventory)
                                            {
                                                deduct_productInventory = true;
                                            }
                                        }

                                        Product addProducts = _productsModel.FindCurrencyCode("SGD");
                                        addProducts.ProductInventories[0].TotalInAccount -= transaction.AmountLocal;
                                        if (transactAmount.ContainsKey(addProducts.CurrencyCode))
                                        {
                                            transactAmount[addProducts.CurrencyCode] -= transaction.AmountLocal;
                                        }
                                        else
                                        {
                                            transactAmount.Add(addProducts.CurrencyCode, transaction.AmountLocal * -1);
                                        }
                                        bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

                                        if (result_addProductInventory)
                                        {
                                            Inventory inventory = new Inventory();
                                            inventory.ProductId = addProducts.ID;
                                            inventory.Type = "Deduct";
                                            inventory.Amount = transaction.AmountLocal;
                                            inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                            bool result_inventory = _inventoriesModel.Add(inventory);

                                            if (result_inventory)
                                            {
                                                if (!add_inventory)
                                                {
                                                    add_inventory = true;
                                                }
                                            }

                                            if (!add_productInventory)
                                            {
                                                add_productInventory = true;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Update End of Day Trade
                                bool hasDeleteEndDayTradeTransactions = false;
                                bool hasUpdateTrade = false;

                                List<EndDayTradeTransaction> endDayTradeTransactions = _endDayTradeTransactionsModel.GetAll(sales.SaleTransactions.Select(e => e.ID).ToList(), sales.LastApprovalOn);

                                if (endDayTradeTransactions.Count > 0)
                                {
                                    List<EndDayTrade> endDayTrades = endDayTradeTransactions.Select(e => e.EndDayTrade).Distinct().ToList();

                                    //Delete existing records
                                    foreach (EndDayTradeTransaction tradeTransaction in endDayTradeTransactions)
                                    {
                                        bool endDayTradeTransactions_delete = _endDayTradeTransactionsModel.Delete(tradeTransaction.ID);

                                        if (endDayTradeTransactions_delete)
                                        {
                                            hasDeleteEndDayTradeTransactions = true;
                                        }
                                    }

                                    //Recalculate End of Day Trades
                                    if (hasDeleteEndDayTradeTransactions)
                                    {
                                        //Dictionary<int, decimal[]> currentClosings = new Dictionary<int, decimal[]>();

                                        foreach (EndDayTrade trade in endDayTrades)
                                        {
                                            decimal totalAmountForeign = 0;
                                            decimal totalAmountLocal = 0;
                                            List<string> desciptions = new List<string>();

                                            List<EndDayTradeTransaction> tradeTransactions = _endDayTradeTransactionsModel.GetAll(trade.ID);

                                            string amountForeignFormat = GetDecimalFormat(trade.Products.Decimal);
                                            string sgdFormat = GetDecimalFormat(sgdDp);

                                            foreach (EndDayTradeTransaction transaction in tradeTransactions)
                                            {
                                                totalAmountForeign += transaction.AmountForeign;
                                                totalAmountLocal += transaction.AmountLocal;
                                                desciptions.Add(String.Format("{0}-{1}:{2}:{3}:{4}:{5}", transaction.SaleTransaction.Sales.MemoID, transaction.SaleTransaction.TransactionID, transaction.SaleTransaction.Products.Symbol + transaction.AmountForeign.ToString(amountForeignFormat), transaction.SaleTransaction.Rate.ToString(GetRateFormat(rateDP)), transaction.SaleTransaction.EncashmentRate != null ? Convert.ToDecimal(transaction.SaleTransaction.EncashmentRate).ToString(GetRateFormat(rateDP)) : "-", "SGD" + (transaction.AmountLocal * trade.Products.Unit).ToString(sgdFormat)));
                                            }

                                            EndDayTrade previousTrade = _endDayTradesModel.GetProductPreviousTrade(trade.CurrencyId, trade.LastActivationTime);

                                            decimal openingBankAmount = trade.OpeningBankAmount;
                                            decimal openingCashAmount = trade.OpeningCashAmount;
                                            decimal openingForeignCurrencyBal = trade.OpeningForeignCurrencyBalance;
                                            decimal openingAveragePurchaseCost = trade.OpeningAveragePurchaseCost;
                                            decimal openingBalanceAtAveragePurchase = trade.OpeningBalanceAtAveragePurchase;
                                            decimal openingProfitAmount = trade.OpeningProfitAmount;
                                            decimal closingBankAmount = trade.ClosingBankAmount;
                                            decimal closingCashAmount = trade.ClosingCashAmount;
                                            decimal closingForeignCurrencyBal = trade.ClosingForeignCurrencyBalance;
                                            decimal closingAveragePurchaseCost = trade.ClosingAveragePurchaseCost;
                                            decimal closingBalanceAtAveragePurchase = trade.ClosingBalanceAtAveragePurchase;
                                            decimal closingProfitAmount = trade.ClosingProfitAmount;
                                            List<string> transactionDescription = new List<string>();

                                            decimal foreignCurrencyBal = trade.ClosingForeignCurrencyBalance;
                                            decimal averageRate = trade.ClosingAveragePurchaseCost;
                                            decimal closingBal = trade.ClosingBalanceAtAveragePurchase;

                                            if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                            {
                                                foreignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                            }

                                            if (previousTrade == null)
                                            {
                                                if (transactAmount.ContainsKey("SGD"))
                                                {
                                                    openingBankAmount += transactAmount["SGD"];
                                                    openingCashAmount += transactAmount["SGD"];
                                                }

                                                if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                                {
                                                    openingForeignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                                }

                                                if (totalAmountForeign != 0)
                                                {
                                                    averageRate = totalAmountLocal / totalAmountForeign;
                                                }

                                                closingBal = foreignCurrencyBal * averageRate;

                                                openingAveragePurchaseCost = averageRate;
                                                openingBalanceAtAveragePurchase = closingBal;
                                            }

                                            if (transactAmount.ContainsKey("SGD"))
                                            {
                                                closingBankAmount += transactAmount["SGD"];
                                                closingCashAmount += transactAmount["SGD"];
                                            }

                                            if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                            {
                                                closingForeignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                            }

                                            closingAveragePurchaseCost = averageRate;
                                            closingBalanceAtAveragePurchase = closingBal;
                                            closingProfitAmount = (openingForeignCurrencyBal * trade.CurrentSGDBuyRate) + closingBankAmount + closingCashAmount - foreignCurrencyBal * trade.CurrentSGDBuyRate - closingBal * trade.CurrentSGDBuyRate;

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
                                            trade.Description = String.Join("|", desciptions);

                                            bool trade_update = _endDayTradesModel.Update(trade.ID, trade);

                                            if (trade_update)
                                            {
                                                hasUpdateTrade = true;

                                                //Record for closings, to update the next opening if any
                                                //currentClosings.Add(
                                                //    trade.CurrencyId,
                                                //    new decimal[7]
                                                //    {
                                                //        Convert.ToDecimal(trade.CurrentActivationTime.ToString("yyyyMMddHHmmss.fffffff")),
                                                //        trade.ClosingBankAmount,
                                                //        trade.ClosingCashAmount,
                                                //        trade.ClosingForeignCurrencyBalance,
                                                //        trade.ClosingAveragePurchaseCost,
                                                //        trade.ClosingBalanceAtAveragePurchase,
                                                //        trade.ClosingProfitAmount
                                                //    });
                                            }
                                        }

                                        //Update next end day trade if any
                                        //foreach (KeyValuePair<int, decimal[]> key in currentClosings)
                                        //{
                                        //    int currencyId = key.Key;
                                        //    DateTime currentActivation = Convert.ToDateTime(String.Format("{0}/{1}/{2} {3}:{4}:{5}", key.Value[0].ToString().Substring(0, 4), key.Value[0].ToString().Substring(4, 2), key.Value[0].ToString().Substring(6, 2), key.Value[0].ToString().Substring(8, 2), key.Value[0].ToString().Substring(10)));

                                        //    EndDayTrade nextTrade = _endDayTradesModel.GetProductNextTrade(currencyId, currentActivation);

                                        //    while (nextTrade != null)
                                        //    {

                                        //        currentActivation = nextTrade.CurrentActivationTime;

                                        //        nextTrade = _endDayTradesModel.GetProductNextTrade(currencyId, currentActivation);
                                        //    }
                                        //}
                                    }
                                }
                                #endregion

                                if (add_productInventory)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "ProductInventories";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Product Inventories [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                if (add_inventory)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "Inventories";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Inventories [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                if (deduct_productInventory)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "ProductInventories";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Product Inventories [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                if (deduct_inventory)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "Inventories";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Inventories [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                if (hasDeleteEndDayTradeTransactions)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "EndDayTradeTransactions";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted End Day Trade Transactions [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                if (hasUpdateTrade)
                                {
                                    userid = Convert.ToInt32(Session["UserId"]);
                                    tableAffected = "EndDayTrade";
                                    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated End Day Trades [" + sales.MemoID + "]";

                                    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                }

                                TempData.Add("Result", "success|" + sales.MemoID + " has been successfully approved and cancelled!");
                            }
                            else
                            {
                                TempData.Add("Result", "danger|An error occured while approving delete sale record!");
                            }
                        }
                        else
                        {
                            sales.Status = "Completed";

                            bool result = _salesModel.Update(sales.ID, sales);

                            if (result)
                            {
                                int userid = Convert.ToInt32(Session["UserId"]);
                                string tableAffected = "Sales";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Rejected Pending Delete Sale [" + sales.MemoID + "]";

                                bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                TempData.Add("Result", "success|" + sales.MemoID + " has been successfully rejected and returned to completed state!");
                            }
                            else
                            {
                                TempData.Add("Result", "danger|An error occured while rejecting delete sale record!");
                            }
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|You have no access to delete this sale!");
                    }
                }
                else
                {
                    TempData.Add("Result", "error|Cannot delete sale at the moment!");
                }
            }
            else
            {
                TempData.Add("Result", "error|Memo ID not found!");
            }

            return RedirectToAction("Listing");
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

                                        if (mimeType != "application/pdf")
                                        {
                                            oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), "ori_" + newFileName);

                                            attachment.SaveAs(oriPath);

                                            string resizedPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

                                            int maxWidth = int.Parse(ConfigurationManager.AppSettings["MaxImgWidth"].ToString());
                                            int maxHeight = int.Parse(ConfigurationManager.AppSettings["MaxImgHeight"].ToString());

                                            int width = 0;
                                            int height = 0;

                                            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(oriPath))
                                            {
                                                width = Img.Width;
                                                height = Img.Height;
                                            }

                                            if (width >= maxWidth || height >= maxHeight)
                                            {
                                                ImageResizer.ImageJob i = new ImageResizer.ImageJob(oriPath, resizedPath, new ImageResizer.ResizeSettings(
                                           "width=" + maxWidth + ";height=" + maxHeight + ";format=jpg;mode=pad"));//mode=null, max, pad(default), crop, carve, stretch

                                                i.Build();
                                            }
                                            else
                                            {
                                                ImageResizer.ImageJob i = new ImageResizer.ImageJob(oriPath, resizedPath, new ImageResizer.ResizeSettings(
                                           "width=" + maxWidth + ";height=" + maxHeight + ";format=jpg;scale=canvas"));

                                                i.Build();
                                            }

                                            System.IO.File.Delete(oriPath);
                                        }
                                        else
                                        {
                                            oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

                                            attachment.SaveAs(oriPath);
                                        }

                                        filesUploaded += newFileName + ",";
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
                    Response.Write("{\"result\":\"success\",\"msg\":\"" + filesUploaded.Substring(0, filesUploaded.Length - 1) + "\"}");
                }
            }
            catch
            {
                Response.Write("{\"result\":\"error\",\"msg\":\"An error occured while uploading file.\"}");
            }
        }

        //Disapprove Reason Dropdown
        public Dropdown[] ReasonDDL()
        {
            Dropdown[] ddl = new Dropdown[5];
            ddl[0] = new Dropdown { name = "Cheque Not Ready", val = "Cheque Not Ready" };
            ddl[1] = new Dropdown { name = "Repacking Error", val = "Repacking Error" };
            ddl[2] = new Dropdown { name = "Reason 3", val = "Reason 3" };
            ddl[3] = new Dropdown { name = "Reason 4", val = "Reason 4" };
            ddl[4] = new Dropdown { name = "Other", val = "Other" };
            return ddl;
        }

        //CollectionTime Dropdown
        public Dropdown[] CollectionTimeDDL()
        {
            Dropdown[] ddl = new Dropdown[4];
            ddl[0] = new Dropdown { name = "9am to 10am", val = "9am to 10am" };
            ddl[1] = new Dropdown { name = "10am to 12pm", val = "10am to 12pm" };
            ddl[2] = new Dropdown { name = "2pm to 3pm", val = "2pm to 3pm" };
            ddl[3] = new Dropdown { name = "3pm to 5pm", val = "3pm to 5pm" };
            return ddl;
        }

        //LocalPaymentBank Dropdown
        public Dropdown[] LocalPaymentBankDDL(string selectedBank = "")
        {
            List<string> localPaymentBanks = _settingsModel.GetCodeValue("LOCAL_PAYMENT_BANK").Split('|').OrderByDescending(e => e == "CASH").ThenBy(e => e).ToList();

            if (!string.IsNullOrEmpty(selectedBank) && !localPaymentBanks.Contains(selectedBank))
            {
                localPaymentBanks.Insert(0, selectedBank);
            }

            Dropdown[] ddl = new Dropdown[localPaymentBanks.Count + 1];
            ddl[0] = new Dropdown { name = "No selected", val = "" };

            int count = 1;

            foreach (string bank in localPaymentBanks)
            {
                ddl[count] = new Dropdown { name = bank, val = bank };

                count++;
            }

            return ddl;
        }

        //GET: DisapproveSale
        public ActionResult DisapproveRemittanceSale(int id)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;

                //Exception Status for Disapproval
                List<string> exceptionStatus = new List<string>();
                //exceptionStatus.Add("Pending Assign Delivery");
                //exceptionStatus.Add("Pending Incoming Delivery");
                //exceptionStatus.Add("Pending Cashier");
                exceptionStatus.Add("Completed");
                exceptionStatus.Add("Pending Delete GM Approval");
                exceptionStatus.Add("Pending GM Approval (Rejected)");
                exceptionStatus.Add("Cancelled");

                if (!exceptionStatus.Contains(sales.Status))
                {
                    Dropdown[] reasonDDL = ReasonDDL();
                    ViewData["ReasonDropdown"] = new SelectList(reasonDDL, "val", "name");

                    return View();
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Disapproval for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|Disapproval for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //POST: DisapproveSale
        [HttpPost]
        public ActionResult DisapproveRemittanceSale(int id, FormCollection form)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;

                //Exception Status for Disapproval
                List<string> exceptionStatus = new List<string>();
                //exceptionStatus.Add("Pending Assign Delivery");
                //exceptionStatus.Add("Pending Incoming Delivery");
                //exceptionStatus.Add("Pending Cashier");
                exceptionStatus.Add("Completed");
                exceptionStatus.Add("Pending Delete GM Approval");
                exceptionStatus.Add("Pending GM Approval (Rejected)");
                exceptionStatus.Add("Cancelled");

                if (!exceptionStatus.Contains(sales.Status))
                {
                    List<string[]> modelErrors = new List<string[]>();

                    if (string.IsNullOrEmpty(form["Reason"]))
                    {
                        string[] error = new string[2];
                        error[0] = "Reason";
                        error[1] = "Reason is required!";
                        modelErrors.Add(error);
                    }

                    if (string.IsNullOrEmpty(form["Remarks"]))
                    {
                        string[] error = new string[2];
                        error[0] = "Remarks";
                        error[1] = "Remarks is required!";
                        modelErrors.Add(error);
                    }

                    if (modelErrors.Count == 0)
                    {
                        sales.DisapprovedReason = form["Reason"].ToString();
                        sales.Remarks += "    [Disapproved Remarks: " + form["Remarks"].Trim() + "]";

                        bool result = _remittancesalesModel.DisapproveSale(id, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "RemittanceSales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Disapproved Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSales", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Disapprove", "[" + Session["Username"].ToString() + "] Disapproved Remittance Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been disapproved!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been disapproved!\"}";
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while disapproving sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Disapproval for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|Disapproval for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult);
        }

        //GET: UpdateDisapprovedSale
        public ActionResult UpdateDisapprovedRemittanceSale(int id, string status)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending GM Approval (Rejected)")
                {
                    bool result = false;

                    if (status == "Repack")
                    {
                        result = _remittancesalesModel.UpdateStatus(id, "Pending Dealer");
                    }
                    else
                    {
                        result = _remittancesalesModel.UpdateStatus(id, "Cancelled");
                    }

                    if (result)
                    {
                        if (status == "Repack")
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "RemittanceSales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Repacked Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Repack", "[" + Session["Username"].ToString() + "] Repacked Remittance Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been sent to repack!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been sent to repack!\"}";
                        }
                        else
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "RemittanceSales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Cancelled Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Cancel", "[" + Session["Username"].ToString() + "] Cancelled Remittance Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been cancelled!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been cancelled!\"}";
                        }
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Error while disapproving sale record!\"}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"" + status + " for " + sales.MemoID + " is not available at the moment!\"}";
                    TempData.Add("Result", "danger|" + status + " for " + sales.MemoID + " is not available at the moment!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //GET: GMApprovalSale
        public ActionResult GMApprovalRemittanceSale(int id, string approval)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            if (sales != null)
            {
                if (sales.Status == "Pending GM Approval")
                {
                    if (approval == "approve")
                    {
                        bool result = _remittancesalesModel.UpdateStatus(id, "Pending Accounts (Check Funds)");

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Remittance Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Approve", "[" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully approved!");
                        }
                        else
                        {
                            TempData.Add("Result", "error|Error while approving sale record!");
                        }
                    }
                    else
                    {
                        bool result = _remittancesalesModel.UpdateStatus(id, "Rejected");

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Remittance Sales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Rejected Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Reject", "[" + Session["Username"].ToString() + "] Rejected Remittance Sale [" + sales.MemoID + "]");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully rejected!");
                        }
                        else
                        {
                            TempData.Add("Result", "error|Error while rejecting sale record!");
                        }
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|Memo ID no need approval!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return RedirectToAction("Listing");
        }

        //GET: GMDeleteSaleApproval
        public ActionResult GMDeleteRemittanceSaleApproval(int id, string d)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            if (sales != null)
            {
                string userRole = Session["UserRole"].ToString();
                string[] userRoleList = userRole.Split(',');

                if (sales.Status == "Pending Delete GM Approval")
                {
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        if (d == "approve")
                        {
                            sales.Status = "Cancelled";

                            bool result = _remittancesalesModel.Update(sales.ID, sales);

                            if (result)
                            {
                                int userid = Convert.ToInt32(Session["UserId"]);
                                string tableAffected = "RemittanceSales";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Pending Delete Remittance Sale [" + sales.MemoID + "]";

                                bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                if (sale_log)
                                {
                                    //Add Approval History
                                    bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Cancel", "[" + Session["Username"].ToString() + "] Approved Pending Delete Remittance Sale [" + sales.MemoID + "]");
                                }

                                ////Update Each Related Parts
                                //#region Return amounts to Product
                                //bool add_productInventory = false;
                                //bool deduct_productInventory = false;

                                //bool add_inventory = false;
                                //bool deduct_inventory = false;

                                //Dictionary<string, decimal> transactAmount = new Dictionary<string, decimal>();

                                //foreach (RemittanceOrders transaction in sales.RemittanceOders)
                                //{
                                //    //if (transaction.TransactionType == "Buy")
                                //    //{
                                //    //    Product addProducts = _productsModel.GetSingle(transaction.CurrencyId);
                                //    //    addProducts.ProductInventories[0].TotalInAccount -= transaction.AmountForeign;
                                //    //    if (transactAmount.ContainsKey(addProducts.CurrencyCode))
                                //    //    {
                                //    //        transactAmount[addProducts.CurrencyCode] -= transaction.AmountForeign;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        transactAmount.Add(addProducts.CurrencyCode, transaction.AmountForeign * -1);
                                //    //    }
                                //    //    bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

                                //    //    if (result_addProductInventory)
                                //    //    {
                                //    //        Inventory inventory = new Inventory();
                                //    //        inventory.ProductId = addProducts.ID;
                                //    //        inventory.Type = "Deduct";
                                //    //        inventory.Amount = transaction.AmountForeign;
                                //    //        inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                //    //        bool result_inventory = _inventoriesModel.Add(inventory);

                                //    //        if (result_inventory)
                                //    //        {
                                //    //            if (!add_inventory)
                                //    //            {
                                //    //                add_inventory = true;
                                //    //            }
                                //    //        }

                                //    //        if (!add_productInventory)
                                //    //        {
                                //    //            add_productInventory = true;
                                //    //        }
                                //    //    }

                                //    //    Product deductProducts = _productsModel.FindCurrencyCode("SGD");
                                //    //    deductProducts.ProductInventories[0].TotalInAccount += transaction.AmountLocal;
                                //    //    if (transactAmount.ContainsKey(deductProducts.CurrencyCode))
                                //    //    {
                                //    //        transactAmount[deductProducts.CurrencyCode] += transaction.AmountLocal;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        transactAmount.Add(deductProducts.CurrencyCode, transaction.AmountLocal);
                                //    //    }
                                //    //    bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

                                //    //    if (result_deductProductInventory)
                                //    //    {
                                //    //        Inventory inventory = new Inventory();
                                //    //        inventory.ProductId = deductProducts.ID;
                                //    //        inventory.Type = "Add";
                                //    //        inventory.Amount = transaction.AmountLocal;
                                //    //        inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                //    //        bool result_inventory = _inventoriesModel.Add(inventory);

                                //    //        if (result_inventory)
                                //    //        {
                                //    //            if (!deduct_inventory)
                                //    //            {
                                //    //                deduct_inventory = true;
                                //    //            }
                                //    //        }

                                //    //        if (!deduct_productInventory)
                                //    //        {
                                //    //            deduct_productInventory = true;
                                //    //        }
                                //    //    }
                                //    //}
                                //    //else
                                //    //{
                                //    //    Product deductProducts = _productsModel.GetSingle(transaction.CurrencyId);
                                //    //    deductProducts.ProductInventories[0].TotalInAccount += transaction.AmountForeign;
                                //    //    if (transactAmount.ContainsKey(deductProducts.CurrencyCode))
                                //    //    {
                                //    //        transactAmount[deductProducts.CurrencyCode] += transaction.AmountForeign;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        transactAmount.Add(deductProducts.CurrencyCode, transaction.AmountForeign);
                                //    //    }
                                //    //    bool result_deductProductInventory = _productInventoriesModel.Update(deductProducts.ProductInventories[0].ID, deductProducts.ProductInventories[0]);

                                //    //    if (result_deductProductInventory)
                                //    //    {
                                //    //        Inventory inventory = new Inventory();
                                //    //        inventory.ProductId = deductProducts.ID;
                                //    //        inventory.Type = "Add";
                                //    //        inventory.Amount = transaction.AmountForeign;
                                //    //        inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                //    //        bool result_inventory = _inventoriesModel.Add(inventory);

                                //    //        if (result_inventory)
                                //    //        {
                                //    //            if (!deduct_inventory)
                                //    //            {
                                //    //                deduct_inventory = true;
                                //    //            }
                                //    //        }

                                //    //        if (!deduct_productInventory)
                                //    //        {
                                //    //            deduct_productInventory = true;
                                //    //        }
                                //    //    }

                                //    //    Product addProducts = _productsModel.FindCurrencyCode("SGD");
                                //    //    addProducts.ProductInventories[0].TotalInAccount -= transaction.AmountLocal;
                                //    //    if (transactAmount.ContainsKey(addProducts.CurrencyCode))
                                //    //    {
                                //    //        transactAmount[addProducts.CurrencyCode] -= transaction.AmountLocal;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        transactAmount.Add(addProducts.CurrencyCode, transaction.AmountLocal * -1);
                                //    //    }
                                //    //    bool result_addProductInventory = _productInventoriesModel.Update(addProducts.ProductInventories[0].ID, addProducts.ProductInventories[0]);

                                //    //    if (result_addProductInventory)
                                //    //    {
                                //    //        Inventory inventory = new Inventory();
                                //    //        inventory.ProductId = addProducts.ID;
                                //    //        inventory.Type = "Deduct";
                                //    //        inventory.Amount = transaction.AmountLocal;
                                //    //        inventory.Description = "Deleted Sale Transaction [MemoID: " + sales.MemoID + "]";
                                //    //        bool result_inventory = _inventoriesModel.Add(inventory);

                                //    //        if (result_inventory)
                                //    //        {
                                //    //            if (!add_inventory)
                                //    //            {
                                //    //                add_inventory = true;
                                //    //            }
                                //    //        }

                                //    //        if (!add_productInventory)
                                //    //        {
                                //    //            add_productInventory = true;
                                //    //        }
                                //    //    }
                                //    //}
                                //}
                                //#endregion

                                //#region Update End of Day Trade
                                //bool hasDeleteEndDayTradeTransactions = false;
                                //bool hasUpdateTrade = false;

                                //List<EndDayTradeTransaction> endDayTradeTransactions = _endDayTradeTransactionsModel.GetAll(sales.RemittanceOders.Select(e => e.ID).ToList(), sales.LastApprovalOn);

                                //if (endDayTradeTransactions.Count > 0)
                                //{
                                //    List<EndDayTrade> endDayTrades = endDayTradeTransactions.Select(e => e.EndDayTrade).Distinct().ToList();

                                //    //Delete existing records
                                //    foreach (EndDayTradeTransaction tradeTransaction in endDayTradeTransactions)
                                //    {
                                //        bool endDayTradeTransactions_delete = _endDayTradeTransactionsModel.Delete(tradeTransaction.ID);

                                //        if (endDayTradeTransactions_delete)
                                //        {
                                //            hasDeleteEndDayTradeTransactions = true;
                                //        }
                                //    }

                                //    //Recalculate End of Day Trades
                                //    if (hasDeleteEndDayTradeTransactions)
                                //    {
                                //        //Dictionary<int, decimal[]> currentClosings = new Dictionary<int, decimal[]>();

                                //        foreach (EndDayTrade trade in endDayTrades)
                                //        {
                                //            decimal totalAmountForeign = 0;
                                //            decimal totalAmountLocal = 0;
                                //            List<string> desciptions = new List<string>();

                                //            List<EndDayTradeTransaction> tradeTransactions = _endDayTradeTransactionsModel.GetAll(trade.ID);

                                //            string amountForeignFormat = GetDecimalFormat(trade.Products.Decimal);
                                //            string sgdFormat = GetDecimalFormat(sgdDp);

                                //            foreach (EndDayTradeTransaction transaction in tradeTransactions)
                                //            {
                                //                totalAmountForeign += transaction.AmountForeign;
                                //                totalAmountLocal += transaction.AmountLocal;
                                //                desciptions.Add(String.Format("{0}-{1}:{2}:{3}:{4}:{5}", transaction.SaleTransaction.Sales.MemoID, transaction.SaleTransaction.TransactionID, transaction.SaleTransaction.Products.Symbol + transaction.AmountForeign.ToString(amountForeignFormat), transaction.SaleTransaction.Rate.ToString(GetRateFormat(rateDP)), transaction.SaleTransaction.EncashmentRate != null ? Convert.ToDecimal(transaction.SaleTransaction.EncashmentRate).ToString(GetRateFormat(rateDP)) : "-", "SGD" + (transaction.AmountLocal * trade.Products.Unit).ToString(sgdFormat)));
                                //            }

                                //            EndDayTrade previousTrade = _endDayTradesModel.GetProductPreviousTrade(trade.CurrencyId, trade.LastActivationTime);

                                //            decimal openingBankAmount = trade.OpeningBankAmount;
                                //            decimal openingCashAmount = trade.OpeningCashAmount;
                                //            decimal openingForeignCurrencyBal = trade.OpeningForeignCurrencyBalance;
                                //            decimal openingAveragePurchaseCost = trade.OpeningAveragePurchaseCost;
                                //            decimal openingBalanceAtAveragePurchase = trade.OpeningBalanceAtAveragePurchase;
                                //            decimal openingProfitAmount = trade.OpeningProfitAmount;
                                //            decimal closingBankAmount = trade.ClosingBankAmount;
                                //            decimal closingCashAmount = trade.ClosingCashAmount;
                                //            decimal closingForeignCurrencyBal = trade.ClosingForeignCurrencyBalance;
                                //            decimal closingAveragePurchaseCost = trade.ClosingAveragePurchaseCost;
                                //            decimal closingBalanceAtAveragePurchase = trade.ClosingBalanceAtAveragePurchase;
                                //            decimal closingProfitAmount = trade.ClosingProfitAmount;
                                //            List<string> transactionDescription = new List<string>();

                                //            decimal foreignCurrencyBal = trade.ClosingForeignCurrencyBalance;
                                //            decimal averageRate = trade.ClosingAveragePurchaseCost;
                                //            decimal closingBal = trade.ClosingBalanceAtAveragePurchase;

                                //            if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                //            {
                                //                foreignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                //            }

                                //            if (previousTrade == null)
                                //            {
                                //                if (transactAmount.ContainsKey("SGD"))
                                //                {
                                //                    openingBankAmount += transactAmount["SGD"];
                                //                    openingCashAmount += transactAmount["SGD"];
                                //                }

                                //                if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                //                {
                                //                    openingForeignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                //                }

                                //                if (totalAmountForeign != 0)
                                //                {
                                //                    averageRate = totalAmountLocal / totalAmountForeign;
                                //                }

                                //                closingBal = foreignCurrencyBal * averageRate;

                                //                openingAveragePurchaseCost = averageRate;
                                //                openingBalanceAtAveragePurchase = closingBal;
                                //            }

                                //            if (transactAmount.ContainsKey("SGD"))
                                //            {
                                //                closingBankAmount += transactAmount["SGD"];
                                //                closingCashAmount += transactAmount["SGD"];
                                //            }

                                //            if (transactAmount.ContainsKey(trade.Products.CurrencyCode))
                                //            {
                                //                closingForeignCurrencyBal += transactAmount[trade.Products.CurrencyCode];
                                //            }

                                //            closingAveragePurchaseCost = averageRate;
                                //            closingBalanceAtAveragePurchase = closingBal;
                                //            closingProfitAmount = (openingForeignCurrencyBal * trade.CurrentSGDBuyRate) + closingBankAmount + closingCashAmount - foreignCurrencyBal * trade.CurrentSGDBuyRate - closingBal * trade.CurrentSGDBuyRate;

                                //            trade.OpeningBankAmount = openingBankAmount;
                                //            trade.OpeningCashAmount = openingCashAmount;
                                //            trade.OpeningForeignCurrencyBalance = openingForeignCurrencyBal;
                                //            trade.OpeningAveragePurchaseCost = openingAveragePurchaseCost;
                                //            trade.OpeningBalanceAtAveragePurchase = openingBalanceAtAveragePurchase;
                                //            trade.OpeningProfitAmount = openingProfitAmount;
                                //            trade.ClosingBankAmount = closingBankAmount;
                                //            trade.ClosingCashAmount = closingCashAmount;
                                //            trade.ClosingForeignCurrencyBalance = closingForeignCurrencyBal;
                                //            trade.ClosingAveragePurchaseCost = closingAveragePurchaseCost;
                                //            trade.ClosingBalanceAtAveragePurchase = closingBalanceAtAveragePurchase;
                                //            trade.ClosingProfitAmount = closingProfitAmount;
                                //            trade.Description = String.Join("|", desciptions);

                                //            bool trade_update = _endDayTradesModel.Update(trade.ID, trade);

                                //            if (trade_update)
                                //            {
                                //                hasUpdateTrade = true;

                                //                //Record for closings, to update the next opening if any
                                //                //currentClosings.Add(
                                //                //    trade.CurrencyId,
                                //                //    new decimal[7]
                                //                //    {
                                //                //        Convert.ToDecimal(trade.CurrentActivationTime.ToString("yyyyMMddHHmmss.fffffff")),
                                //                //        trade.ClosingBankAmount,
                                //                //        trade.ClosingCashAmount,
                                //                //        trade.ClosingForeignCurrencyBalance,
                                //                //        trade.ClosingAveragePurchaseCost,
                                //                //        trade.ClosingBalanceAtAveragePurchase,
                                //                //        trade.ClosingProfitAmount
                                //                //    });
                                //            }
                                //        }

                                //        //Update next end day trade if any
                                //        //foreach (KeyValuePair<int, decimal[]> key in currentClosings)
                                //        //{
                                //        //    int currencyId = key.Key;
                                //        //    DateTime currentActivation = Convert.ToDateTime(String.Format("{0}/{1}/{2} {3}:{4}:{5}", key.Value[0].ToString().Substring(0, 4), key.Value[0].ToString().Substring(4, 2), key.Value[0].ToString().Substring(6, 2), key.Value[0].ToString().Substring(8, 2), key.Value[0].ToString().Substring(10)));

                                //        //    EndDayTrade nextTrade = _endDayTradesModel.GetProductNextTrade(currencyId, currentActivation);

                                //        //    while (nextTrade != null)
                                //        //    {

                                //        //        currentActivation = nextTrade.CurrentActivationTime;

                                //        //        nextTrade = _endDayTradesModel.GetProductNextTrade(currencyId, currentActivation);
                                //        //    }
                                //        //}
                                //    }
                                //}
                                //#endregion

                                //if (add_productInventory)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "ProductInventories";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Product Inventories [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                //if (add_inventory)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "Inventories";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Added Inventories [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                //if (deduct_productInventory)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "ProductInventories";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Product Inventories [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                //if (deduct_inventory)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "Inventories";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deducted Inventories [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                //if (hasDeleteEndDayTradeTransactions)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "EndDayTradeTransactions";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted End Day Trade Transactions [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                //if (hasUpdateTrade)
                                //{
                                //    userid = Convert.ToInt32(Session["UserId"]);
                                //    tableAffected = "EndDayTrade";
                                //    description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated End Day Trades [" + sales.MemoID + "]";

                                //    bool transaction_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                                //}

                                TempData.Add("Result", "success|" + sales.MemoID + " has been successfully approved and cancelled!");
                            }
                            else
                            {
                                TempData.Add("Result", "danger|An error occured while approving delete sale record!");
                            }
                        }
                        else
                        {
                            sales.Status = "Completed";

                            bool result = _remittancesalesModel.Update(sales.ID, sales);

                            if (result)
                            {
                                int userid = Convert.ToInt32(Session["UserId"]);
                                string tableAffected = "RemittanceSales";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Rejected Pending Delete Remittance Sale [" + sales.MemoID + "]";

                                bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                TempData.Add("Result", "success|" + sales.MemoID + " has been successfully rejected and returned to completed state!");
                            }
                            else
                            {
                                TempData.Add("Result", "danger|An error occured while rejecting delete sale record!");
                            }
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|You have no access to delete this sale!");
                    }
                }
                else
                {
                    TempData.Add("Result", "error|Cannot delete sale at the moment!");
                }
            }
            else
            {
                TempData.Add("Result", "error|Memo ID not found!");
            }

            return RedirectToAction("Listing");
        }

        public ActionResult UpdateRemittanceSale(int id)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);
            List<RemittanceOrderData> remittanceOrderDatas = new List<RemittanceOrderData>();
            var remittanceOrders = sales.RemittanceOders;
            string updateResult = "";

            string sgdFormat = GetDecimalFormat(sgdDp);
            string rateFormat = GetDecimalFormat(rateDP);

            if (sales != null)
            {
                ViewData["Sale"] = sales;
                ViewData["SaleId"] = sales.ID;
                ViewData["MemoID"] = sales.MemoID;
                ViewData["TransactionType"] = "Remittance";

                string userRole = Session["UserRole"].ToString();
                string[] userRoleList = userRole.Split(',');

                if (sales.Status == "Pending Dealer")
                {
                    //if (Array.IndexOf(userRoleList, "Finance") >= 0)
                    if (Array.IndexOf(userRoleList, "Dealer") >= 0)
                    {
                        //ViewData["CollectionDate"] = Convert.ToDateTime(sales.CollectionDate).ToString("dd/MM/yyyy dddd");

                        //Dropdown[] collectionTimeDDL = CollectionTimeDDL();
                        //ViewData["CollectionTimeDropdown"] = new SelectList(collectionTimeDDL, "val", "name", sales.CollectionTime);

                        //ViewData["BankReferenceNumber"] = "";
                        //if (!string.IsNullOrEmpty(sales.BankTransferNo))
                        //{
                        //    ViewData["BankReferenceNumber"] = sales.BankTransferNo;
                        //}

                        //if (!string.IsNullOrEmpty(sales.LocalPaymentMode))
                        //{
                        //    if (sales.LocalPaymentMode.Contains("Bank Transfer"))
                        //    {
                        //        ViewData["BankTransfer"] = true;
                        //    }
                        //}

                        //ViewData["Remarks"] = !string.IsNullOrEmpty(sales.Remarks) ? sales.Remarks : "";

                        return View("PendingDealer");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status.Contains("Pending Accounts"))
                {
                    //if (Array.IndexOf(userRoleList, "Inventory") >= 0)
                    if (Array.IndexOf(userRoleList, "Finance") >= 0)
                    {
                        //foreach (var data in remittanceOrders)
                        //{
                        //    string paymentModeVal = data.PayPaymentType;
                        //    Dropdown[] paymentMode = PaymentModeDDL(Convert.ToInt32(data.PayCurrency));
                        //    SelectList paymentModeDDL = new SelectList(paymentMode, "val", "name", selectedValue: paymentModeVal);
                        //    Dropdown[] paymentBankDDL = PayBankDDL();
                        //    var depositAccDDL = new SelectList(paymentBankDDL, "val", "name", data.PayDepositAccount);
                        //    string disabledCheque = "disabled";
                        //    string disabledBankTranfer = "disabled";
                        //    string disabledDepositAcc = "disabled";
                        //    if (paymentModeVal == "2")
                        //    {
                        //        disabledCheque = "";
                        //        disabledDepositAcc = "";
                        //    }
                        //    else if (paymentModeVal == "3")
                        //    {
                        //        disabledBankTranfer = "";
                        //        disabledDepositAcc = "";
                        //    }

                        //    string chequeNo = "";
                        //    if (!string.IsNullOrEmpty(data.ChequeNo))
                        //    {
                        //        chequeNo = data.ChequeNo;
                        //    }

                        //    string bankTransferNo = "";
                        //    if (!string.IsNullOrEmpty(data.BankTransferNo))
                        //    {
                        //        bankTransferNo = data.BankTransferNo;
                        //    }
                        //    var remittanceOrder = new RemittanceOrderData()
                        //    {
                        //        ID = data.ID,
                        //        transactionID = data.TransactionID,
                        //        PayPaymentType = data.PayPaymentType,
                        //        PayPaymentModeDDL = paymentModeDDL,
                        //        DepositAccountDDL = depositAccDDL,
                        //        PayDepositAccount = data.PayDepositAccount.ToString(),
                        //        DisabledChequeNo = disabledCheque,
                        //        DisabledBankTransferNo = disabledBankTranfer,
                        //        DisabledDepositAccount = disabledDepositAcc,
                        //        ChequeNo = chequeNo,
                        //        BankTransferNo = bankTransferNo,
                        //        PayCurrency = data.PayCurrencyDecimal.CurrencyCode,
                        //        PayAmount = data.PayAmount.ToString(GetDecimalFormat(data.PayCurrencyDecimal.ProductDecimal)),
                        //        GetCurrency = data.GetCurrencyDecimal.CurrencyCode,
                        //        GetAmount = data.GetAmount.ToString(GetDecimalFormat(data.GetCurrencyDecimal.ProductDecimal)),
                        //        Rate = data.Rate.ToString(GetRateFormat(8)),
                        //        transactionFees = data.Fee.ToString(GetRateFormat(8))
                        //    };
                        //    remittanceOrderDatas.Add(remittanceOrder);
                        //}
                        ViewData["CustomerRemarks"] = "";
                        if (!string.IsNullOrEmpty(sales.CustomerRemarks))
                        {
                            ViewData["CustomerRemarks"] = sales.CustomerRemarks;
                        }

                        //ViewData["SaleRemarks"] = "";
                        //if (!string.IsNullOrEmpty(sales.Remarks))
                        //{
                        //    ViewData["SaleRemarks"] = sales.Remarks;
                        //}
                        //ViewData["Remarks"] = !string.IsNullOrEmpty(sales.Remarks) ? sales.Remarks : "";

                        ViewData["SaleTransactions"] = sales.RemittanceOders;
                        ViewData["SaleTransactionsData"] = remittanceOrderDatas;

                        ViewData["SaleTransactionType"] = "Remittances";


                        return View("PendingAccountRemittance");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending Customer")
                {

                    //if (Array.IndexOf(userRoleList, "Cashier") >= 0)
                    if (Array.IndexOf(userRoleList, "Dealer") >= 0)
                    {
                        //foreach (var data in remittanceOrders)
                        //{
                        //    string paymentModeVal = data.PayPaymentType;
                        //    Dropdown[] paymentMode = PaymentModeDDL(Convert.ToInt32(data.PayCurrency));
                        //    SelectList paymentModeDDL = new SelectList(paymentMode, "val", "name", selectedValue: paymentModeVal);
                        //    Dropdown[] paymentBankDDL = PayBankDDL();
                        //    var depositAccDDL = new SelectList(paymentBankDDL, "val", "name", data.PayDepositAccount);
                        //    string disabledCheque = "disabled";
                        //    string disabledBankTranfer = "disabled";
                        //    string disabledDepositAcc = "disabled";
                        //    if (paymentModeVal == "2")
                        //    {
                        //        disabledCheque = "";
                        //        disabledDepositAcc = "";
                        //    }
                        //    else if (paymentModeVal == "3")
                        //    {
                        //        disabledBankTranfer = "";
                        //        disabledDepositAcc = "";
                        //    }

                        //    string chequeNo = "";
                        //    if (!string.IsNullOrEmpty(data.ChequeNo))
                        //    {
                        //        chequeNo = data.ChequeNo;
                        //    }

                        //    string bankTransferNo = "";
                        //    if (!string.IsNullOrEmpty(data.BankTransferNo))
                        //    {
                        //        bankTransferNo = data.BankTransferNo;
                        //    }
                        //    var remittanceOrder = new RemittanceOrderData()
                        //    {
                        //        ID = data.ID,
                        //        transactionID = data.TransactionID,
                        //        PayPaymentType = data.PayPaymentType,
                        //        PayPaymentModeDDL = paymentModeDDL,
                        //        DepositAccountDDL = depositAccDDL,
                        //        PayDepositAccount = data.PayDepositAccount.ToString(),
                        //        DisabledChequeNo = disabledCheque,
                        //        DisabledBankTransferNo = disabledBankTranfer,
                        //        DisabledDepositAccount = disabledDepositAcc,
                        //        ChequeNo = chequeNo,
                        //        BankTransferNo = bankTransferNo,
                        //        PayCurrency = data.PayCurrencyDecimal.CurrencyCode,
                        //        PayAmount = data.PayAmount.ToString(GetDecimalFormat(data.PayCurrencyDecimal.ProductDecimal)),
                        //        GetCurrency = data.GetCurrencyDecimal.CurrencyCode,
                        //        GetAmount = data.GetAmount.ToString(GetDecimalFormat(data.GetCurrencyDecimal.ProductDecimal)),
                        //        Rate = data.Rate.ToString(GetRateFormat(8)),
                        //        transactionFees = data.Fee.ToString(GetRateFormat(2))
                        //    };
                        //    remittanceOrderDatas.Add(remittanceOrder);
                        //}
                        ViewData["CustomerRemarks"] = "";
                        if (!string.IsNullOrEmpty(sales.CustomerRemarks))
                        {
                            ViewData["CustomerRemarks"] = sales.CustomerRemarks;
                        }

                        //ViewData["SaleRemarks"] = "";
                        //if (!string.IsNullOrEmpty(sales.Remarks))
                        //{
                        //    ViewData["SaleRemarks"] = sales.Remarks;
                        //}
                        //ViewData["Remarks"] = !string.IsNullOrEmpty(sales.Remarks) ? sales.Remarks : "";

                        ViewData["SaleTransactions"] = sales.RemittanceOders;
                        ViewData["SaleTransactionsData"] = remittanceOrderDatas;

                        ViewData["SaleTransactionType"] = "Remittances";

                        return View("PendingCustomer");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending GM Approval (Rejected)")
                {
                    //if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        ViewData["Sales"] = sales;

                        return View("DisapprovedRemittance");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else if (sales.Status == "Pending Delete GM Approval")
                {
                    //if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                    {
                        ViewData["Sales"] = sales;

                        return View("GMDeleteSale");
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                        TempData.Add("Result", "danger|Memo ID not ready!");
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                    TempData.Add("Result", "danger|Memo ID not ready!");
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        //POST: PendingDealer
        [HttpPost]
        public ActionResult PendingDealer(int id, FormCollection form)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending Dealer")
                {
                    //sales.Remarks = form["Remarks"];
                    sales.Status = "Pending Accounts (Check Funds)";
                    sales.LastApprovalOn = DateTime.Now;

                    bool result = _remittancesalesModel.Update(sales.ID, sales);

                    if (result)
                    {
                        int userid = Convert.ToInt32(Session["UserId"]);
                        string tableAffected = "RemittanceSales";
                        string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "]";

                        bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                        if (sale_log)
                        {
                            //Add Approval History
                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Approve", "[" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "] to status (" + sales.Status + ")");
                        }

                        TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                        updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";
                    }
                    else
                    {
                        updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //POST: PendingAccountsRemittance
        [HttpPost]
        public ActionResult PendingAccountRemittance(int id, FormCollection form)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);
            var remittanceOrderList = sales.RemittanceOders;

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status.Contains("Pending Accounts"))
                {
                    List<string[]> modelErrors = new List<string[]>();

                    //if (string.IsNullOrEmpty(form["CollectionDate"]))
                    //{
                    //    string[] error = new string[2];
                    //    error[0] = "CollectionDate";
                    //    error[1] = "Collection Date is required!";
                    //    modelErrors.Add(error);
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        sales.CollectionDate = Convert.ToDateTime(form["CollectionDate"]);
                    //    }
                    //    catch
                    //    {
                    //        string[] error = new string[2];
                    //        error[0] = "CollectionDate";
                    //        error[1] = "Collection Date is not valid!";
                    //        modelErrors.Add(error);
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(form["CollectionTime"]))
                    //{
                    //    string[] error = new string[2];
                    //    error[0] = "CollectionTime";
                    //    error[1] = "Collection Time is required!";
                    //    modelErrors.Add(error);
                    //}
                    //var transactionKey = form.AllKeys.Where(e => e.Contains("Transaction_PayPaymentType_")).ToList();
                    //if (transactionKey.Count > 0)
                    //{
                    //    foreach (var data in transactionKey)
                    //    {
                    //        string rowId = data.Split('_')[2];
                    //        if (data.Contains("Transaction_PayPaymentType_"))
                    //        {
                    //            if (form["Transaction_PayPaymentType_" + rowId].ToString() == "2")
                    //            {
                    //                if (string.IsNullOrEmpty(form["Transaction_ChequeNo_" + rowId]))
                    //                {
                    //                    string[] error = new string[2];
                    //                    error[0] = "Transaction_ChequeNo_" + rowId;
                    //                    error[1] = "Cheque No is required!";
                    //                    modelErrors.Add(error);
                    //                }
                    //                else
                    //                {
                    //                    if (string.IsNullOrEmpty(form["Transaction_DepositAccount_" + rowId]))
                    //                    {
                    //                        string[] error = new string[2];
                    //                        error[0] = "Transaction_DepositAccount_" + rowId;
                    //                        error[1] = "Deposit Account is required!";
                    //                        modelErrors.Add(error);
                    //                    }
                    //                }
                    //            }
                    //            else if (form["Transaction_PayPaymentType_" + rowId].ToString() == "3")
                    //            {
                    //                if (string.IsNullOrEmpty(form["Transaction_BankTransferNo_" + rowId]))
                    //                {
                    //                    string[] error = new string[2];
                    //                    error[0] = "Transaction_BankTransferNo_" + rowId;
                    //                    error[1] = "Bank Transfer No is required!";
                    //                    modelErrors.Add(error);
                    //                }
                    //                else
                    //                {
                    //                    if (string.IsNullOrEmpty(form["Transaction_DepositAccount_" + rowId]))
                    //                    {
                    //                        string[] error = new string[2];
                    //                        error[0] = "Transaction_DepositAccount_" + rowId;
                    //                        error[1] = "Deposit Account is required!";
                    //                        modelErrors.Add(error);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}



                    if (modelErrors.Count == 0)
                    {
                        //sales.Remarks = form["Remarks"];
                        if (sales.Status == "Pending Accounts (Check Funds)")
                        {
                            sales.Status = "Pending Accounts (Check Transaction)";
                        }
                        else
                        {
                            sales.Status = "Pending Customer";
                        }
                        sales.LastApprovalOn = DateTime.Now;

                        bool result = _remittancesalesModel.Update(sales.ID, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "RemittanceSales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Approve", "[" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "] to status (" + sales.Status + ")");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";

                            //if (transactionKey.Count > 0)
                            //{
                            //    foreach (var data in transactionKey)
                            //    {
                            //        string rowId = data.Split('_')[2];
                            //        if (data.Contains("Transaction_PayPaymentType_"))
                            //        {
                            //            if (form["Transaction_PayPaymentType_" + rowId] == "2")
                            //            {
                            //                if (!string.IsNullOrEmpty(form["Transaction_ChequeNo_" + rowId]))
                            //                {
                            //                    var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                    var oldtype = ro.PayPaymentType;
                            //                    var oldCheque = ro.ChequeNo;
                            //                    var oldDepositAcc = ro.PayDepositAccount;
                            //                    ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                    ro.ChequeNo = form["Transaction_ChequeNo_" + rowId].ToString();
                            //                    ro.PayDepositAccount = Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]);
                            //                    ro.BankTransferNo = null;
                            //                    bool isSame = true;
                            //                    if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldCheque != form["Transaction_ChequeNo_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldDepositAcc != Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]))
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    bool updateRO = false;
                            //                    if (!isSame)
                            //                    {
                            //                        updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                    }
                            //                    if (updateRO)
                            //                    {
                            //                        string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                        bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                        if (sale_log2)
                            //                        {
                            //                            //Add Approval History
                            //                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            else if (form["Transaction_PayPaymentType_" + rowId] == "3")
                            //            {
                            //                if (!string.IsNullOrEmpty(form["Transaction_BankTransferNo_" + rowId]))
                            //                {
                            //                    var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                    var oldtype = ro.PayPaymentType;
                            //                    var oldBankTransfer = ro.BankTransferNo;
                            //                    var oldDepositAcc = ro.PayDepositAccount;
                            //                    ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                    ro.BankTransferNo = form["Transaction_BankTransferNo_" + rowId].ToString();
                            //                    ro.PayDepositAccount = Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]);
                            //                    ro.ChequeNo = null;
                            //                    bool isSame = true;
                            //                    if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldBankTransfer != form["Transaction_BankTransferNo_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldDepositAcc != Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]))
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    bool updateRO = false;
                            //                    if (!isSame)
                            //                    {
                            //                        updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                    }
                            //                    if (updateRO)
                            //                    {
                            //                        string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                        bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                        if (sale_log2)
                            //                        {
                            //                            //Add Approval History
                            //                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            else if (form["Transaction_PayPaymentType_" + rowId] == "1")
                            //            {
                            //                    var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                    var oldtype = ro.PayPaymentType;
                            //                    ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                    ro.ChequeNo = null;
                            //                    ro.BankTransferNo = null;
                            //                    ro.PayDepositAccount = 0;
                            //                    bool isSame = true;
                            //                    if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    bool updateRO = false;
                            //                    if (!isSame)
                            //                    {
                            //                        updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                    }
                            //                    if (updateRO)
                            //                    {
                            //                        string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                        bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                        if (sale_log2)
                            //                        {
                            //                            //Add Approval History
                            //                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                        }
                            //                    }                                            
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
        }

        //POST: PendingCustomer
        [HttpPost]
        public ActionResult PendingCustomer(int id, FormCollection form)
        {
            Remittances sales = _remittancesalesModel.GetSingle(id);
            var remittanceOrderList = sales.RemittanceOders;

            string updateResult = "";

            if (sales != null)
            {
                if (sales.Status == "Pending Customer")
                {
                    List<string[]> modelErrors = new List<string[]>();

                    //if (string.IsNullOrEmpty(form["CollectionDate"]))
                    //{
                    //    string[] error = new string[2];
                    //    error[0] = "CollectionDate";
                    //    error[1] = "Collection Date is required!";
                    //    modelErrors.Add(error);
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        sales.CollectionDate = Convert.ToDateTime(form["CollectionDate"]);
                    //    }
                    //    catch
                    //    {
                    //        string[] error = new string[2];
                    //        error[0] = "CollectionDate";
                    //        error[1] = "Collection Date is not valid!";
                    //        modelErrors.Add(error);
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(form["CollectionTime"]))
                    //{
                    //    string[] error = new string[2];
                    //    error[0] = "CollectionTime";
                    //    error[1] = "Collection Time is required!";
                    //    modelErrors.Add(error);
                    //}
                    //var transactionKey = form.AllKeys.Where(e => e.Contains("Transaction_PayPaymentType_")).ToList();
                    //if (transactionKey.Count > 0)
                    //{
                    //    foreach (var data in transactionKey)
                    //    {
                    //        string rowId = data.Split('_')[2];
                    //        if (data.Contains("Transaction_PayPaymentType_"))
                    //        {
                    //            if (form["Transaction_PayPaymentType_" + rowId].ToString() == "2")
                    //            {
                    //                if (string.IsNullOrEmpty(form["Transaction_ChequeNo_" + rowId]))
                    //                {
                    //                    string[] error = new string[2];
                    //                    error[0] = "Transaction_ChequeNo_" + rowId;
                    //                    error[1] = "Cheque No is required!";
                    //                    modelErrors.Add(error);
                    //                }
                    //                else
                    //                {
                    //                    if (string.IsNullOrEmpty(form["Transaction_DepositAccount_" + rowId]))
                    //                    {
                    //                        string[] error = new string[2];
                    //                        error[0] = "Transaction_DepositAccount_" + rowId;
                    //                        error[1] = "Deposit Account is required!";
                    //                        modelErrors.Add(error);
                    //                    }
                    //                }
                    //            }
                    //            else if (form["Transaction_PayPaymentType_" + rowId].ToString() == "3")
                    //            {
                    //                if (string.IsNullOrEmpty(form["Transaction_BankTransferNo_" + rowId]))
                    //                {
                    //                    string[] error = new string[2];
                    //                    error[0] = "Transaction_BankTransferNo_" + rowId;
                    //                    error[1] = "Bank Transfer No is required!";
                    //                    modelErrors.Add(error);
                    //                }
                    //                else
                    //                {
                    //                    if (string.IsNullOrEmpty(form["Transaction_DepositAccount_" + rowId]))
                    //                    {
                    //                        string[] error = new string[2];
                    //                        error[0] = "Transaction_DepositAccount_" + rowId;
                    //                        error[1] = "Deposit Account is required!";
                    //                        modelErrors.Add(error);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}



                    if (modelErrors.Count == 0)
                    {
                        //sales.Remarks = form["Remarks"];
                        sales.Status = "Completed";
                        sales.LastApprovalOn = DateTime.Now;

                        bool result = _remittancesalesModel.Update(sales.ID, sales);

                        if (result)
                        {
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "RemittanceSales";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "]";

                            bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            if (sale_log)
                            {
                                //Add Approval History
                                bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Approve", "[" + Session["Username"].ToString() + "] Approved Remittance Sale [" + sales.MemoID + "] to status (" + sales.Status + ")");
                            }

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully updated!");
                            updateResult = "{\"result\":\"success\", \"msg\":\"" + sales.MemoID + " has been successfully updated!\"}";

                            //if (transactionKey.Count > 0)
                            //{
                            //    foreach (var data in transactionKey)
                            //    {
                            //        string rowId = data.Split('_')[2];
                            //        if (data.Contains("Transaction_PayPaymentType_"))
                            //        {
                            //            if (form["Transaction_PayPaymentType_" + rowId] == "2")
                            //            {
                            //                if (!string.IsNullOrEmpty(form["Transaction_ChequeNo_" + rowId]))
                            //                {
                            //                    var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                    var oldtype = ro.PayPaymentType;
                            //                    var oldCheque = ro.ChequeNo;
                            //                    var oldDepositAcc = ro.PayDepositAccount;
                            //                    ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                    ro.ChequeNo = form["Transaction_ChequeNo_" + rowId].ToString();
                            //                    ro.PayDepositAccount = Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]);
                            //                    ro.BankTransferNo = null;
                            //                    bool isSame = true;
                            //                    if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldCheque != form["Transaction_ChequeNo_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldDepositAcc != Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]))
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    bool updateRO = false;
                            //                    if (!isSame)
                            //                    {
                            //                        updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                    }
                            //                    if (updateRO)
                            //                    {
                            //                        string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                        bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                        if (sale_log2)
                            //                        {
                            //                            //Add Approval History
                            //                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            else if (form["Transaction_PayPaymentType_" + rowId] == "3")
                            //            {
                            //                if (!string.IsNullOrEmpty(form["Transaction_BankTransferNo_" + rowId]))
                            //                {
                            //                    var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                    var oldtype = ro.PayPaymentType;
                            //                    var oldBankTransfer = ro.BankTransferNo;
                            //                    var oldDepositAcc = ro.PayDepositAccount;
                            //                    ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                    ro.BankTransferNo = form["Transaction_BankTransferNo_" + rowId].ToString();
                            //                    ro.PayDepositAccount = Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]);
                            //                    ro.ChequeNo = null;
                            //                    bool isSame = true;
                            //                    if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldBankTransfer != form["Transaction_BankTransferNo_" + rowId].ToString())
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    else if (oldDepositAcc != Convert.ToInt32(form["Transaction_DepositAccount_" + rowId]))
                            //                    {
                            //                        isSame = false;
                            //                    }
                            //                    bool updateRO = false;
                            //                    if (!isSame)
                            //                    {
                            //                        updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                    }
                            //                    if (updateRO)
                            //                    {
                            //                        string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                        bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                        if (sale_log2)
                            //                        {
                            //                            //Add Approval History
                            //                            bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            else if (form["Transaction_PayPaymentType_" + rowId] == "1")
                            //            {
                            //                var ro = remittanceOrderList.Where(e => e.TransactionID == rowId).FirstOrDefault();
                            //                var oldtype = ro.PayPaymentType;
                            //                ro.PayPaymentType = form["Transaction_PayPaymentType_" + rowId].ToString();
                            //                ro.ChequeNo = null;
                            //                ro.BankTransferNo = null;
                            //                bool isSame = true;
                            //                if (oldtype != form["Transaction_PayPaymentType_" + rowId].ToString())
                            //                {
                            //                    isSame = false;
                            //                }
                            //                bool updateRO = false;
                            //                if (!isSame)
                            //                {
                            //                    updateRO = _remittanceordersModel.Update(ro.ID, ro);
                            //                }
                            //                if (updateRO)
                            //                {
                            //                    string description2 = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Remittance Sale Order (" + ro.TransactionID + ") [" + sales.MemoID + "]";

                            //                    bool sale_log2 = AuditLogHelper.WriteAuditLog(userid, tableAffected, description2);

                            //                    if (sale_log2)
                            //                    {
                            //                        //Add Approval History
                            //                        bool approval_history = _approvalHistorysModel.Add("RemittanceSale", sales.ID, userid, Session["Username"].ToString(), Session["UserRole"].ToString(), "Update", "[" + Session["Username"].ToString() + "] Updated Remittance Sale [" + sales.MemoID + "]");
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            updateResult = "{\"result\":\"error\", \"msg\":\"Error while updating sale record!\"}";
                        }
                    }
                    else
                    {
                        string msg = "";

                        foreach (string[] error in modelErrors)
                        {
                            msg += "{\"id\":\"" + error[0] + "\",\"error\":\"" + error[1] + "\"},";
                        }

                        updateResult = "{\"result\":\"form-error\", \"msg\":[" + msg.Substring(0, msg.Length - 1) + "]}";
                    }
                }
                else
                {
                    updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not ready!\"}";
                }
            }
            else
            {
                updateResult = "{\"result\":\"error\", \"msg\":\"Memo ID not found!\"}";
            }

            return Json(updateResult);
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
            string format = "#,##0";

            switch (dp)
            {
                case 1:
                    format += ".#";
                    break;
                case 2:
                    format += ".##";
                    break;
                case 3:
                    format += ".###";
                    break;
                case 4:
                    format += ".####";
                    break;
                case 5:
                    format += ".#####";
                    break;
                case 6:
                    format += ".######";
                    break;
                case 7:
                    format += ".#######";
                    break;
                case 8:
                    format += ".########";
                    break;
                default:
                    break;
            }

            return format;
        }

        //PaymentMode Dropdown
        public Dropdown[] PaymentModeDDL(int selectedProductId)
        {
            RemittanceProducts selectedProduct = _remittanceproductsModel.GetSingle(selectedProductId);
            var bankMode = new List<PaymentModeLists>();
            using (var context = new DataAccess.GreatEastForex())
            {
                bankMode = context.PaymentModeLists.Where(e => e.IsDeleted == 0).ToList();
            }
            string[] paymentMode = null;

            if (selectedProduct != null)
            {
                paymentMode = selectedProduct.PaymentModeAllowed.Split(',');
            }

            if (paymentMode != null)
            {
                Dropdown[] ddl = new Dropdown[paymentMode.Length];

                int count = 0;

                foreach (string mode in paymentMode)
                {
                    var modeDetails = bankMode.Where(e => e.ID == Convert.ToInt32(mode)).FirstOrDefault();
                    ddl[count] = new Dropdown { name = modeDetails.Name, val = modeDetails.ID.ToString() };
                    count++;
                }

                return ddl;
            }
            else
            {
                Dropdown[] ddl = new Dropdown[1];
                ddl[0] = new Dropdown { name = "Pending", val = "Pending" };
                return ddl;
            }
        }

        public Dropdown[] PayBankDDL(string selectedBank = "")
        {
            List<PayBankLists> payBankLists = new List<PayBankLists>();
            using (var context = new DataAccess.GreatEastForex())
            {
                payBankLists = context.PayBankLists.Where(e => e.IsDeleted == 0).ToList();
            }

            Dropdown[] ddl = new Dropdown[payBankLists.Count + 1];

            int count = 1;

            foreach (var bank in payBankLists)
            {
                ddl[count] = new Dropdown { name = bank.BankName, val = bank.ID.ToString() };

                count++;
            }

            return ddl;
        }
    }
}