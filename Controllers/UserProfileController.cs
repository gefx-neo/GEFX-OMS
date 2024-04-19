using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
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
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class UserProfileController : ControllerBase
    {
        private IUserRepository _usersModel;

        public UserProfileController()
            : this(new UserRepository())
        {

        }

        public UserProfileController(IUserRepository usersModel)
        {
            _usersModel = usersModel;
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            int userid = Convert.ToInt32(Session["UserId"]);

            User users = _usersModel.GetSingle(userid);
            ViewData["NRIC"] = users.NRIC;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: UserProfile
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            User oldData = _usersModel.GetSingle(userid);

            if (!string.IsNullOrEmpty(form["Password"]))
            {
                if (string.IsNullOrEmpty(form["RepeatPassword"]))
                {
                    ModelState.AddModelError("RepeatPassword", "Repeat Password is required!");
                }
                else
                {
                    if (form["Password"].ToString() != form["RepeatPassword"].ToString())
                    {
                        ModelState.AddModelError("RepeatPassword", "Password and Repeat Password not matched!");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(form["RepeatPassword"]))
                {
                    ModelState.AddModelError("Password", "Password is required!");
                }
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(form["Password"]))
                {
                    User users = new User();
                    users.Password = EncryptionHelper.Encrypt(form["Password"].ToString());

                    bool result = _usersModel.UpdateProfile(userid, users);

                    if (result)
                    {
                        string tableAffected = "Users";
                        string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Profile";
                        bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                        TempData.Add("Result", "success|" + oldData.Name + "'s Profile has been successfully updated!");
                    }
                    else
                    {
                        TempData.Add("Result", "danger|An error occured while saving user record!");
                    }
                }
                else
                {
                    TempData.Add("Result", "success|No record was updated!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            ViewData["NRIC"] = oldData.NRIC;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }
    }
}