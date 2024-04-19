using DataAccess;
using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productsModel;
        private IProductInventoryRepository _productInventoriesModel;
        private IProductDenominationRepository _productDenominationsModel;
        private IInventoryRepository _inventoriesModel;

        public ProductController()
            : this(new ProductRepository(), new ProductInventoryRepository(), new ProductDenominationRepository(), new InventoryRepository())
        {

        }

        public ProductController(IProductRepository productsModel, IProductInventoryRepository productInventoriesModel, IProductDenominationRepository productDenominationsModel, IInventoryRepository inventoriesModel)
        {
            _productsModel = productsModel;
            _productInventoriesModel = productInventoriesModel;
            _productDenominationsModel = productDenominationsModel;
            _inventoriesModel = inventoriesModel;
        }

        // GET: Product
        public ActionResult Index()
        {
            if (TempData["SearchKeyword"] != null)
            {
                TempData.Remove("SearchKeyword");
            }

            return RedirectToAction("Listing");
        }

        //GET: Listing [SA, GM, FN, IV]
        [RedirectingActionWithFNIV]
        public ActionResult Listing(int page = 1)
        {
            if (TempData["TempToken"] != null)
            {
                string path = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + TempData["TempToken"].ToString() + ".txt");

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                TempData.Remove("TempToken");
            }

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

            IPagedList<Product> products = _productsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
            ViewData["Product"] = products;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Listing [SA, GM, FN, IV]
        [RedirectingActionWithFNIV]
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

            IPagedList<Product> products = _productsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize);
            ViewData["Product"] = products;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Create [SA, GM]
        [RedirectingActionWithGMSA]
        public ActionResult Create()
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            string tempToken = EncryptionHelper.GenerateRandomString(12);

            if (TempData["TempToken"] == null)
            {
                tempToken = tempToken.Replace(@"\", "_");
                tempToken = tempToken.Replace(@"/", "_");
                TempData["TempToken"] = tempToken;
            }
            else
            {
                TempData.Keep("TempToken");
            }

            Dropdown[] decimalDDL = DecimalDDL(5);
            ViewData["DecimalDropdown"] = new SelectList(decimalDDL, "val", "name", "2");

            Dropdown[] unitDDL = UnitDDL();
            ViewData["UnitDropdown"] = new SelectList(unitDDL, "val", "name");

            ViewData["PendingCheckBox"] = "";
            ViewData["CashCheckBox"] = "";
            ViewData["ChequeCheckBox"] = "";
            ViewData["BankTransferCheckBox"] = "";
            ViewData["BuyCheckBox"] = "";
            ViewData["SellCheckBox"] = "";
            ViewData["EncashmentCheckBox"] = "";
            ViewData["SwapCheckBox"] = "";
            ViewData["CrossCurrencyCheckBox"] = "";
            ViewData["CurrencyExchangeYouPayCheckBox"] = "";
            ViewData["CurrencyExchangeYouGetCheckBox"] = "";
            ViewData["CashWithdrawalYouPayCheckBox"] = "";
            ViewData["CashWithdrawalYouGetCheckBox"] = "";

            Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name");

            Dropdown[] productInventoryDDL = ProductInventoryTypeDDL();
            ViewData["ProductInventoryTypeDropdown"] = new SelectList(productInventoryDDL, "val", "name");

			Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
			ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name");

			Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
			ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name");

			Dropdown[] encashmentMappingDDL = EncashmentMappingDDL(0);
			ViewData["EncashmentMappingDropdown"] = new SelectList(encashmentMappingDDL, "val", "name");

			ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Create [SA, GM]
        [RedirectingActionWithGMSA]
        [HttpPost]
        public ActionResult Create(Product products, ProductInventory productInventories, FormCollection form)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            if (!string.IsNullOrEmpty(products.CurrencyCode))
            {
                Product checkCode = _productsModel.FindCurrencyCode(products.CurrencyCode);

                if (checkCode != null)
                {
                    ModelState.AddModelError("products.CurrencyCode", products.CurrencyCode + " already existed!");
                }
            }

            if (products.BuyRate != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRate.ToString()))
                {
                    ModelState.AddModelError("products.BuyRate", "Buy Rate is not valid!");
                }
            }

            if (products.SellRate != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRate.ToString()))
                {
                    ModelState.AddModelError("products.SellRate", "Sell Rate is not valid!");
                }
            }

            if (products.MaxAmount != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.MaxAmount.ToString()))
                {
                    ModelState.AddModelError("products.MaxAmount", "Max Amount is not valid!");
                }
            }

            if (products.BuyRateAdjustment != null)
            {
				if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment is not valid!");
				}
				else
				{
					if (products.BuyRateAdjustment > 999)
					{
						ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment cannot more than 999!");
					}
				}
            }

            if (products.SellRateAdjustment != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRateAdjustment.ToString()))
                {
                    ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment is not valid!");
				}
				else
				{
					if (products.SellRateAdjustment > 999)
					{
						ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment cannot more than 999!");
					}
				}
			}

            if (!string.IsNullOrEmpty(products.EncashmentRate.ToString()))
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.EncashmentRate.ToString()))
                {
                    ModelState.AddModelError("products.EncashmentRate", "Encashment Rate is not valid!");
                }
            }

            if (string.IsNullOrEmpty(form["PaymentModeAllowed"]))
            {
                ModelState.AddModelError("products.PaymentModeAllowed", "Payment Mode Allowed is required!");
            }

            if (string.IsNullOrEmpty(form["TransactionTypeAllowed"]))
            {
                ModelState.AddModelError("products.TransactionTypeAllowed", "Transaction Type Allowed is required!");
            }

			//if (string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
			//{
			//	ModelState.AddModelError("products.TransactionTypeAllowedCustomer", "Transaction Type Allowed (Customer Portal) is required!");
			//}

			if (ModelState.IsValid)
            {
                products.PaymentModeAllowed = form["PaymentModeAllowed"].ToString();
                products.TransactionTypeAllowed = form["TransactionTypeAllowed"].ToString();

				if (!string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
				{
					products.TransactionTypeAllowedCustomer = form["TransactionTypeAllowedCustomer"].ToString();
				}

				bool result = _productsModel.Add(products);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Products";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Product";

                    bool product_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    productInventories.ProductId = products.ID;

                    bool result_productInventory = _productInventoriesModel.Add(productInventories);

                    if (result_productInventory)
                    {
                        tableAffected = "ProductInventories";
                        description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Product Inventory";

                        bool productInventory_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

					//Determine if there is any pending update inventory
					if (TempData["TempToken"] != null)
					{
						string path = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + TempData.Peek("TempToken").ToString() + ".txt");

						if (System.IO.File.Exists(path))
						{
							var lines = System.IO.File.ReadAllLines(path);

							bool result_inventory = false;

							foreach (string line in lines)
							{
								string[] data = line.Split('|');

								Inventory inventory = new Inventory();
								inventory.ProductId = products.ID;
								inventory.Type = data[0];
								inventory.Amount = Convert.ToDecimal(data[1]);
								inventory.Description = data[2];
								bool res = _inventoriesModel.Add(inventory);

								if (res)
								{
									if (!result_inventory)
									{
										result_inventory = true;
									}
								}
							}

							if (result_inventory)
							{
								tableAffected = "Inventories";
								description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Inventory";

								bool inventory_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
							}
						}

						if (System.IO.File.Exists(path))
						{
							System.IO.File.Delete(path);
						}
					}

                    //Determine if there is any product denomination
                    List<string> keys = form.AllKeys.Where(e => e.Contains("DenominationValue_")).ToList();

                    bool result_productDenomination = false;

                    foreach (string key in keys)
                    {
                        ProductDenomination denomination = new ProductDenomination();
                        denomination.ProductId = products.ID;
                        denomination.DenominationValue = Convert.ToInt32(form[key]);
                        bool res = _productDenominationsModel.Add(denomination);

                        if (res)
                        {
                            if (!result_productDenomination)
                            {
                                result_productDenomination = true;
                            }
                        }
                    }

                    if (result_productDenomination)
                    {
                        tableAffected = "ProductDenominations";
                        description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Product Denominations";

                        bool productDenomination_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    TempData.Remove("TempToken");

					TempData["Result"] = "success|" + products.CurrencyCode + " has been successfully created!";

					//TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully created!");

                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
					TempData["Result"] = "danger|An error occured while saving product record!";
					//TempData.Add("Result", "danger|An error occured while saving product record!");
                }
            }
            else
            {
				TempData["Result"] = "danger|There is something wrong in the form!";
				//TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            Dropdown[] decimalDDL = DecimalDDL(5);
            ViewData["DecimalDropdown"] = new SelectList(decimalDDL, "val", "name", products.Decimal.ToString());

            Dropdown[] unitDDL = UnitDDL();
            ViewData["UnitDropdown"] = new SelectList(unitDDL, "val", "name", products.Unit);

			Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
			ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

			Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
			ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

			Dropdown[] encashmentMappingDDL = EncashmentMappingDDL(0);
			ViewData["EncashmentMappingDropdown"] = new SelectList(encashmentMappingDDL, "val", "name", products.EncashmentMapping);

			//Determine Payment Mode Checkbox
			ViewData["PendingCheckBox"] = "";
            ViewData["CashCheckBox"] = "";
            ViewData["ChequeCheckBox"] = "";
            ViewData["BankTransferCheckBox"] = "";

            if (!string.IsNullOrEmpty(form["PaymentModeAllowed"]))
            {
                if (form["PaymentModeAllowed"].ToString().Contains("Pending"))
                {
                    ViewData["PendingCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Cash"))
                {
                    ViewData["CashCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Cheque"))
                {
                    ViewData["ChequeCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Bank Transfer"))
                {
                    ViewData["BankTransferCheckBox"] = "checked";
                }
           }

            //Determine Transaction Type Checkbox
            ViewData["BuyCheckBox"] = "";
            ViewData["SellCheckBox"] = "";
            ViewData["EncashmentCheckBox"] = "";
            ViewData["SwapCheckBox"] = "";
            ViewData["CrossCurrencyCheckBox"] = "";
            ViewData["CurrencyExchangeYouPayCheckBox"] = "";
            ViewData["CurrencyExchangeYouGetCheckBox"] = "";
            ViewData["CashWithdrawalYouPayCheckBox"] = "";
            ViewData["CashWithdrawalYouGetCheckBox"] = "";

            if (!string.IsNullOrEmpty(form["TransactionTypeAllowed"]))
            {
                if (form["TransactionTypeAllowed"].ToString().Contains("Buy"))
                {
                    ViewData["BuyCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Sell"))
                {
                    ViewData["SellCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Encashment"))
                {
                    ViewData["EncashmentCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Swap"))
                {
                    ViewData["SwapCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Cross Currency"))
                {
                    ViewData["CrossCurrencyCheckBox"] = "checked";
                }

				if (!string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
				{
					if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouPay"))
					{
						ViewData["CurrencyExchangeYouPayCheckBox"] = "checked";
					}

					if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouGet"))
					{
						ViewData["CurrencyExchangeYouGetCheckBox"] = "checked";
					}

					if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouPay"))
					{
						ViewData["CashWithdrawalYouPayCheckBox"] = "checked";
					}

					if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouGet"))
					{
						ViewData["CashWithdrawalYouGetCheckBox"] = "checked";
					}
				}
            }

			if (!string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
			{
				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouPay"))
				{
					ViewData["CurrencyExchangeYouPayCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouGet"))
				{
					ViewData["CurrencyExchangeYouGetCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouPay"))
				{
					ViewData["CashWithdrawalYouPayCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouGet"))
				{
					ViewData["CashWithdrawalYouGetCheckBox"] = "checked";
				}
			}

			Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

            Dropdown[] productInventoryDDL = ProductInventoryTypeDDL();
            ViewData["ProductInventoryTypeDropdown"] = new SelectList(productInventoryDDL, "val", "name");

            //Determine Product Denomination
            List<string> values = form.AllKeys.Where(e => e.Contains("DenominationValue_")).ToList();
            List<string> returnedValues = new List<string>();

            foreach (string val in values)
            {
                if (val != "DenominationValue_1")
                {
                    string id = val.Substring(18);
                    returnedValues.Add(id + "|" + form[val].ToString());
                }
            }

            ViewData["ReturnedDenomination"] = returnedValues;

            TempData.Keep("TempToken");

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Edit [SA, GM, FN, IV]
        [RedirectingActionWithFNIV]
        public ActionResult Edit(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            Product products = _productsModel.GetSingle(id);

            if (products != null)
            {
                string tempToken = EncryptionHelper.GenerateRandomString(12);
                tempToken = tempToken.Replace(@"\", "_");
                tempToken = tempToken.Replace(@"/", "_");

                if (TempData["FromPostBack"] == null)
                {
                    TempData["TempToken"] = tempToken;
                    //if (TempData["TempToken"] == null)
                    //{
                    //    TempData.Add("TempToken", tempToken);
                    //}
                    //else
                    //{
                    //    TempData["TempToken"] = tempToken;
                    //    //TempData.Keep("TempToken");
                    //}
                }
                else
                {
                    TempData["TempToken"] = tempToken;
                }

                Dropdown[] decimalDDL = DecimalDDL(5);
                ViewData["DecimalDropdown"] = new SelectList(decimalDDL, "val", "name", products.Decimal.ToString());

                Dropdown[] unitDDL = UnitDDL();
                ViewData["UnitDropdown"] = new SelectList(unitDDL, "val", "name", products.Unit);

				Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
				ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

				Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
				ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

				Dropdown[] encashmentMappingDDL = EncashmentMappingDDL(id);
				ViewData["EncashmentMappingDropdown"] = new SelectList(encashmentMappingDDL, "val", "name", products.EncashmentMapping);

				ViewData["PendingCheckBox"] = "";
                ViewData["CashCheckBox"] = "";
                ViewData["ChequeCheckBox"] = "";
                ViewData["BankTransferCheckBox"] = "";

                if (products.PaymentModeAllowed.Contains("Pending"))
                {
                    ViewData["PendingCheckBox"] = "checked";
                }

                if (products.PaymentModeAllowed.Contains("Cash"))
                {
                    ViewData["CashCheckBox"] = "checked";
                }

                if (products.PaymentModeAllowed.Contains("Cheque"))
                {
                    ViewData["ChequeCheckBox"] = "checked";
                }

                if (products.PaymentModeAllowed.Contains("Bank Transfer"))
                {
                    ViewData["BankTransferCheckBox"] = "checked";
                }

                ViewData["BuyCheckBox"] = "";
                ViewData["SellCheckBox"] = "";
                ViewData["EncashmentCheckBox"] = "";
                ViewData["SwapCheckBox"] = "";
                ViewData["CrossCurrencyCheckBox"] = "";
                ViewData["CurrencyExchangeYouPayCheckBox"] = "";
                ViewData["CurrencyExchangeYouGetCheckBox"] = "";
                ViewData["CashWithdrawalYouPayCheckBox"] = "";
                ViewData["CashWithdrawalYouGetCheckBox"] = "";                  
                
                if (products.TransactionTypeAllowed.Contains("Buy"))
                {
                    ViewData["BuyCheckBox"] = "checked";
                }

                if (products.TransactionTypeAllowed.Contains("Sell"))
                {
                    ViewData["SellCheckBox"] = "checked";
                }

                if (products.TransactionTypeAllowed.Contains("Encashment"))
                {
                    ViewData["EncashmentCheckBox"] = "checked";
                }

                if (products.TransactionTypeAllowed.Contains("Swap"))
                {
                    ViewData["SwapCheckBox"] = "checked";
                }

                if (products.TransactionTypeAllowed.Contains("Cross Currency"))
                {
                    ViewData["CrossCurrencyCheckBox"] = "checked";
                }

				if (products.TransactionTypeAllowedCustomer != null)
				{
					if (products.TransactionTypeAllowedCustomer.Contains("CurrencyExchangeYouPay"))
					{
						ViewData["CurrencyExchangeYouPayCheckBox"] = "checked";
					}

					if (products.TransactionTypeAllowedCustomer.Contains("CurrencyExchangeYouGet"))
					{
						ViewData["CurrencyExchangeYouGetCheckBox"] = "checked";
					}

					if (products.TransactionTypeAllowedCustomer.Contains("CashWithdrawalYouPay"))
					{
						ViewData["CashWithdrawalYouPayCheckBox"] = "checked";
					}

					if (products.TransactionTypeAllowedCustomer.Contains("CashWithdrawalYouGet"))
					{
						ViewData["CashWithdrawalYouGetCheckBox"] = "checked";
					}
				}

                Dropdown[] statusDDL = StatusDDL();
                ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

                Dropdown[] productInventoryDDL = ProductInventoryTypeDDL();
                ViewData["ProductInventoryTypeDropdown"] = new SelectList(productInventoryDDL, "val", "name");

                ViewData["Product"] = products;
                ViewData["ProductInventory"] = products.ProductInventories[0];
                ViewData["ProductId"] = id;
                ViewData["OriTotalInAccount"] = products.ProductInventories[0].TotalInAccount;

                if (products.ProductDenominations.Count > 0)
                {
                    products.ProductDenominations = products.ProductDenominations.OrderByDescending(e => e.DenominationValue).ToList();

                    List<string> returnedValues = new List<string>();

                    int count = 1;

                    foreach (ProductDenomination denomination in products.ProductDenominations)
                    {
                        returnedValues.Add(count + "|" + denomination.DenominationValue);
                        count++;
                    }

                    ViewData["ReturnedDenomination"] = returnedValues;
                }

                ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                return View();
            }
            else
            {
				TempData["Result"] = "danger|Product record not found!";

				//TempData.Add("Result", "danger|Product record not found!");
            }

            return RedirectToAction("Listing", new { @page = page });
        }

        //POST: Edit [SA, GM, FN, IV]
        [RedirectingActionWithFNIV]
        [HttpPost]
        public ActionResult Edit(int id, Product products, ProductInventory productInventories, FormCollection form)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            if (!string.IsNullOrEmpty(products.CurrencyCode))
            {
                Product checkCode = _productsModel.FindCurrencyCode(products.CurrencyCode);

                if (checkCode != null)
                {
                    if (checkCode.ID != id)
                    {
                        ModelState.AddModelError("products.CurrencyCode", products.CurrencyCode + " already existed!");
                    }
                }
            }

            if (products.BuyRate != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRate.ToString()))
                {
                    ModelState.AddModelError("products.BuyRate", "Buy Rate is not valid!");
                }
            }

            if (products.SellRate != null)
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRate.ToString()))
                {
                    ModelState.AddModelError("products.SellRate", "Sell Rate is not valid!");
                }
            }

            if (!string.IsNullOrEmpty(products.EncashmentRate.ToString()))
            {
                if (!FormValidationHelper.NonNegativeAmountValidation(products.EncashmentRate.ToString()))
                {
                    ModelState.AddModelError("products.EncashmentRate", "Encashment Rate is not valid!");
                }
            }

			if (products.BuyRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.BuyRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment is not valid!");
				}
				else
				{
					if (products.BuyRateAdjustment > 999)
					{
						ModelState.AddModelError("products.BuyRateAdjustment", "Buy Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (products.SellRateAdjustment != null)
			{
				if (!FormValidationHelper.NonNegativeAmountValidation(products.SellRateAdjustment.ToString()))
				{
					ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment is not valid!");
				}
				else
				{
					if (products.SellRateAdjustment > 999)
					{
						ModelState.AddModelError("products.SellRateAdjustment", "Sell Rate Adjustment cannot more than 999!");
					}
				}
			}

			if (string.IsNullOrEmpty(form["PaymentModeAllowed"]))
            {
                ModelState.AddModelError("products.PaymentModeAllowed", "Payment Mode Allowed is required!");
            }

            if (string.IsNullOrEmpty(form["TransactionTypeAllowed"]))
            {
                ModelState.AddModelError("products.TransactionTypeAllowed", "Transaction Type Allowed is required!");
            }

			//if (string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
			//{
			//	ModelState.AddModelError("products.TransactionTypeAllowedCustomer", "Transaction Type Allowed (Customer Portal) is required!");
			//}

			if (ModelState.IsValid)
            {
                products.PaymentModeAllowed = form["PaymentModeAllowed"].ToString();
                products.TransactionTypeAllowed = form["TransactionTypeAllowed"].ToString();

				if (!string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
				{
					products.TransactionTypeAllowedCustomer = form["TransactionTypeAllowedCustomer"].ToString();
				}

				bool result = _productsModel.Update(id, products);

                if (result)
                {
                    int userid = Convert.ToInt32(Session["UserId"]);
                    string tableAffected = "Products";
                    string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Product";

                    bool product_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                    ProductInventory oldProductInventory = _productInventoriesModel.GetProductInventory(id);

                    //calculate here instead of direct assign.
                    string getTempToken = "";

                    if (TempData["FromPostBack"] != null)
                    {
                        getTempToken = TempData.Peek("TempToken").ToString();
                    }
                    else
                    {
                        if (TempData["TempToken"] != null)
                        {
                            getTempToken = TempData.Peek("TempToken").ToString();
                        }
                            //getTempToken = TempData["TempToken"].ToString();
                    }

                    string GetTempPath = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + getTempToken + ".txt");

                    decimal total = 0;
                    if (System.IO.File.Exists(GetTempPath))
                    {
                    //Read from Text File
                    var GetTemplines = System.IO.File.ReadAllLines(GetTempPath);

                    //Calculate Total In Account
                    
                    foreach (string line in GetTemplines)
                    {
                        string[] data = line.Split('|');

                        if (data[0] == "Add")
                        {
                            total += Convert.ToDecimal(data[1]);
                        }
                        else
                        {
                            total -= Convert.ToDecimal(data[1]);
                        }
                    }
                    //End Here                        
                    }


                    //decimal PreventPostBackAmountIssue = productInventories.TotalInAccount;
                    //decimal TheDifferentAmount = PreventPostBackAmountIssue - total;

                    decimal GetLatestAmount = oldProductInventory.TotalInAccount + total;

                    oldProductInventory.TotalInAccount = GetLatestAmount;

                    bool result_productInventory = _productInventoriesModel.Update(oldProductInventory.ID, oldProductInventory);

                    if (result_productInventory)
                    {
                        tableAffected = "ProductInventories";
                        description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Updated Product Inventory";

                        bool productInventory_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

					//Determine if there is any pending update inventory
					if (TempData["TempToken"] != null)
					{
						string path = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + TempData.Peek("TempToken").ToString() + ".txt");

						if (System.IO.File.Exists(path))
						{
							var lines = System.IO.File.ReadAllLines(path);

							bool result_inventory = false;

							foreach (string line in lines)
							{
								string[] data = line.Split('|');

								Inventory inventory = new Inventory();
								inventory.ProductId = id;
								inventory.Type = data[0];
								inventory.Amount = Convert.ToDecimal(data[1]);
								inventory.Description = data[2] + " [After Bal: " + GetLatestAmount +"]";
								bool res = _inventoriesModel.Add(inventory);

								if (res)
								{
									if (!result_inventory)
									{
										result_inventory = true;
									}
								}
							}

							if (result_inventory)
							{
								tableAffected = "Inventories";
								description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Inventory";

								bool inventory_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
							}
						}

						if (System.IO.File.Exists(path))
						{
							System.IO.File.Delete(path);
						}
					}

                    //Determine any changes to Product Denominations
                    IList<ProductDenomination> oldProductDenominations = _productDenominationsModel.GetProductDenomination(id);

                    List<string> keys = form.AllKeys.Where(e => e.Contains("DenominationValue_")).ToList();

                    foreach (string key in keys.ToList())
                    {
                        int denominationValue = Convert.ToInt32(form[key].ToString());

                        ProductDenomination hasDenomination = oldProductDenominations.Where(e => e.DenominationValue == denominationValue).FirstOrDefault();

                        if (hasDenomination != null)
                        {
                            keys.Remove(key);
                            oldProductDenominations.Remove(hasDenomination);
                        }
                    }

                    //Delete Denomination if oldProductDenominations.Count > 0
                    bool delete_productDenomination = false;

                    foreach (ProductDenomination deleteDenomination in oldProductDenominations)
                    {
                        bool res_deleteDenomination = _productDenominationsModel.Delete(deleteDenomination.ID);

                        if (res_deleteDenomination)
                        {
                            if (!delete_productDenomination)
                            {
                                delete_productDenomination = true;
                            }
                        }
                    }

                    if (delete_productDenomination)
                    {
                        tableAffected = "ProductDenominations";
                        description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted Product Denominations";

                        bool productDenomination_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    //Create Denomination if keys.Count > 0
                    bool create_productDenomination = false;

                    foreach (string key in keys)
                    {
                        ProductDenomination denomination = new ProductDenomination();
                        denomination.ProductId = id;
                        denomination.DenominationValue = Convert.ToInt32(form[key]);
                        bool res = _productDenominationsModel.Add(denomination);

                        if (res)
                        {
                            if (!create_productDenomination)
                            {
                                create_productDenomination = true;
                            }
                        }
                    }

                    if (create_productDenomination)
                    {
                        tableAffected = "ProductDenominations";
                        description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Created Product Denominations";

                        bool productDenomination_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);
                    }

                    TempData.Remove("TempToken");

					TempData["Result"] = "success|" + products.CurrencyCode + " has been successfully updated!";

					//TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully updated!");

                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
					TempData["Result"] = "danger|An error occured while saving product record!";
					//TempData.Add("Result", "danger|An error occured while saving product record!");
                }
            }
            else
            {
				TempData["Result"] = "danger|There is something wrong in the form!";
				//TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            Dropdown[] decimalDDL = DecimalDDL(5);
            ViewData["DecimalDropdown"] = new SelectList(decimalDDL, "val", "name", products.Decimal.ToString());

            Dropdown[] unitDDL = UnitDDL();
            ViewData["UnitDropdown"] = new SelectList(unitDDL, "val", "name", products.Unit);

			Dropdown[] guaranteeRateDDL = GuaranteeRateDDL();
			ViewData["GuaranteeRateDropdown"] = new SelectList(guaranteeRateDDL, "val", "name", products.GuaranteeRates);

			Dropdown[] popularCurrencyDDL = PopularCurrencyDDL();
			ViewData["PopularCurrencyDropdown"] = new SelectList(popularCurrencyDDL, "val", "name", products.PopularCurrencies);

			Dropdown[] encashmentMappingDDL = EncashmentMappingDDL(id);
			ViewData["EncashmentMappingDropdown"] = new SelectList(encashmentMappingDDL, "val", "name", products.EncashmentMapping);

			//Determine Payment Mode Checkbox
			ViewData["PendingCheckBox"] = "";
            ViewData["CashCheckBox"] = "";
            ViewData["ChequeCheckBox"] = "";
            ViewData["BankTransferCheckBox"] = "";

            if (!string.IsNullOrEmpty(form["PaymentModeAllowed"]))
            {
                if (form["PaymentModeAllowed"].ToString().Contains("Pending"))
                {
                    ViewData["PendingCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Cash"))
                {
                    ViewData["CashCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Cheque"))
                {
                    ViewData["ChequeCheckBox"] = "checked";
                }

                if (form["PaymentModeAllowed"].ToString().Contains("Bank Transfer"))
                {
                    ViewData["BankTransferCheckBox"] = "checked";
                }
            }

            //Determine Transaction Type Checkbox
            ViewData["BuyCheckBox"] = "";
            ViewData["SellCheckBox"] = "";
            ViewData["EncashmentCheckBox"] = "";
            ViewData["SwapCheckBox"] = "";
            ViewData["CrossCurrencyCheckBox"] = "";
            ViewData["CurrencyExchangeYouPayCheckBox"] = "";
            ViewData["CurrencyExchangeYouGetCheckBox"] = "";
            ViewData["CashWithdrawalYouPayCheckBox"] = "";
            ViewData["CashWithdrawalYouGetCheckBox"] = "";

            if (!string.IsNullOrEmpty(form["TransactionTypeAllowed"]))
            {
                if (form["TransactionTypeAllowed"].ToString().Contains("Buy"))
                {
                    ViewData["BuyCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Sell"))
                {
                    ViewData["SellCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Encashment"))
                {
                    ViewData["EncashmentCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Swap"))
                {
                    ViewData["SwapCheckBox"] = "checked";
                }

                if (form["TransactionTypeAllowed"].ToString().Contains("Cross Currency"))
                {
                    ViewData["CrossCurrencyCheckBox"] = "checked";
                }
            }

			if (!string.IsNullOrEmpty(form["TransactionTypeAllowedCustomer"]))
			{
				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouPay"))
				{
					ViewData["CurrencyExchangeYouPayCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CurrencyExchangeYouGet"))
				{
					ViewData["CurrencyExchangeYouGetCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouPay"))
				{
					ViewData["CashWithdrawalYouPayCheckBox"] = "checked";
				}

				if (form["TransactionTypeAllowedCustomer"].ToString().Contains("CashWithdrawalYouGet"))
				{
					ViewData["CashWithdrawalYouGetCheckBox"] = "checked";
				}
			}

			Dropdown[] statusDDL = StatusDDL();
            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", products.Status);

            Dropdown[] productInventoryDDL = ProductInventoryTypeDDL();
            ViewData["ProductInventoryTypeDropdown"] = new SelectList(productInventoryDDL, "val", "name");

            ViewData["Product"] = products;
            ViewData["ProductInventory"] = productInventories;
            ViewData["ProductId"] = id;

            ViewData["OriTotalInAccount"] = productInventories.TotalInAccount;

            TempData["FromPostBack"] = "Yes";
            TempData["TempToken"] = TempData.Peek("TempToken").ToString();
            TempData.Keep("TempToken");

            //Determine Product Denomination
            List<string> values = form.AllKeys.Where(e => e.Contains("DenominationValue_")).ToList();
            List<string> returnedValues = new List<string>();

            foreach (string val in values)
            {
                string _id = val.Substring(18);
                returnedValues.Add(_id + "|" + form[val].ToString());
            }

            ViewData["ReturnedDenomination"] = returnedValues;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Delete [SA, GM]
        [RedirectingActionWithGMSA]
        public ActionResult Delete(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
            }

            Product products = _productsModel.GetSingle(id);

            if (products != null)
            {
                if (products.CurrencyCode != "SGD")
                {
                    using (var context = new DataAccess.GreatEastForex())
                    {
                        var param1 = new SqlParameter();
                        param1.ParameterName = "@id";
                        param1.SqlDbType = SqlDbType.Int;
                        param1.SqlValue = products.ID;

                        var record = context.CheckSalesCurrencyIDExists.SqlQuery("dbo.CheckSalesCurrencyIDExist @id", param1).Count();

                        if (record > 0)
                        {
                            //means have encashment mapping hook in sale
                            TempData["Result"] = "danger|This Product cannot delete because is in use in Sale.";
                        }
                        else
                        {
                            bool result = _productsModel.Delete(id);

                            if (result)
                            {
                                int userid = Convert.ToInt32(Session["UserId"]);
                                string tableAffected = "Products";
                                string description = Session["UserRole"].ToString() + " [" + Session["Username"].ToString() + "] Deleted Product";

                                bool product_log = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                                //TempData.Add("Result", "success|" + products.CurrencyCode + " has been successfully deleted!");
                                TempData["Result"] = "success|" + products.CurrencyCode + " has been successfully deleted.";
                            }
                            else
                            {
                                TempData["Result"] = "danger|An error occured while deleting product.";
                                //TempData.Add("Result", "danger|An error occured while deleting product!");
                            }
                        }
                    }
                }
                else
                {
					TempData["Result"] = "danger|SGD cannot be deleted.";
					//TempData.Add("Result", "danger|SGD cannot be deleted!");
                }
            }
            else
            {
				TempData["Result"] = "danger|Product record not found.";
				//TempData.Add("Result", "danger|Product record not found!");
            }

            return RedirectToAction("Listing", new { @page = page });
        }

        //GET: ValidateAmount
        public void ValidateAmount(string amount, string field)
        {
            bool result = FormValidationHelper.NonNegativeAmountValidation(amount);

            if (result)
            {
                Response.Write("{\"result\":\"true\", \"msg\":\"\"}");
            }
            else
            {
                Response.Write("{\"result\":\"false\", \"msg\":\"'" + amount + "' is not a valid " + field + "!\"}");
            }
            //TempData.Keep("TempToken");
        }

        //GET: FormatAmount
        public void FormatAmount(decimal amount, int dp, string field = "amount")
        {
            string amt = null;

            if (field == "rate")
            {
                amt = amount.ToString("#,##0.########");
            }
            else
            {
                amt = FormValidationHelper.AmountFormatter(amount, dp);
            }

            Response.Write("{\"result\":\"true\", \"msg\":\"" + amt + "\"}");
        }

        //GET: ViewLog
        [RedirectingActionWithFNIV]
        public ActionResult ViewLog(string symbol, int id = 0, int dp = 2, int page = 1)
        {
            TempData["Page"] = page;
            ViewData["Page"] = page;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["CustomerPageSize"]);

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;
            //From Existing Inventory Logs
            IList<Inventory> inventories = _inventoriesModel.GetAll(id);
            List<ViewLog> inventoryLogs = new List<ViewLog>();

            //foreach (Inventory inventory in inventories)
            //{
            //    ViewLog log = new ViewLog();
            //    log.ID = inventory.ProductId;
            //    log.Type = inventory.Type;
            //    log.Amount = inventory.Products.Symbol + FormValidationHelper.AmountFormatter(inventory.Amount, inventory.Products.Decimal);
            //    log.Description = inventory.Description;
            //    inventoryLogs.Add(log);
            //}
            var getPageCount = _inventoriesModel.GetTotalCount(id);
            var inventoryLogsPagi = _inventoriesModel.ViewlogPagination(page, id);

            ViewData["Paginations"] = getPageCount;
            ViewData["getPaging"] = page;
            ViewData["InventoryLog"] = inventoryLogsPagi;
            ViewData["Symbol"] = symbol;
            ViewData["currency_ID"] = id;
            ViewData["dp"] = dp;
            //ViewData["InventoryLog"] = inventoryLogs;

            //For Pending Update Inventory Logs
            List<ViewLog> pendingUpdateLogs = new List<ViewLog>();
			//Tempdata["TempTokenMissing"]

			if (TempData["TempToken"] != null)
			{
				string path = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + TempData["TempToken"].ToString() + ".txt");

				if (System.IO.File.Exists(path))
				{
					var lines = System.IO.File.ReadAllLines(path);

					foreach (string line in lines)
					{
						string[] data = line.Split('|');

						ViewLog log = new ViewLog();
						log.ID = id;
						log.Type = data[0];
						log.Amount = symbol + FormValidationHelper.AmountFormatter(Convert.ToDecimal(data[1]), dp);
						log.Description = data[2];
						pendingUpdateLogs.Add(log);
					}
				}
			}

            ViewData["PendingUpdateLogs"] = pendingUpdateLogs;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            TempData.Keep("TempToken");
            return View();
        }

        //Decimal Dropdown
        public Dropdown[] DecimalDDL(int count)
        {
            Dropdown[] ddl = new Dropdown[count];

            for (int i = 0; i < count; i++)
            {
                ddl[i] = new Dropdown { name = i.ToString(), val = i.ToString() };
            }

            return ddl;
        }

        //Unit Dropdown
        public Dropdown[] UnitDDL()
        {
            List<string> units = new List<string>() { "1|1", "10|10", "100|100", "1000|1000", "10000|10000" };

            Dropdown[] ddl = new Dropdown[units.Count];

            int count = 0;

            foreach (string unit in units)
            {
                string[] _unit = unit.Split('|');

                ddl[count] = new Dropdown { name = _unit[0], val = _unit[1] };

                count++;
            }

            return ddl;
        }

        //Status Dropdown
        public Dropdown[] StatusDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Active", val = "Active" };
            ddl[1] = new Dropdown { name = "Suspended", val = "Suspended" };
            return ddl;
        }

        //ProductInventoryType Dropdown
        public Dropdown[] ProductInventoryTypeDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Add", val = "Add" };
            ddl[1] = new Dropdown { name = "Deduct", val = "Deduct" };
            return ddl;
        }

		//Guarantee Rate Dropdown
		public Dropdown[] GuaranteeRateDDL()
		{
			Dropdown[] ddl = new Dropdown[2];
			ddl[0] = new Dropdown { name = "No", val = "0" };
			ddl[1] = new Dropdown { name = "Yes", val = "1" };
			return ddl;
		}

		//Popular Currency Dropdown
		public Dropdown[] PopularCurrencyDDL()
		{
			Dropdown[] ddl = new Dropdown[2];
			ddl[0] = new Dropdown { name = "No", val = "0" };
			ddl[1] = new Dropdown { name = "Yes", val = "1" };
			return ddl;
		}

		public Dropdown[] EncashmentMappingDDL(int id)
		{
			using (var context = new DataAccess.GreatEastForex())
			{
				var record = context.Products.Where(e => e.IsDeleted == "N");
				var FinalList = new List<Product>();

				if (id > 0)
				{
					FinalList = record.Where(e => e.ID != id).ToList();
				}
				else
				{
					FinalList = record.ToList();
				}

				Dropdown[] ddl = new Dropdown[FinalList.Count];
				int count = 0;

				foreach (var _record in FinalList)
				{
					ddl[count] = new Dropdown { name = _record.CurrencyCode + " - " + _record.CurrencyName, val = _record.ID.ToString() };
					count++;
				}

				return ddl;
			}
		}

		//UpdateInventory Temporarily Data
		public string TemporarilyInventoryData(string text, int dp = 2, string oriAmount = "")
        {
            string getTempToken = TempData["TempToken"].ToString();//TempData.Peek("TempToken").ToString();

            if (TempData["FromPostBack"] != null)
            {
                //this is from postback
                getTempToken = TempData.Peek("TempToken").ToString();
                TempData.Keep("FromPostBack");
                TempData["TempToken"] = getTempToken;
            }
            else
            {
                //this is not from postback
                //getTempToken = TempData["TempToken"].ToString();
                TempData["TempToken"] = getTempToken;
            }

            string path = Server.MapPath(ConfigurationManager.AppSettings["TextFilePath"].ToString() + "ProductInventory_" + getTempToken + ".txt");

            //Write to Text File
            using (StreamWriter write = System.IO.File.AppendText(path))
            {
                write.WriteLine(text);
            }

            //Read from Text File
            var lines = System.IO.File.ReadAllLines(path);

            //Calculate Total In Account
            decimal total = 0;

            if (TempData["FromPostBack"] != null)
            {
                string[] data = text.Split('|');

                if (data[0] == "Add")
                {
                    total += Convert.ToDecimal(data[1]);
                }
                else
                {
                    total -= Convert.ToDecimal(data[1]);
                }
            }
            else
            {
                foreach (string line in lines)
                {
                    string[] data = line.Split('|');

                    if (data[0] == "Add")
                    {
                        total += Convert.ToDecimal(data[1]);
                    }
                    else
                    {
                        total -= Convert.ToDecimal(data[1]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(oriAmount))
            {
                decimal amount = Convert.ToDecimal(oriAmount);

                total += amount;
            }

            return FormValidationHelper.AmountFormatter(total, dp);
        }
    }

    public class RedirectingActionWithGMSA : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(HttpContext.Current.Session["UserRole"].ToString().Contains("Super Admin") || HttpContext.Current.Session["UserRole"].ToString().Contains("General Manager")))
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

    public class RedirectingActionWithFNIV : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(HttpContext.Current.Session["UserRole"].ToString().Contains("Super Admin") || HttpContext.Current.Session["UserRole"].ToString().Contains("General Manager") || HttpContext.Current.Session["UserRole"].ToString().Contains("Finance") || HttpContext.Current.Session["UserRole"].ToString().Contains("Inventory")))
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