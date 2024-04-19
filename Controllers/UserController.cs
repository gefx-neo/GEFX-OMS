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
    [RedirectingAction]
    [RedirectingActionWithSuperAdmin]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class UserController : ControllerBase
    {
        private IUserRepository _usersModel;

        public UserController()
            : this(new UserRepository())
        {

        }

        public UserController(IUserRepository usersModel)
        {
            _usersModel = usersModel;
        }

        // GET: User
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

            IPagedList<User> users = _usersModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
            ViewData["User"] = users;

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

            IPagedList<User> users = _usersModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
            ViewData["User"] = users;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Create
        public ActionResult Create()
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            ViewData["DealerCheckbox"] = "";
            ViewData["FinanceCheckbox"] = "";
            ViewData["InventoryCheckbox"] = "";
            ViewData["OpsManagerCheckbox"] = "";
            ViewData["OpsExecCheckbox"] = "";
            ViewData["CashierCheckbox"] = "";
            ViewData["GeneralManagerCheckbox"] = "";
            ViewData["SuperAdminCheckbox"] = "";
			ViewData["JuniorDealerCheckbox"] = "";
			ViewData["CustomerViewerCheckbox"] = "";
			ViewData["SalesAdminCheckbox"] = "";

			Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name");
            
            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Create
        [HttpPost]
        public ActionResult Create(User users, FormCollection form)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            if (!string.IsNullOrEmpty(users.NRIC))
            {
                User checkNric = _usersModel.FindNric(users.NRIC);

                if (checkNric != null)
                {
                    ModelState.AddModelError("users.NRIC", "NRIC already existed!");
                }
            }

            if (!string.IsNullOrEmpty(users.Email))
            {
                bool checkEmailFormat = FormValidationHelper.EmailValidation(users.Email);

                if (!checkEmailFormat)
                {
                    ModelState.AddModelError("users.Email", "Email is not valid!");
                }
                else
                {
                    User checkEmail = _usersModel.FindEmail(users.Email);

                    if (checkEmail != null)
                    {
                        ModelState.AddModelError("users.Email", "Email already existed!");
                    }
                }
            }

            if (string.IsNullOrEmpty(form["Role"]))
            {
                ModelState.AddModelError("users.Role", "Role is required!");
            }

            if (string.IsNullOrEmpty(users.Password))
            {
                ModelState.AddModelError("users.Password", "Password is required!");
            }

            if (string.IsNullOrEmpty(form["RepeatPassword"]))
            {
                ModelState.AddModelError("RepeatPassword", "Repeat Password is required!");
            }
            else
            {
                if (users.Password != form["RepeatPassword"].ToString())
                {
                    ModelState.AddModelError("RepeatPassword", "Password and Repeat Password not matched!");
                }
            }

            if (ModelState.IsValid)
            {
                users.Role = form["Role"].ToString();
                users.Password = EncryptionHelper.Encrypt(users.Password);

                bool result = _usersModel.Add(users);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Users";
                    string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created User";

                    bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|" + users.NRIC + " has been successfully created!");
                    
                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
                    TempData.Add("Result", "danger|An error occured while saving user record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            ViewData["DealerCheckbox"] = "";
            ViewData["FinanceCheckbox"] = "";
            ViewData["InventoryCheckbox"] = "";
            ViewData["OpsManagerCheckbox"] = "";
            ViewData["OpsExecCheckbox"] = "";
            ViewData["CashierCheckbox"] = "";
            ViewData["GeneralManagerCheckbox"] = "";
            ViewData["SuperAdminCheckbox"] = "";
			ViewData["JuniorDealerCheckbox"] = "";
			ViewData["CustomerViewerCheckbox"] = "";
			ViewData["SalesAdminCheckbox"] = "";

			if (!string.IsNullOrEmpty(form["Role"]))
            {

				string[] userRoleList = form["Role"].ToString().Split(',');

				if (Array.IndexOf(userRoleList, "Dealer") >= 0)
                {
                    ViewData["DealerCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Finance") >= 0)
                {
                    ViewData["FinanceCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Inventory") >= 0)
                {
                    ViewData["InventoryCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Ops Manager") >= 0)
                {
                    ViewData["OpsManagerCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Ops Exec") >= 0)
                {
                    ViewData["OpsExecCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Cashier") >= 0)
                {
                    ViewData["CashierCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                {
                    ViewData["GeneralManagerCheckbox"] = "checked";
                }

                if (Array.IndexOf(userRoleList, "Super Admin") >= 0)
                {
                    ViewData["SuperAdminCheckbox"] = "checked";
                }

				if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0)
				{
					ViewData["JuniorDealerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
				{
					ViewData["CustomerViewerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Sales Admin") >= 0)
				{
					ViewData["SalesAdminCheckbox"] = "checked";
				}
            }

            Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", users.Status);

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Edit
        public ActionResult Edit(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            User users = _usersModel.GetSingle(id);

            if (users != null)
            {
                ViewData["DealerCheckbox"] = "";
                ViewData["FinanceCheckbox"] = "";
                ViewData["InventoryCheckbox"] = "";
                ViewData["OpsManagerCheckbox"] = "";
                ViewData["OpsExecCheckbox"] = "";
                ViewData["CashierCheckbox"] = "";
                ViewData["GeneralManagerCheckbox"] = "";
                ViewData["SuperAdminCheckbox"] = "";
				ViewData["JuniorDealerCheckbox"] = "";
				ViewData["CustomerViewerCheckbox"] = "";
				ViewData["SalesAdminCheckbox"] = "";
				ViewData["UserID"] = users.ID;

				string[] userRoleList = users.Role.ToString().Split(',');

				if (Array.IndexOf(userRoleList, "Dealer") >= 0)
				{
					ViewData["DealerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Finance") >= 0)
				{
					ViewData["FinanceCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Inventory") >= 0)
				{
					ViewData["InventoryCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Ops Manager") >= 0)
				{
					ViewData["OpsManagerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Ops Exec") >= 0)
				{
					ViewData["OpsExecCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Cashier") >= 0)
				{
					ViewData["CashierCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "General Manager") >= 0)
				{
					ViewData["GeneralManagerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Super Admin") >= 0)
				{
					ViewData["SuperAdminCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0)
				{
					ViewData["JuniorDealerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
				{
					ViewData["CustomerViewerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Sales Admin") >= 0)
				{
					ViewData["SalesAdminCheckbox"] = "checked";
				}

				Dropdown[] statusDDL = StatusDDL();
                ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", users.Status);

                ViewData["User"] = users;

                ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                return View();
            }

            TempData.Add("Result", "danger|User record not found!");
                        
            return RedirectToAction("Listing", new { @page = page });
        }

        //POST: Edit
        [HttpPost]
        public ActionResult Edit(int id, User users, FormCollection form)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            User oldData = _usersModel.GetSingle(id);

            if (!string.IsNullOrEmpty(users.NRIC))
            {
                User checkNric = _usersModel.FindNric(users.NRIC);

                if (checkNric != null)
                {
                    if (id != checkNric.ID)
                    {
                        ModelState.AddModelError("users.NRIC", "NRIC already existed!");
                    }
                }
            }

            if (!string.IsNullOrEmpty(users.Email))
            {
                bool checkEmailFormat = FormValidationHelper.EmailValidation(users.Email);

                if (!checkEmailFormat)
                {
                    ModelState.AddModelError("users.Email", "Email is not valid!");
                }
                else
                {
                    User checkEmail = _usersModel.FindEmail(users.Email);

                    if (checkEmail != null)
                    {
                        if (id != checkEmail.ID)
                        {
                            ModelState.AddModelError("users.Email", "Email already existed!");
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(form["Role"]))
            {
                ModelState.AddModelError("users.Role", "Role is required!");
            }

            if (!string.IsNullOrEmpty(users.Password))
            {
                if (!string.IsNullOrEmpty(form["RepeatPassword"]))
                {
                    if (users.Password != form["RepeatPassword"].ToString())
                    {
                        ModelState.AddModelError("RepeatPassword", "Password and Repeat Password not matched!");
                    }
                }
                else
                {
                    ModelState.AddModelError("RepeatPassword", "Repeat Password is required!");
                }
            }

            if (ModelState.IsValid)
            {
                users.Role = form["Role"].ToString();

                if (!string.IsNullOrEmpty(users.Password))
                {
                    users.Password = EncryptionHelper.Encrypt(users.Password);
                }
                else
                {
                    users.Password = oldData.Password;
                }

                bool result = _usersModel.Update(id, users);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Users";
                    string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated User";

                    bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|" + users.NRIC + " has been successfully updated!");
                    
                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
                    TempData.Add("Result", "danger|An error occured while saving user record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            ViewData["DealerCheckbox"] = "";
            ViewData["FinanceCheckbox"] = "";
            ViewData["InventoryCheckbox"] = "";
            ViewData["OpsManagerCheckbox"] = "";
            ViewData["OpsExecCheckbox"] = "";
            ViewData["CashierCheckbox"] = "";
            ViewData["GeneralManagerCheckbox"] = "";
            ViewData["SuperAdminCheckbox"] = "";
			ViewData["JuniorDealerCheckbox"] = "";
			ViewData["CustomerViewerCheckbox"] = "";
			ViewData["SalesAdminCheckbox"] = "";

			if (!string.IsNullOrEmpty(form["Role"]))
            {
				string[] userRoleList = form["Role"].ToString().Split(',');

				if (Array.IndexOf(userRoleList, "Dealer") >= 0)
				{
					ViewData["DealerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Finance") >= 0)
				{
					ViewData["FinanceCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Inventory") >= 0)
				{
					ViewData["InventoryCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Ops Manager") >= 0)
				{
					ViewData["OpsManagerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Ops Exec") >= 0)
				{
					ViewData["OpsExecCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Cashier") >= 0)
				{
					ViewData["CashierCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "General Manager") >= 0)
				{
					ViewData["GeneralManagerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Super Admin") >= 0)
				{
					ViewData["SuperAdminCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0)
				{
					ViewData["JuniorDealerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
				{
					ViewData["CustomerViewerCheckbox"] = "checked";
				}

				if (Array.IndexOf(userRoleList, "Sales Admin") >= 0)
				{
					ViewData["SalesAdminCheckbox"] = "checked";
				}
			}

            Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", users.Status);

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Delete
        public ActionResult Delete(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
            }

            User users = _usersModel.GetSingle(id);

            if (users != null)
            {
                bool result = _usersModel.Delete(id);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Users";
                    string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted User";

                    bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|" + users.NRIC + " has been successfully deleted!");
                }
                else
                {
                    TempData.Add("Result", "danger|An error occured while deleting user record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|User record not found!");
            }
            
            return RedirectToAction("Listing", new { @page = page });
        }

        //Status Dropdown
        public Dropdown[] StatusDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Active", val = "Active" };
            ddl[1] = new Dropdown { name = "Suspended", val = "Suspended" };
            return ddl;
        }
    }
}