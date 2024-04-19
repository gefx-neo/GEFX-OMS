using DataAccess;
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
    [RedirectingActionWithSuperAdmin]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SettingController : ControllerBase
    {
        private ISettingRepository _settingsModel;
		private ISearchTagRepository _searchTagsModel;

		public SettingController()
            : this(new SettingRepository(), new SearchTagRepository())
        {

        }

        public SettingController(ISettingRepository settingsModel, ISearchTagRepository searchTagsModel)
        {
            _settingsModel = settingsModel;
			_searchTagsModel = searchTagsModel;
        }

        // GET: Setting
        public ActionResult Index()
        {
            if (TempData["SearchKeyword"] != null)
            {
                TempData.Remove("SearchKeyword");
            }

            if (TempData["LogTable"] != null)
            {
                TempData.Remove("LogTable");
            }

            if (TempData["FromDate"] != null)
            {
                TempData.Remove("FromDate");
            }

            if (TempData["ToDate"] != null)
            {
                TempData.Remove("ToDate");
            }

            return RedirectToAction("Listing");
        }

        //GET: Listing
        public ActionResult Listing()
        {
            IList<Setting> settings = _settingsModel.GetAll().OrderBy(e => e.Code == "LOCAL_PAYMENT_BANK").ToList();
			IList<SearchTags> searchTags = _searchTagsModel.GetAll().ToList();
            ViewData["Setting"] = settings;
			ViewData["SearchTags"] = searchTags;

			Dropdown[] customerDDL = CustomerCreatedByDDL();
			ViewData["CustomerPortalCreatedByDropdown"] = new SelectList(customerDDL, "val", "name", _settingsModel.GetCodeValue("CUSTOMER_PORTAL_CREATED_BY"));

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Edit
        public ActionResult Edit()
        {
            IList<Setting> settings = _settingsModel.GetAll().OrderBy(e => e.Code == "LOCAL_PAYMENT_BANK").ToList();
            ViewData["Setting"] = settings;

			List<SearchTags> searchTags = _searchTagsModel.GetAll().ToList();

			List<SystemSettingSearchTagList> systemSearchTagList = new List<SystemSettingSearchTagList>();

			int tagcount = 1;
			foreach (SearchTags _search in searchTags)
			{
				systemSearchTagList.Add(new SystemSettingSearchTagList()
				{
					ID = tagcount,
					SearchTagId = _search.ID,
					TagName = _search.TagName,
					isNewTag = false
				});

				tagcount++;
			}

			ViewData["SearchTagList"] = systemSearchTagList.ToList();

			string baseCurrency = settings.Where(e => e.Code == "BASE_CURRENCY").FirstOrDefault().Value;
            Dropdown[] baseCurrencyDDL = BaseCurrencyDDL();
            ViewData["BaseCurrencyDropdown"] = new SelectList(baseCurrencyDDL, "val", "name", baseCurrency);

			Dropdown[] customerDDL = CustomerCreatedByDDL();
			ViewData["CustomerPortalCreatedByDropdown"] = new SelectList(customerDDL, "val", "name", _settingsModel.GetCodeValue("CUSTOMER_PORTAL_CREATED_BY"));

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Edit
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            List<string> allKeys = form.AllKeys.Where(e => e.Contains("SettingID")).ToList();

            foreach (string key in allKeys)
            {
                int id = Convert.ToInt32(key.Substring(10));

				if (form["Code_" + id].ToString() == "LOCAL_PAYMENT_BANK")
				{
					List<string> bankKeys = form.AllKeys.Where(e => e.Contains("Value_" + id + "_")).ToList();

					if (bankKeys.Count == 0)
					{
						ModelState.AddModelError("Code_" + id, "Please add AT LEAST ONE Bank Name!");
					}
					else
					{
						List<string> existingBanks = new List<string>();

						foreach (string bankKey in bankKeys)
						{
							string b = form[bankKey];

							if (string.IsNullOrEmpty(b))
							{
								ModelState.AddModelError(bankKey, "Bank Name is required!");
							}
							else
							{
								if (existingBanks.Contains(b))
								{
									ModelState.AddModelError(bankKey, "'" + b + "' is duplicated!");
								}
								else
								{
									existingBanks.Add(b);
								}
							}
						}

						if (!existingBanks.Contains("CASH"))
						{
							ModelState.AddModelError("Code_" + id, "Default CASH is required!");
						}
					}
				}
				else if (form["Code_" + id].ToString() == "CUSTOMER_PORTAL_CREATED_BY")
				{
					if (string.IsNullOrEmpty(form["Value_" + id]))
					{
						ModelState.AddModelError(key, form["Code_" + id].ToString() + " is required!");
					}
				}
				else
				{
					if (string.IsNullOrEmpty(form["Value_" + id]))
					{
						ModelState.AddModelError(key, form["Code_" + id].ToString() + " is required!");
					}
				}
            }

			//Update Tag List Record
			List<string> dataSetupKeysTag = form.AllKeys.Where(e => e.Contains("TagList_")).ToList();
			List<string> duplicateTag = new List<string>();
			//first digit is company primary id
			//second loop is abbreviation
			//check validation first
			if (dataSetupKeysTag.Count > 0)
			{
				foreach (string key in dataSetupKeysTag)
				{
					string[] id = key.Split('_');

					if (id[2] == "NewTagList")
					{
						if (string.IsNullOrEmpty(form[key]))
						{
							if (id[1] == "Value")
							{
								ModelState.AddModelError(key, "Tag Name cannot be empty!");
							}
							else
							{
								ModelState.AddModelError(key, id[1] + " cannot be empty!");
							}
						}
						else
						{
							if (id[1] == "Value")
							{
								//check tag name cannot same with db one.
								IList<SearchTags> getAll = _searchTagsModel.GetAll().Where(e => e.TagName == form[key]).ToList();

								if (getAll.Count > 0)
								{
									//exist in db
									ModelState.AddModelError(key, "Tag Name already exist in db!");
								}
								else
								{
									//if no exist, check current tag have same value or not.
									if (duplicateTag.Where(e => e.ToString() == form[key]).FirstOrDefault() == null)
									{
										duplicateTag.Add(form[key]);
									}
									else
									{
										ModelState.AddModelError(key, "Tag Name already exist!");
									}
								}

								string value = form[key];
							}
						}
					}
					else
					{
						if (string.IsNullOrEmpty(form[key]))
						{
							if (id[1] == "Value")
							{
								ModelState.AddModelError(key, "Tag Name cannot be empty!");
							}
							else
							{
								ModelState.AddModelError(key, id[1] + " cannot be empty!");
							}

						}
						else
						{
							if (id[1] == "Value")
							{
								//check tag name cannot same with db one.
								IList<SearchTags> getAll = _searchTagsModel.GetAll().Where(e => e.TagName == form[key] && e.ID != Convert.ToInt32(id[3])).ToList();

								if (getAll.Count > 0)
								{
									//exist in db
									ModelState.AddModelError(key, "Tag Name already exist in db!");
								}
								else
								{
									//if no exist, check current tag have same value or not.
									if (duplicateTag.Where(e => e.ToString() == form[key]).FirstOrDefault() == null)
									{
										duplicateTag.Add(form[key]);
									}
									else
									{
										ModelState.AddModelError(key, "Tag Name already exist!");
									}
								}

								string value = form[key];
							}
						}
					}
				}
			}
			else
			{
				ModelState.AddModelError("GeneralTagListError", "At least one Search Tag!");
			}
			//End

			List<SearchTags> SearchTagAddList = new List<SearchTags>();
			List<int> NewCreatedTagID = new List<int>();
			SearchTags TagAdd = new SearchTags();

			if (ModelState.IsValid)
            {
                bool result = false;

                foreach (string key in allKeys)
                {
                    int id = Convert.ToInt32(key.Substring(10));
                    string value = "";

                    if (form["Code_" + id] == "LOCAL_PAYMENT_BANK")
                    {
                        List<string> bankKeys = form.AllKeys.Where(e => e.Contains("Value_" + id + "_")).ToList();

                        foreach (string bankKey in bankKeys)
                        {
                            string b = form[bankKey];

                            value += b + "|";
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Substring(0, value.Length - 1);
                        }
                    }
                    else
                    {
                        value = form["Value_" + id].ToString();
                    }

                    bool update = _settingsModel.Update(id, value);

                    if (update)
                    {
                        if (!result)
                        {
                            result = true;
                        }
                    }
                }

				//Search Tag List Here
				//Loop Add New Tag first
				List<string> newTagListKeys = form.AllKeys.Where(e => e.Contains("Value_NewTagList")).ToList();

				foreach (string key in newTagListKeys)
				{
					string[] id = key.Split('_');

					TagAdd.TagName = form[id[0] + "_Value_" + "NewTagList_" + id[3]];

					//add new item
					bool addNewTag = _searchTagsModel.Add(TagAdd);
					result = true;
					SearchTags getlast = _searchTagsModel.GetLast();
					NewCreatedTagID.Add(getlast.ID);
				}

				//Loop Edit Tag
				List<string> UpdateTagListKeys = form.AllKeys.Where(e => e.Contains("Value_TagList")).ToList();
				foreach (string key in UpdateTagListKeys)
				{
					string[] id = key.Split('_');

					TagAdd = new SearchTags();//reset tag
					TagAdd.TagName = form[id[0] + "_Value_" + "TagList_" + id[3] + "_" + id[4]];

					SearchTags oldData = _searchTagsModel.GetSingle(Convert.ToInt32(id[3]));

					if (oldData != null)
					{
						if (oldData.TagName != TagAdd.TagName)
						{
							//update item
							bool updateExistingTag = _searchTagsModel.Update(Convert.ToInt32(id[3]), TagAdd);
							result = true;
						}
					}
				}

				//DELETE Tag Item
				//Get All Tag List From database

				IList<SearchTags> getAllTags = _searchTagsModel.GetAll();
				List<int> DBAllTagID = new List<int>();

				foreach (SearchTags _getAll in getAllTags)
				{
					DBAllTagID.Add(_getAll.ID);
				}

				//Get All Current Have Company Item
				List<int> CurrentTagID = new List<int>();

				List<string> CurrentTagKeys = form.AllKeys.Where(e => e.Contains("Value_TagList")).ToList();
				foreach (string key in CurrentTagKeys)
				{
					string[] id = key.Split('_');
					CurrentTagID.Add(Convert.ToInt32(id[3]));
				}

				CurrentTagID.AddRange(NewCreatedTagID);

				var itemtoberemovedTag = DBAllTagID.Except(CurrentTagID).ToList();

				foreach (var _item in itemtoberemovedTag)
				{
					bool Delete = _searchTagsModel.Delete(_item);
					result = true;
				}
				//End

				if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Settings";
                    string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated System Settings";
                    bool log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    TempData.Add("Result", "success|System Settings have been successfully updated!");

                    return RedirectToAction("Listing");
                }
            }

            IList<Setting> settings = new List<Setting>();

			Dropdown[] customerDDL = CustomerCreatedByDDL();
			ViewData["CustomerPortalCreatedByDropdown"] = new SelectList(customerDDL, "val", "name", _settingsModel.GetCodeValue("CUSTOMER_PORTAL_CREATED_BY"));

			foreach (string key in allKeys)
            {
                int id = Convert.ToInt32(key.Substring(10));

                Setting setting = new Setting();
                setting.ID = id;
                setting.Code = form["Code_" + id].ToString();
                setting.Value = "";

				if (form["Code_" + id].ToString() == "LOCAL_PAYMENT_BANK")
				{
					List<string> bankKeys = form.AllKeys.Where(e => e.Contains("Value_" + id + "_")).ToList();

					foreach (string bankKey in bankKeys)
					{
						string b = form[bankKey];

						setting.Value += b + "|";
					}

					if (!string.IsNullOrEmpty(setting.Value))
					{
						setting.Value = setting.Value.Substring(0, setting.Value.Length - 1);
					}
				}
				else if (form["Code_" + id].ToString() == "CUSTOMER_PORTAL_CREATED_BY")
				{
					ViewData["CustomerPortalCreatedByDropdown"] = new SelectList(customerDDL, "val", "name", form["Value_" + id].ToString());
				}
				else
				{
					setting.Value = form["Value_" + id].ToString();
				}

                settings.Add(setting);
            }

            ViewData["Setting"] = settings;

            string baseCurrency = settings.Where(e => e.Code == "BASE_CURRENCY").FirstOrDefault().Value;
            Dropdown[] baseCurrencyDDL = BaseCurrencyDDL();
            ViewData["BaseCurrencyDropdown"] = new SelectList(baseCurrencyDDL, "val", "name", baseCurrency);

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //BaseCurrency Dropdown
        public Dropdown[] BaseCurrencyDDL()
        {
            Dropdown[] ddl = new Dropdown[3];
            ddl[0] = new Dropdown { name = "SGD", val = "SGD" };
            ddl[1] = new Dropdown { name = "USD", val = "USD" };
            ddl[2] = new Dropdown { name = "EUR", val = "EUR" };
            return ddl;
        }

		//Status Dropdown
		public Dropdown[] CustomerCreatedByDDL()
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var getAdminRecord = context.Users.Where(e => e.IsDeleted == "N").ToList();

				if (getAdminRecord.Count > 0)
				{
					Dropdown[] ddl = new Dropdown[getAdminRecord.Count];
					int count = 0;

					foreach (User _user in getAdminRecord)
					{
						ddl[count] = new Dropdown { name = _user.Name, val = _user.ID.ToString() };
						count++;
					}
					return ddl;
				}
				else
				{
					Dropdown[] ddl = new Dropdown[0];
					return ddl;
				}
			}
		}
	}
}