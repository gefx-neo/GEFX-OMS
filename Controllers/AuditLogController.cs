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
    [HandleError]
    [RedirectingAction]
    [RedirectingActionWithSuperAdmin]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AuditLogController : ControllerBase
    {
        private IAuditLogRepository _auditLogsModel;
        private IUserRepository _usersModel;

        public AuditLogController()
            : this(new AuditLogRepository(), new UserRepository())
        {

        }

        public AuditLogController(IAuditLogRepository auditLogsModel, IUserRepository usersModel)
        {
            _auditLogsModel = auditLogsModel;
            _usersModel = usersModel;
        }

        // GET: AuditLog
        public ActionResult Index()
        {
            if (TempData["FromDate"] != null)
            {
                TempData.Remove("FromDate");
            }

            if (TempData["ToDate"] != null)
            {
                TempData.Remove("ToDate");
            }

            if (TempData["Page"] != null)
            {
                TempData.Remove("Page");
            }

            if (TempData["PageSize"] != null)
            {
                TempData.Remove("PageSize");
            }

            return RedirectToAction("Select");
        }

        //GET: Select
        public ActionResult Select()
        {
            string logTable = "";

            if (TempData["LogTable"] != null)
            {
                logTable = TempData["LogTable"].ToString();
            }

            ViewData["FromDate"] = DateTime.Now.ToString("dd/MM/yyyy");

            if (TempData["FromDate"] != null)
            {
                ViewData["FromDate"] = TempData["FromDate"];
            }

            ViewData["ToDate"] = DateTime.Now.ToString("dd/MM/yyyy");

            if (TempData["ToDate"] != null)
            {
                ViewData["ToDate"] = TempData["ToDate"];
            }

            Dropdown[] logTableDDL = LogTableDropdown();
            ViewData["LogTableDropdown"] = new SelectList(logTableDDL, "val", "name", logTable);

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Select
        [HttpPost]
        public ActionResult Select(FormCollection form)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrEmpty(form["FromDate"]))
            {
                try
                {
                    fromDate = Convert.ToDateTime(form["FromDate"].ToString());
                }
                catch
                {
                    ModelState.AddModelError("FromDate", "From Date is not valid!");
                }
            }

            if (!string.IsNullOrEmpty(form["ToDate"]))
            {
                try
                {
                    toDate = Convert.ToDateTime(form["ToDate"].ToString());
                }
                catch
                {
                    ModelState.AddModelError("ToDate", "To Date is not valid!");
                }
            }

            if (fromDate > toDate)
            {
                ModelState.AddModelError("ToDate", "To Date is earlier than From Date!");
            }

            if (ModelState.IsValid)
            {
                TempData["LogTable"] = form["LogTable"].ToString();
                TempData["FromDate"] = form["FromDate"].ToString();
                TempData["ToDate"] = form["ToDate"].ToString();
                return RedirectToAction("Listing");
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            Dropdown[] logTableDDL = LogTableDropdown();
            ViewData["LogTableDropdown"] = new SelectList(logTableDDL, "val", "name", form["LogTable"].ToString());

            ViewData["FromDate"] = form["FromDate"].ToString();
            TempData["FromDate"] = form["FromDate"].ToString();

            ViewData["ToDate"] = form["ToDate"].ToString();
            TempData["ToDate"] = form["ToDate"].ToString();

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Listing
        public ActionResult Listing(int page = 1)
        {
            int pageSize = 20;
            if (TempData["PageSize"] != null)
            {
                pageSize = Convert.ToInt32(TempData["PageSize"]);
            }

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string tableAffected = TempData["LogTable"].ToString();
            ViewData["LogTable"] = tableAffected;
            TempData.Keep("LogTable");

            string fromDate = TempData["FromDate"].ToString();
            ViewData["FromDate"] = fromDate;
            TempData.Keep("FromDate");

            string toDate = TempData["ToDate"].ToString();
            ViewData["ToDate"] = toDate;
            TempData.Keep("ToDate");

            IPagedList<AuditLog> logs = _auditLogsModel.GetPaged(tableAffected, fromDate, toDate, page, pageSize);
            ViewData["AuditLogs"] = logs;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Listing
        [HttpPost]
        public ActionResult Listing(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(form["PageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            string tableAffected = TempData["LogTable"].ToString();
            ViewData["LogTable"] = tableAffected;
			TempData.Keep("LogTable");

			string fromDate = TempData["FromDate"].ToString();
            ViewData["FromDate"] = fromDate;
			TempData.Keep("FromDate");

			string toDate = TempData["ToDate"].ToString();
            ViewData["ToDate"] = toDate;
			TempData.Keep("ToDate");

			IPagedList<AuditLog> logs = _auditLogsModel.GetPaged(tableAffected, fromDate, toDate, page, pageSize);
            ViewData["AuditLogs"] = logs;

            Dropdown[] pageSizeDDL = PageSizeDDL();
            ViewData["PageSizeDropdown"] = new SelectList(pageSizeDDL, "val", "name", pageSize.ToString());

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: ExportExcel
        public void ExportExcel(string logTable, string fromDate, string toDate)
        {
            IList<AuditLog> logs = _auditLogsModel.GetAll(logTable, fromDate, toDate);

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Audit Logs");

                //set first row name
                ws.Cells[1, 1].Value = "Log ID";
                ws.Cells[1, 2].Value = "IP Address";
                ws.Cells[1, 3].Value = "Timestamp";
                ws.Cells[1, 4].Value = "User Triggering";
                ws.Cells[1, 5].Value = "Table Affected";
                ws.Cells[1, 6].Value = "Description";

                int rowCount = 2;

                foreach (AuditLog log in logs)
                {
                    ws.Cells[rowCount, 1].Value = log.ID;
                    ws.Cells[rowCount, 2].Value = log.IpAddress;
                    ws.Cells[rowCount, 3].Value = log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss");
                    if (log.UserTriggering == 0)
                    {
                        ws.Cells[rowCount, 4].Value = "System";
                    }
                    else
                    {
                        ws.Cells[rowCount, 4].Value = log.Users.Name;
                    }
                    ws.Cells[rowCount, 5].Value = log.TableAffected;
                    ws.Cells[rowCount, 6].Value = log.Description;
                    rowCount++;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=audit-logs-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //LogTable Dropdown
        public Dropdown[] LogTableDropdown()
        {
            IList<AuditLog> uniqueTables = _auditLogsModel.GetUniqueTable();

            Dropdown[] ddl = new Dropdown[uniqueTables.Count + 1];
            ddl[0] = new Dropdown { name = "All Tables", val = "" };

            int count = 1;

            foreach (AuditLog log in uniqueTables)
            {
                ddl[count] = new Dropdown { name = log.TableAffected, val = log.TableAffected };
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
    }
}