using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [RedirectingActionScanIncoming]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ScanIncomingController : ControllerBase
    {
        private IProductRepository _productsModel;
        private IScanIncomingRepository _incomingsModel;
        private IScanOutgoingRepository _outgoingsModel;
        private ISaleRepository _salesModel;
        private ISettingRepository _settingsModel;
        private int sgdDp = 2;
        private int rateDP = 6;

        public ScanIncomingController()
            : this(new ProductRepository(), new ScanIncomingRepository(),new ScanOutgoingRepository(), new SaleRepository(), new SettingRepository())
        {

        }

        public ScanIncomingController(IProductRepository productsModel, IScanIncomingRepository incomingsModel, IScanOutgoingRepository outgoingsModel, ISaleRepository salesModel, ISettingRepository settingsModel)
        {
            _productsModel = productsModel;
            _incomingsModel = incomingsModel;
            _outgoingsModel = outgoingsModel;
            _salesModel = salesModel;
            _settingsModel = settingsModel;
            Product sgd = _productsModel.FindCurrencyCode("SGD");
            sgdDp = sgd.Decimal;
            ViewData["SGDDP"] = GetDecimalFormat(sgdDp);
            ViewData["RateDP"] = GetDecimalFormat(rateDP);
        }

        // GET: ScanIncoming
        public ActionResult Index()
        {
            if (TempData["SearchKeyword"] != null)
            {
                TempData.Remove("SearchKeyword");
            }

            return RedirectToAction("Listing");
        }

        //GET: Listing
        public ActionResult Listing()
        {

            int userId = Convert.ToInt32(Session["UserId"]);

            IList<ScanIncoming> incoming = _incomingsModel.GetAllScanBy(userId);
            ViewData["Incoming"] = incoming;

            string prefixMemo = _settingsModel.GetCodeValue("PREFIX_SALE");
            ViewData["MemoIDPrefix"] = prefixMemo;

            string prefixBarcode = _settingsModel.GetCodeValue("PREFIX_BARCODE");
            ViewData["BarcodePrefix"] = prefixBarcode;

            ViewData["MemoID"] = "";

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Listing
        [HttpPost]
        public ActionResult Listing(FormCollection form)
        {
            string errorMsg = "";

            int userid = Convert.ToInt32(Session["UserId"]);

            if (string.IsNullOrEmpty(form["MemoID"]))
            {
                ModelState.AddModelError("MemoID", "Memo ID is required!");
                errorMsg = "Memo ID is required!";
            }
            else
            {
                ScanIncoming checkUnique = _incomingsModel.FindMemoID(form["MemoID"].Trim());

                if (checkUnique != null)
                {
                    if (checkUnique.Status != "Confirmed")
                    {
                        if (checkUnique.ScanById != userid)
                        {
                            ModelState.AddModelError("MemoID", "Memo ID Scan by others!");
                            errorMsg = "Memo ID Scan by others!";
                        }
                        else
                        {
                            ModelState.AddModelError("MemoID", "Memo ID already existed!");
                            errorMsg = "Memo ID already existed!";
                        }

                       
                    }
                }
            }

            if (ModelState.IsValid)
            {
                string memoID = form["MemoID"].Trim();

                Sale sales = _salesModel.GetSingle(memoID);

                if (sales != null)
                {
                    if (sales.Status.Contains("Pending Incoming Delivery by "))
                    {

                        ScanOutgoing outgoingItem = _outgoingsModel.GetSingleScanBy(sales.ID, userid);

                        if(outgoingItem != null)
                        {
                            ScanIncoming scanIncoming = new ScanIncoming();
                            scanIncoming.SaleId = sales.ID;
                            scanIncoming.Status = sales.Status;
                            scanIncoming.ScanById = userid;

                            bool result = _incomingsModel.Add(scanIncoming);

                            if (result)
                            {

                                string tableAffected = "ScanIncomings";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Incoming Delivery";

                                bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                TempData.Add("Result", "success|" + sales.MemoID + " has been successfully created!");

                                return RedirectToAction("Listing");
                            }
                            else
                            {
                                TempData.Add("Result", "danger|An error occured while saving incoming delivery!");
                            }
                        }
                        else
                        {
                            TempData.Add("Result", "danger|This Memo ID is scanned by others!");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|Memo ID not ready for delivery!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|Memo ID not found!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|" + errorMsg);
            }

            IList<ScanIncoming> incoming = _incomingsModel.GetAllScanBy(userid);
            ViewData["Incoming"] = incoming;

            string prefixMemo = _settingsModel.GetCodeValue("PREFIX_SALE");
            ViewData["MemoIDPrefix"] = prefixMemo;

            string prefixBarcode = _settingsModel.GetCodeValue("PREFIX_BARCODE");
            ViewData["BarcodePrefix"] = prefixBarcode;

            ViewData["MemoID"] = form["MemoID"];

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ConfirmIncomingDelivery
        public ActionResult ConfirmIncomingDelivery()
        {

            int userid = Convert.ToInt32(Session["UserId"]);

            IList<ScanIncoming> incoming = _incomingsModel.GetAllScanBy(userid);

            if (incoming.Count > 0)
            {
                bool add_incoming = false;
                bool add_sale = false;
                bool isValid = true;

                foreach (ScanIncoming data in incoming)
                {
                    if (data.ScanById != userid)
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    foreach (ScanIncoming data in incoming)
                    {
                        bool result = _incomingsModel.ConfirmIncoming(data.ID);

                        if (result)
                        {
                            if (!add_incoming)
                            {
                                add_incoming = true;
                            }

                            bool result_sale = _salesModel.UpdateStatus(data.SaleId, "Pending Cashier");

                            if (result_sale)
                            {
                                if (!add_sale)
                                {
                                    add_sale = true;
                                }
                            }
                        }
                    }

                    if (add_incoming)
                    {
                        string tableAffected = "ScanIncomings";
                        string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Confirmed Incoming Delivery";

                        bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    if (add_sale)
                    {
                        string tableAffected = "Sales";
                        string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Sales Status [Pending Cashier]";

                        bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }


                    if (add_incoming && add_sale)
                    {
                        TempData.Add("Result", "success|You have been successfully completed delivery of these sales!");
                    }
                    else
                    {
                        TempData.Add("Result", "danger|An error occured while completing incoming delivery!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|The Sales item is not scan by the same person!");
                }

            }
            else
            {
                TempData.Add("Result", "danger|No incoming delivery available!");
            }

            return RedirectToAction("Listing");
        }

        //GET: Delete
        public ActionResult Delete(int id)
        {
            ScanIncoming incoming = _incomingsModel.GetSingle(id);

            if (incoming != null)
            {
                bool result = _incomingsModel.Delete(incoming.ID);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "ScanIncomings";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted Incoming Delivery";

                    bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|" + incoming.Sales.MemoID + " has been successfully deleted!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Memo ID not found!");
            }

            return RedirectToAction("Listing");
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

    public class RedirectingActionScanIncoming : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.Session["UserRole"].ToString().Contains("Ops Exec"))
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