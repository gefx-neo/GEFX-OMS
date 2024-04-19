using DataAccess.POCO;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GreatEastForex.Controllers
{
	public class ControllerBase : Controller
	{
		protected override void OnException(ExceptionContext filterContext)
		{
			string logPath = Server.MapPath(ConfigurationManager.AppSettings["ErrorLogPath"].ToString() + "Error_" + DateTime.Now.ToString("yyyyMMdd") + ".log");

			WriteErrorLog(logPath, filterContext.Exception.ToString());

			if (filterContext.HttpContext.IsCustomErrorEnabled)
			{
				filterContext.ExceptionHandled = true;
				this.View("Error", filterContext.Exception);
			}
		}

		static void WriteErrorLog(string logFile, string text)
		{
			StringBuilder message = new StringBuilder();
			message.AppendLine(DateTime.Now.ToString());
			message.AppendLine(text);
			message.AppendLine("=========================================");

			System.IO.File.AppendAllText(logFile, message.ToString());
		}
	}

	public class RedirectingActionAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Current.Session["UserName"] == null)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					controller = "Access",
					action = "Index"
				}));
			}
			else
			{
				//Sale Task List
				List<string> status = new List<string>();

				//if ops exe, then assign the user id just only view own order.
				string isOpsExec = "No";
				string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

				//New Update
				string userRole = HttpContext.Current.Session["UserRole"].ToString();
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

				if (isOpsExec == "Yes")
				{
					status.Remove("Pending Delivery by ");
					status.Remove("Pending Incoming Delivery by ");
				}

				if (isOpsManager == "Yes")
				{
					status.Add("Pending Delivery by ");
				}

				SaleRepository _salesModel = new SaleRepository();
				RemittanceSaleRepository _remittancesalesModel = new RemittanceSaleRepository();

				//new update get count only
				int totalTasks = _salesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);
				int totalTasks2 = _remittancesalesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);

				//Customer Task List
				if (Array.IndexOf(userRoleList, "General Manager") >= 0)
				{
					CustomerParticularRepository _customerParticularsModel = new CustomerParticularRepository();
					int customerParticulars = _customerParticularsModel.GetAllByStatusCount("Pending Approval");

					totalTasks += customerParticulars;
				}
				totalTasks += totalTasks2;
				filterContext.Controller.ViewData["TaskListCount"] = totalTasks;
			}
		}
	}

	public class RedirectingActionAttributeSale : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Current.Session["UserName"] == null)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					controller = "Access",
					action = "Index"
				}));
			}
			else
			{
				//Sale Task List
				List<string> status = new List<string>();

				//if ops exe, then assign the user id just only view own order.
				string isOpsExec = "No";
				string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

				//New Update
				string userRole = HttpContext.Current.Session["UserRole"].ToString();
				string[] userRoleList = userRole.Split(',');

				//Check Who can access to this sale
				if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "Inventory") >= 0 || Array.IndexOf(userRoleList, "Ops Manager") >= 0 || Array.IndexOf(userRoleList, "Cashier") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "Junior Dealer") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0)
				{

				}
				else
				{
					filterContext.Controller.TempData["Result"] = "danger|You are not allow to access this sale.";
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
					{
						controller = "Access",
						action = "Index"
					}));
				}

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

				if (isOpsExec == "Yes")
				{
					status.Remove("Pending Delivery by ");
					status.Remove("Pending Incoming Delivery by ");
				}

				if (isOpsManager == "Yes")
				{
					status.Add("Pending Delivery by ");
				}

				SaleRepository _salesModel = new SaleRepository();
				RemittanceSaleRepository _remittancesalesModel = new RemittanceSaleRepository();

				//new update get count only
				int totalTasks = _salesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);
				int totalTasks2 = _remittancesalesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);

				//Customer Task List
				if (Array.IndexOf(userRoleList, "General Manager") >= 0)
				{
					CustomerParticularRepository _customerParticularsModel = new CustomerParticularRepository();
					int customerParticulars = _customerParticularsModel.GetAllByStatusCount("Pending Approval");

					totalTasks += customerParticulars;
				}
				totalTasks += totalTasks2;
				filterContext.Controller.ViewData["TaskListCount"] = totalTasks;
			}
		}
	}

	public class RedirectingActionAttributeRemittance : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Current.Session["UserName"] == null)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					controller = "Access",
					action = "Index"
				}));
			}
			else
			{
				//Sale Task List
				List<string> status = new List<string>();

				//if ops exe, then assign the user id just only view own order.
				string isOpsExec = "No";
				string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

				//New Update
				string userRole = HttpContext.Current.Session["UserRole"].ToString();
				string[] userRoleList = userRole.Split(',');

				//Check Who can access to this sale
				if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "Junior Dealer") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0)
				{

				}
				else
				{
					filterContext.Controller.TempData["Result"] = "danger|You are not allow to access this remittance.";
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
					{
						controller = "Access",
						action = "Index",
					}));
				}

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

				if (isOpsExec == "Yes")
				{
					status.Remove("Pending Delivery by ");
					status.Remove("Pending Incoming Delivery by ");
				}

				if (isOpsManager == "Yes")
				{
					status.Add("Pending Delivery by ");
				}

				SaleRepository _salesModel = new SaleRepository();
				RemittanceSaleRepository _remittancesalesModel = new RemittanceSaleRepository();

				//new update get count only
				int totalTasks = _salesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);
				int totalTasks2 = _remittancesalesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);

				//Customer Task List
				if (Array.IndexOf(userRoleList, "General Manager") >= 0)
				{
					CustomerParticularRepository _customerParticularsModel = new CustomerParticularRepository();
					int customerParticulars = _customerParticularsModel.GetAllByStatusCount("Pending Approval");

					totalTasks += customerParticulars;
				}
				totalTasks += totalTasks2;
				filterContext.Controller.ViewData["TaskListCount"] = totalTasks;
			}
		}
	}

	public class RedirectingActionWithSuperAdminAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Current.Session["UserName"] == null)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					controller = "Access",
					action = "Index"
				}));
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(HttpContext.Current.Session["UserRole"].ToString());
				if (!HttpContext.Current.Session["UserRole"].ToString().Contains("Super Admin"))
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

	public class RedirectingActionDealerOrSuperAdmin : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Current.Session["UserName"] == null)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					controller = "Access",
					action = "Index"
				}));
			}
			else
			{
				string userRole = HttpContext.Current.Session["UserRole"].ToString();
				string[] userRoleList = userRole.Split(',');

				if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
				{
					//Sale Task List
					List<string> status = new List<string>();

					//if ops exe, then assign the user id just only view own order.
					string isOpsExec = "No";
					string isOpsManager = "No"; //to check is ops manager or not, if yes, add one pending delivery into status list.

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

					if (isOpsExec == "Yes")
					{
						status.Remove("Pending Delivery by ");
						status.Remove("Pending Incoming Delivery by ");
					}

					if (isOpsManager == "Yes")
					{
						status.Add("Pending Delivery by ");
					}

					SaleRepository _salesModel = new SaleRepository();
					RemittanceSaleRepository _remittancesalesModel = new RemittanceSaleRepository();

					//new update get count only
					int totalTasks = _salesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);
					int totalTasks2 = _remittancesalesModel.GetUserSalesCount2(status, Convert.ToInt32(HttpContext.Current.Session["UserId"]), isOpsExec, isOpsManager);

					//Customer Task List
					if (Array.IndexOf(userRoleList, "General Manager") >= 0)
					{
						CustomerParticularRepository _customerParticularsModel = new CustomerParticularRepository();
						int customerParticulars = _customerParticularsModel.GetAllByStatusCount("Pending Approval");

						totalTasks += customerParticulars;
					}
					totalTasks += totalTasks2;
					filterContext.Controller.ViewData["TaskListCount"] = totalTasks;
				}
				else
				{
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
					{
						controller = "Access",
						action = "Index"
					}));
				}
			}
		}
	}

    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public string BasicRealm { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public BasicAuthenticationAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!String.IsNullOrEmpty(auth))
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                if (user.Name == Username && user.Pass == Password) return;
            }
            filterContext.HttpContext.Response.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Ryadel"));
            /// thanks to eismanpat for this line: http://www.ryadel.com/en/http-basic-authentication-asp-net-mvc-using-custom-actionfilter/#comment-2507605761
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}