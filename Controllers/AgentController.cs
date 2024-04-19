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
	public class AgentController : ControllerBase
	{
		private IAgentRepository _agentsModel;

		public AgentController()
			: this(new AgentRepository())
		{

		}

		public AgentController(IAgentRepository agentsModel)
		{
			_agentsModel = agentsModel;
		}

		// GET: Agent
		public ActionResult Index()
		{
			return RedirectToAction("Listing");
		}

		public ActionResult Create()
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				Dropdown[] statusDDL = StatusDDL();
				ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name");

				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to access this page!");
				return RedirectToAction("Index", "TaskList");
			}
		}

		[HttpPost]
		public ActionResult Create(Agents agents, FormCollection form)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData["Page"]);
				ViewData["Page"] = page;
			}

			if (!string.IsNullOrEmpty(agents.AgentId))
			{
				if (string.IsNullOrEmpty(agents.AgentId.Trim()))
				{
					ModelState.AddModelError("agents.AgentId", "Agent ID cannot be empty!");
				}
				else
				{
					if (agents.AgentId.Length > 10)
					{
						ModelState.AddModelError("agents.AgentId", "Agent ID cannot more than 10 characters!");
					}
					else
					{
						Agents checkAgentId = _agentsModel.FindAgentId(agents.AgentId);

						if (checkAgentId != null)
						{
							ModelState.AddModelError("agents.AgentId", "Agent ID already existed!");
						}
					}
				}
			}
			else
			{
				ModelState.AddModelError("agents.AgentId", "Agent ID cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.CompanyName) || string.IsNullOrEmpty(agents.CompanyName.Trim()))
			{
				ModelState.AddModelError("agents.CompanyName", "Company Name cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.ContactPerson) || string.IsNullOrEmpty(agents.ContactPerson.Trim()))
			{
				ModelState.AddModelError("agents.ContactPerson", "Contact Person cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.ContactNumber) || string.IsNullOrEmpty(agents.ContactNumber.Trim()))
			{
				ModelState.AddModelError("agents.ContactNumber", "Contact Number cannot be empty!");
			}

			if (ModelState.IsValid)
			{
				bool result = _agentsModel.Add(agents);

				if (result)
				{
					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "Agents";
					string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Agents";

					bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData.Add("Result", "success|" + agents.AgentId + " has been successfully created!");

					return RedirectToAction("Listing", new { @page = page });
				}
				else
				{
					TempData.Add("Result", "danger|An error occured while saving agents record!");
				}
			}
			else
			{
				TempData.Add("Result", "danger|There is something wrong in the form!");
			}

			Dropdown[] statusDDL = StatusDDL();
			ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", agents.Status);

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

		public ActionResult Edit(int id)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData.Peek("Page"));
				ViewData["Page"] = page;
				TempData.Keep("Page");
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');
			ViewData["ViewOnly"] = "";

			if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0 || Array.IndexOf(userRoleList, "Customer Viewer") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0)
			{
				ViewData["ViewOnly"] = "Yes";
			}

			Agents agents = _agentsModel.GetSingle(id);

			if (agents != null)
			{
				ViewData["AgentID"] = agents.ID;
				Dropdown[] statusDDL = StatusDDL();
				ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", agents.Status);

				ViewData["Agents"] = agents;

				ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
				return View();
			}

			TempData.Add("Result", "danger|Agent record not found!");

			return RedirectToAction("Listing", new { @page = page });
		}

		[HttpPost]
		public ActionResult Edit(int id, Agents agents, FormCollection form)
		{
			int page = 1;

			if (TempData["Page"] != null)
			{
				page = Convert.ToInt32(TempData["Page"]);
				ViewData["Page"] = page;
			}

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');
			ViewData["ViewOnly"] = "";

			if (Array.IndexOf(userRoleList, "Junior Dealer") >= 0 || Array.IndexOf(userRoleList, "Customer Viewer") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0)
			{
				ViewData["ViewOnly"] = "Yes";
			}

			Agents oldData = _agentsModel.GetSingle(id);

			if (!string.IsNullOrEmpty(agents.AgentId))
			{
				if (string.IsNullOrEmpty(agents.AgentId.Trim()))
				{
					ModelState.AddModelError("agents.AgentId", "Agent ID cannot be empty!");
				}
				else
				{
					if (agents.AgentId.Length > 10)
					{
						ModelState.AddModelError("agents.AgentId", "Agent ID cannot more than 10 characters!");
					}
					else
					{
						Agents checkAgentId = _agentsModel.FindAgentIdNotOwnSelf(id, agents.AgentId);

						if (checkAgentId != null)
						{
							ModelState.AddModelError("agents.AgentId", "Agent ID already existed!");
						}
					}
				}
			}
			else
			{
				ModelState.AddModelError("agents.AgentId", "Agent ID cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.CompanyName) || string.IsNullOrEmpty(agents.CompanyName.Trim()))
			{
				ModelState.AddModelError("agents.CompanyName", "Company Name cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.ContactPerson) || string.IsNullOrEmpty(agents.ContactPerson.Trim()))
			{
				ModelState.AddModelError("agents.ContactPerson", "Contact Person cannot be empty!");
			}

			if (string.IsNullOrEmpty(agents.ContactNumber) || string.IsNullOrEmpty(agents.ContactNumber.Trim()))
			{
				ModelState.AddModelError("agents.ContactNumber", "Contact Number cannot be empty!");
			}
			else
			{
				if (agents.ContactNumber.Length > 15)
				{
					ModelState.AddModelError("agents.ContactNumber", "Contact Number cannot more than 15 characters!");
				}
			}

			if (ModelState.IsValid)
			{

				bool result = _agentsModel.Update(id, agents);

				if (result)
				{
					int userid = Convert.ToInt32(Session["UserId"]);
					string tableAffected = "Agents";
					string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Agent";

					bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

					TempData.Add("Result", "success|" + agents.AgentId + " has been successfully updated!");

					return RedirectToAction("Listing", new { @page = page });
				}
				else
				{
					TempData.Add("Result", "danger|An error occured while saving agent record!");
				}
			}
			else
			{
				TempData.Add("Result", "danger|There is something wrong in the form!");
			}

			ViewData["AgentID"] = agents.ID;
			ViewData["Agents"] = agents;
			Dropdown[] statusDDL = StatusDDL();
			ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", agents.Status);

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
			return View();
		}

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

			IPagedList<Agents> agents = _agentsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
			ViewData["Agents"] = agents;

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

			IPagedList<Agents> agents = _agentsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
			ViewData["Agents"] = agents;

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

			string userRole = Session["UserRole"].ToString();
			string[] userRoleList = userRole.Split(',');

			if (Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0)
			{
				Agents agents = _agentsModel.GetSingle(id);

				if (agents != null)
				{
					bool result = _agentsModel.Delete(id);

					if (result)
					{
						int userid = Convert.ToInt32(Session["UserId"]);
						string tableAffected = "Agents";
						string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Agent";

						bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

						TempData.Add("Result", "success|" + agents.AgentId + " has been successfully deleted!");
					}
					else
					{
						TempData.Add("Result", "danger|An error occured while deleting agent record!");
					}
				}
				else
				{
					TempData.Add("Result", "danger|Agent record not found!");
				}

				return RedirectToAction("Listing", new { @page = page });
			}
			else
			{
				TempData.Add("Result", "danger|You are not allow to delete this record!");
				return RedirectToAction("Index", "TaskList");
			}
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