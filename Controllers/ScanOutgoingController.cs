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
    [RedirectingActionScanOutgoing]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ScanOutgoingController : ControllerBase
    {
        private IProductRepository _productsModel;
        private IScanOutgoingRepository _outgoingsModel;
        private ISaleRepository _salesModel;
        private ISettingRepository _settingsModel;
        private int sgdDp = 2;
        private int rateDP = 6;

        public ScanOutgoingController()
            : this(new ProductRepository(), new ScanOutgoingRepository(), new SaleRepository(), new SettingRepository())
        {

        }

        public ScanOutgoingController(IProductRepository productsModel, IScanOutgoingRepository outgoingsModel, ISaleRepository salesModel, ISettingRepository settingsModel)
        {
            _productsModel = productsModel;
            _outgoingsModel = outgoingsModel;
            _salesModel = salesModel;
            _settingsModel = settingsModel;
            Product sgd = _productsModel.FindCurrencyCode("SGD");
            sgdDp = sgd.Decimal;
            ViewData["SGDDP"] = GetDecimalFormat(sgdDp);
            ViewData["RateDP"] = GetDecimalFormat(rateDP);
        }

        // GET: ScanOutgoing
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
            int userid = Convert.ToInt32(Session["UserId"]);

            IList<ScanOutgoing> outgoing = _outgoingsModel.GetAllScanBy(userid);
            ViewData["Outgoing"] = outgoing;

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

            Sale sales = null;

            string memoID = form["MemoID"];

            if (string.IsNullOrEmpty(memoID))
            {
                ModelState.AddModelError("MemoID", "Memo ID is required!");
                errorMsg = "Memo ID is required!";
            }
            else
            {
                memoID = memoID.Trim().ToUpper();

                if (memoID.StartsWith("BCID"))
                {
                    memoID = memoID.Replace("BCID", "");
                }

                sales = _salesModel.GetSingle(memoID);

                if (sales == null)
                {
                    ModelState.AddModelError("MemoID", "Memo record not found!");
                    errorMsg = "Memo record not found!";
                }
                else
                {
                    if (sales.Status != "Pending Assign Delivery")
                    {
                        ModelState.AddModelError("MemoID", "Memo ID not ready!");
                        errorMsg = "Memo ID not ready!";
                    }
                    else
                    {
                        ScanOutgoing checkUnique = _outgoingsModel.FindMemoID(memoID);

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
                }
            }

            if (ModelState.IsValid)
            {
                if (sales != null)
                {
                    if (sales.Status == "Pending Assign Delivery" || sales.Status.Contains("Pending Delivery by"))
                    {
                        ScanOutgoing scanOutgoing = new ScanOutgoing();
                        scanOutgoing.SaleId = sales.ID;
                        scanOutgoing.Status = "Pending Assign Delivery";
                        scanOutgoing.ScanById = Convert.ToInt32(Session["UserId"]);

                        bool result = _outgoingsModel.Add(scanOutgoing);

                        if (result)
                        {
                            //int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "ScanOutgoings";
                            string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Outgoing Delivery";

                            bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            TempData.Add("Result", "success|" + sales.MemoID + " has been successfully created!");

                            return RedirectToAction("Listing");
                        }
                        else
                        {
                            TempData.Add("Result", "danger|An error occured while saving outgoing delivery!");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|Memo ID not ready for delivery!");
                    }
                }
            }
            else
            {
                TempData.Add("Result", "danger|" + errorMsg);
            }

            IList<ScanOutgoing> outgoing = _outgoingsModel.GetAllScanBy(userid);
            ViewData["Outgoing"] = outgoing;

            string prefixMemo = _settingsModel.GetCodeValue("PREFIX_SALE");
            ViewData["MemoIDPrefix"] = prefixMemo;

            string prefixBarcode = _settingsModel.GetCodeValue("PREFIX_BARCODE");
            ViewData["BarcodePrefix"] = prefixBarcode;

            ViewData["MemoID"] = form["MemoID"];

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ConfirmPickup
        public ActionResult ConfirmPickup()
        {

            int userid = Convert.ToInt32(Session["UserId"]);

            IList<ScanOutgoing> outgoing = _outgoingsModel.GetAllScanBy(userid);

            if (outgoing.Count > 0)
            {
                bool add_outgoing = false;
                bool add_sale = false;
                bool isValid = true;

                //check have different scanbyuser id with the loggin user id or not.
                foreach (ScanOutgoing data in outgoing)
                { 
                    if(data.ScanById != userid)
                    {
                        isValid = false;
                    }
                }

                if(isValid)
                {
                    foreach (ScanOutgoing data in outgoing)
                    {

                        //check is it the scanbyuser is same with logged in user.

                        bool result = _outgoingsModel.ConfirmOutgoing(data.ID);

                        if (result)
                        {
                            if (!add_outgoing)
                            {
                                add_outgoing = true;
                            }

                            bool result_sale = _salesModel.UpdateStatus(data.SaleId, "Pending Delivery by " + Session["UserName"].ToString(), Convert.ToInt32(Session["UserId"]));

                            if (result_sale)
                            {
                                if (!add_sale)
                                {
                                    add_sale = true;
                                }
                            }
                        }
                    }

                    if (add_outgoing)
                    {
                        string tableAffected = "ScanOutgoings";
                        string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Confirmed Outgoing Delivery";

                        bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    if (add_sale)
                    {
                        string tableAffected = "Sales";
                        string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Sales Status [Pending Delivery]";

                        bool sale_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    if (add_outgoing && add_sale)
                    {
                        TempData.Add("Result", "success|You have been successfully assigned to deliver these sales!");
                    }
                    else
                    {
                        TempData.Add("Result", "danger|An error occured while assigning outgoing delivery!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|The Sales item is not scan by the same person!");
                }

            }
            else
            {
                TempData.Add("Result", "danger|No outgoing delivery available!");
            }

            return RedirectToAction("Listing");
        }

        //GET: Delete
        public ActionResult Delete(int id)
        {
            ScanOutgoing outgoing = _outgoingsModel.GetSingle(id);

            if (outgoing != null)
            {
                bool result = _outgoingsModel.Delete(outgoing.ID);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "ScanOutgoings";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted Outgoing Delivery";

                    bool outgoing_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|" + outgoing.Sales.MemoID + " has been successfully deleted!");
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
            string format = "#,##0.";
            switch (dp)
            {
                case 1:
                    format += "0";
                    break;
                case 2:
                    format += "00";
                    break;
                case 3:
                    format += "000";
                    break;
                case 4:
                    format += "0000";
                    break;
                case 5:
                    format += "00000";
                    break;
                case 6:
                    format += "000000";
                    break;
                default:
                    format += "0000";
                    break;
            }

            return format;
        }
    }

    public class RedirectingActionScanOutgoing : ActionFilterAttribute
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