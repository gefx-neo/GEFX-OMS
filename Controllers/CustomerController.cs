﻿using DataAccess;
using DataAccess.POCO;
using GreatEastForex.Helper;
using GreatEastForex.Models;
using PagedList;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;

namespace GreatEastForex.Controllers
{
    [HandleError]
    [RedirectingAction]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerController : ControllerBase
    {
        private ICustomerParticularRepository _customerParticularsModel;
        private ICustomerSourceOfFundRepository _customerSourceOfFundsModel;
        private ICustomerActingAgentRepository _customerActingAgentsModel;
        private ICustomerAppointmentOfStaffRepository _customerAppointmentOfStaffsModel;
        private ICustomerDocumentCheckListRepository _customerDocumentCheckListsModel;
        private ICustomerScreeningReportRepository _customerScreeningReportsModel;
        private ICustomerOtherRepository _customerOthersModel;
        private ICustomerCustomRateRepository _customerCustomRatesModel;
        private IProductRepository _productsModel;
        private ICustomerActivityLogRepository _customerActivityLogsModel;
        private ISearchTagRepository _searchTagsModel;
        private ITemp_CustomerParticularRepository _TempCustomerParticularsModel;
        private ITemp_CustomerOtherRepository _TempCustomerOthersModel;
        private ITemp_CustomerSourceOfFundRepository _TempCustomerSourceOfFundsModel;
        private ITemp_CustomerActingAgentRepository _TempCustomerActingAgentsModel;
        private ITemp_CustomerAppointmentOfStaffRepository _TempCustomerAppointmentOfStaffsModel;
        private ITemp_CustomerDocumentCheckListRepository _TempCustomerDocumentsCheckListModel;
        private ITemp_CustomerScreeningReportRepository _TempCustomerScreeningReportsModel;
        private ITemp_CustomerActivityLogRepository _TempCustomerActivityLogsModel;
        private ITemp_CustomerCustomRateRepository _TempCustomerCustomRatesModel;
        private IKYC_CustomerParticularRepository _KYCCustomerParticularsModel;
        private IKYC_CustomerActingAgentRepository _KYCCustomerActingAgentsModel;
        private IKYC_CustomerDocumentCheckListRepository _KYCCustomerDocumentCheckListsModel;
        private IKYC_CustomerOtherRepository _KYCCustomerOthersModel;
        private IKYC_CustomerSourceOfFundRepository _KYCCustomerSourceOfFundsModel;
        private ICustomerRemittanceProductCustomRateRepository _CustomerRemittanceProductCustomRatesModel;
        private ITemp_CustomerRemittanceProductCustomRateRepository _TempCustomerRemittanceProductCustomRatesModel;

        public CustomerController()
            : this(new CustomerParticularRepository(), new CustomerSourceOfFundRepository(), new CustomerActingAgentRepository(), new CustomerAppointmentOfStaffRepository(), new CustomerDocumentCheckListRepository(), new CustomerScreeningReportRepository(), new CustomerOtherRepository(), new CustomerCustomRateRepository(), new ProductRepository(), new CustomerActivityLogRepository(), new SearchTagRepository(), new Temp_CustomerParticularRepository(), new Temp_CustomerOtherRepository(), new Temp_CustomerSourceOfFundRepository(), new Temp_CustomerActingAgentRepository(), new Temp_CustomerAppointmentOfStaffRepository(), new Temp_CustomerDocumentCheckListRepository(), new Temp_CustomerScreeningReportRepository(), new Temp_CustomerActivityLogRepository(), new Temp_CustomerCustomRateRepository(), new KYC_CustomerParticularRepository(), new KYC_CustomerActingAgentRepository(), new KYC_CustomerDocumentCheckListRepository(), new KYC_CustomerOtherRepository(), new KYC_CustomerSourceOfFundRepository(), new CustomerRemittanceProductCustomRateRepository(), new Temp_CustomerRemittanceProductCustomRateRepository())
        {

        }

        public CustomerController(ICustomerParticularRepository customerParticularsModel, ICustomerSourceOfFundRepository customerSourceOfFundsModel, ICustomerActingAgentRepository customerActingAgentsModel, ICustomerAppointmentOfStaffRepository customerAppointmentOfStaffsModel, ICustomerDocumentCheckListRepository customerDocumentCheckListsModel, ICustomerScreeningReportRepository customerScreeningReportsModel, ICustomerOtherRepository customerOthersModel, ICustomerCustomRateRepository customerCustomRatesModel, IProductRepository productsModel, ICustomerActivityLogRepository activityLogsModel, ISearchTagRepository searchTagsModel, ITemp_CustomerParticularRepository temp_CustomerParticularsModel, ITemp_CustomerOtherRepository temp_CustomerOthersModel, ITemp_CustomerSourceOfFundRepository temp_CustomerSourceOfFundsModel, ITemp_CustomerActingAgentRepository temp_CustomerActingAgentsModel, ITemp_CustomerAppointmentOfStaffRepository temp_CustomerAppointmentOfStaffsModel, ITemp_CustomerDocumentCheckListRepository temp_CustomerDocumentsCheckListModel, ITemp_CustomerScreeningReportRepository temp_CustomerScreeningReportsModel, ITemp_CustomerActivityLogRepository temp_CustomerActivityLogsModel, ITemp_CustomerCustomRateRepository temp_CustomerCustomRatesModel, IKYC_CustomerParticularRepository kyc_CustomerParticularsModel, IKYC_CustomerActingAgentRepository kyc_CustomerActingAgentsModel, IKYC_CustomerDocumentCheckListRepository kyc_CustomerDocumentCheckListsModel, IKYC_CustomerOtherRepository kyc_CustomerOthersModel, IKYC_CustomerSourceOfFundRepository kyc_CustomerSourceOfFundsModel, ICustomerRemittanceProductCustomRateRepository customerRemittanceProductCustomRateModel, ITemp_CustomerRemittanceProductCustomRateRepository temp_CustomerRemittanceProductCustomRateModel)
        {
            _customerParticularsModel = customerParticularsModel;
            _customerSourceOfFundsModel = customerSourceOfFundsModel;
            _customerActingAgentsModel = customerActingAgentsModel;
            _customerAppointmentOfStaffsModel = customerAppointmentOfStaffsModel;
            _customerDocumentCheckListsModel = customerDocumentCheckListsModel;
            _customerScreeningReportsModel = customerScreeningReportsModel;
            _customerOthersModel = customerOthersModel;
            _customerCustomRatesModel = customerCustomRatesModel;
            _productsModel = productsModel;
            _customerActivityLogsModel = activityLogsModel;
            _searchTagsModel = searchTagsModel;
            _TempCustomerParticularsModel = temp_CustomerParticularsModel;
            _TempCustomerOthersModel = temp_CustomerOthersModel;
            _TempCustomerSourceOfFundsModel = temp_CustomerSourceOfFundsModel;
            _TempCustomerActingAgentsModel = temp_CustomerActingAgentsModel;
            _TempCustomerAppointmentOfStaffsModel = temp_CustomerAppointmentOfStaffsModel;
            _TempCustomerDocumentsCheckListModel = temp_CustomerDocumentsCheckListModel;
            _TempCustomerScreeningReportsModel = temp_CustomerScreeningReportsModel;
            _TempCustomerActivityLogsModel = temp_CustomerActivityLogsModel;
            _TempCustomerCustomRatesModel = temp_CustomerCustomRatesModel;
            _KYCCustomerParticularsModel = kyc_CustomerParticularsModel;
            _KYCCustomerActingAgentsModel = kyc_CustomerActingAgentsModel;
            _KYCCustomerDocumentCheckListsModel = kyc_CustomerDocumentCheckListsModel;
            _KYCCustomerOthersModel = kyc_CustomerOthersModel;
            _KYCCustomerSourceOfFundsModel = kyc_CustomerSourceOfFundsModel;
            _CustomerRemittanceProductCustomRatesModel = customerRemittanceProductCustomRateModel;
            _TempCustomerRemittanceProductCustomRatesModel = temp_CustomerRemittanceProductCustomRateModel;
        }

        // GET: Customer
        public ActionResult Index()
        {
            if (TempData["SearchKeyword"] != null)
            {
                TempData.Remove("SearchKeyword");
            }

            return RedirectToAction("Listing");
        }

        // GET: Compare
        public ActionResult Compare(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            ViewBag.Title = "Compare";

            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            if (customerParticulars != null)
            {
                //Start Here
                if (customerParticulars.Others[0].Status != "Pending Approval")
                {
                    //Redirect to Listing page if the selected customer is not in pending approval.
                    TempData.Add("Result", "danger|Customer is not pending approval.");
                    return RedirectToAction("Listing", new { @page = page });
                }

                //Declare Active Record Here
                //Show Main Record
                CustomerSourceOfFund customerSourceOfFunds = (customerParticulars.SourceOfFunds.Count == 0 ? new CustomerSourceOfFund() : customerParticulars.SourceOfFunds[0]);
                CustomerActingAgent customerActingAgents = (customerParticulars.ActingAgents.Count == 0 ? new CustomerActingAgent() : customerParticulars.ActingAgents[0]);
                CustomerDocumentCheckList customerDocumentChecklists = (customerParticulars.DocumentCheckLists.Count == 0 ? new CustomerDocumentCheckList() : customerParticulars.DocumentCheckLists[0]);
                CustomerOther customerOthers = customerParticulars.Others[0];

                ViewData["CustomerParticular"] = customerParticulars;
                ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                ViewData["CustomerActingAgent"] = customerActingAgents;
                ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                ViewData["CustomerOther"] = customerOthers;
                ViewData["PendingApproval"] = "No";
                ViewData["CustomerID"] = customerParticulars.ID;
                ViewData["CustomerTitle"] = customerParticulars.Customer_Title;
                ViewData["EditStatus"] = "KYCNoVerifyCreatedFromOMS";
                ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                if (!string.IsNullOrEmpty(customerParticulars.EnableTransactionType))
                {
                    string[] type = customerParticulars.EnableTransactionType.Split(',');

                    if (Array.IndexOf(type, "Remittance") >= 0)
                    {
                        ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                    }

                    if (Array.IndexOf(type, "Currency Exchange") >= 0)
                    {
                        ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                    }

                    if (Array.IndexOf(type, "Withdrawal") >= 0)
                    {
                        ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                    }
                }

                //Start
                //Has Customer Account DDL
                Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", customerParticulars.hasCustomerAccount);

                Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                //Is Main Account DDL
                //0 - Main Account
                //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                if (customerParticulars.IsSubAccount == 0)
                {
                    //Main Account Customer DDL
                    Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                    ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                    ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                }
                else
                {
                    //have subaccount
                    Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(customerParticulars.IsSubAccount);
                    ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", customerParticulars.IsSubAccount.ToString());
                    ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                }
                //End

                //List Search Tag
                IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                ViewData["SearchTagList"] = getSearchTags;
                ViewData["SearchTagSelectedItem"] = "";

                if (!string.IsNullOrEmpty(customerParticulars.SearchTags))
                {
                    ViewData["SearchTagSelectedItem"] = customerParticulars.SearchTags.Replace("-", "");
                }

                Dropdown[] customerTypeDDL = CustomerTypeDDL();
                ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

                Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

                Dropdown[] gradingDDL = GradingDDL();
                ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                Dropdown[] customerProfileDDL = CustomerProfileDDL();
                ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                if (customerOthers.Status != "Pending Approval")
                {
                    Dropdown[] statusDDL = StatusDDL();
                    ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                }

                ViewData["CompanyForm"] = "";
                ViewData["NaturalForm"] = "";

                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    ViewData["NaturalForm"] = "display:none;";
                }
                else
                {
                    ViewData["CompanyForm"] = "display:none;";
                }

                ViewData["NaturalEmployedForm"] = "display:none;";
                ViewData["NaturalSelfEmployedForm"] = "display:none;";

                ViewData["NaturalEmploymentEmployedRadio"] = "";
                ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
                {
                    if (customerParticulars.Natural_EmploymentType == "Employed")
                    {
                        ViewData["NaturalEmployedForm"] = "";
                        ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                    }
                    else
                    {
                        ViewData["NaturalSelfEmployedForm"] = "";
                        ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                    }
                }

                //ViewData for Checkboxes and Radio buttons
                ViewData["CompanySOFBankCreditCheckbox"] = "";
                ViewData["CompanySOFInvestmentCheckbox"] = "";
                ViewData["CompanySOFOthersCheckbox"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                ViewData["NaturalSOFSalaryCheckbox"] = "";
                ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                ViewData["NaturalSOFSavingsCheckbox"] = "";
                ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                ViewData["NaturalSOFGiftCheckbox"] = "";
                ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                ViewData["NaturalSOFOthersCheckbox"] = "";
                ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                {
                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                        ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                        ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                        ViewData["CustomerDOB"] = "";
                        ViewData["CustomerCountry"] = customerParticulars.Company_Country;
                        ViewData["CustomerCountryCode"] = customerParticulars.Company_CountryCode;

                        ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                        if (customerParticulars.DOB != null)
                        {
                            ViewData["CustomerDOB"] = Convert.ToDateTime(customerParticulars.DOB).ToString("dd-MM-yyyy");
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                        {
                            if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                            }
                            else
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                        {
                            if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                            }
                            else
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                        {
                            if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                            }
                            else
                            {
                                ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                            }
                        }
                    }
                    else
                    {

                        ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                        ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                        ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                        ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                        {
                            if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                            {
                                ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                            }
                            else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                            {
                                ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                            }
                            else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                            {
                                ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                            }
                            else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                            {
                                ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                            }
                            else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                            {
                                ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                        {
                            if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                        {
                            if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                        {
                            if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                            }
                        }
                    }
                }

                ViewData["ActingAgentForm"] = "display:none;";
                ViewData["CompanyActingAgentYesRadio"] = "";
                ViewData["CompanyActingAgentNoRadio"] = "";
                ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                {
                    if (customerActingAgents.ActingAgent == "Yes")
                    {
                        ViewData["ActingAgentForm"] = "";
                        ViewData["CompanyActingAgentYesRadio"] = "checked";

                        if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                        {
                            if (customerActingAgents.Company_CustomerType == "Entity")
                            {
                                ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                            }
                            else
                            {
                                ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                            }
                        }
                    }
                    else
                    {
                        ViewData["CompanyActingAgentNoRadio"] = "checked";
                    }
                }

                if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                {
                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        List<CustomerAppointmentOfStaff> appointmentOfStaffs = customerParticulars.AppointmentOfStaffs;
                        List<string> personnels = new List<string>();
                        int count = 1;

                        foreach (CustomerAppointmentOfStaff appointment in appointmentOfStaffs)
                        {
                            //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                            personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                            count++;
                        }

                        ViewData["Personnels"] = personnels;
                    }
                }

                //ViewData for File Uploads Domain Folders
                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                {
                    ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                    string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                {
                    ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                    string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                {
                    ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                    string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                {
                    ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                    string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                {
                    ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                    string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                {
                    ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                    string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                {
                    ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                    string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                {
                    ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                    string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                {
                    ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                    string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                {
                    ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                    string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                {
                    ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                    string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                        count++;
                    }
                }

                if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                {
                    ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                    string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                    int count = 1;

                    foreach (string file in files)
                    {
                        ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                        count++;
                    }
                }

                List<string> bankAccountNos = new List<string>();

                if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                {
                    bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                }

                ViewData["BankAccountNo"] = bankAccountNos;

                List<string> screeningReports = new List<string>();

                foreach (CustomerScreeningReport screeningReport in customerParticulars.PEPScreeningReports)
                {
                    screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                }

                IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

                List<string> activityLogs = new List<string>();

                foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                {
                    activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                }

                ViewData["ActivityLog"] = activityLogs;

                IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                List<string> customRates = new List<string>();
                string GetBuyRate = "0";
                string GetSellRate = "0";
                string AdjustmentBuyRate = "0";
                string AdjustmentSellRate = "0";

                foreach (Product product in products)
                {
                    CustomerCustomRate customRate = _customerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                    GetBuyRate = "0";
                    GetSellRate = "0";
                    AdjustmentBuyRate = "0";
                    AdjustmentSellRate = "0";

                    if (product.BuyRate != 0 && product.BuyRate != null)
                    {
                        GetBuyRate = product.BuyRate.ToString();
                    }
                    else
                    {
                        if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                        {
                            GetBuyRate = product.AutomatedBuyRate.ToString();
                        }
                        else
                        {
                            GetBuyRate = "0";
                        }
                    }

                    if (product.SellRate != 0 && product.SellRate != null)
                    {
                        GetSellRate = product.SellRate.ToString();
                    }
                    else
                    {
                        if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                        {
                            GetSellRate = product.AutomatedSellRate.ToString();
                        }
                        else
                        {
                            GetSellRate = "0";
                        }
                    }

                    if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                    {
                        AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                    }
                    else
                    {
                        AdjustmentBuyRate = "0";
                    }

                    if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                    {
                        AdjustmentSellRate = product.SellRateAdjustment.ToString();
                    }
                    else
                    {
                        AdjustmentSellRate = "0";
                    }

                    if (customRate != null)
                    {
                        customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                        //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                    }
                    else
                    {
                        customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                        //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                        //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                    }
                }
                ViewData["CustomRates"] = customRates;

                using (var context = new DataAccess.GreatEastForex())
                {
                    List<string> remittanceProductCustomRates = new List<string>();

                    var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                    string RemittanceGetBuyRate = "0";
                    string RemittanceGetSellRate = "0";
                    string RemittanceAdjustmentBuyRate = "0";
                    string RemittanceAdjustmentSellRate = "0";

                    foreach (RemittanceProducts r in RemittanceProducts)
                    {

                        RemittanceGetBuyRate = "0";
                        RemittanceGetSellRate = "0";
                        RemittanceAdjustmentBuyRate = "0";
                        RemittanceAdjustmentSellRate = "0";

                        var checkRemittanceCustomRate = context.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                        if (r.PayRate != 0 && r.PayRate != null)
                        {
                            RemittanceGetBuyRate = r.PayRate.ToString();
                        }
                        else
                        {
                            if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                            {
                                RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                            }
                            else
                            {
                                RemittanceGetBuyRate = "0";
                            }
                        }

                        if (r.GetRate != 0 && r.GetRate != null)
                        {
                            RemittanceGetSellRate = r.GetRate.ToString();
                        }
                        else
                        {
                            if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                            {
                                RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                            }
                            else
                            {
                                RemittanceGetSellRate = "0";
                            }
                        }

                        if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                        {
                            RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                        }
                        else
                        {
                            RemittanceAdjustmentBuyRate = "0";
                        }

                        if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                        {
                            RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                        }
                        else
                        {
                            RemittanceAdjustmentSellRate = "0";
                        }

                        if (checkRemittanceCustomRate != null)
                        {
                            //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                            //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                            remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                        }
                        else
                        {
                            //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                            remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                        }
                    }

                    ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;
                }

                ViewData["PEPScreeningReports"] = screeningReports;
                ViewData["CustomerID"] = id;
                return View();
            }
            else
            {
                TempData.Add("Result", "danger|Customer record not found!");
            }

            return RedirectToAction("Listing", new { @page = page });
        }

        // GET: Compare (TempRecord)
        public ActionResult TempRecord(int id)
        {
            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            if (customerParticulars != null)
            {
                if (customerParticulars.Others[0].Status != "Pending Approval")
                {
                    //Redirect to Listing page if the selected customer is not in pending approval.
                    TempData.Add("Result", "danger|Customer is not pending approval.");
                    return RedirectToAction("Listing", new { @page = 1 });
                }

                //Start Here
                if (customerParticulars.Others[0].Status == "Pending Approval")
                {
                    var hasTempRecord = false;

                    //check have temp record or not first
                    using (var context2 = new DataAccess.GreatEastForex())
                    {
                        Temp_CustomerParticulars TempCustomerParticulars = context2.Temp_CustomerParticulars.Where(e => e.Customer_MainID == id).FirstOrDefault();

                        if (TempCustomerParticulars != null)
                        {
                            hasTempRecord = true;
                        }
                    }

                    if (hasTempRecord)
                    {
                        //Pending Approval, Show Temp Record
                        //This is Temp Table item
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            //This is verified and update from customre portal and pending approval status
                            Temp_CustomerParticulars TempCustomerParticulars = context.Temp_CustomerParticulars.Where(e => e.Customer_MainID == id).FirstOrDefault();
                            Temp_CustomerSourceOfFunds customerSourceOfFunds = context.Temp_CustomerSourceOfFunds.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();

                            if (customerSourceOfFunds == null)
                            {
                                customerSourceOfFunds = new Temp_CustomerSourceOfFunds();
                            }

                            Temp_CustomerActingAgents customerActingAgents = context.Temp_CustomerActingAgents.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();
                            if (customerActingAgents == null)
                            {
                                customerActingAgents = new Temp_CustomerActingAgents();
                            }

                            Temp_CustomerDocumentCheckLists customerDocumentChecklists = context.Temp_CustomerDocumentsCheckList.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();

                            if (customerDocumentChecklists == null)
                            {
                                customerDocumentChecklists = new Temp_CustomerDocumentCheckLists();
                            }

                            Temp_CustomerOthers customerOthers = context.Temp_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

                            ViewData["CustomerParticular"] = TempCustomerParticulars;
                            ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                            ViewData["CustomerActingAgent"] = customerActingAgents;
                            ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                            ViewData["CustomerOther"] = customerOthers;
                            ViewData["PendingApproval"] = "No";
                            ViewData["CustomerID"] = customerParticulars.ID;
                            ViewData["CustomerTitle"] = TempCustomerParticulars.Customer_Title;
                            ViewData["EditStatus"] = "KYCVerifiedAndReupdate";
                            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.EnableTransactionType))
                            {
                                string[] type = TempCustomerParticulars.EnableTransactionType.Split(',');

                                if (Array.IndexOf(type, "Remittance") >= 0)
                                {
                                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                                }

                                if (Array.IndexOf(type, "Currency Exchange") >= 0)
                                {
                                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                                }

                                if (Array.IndexOf(type, "Withdrawal") >= 0)
                                {
                                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                                }
                            }

                            //Start
                            //Has Customer Account DDL
                            Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                            ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", TempCustomerParticulars.hasCustomerAccount);

                            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", TempCustomerParticulars.isVerify);

                            //Is Main Account DDL
                            //0 - Main Account
                            //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                            Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                            if (customerParticulars.IsSubAccount == 0)
                            {
                                //Main Account Customer DDL
                                Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                                ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                                ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                            }
                            else
                            {
                                //Main Account Customer DDL
                                Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(TempCustomerParticulars.IsSubAccount);
                                ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", TempCustomerParticulars.IsSubAccount.ToString());
                                ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                            }
                            //End

                            //List Search Tag
                            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                            ViewData["SearchTagList"] = getSearchTags;
                            ViewData["SearchTagSelectedItem"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.SearchTags))
                            {
                                ViewData["SearchTagSelectedItem"] = TempCustomerParticulars.SearchTags.Replace("-", "");
                            }

                            if (customerOthers.Status == "Pending Approval")
                            {
                                ViewData["PendingApproval"] = "Yes";
                            }

                            Dropdown[] customerTypeDDL = CustomerTypeDDL();
                            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", TempCustomerParticulars.CustomerType);

                            Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                            ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", TempCustomerParticulars.Company_TypeOfEntity);

                            Dropdown[] gradingDDL = GradingDDL();
                            ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                            Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                            ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                            Dropdown[] customerProfileDDL = CustomerProfileDDL();
                            ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                            if (customerOthers.Status != "Pending Approval")
                            {
                                Dropdown[] statusDDL = StatusDDL();
                                ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                            }

                            ViewData["CompanyForm"] = "";
                            ViewData["NaturalForm"] = "";

                            if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                ViewData["NaturalForm"] = "display:none;";
                            }
                            else
                            {
                                ViewData["CompanyForm"] = "display:none;";
                            }

                            ViewData["NaturalEmployedForm"] = "display:none;";
                            ViewData["NaturalSelfEmployedForm"] = "display:none;";

                            ViewData["NaturalEmploymentEmployedRadio"] = "";
                            ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.Natural_EmploymentType))
                            {
                                if (TempCustomerParticulars.Natural_EmploymentType == "Employed")
                                {
                                    ViewData["NaturalEmployedForm"] = "";
                                    ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSelfEmployedForm"] = "";
                                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                                }
                            }

                            //ViewData for Checkboxes and Radio buttons
                            ViewData["CompanySOFBankCreditCheckbox"] = "";
                            ViewData["CompanySOFInvestmentCheckbox"] = "";
                            ViewData["CompanySOFOthersCheckbox"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                            ViewData["NaturalSOFSalaryCheckbox"] = "";
                            ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                            ViewData["NaturalSOFSavingsCheckbox"] = "";
                            ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                            ViewData["NaturalSOFGiftCheckbox"] = "";
                            ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                            ViewData["NaturalSOFOthersCheckbox"] = "";
                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.CustomerType))
                            {
                                if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                                {
                                    ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                                    ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                                    ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                                    ViewData["CustomerCountry"] = TempCustomerParticulars.Company_Country;
                                    ViewData["CustomerCountryCode"] = TempCustomerParticulars.Company_CountryCode;

                                    ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                                    ViewData["CustomerDOB"] = "";

                                    if (TempCustomerParticulars.DOB != null)
                                    {
                                        ViewData["CustomerDOB"] = Convert.ToDateTime(TempCustomerParticulars.DOB).ToString("dd-MM-yyyy");
                                    }

                                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                                    {
                                        ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;
                                    }

                                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                                    {
                                        ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                        }
                                    }
                                }
                                else
                                {
                                    ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                                    ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                                    ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                                    ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                                    {
                                        if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                        }
                                    }
                                }
                            }

                            ViewData["ActingAgentForm"] = "display:none;";
                            ViewData["CompanyActingAgentYesRadio"] = "";
                            ViewData["CompanyActingAgentNoRadio"] = "";
                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                            {
                                if (customerActingAgents.ActingAgent == "Yes")
                                {
                                    ViewData["ActingAgentForm"] = "";
                                    ViewData["CompanyActingAgentYesRadio"] = "checked";

                                    if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                                    {
                                        if (customerActingAgents.Company_CustomerType == "Entity")
                                        {
                                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                        }
                                    }
                                }
                                else
                                {
                                    ViewData["CompanyActingAgentNoRadio"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.CustomerType))
                            {
                                if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                                {
                                    List<Temp_CustomerAppointmentOfStaffs> appointmentOfStaffs = context.Temp_CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == id).ToList();
                                    List<string> personnels = new List<string>();
                                    int count = 1;

                                    foreach (Temp_CustomerAppointmentOfStaffs appointment in appointmentOfStaffs)
                                    {
                                        //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                        personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                        count++;
                                    }

                                    ViewData["Personnels"] = personnels;
                                }
                            }

                            //ViewData for File Uploads Domain Folders
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                            {
                                ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                                string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                            {
                                ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                                string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                            {
                                ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                                string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                            {
                                ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                                string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                            {
                                ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                                string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                            {
                                ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                                string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                            {
                                ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                                string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                            {
                                ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                                string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                            {
                                ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                                string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                            {
                                ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                                string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                            {
                                ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                                string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                            {
                                ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                                string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                                    count++;
                                }
                            }

                            List<string> bankAccountNos = new List<string>();

                            if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                            {
                                bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                            }

                            ViewData["BankAccountNo"] = bankAccountNos;

                            List<string> screeningReports = new List<string>();
                            List<Temp_CustomerScreeningReports> TempCustomerScreeningReports = context.Temp_CustomerScreeningReports.Where(e => e.CustomerParticularId == id).ToList();

                            if (TempCustomerScreeningReports.Count > 0)
                            {
                                foreach (Temp_CustomerScreeningReports screeningReport in TempCustomerScreeningReports)
                                {
                                    screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                                }
                            }

                            List<CustomerActivityLog> getActivityLogs = context.CustomerActivityLogs.Where(e => e.CustomerParticularId == id).ToList();

                            List<string> activityLogs = new List<string>();

                            if (getActivityLogs.Count > 0)
                            {
                                foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                                {
                                    activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                                }
                            }

                            ViewData["ActivityLog"] = activityLogs;

                            IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                            List<string> customRates = new List<string>();
                            string GetBuyRate = "0";
                            string GetSellRate = "0";
                            string AdjustmentBuyRate = "0";
                            string AdjustmentSellRate = "0";

                            foreach (Product product in products)
                            {
                                Temp_CustomerCustomRates customRate = _TempCustomerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                                GetBuyRate = "0";
                                GetSellRate = "0";
                                AdjustmentBuyRate = "0";
                                AdjustmentSellRate = "0";

                                if (product.BuyRate != 0 && product.BuyRate != null)
                                {
                                    GetBuyRate = product.BuyRate.ToString();
                                }
                                else
                                {
                                    if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                                    {
                                        GetBuyRate = product.AutomatedBuyRate.ToString();
                                    }
                                    else
                                    {
                                        GetBuyRate = "0";
                                    }
                                }

                                if (product.SellRate != 0 && product.SellRate != null)
                                {
                                    GetSellRate = product.SellRate.ToString();
                                }
                                else
                                {
                                    if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                                    {
                                        GetSellRate = product.AutomatedSellRate.ToString();
                                    }
                                    else
                                    {
                                        GetSellRate = "0";
                                    }
                                }

                                if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                                {
                                    AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    AdjustmentBuyRate = "0";
                                }

                                if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                                {
                                    AdjustmentSellRate = product.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    AdjustmentSellRate = "0";
                                }

                                if (customRate != null)
                                {
                                    customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                                }
                                else
                                {
                                    customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                                }
                            }
                            ViewData["CustomRates"] = customRates;

                            List<string> remittanceProductCustomRates = new List<string>();

                            var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                            string RemittanceGetBuyRate = "0";
                            string RemittanceGetSellRate = "0";
                            string RemittanceAdjustmentBuyRate = "0";
                            string RemittanceAdjustmentSellRate = "0";

                            foreach (RemittanceProducts r in RemittanceProducts)
                            {
                                RemittanceGetBuyRate = "0";
                                RemittanceGetSellRate = "0";
                                RemittanceAdjustmentBuyRate = "0";
                                RemittanceAdjustmentSellRate = "0";

                                var checkRemittanceCustomRate = context.Temp_CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                                if (r.PayRate != 0 && r.PayRate != null)
                                {
                                    RemittanceGetBuyRate = r.PayRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                    {
                                        RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetBuyRate = "0";
                                    }
                                }

                                if (r.GetRate != 0 && r.GetRate != null)
                                {
                                    RemittanceGetSellRate = r.GetRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                    {
                                        RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetSellRate = "0";
                                    }
                                }

                                if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                                {
                                    RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentBuyRate = "0";
                                }

                                if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                                {
                                    RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentSellRate = "0";
                                }

                                if (checkRemittanceCustomRate != null)
                                {
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                    remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                                else
                                {
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked");
                                    remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                            }

                            ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;

                            ViewData["PEPScreeningReports"] = screeningReports;
                            return View();
                        }
                    }
                    else
                    {
                        //no temp record, get main data
                        //Show Main Record
                        CustomerSourceOfFund customerSourceOfFunds = (customerParticulars.SourceOfFunds.Count == 0 ? new CustomerSourceOfFund() : customerParticulars.SourceOfFunds[0]);
                        CustomerActingAgent customerActingAgents = (customerParticulars.ActingAgents.Count == 0 ? new CustomerActingAgent() : customerParticulars.ActingAgents[0]);
                        CustomerDocumentCheckList customerDocumentChecklists = (customerParticulars.DocumentCheckLists.Count == 0 ? new CustomerDocumentCheckList() : customerParticulars.DocumentCheckLists[0]);
                        CustomerOther customerOthers = customerParticulars.Others[0];

                        ViewData["CustomerParticular"] = customerParticulars;
                        ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                        ViewData["CustomerActingAgent"] = customerActingAgents;
                        ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                        ViewData["CustomerOther"] = customerOthers;
                        ViewData["PendingApproval"] = "No";
                        ViewData["CustomerID"] = customerParticulars.ID;
                        ViewData["CustomerTitle"] = customerParticulars.Customer_Title;
                        ViewData["EditStatus"] = "KYCNoVerifyCreatedFromOMS";
                        ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                        ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                        ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.EnableTransactionType))
                        {
                            string[] type = customerParticulars.EnableTransactionType.Split(',');

                            if (Array.IndexOf(type, "Remittance") >= 0)
                            {
                                ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                            }

                            if (Array.IndexOf(type, "Currency Exchange") >= 0)
                            {
                                ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                            }

                            if (Array.IndexOf(type, "Withdrawal") >= 0)
                            {
                                ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                            }
                        }

                        //Start
                        //Has Customer Account DDL
                        Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                        ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", customerParticulars.hasCustomerAccount);

                        Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                        ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                        //Is Main Account DDL
                        //0 - Main Account
                        //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                        Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                        if (customerParticulars.IsSubAccount == 0)
                        {
                            //Main Account Customer DDL
                            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                        }
                        else
                        {
                            //have subaccount
                            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(customerParticulars.IsSubAccount);
                            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", customerParticulars.IsSubAccount.ToString());
                            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                        }
                        //End

                        //List Search Tag
                        IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                        ViewData["SearchTagList"] = getSearchTags;
                        ViewData["SearchTagSelectedItem"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.SearchTags))
                        {
                            ViewData["SearchTagSelectedItem"] = customerParticulars.SearchTags.Replace("-", "");
                        }

                        if (customerOthers.Status == "Pending Approval")
                        {
                            ViewData["PendingApproval"] = "Yes";
                        }

                        Dropdown[] customerTypeDDL = CustomerTypeDDL();
                        ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

                        Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                        ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

                        Dropdown[] gradingDDL = GradingDDL();
                        ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                        Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                        ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                        Dropdown[] customerProfileDDL = CustomerProfileDDL();
                        ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                        if (customerOthers.Status != "Pending Approval")
                        {
                            Dropdown[] statusDDL = StatusDDL();
                            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                        }

                        ViewData["CompanyForm"] = "";
                        ViewData["NaturalForm"] = "";

                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            ViewData["NaturalForm"] = "display:none;";
                        }
                        else
                        {
                            ViewData["CompanyForm"] = "display:none;";
                        }

                        ViewData["NaturalEmployedForm"] = "display:none;";
                        ViewData["NaturalSelfEmployedForm"] = "display:none;";

                        ViewData["NaturalEmploymentEmployedRadio"] = "";
                        ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
                        {
                            if (customerParticulars.Natural_EmploymentType == "Employed")
                            {
                                ViewData["NaturalEmployedForm"] = "";
                                ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSelfEmployedForm"] = "";
                                ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                            }
                        }

                        //ViewData for Checkboxes and Radio buttons
                        ViewData["CompanySOFBankCreditCheckbox"] = "";
                        ViewData["CompanySOFInvestmentCheckbox"] = "";
                        ViewData["CompanySOFOthersCheckbox"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                        ViewData["NaturalSOFSalaryCheckbox"] = "";
                        ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                        ViewData["NaturalSOFSavingsCheckbox"] = "";
                        ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                        ViewData["NaturalSOFGiftCheckbox"] = "";
                        ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                        ViewData["NaturalSOFOthersCheckbox"] = "";
                        ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                        ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                        {
                            if (customerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                                ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                                ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                                ViewData["CustomerDOB"] = "";
                                ViewData["CustomerCountry"] = customerParticulars.Company_Country;
                                ViewData["CustomerCountryCode"] = customerParticulars.Company_CountryCode;

                                ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                                if (customerParticulars.DOB != null)
                                {
                                    ViewData["CustomerDOB"] = Convert.ToDateTime(customerParticulars.DOB).ToString("dd-MM-yyyy");
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                    }
                                }
                            }
                            else
                            {

                                ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                                ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                                ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                                ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                                {
                                    if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                    }
                                }
                            }
                        }

                        ViewData["ActingAgentForm"] = "display:none;";
                        ViewData["CompanyActingAgentYesRadio"] = "";
                        ViewData["CompanyActingAgentNoRadio"] = "";
                        ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                        ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                        if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                        {
                            if (customerActingAgents.ActingAgent == "Yes")
                            {
                                ViewData["ActingAgentForm"] = "";
                                ViewData["CompanyActingAgentYesRadio"] = "checked";

                                if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                                {
                                    if (customerActingAgents.Company_CustomerType == "Entity")
                                    {
                                        ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                    }
                                }
                            }
                            else
                            {
                                ViewData["CompanyActingAgentNoRadio"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                        {
                            if (customerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                List<CustomerAppointmentOfStaff> appointmentOfStaffs = customerParticulars.AppointmentOfStaffs;
                                List<string> personnels = new List<string>();
                                int count = 1;

                                foreach (CustomerAppointmentOfStaff appointment in appointmentOfStaffs)
                                {
                                    //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                    personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                    count++;
                                }

                                ViewData["Personnels"] = personnels;
                            }
                        }

                        //ViewData for File Uploads Domain Folders
                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                        {
                            ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                            string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                        {
                            ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                            string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                        {
                            ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                            string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                        {
                            ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                            string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                        {
                            ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                            string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                        {
                            ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                            string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                        {
                            ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                            string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                        {
                            ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                            string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                        {
                            ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                            string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                        {
                            ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                            string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                        {
                            ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                            string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                        {
                            ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                            string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                                count++;
                            }
                        }

                        List<string> bankAccountNos = new List<string>();

                        if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                        {
                            bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                        }

                        ViewData["BankAccountNo"] = bankAccountNos;

                        List<string> screeningReports = new List<string>();

                        foreach (CustomerScreeningReport screeningReport in customerParticulars.PEPScreeningReports)
                        {
                            screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                        }

                        IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

                        List<string> activityLogs = new List<string>();

                        foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                        {
                            activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                        }

                        ViewData["ActivityLog"] = activityLogs;

                        IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                        List<string> customRates = new List<string>();
                        string GetBuyRate = "0";
                        string GetSellRate = "0";
                        string AdjustmentBuyRate = "0";
                        string AdjustmentSellRate = "0";

                        foreach (Product product in products)
                        {
                            CustomerCustomRate customRate = _customerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                            GetBuyRate = "0";
                            GetSellRate = "0";
                            AdjustmentBuyRate = "0";
                            AdjustmentSellRate = "0";

                            if (product.BuyRate != 0 && product.BuyRate != null)
                            {
                                GetBuyRate = product.BuyRate.ToString();
                            }
                            else
                            {
                                if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                                {
                                    GetBuyRate = product.AutomatedBuyRate.ToString();
                                }
                                else
                                {
                                    GetBuyRate = "0";
                                }
                            }

                            if (product.SellRate != 0 && product.SellRate != null)
                            {
                                GetSellRate = product.SellRate.ToString();
                            }
                            else
                            {
                                if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                                {
                                    GetSellRate = product.AutomatedSellRate.ToString();
                                }
                                else
                                {
                                    GetSellRate = "0";
                                }
                            }

                            if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                            {
                                AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                            }
                            else
                            {
                                AdjustmentBuyRate = "0";
                            }

                            if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                            {
                                AdjustmentSellRate = product.SellRateAdjustment.ToString();
                            }
                            else
                            {
                                AdjustmentSellRate = "0";
                            }

                            if (customRate != null)
                            {
                                customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                            }
                            else
                            {
                                customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                            }
                        }
                        ViewData["CustomRates"] = customRates;

                        using (var context = new DataAccess.GreatEastForex())
                        {
                            List<string> remittanceProductCustomRates = new List<string>();

                            var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                            string RemittanceGetBuyRate = "0";
                            string RemittanceGetSellRate = "0";
                            string RemittanceAdjustmentBuyRate = "0";
                            string RemittanceAdjustmentSellRate = "0";

                            foreach (RemittanceProducts r in RemittanceProducts)
                            {

                                RemittanceGetBuyRate = "0";
                                RemittanceGetSellRate = "0";
                                RemittanceAdjustmentBuyRate = "0";
                                RemittanceAdjustmentSellRate = "0";

                                var checkRemittanceCustomRate = context.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                                if (r.PayRate != 0 && r.PayRate != null)
                                {
                                    RemittanceGetBuyRate = r.PayRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                    {
                                        RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetBuyRate = "0";
                                    }
                                }

                                if (r.GetRate != 0 && r.GetRate != null)
                                {
                                    RemittanceGetSellRate = r.GetRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                    {
                                        RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetSellRate = "0";
                                    }
                                }

                                if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                                {
                                    RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentBuyRate = "0";
                                }

                                if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                                {
                                    RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentSellRate = "0";
                                }

                                if (checkRemittanceCustomRate != null)
                                {
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                    remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                                else
                                {
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                    remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                            }

                            ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;
                        }

                        ViewData["PEPScreeningReports"] = screeningReports;
                        return View();
                    }
                }
                else
                {
                    //Show Main Record
                    CustomerSourceOfFund customerSourceOfFunds = (customerParticulars.SourceOfFunds.Count == 0 ? new CustomerSourceOfFund() : customerParticulars.SourceOfFunds[0]);
                    CustomerActingAgent customerActingAgents = (customerParticulars.ActingAgents.Count == 0 ? new CustomerActingAgent() : customerParticulars.ActingAgents[0]);
                    CustomerDocumentCheckList customerDocumentChecklists = (customerParticulars.DocumentCheckLists.Count == 0 ? new CustomerDocumentCheckList() : customerParticulars.DocumentCheckLists[0]);
                    CustomerOther customerOthers = customerParticulars.Others[0];

                    ViewData["CustomerParticular"] = customerParticulars;
                    ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                    ViewData["CustomerActingAgent"] = customerActingAgents;
                    ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                    ViewData["CustomerOther"] = customerOthers;
                    ViewData["PendingApproval"] = "No";
                    ViewData["CustomerID"] = customerParticulars.ID;
                    ViewData["CustomerTitle"] = customerParticulars.Customer_Title;
                    ViewData["EditStatus"] = "KYCNoVerifyCreatedFromOMS";
                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.EnableTransactionType))
                    {
                        string[] type = customerParticulars.EnableTransactionType.Split(',');

                        if (Array.IndexOf(type, "Remittance") >= 0)
                        {
                            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                        }

                        if (Array.IndexOf(type, "Currency Exchange") >= 0)
                        {
                            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                        }

                        if (Array.IndexOf(type, "Withdrawal") >= 0)
                        {
                            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                        }
                    }

                    //Start
                    //Has Customer Account DDL
                    Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                    ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", customerParticulars.hasCustomerAccount);

                    Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                    ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                    //Is Main Account DDL
                    //0 - Main Account
                    //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                    Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                    if (customerParticulars.IsSubAccount == 0)
                    {
                        //Main Account Customer DDL
                        Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                        ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                        ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                    }
                    else
                    {
                        //have subaccount
                        Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(customerParticulars.IsSubAccount);
                        ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", customerParticulars.IsSubAccount.ToString());
                        ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                    }
                    //End

                    //List Search Tag
                    IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                    ViewData["SearchTagList"] = getSearchTags;
                    ViewData["SearchTagSelectedItem"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.SearchTags))
                    {
                        ViewData["SearchTagSelectedItem"] = customerParticulars.SearchTags.Replace("-", "");
                    }

                    if (customerOthers.Status == "Pending Approval")
                    {
                        ViewData["PendingApproval"] = "Yes";
                    }

                    Dropdown[] customerTypeDDL = CustomerTypeDDL();
                    ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

                    Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                    ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

                    Dropdown[] gradingDDL = GradingDDL();
                    ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                    Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                    ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                    Dropdown[] customerProfileDDL = CustomerProfileDDL();
                    ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                    if (customerOthers.Status != "Pending Approval")
                    {
                        Dropdown[] statusDDL = StatusDDL();
                        ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                    }

                    ViewData["CompanyForm"] = "";
                    ViewData["NaturalForm"] = "";

                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        ViewData["NaturalForm"] = "display:none;";
                    }
                    else
                    {
                        ViewData["CompanyForm"] = "display:none;";
                    }

                    ViewData["NaturalEmployedForm"] = "display:none;";
                    ViewData["NaturalSelfEmployedForm"] = "display:none;";

                    ViewData["NaturalEmploymentEmployedRadio"] = "";
                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
                    {
                        if (customerParticulars.Natural_EmploymentType == "Employed")
                        {
                            ViewData["NaturalEmployedForm"] = "";
                            ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSelfEmployedForm"] = "";
                            ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                        }
                    }

                    //ViewData for Checkboxes and Radio buttons
                    ViewData["CompanySOFBankCreditCheckbox"] = "";
                    ViewData["CompanySOFInvestmentCheckbox"] = "";
                    ViewData["CompanySOFOthersCheckbox"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                    ViewData["NaturalSOFSalaryCheckbox"] = "";
                    ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                    ViewData["NaturalSOFSavingsCheckbox"] = "";
                    ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                    ViewData["NaturalSOFGiftCheckbox"] = "";
                    ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                    ViewData["NaturalSOFOthersCheckbox"] = "";
                    ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                    ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                    {
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                            ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                            ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                            ViewData["CustomerDOB"] = "";
                            ViewData["CustomerCountry"] = customerParticulars.Company_Country;
                            ViewData["CustomerCountryCode"] = customerParticulars.Company_CountryCode;

                            ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                            if (customerParticulars.DOB != null)
                            {
                                ViewData["CustomerDOB"] = Convert.ToDateTime(customerParticulars.DOB).ToString("dd-MM-yyyy");
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                }
                            }
                        }
                        else
                        {

                            ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                            ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                            ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                            ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                            {
                                if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                {
                                    ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                {
                                    ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                {
                                    ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                {
                                    ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                {
                                    ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                }
                            }
                        }
                    }

                    ViewData["ActingAgentForm"] = "display:none;";
                    ViewData["CompanyActingAgentYesRadio"] = "";
                    ViewData["CompanyActingAgentNoRadio"] = "";
                    ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                    ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                    if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                    {
                        if (customerActingAgents.ActingAgent == "Yes")
                        {
                            ViewData["ActingAgentForm"] = "";
                            ViewData["CompanyActingAgentYesRadio"] = "checked";

                            if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                            {
                                if (customerActingAgents.Company_CustomerType == "Entity")
                                {
                                    ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                }
                            }
                        }
                        else
                        {
                            ViewData["CompanyActingAgentNoRadio"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                    {
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            List<CustomerAppointmentOfStaff> appointmentOfStaffs = customerParticulars.AppointmentOfStaffs;
                            List<string> personnels = new List<string>();
                            int count = 1;

                            foreach (CustomerAppointmentOfStaff appointment in appointmentOfStaffs)
                            {
                                //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                count++;
                            }

                            ViewData["Personnels"] = personnels;
                        }
                    }

                    //ViewData for File Uploads Domain Folders
                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                    {
                        ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                        string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                    {
                        ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                        string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                    {
                        ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                        string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                    {
                        ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                        string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                    {
                        ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                        string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                    {
                        ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                        string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                    {
                        ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                        string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                    {
                        ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                        string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                    {
                        ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                        string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                    {
                        ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                        string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                    {
                        ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                        string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                    {
                        ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                        string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                            count++;
                        }
                    }

                    List<string> bankAccountNos = new List<string>();

                    if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                    {
                        bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                    }

                    ViewData["BankAccountNo"] = bankAccountNos;

                    List<string> screeningReports = new List<string>();

                    foreach (CustomerScreeningReport screeningReport in customerParticulars.PEPScreeningReports)
                    {
                        screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                    }

                    IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

                    List<string> activityLogs = new List<string>();

                    foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                    {
                        activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                    }

                    ViewData["ActivityLog"] = activityLogs;

                    IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                    List<string> customRates = new List<string>();
                    string GetBuyRate = "0";
                    string GetSellRate = "0";
                    string AdjustmentBuyRate = "0";
                    string AdjustmentSellRate = "0";

                    foreach (Product product in products)
                    {
                        CustomerCustomRate customRate = _customerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                        GetBuyRate = "0";
                        GetSellRate = "0";
                        AdjustmentBuyRate = "0";
                        AdjustmentSellRate = "0";

                        if (product.BuyRate != 0 && product.BuyRate != null)
                        {
                            GetBuyRate = product.BuyRate.ToString();
                        }
                        else
                        {
                            if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                            {
                                GetBuyRate = product.AutomatedBuyRate.ToString();
                            }
                            else
                            {
                                GetBuyRate = "0";
                            }
                        }

                        if (product.SellRate != 0 && product.SellRate != null)
                        {
                            GetSellRate = product.SellRate.ToString();
                        }
                        else
                        {
                            if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                            {
                                GetSellRate = product.AutomatedSellRate.ToString();
                            }
                            else
                            {
                                GetSellRate = "0";
                            }
                        }

                        if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                        {
                            AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                        }
                        else
                        {
                            AdjustmentBuyRate = "0";
                        }

                        if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                        {
                            AdjustmentSellRate = product.SellRateAdjustment.ToString();
                        }
                        else
                        {
                            AdjustmentSellRate = "0";
                        }

                        if (customRate != null)
                        {
                            customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                        }
                        else
                        {
                            customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                        }
                    }
                    ViewData["CustomRates"] = customRates;

                    using (var context = new DataAccess.GreatEastForex())
                    {
                        List<string> remittanceProductCustomRates = new List<string>();

                        var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                        string RemittanceGetBuyRate = "0";
                        string RemittanceGetSellRate = "0";
                        string RemittanceAdjustmentBuyRate = "0";
                        string RemittanceAdjustmentSellRate = "0";

                        foreach (RemittanceProducts r in RemittanceProducts)
                        {

                            RemittanceGetBuyRate = "0";
                            RemittanceGetSellRate = "0";
                            RemittanceAdjustmentBuyRate = "0";
                            RemittanceAdjustmentSellRate = "0";

                            var checkRemittanceCustomRate = context.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                            if (r.PayRate != 0 && r.PayRate != null)
                            {
                                RemittanceGetBuyRate = r.PayRate.ToString();
                            }
                            else
                            {
                                if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                {
                                    RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                }
                                else
                                {
                                    RemittanceGetBuyRate = "0";
                                }
                            }

                            if (r.GetRate != 0 && r.GetRate != null)
                            {
                                RemittanceGetSellRate = r.GetRate.ToString();
                            }
                            else
                            {
                                if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                {
                                    RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                }
                                else
                                {
                                    RemittanceGetSellRate = "0";
                                }
                            }

                            if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                            {
                                RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                            }
                            else
                            {
                                RemittanceAdjustmentBuyRate = "0";
                            }

                            if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                            {
                                RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                            }
                            else
                            {
                                RemittanceAdjustmentSellRate = "0";
                            }

                            if (checkRemittanceCustomRate != null)
                            {
                                //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                            }
                            else
                            {
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                            }
                        }

                        ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;
                    }

                    ViewData["PEPScreeningReports"] = screeningReports;
                    return View();
                }
                //End Heres
            }
            else
            {
                TempData.Add("Result", "danger|Customer record not found!");
            }

            return View();
        }

        //GET: Listing [DL, FN, IV, OM, CS, GM, SA]
        [RedirectingActionWithDLFNIVOMCSGMSA]
        public ActionResult Listing(int page = 1)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["CustomerPageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            ViewData["SearchKeyword"] = "";

            if (TempData["SearchKeyword"] != null)
            {
                ViewData["SearchKeyword"] = TempData["SearchKeyword"];
            }

            //List Search Tag
            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
            ViewData["SearchTagList"] = getSearchTags;
            ViewData["SearchTagSelectedItem"] = "0";

            //check if is only customer viewer or is multiple role
            string userRole = Session["UserRole"].ToString();
            string[] userRoleList = userRole.Split(',');
            int userid = Convert.ToInt32(Session["UserId"].ToString());

            if (Array.IndexOf(userRoleList, "Customer Viewer") >= 0)
            {
                if (userRoleList.Length > 1)
                {
                    //this is multiple role
                    IPagedList<GetAllCustomerActiveList> customerParticulars = _customerParticularsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize, ViewData["SearchTagSelectedItem"].ToString());
                    ViewData["CustomerParticular"] = customerParticulars;

                    ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                    return View();
                }
                else
                {
                    ViewData["CustomerParticular"] = null;

                    ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                    return View();
                }
            }
            else
            {
                //this is not customer viewer
                IPagedList<GetAllCustomerActiveList> customerParticulars = _customerParticularsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize, ViewData["SearchTagSelectedItem"].ToString());
                ViewData["CustomerParticular"] = customerParticulars;

                ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                return View();
            }
        }

        //POST: Listing [DL, FN, IV, OM, CS, GM, SA]
        [RedirectingActionWithDLFNIVOMCSGMSA]
        [HttpPost]
        public ActionResult Listing(FormCollection form)
        {
            int page = 1;
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["CustomerPageSize"]);

            TempData["Page"] = page;
            ViewData["Page"] = page;

            TempData["PageSize"] = pageSize;
            ViewData["PageSize"] = pageSize;

            ViewData["SearchKeyword"] = form["SearchKeyword"].Trim();
            TempData["SearchKeyword"] = form["SearchKeyword"].Trim();

            //List Search Tag
            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
            ViewData["SearchTagList"] = getSearchTags;
            ViewData["SearchTagSelectedItem"] = "";

            if (!string.IsNullOrEmpty(form["Search_Tag_Dropdown_Form"]))
            {
                ViewData["SearchTagSelectedItem"] = form["Search_Tag_Dropdown_Form"];
            }

            IPagedList<GetAllCustomerActiveList> customerParticulars = _customerParticularsModel.GetPaged(ViewData["SearchKeyword"].ToString(), page, pageSize, ViewData["SearchTagSelectedItem"].ToString());
            ViewData["CustomerParticular"] = customerParticulars;

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //GET: Create [DL, GM, SA]
        [RedirectingActionWithDLGMSA]
        public ActionResult Create()
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name");

            Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
            ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name");

            Dropdown[] gradingDDL = GradingDDL();
            ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", "Medium");

            Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
            ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name");

            Dropdown[] customerProfileDDL = CustomerProfileDDL();
            ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", "Complete");

            //Has Customer Account DDL
            Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
            ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name");

            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name");

            //Is Main Account DDL
            //0 - Main Account
            //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
            Dropdown[] isMainAccountDDL = IsMainAccountDDL();
            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name");

            //Main Account Customer DDL
            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL();
            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");

            ViewData["CompanyForm"] = "";
            ViewData["NaturalForm"] = "display:none;";
            ViewData["NaturalEmployedForm"] = "display:none;";
            ViewData["NaturalSelfEmployedForm"] = "display:none;";
            ViewData["ActingAgentForm"] = "display:none;";
            ViewData["CustomerTitle"] = "";

            IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
            List<string> customRates = new List<string>();

            string GetBuyRate = "0";
            string GetSellRate = "0";
            string AdjustmentBuyRate = "0";
            string AdjustmentSellRate = "0";

            foreach (Product product in products)
            {
                GetBuyRate = "0";
                GetSellRate = "0";
                AdjustmentBuyRate = "0";
                AdjustmentSellRate = "0";

                if (product.BuyRate != 0 && product.BuyRate != null)
                {
                    GetBuyRate = product.BuyRate.ToString();
                }
                else
                {
                    if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                    {
                        GetBuyRate = product.AutomatedBuyRate.ToString();
                    }
                    else
                    {
                        GetBuyRate = "0";
                    }
                }

                if (product.SellRate != 0 && product.SellRate != null)
                {
                    GetSellRate = product.SellRate.ToString();
                }
                else
                {
                    if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                    {
                        GetSellRate = product.AutomatedSellRate.ToString();
                    }
                    else
                    {
                        GetSellRate = "0";
                    }
                }

                if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                {
                    AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                }
                else
                {
                    AdjustmentBuyRate = "0";
                }

                if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                {
                    AdjustmentSellRate = product.SellRateAdjustment.ToString();
                }
                else
                {
                    AdjustmentSellRate = "0";
                }

                customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + product.BuyRate + "|" + product.SellRate);
                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
            }

            ViewData["CustomRates"] = customRates;

            List<string> remittanceProductCustomRates = new List<string>();

            using (var context = new DataAccess.GreatEastForex())
            {
                var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                string ReimttanceGetBuyRate = "0";
                string RemittanceGetSellRate = "0";
                string RemittanceAdjustmentBuyRate = "0";
                string RemittanceAdjustmentSellRate = "0";

                foreach (RemittanceProducts r in RemittanceProducts)
                {
                    ReimttanceGetBuyRate = "0";
                    RemittanceGetSellRate = "0";
                    RemittanceAdjustmentBuyRate = "0";
                    RemittanceAdjustmentSellRate = "0";

                    if (r.PayRate != 0 && r.PayRate != null)
                    {
                        ReimttanceGetBuyRate = r.PayRate.ToString();
                    }
                    else
                    {
                        if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                        {
                            ReimttanceGetBuyRate = r.AutomatedPayRate.ToString();
                        }
                        else
                        {
                            ReimttanceGetBuyRate = "0";
                        }
                    }

                    if (r.GetRate != 0 && r.GetRate != null)
                    {
                        RemittanceGetSellRate = r.GetRate.ToString();
                    }
                    else
                    {
                        if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                        {
                            RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                        }
                        else
                        {
                            RemittanceGetSellRate = "0";
                        }
                    }

                    if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                    {
                        RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                    }
                    else
                    {
                        RemittanceAdjustmentBuyRate = "0";
                    }

                    if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                    {
                        RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                    }
                    else
                    {
                        RemittanceAdjustmentSellRate = "0";
                    }

                    remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + ReimttanceGetBuyRate + "|" + RemittanceGetSellRate);
                }
            }

            ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;

            //ViewData for Checkboxes and Radio button
            ViewData["NaturalEmploymentEmployedRadio"] = "";
            ViewData["NaturalEmploymentSelfEmployedRadio"] = "";
            ViewData["CompanySOFBankCreditCheckbox"] = "";
            ViewData["CompanySOFInvestmentCheckbox"] = "";
            ViewData["CompanySOFOthersCheckbox"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["NaturalSOFSalaryCheckbox"] = "";
            ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
            ViewData["NaturalSOFSavingsCheckbox"] = "";
            ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
            ViewData["NaturalSOFGiftCheckbox"] = "";
            ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
            ViewData["NaturalSOFOthersCheckbox"] = "";
            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["CompanyActingAgentYesRadio"] = "";
            ViewData["CompanyActingAgentNoRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";
            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";

            ViewData["AutoFillNextReviewDate"] = true;

            //List Search Tag
            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
            ViewData["SearchTagList"] = getSearchTags;
            ViewData["SearchTagSelectedItem"] = "";

            ViewData["CompanySourceOfFund"] = "";
            ViewData["NaturalSourceOfFund"] = "";

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //POST: Create [DL, GM, SA]
        [RedirectingActionWithDLGMSA]
        [HttpPost]
        public ActionResult Create(CustomerParticular customerParticulars, CustomerSourceOfFund customerSourceOfFunds, CustomerActingAgent customerActingAgents, CustomerDocumentCheckList customerDocumentChecklists, CustomerOther customerOthers, FormCollection form)
        {
            int page = 1;
            int IsSubAccount = 0;//0 = Is Main Account, 1 = Is Sub Account

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            //validate has customer account
            if (form["customerParticulars.hasCustomerAccount"] != null)
            {
                if (form["customerParticulars.hasCustomerAccount"] == "1")
                {
                    //verify the password and confirm password
                    if (string.IsNullOrEmpty(form["Password"].Trim()))
                    {
                        ModelState.AddModelError("Password", "Password is required!");
                    }

                    if (string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                    {
                        ModelState.AddModelError("ConfirmPassword", "Confirm Password is required!");
                    }

                    if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()) && !string.IsNullOrEmpty(form["Password"].Trim()))
                    {
                        string Password = form["Password"].Trim();
                        string ConfirmPassword = form["ConfirmPassword"].Trim();

                        if (Password.Length >= 8 && ConfirmPassword.Length >= 8)
                        {
                            //check regex
                            Regex regexPattern = new Regex("^(?=.*[A-Za-z])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})");

                            if (!regexPattern.IsMatch(Password))
                            {
                                ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters and 1 special characters!");
                            }
                            else
                            {
                                if (form["Password"].Trim() != form["ConfirmPassword"].Trim())
                                {
                                    ModelState.AddModelError("Password", "New Password and Confirm Password not match!");
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters!");
                        }
                    }

                    //To verify if has customer account, check the email is unique or not.
                    customerParticulars.hasCustomerAccount = 1;
                }
            }

            //validate customerParticulars.IsSubAccount
            if (form["customerParticulars.IsSubAccount"] != null)
            {
                if (form["customerParticulars.IsSubAccount"] != "0")
                {
                    IsSubAccount = 1;
                    //validate main customer dropdown value
                    if (form["MainAccountCustomer"] != null)
                    {
                        if (!FormValidationHelper.IntegerValidation(form["MainAccountCustomer"]))
                        {
                            ModelState.AddModelError("MainAccountCustomer", "Something went wrong on selecting Main Account Customer!");
                        }
                    }
                }
            }

            //Title, Surname, given name validation
            //if (form["customerParticulars.Surname"].Trim() == null || string.IsNullOrEmpty(form["customerParticulars.Surname"].Trim()))
            //{
            //	ModelState.AddModelError("customerParticulars.Surname", "Surname is required!");
            //}

            //if (form["customerParticulars.GivenName"].Trim() == null || string.IsNullOrEmpty(form["customerParticulars.GivenName"].Trim()))
            //{
            //	ModelState.AddModelError("customerParticulars.GivenName", "Given Name is required!");
            //}

            List<ModelStateValidation> errors = new List<ModelStateValidation>();

            if (!string.IsNullOrEmpty(customerParticulars.CustomerCode))
            {
                CustomerParticular checkUniqueCode = _customerParticularsModel.FindCustomerCode(customerParticulars.CustomerCode);

                if (checkUniqueCode != null)
                {
                    ModelState.AddModelError("customerParticulars.CustomerCode", "Customer Code already existed!");
                }
            }

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                //Check Company H.O.M Contact
                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoH))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoH", "Company Contact No (Home) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoO))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoO", "Company Contact No (Office) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoM))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoM", "Company Contact No (Mobile) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ICPassport))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ICPassport", "Company IC/Passport cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_JobTitle))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_JobTitle", "Company Job Title cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_Nationality))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_Nationality", "Company Nationality cannot be empty!");
                //}

                if (form["customerParticulars.hasCustomerAccount"] != null)
                {
                    if (form["customerParticulars.hasCustomerAccount"] == "1")
                    {
                        if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        {
                            ModelState.AddModelError("customerParticulars.Company_Email", "Email is required!");
                        }
                    }
                }

                if (IsSubAccount == 0)
                {
                    if (string.IsNullOrEmpty(customerParticulars.Company_RegisteredName))
                    {
                        ModelState.AddModelError("customerParticulars.Company_RegisteredName", "Registered Name cannot be empty!");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_BusinessAddress1))
                    {
                        ModelState.AddModelError("customerParticulars.Company_BusinessAddress1", "Business Address cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_TelNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_TelNo", "Tel No. cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_FaxNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_FaxNo", "Fax No. cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_PostalCode))
                    {
                        ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code cannot be empty.");
                    }
                    else
                    {
                        if (customerParticulars.Company_PostalCode.Length < 5)
                        {
                            ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code must at least 5 digits.");
                        }
                        else
                        {
                            //check is digit or not
                            bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Company_PostalCode);

                            if (!checkDigit)
                            {
                                ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code only allow digits.");
                            }
                        }
                    }

                    //if (string.IsNullOrEmpty(customerParticulars.Shipping_Address1))
                    //{
                    //	ModelState.AddModelError("customerParticulars.Shipping_Address1", "Mailing Address 1 cannot be empty.");
                    //}

                    //if (string.IsNullOrEmpty(customerParticulars.Shipping_PostalCode))
                    //{
                    //	ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code cannot be empty.");
                    //}
                    //else
                    //{
                    //	if (customerParticulars.Shipping_PostalCode.Length < 5)
                    //	{
                    //		ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code must at least 5 digits.");
                    //	}
                    //	else
                    //	{
                    //		//check is digit or not
                    //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Shipping_PostalCode);

                    //		if (!checkDigit)
                    //		{
                    //			ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code only allow digits.");
                    //		}
                    //	}
                    //}

                    if (string.IsNullOrEmpty(form["Country"]))
                    {
                        ModelState.AddModelError("Country", "Please select Country.");
                    }

                    if (string.IsNullOrEmpty(form["CountryCode"]))
                    {
                        ModelState.AddModelError("CountryCode", "Please select Country Code.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_PlaceOfRegistration))
                    {
                        ModelState.AddModelError("customerParticulars.Company_PlaceOfRegistration", "Place of Registration is required.");
                    }

                    if (customerParticulars.Company_DateOfRegistration == null)
                    {
                        ModelState.AddModelError("customerParticulars.Company_DateOfRegistration", "Date of Registration is required.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_RegistrationNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_RegistrationNo", "Registration No is required.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntity))
                    {
                        ModelState.AddModelError("customerParticulars.Company_TypeOfEntity", "Type of Entity is required.");
                    }
                    else
                    {
                        if (customerParticulars.Company_TypeOfEntity == "Others")
                        {
                            if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntityIfOthers))
                            {
                                ModelState.AddModelError("customerParticulars.Company_TypeOfEntityIfOthers", "If Others is required.");
                            }
                        }
                    }
                }

                //Company DOB
                if (string.IsNullOrEmpty(form["dob-datepicker"]))
                {
                    ModelState.AddModelError("dob-datepicker", "Date of Birth cannot be empty.");
                }

                //if (string.IsNullOrEmpty(form["Company_service_like_to_use"]))
                //{
                //	ModelState.AddModelError("CompanyServiceLikeToUse", "This cannot be empty!");
                //}

                if (string.IsNullOrEmpty(form["Company_purpose_of_intended_transactions"]))
                {
                    ModelState.AddModelError("CompanyPurposeOfIntendedTransactions", "This cannot be empty.");
                }

                //if (string.IsNullOrEmpty(form["Company_hear_about_us"]))
                //{
                //	ModelState.AddModelError("CompanyHearAboutUs", "This cannot be empty.");
                //}

                if (string.IsNullOrEmpty(form["Company_source_of_fund"]))
                {
                    //ModelState.AddModelError("CompanySourceOfFund", "Must at least have one source of fund.");
                }
                else
                {
                    string s = form["Company_source_of_fund"];

                    string[] subs = s.Split(',');

                    var arraycontainsturtles = (Array.IndexOf(subs, "Others") > -1);

                    if (arraycontainsturtles)
                    {
                        if (string.IsNullOrEmpty(form["customerSourceOfFunds.Company_SourceOfFundIfOthers"]))
                        {
                            ModelState.AddModelError("customerSourceOfFunds.Company_SourceOfFundIfOthers", "Source of fund others cannot be empty.");
                        }
                    }
                }
            }
            else
            {
                //Natural Person validation
                //if (string.IsNullOrEmpty(customerParticulars.Mailing_PostalCode))
                //{
                //	ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code cannot be empty.");
                //}
                //else
                //{
                //	if (customerParticulars.Mailing_PostalCode.Length < 5)
                //	{
                //		ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code must at least 5 digits.");
                //	}
                //	else
                //	{
                //		//check is digit or not
                //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Mailing_PostalCode);

                //		if (!checkDigit)
                //		{
                //			ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code only allow digits.");
                //		}
                //	}
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Natural_PermanentPostalCode))
                //{
                //	ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code cannot be empty.");
                //}
                //else
                //{
                //	if (customerParticulars.Natural_PermanentPostalCode.Length < 5)
                //	{
                //		ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code must at least 5 digits.");
                //	}
                //	else
                //	{
                //		//check is digit or not
                //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Natural_PermanentPostalCode);

                //		if (!checkDigit)
                //		{
                //			ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code only allow digits.");
                //		}
                //	}
                //}

                //if (string.IsNullOrEmpty(form["Natural_service_like_to_use"]))
                //{
                //	ModelState.AddModelError("NaturalServiceLikeToUse", "This cannot be empty!");
                //}

                if (form["customerParticulars.hasCustomerAccount"] != null)
                {
                    if (form["customerParticulars.hasCustomerAccount"] == "1")
                    {
                        if (string.IsNullOrEmpty(customerParticulars.Natural_Email))
                        {
                            ModelState.AddModelError("customerParticulars.Natural_Email", "Email is required!");
                        }
                    }
                }

                if (string.IsNullOrEmpty(form["Natural_purpose_of_intended_transactions"]))
                {
                    ModelState.AddModelError("NaturalPurposeOfIntendedTransactions", "This cannot be empty.");
                }

                //if (string.IsNullOrEmpty(form["Natural_hear_about_us"]))
                //{
                //	ModelState.AddModelError("NaturalHearAboutUs", "This cannot be empty.");
                //}

                if (string.IsNullOrEmpty(form["Natural_source_of_fund"]))
                {
                    //ModelState.AddModelError("NaturalSourceOfFund", "Must at least have one source of fund.");
                }
                else
                {
                    string s = form["Natural_source_of_fund"];

                    string[] subs = s.Split(',');

                    var arraycontainsturtles = (Array.IndexOf(subs, "Others") > -1);

                    if (arraycontainsturtles)
                    {
                        if (string.IsNullOrEmpty(form["customerSourceOfFunds.Natural_SourceOfFundIfOthers"]))
                        {
                            ModelState.AddModelError("customerSourceOfFunds.Natural_SourceOfFundIfOthers", "Source of fund others cannot be empty.");
                        }
                    }
                }
            }

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                errors = ValidateModelStateForCorporateCompany(customerParticulars, customerSourceOfFunds, customerActingAgents, customerDocumentChecklists, customerOthers, form);
            }
            else
            {
                errors = ValidateModelStateForNaturalPerson(customerParticulars, customerSourceOfFunds, customerActingAgents, customerDocumentChecklists, customerOthers, form);
            }

            if (errors.Count > 0)
            {
                foreach (ModelStateValidation error in errors)
                {
                    ModelState.AddModelError(error.Key, error.ErrorMessage);
                }
            }

            //Customer Bank Account Validation
            List<string> bankAccountKeys = form.AllKeys.Where(e => e.Contains("BankAccountNo_")).ToList();
            List<string> bankAccountNos = new List<string>();

            foreach (string key in bankAccountKeys)
            {
                bankAccountNos.Add(form[key]);
            }

            //Customer Sanctions and PEP Screening Report Validation
            List<string> screeningReportKeys = form.AllKeys.Where(e => e.Contains("PEPScreeningReport_Date_")).ToList();
            List<string> screeningReports = new List<string>();

            int rowCount = 1;

            foreach (string key in screeningReportKeys)
            {
                string rowId = key.Split('_')[2];

                if (string.IsNullOrEmpty(form["PEPScreeningReport_Date_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_Date_" + rowCount, "Date is required!");
                }
                else
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(form["PEPScreeningReport_Date_" + rowId]);
                    }
                    catch
                    {
                        ModelState.AddModelError("PEPScreeningReport_Date_" + rowCount, "Date is not valid!");
                    }
                }

                if (string.IsNullOrEmpty(form["PEPScreeningReport_DateOfAcra_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_DateOfAcra_" + rowCount, "Date of ACRA is required!");
                }
                else
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(form["PEPScreeningReport_DateOfAcra_" + rowId]);
                    }
                    catch
                    {
                        ModelState.AddModelError("PEPScreeningReport_DateOfAcra_" + rowCount, "Date of ACRA is not valid!");
                    }
                }

                if (string.IsNullOrEmpty(form["PEPScreeningReport_ScreenedBy_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_ScreenedBy_" + rowCount, "Screened By is required!");
                }

                screeningReports.Add(form["PEPScreeningReport_Date_" + rowId] + "|" + form["PEPScreeningReport_DateOfAcra_" + rowId] + "|" + form["PEPScreeningReport_ScreenedBy_" + rowId] + "|" + form["PEPScreeningReport_ScreeningReport1_" + rowId] + "|" + form["PEPScreeningReport_ScreeningReport2_" + rowId] + "|" + form["PEPScreeningReport_Remarks_" + rowId]);
            }

            if (!string.IsNullOrEmpty(form["Search_Tag_Dropdown_Form"]))
            {
                string[] splitText = form["Search_Tag_Dropdown_Form"].Split(',');
                string finalText = "";

                foreach (string _split in splitText)
                {
                    finalText = finalText + "-" + _split + "-,";
                }

                customerParticulars.SearchTags = finalText.Substring(0, finalText.Length - 1);
            }

            //Save Customer Custom Rate
            List<string> customRateKeysC = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

            foreach (string key in customRateKeysC)
            {
                string id = key.Substring(14);

                if (string.IsNullOrEmpty(form["default-rate-" + id]))
                {
                    CustomerCustomRate rate = new CustomerCustomRate();
                    rate.ProductId = Convert.ToInt32(form[key]);

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!string.IsNullOrEmpty(form["CustomBuyRate_" + id]))
                        {
                            bool checkBuy = FormValidationHelper.NonNegativeAmountValidation(form["CustomBuyRate_" + id]);

                            if (!checkBuy)
                            {
                                ModelState.AddModelError("CustomBuyRate_" + id, "Invalid Buy Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["CustomBuyRate_" + id]) > 999)
                                {
                                    ModelState.AddModelError("CustomBuyRate_" + id, "Buy rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["CustomSellRate_" + id]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["CustomSellRate_" + id]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("CustomSellRate_" + id, "Invalid Sell Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["CustomSellRate_" + id]) > 999)
                                {
                                    ModelState.AddModelError("CustomSellRate_" + id, "Sell rate Adjustment cannot more than 999!");
                                }
                            }
                        }
                    }
                }
            }

            //Save Customer Remittance Custom Rate
            List<string> customRemittanceRateKeysC = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();

            foreach (string key in customRemittanceRateKeysC)
            {
                string id = key.Substring(19);

                if (string.IsNullOrEmpty(form["default-fee-" + id]))
                {
                    CustomerRemittanceProductCustomRate fee = new CustomerRemittanceProductCustomRate();
                    fee.RemittanceProductId = Convert.ToInt32(form[key]);

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomBuyRate_" + id]))
                        {
                            bool checkBuy = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomBuyRate_" + id]);

                            if (!checkBuy)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomBuyRate_" + id, "Invalid Pay Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomBuyRate_" + id]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomBuyRate_" + id, "Pay Rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomSellRate_" + id]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomSellRate_" + id]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomSellRate_" + id, "Invalid Get Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomSellRate_" + id]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomSellRate_" + id, "Get Rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomFee_" + id]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomFee_" + id]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomFee_" + id, "Invalid Transaction Fee!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomFee_" + id]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomFee_" + id, "Transaction Fee cannot more than 999!");
                                }
                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                customerParticulars.hasCustomerAccount = 0;
                customerParticulars.IsSubAccount = 0;
                customerParticulars.isVerify = 0;
                customerParticulars.Customer_Title = form["Customer_Title"];
                customerParticulars.EnableTransactionType = form["customerParticulars.EnableTransactionType"];

                //Has Customer Account
                if (form["customerParticulars.hasCustomerAccount"] == "1")
                {
                    customerParticulars.hasCustomerAccount = 1;
                    customerParticulars.Password = EncryptionHelper.Encrypt(form["Password"]);
                }

                //Is Sub Account
                if (form["customerParticulars.IsSubAccount"] != "0")
                {
                    customerParticulars.IsSubAccount = Convert.ToInt32(form["MainAccountCustomer"]);
                }

                //Is Email Verify
                if (form["customerParticulars.isVerify"] != "0")
                {
                    customerParticulars.isVerify = 1;
                }

                //Perform Data Cleaning
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    //Empty Customer Particular for Natural Person
                    customerParticulars.Natural_Name = null;
                    customerParticulars.Natural_PermanentAddress = null;
                    customerParticulars.Natural_PermanentAddress2 = null;
                    customerParticulars.Natural_PermanentAddress3 = null;
                    customerParticulars.Natural_PermanentPostalCode = null;
                    customerParticulars.Natural_MailingAddress = null;
                    customerParticulars.Natural_MailingAddress2 = null;
                    customerParticulars.Natural_MailingAddress3 = null;
                    customerParticulars.Mailing_PostalCode = null;
                    customerParticulars.Natural_Nationality = null;
                    customerParticulars.Natural_ICPassportNo = null;
                    customerParticulars.Natural_DOB = null;
                    customerParticulars.Natural_ContactNoH = null;
                    customerParticulars.Natural_ContactNoO = null;
                    customerParticulars.Natural_ContactNoM = null;
                    customerParticulars.Natural_Email = null;
                    customerParticulars.Natural_EmploymentType = null;
                    customerParticulars.Natural_EmployedEmployerName = null;
                    customerParticulars.Natural_EmployedJobTitle = null;
                    customerParticulars.Natural_EmployedRegisteredAddress = null;
                    customerParticulars.Natural_EmployedRegisteredAddress2 = null;
                    customerParticulars.Natural_EmployedRegisteredAddress3 = null;
                    customerParticulars.Natural_SelfEmployedBusinessName = null;
                    customerParticulars.Natural_SelfEmployedRegistrationNo = null;
                    customerParticulars.Natural_SelfEmployedBusinessAddress = null;
                    customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace = null;

                    //Empty Customer Source of Fund for Natural Person
                    customerSourceOfFunds.Natural_SourceOfFund = null;
                    customerSourceOfFunds.Natural_SourceOfFundIfOthers = null;
                    customerSourceOfFunds.Natural_AnnualIncome = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 = null;
                    customerSourceOfFunds.Natural_ServiceLikeToUse = null;
                    customerSourceOfFunds.Natural_PurposeOfIntendedTransactions = null;
                    customerSourceOfFunds.Natural_HearAboutUs = null;

                    //Empty Customer Acting Agent for Natural Person
                    customerActingAgents.Natural_Name = null;
                    customerActingAgents.Natural_PermanentAddress = null;
                    customerActingAgents.Natural_Nationality = null;
                    customerActingAgents.Natural_ICPassportNo = null;
                    customerActingAgents.Natural_DOB = null;

                    //Empty Customer Acting Agent if No
                    if (customerActingAgents.ActingAgent == "No")
                    {
                        customerActingAgents.Company_CustomerType = null;
                        customerActingAgents.Company_Address = null;
                        customerActingAgents.Company_PlaceOfRegistration = null;
                        customerActingAgents.Company_RegistrationNo = null;
                        customerActingAgents.Company_DateOfRegistration = null;
                        customerActingAgents.Relationship = null;
                        customerActingAgents.BasisOfAuthority = null;
                    }

                    //Empty Customer Document Checklist for Natural Person
                    customerDocumentChecklists.Natural_ICOfCustomer = null;
                    customerDocumentChecklists.Natural_BusinessNameCard = null;
                    customerDocumentChecklists.Natural_KYCForm = null;
                    customerDocumentChecklists.Natural_SelfiePhotoID = null;

                    //Assign Checkbox value
                    customerSourceOfFunds.Company_SourceOfFund = form["Company_source_of_fund"];
                    customerSourceOfFunds.Company_ServiceLikeToUse = form["Company_service_like_to_use"];
                    customerSourceOfFunds.Company_PurposeOfIntendedTransactions = form["Company_purpose_of_intended_transactions"];
                    customerSourceOfFunds.Company_HearAboutUs = form["Company_hear_about_us"];
                    customerParticulars.Company_Country = Convert.ToInt32(form["Country"]);
                    customerParticulars.Company_CountryCode = Convert.ToInt32(form["CountryCode"]);
                    customerParticulars.DOB = Convert.ToDateTime(form["dob-datepicker"]);

                    if (customerParticulars.IsSubAccount != 0)
                    {
                        //this is assign main account company value into sub account fields.
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var GetMainAccountDetails = context.CustomerParticulars.Where(e => e.ID == customerParticulars.IsSubAccount).FirstOrDefault();

                            customerParticulars.Company_RegisteredName = GetMainAccountDetails.Company_RegisteredName;
                            customerParticulars.Company_BusinessAddress1 = GetMainAccountDetails.Company_BusinessAddress1;
                            customerParticulars.Company_BusinessAddress2 = GetMainAccountDetails.Company_BusinessAddress2;
                            customerParticulars.Company_BusinessAddress3 = GetMainAccountDetails.Company_BusinessAddress3;
                            customerParticulars.Company_PostalCode = GetMainAccountDetails.Company_PostalCode;

                            customerParticulars.Shipping_Address1 = GetMainAccountDetails.Shipping_Address1;
                            customerParticulars.Shipping_Address2 = GetMainAccountDetails.Shipping_Address2;
                            customerParticulars.Shipping_Address3 = GetMainAccountDetails.Shipping_Address3;
                            customerParticulars.Shipping_PostalCode = GetMainAccountDetails.Shipping_PostalCode;

                            customerParticulars.Company_Country = GetMainAccountDetails.Company_Country;
                            customerParticulars.Company_CountryCode = GetMainAccountDetails.Company_CountryCode;

                            customerParticulars.Company_TelNo = GetMainAccountDetails.Company_TelNo;
                            customerParticulars.Company_FaxNo = GetMainAccountDetails.Company_FaxNo;

                            customerParticulars.Company_PlaceOfRegistration = GetMainAccountDetails.Company_PlaceOfRegistration;
                            customerParticulars.Company_DateOfRegistration = GetMainAccountDetails.Company_DateOfRegistration;

                            customerParticulars.Company_RegistrationNo = GetMainAccountDetails.Company_RegistrationNo;
                            customerParticulars.Company_TypeOfEntity = GetMainAccountDetails.Company_TypeOfEntity;
                            customerParticulars.Company_TypeOfEntityIfOthers = GetMainAccountDetails.Company_TypeOfEntityIfOthers;
                        }
                    }
                }
                else
                {
                    customerParticulars.IsSubAccount = 0;

                    //Empty Customer Particular for Company
                    customerParticulars.Company_RegisteredName = null;
                    //customerParticulars.Company_RegisteredAddress = null;
                    customerParticulars.Company_BusinessAddress1 = null;
                    customerParticulars.Company_BusinessAddress2 = null;
                    customerParticulars.Company_BusinessAddress3 = null;
                    customerParticulars.Company_PostalCode = null;
                    customerParticulars.Company_ContactName = null;
                    customerParticulars.Company_Country = 0;
                    customerParticulars.Company_CountryCode = 0;
                    customerParticulars.Company_TelNo = null;
                    customerParticulars.Company_FaxNo = null;
                    customerParticulars.Company_Email = null;
                    customerParticulars.Company_PlaceOfRegistration = null;
                    customerParticulars.Company_DateOfRegistration = null;
                    customerParticulars.Company_RegistrationNo = null;
                    customerParticulars.Company_TypeOfEntity = null;
                    customerParticulars.Company_TypeOfEntityIfOthers = null;
                    customerParticulars.Company_PurposeAndIntended = null;
                    customerParticulars.Shipping_Address1 = null;
                    customerParticulars.Shipping_Address2 = null;
                    customerParticulars.Shipping_Address3 = null;
                    customerParticulars.Shipping_PostalCode = null;
                    customerParticulars.Company_Nationality = null;
                    customerParticulars.Company_JobTitle = null;
                    customerParticulars.Company_ICPassport = null;
                    customerParticulars.Company_ContactNoH = null;
                    customerParticulars.Company_ContactNoO = null;
                    customerParticulars.Company_ContactNoM = null;

                    //Empty Customer Particular Self Employed if Employed
                    if (customerParticulars.Natural_EmploymentType == "Employed")
                    {
                        customerParticulars.Natural_SelfEmployedBusinessName = null;
                        customerParticulars.Natural_SelfEmployedRegistrationNo = null;
                        customerParticulars.Natural_SelfEmployedBusinessAddress = null;
                        customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace = null;
                    }
                    //Empty Customer Particular Employed if Self Employed
                    else
                    {
                        customerParticulars.Natural_EmployedEmployerName = null;
                        customerParticulars.Natural_EmployedJobTitle = null;
                        customerParticulars.Natural_EmployedRegisteredAddress = null;
                        customerParticulars.Natural_EmployedRegisteredAddress2 = null;
                        customerParticulars.Natural_EmployedRegisteredAddress3 = null;
                    }

                    //Empty Customer Source of Fund for Company
                    customerSourceOfFunds.Company_SourceOfFund = null;
                    customerSourceOfFunds.Company_SourceOfFundIfOthers = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 = null;
                    customerSourceOfFunds.Company_ServiceLikeToUse = null;
                    customerSourceOfFunds.Company_PurposeOfIntendedTransactions = null;
                    customerSourceOfFunds.Company_HearAboutUs = null;

                    //Empty Customer Acting Agent for Company
                    customerActingAgents.Company_CustomerType = null;
                    customerActingAgents.Company_Address = null;
                    customerActingAgents.Company_PlaceOfRegistration = null;
                    customerActingAgents.Company_RegistrationNo = null;
                    customerActingAgents.Company_DateOfRegistration = null;

                    //Empty Customer Acting Agent if No
                    if (customerActingAgents.ActingAgent == "No")
                    {
                        customerActingAgents.Natural_Name = null;
                        customerActingAgents.Natural_PermanentAddress = null;
                        customerActingAgents.Natural_Nationality = null;
                        customerActingAgents.Natural_ICPassportNo = null;
                        customerActingAgents.Natural_DOB = null;
                        customerActingAgents.Relationship = null;
                        customerActingAgents.BasisOfAuthority = null;
                    }

                    //Empty Customer Document Checklist for Company
                    customerDocumentChecklists.Company_AccountOpeningForm = null;
                    customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons = null;
                    customerDocumentChecklists.Company_ICWithDirectors = null;
                    customerDocumentChecklists.Company_BusinessProfileFromAcra = null;
                    customerDocumentChecklists.Company_SelfiePassporWorkingPass = null;
                    customerDocumentChecklists.Company_SelfiePhotoID = null;

                    //Assign Checkbox value
                    customerSourceOfFunds.Natural_SourceOfFund = form["Natural_source_of_fund"];
                    customerSourceOfFunds.Natural_ServiceLikeToUse = form["Natural_service_like_to_use"];
                    customerSourceOfFunds.Natural_PurposeOfIntendedTransactions = form["Natural_purpose_of_intended_transactions"];
                    customerSourceOfFunds.Natural_HearAboutUs = form["Natural_hear_about_us"];
                }

                bool result = _customerParticularsModel.Add(customerParticulars);

                if (result)
                {
                    AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerParticulars", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Particular");

                    int cid = customerParticulars.ID;

                    //Save Customer Source of Funds
                    customerSourceOfFunds.CustomerParticularId = cid;

                    bool result_SOF = _customerSourceOfFundsModel.Add(customerSourceOfFunds);

                    if (result_SOF)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerSourceOfFunds", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Source of Funds");
                    }

                    //Save Customer Acting Agent
                    customerActingAgents.CustomerParticularId = cid;

                    bool result_Agent = _customerActingAgentsModel.Add(customerActingAgents);

                    if (result_Agent)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActingAgents", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Acting Agent");

                        //Move Basis of Authority Files
                        if (customerActingAgents.ActingAgent == "Yes")
                        {
                            string[] basisOfAuthorityFiles = customerActingAgents.BasisOfAuthority.Split(',');

                            foreach (string file in basisOfAuthorityFiles)
                            {
                                string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                                if (System.IO.File.Exists(sourceFile))
                                {
                                    System.IO.File.Move(sourceFile, destinationFile);
                                }
                            }
                        }
                    }

                    //Save Customer Appointment of Staff
                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        List<string> appointmentKeys = form.AllKeys.Where(e => e.Contains("Personnel_FullName_")).ToList();

                        bool res_appointment = false;

                        foreach (string key in appointmentKeys)
                        {
                            string id = key.Substring(19);

                            CustomerAppointmentOfStaff appointment = new CustomerAppointmentOfStaff();
                            appointment.CustomerParticularId = cid;
                            appointment.FullName = form[key].ToString();
                            appointment.ICPassportNo = form["Personnel_ICPassport_" + id].ToString();
                            appointment.Nationality = form["Personnel_Nationality_" + id].ToString();
                            appointment.JobTitle = form["Personnel_JobTitle_" + id].ToString();
                            //appointment.SpecimenSignature = form["Personnel_SpecimenSignature_" + id].ToString();

                            bool result_appointment = _customerAppointmentOfStaffsModel.Add(appointment);

                            if (result_appointment)
                            {
                                if (!res_appointment)
                                {
                                    res_appointment = true;
                                }

                                //Move Specimen Signature Files
                                //string[] specimenSignatureFiles = appointment.SpecimenSignature.Split(',');

                                //foreach (string file in specimenSignatureFiles)
                                //{
                                //    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                //    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SpecimenSignatureFolder"].ToString()), file);

                                //    System.IO.File.Move(sourceFile, destinationFile);
                                //}
                            }
                        }

                        if (res_appointment)
                        {
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerAppointmentOfStaffs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Appointment of Staffs");
                        }
                    }

                    //Save Customer Document Checklist
                    customerDocumentChecklists.CustomerParticularId = cid;

                    bool result_DCL = _customerDocumentCheckListsModel.Add(customerDocumentChecklists);

                    if (result_DCL)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerDocumentChecklists", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Document Checklist");

                        //Move Document Files
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            //Company Selfie Passport
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                            {
                                string[] selfieWorkingPassFiles = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');

                                foreach (string file in selfieWorkingPassFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //Company Selfie Photo
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                            {
                                string[] selfiePhotoFiles = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');

                                foreach (string file in selfiePhotoFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //Account Opening Files
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                            {
                                string[] accountOpeningFiles = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');

                                foreach (string file in accountOpeningFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //IC With Authorized Trading Person
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                            {
                                string[] tradingPersonFiles = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');

                                foreach (string file in tradingPersonFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //IC With Director
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                            {
                                string[] directorFiles = customerDocumentChecklists.Company_ICWithDirectors.Split(',');

                                foreach (string file in directorFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //Business Profile from Acra
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                            {
                                string[] businessAcraFiles = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');

                                foreach (string file in businessAcraFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Natural Selfie Photo
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                            {
                                string[] NaturalSelfiePhotoFiles = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');

                                foreach (string file in NaturalSelfiePhotoFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //IC of Customer
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                            {
                                string[] icOfCustomerFiles = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');

                                foreach (string file in icOfCustomerFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //Business Name Card
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                            {
                                string[] businessNameCardFiles = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');

                                foreach (string file in businessNameCardFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }

                            //KYC Form
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                            {
                                string[] kycFormFiles = customerDocumentChecklists.Natural_KYCForm.Split(',');

                                foreach (string file in kycFormFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        System.IO.File.Move(sourceFile, destinationFile);
                                    }
                                }
                            }
                        }
                    }

                    //Save Customer Sanctions and PEP Screening Report
                    bool hasScreeningReport = false;

                    foreach (string report in screeningReports)
                    {
                        string[] r = report.Split('|');

                        CustomerScreeningReport screeningReport = new CustomerScreeningReport();
                        screeningReport.CustomerParticularId = cid;
                        screeningReport.Date = Convert.ToDateTime(r[0]);
                        screeningReport.DateOfAcra = Convert.ToDateTime(r[1]);
                        screeningReport.ScreenedBy = r[2];
                        screeningReport.ScreeningReport_1 = r[3];
                        screeningReport.ScreeningReport_2 = r[4];
                        screeningReport.Remarks = r[5];

                        bool result_screeningReport = _customerScreeningReportsModel.Add(screeningReport);

                        if (result_screeningReport)
                        {
                            if (!hasScreeningReport)
                            {
                                hasScreeningReport = true;
                            }
                        }
                    }

                    if (hasScreeningReport)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerScreeningReports", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Sanctions and PEP Screening Report");
                    }

                    //Save Customer Others
                    customerOthers.CustomerParticularId = cid;
                    customerOthers.Status = "Pending Approval";
                    customerOthers.ApprovalBy = Convert.ToInt32(Session["UserId"]);

                    //Append Bank Account No
                    foreach (string bankAccount in bankAccountNos)
                    {
                        if (!string.IsNullOrEmpty(bankAccount))
                        {
                            customerOthers.BankAccountNo += bankAccount + "|";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                    {
                        customerOthers.BankAccountNo = customerOthers.BankAccountNo.Substring(0, customerOthers.BankAccountNo.Length - 1);
                    }

                    bool result_Others = _customerOthersModel.Add(customerOthers);

                    if (result_Others)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Others");

                        //Screening Results Document
                        if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                        {
                            string[] screeningResultsDocument = customerOthers.ScreeningResultsDocument.Split(',');

                            foreach (string file in screeningResultsDocument)
                            {
                                string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                                if (System.IO.File.Exists(sourceFile))
                                {
                                    System.IO.File.Move(sourceFile, destinationFile);
                                }
                            }
                        }
                    }

                    //Save Customer Custom Rate
                    List<string> customRateKeys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

                    bool res_CustomRates = false;

                    foreach (string key in customRateKeys)
                    {
                        string id = key.Substring(14);

                        if (string.IsNullOrEmpty(form["default-rate-" + id]))
                        {
                            CustomerCustomRate rate = new CustomerCustomRate();
                            rate.CustomerParticularId = cid;
                            rate.ProductId = Convert.ToInt32(form[key]);
                            int Check = 0;
                            if (!string.IsNullOrEmpty(form["CustomBuyRate_" + id]))
                            {
                                rate.BuyRate = Convert.ToDecimal(form["CustomBuyRate_" + id]);
                                Check = 1;
                            }

                            if (!string.IsNullOrEmpty(form["CustomSellRate_" + id]))
                            {
                                rate.SellRate = Convert.ToDecimal(form["CustomSellRate_" + id]);
                                Check = 1;
                            }

                            if (!string.IsNullOrEmpty(form["CustomEncashmentRate_" + id]))
                            {
                                rate.EncashmentRate = Convert.ToDecimal(form["CustomEncashmentRate_" + id]);
                                Check = 1;
                            }

                            if (Check == 1)
                            {
                                bool result_CustomRate = _customerCustomRatesModel.Add(rate);

                                if (result_CustomRate)
                                {
                                    if (!res_CustomRates)
                                    {
                                        res_CustomRates = true;
                                    }
                                }
                            }

                            //bool result_CustomRate = _customerCustomRatesModel.Add(rate);
                        }
                    }

                    if (res_CustomRates)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerCustomRates", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Custom Rates");
                    }

                    //Save Remittance Product Custom Rate
                    List<string> RemittanceProductCustomRateKeys = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();

                    bool res_RemittanceProductCustomFee = false;
                    int checksave = 0;
                    foreach (string key in RemittanceProductCustomRateKeys)
                    {
                        string id = key.Substring(19);

                        if (string.IsNullOrEmpty(form["default-fee-" + id]))
                        {
                            CustomerRemittanceProductCustomRate fee = new CustomerRemittanceProductCustomRate();
                            fee.CustomerParticularId = cid;
                            fee.RemittanceProductId = Convert.ToInt32(form[key]);

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomFee_" + id]))
                            {
                                fee.Fee = Convert.ToDecimal(form["RemittanceProductsCustomFee_" + id]);
                                checksave = 1;
                            }

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomBuyRate_" + id]))
                            {
                                fee.PayRateAdjustment = Convert.ToDecimal(form["RemittanceProductsCustomBuyRate_" + id]);
                                checksave = 1;
                            }

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomSellRate_" + id]))
                            {
                                fee.GetRateAdjustment = Convert.ToDecimal(form["RemittanceProductsCustomSellRate_" + id]);
                                checksave = 1;
                            }

                            if (checksave == 1)
                            {
                                checksave = 0;
                                bool result_CustomFee = _CustomerRemittanceProductCustomRatesModel.Add(fee);

                                if (result_CustomFee)
                                {
                                    if (!res_RemittanceProductCustomFee)
                                    {
                                        res_RemittanceProductCustomFee = true;
                                    }
                                }
                            }
                        }
                    }

                    if (res_RemittanceProductCustomFee)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerRemittanceProductCustomFee", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Custom Fee");
                    }

                    TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully created!");

                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
                    TempData.Add("Result", "danger|An error while saving customer record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            ViewData["CustomerCountry"] = form["Country"];
            ViewData["CustomerCountryCode"] = form["CountryCode"];
            ViewData["ServiceLikeToUse"] = form["service_like_to_use"];
            ViewData["PurposeOfIntendedTransactions"] = form["purpose_of_intended_transactions"];
            ViewData["HearAboutUs"] = form["hear_about_us"];

            //List Search Tag
            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
            ViewData["SearchTagList"] = getSearchTags;
            ViewData["SearchTagSelectedItem"] = "";

            if (!string.IsNullOrEmpty(form["Search_Tag_Dropdown_Form"]))
            {
                ViewData["SearchTagSelectedItem"] = form["Search_Tag_Dropdown_Form"];
            }

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

            Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
            ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

            Dropdown[] gradingDDL = GradingDDL();
            ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

            Dropdown[] customerProfileDDL = CustomerProfileDDL();
            ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

            Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
            ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

            //Start
            //Has Customer Account DDL
            Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
            ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", form["customerParticulars.hasCustomerAccount"]);

            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", form["customerParticulars.IsVerify"]);

            //Is Main Account DDL
            //0 - Main Account
            //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
            Dropdown[] isMainAccountDDL = IsMainAccountDDL();
            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", form["customerParticulars.IsSubAccount"]);

            //Main Account Customer DDL
            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL();
            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", form["MainAccountCustomer"]);
            //End

            ViewData["CompanyForm"] = "";
            ViewData["NaturalForm"] = "";

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                ViewData["NaturalForm"] = "display:none;";
            }
            else
            {
                ViewData["CompanyForm"] = "display:none;";
            }

            ViewData["NaturalEmployedForm"] = "display:none;";
            ViewData["NaturalSelfEmployedForm"] = "display:none;";

            ViewData["NaturalEmploymentEmployedRadio"] = "";
            ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

            if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
            {
                if (customerParticulars.Natural_EmploymentType == "Employed")
                {
                    ViewData["NaturalEmployedForm"] = "";
                    ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                }
                else
                {
                    ViewData["NaturalSelfEmployedForm"] = "";
                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                }
            }

            ViewData["CompanySOFBankCreditCheckbox"] = "";
            ViewData["CompanySOFInvestmentCheckbox"] = "";
            ViewData["CompanySOFOthersCheckbox"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["NaturalSOFSalaryCheckbox"] = "";
            ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
            ViewData["NaturalSOFSavingsCheckbox"] = "";
            ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
            ViewData["NaturalSOFGiftCheckbox"] = "";
            ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
            ViewData["NaturalSOFOthersCheckbox"] = "";
            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

            if (!string.IsNullOrEmpty(form["customerParticulars.EnableTransactionType"]))
            {
                string[] type = form["customerParticulars.EnableTransactionType"].ToString().Split(',');

                if (Array.IndexOf(type, "Remittance") >= 0)
                {
                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                }

                if (Array.IndexOf(type, "Currency Exchange") >= 0)
                {
                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                }

                if (Array.IndexOf(type, "Withdrawal") >= 0)
                {
                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                }
            }

            if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
            {
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    ViewData["CompanySourceOfFund"] = form["Company_source_of_fund"];
                    //if (!string.IsNullOrEmpty(form["customerSourceOfFunds.Company_SourceOfFund"]))
                    //{
                    //	if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Bank Credit Line"))
                    //	{
                    //		ViewData["CompanySOFBankCreditCheckbox"] = "checked";
                    //	}

                    //	if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Directors' / Shareholders' / Sole Proprietor's Investments"))
                    //	{
                    //		ViewData["CompanySOFInvestmentCheckbox"] = "checked";
                    //	}

                    //	if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Others"))
                    //	{
                    //		ViewData["CompanySOFOthersCheckbox"] = "checked";
                    //	}
                    //}

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                        }
                    }
                }
                else
                {
                    ViewData["NaturalSourceOfFund"] = form["Natural_source_of_fund"];
                    //if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_SourceOfFund))
                    //{


                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Salary"))
                    //{
                    //	ViewData["NaturalSOFSalaryCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Business Profits"))
                    //{
                    //	ViewData["NaturalSOFBusinessProfitCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Savings"))
                    //{
                    //	ViewData["NaturalSOFSavingsCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Sale of Real Estate"))
                    //{
                    //	ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Gift/Inheritance"))
                    //{
                    //	ViewData["NaturalSOFGiftCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Investment Earnings"))
                    //{
                    //	ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "checked";
                    //}

                    //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Others"))
                    //{
                    //	ViewData["NaturalSOFOthersCheckbox"] = "checked";
                    //}
                    //}

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                    {
                        if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                        {
                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                        {
                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                        {
                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                        {
                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                        {
                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                        }
                    }
                }
            }

            ViewData["ActingAgentForm"] = "display:none;";
            ViewData["CompanyActingAgentYesRadio"] = "";
            ViewData["CompanyActingAgentNoRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
            {
                if (customerActingAgents.ActingAgent == "Yes")
                {
                    ViewData["ActingAgentForm"] = "";
                    ViewData["CompanyActingAgentYesRadio"] = "checked";

                    if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                    {
                        if (customerActingAgents.Company_CustomerType == "Entity")
                        {
                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                        }
                    }
                }
                else
                {
                    ViewData["CompanyActingAgentNoRadio"] = "checked";
                }
            }

            if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
            {
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    List<string> personnelKeys = form.AllKeys.Where(e => e.Contains("Personnel_FullName_")).ToList();
                    List<string> personnels = new List<string>();
                    int count = 1;

                    foreach (string key in personnelKeys)
                    {
                        string id = key.Substring(19);
                        //personnels.Add(count + "|" + form[key].ToString() + "|" + form["Personnel_ICPassport_" + id].ToString() + "|" + form["Personnel_Nationality_" + id].ToString() + "|" + form["Personnel_JobTitle_" + id].ToString() + "|" + form["Personnel_SpecimenSignature_" + id].ToString());
                        personnels.Add(count + "|" + form[key].ToString() + "|" + form["Personnel_ICPassport_" + id].ToString() + "|" + form["Personnel_Nationality_" + id].ToString() + "|" + form["Personnel_JobTitle_" + id].ToString());
                        count++;
                    }

                    ViewData["Personnels"] = personnels;
                }
            }

            //Company POST BACK ITEM
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
            {
                ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
            {
                ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;
            }


            if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
            {
                ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
            {
                ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
            {
                ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
            {
                ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
            {
                ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;
            }

            //NATURAL POST BACK
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
            {
                ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
            {
                ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
            {
                ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
            {
                ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;
            }

            if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
            {
                ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;
            }

            List<string> keys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();
            List<string> customRates = new List<string>();

            foreach (string key in keys)
            {
                string id = key.Substring(14);

                if (!string.IsNullOrEmpty(form["default-rate-" + id]))
                {
                    customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked|" + form["CustomBuyRateDefault_" + id].ToString() + "|" + form["CustomSellRateDefault_" + id].ToString());
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked");
                }
                else
                {
                    customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "||" + form["CustomBuyRateDefault_" + id].ToString() + "|" + form["CustomSellRateDefault_" + id].ToString());
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|");
                }
            }
            ViewData["CustomRates"] = customRates;

            List<string> Ckeys = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();
            List<string> customFees = new List<string>();

            foreach (string key in Ckeys)
            {
                string id = key.Substring(19);

                if (!string.IsNullOrEmpty(form["default-fee-" + id]))
                {
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked");
                    //customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "|checked");
                    customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "|checked|" + form["RemittanceProductsCustomBuyRate_" + id].ToString() + "|" + form["RemittanceProductsCustomSellRate_" + id].ToString() + "|" + form["RemittanceProductsCustomBuyRateDefault_" + id].ToString() + "|" + form["RemittanceProductsCustomSellRateDefault_" + id].ToString());
                }
                else
                {
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|");
                    //customFees.Add(id + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "|");
                    customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "||" + form["RemittanceProductsCustomBuyRate_" + id].ToString() + "|" + form["RemittanceProductsCustomSellRate_" + id].ToString() + "|" + form["RemittanceProductsCustomBuyRateDefault_" + id].ToString() + "|" + form["RemittanceProductsCustomSellRateDefault_" + id].ToString());
                }
            }
            ViewData["remittanceProductCustomFees"] = customFees;

            ViewData["BankAccountNo"] = bankAccountNos;
            ViewData["PEPScreeningReports"] = screeningReports;

            ViewData["AutoFillNextReviewDate"] = false;
            ViewData["CustomerTitle"] = form["Customer_Title"];

            ViewData["CompanyServiceLikeToUse"] = form["Company_service_like_to_use"];
            ViewData["CompanyPurposeOfIntendedTransactions"] = form["Company_purpose_of_intended_transactions"];
            ViewData["CompanyHearAboutUs"] = form["Company_hear_about_us"];

            ViewData["NaturalServiceLikeToUse"] = form["Natural_service_like_to_use"];
            ViewData["NaturalPurposeOfIntendedTransactions"] = form["Natural_purpose_of_intended_transactions"];
            ViewData["NaturalHearAboutUs"] = form["Natural_hear_about_us"];

            ViewData["CustomerDOB"] = form["dob-datepicker"];

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }


        [RedirectingActionWithDLFNIVOMCSGMSA]
        public ActionResult Edit(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData.Peek("Page"));
                ViewData["Page"] = page;
                TempData.Keep("Page");
            }

            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            if (customerParticulars != null)
            {
                //API
                string token = GetTokenDemo();
                eKYCRepository kyc = new eKYCRepository();
                int art_id = kyc.FindArtemisID(customerParticulars.ID);
                if (art_id > 0)
                {
                    ViewData["ArtemisStatus"] = getArtemisStatus(art_id, token);
                }
                else
                {
                    ViewData["ArtemisStatus"] = "No Record";
                }
                

                //Start Here
                if (customerParticulars.Others[0].Status == "Pending Approval")
                {
                    var hasTempRecord = false;

                    //check have temp record or not first
                    using (var context2 = new DataAccess.GreatEastForex())
                    {
                        Temp_CustomerParticulars TempCustomerParticulars = context2.Temp_CustomerParticulars.Where(e => e.Customer_MainID == id).FirstOrDefault();

                        if (TempCustomerParticulars != null)
                        {
                            hasTempRecord = true;
                        }
                    }

                    if (hasTempRecord)
                    {
                        //Pending Approval, Show Temp Record
                        //This is Temp Table item
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            //This is verified and update from customre portal and pending approval status
                            Temp_CustomerParticulars TempCustomerParticulars = context.Temp_CustomerParticulars.Where(e => e.Customer_MainID == id).FirstOrDefault();
                            Temp_CustomerSourceOfFunds customerSourceOfFunds = context.Temp_CustomerSourceOfFunds.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();

                            if (customerSourceOfFunds == null)
                            {
                                customerSourceOfFunds = new Temp_CustomerSourceOfFunds();
                            }

                            Temp_CustomerActingAgents customerActingAgents = context.Temp_CustomerActingAgents.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();
                            if (customerActingAgents == null)
                            {
                                customerActingAgents = new Temp_CustomerActingAgents();
                            }

                            Temp_CustomerDocumentCheckLists customerDocumentChecklists = context.Temp_CustomerDocumentsCheckList.Where(e => e.CustomerParticularId == TempCustomerParticulars.Customer_MainID).FirstOrDefault();

                            if (customerDocumentChecklists == null)
                            {
                                customerDocumentChecklists = new Temp_CustomerDocumentCheckLists();
                            }

                            Temp_CustomerOthers customerOthers = context.Temp_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

                            if (customerParticulars.Others[0].Status == "Pending Approval")
                            {
                                customerOthers.Status = "Pending Approval";
                            }

                            ViewData["CustomerParticular"] = TempCustomerParticulars;
                            ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                            ViewData["CustomerActingAgent"] = customerActingAgents;
                            ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                            ViewData["CustomerOther"] = customerOthers;
                            ViewData["PendingApproval"] = "No";
                            ViewData["CustomerID"] = customerParticulars.ID;
                            ViewData["CustomerTitle"] = TempCustomerParticulars.Customer_Title;
                            ViewData["EditStatus"] = "KYCVerifiedAndReupdate";
                            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.EnableTransactionType))
                            {
                                string[] type = TempCustomerParticulars.EnableTransactionType.Split(',');

                                if (Array.IndexOf(type, "Remittance") >= 0)
                                {
                                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                                }

                                if (Array.IndexOf(type, "Currency Exchange") >= 0)
                                {
                                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                                }

                                if (Array.IndexOf(type, "Withdrawal") >= 0)
                                {
                                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                                }
                            }

                            //Start
                            //Has Customer Account DDL
                            Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                            ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", TempCustomerParticulars.hasCustomerAccount);

                            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", TempCustomerParticulars.isVerify);

                            //Is Main Account DDL
                            //0 - Main Account
                            //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                            Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                            if (customerParticulars.IsSubAccount == 0)
                            {
                                //Main Account Customer DDL
                                Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                                ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                                ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                            }
                            else
                            {
                                //Main Account Customer DDL
                                Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(TempCustomerParticulars.IsSubAccount);
                                ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", TempCustomerParticulars.IsSubAccount.ToString());
                                ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                            }
                            //End

                            //List Search Tag
                            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                            ViewData["SearchTagList"] = getSearchTags;
                            ViewData["SearchTagSelectedItem"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.SearchTags))
                            {
                                ViewData["SearchTagSelectedItem"] = TempCustomerParticulars.SearchTags.Replace("-", "");
                            }

                            if (customerOthers.Status == "Pending Approval")
                            {
                                ViewData["PendingApproval"] = "Yes";
                            }

                            Dropdown[] customerTypeDDL = CustomerTypeDDL();
                            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", TempCustomerParticulars.CustomerType);

                            Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                            ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", TempCustomerParticulars.Company_TypeOfEntity);

                            Dropdown[] gradingDDL = GradingDDL();
                            ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                            Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                            ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                            Dropdown[] customerProfileDDL = CustomerProfileDDL();
                            ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                            if (customerOthers.Status != "Pending Approval")
                            {
                                Dropdown[] statusDDL = StatusDDL();
                                ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                            }

                            ViewData["CompanyForm"] = "";
                            ViewData["NaturalForm"] = "";

                            if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                ViewData["NaturalForm"] = "display:none;";
                            }
                            else
                            {
                                ViewData["CompanyForm"] = "display:none;";
                            }

                            ViewData["NaturalEmployedForm"] = "display:none;";
                            ViewData["NaturalSelfEmployedForm"] = "display:none;";

                            ViewData["NaturalEmploymentEmployedRadio"] = "";
                            ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.Natural_EmploymentType))
                            {
                                if (TempCustomerParticulars.Natural_EmploymentType == "Employed")
                                {
                                    ViewData["NaturalEmployedForm"] = "";
                                    ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSelfEmployedForm"] = "";
                                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                                }
                            }

                            //ViewData for Checkboxes and Radio buttons
                            ViewData["CompanySOFBankCreditCheckbox"] = "";
                            ViewData["CompanySOFInvestmentCheckbox"] = "";
                            ViewData["CompanySOFOthersCheckbox"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                            ViewData["NaturalSOFSalaryCheckbox"] = "";
                            ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                            ViewData["NaturalSOFSavingsCheckbox"] = "";
                            ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                            ViewData["NaturalSOFGiftCheckbox"] = "";
                            ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                            ViewData["NaturalSOFOthersCheckbox"] = "";
                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.CustomerType))
                            {
                                if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                                {
                                    ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                                    ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                                    ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                                    ViewData["CustomerCountry"] = TempCustomerParticulars.Company_Country;
                                    ViewData["CustomerCountryCode"] = TempCustomerParticulars.Company_CountryCode;

                                    ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                                    ViewData["CustomerDOB"] = "";

                                    if (TempCustomerParticulars.DOB != null)
                                    {
                                        ViewData["CustomerDOB"] = Convert.ToDateTime(TempCustomerParticulars.DOB).ToString("dd-MM-yyyy");
                                    }

                                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                                    {
                                        ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;
                                    }

                                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                                    {
                                        ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                                    {
                                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                        }
                                    }
                                }
                                else
                                {
                                    ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                                    ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                                    ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                                    ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                                    {
                                        if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                        }
                                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                        {
                                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                                    {
                                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                        }
                                    }
                                }
                            }

                            ViewData["ActingAgentForm"] = "display:none;";
                            ViewData["CompanyActingAgentYesRadio"] = "";
                            ViewData["CompanyActingAgentNoRadio"] = "";
                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                            {
                                if (customerActingAgents.ActingAgent == "Yes")
                                {
                                    ViewData["ActingAgentForm"] = "";
                                    ViewData["CompanyActingAgentYesRadio"] = "checked";

                                    if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                                    {
                                        if (customerActingAgents.Company_CustomerType == "Entity")
                                        {
                                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                        }
                                        else
                                        {
                                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                        }
                                    }
                                }
                                else
                                {
                                    ViewData["CompanyActingAgentNoRadio"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(TempCustomerParticulars.CustomerType))
                            {
                                if (TempCustomerParticulars.CustomerType == "Corporate & Trading Company")
                                {
                                    List<Temp_CustomerAppointmentOfStaffs> appointmentOfStaffs = context.Temp_CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == id).ToList();
                                    List<string> personnels = new List<string>();
                                    int count = 1;

                                    foreach (Temp_CustomerAppointmentOfStaffs appointment in appointmentOfStaffs)
                                    {
                                        //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                        personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                        count++;
                                    }

                                    ViewData["Personnels"] = personnels;
                                }
                            }

                            //ViewData for File Uploads Domain Folders
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                            {
                                ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                                string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                            {
                                ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                                string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                            {
                                ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                                string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                            {
                                ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                                string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                            {
                                ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                                string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                                    count++;
                                }
                            }
                            else
                            {
                                if (customerDocumentChecklists.Company_AccountOpeningForm == "")
                                {
                                    ViewData["FromCustomerPortalCompanyAccountOpeningFormFiles"] = "1";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                            {
                                ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                                string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                            {
                                ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                                string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                            {
                                ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                                string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                            {
                                ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                                string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                            {
                                ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                                string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                            {
                                ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                                string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                                    count++;
                                }
                            }

                            if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                            {
                                ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                                string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                                int count = 1;

                                foreach (string file in files)
                                {
                                    ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                                    count++;
                                }
                            }

                            List<string> bankAccountNos = new List<string>();

                            if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                            {
                                bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                            }

                            ViewData["BankAccountNo"] = bankAccountNos;

                            List<string> screeningReports = new List<string>();
                            List<Temp_CustomerScreeningReports> TempCustomerScreeningReports = context.Temp_CustomerScreeningReports.Where(e => e.CustomerParticularId == id).ToList();

                            if (TempCustomerScreeningReports.Count > 0)
                            {
                                foreach (Temp_CustomerScreeningReports screeningReport in TempCustomerScreeningReports)
                                {
                                    screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                                }
                            }

                            List<CustomerActivityLog> getActivityLogs = context.CustomerActivityLogs.Where(e => e.CustomerParticularId == id).ToList();

                            List<string> activityLogs = new List<string>();

                            if (getActivityLogs.Count > 0)
                            {
                                foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                                {
                                    activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                                }
                            }

                            ViewData["ActivityLog"] = activityLogs;

                            IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                            List<string> customRates = new List<string>();
                            string GetBuyRate = "0";
                            string GetSellRate = "0";
                            string AdjustmentBuyRate = "0";
                            string AdjustmentSellRate = "0";

                            foreach (Product product in products)
                            {
                                Temp_CustomerCustomRates customRate = _TempCustomerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                                GetBuyRate = "0";
                                GetSellRate = "0";
                                AdjustmentBuyRate = "0";
                                AdjustmentSellRate = "0";

                                if (product.BuyRate != 0 && product.BuyRate != null)
                                {
                                    GetBuyRate = product.BuyRate.ToString();
                                }
                                else
                                {
                                    if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                                    {
                                        GetBuyRate = product.AutomatedBuyRate.ToString();
                                    }
                                    else
                                    {
                                        GetBuyRate = "0";
                                    }
                                }

                                if (product.SellRate != 0 && product.SellRate != null)
                                {
                                    GetSellRate = product.SellRate.ToString();
                                }
                                else
                                {
                                    if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                                    {
                                        GetSellRate = product.AutomatedSellRate.ToString();
                                    }
                                    else
                                    {
                                        GetSellRate = "0";
                                    }
                                }

                                if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                                {
                                    AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    AdjustmentBuyRate = "0";
                                }

                                if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                                {
                                    AdjustmentSellRate = product.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    AdjustmentSellRate = "0";
                                }

                                if (customRate != null)
                                {
                                    customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                                }
                                else
                                {
                                    customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                                }
                            }
                            ViewData["CustomRates"] = customRates;

                            List<string> remittanceProductCustomRates = new List<string>();

                            var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                            string RemittanceGetBuyRate = "0";
                            string RemittanceGetSellRate = "0";
                            string RemittanceAdjustmentBuyRate = "0";
                            string RemittanceAdjustmentSellRate = "0";

                            foreach (RemittanceProducts r in RemittanceProducts)
                            {
                                RemittanceGetBuyRate = "0";
                                RemittanceGetSellRate = "0";
                                RemittanceAdjustmentBuyRate = "0";
                                RemittanceAdjustmentSellRate = "0";

                                var checkRemittanceCustomRate = context.Temp_CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                                if (r.PayRate != 0 && r.PayRate != null)
                                {
                                    RemittanceGetBuyRate = r.PayRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                    {
                                        RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetBuyRate = "0";
                                    }
                                }

                                if (r.GetRate != 0 && r.GetRate != null)
                                {
                                    RemittanceGetSellRate = r.GetRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                    {
                                        RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetSellRate = "0";
                                    }
                                }

                                if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                                {
                                    RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentBuyRate = "0";
                                }

                                if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                                {
                                    RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentSellRate = "0";
                                }

                                if (checkRemittanceCustomRate != null)
                                {
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                    remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                                else
                                {
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked");
                                    remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                            }

                            ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;

                            ViewData["PEPScreeningReports"] = screeningReports;
                            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                            return View();
                        }
                    }
                    else
                    {
                        //no temp record, get main data
                        //Show Main Record
                        CustomerSourceOfFund customerSourceOfFunds = (customerParticulars.SourceOfFunds.Count == 0 ? new CustomerSourceOfFund() : customerParticulars.SourceOfFunds[0]);
                        CustomerActingAgent customerActingAgents = (customerParticulars.ActingAgents.Count == 0 ? new CustomerActingAgent() : customerParticulars.ActingAgents[0]);
                        CustomerDocumentCheckList customerDocumentChecklists = (customerParticulars.DocumentCheckLists.Count == 0 ? new CustomerDocumentCheckList() : customerParticulars.DocumentCheckLists[0]);
                        CustomerOther customerOthers = customerParticulars.Others[0];

                        ViewData["CustomerParticular"] = customerParticulars;
                        ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                        ViewData["CustomerActingAgent"] = customerActingAgents;
                        ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                        ViewData["CustomerOther"] = customerOthers;
                        ViewData["PendingApproval"] = "No";
                        ViewData["CustomerID"] = customerParticulars.ID;
                        ViewData["CustomerTitle"] = customerParticulars.Customer_Title;
                        ViewData["EditStatus"] = "KYCNoVerifyCreatedFromOMS";
                        ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                        ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                        ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.EnableTransactionType))
                        {
                            string[] type = customerParticulars.EnableTransactionType.Split(',');

                            if (Array.IndexOf(type, "Remittance") >= 0)
                            {
                                ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                            }

                            if (Array.IndexOf(type, "Currency Exchange") >= 0)
                            {
                                ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                            }

                            if (Array.IndexOf(type, "Withdrawal") >= 0)
                            {
                                ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                            }
                        }

                        //Start
                        //Has Customer Account DDL
                        Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                        ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", customerParticulars.hasCustomerAccount);

                        Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                        ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                        //Is Main Account DDL
                        //0 - Main Account
                        //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                        Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                        if (customerParticulars.IsSubAccount == 0)
                        {
                            //Main Account Customer DDL
                            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                        }
                        else
                        {
                            //have subaccount
                            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(customerParticulars.IsSubAccount);
                            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", customerParticulars.IsSubAccount.ToString());
                            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                        }
                        //End

                        //List Search Tag
                        IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                        ViewData["SearchTagList"] = getSearchTags;
                        ViewData["SearchTagSelectedItem"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.SearchTags))
                        {
                            ViewData["SearchTagSelectedItem"] = customerParticulars.SearchTags.Replace("-", "");
                        }

                        if (customerOthers.Status == "Pending Approval")
                        {
                            ViewData["PendingApproval"] = "Yes";
                        }

                        Dropdown[] customerTypeDDL = CustomerTypeDDL();
                        ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

                        Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                        ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

                        Dropdown[] gradingDDL = GradingDDL();
                        ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                        Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                        ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                        Dropdown[] customerProfileDDL = CustomerProfileDDL();
                        ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                        if (customerOthers.Status != "Pending Approval")
                        {
                            Dropdown[] statusDDL = StatusDDL();
                            ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                        }

                        ViewData["CompanyForm"] = "";
                        ViewData["NaturalForm"] = "";

                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            ViewData["NaturalForm"] = "display:none;";
                        }
                        else
                        {
                            ViewData["CompanyForm"] = "display:none;";
                        }

                        ViewData["NaturalEmployedForm"] = "display:none;";
                        ViewData["NaturalSelfEmployedForm"] = "display:none;";

                        ViewData["NaturalEmploymentEmployedRadio"] = "";
                        ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
                        {
                            if (customerParticulars.Natural_EmploymentType == "Employed")
                            {
                                ViewData["NaturalEmployedForm"] = "";
                                ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                            }
                            else
                            {
                                ViewData["NaturalSelfEmployedForm"] = "";
                                ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                            }
                        }

                        //ViewData for Checkboxes and Radio buttons
                        ViewData["CompanySOFBankCreditCheckbox"] = "";
                        ViewData["CompanySOFInvestmentCheckbox"] = "";
                        ViewData["CompanySOFOthersCheckbox"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                        ViewData["NaturalSOFSalaryCheckbox"] = "";
                        ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                        ViewData["NaturalSOFSavingsCheckbox"] = "";
                        ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                        ViewData["NaturalSOFGiftCheckbox"] = "";
                        ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                        ViewData["NaturalSOFOthersCheckbox"] = "";
                        ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                        ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                        ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                        if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                        {
                            if (customerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                                ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                                ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                                ViewData["CustomerDOB"] = "";
                                ViewData["CustomerCountry"] = customerParticulars.Company_Country;
                                ViewData["CustomerCountryCode"] = customerParticulars.Company_CountryCode;

                                ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                                if (customerParticulars.DOB != null)
                                {
                                    ViewData["CustomerDOB"] = Convert.ToDateTime(customerParticulars.DOB).ToString("dd-MM-yyyy");
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                                {
                                    if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                    }
                                }
                            }
                            else
                            {

                                ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                                ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                                ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                                ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                                {
                                    if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                    }
                                    else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                    {
                                        ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                                {
                                    if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                    }
                                }
                            }
                        }

                        ViewData["ActingAgentForm"] = "display:none;";
                        ViewData["CompanyActingAgentYesRadio"] = "";
                        ViewData["CompanyActingAgentNoRadio"] = "";
                        ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                        ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                        if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                        {
                            if (customerActingAgents.ActingAgent == "Yes")
                            {
                                ViewData["ActingAgentForm"] = "";
                                ViewData["CompanyActingAgentYesRadio"] = "checked";

                                if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                                {
                                    if (customerActingAgents.Company_CustomerType == "Entity")
                                    {
                                        ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                    }
                                    else
                                    {
                                        ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                    }
                                }
                            }
                            else
                            {
                                ViewData["CompanyActingAgentNoRadio"] = "checked";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                        {
                            if (customerParticulars.CustomerType == "Corporate & Trading Company")
                            {
                                List<CustomerAppointmentOfStaff> appointmentOfStaffs = customerParticulars.AppointmentOfStaffs;
                                List<string> personnels = new List<string>();
                                int count = 1;

                                foreach (CustomerAppointmentOfStaff appointment in appointmentOfStaffs)
                                {
                                    //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                    personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                    count++;
                                }

                                ViewData["Personnels"] = personnels;
                            }
                        }

                        //ViewData for File Uploads Domain Folders
                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                        {
                            ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                            string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                        {
                            ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                            string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                        {
                            ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                            string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                        {
                            ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                            string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                        {
                            ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                            string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                                count++;
                            }
                        }
                        else
                        {
                            if (customerDocumentChecklists.Company_AccountOpeningForm == "")
                            {
                                ViewData["FromCustomerPortalCompanyAccountOpeningFormFiles"] = "1";
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                        {
                            ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                            string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                        {
                            ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                            string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                        {
                            ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                            string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                        {
                            ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                            string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                        {
                            ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                            string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                        {
                            ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                            string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                                count++;
                            }
                        }

                        if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                        {
                            ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                            string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                            int count = 1;

                            foreach (string file in files)
                            {
                                ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                                count++;
                            }
                        }

                        List<string> bankAccountNos = new List<string>();

                        if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                        {
                            bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                        }

                        ViewData["BankAccountNo"] = bankAccountNos;

                        List<string> screeningReports = new List<string>();

                        foreach (CustomerScreeningReport screeningReport in customerParticulars.PEPScreeningReports)
                        {
                            screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                        }

                        IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

                        List<string> activityLogs = new List<string>();

                        foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                        {
                            activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                        }

                        ViewData["ActivityLog"] = activityLogs;

                        IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                        List<string> customRates = new List<string>();
                        string GetBuyRate = "0";
                        string GetSellRate = "0";
                        string AdjustmentBuyRate = "0";
                        string AdjustmentSellRate = "0";

                        foreach (Product product in products)
                        {
                            CustomerCustomRate customRate = _customerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                            GetBuyRate = "0";
                            GetSellRate = "0";
                            AdjustmentBuyRate = "0";
                            AdjustmentSellRate = "0";

                            if (product.BuyRate != 0 && product.BuyRate != null)
                            {
                                GetBuyRate = product.BuyRate.ToString();
                            }
                            else
                            {
                                if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                                {
                                    GetBuyRate = product.AutomatedBuyRate.ToString();
                                }
                                else
                                {
                                    GetBuyRate = "0";
                                }
                            }

                            if (product.SellRate != 0 && product.SellRate != null)
                            {
                                GetSellRate = product.SellRate.ToString();
                            }
                            else
                            {
                                if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                                {
                                    GetSellRate = product.AutomatedSellRate.ToString();
                                }
                                else
                                {
                                    GetSellRate = "0";
                                }
                            }

                            if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                            {
                                AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                            }
                            else
                            {
                                AdjustmentBuyRate = "0";
                            }

                            if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                            {
                                AdjustmentSellRate = product.SellRateAdjustment.ToString();
                            }
                            else
                            {
                                AdjustmentSellRate = "0";
                            }

                            if (customRate != null)
                            {
                                customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                            }
                            else
                            {
                                customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                            }
                        }
                        ViewData["CustomRates"] = customRates;

                        using (var context = new DataAccess.GreatEastForex())
                        {
                            List<string> remittanceProductCustomRates = new List<string>();

                            var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                            string RemittanceGetBuyRate = "0";
                            string RemittanceGetSellRate = "0";
                            string RemittanceAdjustmentBuyRate = "0";
                            string RemittanceAdjustmentSellRate = "0";

                            foreach (RemittanceProducts r in RemittanceProducts)
                            {

                                RemittanceGetBuyRate = "0";
                                RemittanceGetSellRate = "0";
                                RemittanceAdjustmentBuyRate = "0";
                                RemittanceAdjustmentSellRate = "0";

                                var checkRemittanceCustomRate = context.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                                if (r.PayRate != 0 && r.PayRate != null)
                                {
                                    RemittanceGetBuyRate = r.PayRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                    {
                                        RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetBuyRate = "0";
                                    }
                                }

                                if (r.GetRate != 0 && r.GetRate != null)
                                {
                                    RemittanceGetSellRate = r.GetRate.ToString();
                                }
                                else
                                {
                                    if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                    {
                                        RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                    }
                                    else
                                    {
                                        RemittanceGetSellRate = "0";
                                    }
                                }

                                if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                                {
                                    RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentBuyRate = "0";
                                }

                                if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                                {
                                    RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                                }
                                else
                                {
                                    RemittanceAdjustmentSellRate = "0";
                                }

                                if (checkRemittanceCustomRate != null)
                                {
                                    //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                    //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                    remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                                else
                                {
                                    //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                    remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                                }
                            }

                            ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;
                        }

                        ViewData["PEPScreeningReports"] = screeningReports;
                        ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                        return View();
                    }
                }
                else
                {
                    //Show Main Record
                    CustomerSourceOfFund customerSourceOfFunds = (customerParticulars.SourceOfFunds.Count == 0 ? new CustomerSourceOfFund() : customerParticulars.SourceOfFunds[0]);
                    CustomerActingAgent customerActingAgents = (customerParticulars.ActingAgents.Count == 0 ? new CustomerActingAgent() : customerParticulars.ActingAgents[0]);
                    CustomerDocumentCheckList customerDocumentChecklists = (customerParticulars.DocumentCheckLists.Count == 0 ? new CustomerDocumentCheckList() : customerParticulars.DocumentCheckLists[0]);
                    CustomerOther customerOthers = customerParticulars.Others[0];

                    ViewData["CustomerParticular"] = customerParticulars;
                    ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
                    ViewData["CustomerActingAgent"] = customerActingAgents;
                    ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
                    ViewData["CustomerOther"] = customerOthers;
                    ViewData["PendingApproval"] = "No";
                    ViewData["CustomerID"] = customerParticulars.ID;
                    ViewData["CustomerTitle"] = customerParticulars.Customer_Title;
                    ViewData["EditStatus"] = "KYCNoVerifyCreatedFromOMS";
                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.EnableTransactionType))
                    {
                        string[] type = customerParticulars.EnableTransactionType.Split(',');

                        if (Array.IndexOf(type, "Remittance") >= 0)
                        {
                            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                        }

                        if (Array.IndexOf(type, "Currency Exchange") >= 0)
                        {
                            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                        }

                        if (Array.IndexOf(type, "Withdrawal") >= 0)
                        {
                            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                        }
                    }

                    //Start
                    //Has Customer Account DDL
                    Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
                    ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", customerParticulars.hasCustomerAccount);

                    Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                    ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                    //Is Main Account DDL
                    //0 - Main Account
                    //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
                    Dropdown[] isMainAccountDDL = IsMainAccountDDL();

                    if (customerParticulars.IsSubAccount == 0)
                    {
                        //Main Account Customer DDL
                        Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(0);
                        ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name");
                        ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "0");
                    }
                    else
                    {
                        //have subaccount
                        Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(customerParticulars.IsSubAccount);
                        ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", customerParticulars.IsSubAccount.ToString());
                        ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", "1");
                    }
                    //End

                    //List Search Tag
                    IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
                    ViewData["SearchTagList"] = getSearchTags;
                    ViewData["SearchTagSelectedItem"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.SearchTags))
                    {
                        ViewData["SearchTagSelectedItem"] = customerParticulars.SearchTags.Replace("-", "");
                    }

                    if (customerOthers.Status == "Pending Approval")
                    {
                        ViewData["PendingApproval"] = "Yes";
                    }

                    Dropdown[] customerTypeDDL = CustomerTypeDDL();
                    ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

                    Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
                    ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

                    Dropdown[] gradingDDL = GradingDDL();
                    ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

                    Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
                    ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

                    Dropdown[] customerProfileDDL = CustomerProfileDDL();
                    ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

                    if (customerOthers.Status != "Pending Approval")
                    {
                        Dropdown[] statusDDL = StatusDDL();
                        ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
                    }

                    ViewData["CompanyForm"] = "";
                    ViewData["NaturalForm"] = "";

                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        ViewData["NaturalForm"] = "display:none;";
                    }
                    else
                    {
                        ViewData["CompanyForm"] = "display:none;";
                    }

                    ViewData["NaturalEmployedForm"] = "display:none;";
                    ViewData["NaturalSelfEmployedForm"] = "display:none;";

                    ViewData["NaturalEmploymentEmployedRadio"] = "";
                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
                    {
                        if (customerParticulars.Natural_EmploymentType == "Employed")
                        {
                            ViewData["NaturalEmployedForm"] = "";
                            ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSelfEmployedForm"] = "";
                            ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                        }
                    }

                    //ViewData for Checkboxes and Radio buttons
                    ViewData["CompanySOFBankCreditCheckbox"] = "";
                    ViewData["CompanySOFInvestmentCheckbox"] = "";
                    ViewData["CompanySOFOthersCheckbox"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
                    ViewData["NaturalSOFSalaryCheckbox"] = "";
                    ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
                    ViewData["NaturalSOFSavingsCheckbox"] = "";
                    ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
                    ViewData["NaturalSOFGiftCheckbox"] = "";
                    ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
                    ViewData["NaturalSOFOthersCheckbox"] = "";
                    ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
                    ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
                    ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";

                    if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                    {
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            ViewData["CompanyServiceLikeToUse"] = customerSourceOfFunds.Company_ServiceLikeToUse;
                            ViewData["CompanyPurposeOfIntendedTransactions"] = customerSourceOfFunds.Company_PurposeOfIntendedTransactions;
                            ViewData["CompanyHearAboutUs"] = customerSourceOfFunds.Company_HearAboutUs;

                            ViewData["CustomerDOB"] = "";
                            ViewData["CustomerCountry"] = customerParticulars.Company_Country;
                            ViewData["CustomerCountryCode"] = customerParticulars.Company_CountryCode;

                            ViewData["CompanySourceOfFund"] = customerSourceOfFunds.Company_SourceOfFund;

                            if (customerParticulars.DOB != null)
                            {
                                ViewData["CustomerDOB"] = Convert.ToDateTime(customerParticulars.DOB).ToString("dd-MM-yyyy");
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                            {
                                if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                }
                            }
                        }
                        else
                        {

                            ViewData["NaturalServiceLikeToUse"] = customerSourceOfFunds.Natural_ServiceLikeToUse;
                            ViewData["NaturalPurposeOfIntendedTransactions"] = customerSourceOfFunds.Natural_PurposeOfIntendedTransactions;
                            ViewData["NaturalHearAboutUs"] = customerSourceOfFunds.Natural_HearAboutUs;

                            ViewData["NaturalSourceOfFund"] = customerSourceOfFunds.Natural_SourceOfFund;

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                            {
                                if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                                {
                                    ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                                {
                                    ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                                {
                                    ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                                {
                                    ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                                }
                                else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                                {
                                    ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                                }
                            }

                            if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                            {
                                if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                                }
                                else
                                {
                                    ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                                }
                            }
                        }
                    }

                    ViewData["ActingAgentForm"] = "display:none;";
                    ViewData["CompanyActingAgentYesRadio"] = "";
                    ViewData["CompanyActingAgentNoRadio"] = "";
                    ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
                    ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

                    if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
                    {
                        if (customerActingAgents.ActingAgent == "Yes")
                        {
                            ViewData["ActingAgentForm"] = "";
                            ViewData["CompanyActingAgentYesRadio"] = "checked";

                            if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                            {
                                if (customerActingAgents.Company_CustomerType == "Entity")
                                {
                                    ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                                }
                                else
                                {
                                    ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                                }
                            }
                        }
                        else
                        {
                            ViewData["CompanyActingAgentNoRadio"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
                    {
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {
                            List<CustomerAppointmentOfStaff> appointmentOfStaffs = customerParticulars.AppointmentOfStaffs;
                            List<string> personnels = new List<string>();
                            int count = 1;

                            foreach (CustomerAppointmentOfStaff appointment in appointmentOfStaffs)
                            {
                                //personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|" + appointment.SpecimenSignature + "|Old");
                                personnels.Add(count + "|" + appointment.FullName + "|" + appointment.ICPassportNo + "|" + appointment.Nationality + "|" + appointment.JobTitle + "|Old");
                                count++;
                            }

                            ViewData["Personnels"] = personnels;
                        }
                    }

                    //ViewData for File Uploads Domain Folders
                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                    {
                        ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                        string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                    {
                        ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                        string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                    {
                        ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                        string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                    {
                        ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                        string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                    {
                        ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                        string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();

                            count++;
                        }
                    }
                    else
                    {
                        if (customerDocumentChecklists.Company_AccountOpeningForm == "")
                        {
                            ViewData["FromCustomerPortalCompanyAccountOpeningFormFiles"] = "1";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                    {
                        ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                        string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                    {
                        ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                        string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                    {
                        ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                        string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                    {
                        ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                        string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                    {
                        ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                        string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                    {
                        ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                        string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();

                            count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                    {
                        ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                        string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                        int count = 1;

                        foreach (string file in files)
                        {
                            ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();

                            count++;
                        }
                    }

                    List<string> bankAccountNos = new List<string>();

                    if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                    {
                        bankAccountNos.AddRange(customerOthers.BankAccountNo.Split('|').ToList());
                    }

                    ViewData["BankAccountNo"] = bankAccountNos;

                    List<string> screeningReports = new List<string>();

                    foreach (CustomerScreeningReport screeningReport in customerParticulars.PEPScreeningReports)
                    {
                        screeningReports.Add(screeningReport.Date.ToString("dd/MM/yyyy") + "|" + screeningReport.DateOfAcra.ToString("dd/MM/yyyy") + "|" + screeningReport.ScreenedBy + "|" + screeningReport.ScreeningReport_1 + "|" + screeningReport.ScreeningReport_2 + "|" + screeningReport.Remarks);
                    }

                    IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

                    List<string> activityLogs = new List<string>();

                    foreach (CustomerActivityLog _activitylogs in getActivityLogs)
                    {
                        activityLogs.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note + "|" + _activitylogs.ID);
                    }

                    ViewData["ActivityLog"] = activityLogs;

                    IList<Product> products = _productsModel.GetAll(new List<string> { "Encashment", "Deposit" });
                    List<string> customRates = new List<string>();
                    string GetBuyRate = "0";
                    string GetSellRate = "0";
                    string AdjustmentBuyRate = "0";
                    string AdjustmentSellRate = "0";

                    foreach (Product product in products)
                    {
                        CustomerCustomRate customRate = _customerCustomRatesModel.GetCustomerProductRate(id, product.ID);

                        GetBuyRate = "0";
                        GetSellRate = "0";
                        AdjustmentBuyRate = "0";
                        AdjustmentSellRate = "0";

                        if (product.BuyRate != 0 && product.BuyRate != null)
                        {
                            GetBuyRate = product.BuyRate.ToString();
                        }
                        else
                        {
                            if (product.AutomatedBuyRate != 0 && product.AutomatedBuyRate != null)
                            {
                                GetBuyRate = product.AutomatedBuyRate.ToString();
                            }
                            else
                            {
                                GetBuyRate = "0";
                            }
                        }

                        if (product.SellRate != 0 && product.SellRate != null)
                        {
                            GetSellRate = product.SellRate.ToString();
                        }
                        else
                        {
                            if (product.AutomatedSellRate != 0 && product.AutomatedSellRate != null)
                            {
                                GetSellRate = product.AutomatedSellRate.ToString();
                            }
                            else
                            {
                                GetSellRate = "0";
                            }
                        }

                        if (product.BuyRateAdjustment != 0 && product.BuyRateAdjustment != null)
                        {
                            AdjustmentBuyRate = product.BuyRateAdjustment.ToString();
                        }
                        else
                        {
                            AdjustmentBuyRate = "0";
                        }

                        if (product.SellRateAdjustment != 0 && product.SellRateAdjustment != null)
                        {
                            AdjustmentSellRate = product.SellRateAdjustment.ToString();
                        }
                        else
                        {
                            AdjustmentSellRate = "0";
                        }

                        if (customRate != null)
                        {
                            customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|unchecked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.EncashmentRate.ToString("#,##0.########") + "|unchecked");
                        }
                        else
                        {
                            customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + AdjustmentBuyRate + "|" + AdjustmentSellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked|" + GetBuyRate + "|" + GetSellRate);
                            //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.EncashmentRate.ToString("#,##0.########") + "|checked");
                        }
                    }
                    ViewData["CustomRates"] = customRates;

                    using (var context = new DataAccess.GreatEastForex())
                    {
                        List<string> remittanceProductCustomRates = new List<string>();

                        var RemittanceProducts = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();

                        string RemittanceGetBuyRate = "0";
                        string RemittanceGetSellRate = "0";
                        string RemittanceAdjustmentBuyRate = "0";
                        string RemittanceAdjustmentSellRate = "0";

                        foreach (RemittanceProducts r in RemittanceProducts)
                        {

                            RemittanceGetBuyRate = "0";
                            RemittanceGetSellRate = "0";
                            RemittanceAdjustmentBuyRate = "0";
                            RemittanceAdjustmentSellRate = "0";

                            var checkRemittanceCustomRate = context.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id && e.RemittanceProductId == r.ID).FirstOrDefault();

                            if (r.PayRate != 0 && r.PayRate != null)
                            {
                                RemittanceGetBuyRate = r.PayRate.ToString();
                            }
                            else
                            {
                                if (r.AutomatedPayRate != 0 && r.AutomatedPayRate != null)
                                {
                                    RemittanceGetBuyRate = r.AutomatedPayRate.ToString();
                                }
                                else
                                {
                                    RemittanceGetBuyRate = "0";
                                }
                            }

                            if (r.GetRate != 0 && r.GetRate != null)
                            {
                                RemittanceGetSellRate = r.GetRate.ToString();
                            }
                            else
                            {
                                if (r.AutomatedGetRate != 0 && r.AutomatedGetRate != null)
                                {
                                    RemittanceGetSellRate = r.AutomatedGetRate.ToString();
                                }
                                else
                                {
                                    RemittanceGetSellRate = "0";
                                }
                            }

                            if (r.BuyRateAdjustment != 0 && r.BuyRateAdjustment != null)
                            {
                                RemittanceAdjustmentBuyRate = r.BuyRateAdjustment.ToString();
                            }
                            else
                            {
                                RemittanceAdjustmentBuyRate = "0";
                            }

                            if (r.SellRateAdjustment != 0 && r.SellRateAdjustment != null)
                            {
                                RemittanceAdjustmentSellRate = r.SellRateAdjustment.ToString();
                            }
                            else
                            {
                                RemittanceAdjustmentSellRate = "0";
                            }

                            if (checkRemittanceCustomRate != null)
                            {
                                //customRates.Add(customRate.ProductId + "|" + customRate.Products.CurrencyCode + "|" + customRate.BuyRate + "|" + customRate.SellRate + "|" + customRate.EncashmentRate + "|checked");
                                //remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + checkRemittanceCustomRate.Fee.ToString("#,##0.########") + "|unchecked");
                                remittanceProductCustomRates.Add(checkRemittanceCustomRate.RemittanceProductId + "|" + r.CurrencyCode + "|" + (checkRemittanceCustomRate.Fee.HasValue ? Convert.ToDecimal(checkRemittanceCustomRate.Fee.Value.ToString("#,##0.########")) : 0) + "|unchecked|" + checkRemittanceCustomRate.PayRateAdjustment + "|" + checkRemittanceCustomRate.GetRateAdjustment + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                            }
                            else
                            {
                                //customRates.Add(product.ID + "|" + product.CurrencyCode + "|" + product.BuyRate + "|" + product.SellRate + "|" + product.EncashmentRate + "|checked");
                                remittanceProductCustomRates.Add(r.ID + "|" + r.CurrencyCode + "|" + r.TransactionFee.ToString("#,##0.########") + "|checked|" + RemittanceAdjustmentBuyRate + "|" + RemittanceAdjustmentSellRate + "|" + RemittanceGetBuyRate + "|" + RemittanceGetSellRate);
                            }
                        }

                        ViewData["remittanceProductCustomFees"] = remittanceProductCustomRates;
                    }

                    ViewData["PEPScreeningReports"] = screeningReports;
                    ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                    return View();
                }
                //End Heres
            }
            else
            {
                TempData.Add("Result", "danger|Customer record not found!");
            }

            return RedirectToAction("Listing", new { @page = page });
        }

        //GET: Edit [DL, GM, SA] [FN, IV, OM, CS for view only]
        [RedirectingActionWithDLFNIVOMCSGMSA]
        [HttpPost]
        public ActionResult Edit(int id, CustomerParticular customerParticulars, CustomerSourceOfFund customerSourceOfFunds, CustomerActingAgent customerActingAgents, CustomerDocumentCheckList customerDocumentChecklists, CustomerOther customerOthers, FormCollection form)
        {
            int page = 1;
            int IsSubAccount = 0;//0 = Is Main Account, 1 = Is Sub Account

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
                ViewData["Page"] = page;
            }

            string chck = form["EditStatusHidden"];
            ViewData["EditStatus"] = form["EditStatusHidden"];

            //validate has customer account
            if (form["customerParticulars.hasCustomerAccount"] != null)
            {
                string checkValue = form["customerParticulars.hasCustomerAccount"];

                if (form["customerParticulars.hasCustomerAccount"] == "1")
                {
                    CustomerParticular CheckBeforeHasCustomerAccount = _customerParticularsModel.GetSingle(id);

                    if (CheckBeforeHasCustomerAccount != null)
                    {
                        if (CheckBeforeHasCustomerAccount.hasCustomerAccount == 0)
                        {
                            //0 - 1 (must add password)
                            //verify the password and confirm password
                            if (string.IsNullOrEmpty(form["Password"].Trim()))
                            {
                                ModelState.AddModelError("Password", "Password is required!");
                            }

                            if (string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                            {
                                ModelState.AddModelError("ConfirmPassword", "Confirm Password is required!");
                            }

                            if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()) && !string.IsNullOrEmpty(form["Password"].Trim()))
                            {
                                string Password = form["Password"].Trim();
                                string ConfirmPassword = form["ConfirmPassword"].Trim();

                                if (Password.Length >= 8 && ConfirmPassword.Length >= 8)
                                {
                                    //check regex
                                    Regex regexPattern = new Regex("^(?=.*[A-Za-z])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})");

                                    if (!regexPattern.IsMatch(Password))
                                    {
                                        ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters and 1 special characters!");
                                    }
                                    else
                                    {
                                        if (form["Password"].Trim() != form["ConfirmPassword"].Trim())
                                        {
                                            ModelState.AddModelError("Password", "New Password and Confirm Password not match!");
                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters!");
                                }
                            }
                        }
                        else
                        {
                            //1 = 1, check have password keyin or not.
                            //Check have password or not, if have, then check validation
                            //verify the password and confirm password
                            if (!string.IsNullOrEmpty(form["Password"].Trim()))
                            {
                                if (string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                                {
                                    ModelState.AddModelError("ConfirmPassword", "Confirm Password is required!");
                                }
                            }

                            if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                            {
                                if (string.IsNullOrEmpty(form["Password"].Trim()))
                                {
                                    ModelState.AddModelError("Password", "Password is required!");
                                }
                            }

                            if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()) && !string.IsNullOrEmpty(form["Password"].Trim()))
                            {
                                string Password = form["Password"].Trim();
                                string ConfirmPassword = form["ConfirmPassword"].Trim();

                                if (Password.Length >= 8 && ConfirmPassword.Length >= 8)
                                {
                                    //check regex
                                    Regex regexPattern = new Regex("^(?=.*[A-Za-z])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})");

                                    if (!regexPattern.IsMatch(Password))
                                    {
                                        ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters and 1 special characters!");
                                    }
                                    else
                                    {
                                        if (form["Password"].Trim() != form["ConfirmPassword"].Trim())
                                        {
                                            ModelState.AddModelError("Password", "New Password and Confirm Password not match!");
                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters!");
                                }
                            }
                        }
                    }
                }
            }

            //validate customerParticulars.IsSubAccount
            if (form["customerParticulars.IsSubAccount"] != null)
            {
                if (form["customerParticulars.IsSubAccount"] != "0")
                {
                    IsSubAccount = 1;
                    //validate main customer dropdown value
                    if (form["MainAccountCustomer"] != null)
                    {
                        if (!FormValidationHelper.IntegerValidation(form["MainAccountCustomer"]))
                        {
                            ModelState.AddModelError("MainAccountCustomer", "Something went wrong on selecting Main Account Customer!");
                        }
                    }
                }
            }

            //Title, Surname, given name validation
            //if (form["customerParticulars.Surname"].Trim() == null || string.IsNullOrEmpty(form["customerParticulars.Surname"].Trim()))
            //{
            //	ModelState.AddModelError("customerParticulars.Surname", "Surname is required!");
            //}

            //if (form["customerParticulars.GivenName"].Trim() == null || string.IsNullOrEmpty(form["customerParticulars.GivenName"].Trim()))
            //{
            //	ModelState.AddModelError("customerParticulars.GivenName", "Given Name is required!");
            //}

            List<ModelStateValidation> errors = new List<ModelStateValidation>();

            if (!string.IsNullOrEmpty(customerParticulars.CustomerCode))
            {
                CustomerParticular checkUniqueCode = _customerParticularsModel.FindCustomerCode(customerParticulars.CustomerCode);

                if (checkUniqueCode != null)
                {
                    if (checkUniqueCode.ID != id)
                    {
                        ModelState.AddModelError("customerParticulars.CustomerCode", "Customer Code already existed!");
                    }
                }
            }

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                //Check Company H.O.M Contact
                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoH))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoH", "Company Contact No (Home) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoO))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoO", "Company Contact No (Office) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ContactNoM))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ContactNoM", "Company Contact No (Mobile) cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_ICPassport))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_ICPassport", "Company IC/Passport cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_JobTitle))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_JobTitle", "Company Job Title cannot be empty!");
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Company_Nationality))
                //{
                //	ModelState.AddModelError("customerParticulars.Company_Nationality", "Company Nationality cannot be empty!");
                //}

                if (form["customerParticulars.hasCustomerAccount"] != null)
                {
                    if (form["customerParticulars.hasCustomerAccount"] == "1")
                    {
                        if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        {
                            ModelState.AddModelError("customerParticulars.Company_Email", "Email is required!");
                        }
                    }
                }

                if (IsSubAccount == 0)
                {
                    if (string.IsNullOrEmpty(customerParticulars.Company_RegisteredName))
                    {
                        ModelState.AddModelError("customerParticulars.Company_RegisteredName", "Registered Name cannot be empty!");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_BusinessAddress1))
                    {
                        ModelState.AddModelError("customerParticulars.Company_BusinessAddress1", "Business Address cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_TelNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_TelNo", "Tel No. cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_FaxNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_FaxNo", "Fax No. cannot be empty.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_PostalCode))
                    {
                        ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code cannot be empty.");
                    }
                    else
                    {
                        if (customerParticulars.Company_PostalCode.Length < 5)
                        {
                            ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code must at least 5 digits.");
                        }
                        else
                        {
                            //check is digit or not
                            bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Company_PostalCode);

                            if (!checkDigit)
                            {
                                ModelState.AddModelError("customerParticulars.Company_PostalCode", "Company Postal Code only allow digits.");
                            }
                        }
                    }

                    //if (string.IsNullOrEmpty(customerParticulars.Shipping_Address1))
                    //{
                    //	ModelState.AddModelError("customerParticulars.Shipping_Address1", "Mailing Address 1 cannot be empty.");
                    //}

                    //if (string.IsNullOrEmpty(customerParticulars.Shipping_PostalCode))
                    //{
                    //	ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code cannot be empty.");
                    //}
                    //else
                    //{
                    //	if (customerParticulars.Shipping_PostalCode.Length < 5)
                    //	{
                    //		ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code must at least 5 digits.");
                    //	}
                    //	else
                    //	{
                    //		//check is digit or not
                    //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Shipping_PostalCode);

                    //		if (!checkDigit)
                    //		{
                    //			ModelState.AddModelError("customerParticulars.Shipping_PostalCode", "Mailing Postal Code only allow digits.");
                    //		}
                    //	}
                    //}

                    if (string.IsNullOrEmpty(form["Country"]))
                    {
                        ModelState.AddModelError("Country", "Please select Country.");
                    }

                    if (string.IsNullOrEmpty(form["CountryCode"]))
                    {
                        ModelState.AddModelError("CountryCode", "Please select Country Code.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_PlaceOfRegistration))
                    {
                        ModelState.AddModelError("customerParticulars.Company_PlaceOfRegistration", "Place of Registration is required.");
                    }

                    if (customerParticulars.Company_DateOfRegistration == null)
                    {
                        ModelState.AddModelError("customerParticulars.Company_DateOfRegistration", "Date of Registration is required.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_RegistrationNo))
                    {
                        ModelState.AddModelError("customerParticulars.Company_RegistrationNo", "Registration No is required.");
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntity))
                    {
                        ModelState.AddModelError("customerParticulars.Company_TypeOfEntity", "Type of Entity is required.");
                    }
                    else
                    {
                        if (customerParticulars.Company_TypeOfEntity == "Others")
                        {
                            if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntityIfOthers))
                            {
                                ModelState.AddModelError("customerParticulars.Company_TypeOfEntityIfOthers", "If Others is required.");
                            }
                        }
                    }
                }

                //Company DOB
                if (string.IsNullOrEmpty(form["dob-datepicker"]))
                {
                    ModelState.AddModelError("dob-datepicker", "Date of Birth cannot be empty.");
                }

                //if (string.IsNullOrEmpty(form["Company_service_like_to_use"]))
                //{
                //	ModelState.AddModelError("CompanyServiceLikeToUse", "This cannot be empty!");
                //}

                if (string.IsNullOrEmpty(form["Company_purpose_of_intended_transactions"]))
                {
                    ModelState.AddModelError("CompanyPurposeOfIntendedTransactions", "This cannot be empty.");
                }

                //if (string.IsNullOrEmpty(form["Company_hear_about_us"]))
                //{
                //	ModelState.AddModelError("CompanyHearAboutUs", "This cannot be empty.");
                //}

                if (string.IsNullOrEmpty(form["Company_source_of_fund"]))
                {
                    ModelState.AddModelError("CompanySourceOfFund", "Must at least have one source of fund.");
                }
                else
                {
                    string s = form["Company_source_of_fund"];

                    string[] subs = s.Split(',');

                    var arraycontainsturtles = (Array.IndexOf(subs, "Others") > -1);

                    if (arraycontainsturtles)
                    {
                        if (string.IsNullOrEmpty(form["customerSourceOfFunds.Company_SourceOfFundIfOthers"]))
                        {
                            ModelState.AddModelError("customerSourceOfFunds.Company_SourceOfFundIfOthers", "Source of fund others cannot be empty.");
                        }
                    }
                }
            }
            else
            {
                //Natural Person validation
                //if (string.IsNullOrEmpty(customerParticulars.Mailing_PostalCode))
                //{
                //	ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code cannot be empty!");
                //}
                //else
                //{
                //	if (customerParticulars.Mailing_PostalCode.Length < 5)
                //	{
                //		ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code must at least 5 digits!");
                //	}
                //	else
                //	{
                //		//check is digit or not
                //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Mailing_PostalCode);

                //		if (!checkDigit)
                //		{
                //			ModelState.AddModelError("customerParticulars.Mailing_PostalCode", "Mailing Postal Code only allow digits!");
                //		}
                //	}
                //}

                //if (string.IsNullOrEmpty(customerParticulars.Natural_PermanentPostalCode))
                //{
                //	ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code cannot be empty!");
                //}
                //else
                //{
                //	if (customerParticulars.Natural_PermanentPostalCode.Length < 5)
                //	{
                //		ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code must at least 5 digits!");
                //	}
                //	else
                //	{
                //		//check is digit or not
                //		bool checkDigit = FormValidationHelper.IntegerValidation(customerParticulars.Natural_PermanentPostalCode);

                //		if (!checkDigit)
                //		{
                //			ModelState.AddModelError("customerParticulars.Natural_PermanentPostalCode", "Permanent Postal Code only allow digits!");
                //		}
                //	}
                //}

                //if (string.IsNullOrEmpty(form["Natural_service_like_to_use"]))
                //{
                //	ModelState.AddModelError("NaturalServiceLikeToUse", "This cannot be empty!");
                //}

                if (form["customerParticulars.hasCustomerAccount"] != null)
                {
                    if (form["customerParticulars.hasCustomerAccount"] == "1")
                    {
                        if (string.IsNullOrEmpty(customerParticulars.Natural_Email))
                        {
                            ModelState.AddModelError("customerParticulars.Natural_Email", "Email is required!");
                        }
                    }
                }

                if (string.IsNullOrEmpty(form["Natural_purpose_of_intended_transactions"]))
                {
                    ModelState.AddModelError("NaturalPurposeOfIntendedTransactions", "This cannot be empty!");
                }

                //if (string.IsNullOrEmpty(form["Natural_hear_about_us"]))
                //{
                //	ModelState.AddModelError("NaturalHearAboutUs", "This cannot be empty!");
                //}

                if (string.IsNullOrEmpty(form["Natural_source_of_fund"]))
                {
                    //ModelState.AddModelError("NaturalSourceOfFund", "Must at least have one source of fund!");
                }
                else
                {
                    string s = form["Natural_source_of_fund"];

                    string[] subs = s.Split(',');

                    var arraycontainsturtles = (Array.IndexOf(subs, "Others") > -1);

                    if (arraycontainsturtles)
                    {
                        if (string.IsNullOrEmpty(form["customerSourceOfFunds.Natural_SourceOfFundIfOthers"]))
                        {
                            ModelState.AddModelError("customerSourceOfFunds.Natural_SourceOfFundIfOthers", "Source of fund others cannot be empty!");
                        }
                    }
                }
            }

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                errors = ValidateModelStateForCorporateCompany(customerParticulars, customerSourceOfFunds, customerActingAgents, customerDocumentChecklists, customerOthers, form, id);
            }
            else
            {
                errors = ValidateModelStateForNaturalPerson(customerParticulars, customerSourceOfFunds, customerActingAgents, customerDocumentChecklists, customerOthers, form, id);
            }

            if (errors.Count > 0)
            {
                foreach (ModelStateValidation error in errors)
                {
                    ModelState.AddModelError(error.Key, error.ErrorMessage);
                }
            }

            //Determine if File Uploads have changed
            CustomerParticular oldParticulars = _customerParticularsModel.GetSingle(id);
            bool changeBasisOfAuthority = false;
            bool changeAccountOpening = false;
            bool changeICWithTradingPerson = false;
            bool changeICWithDirector = false;
            bool changeBusinessAcra = false;
            bool changeICOfCustomer = false;
            bool changeBusinessNameCard = false;
            bool changeKYCForm = false;
            bool changeScreeningResultsDocument = false;

            //Agent Acting: Basis of Authority
            if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
            {
                if (oldParticulars.ActingAgents.Count > 0)
                {
                    if (oldParticulars.ActingAgents[0].BasisOfAuthority != customerActingAgents.BasisOfAuthority)
                    {
                        changeBasisOfAuthority = true;
                    }

                }
                else
                {
                    changeBasisOfAuthority = true;
                }
            }

            //Document Checklist: Account Opening Form
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Company_AccountOpeningForm != customerDocumentChecklists.Company_AccountOpeningForm)
                    {
                        changeAccountOpening = true;
                    }
                }
                else
                {
                    changeAccountOpening = true;
                }
            }

            //Document Checklist: IC With Authorized Trading Person
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Company_ICWithAuthorizedTradingPersons != customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons)
                    {
                        changeICWithTradingPerson = true;
                    }
                }
                else
                {
                    changeICWithTradingPerson = true;
                }
            }

            //Document Checklist: IC With Director
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Company_ICWithDirectors != customerDocumentChecklists.Company_ICWithDirectors)
                    {
                        changeICWithDirector = true;
                    }
                }
                else
                {
                    changeICWithDirector = true;
                }
            }

            //Document Checklist: Business Profile From Acra
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Company_BusinessProfileFromAcra != customerDocumentChecklists.Company_BusinessProfileFromAcra)
                    {
                        changeBusinessAcra = true;
                    }
                }
                else
                {
                    changeBusinessAcra = true;
                }
            }

            //Document Checklist: IC of Customer
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Natural_ICOfCustomer != customerDocumentChecklists.Natural_ICOfCustomer)
                    {
                        changeICOfCustomer = true;
                    }
                }
                else
                {
                    changeICOfCustomer = true;
                }
            }

            //Document Checklist: Business Name Card
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Natural_BusinessNameCard != customerDocumentChecklists.Natural_BusinessNameCard)
                    {
                        changeBusinessNameCard = true;
                    }
                }
                else
                {
                    changeBusinessNameCard = true;
                }
            }

            //Document Checklist: KYC Form
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
            {
                if (oldParticulars.DocumentCheckLists.Count > 0)
                {
                    if (oldParticulars.DocumentCheckLists[0].Natural_KYCForm != customerDocumentChecklists.Natural_KYCForm)
                    {
                        changeKYCForm = true;
                    }
                }
                else
                {
                    changeKYCForm = true;
                }
            }

            //Customer Other: Screening Results Document
            if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
            {
                if (oldParticulars.Others.Count > 0)
                {
                    if (oldParticulars.Others[0].ScreeningResultsDocument != customerOthers.ScreeningResultsDocument)
                    {
                        changeScreeningResultsDocument = true;
                    }
                }
                else
                {
                    changeScreeningResultsDocument = true;
                }
            }

            //Customer Bank Account Validation
            List<string> bankAccountKeys = form.AllKeys.Where(e => e.Contains("BankAccountNo_")).ToList();
            List<string> bankAccountNos = new List<string>();

            foreach (string key in bankAccountKeys)
            {
                bankAccountNos.Add(form[key]);
            }

            //Customer Sanctions and PEP Screening Report Validation
            List<string> screeningReportKeys = form.AllKeys.Where(e => e.Contains("PEPScreeningReport_Date_")).ToList();
            List<string> screeningReports = new List<string>();

            int rowCount = 1;

            foreach (string key in screeningReportKeys)
            {
                string rowId = key.Split('_')[2];

                if (string.IsNullOrEmpty(form["PEPScreeningReport_Date_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_Date_" + rowCount, "Date is required!");
                }
                else
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(form["PEPScreeningReport_Date_" + rowId]);
                    }
                    catch
                    {
                        ModelState.AddModelError("PEPScreeningReport_Date_" + rowCount, "Date is not valid!");
                    }
                }

                if (string.IsNullOrEmpty(form["PEPScreeningReport_DateOfAcra_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_DateOfAcra_" + rowCount, "Date of ACRA is required!");
                }
                else
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(form["PEPScreeningReport_DateOfAcra_" + rowId]);
                    }
                    catch
                    {
                        ModelState.AddModelError("PEPScreeningReport_DateOfAcra_" + rowCount, "Date of ACRA is not valid!");
                    }
                }

                if (string.IsNullOrEmpty(form["PEPScreeningReport_ScreenedBy_" + rowId]))
                {
                    ModelState.AddModelError("PEPScreeningReport_ScreenedBy_" + rowCount, "Screened By is required!");
                }

                screeningReports.Add(form["PEPScreeningReport_Date_" + rowId] + "|" + form["PEPScreeningReport_DateOfAcra_" + rowId] + "|" + form["PEPScreeningReport_ScreenedBy_" + rowId] + "|" + form["PEPScreeningReport_ScreeningReport1_" + rowId] + "|" + form["PEPScreeningReport_ScreeningReport2_" + rowId] + "|" + form["PEPScreeningReport_Remarks_" + rowId]);
            }

            //Activity Logs
            List<string> ActivityLogsKey = form.AllKeys.Where(e => e.Contains("CustomerActivityLogs_Key_Title")).ToList();
            List<string> ActivityLogs = new List<string>();

            int rowCount_Activity = 1;

            if (ActivityLogsKey.Count != 0)
            {
                foreach (string key in ActivityLogsKey)
                {
                    string rowId = key.Split('_')[3];

                    if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_Title_" + rowId]))
                    {
                        ModelState.AddModelError("CustomerActivityLogs_Key_Title_" + rowCount_Activity, "Title is required!");
                    }

                    if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_DateTime_" + rowId]))
                    {
                        ModelState.AddModelError("CustomerActivityLogs_Key_DateTime_" + rowCount_Activity, "Activity Log DateTime is required!");
                    }
                    else
                    {
                        try
                        {
                            DateTime date = Convert.ToDateTime(form["CustomerActivityLogs_Key_DateTime_" + rowId]);
                        }
                        catch
                        {
                            ModelState.AddModelError("CustomerActivityLogs_Key_DateTime_" + rowCount_Activity, "Activity Log DateTime is not valid!");
                        }
                    }

                    if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_Note_" + rowId]))
                    {
                        ModelState.AddModelError("CustomerActivityLogs_Key_Note_" + rowCount_Activity, "Note is required!");
                    }

                    ActivityLogs.Add(form["CustomerActivityLogs_Key_Title_" + rowId] + "|" + form["CustomerActivityLogs_Key_DateTime_" + rowId] + "|" + form["CustomerActivityLogs_Key_Note_" + rowId]);
                }
            }
            else
            {
                //ModelState.AddModelError("GeneralActivityLog", "Activity Log is required!");
            }

            if (!string.IsNullOrEmpty(form["Search_Tag_Dropdown_Form"]))
            {
                string[] splitText = form["Search_Tag_Dropdown_Form"].Split(',');
                string finalText = "";

                foreach (string _split in splitText)
                {
                    finalText = finalText + "-" + _split + "-,";
                }

                customerParticulars.SearchTags = finalText.Substring(0, finalText.Length - 1);
            }

            //Save Customer Custom Rate
            List<string> customRateKeysC = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

            foreach (string key in customRateKeysC)
            {
                string _id = key.Substring(14);

                if (string.IsNullOrEmpty(form["default-rate-" + _id]))
                {
                    CustomerCustomRate rate = new CustomerCustomRate();
                    rate.ProductId = Convert.ToInt32(form[key]);

                    if (!string.IsNullOrEmpty(_id))
                    {
                        if (!string.IsNullOrEmpty(form["CustomBuyRate_" + _id]))
                        {
                            bool checkBuy = FormValidationHelper.NonNegativeAmountValidation(form["CustomBuyRate_" + _id]);

                            if (!checkBuy)
                            {
                                ModelState.AddModelError("CustomBuyRate_" + _id, "Invalid Buy Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["CustomBuyRate_" + _id]) > 999)
                                {
                                    ModelState.AddModelError("CustomBuyRate_" + _id, "Buy rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["CustomSellRate_" + _id]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["CustomSellRate_" + _id]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("CustomSellRate_" + _id, "Invalid Sell Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["CustomSellRate_" + _id]) > 999)
                                {
                                    ModelState.AddModelError("CustomSellRate_" + _id, "Sell rate Adjustment cannot more than 999!");
                                }
                            }
                        }
                    }
                }
            }

            //Save Customer Remittance Custom Rate
            List<string> customRemittanceRateKeysC = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();

            foreach (string key in customRemittanceRateKeysC)
            {
                string rid = key.Substring(19);

                if (string.IsNullOrEmpty(form["default-fee-" + rid]))
                {
                    CustomerRemittanceProductCustomRate fee = new CustomerRemittanceProductCustomRate();
                    fee.RemittanceProductId = Convert.ToInt32(form[key]);

                    if (!string.IsNullOrEmpty(rid))
                    {
                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomBuyRate_" + rid]))
                        {
                            bool checkBuy = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomBuyRate_" + rid]);

                            if (!checkBuy)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomBuyRate_" + rid, "Invalid Pay Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomBuyRate_" + rid]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomBuyRate_" + rid, "Pay Rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomSellRate_" + rid]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomSellRate_" + rid]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomSellRate_" + rid, "Invalid Get Rate Adjustment!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomSellRate_" + rid]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomSellRate_" + rid, "Get Rate Adjustment cannot more than 999!");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(form["RemittanceProductsCustomFee_" + rid]))
                        {
                            bool checkSell = FormValidationHelper.NonNegativeAmountValidation(form["RemittanceProductsCustomFee_" + rid]);

                            if (!checkSell)
                            {
                                ModelState.AddModelError("RemittanceProductsCustomFee_" + rid, "Invalid Transaction Fee!");
                            }
                            else
                            {
                                if (Convert.ToDecimal(form["RemittanceProductsCustomFee_" + rid]) > 999)
                                {
                                    ModelState.AddModelError("RemittanceProductsCustomFee_" + rid, "Transaction Fee cannot more than 999!");
                                }
                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                customerParticulars.hasCustomerAccount = 0;
                customerParticulars.IsSubAccount = 0;
                customerParticulars.isVerify = 0;
                customerParticulars.EnableTransactionType = form["customerParticulars.EnableTransactionType"];

                //preset old record
                customerParticulars.IsDeleted = oldParticulars.IsDeleted;
                customerParticulars.CreatedOn = oldParticulars.CreatedOn;
                customerParticulars.UpdatedOn = oldParticulars.UpdatedOn;
                customerParticulars.isKYCVerify = oldParticulars.isKYCVerify;
                customerParticulars.ID = oldParticulars.ID;

                //Has Customer Account
                if (form["customerParticulars.hasCustomerAccount"] == "1")
                {
                    customerParticulars.hasCustomerAccount = 1;
                    if (!string.IsNullOrEmpty(form["Password"].Trim()))
                    {
                        customerParticulars.Password = EncryptionHelper.Encrypt(form["Password"].Trim());
                        customerParticulars.LastPasswordUpdated = DateTime.Now;
                    }
                    else
                    {
                        customerParticulars.Password = oldParticulars.Password;
                        customerParticulars.LastPasswordUpdated = oldParticulars.LastPasswordUpdated;
                    }
                }

                //Is Sub Account
                if (form["customerParticulars.IsSubAccount"] != "0")
                {
                    customerParticulars.IsSubAccount = Convert.ToInt32(form["MainAccountCustomer"]);
                }

                //Is Email Verify
                if (form["customerParticulars.isVerify"] != "0")
                {
                    customerParticulars.isVerify = 1;
                }

                //Perform Data Cleaning
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    //Empty Customer Particular for Natural Person
                    customerParticulars.Natural_Name = null;
                    customerParticulars.Natural_PermanentAddress = null;
                    customerParticulars.Natural_PermanentAddress2 = null;
                    customerParticulars.Natural_PermanentAddress3 = null;
                    customerParticulars.Natural_PermanentPostalCode = null;
                    customerParticulars.Natural_MailingAddress = null;
                    customerParticulars.Natural_MailingAddress2 = null;
                    customerParticulars.Natural_MailingAddress3 = null;
                    customerParticulars.Mailing_PostalCode = null;
                    customerParticulars.Natural_Nationality = null;
                    customerParticulars.Natural_ICPassportNo = null;
                    customerParticulars.Natural_DOB = null;
                    customerParticulars.Natural_ContactNoH = null;
                    customerParticulars.Natural_ContactNoO = null;
                    customerParticulars.Natural_ContactNoM = null;
                    customerParticulars.Natural_Email = null;
                    customerParticulars.Natural_EmploymentType = null;
                    customerParticulars.Natural_EmployedEmployerName = null;
                    customerParticulars.Natural_EmployedJobTitle = null;
                    customerParticulars.Natural_EmployedRegisteredAddress = null;
                    customerParticulars.Natural_EmployedRegisteredAddress2 = null;
                    customerParticulars.Natural_EmployedRegisteredAddress3 = null;
                    customerParticulars.Natural_SelfEmployedBusinessName = null;
                    customerParticulars.Natural_SelfEmployedRegistrationNo = null;
                    customerParticulars.Natural_SelfEmployedBusinessAddress = null;
                    customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace = null;

                    //Empty Customer Source of Fund for Natural Person
                    customerSourceOfFunds.Natural_SourceOfFund = null;
                    customerSourceOfFunds.Natural_SourceOfFundIfOthers = null;
                    customerSourceOfFunds.Natural_AnnualIncome = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 = null;
                    customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 = null;
                    customerSourceOfFunds.Natural_ServiceLikeToUse = null;
                    customerSourceOfFunds.Natural_PurposeOfIntendedTransactions = null;
                    customerSourceOfFunds.Natural_HearAboutUs = null;

                    //Empty Customer Acting Agent for Natural Person
                    customerActingAgents.Natural_Name = null;
                    customerActingAgents.Natural_PermanentAddress = null;
                    customerActingAgents.Natural_Nationality = null;
                    customerActingAgents.Natural_ICPassportNo = null;
                    customerActingAgents.Natural_DOB = null;

                    //Empty Customer Acting Agent if No
                    if (customerActingAgents.ActingAgent == "No")
                    {
                        customerActingAgents.Company_CustomerType = null;
                        customerActingAgents.Company_Address = null;
                        customerActingAgents.Company_PlaceOfRegistration = null;
                        customerActingAgents.Company_RegistrationNo = null;
                        customerActingAgents.Company_DateOfRegistration = null;
                        customerActingAgents.Relationship = null;
                        customerActingAgents.BasisOfAuthority = null;
                    }

                    //Empty Customer Document Checklist for Natural Person
                    customerDocumentChecklists.Natural_ICOfCustomer = null;
                    customerDocumentChecklists.Natural_BusinessNameCard = null;
                    customerDocumentChecklists.Natural_KYCForm = null;
                    customerDocumentChecklists.Natural_SelfiePhotoID = null;

                    //Assign Checkbox value
                    customerSourceOfFunds.Company_SourceOfFund = form["Company_source_of_fund"];
                    customerSourceOfFunds.Company_ServiceLikeToUse = form["Company_service_like_to_use"];
                    customerSourceOfFunds.Company_PurposeOfIntendedTransactions = form["Company_purpose_of_intended_transactions"];
                    customerSourceOfFunds.Company_HearAboutUs = form["Company_hear_about_us"];
                    customerParticulars.Company_Country = Convert.ToInt32(form["Country"]);
                    customerParticulars.Company_CountryCode = Convert.ToInt32(form["CountryCode"]);
                    customerParticulars.DOB = Convert.ToDateTime(form["dob-datepicker"]);
                    customerParticulars.Customer_Title = form["Customer_Title"];

                    if (customerParticulars.IsSubAccount != 0)
                    {
                        //this is assign main account company value into sub account fields.
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var GetMainAccountDetails = context.CustomerParticulars.Where(e => e.ID == customerParticulars.IsSubAccount).FirstOrDefault();
                            //var GetOldDataAccountDetails = context.CustomerParticulars.Where(e => e.ID == oldParticulars.IsSubAccount).FirstOrDefault();

                            customerParticulars.Company_RegisteredName = GetMainAccountDetails.Company_RegisteredName;
                            customerParticulars.Company_BusinessAddress1 = GetMainAccountDetails.Company_BusinessAddress1;
                            customerParticulars.Company_BusinessAddress2 = GetMainAccountDetails.Company_BusinessAddress2;
                            customerParticulars.Company_BusinessAddress3 = GetMainAccountDetails.Company_BusinessAddress3;
                            customerParticulars.Company_PostalCode = GetMainAccountDetails.Company_PostalCode;

                            customerParticulars.Shipping_Address1 = GetMainAccountDetails.Shipping_Address1;
                            customerParticulars.Shipping_Address2 = GetMainAccountDetails.Shipping_Address2;
                            customerParticulars.Shipping_Address3 = GetMainAccountDetails.Shipping_Address3;
                            customerParticulars.Shipping_PostalCode = GetMainAccountDetails.Shipping_PostalCode;

                            customerParticulars.Company_Country = GetMainAccountDetails.Company_Country;
                            customerParticulars.Company_CountryCode = GetMainAccountDetails.Company_CountryCode;

                            customerParticulars.Company_TelNo = GetMainAccountDetails.Company_TelNo;
                            customerParticulars.Company_FaxNo = GetMainAccountDetails.Company_FaxNo;

                            customerParticulars.Company_PlaceOfRegistration = GetMainAccountDetails.Company_PlaceOfRegistration;
                            customerParticulars.Company_DateOfRegistration = GetMainAccountDetails.Company_DateOfRegistration;

                            customerParticulars.Company_RegistrationNo = GetMainAccountDetails.Company_RegistrationNo;
                            customerParticulars.Company_TypeOfEntity = GetMainAccountDetails.Company_TypeOfEntity;
                            customerParticulars.Company_TypeOfEntityIfOthers = GetMainAccountDetails.Company_TypeOfEntityIfOthers;

                            //Update Old CustomerParticulars company record too.
                            //if (GetOldDataAccountDetails != null)
                            //{
                            //	oldParticulars.Company_BusinessAddress1 = GetOldDataAccountDetails.Company_BusinessAddress1;
                            //	oldParticulars.Company_BusinessAddress2 = GetOldDataAccountDetails.Company_BusinessAddress2;
                            //	oldParticulars.Company_BusinessAddress3 = GetOldDataAccountDetails.Company_BusinessAddress3;
                            //	oldParticulars.Company_PostalCode = GetOldDataAccountDetails.Company_PostalCode;

                            //	oldParticulars.Shipping_Address1 = GetOldDataAccountDetails.Shipping_Address1;
                            //	oldParticulars.Shipping_Address2 = GetOldDataAccountDetails.Shipping_Address2;
                            //	oldParticulars.Shipping_Address3 = GetOldDataAccountDetails.Shipping_Address3;
                            //	oldParticulars.Shipping_PostalCode = GetOldDataAccountDetails.Shipping_PostalCode;

                            //	oldParticulars.Company_Country = GetOldDataAccountDetails.Company_Country;
                            //	oldParticulars.Company_CountryCode = GetOldDataAccountDetails.Company_CountryCode;

                            //	oldParticulars.Company_TelNo = GetOldDataAccountDetails.Company_TelNo;
                            //	oldParticulars.Company_FaxNo = GetOldDataAccountDetails.Company_FaxNo;

                            //	oldParticulars.Company_PlaceOfRegistration = GetOldDataAccountDetails.Company_PlaceOfRegistration;
                            //	oldParticulars.Company_DateOfRegistration = GetOldDataAccountDetails.Company_DateOfRegistration;

                            //	oldParticulars.Company_RegistrationNo = GetOldDataAccountDetails.Company_RegistrationNo;
                            //	oldParticulars.Company_TypeOfEntity = GetOldDataAccountDetails.Company_TypeOfEntity;
                            //	oldParticulars.Company_TypeOfEntityIfOthers = GetOldDataAccountDetails.Company_TypeOfEntityIfOthers;
                            //}
                        }
                    }
                }
                else
                {
                    customerParticulars.IsSubAccount = 0;

                    //Empty Customer Particular for Company
                    customerParticulars.Company_RegisteredName = null;
                    //customerParticulars.Company_RegisteredAddress = null;
                    customerParticulars.Company_BusinessAddress1 = null;
                    customerParticulars.Company_BusinessAddress2 = null;
                    customerParticulars.Company_BusinessAddress3 = null;
                    customerParticulars.Company_PostalCode = null;
                    customerParticulars.Company_ContactName = null;
                    customerParticulars.Company_Country = 0;
                    customerParticulars.Company_CountryCode = 0;
                    customerParticulars.Company_TelNo = null;
                    customerParticulars.Company_FaxNo = null;
                    customerParticulars.Company_Email = null;
                    customerParticulars.Company_PlaceOfRegistration = null;
                    customerParticulars.Company_DateOfRegistration = null;
                    customerParticulars.Company_RegistrationNo = null;
                    customerParticulars.Company_TypeOfEntity = null;
                    customerParticulars.Company_TypeOfEntityIfOthers = null;
                    customerParticulars.Company_PurposeAndIntended = null;
                    customerParticulars.Shipping_Address1 = null;
                    customerParticulars.Shipping_Address2 = null;
                    customerParticulars.Shipping_Address3 = null;
                    customerParticulars.Shipping_PostalCode = null;
                    customerParticulars.Company_Nationality = null;
                    customerParticulars.Company_JobTitle = null;
                    customerParticulars.Company_ICPassport = null;
                    customerParticulars.Company_ContactNoH = null;
                    customerParticulars.Company_ContactNoO = null;
                    customerParticulars.Company_ContactNoM = null;

                    //Empty Customer Particular Self Employed if Employed
                    if (customerParticulars.Natural_EmploymentType == "Employed")
                    {
                        customerParticulars.Natural_SelfEmployedBusinessName = null;
                        customerParticulars.Natural_SelfEmployedRegistrationNo = null;
                        customerParticulars.Natural_SelfEmployedBusinessAddress = null;
                        customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace = null;
                    }
                    //Empty Customer Particular Employed if Self Employed
                    else
                    {
                        customerParticulars.Natural_EmployedEmployerName = null;
                        customerParticulars.Natural_EmployedJobTitle = null;
                        customerParticulars.Natural_EmployedRegisteredAddress = null;
                        customerParticulars.Natural_EmployedRegisteredAddress2 = null;
                        customerParticulars.Natural_EmployedRegisteredAddress3 = null;
                    }

                    //Empty Customer Source of Fund for Company
                    customerSourceOfFunds.Company_SourceOfFund = null;
                    customerSourceOfFunds.Company_SourceOfFundIfOthers = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 = null;
                    customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 = null;
                    customerSourceOfFunds.Company_ServiceLikeToUse = null;
                    customerSourceOfFunds.Company_PurposeOfIntendedTransactions = null;
                    customerSourceOfFunds.Company_HearAboutUs = null;

                    //Empty Customer Acting Agent for Company
                    customerActingAgents.Company_CustomerType = null;
                    customerActingAgents.Company_Address = null;
                    customerActingAgents.Company_PlaceOfRegistration = null;
                    customerActingAgents.Company_RegistrationNo = null;
                    customerActingAgents.Company_DateOfRegistration = null;

                    //Empty Customer Acting Agent if No
                    if (customerActingAgents.ActingAgent == "No")
                    {
                        customerActingAgents.Natural_Name = null;
                        customerActingAgents.Natural_PermanentAddress = null;
                        customerActingAgents.Natural_Nationality = null;
                        customerActingAgents.Natural_ICPassportNo = null;
                        customerActingAgents.Natural_DOB = null;
                        customerActingAgents.Relationship = null;
                        customerActingAgents.BasisOfAuthority = null;
                    }

                    //Empty Customer Document Checklist for Company
                    customerDocumentChecklists.Company_AccountOpeningForm = null;
                    customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons = null;
                    customerDocumentChecklists.Company_ICWithDirectors = null;
                    customerDocumentChecklists.Company_BusinessProfileFromAcra = null;
                    customerDocumentChecklists.Company_SelfiePassporWorkingPass = null;
                    customerDocumentChecklists.Company_SelfiePhotoID = null;

                    //Assign Checkbox value
                    customerSourceOfFunds.Natural_SourceOfFund = form["Natural_source_of_fund"];
                    customerSourceOfFunds.Natural_ServiceLikeToUse = form["Natural_service_like_to_use"];
                    customerSourceOfFunds.Natural_PurposeOfIntendedTransactions = form["Natural_purpose_of_intended_transactions"];
                    customerSourceOfFunds.Natural_HearAboutUs = form["Natural_hear_about_us"];
                    customerParticulars.Customer_Title = form["Customer_Title"];
                }

                //1. Save new update into Temp Table ONLY
                //2. Update the main table status to Pending Approval.


                //before update, get the old data value and save into TempTable.
                //1. Check in TempTable have existing record or not, if have, update only.
                //2. If dont have, add new TempTable record.
                var TempCustomerParticular = _TempCustomerParticularsModel.GetSingle2(id);

                if (TempCustomerParticular != null)
                {
                    //Save Update Record into Temp if temp record found
                    bool updateTempData = _TempCustomerParticularsModel.Update(TempCustomerParticular.ID, customerParticulars);
                }
                else
                {
                    bool addTempData = _TempCustomerParticularsModel.Add(customerParticulars);
                }

                customerParticulars.lastTempPageUpdate = 6;
                bool result = _customerParticularsModel.UpdateStatus(id, 6);

                if (result)
                {
                    AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerParticulars", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Particular");

                    //check source of fund models (Temp)
                    var checkTempSourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(oldParticulars.ID);
                    bool sourceOfFundUpdates = false;

                    if (checkTempSourceOfFund != null)
                    {
                        //update
                        bool updateTempSourceOfFund = _TempCustomerSourceOfFundsModel.Update(checkTempSourceOfFund.ID, customerSourceOfFunds);
                        sourceOfFundUpdates = updateTempSourceOfFund;
                    }
                    else
                    {
                        //add
                        customerSourceOfFunds.CustomerParticularId = oldParticulars.ID;
                        bool addTempSourceOfFund = _TempCustomerSourceOfFundsModel.Add(customerSourceOfFunds);
                        sourceOfFundUpdates = addTempSourceOfFund;
                    }

                    //bool result_SOF = _customerSourceOfFundsModel.Update(oldParticulars.SourceOfFunds[0].ID, customerSourceOfFunds);

                    if (sourceOfFundUpdates)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerSourceOfFunds", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Source of Funds");
                    }

                    //Save Customer Source of Funds
                    //if (oldParticulars.SourceOfFunds.Count > 0)
                    //{
                    //	//check source of fund models (Temp)
                    //	var checkTempSourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(oldParticulars.ID);
                    //	bool sourceOfFundUpdates = false;

                    //	if (checkTempSourceOfFund != null)
                    //	{
                    //		//update
                    //		bool updateTempSourceOfFund = _TempCustomerSourceOfFundsModel.Update(checkTempSourceOfFund.ID, customerSourceOfFunds);
                    //		sourceOfFundUpdates = updateTempSourceOfFund;
                    //	}
                    //	else
                    //	{
                    //		//add
                    //		bool addTempSourceOfFund = _TempCustomerSourceOfFundsModel.Add(customerSourceOfFunds);
                    //		sourceOfFundUpdates = addTempSourceOfFund;
                    //	}

                    //	//bool result_SOF = _customerSourceOfFundsModel.Update(oldParticulars.SourceOfFunds[0].ID, customerSourceOfFunds);

                    //	if (sourceOfFundUpdates)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerSourceOfFunds", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Source of Funds");
                    //	}
                    //}
                    //else
                    //{
                    //	customerSourceOfFunds.CustomerParticularId = id;

                    //	//check source of fund models (Temp)
                    //	var checkTempSourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(oldParticulars.ID);
                    //	bool sourceOfFundUpdates = false;

                    //	if (checkTempSourceOfFund != null)
                    //	{
                    //		//update
                    //		bool updateTempSourceOfFund = _TempCustomerSourceOfFundsModel.Update(checkTempSourceOfFund.ID, customerSourceOfFunds);
                    //		sourceOfFundUpdates = updateTempSourceOfFund;
                    //	}
                    //	else
                    //	{
                    //		//add empty source of fund if is empty.
                    //		bool addTempSourceOfFund = _TempCustomerSourceOfFundsModel.Add(new CustomerSourceOfFund());
                    //		sourceOfFundUpdates = addTempSourceOfFund;
                    //	}

                    //	//bool result_SOF_add = _customerSourceOfFundsModel.Add(customerSourceOfFunds);

                    //	if (sourceOfFundUpdates)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerSourceOfFunds", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Added Customer Source of Funds");
                    //	}
                    //}

                    //check source of fund models (Temp)
                    var checkActingAgent = _TempCustomerActingAgentsModel.GetActingAgent(oldParticulars.ID);
                    bool ActingAgentUpdates = false;

                    if (checkActingAgent != null)
                    {
                        //update
                        bool updateActingAgent = _TempCustomerActingAgentsModel.Update(checkActingAgent.ID, customerActingAgents);
                        ActingAgentUpdates = updateActingAgent;
                    }
                    else
                    {
                        //add
                        customerActingAgents.CustomerParticularId = oldParticulars.ID;
                        bool addActingAgent = _TempCustomerActingAgentsModel.AddSingle(customerActingAgents);
                        ActingAgentUpdates = addActingAgent;
                    }

                    //Save Customer Acting Agent
                    //bool result_Agent = _customerActingAgentsModel.Update(oldParticulars.ActingAgents[0].ID, customerActingAgents);

                    if (ActingAgentUpdates)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActingAgents", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Acting Agent");

                        //Move Basis of Authority Files
                        if (customerActingAgents.ActingAgent == "Yes")
                        {
                            if (changeBasisOfAuthority)
                            {
                                string[] basisOfAuthorityFiles = customerActingAgents.BasisOfAuthority.Split(',');

                                foreach (string file in basisOfAuthorityFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        if (!System.IO.File.Exists(destinationFile))
                                        {
                                            System.IO.File.Move(sourceFile, destinationFile);
                                        }
                                    }
                                    else
                                    {
                                        string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile2))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile2, destinationFile);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //if (oldParticulars.ActingAgents.Count > 0)
                    //{
                    //	//check source of fund models (Temp)
                    //	var checkActingAgent = _TempCustomerActingAgentsModel.GetActingAgent(oldParticulars.ID);
                    //	bool ActingAgentUpdates = false;

                    //	if (checkActingAgent != null)
                    //	{
                    //		//update
                    //		bool updateActingAgent = _TempCustomerActingAgentsModel.Update(checkActingAgent.ID, customerActingAgents);
                    //		ActingAgentUpdates = updateActingAgent;
                    //	}
                    //	else
                    //	{
                    //		//add
                    //		bool addActingAgent = _TempCustomerActingAgentsModel.AddSingle(customerActingAgents);
                    //		ActingAgentUpdates = addActingAgent;
                    //	}

                    //	//Save Customer Acting Agent
                    //	//bool result_Agent = _customerActingAgentsModel.Update(oldParticulars.ActingAgents[0].ID, customerActingAgents);

                    //	if (ActingAgentUpdates)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActingAgents", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Acting Agent");

                    //		//Move Basis of Authority Files
                    //		if (customerActingAgents.ActingAgent == "Yes")
                    //		{
                    //			if (changeBasisOfAuthority)
                    //			{
                    //				string[] basisOfAuthorityFiles = customerActingAgents.BasisOfAuthority.Split(',');

                    //				foreach (string file in basisOfAuthorityFiles)
                    //				{
                    //					string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //					string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                    //					if (System.IO.File.Exists(sourceFile))
                    //					{
                    //						if (!System.IO.File.Exists(destinationFile))
                    //						{
                    //							System.IO.File.Move(sourceFile, destinationFile);
                    //						}
                    //					}
                    //					else
                    //					{
                    //						string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile2))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile2, destinationFile);
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //	}
                    //}
                    //else
                    //{
                    //	//Save Customer Acting Agent
                    //	customerActingAgents.CustomerParticularId = id;

                    //	//check source of fund models (Temp)
                    //	var checkActingAgent = _TempCustomerActingAgentsModel.GetActingAgent(oldParticulars.ID);

                    //	if (checkActingAgent != null)
                    //	{
                    //		//update
                    //		//delete actingagent because previously dont have acting agent data.
                    //		bool deleteActingAgent = _TempCustomerActingAgentsModel.Delete(checkActingAgent.ID);
                    //		//bool updateActingAgent = _TempCustomerActingAgentsModel.Update(checkActingAgent.ID, oldParticulars.ActingAgents[0]);
                    //	}
                    //	else
                    //	{
                    //		//add
                    //		//bool addActingAgent = _TempCustomerActingAgentsModel.Add(new CustomerActingAgent());
                    //	}

                    //	bool result_Agent = _customerActingAgentsModel.Add(customerActingAgents);

                    //	if (result_Agent)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActingAgents", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Added Customer Acting Agent");

                    //		//Move Basis of Authority Files
                    //		if (customerActingAgents.ActingAgent == "Yes")
                    //		{
                    //			if (changeBasisOfAuthority)
                    //			{
                    //				string[] basisOfAuthorityFiles = customerActingAgents.BasisOfAuthority.Split(',');

                    //				foreach (string file in basisOfAuthorityFiles)
                    //				{
                    //					string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //					string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                    //					if (System.IO.File.Exists(sourceFile))
                    //					{
                    //						if (!System.IO.File.Exists(destinationFile))
                    //						{
                    //							System.IO.File.Move(sourceFile, destinationFile);
                    //						}
                    //					}
                    //					else
                    //					{
                    //						string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //						if (System.IO.File.Exists(sourceFile2))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile2, destinationFile);
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //	}
                    //}

                    //Save Customer Appointment of Staff
                    //Delete Old Data
                    //List<CustomerAppointmentOfStaff> oldAppointments = oldParticulars.AppointmentOfStaffs;

                    //remove tempdata and add new
                    bool RemoveTempCustomerAppointmentOfStaff = _TempCustomerAppointmentOfStaffsModel.DeleteAll(oldParticulars.ID);

                    //if (oldParticulars.AppointmentOfStaffs.Count > 0)
                    //{
                    //	//add olddata to temp
                    //	bool AddOldTempCustomerAppointmentOfStaff = _TempCustomerAppointmentOfStaffsModel.Add(oldParticulars.AppointmentOfStaffs);
                    //}

                    //bool del_appointment = false;

                    //foreach (CustomerAppointmentOfStaff oldAppointment in oldAppointments)
                    //{
                    //	bool del_OldAppointment = _customerAppointmentOfStaffsModel.Delete(oldAppointment.ID);

                    //	if (del_OldAppointment)
                    //	{
                    //		if (!del_appointment)
                    //		{
                    //			del_appointment = true;
                    //		}
                    //	}
                    //}

                    //if (del_appointment)
                    //{
                    //	AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerAppointmentOfStaffs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Appointment of Staffs");
                    //}

                    //Save into Temp Table (Because is new update record)
                    if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    {
                        List<string> appointmentKeys = form.AllKeys.Where(e => e.Contains("Personnel_FullName_")).ToList();

                        bool res_appointment = false;

                        foreach (string key in appointmentKeys)
                        {
                            string _id = key.Substring(19);

                            Temp_CustomerAppointmentOfStaffs appointment = new Temp_CustomerAppointmentOfStaffs();
                            appointment.CustomerParticularId = oldParticulars.ID;
                            appointment.FullName = form[key].ToString();
                            appointment.ICPassportNo = form["Personnel_ICPassport_" + _id].ToString();
                            appointment.Nationality = form["Personnel_Nationality_" + _id].ToString();
                            appointment.JobTitle = form["Personnel_JobTitle_" + _id].ToString();
                            //appointment.SpecimenSignature = form["Personnel_SpecimenSignature_" + _id].ToString();

                            bool result_appointment = _TempCustomerAppointmentOfStaffsModel.AddSingle(appointment);

                            if (result_appointment)
                            {
                                if (!res_appointment)
                                {
                                    res_appointment = true;
                                }

                                //Move Specimen Signature Files
                                //if (string.IsNullOrEmpty(form["Personnel_SpecimenSignature_Old_" + _id]))
                                //{
                                //    string[] specimenSignatureFiles = appointment.SpecimenSignature.Split(',');

                                //    foreach (string file in specimenSignatureFiles)
                                //    {
                                //        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                //        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SpecimenSignatureFolder"].ToString()), file);

                                //        System.IO.File.Move(sourceFile, destinationFile);
                                //    }
                                //}
                            }
                        }

                        if (res_appointment)
                        {
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerAppointmentOfStaffs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Appointment of Staffs");
                        }
                    }

                    //Save New Data
                    //if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    //{
                    //	List<string> appointmentKeys = form.AllKeys.Where(e => e.Contains("Personnel_FullName_")).ToList();

                    //	bool res_appointment = false;

                    //	foreach (string key in appointmentKeys)
                    //	{
                    //		string _id = key.Substring(19);

                    //		CustomerAppointmentOfStaff appointment = new CustomerAppointmentOfStaff();
                    //		appointment.CustomerParticularId = id;
                    //		appointment.FullName = form[key].ToString();
                    //		appointment.ICPassportNo = form["Personnel_ICPassport_" + _id].ToString();
                    //		appointment.Nationality = form["Personnel_Nationality_" + _id].ToString();
                    //		appointment.JobTitle = form["Personnel_JobTitle_" + _id].ToString();
                    //		//appointment.SpecimenSignature = form["Personnel_SpecimenSignature_" + _id].ToString();

                    //		bool result_appointment = _customerAppointmentOfStaffsModel.Add(appointment);

                    //		if (result_appointment)
                    //		{
                    //			if (!res_appointment)
                    //			{
                    //				res_appointment = true;
                    //			}

                    //			//Move Specimen Signature Files
                    //			//if (string.IsNullOrEmpty(form["Personnel_SpecimenSignature_Old_" + _id]))
                    //			//{
                    //			//    string[] specimenSignatureFiles = appointment.SpecimenSignature.Split(',');

                    //			//    foreach (string file in specimenSignatureFiles)
                    //			//    {
                    //			//        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //			//        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SpecimenSignatureFolder"].ToString()), file);

                    //			//        System.IO.File.Move(sourceFile, destinationFile);
                    //			//    }
                    //			//}
                    //		}
                    //	}

                    //	if (res_appointment)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerAppointmentOfStaffs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Appointment of Staffs");
                    //	}
                    //}

                    //Delete all the Temp Document CheckList First
                    //Delete Old Data
                    //bool RemoveTempCustomerDocumentCheckList = _TempCustomerDocumentsCheckListModel.DeleteAll(oldParticulars.ID);

                    //Save Customer Document Checklist
                    //check customer document check list models (Temp)
                    var checkDocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(oldParticulars.ID);
                    var checkDocumentCheckListUpdates = false;
                    if (checkDocumentCheckList != null)
                    {
                        //update
                        bool updateDocumentCheckList = _TempCustomerDocumentsCheckListModel.Update(checkDocumentCheckList.ID, customerDocumentChecklists);
                        checkDocumentCheckListUpdates = updateDocumentCheckList;
                    }
                    else
                    {
                        //add
                        customerDocumentChecklists.CustomerParticularId = oldParticulars.ID;
                        bool addDocumentCheckList = _TempCustomerDocumentsCheckListModel.AddSingle(customerDocumentChecklists);
                        checkDocumentCheckListUpdates = addDocumentCheckList;
                    }

                    //bool result_DCL = _customerDocumentCheckListsModel.Update(oldParticulars.DocumentCheckLists[0].ID, customerDocumentChecklists);

                    if (checkDocumentCheckListUpdates)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerDocumentChecklists", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Document Checklist");

                        //Move Document Files
                        if (customerParticulars.CustomerType == "Corporate & Trading Company")
                        {

                            //Company Selfie Passport
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                            {
                                string[] selfieWorkingPassFiles = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');

                                foreach (string file in selfieWorkingPassFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        if (!System.IO.File.Exists(destinationFile))
                                        {
                                            System.IO.File.Move(sourceFile, destinationFile);
                                        }
                                    }
                                    else
                                    {
                                        string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                        if (System.IO.File.Exists(sourceFile2))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile2, destinationFile);
                                            }
                                        }
                                    }
                                }
                            }

                            //Company Selfie Photo
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                            {
                                string[] selfiePhotoFiles = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');

                                foreach (string file in selfiePhotoFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        if (!System.IO.File.Exists(destinationFile))
                                        {
                                            System.IO.File.Move(sourceFile, destinationFile);
                                        }
                                    }
                                    else
                                    {
                                        string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                        if (System.IO.File.Exists(sourceFile2))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile2, destinationFile);
                                            }
                                        }
                                    }
                                }
                            }

                            //Account Opening Files
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                            {
                                if (changeAccountOpening)
                                {
                                    string[] accountOpeningFiles = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');

                                    foreach (string file in accountOpeningFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //IC With Authorized Trading Person
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                            {
                                if (changeICWithTradingPerson)
                                {
                                    string[] tradingPersonFiles = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');

                                    foreach (string file in tradingPersonFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //IC With Director
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                            {
                                if (changeICWithDirector)
                                {
                                    string[] directorFiles = customerDocumentChecklists.Company_ICWithDirectors.Split(',');

                                    foreach (string file in directorFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Business Profile from Acra
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                            {
                                if (changeBusinessAcra)
                                {
                                    string[] businessAcraFiles = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');

                                    foreach (string file in businessAcraFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Natural Selfie Photo
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                            {
                                string[] NaturalSelfiePhotoFiles = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');

                                foreach (string file in NaturalSelfiePhotoFiles)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        if (!System.IO.File.Exists(destinationFile))
                                        {
                                            System.IO.File.Move(sourceFile, destinationFile);
                                        }
                                    }
                                    else
                                    {
                                        string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                        if (System.IO.File.Exists(sourceFile2))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile2, destinationFile);
                                            }
                                        }
                                    }
                                }
                            }

                            //IC of Customer
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                            {
                                if (changeICOfCustomer)
                                {
                                    string[] icOfCustomerFiles = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');

                                    foreach (string file in icOfCustomerFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Business Name Card
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                            {
                                if (changeBusinessNameCard)
                                {
                                    string[] businessNameCardFiles = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');

                                    foreach (string file in businessNameCardFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //KYC Form
                            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                            {
                                if (changeKYCForm)
                                {
                                    string[] kycFormFiles = customerDocumentChecklists.Natural_KYCForm.Split(',');

                                    foreach (string file in kycFormFiles)
                                    {
                                        string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                        string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                                        if (System.IO.File.Exists(sourceFile))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile, destinationFile);
                                            }
                                        }
                                        else
                                        {
                                            string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                            if (System.IO.File.Exists(sourceFile2))
                                            {
                                                if (!System.IO.File.Exists(destinationFile))
                                                {
                                                    System.IO.File.Move(sourceFile2, destinationFile);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //if (oldParticulars.DocumentCheckLists.Count > 0)
                    //{
                    //	//check customer document check list models (Temp)
                    //	var checkDocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(oldParticulars.DocumentCheckLists[0].ID);

                    //	if (checkDocumentCheckList != null)
                    //	{
                    //		//update
                    //		bool updateDocumentCheckList = _TempCustomerDocumentsCheckListModel.Update(checkDocumentCheckList.ID, oldParticulars.DocumentCheckLists[0]);
                    //	}
                    //	else
                    //	{
                    //		//add
                    //		bool addDocumentCheckList = _TempCustomerDocumentsCheckListModel.Add(oldParticulars.DocumentCheckLists);
                    //	}

                    //	bool result_DCL = _customerDocumentCheckListsModel.Update(oldParticulars.DocumentCheckLists[0].ID, customerDocumentChecklists);

                    //	if (result_DCL)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerDocumentChecklists", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Document Checklist");

                    //		//Move Document Files
                    //		if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    //		{

                    //			//Company Selfie Passport
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
                    //			{
                    //				string[] selfieWorkingPassFiles = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');

                    //				foreach (string file in selfieWorkingPassFiles)
                    //				{
                    //					string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //					string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                    //					if (System.IO.File.Exists(sourceFile))
                    //					{
                    //						if (!System.IO.File.Exists(destinationFile))
                    //						{
                    //							System.IO.File.Move(sourceFile, destinationFile);
                    //						}
                    //					}
                    //					else
                    //					{
                    //						string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //						if (System.IO.File.Exists(sourceFile2))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile2, destinationFile);
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Company Selfie Photo
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
                    //			{
                    //				string[] selfiePhotoFiles = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');

                    //				foreach (string file in selfiePhotoFiles)
                    //				{
                    //					string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //					string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                    //					if (System.IO.File.Exists(sourceFile))
                    //					{
                    //						if (!System.IO.File.Exists(destinationFile))
                    //						{
                    //							System.IO.File.Move(sourceFile, destinationFile);
                    //						}
                    //					}
                    //					else
                    //					{
                    //						string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //						if (System.IO.File.Exists(sourceFile2))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile2, destinationFile);
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Account Opening Files
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                    //			{
                    //				if (changeAccountOpening)
                    //				{
                    //					string[] accountOpeningFiles = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');

                    //					foreach (string file in accountOpeningFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//IC With Authorized Trading Person
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                    //			{
                    //				if (changeICWithTradingPerson)
                    //				{
                    //					string[] tradingPersonFiles = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');

                    //					foreach (string file in tradingPersonFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//IC With Director
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                    //			{
                    //				if (changeICWithDirector)
                    //				{
                    //					string[] directorFiles = customerDocumentChecklists.Company_ICWithDirectors.Split(',');

                    //					foreach (string file in directorFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Business Profile from Acra
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                    //			{
                    //				if (changeBusinessAcra)
                    //				{
                    //					string[] businessAcraFiles = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');

                    //					foreach (string file in businessAcraFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //		else
                    //		{
                    //			//Natural Selfie Photo
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
                    //			{
                    //				string[] NaturalSelfiePhotoFiles = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');

                    //				foreach (string file in NaturalSelfiePhotoFiles)
                    //				{
                    //					string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //					string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                    //					if (System.IO.File.Exists(sourceFile))
                    //					{
                    //						if (!System.IO.File.Exists(destinationFile))
                    //						{
                    //							System.IO.File.Move(sourceFile, destinationFile);
                    //						}
                    //					}
                    //					else
                    //					{
                    //						string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //						if (System.IO.File.Exists(sourceFile2))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile2, destinationFile);
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//IC of Customer
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                    //			{
                    //				if (changeICOfCustomer)
                    //				{
                    //					string[] icOfCustomerFiles = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');

                    //					foreach (string file in icOfCustomerFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Business Name Card
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                    //			{
                    //				if (changeBusinessNameCard)
                    //				{
                    //					string[] businessNameCardFiles = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');

                    //					foreach (string file in businessNameCardFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//KYC Form
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                    //			{
                    //				if (changeKYCForm)
                    //				{
                    //					string[] kycFormFiles = customerDocumentChecklists.Natural_KYCForm.Split(',');

                    //					foreach (string file in kycFormFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //	}
                    //}
                    //else
                    //{
                    //	customerDocumentChecklists.CustomerParticularId = id;

                    //	//check customer document check list models (Temp)
                    //	var checkDocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(oldParticulars.ID);

                    //	if (checkDocumentCheckList != null)
                    //	{
                    //		//update
                    //		//delete
                    //		bool deleteDocumentCheckList = _TempCustomerDocumentsCheckListModel.Delete(checkDocumentCheckList.ID);
                    //		//bool updateDocumentCheckList = _TempCustomerDocumentsCheckListModel.Update(checkDocumentCheckList.ID, oldParticulars.DocumentCheckLists[0]);
                    //	}
                    //	else
                    //	{
                    //		//add
                    //		//bool addDocumentCheckList = _TempCustomerDocumentsCheckListModel.Add(new CustomerDocumentCheckList());
                    //	}

                    //	bool result_DCL = _customerDocumentCheckListsModel.Add(customerDocumentChecklists);

                    //	if (result_DCL)
                    //	{
                    //		AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerDocumentChecklists", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Added Customer Document Checklist");

                    //		//Move Document Files
                    //		if (customerParticulars.CustomerType == "Corporate & Trading Company")
                    //		{
                    //			//Account Opening Files
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
                    //			{
                    //				if (changeAccountOpening)
                    //				{
                    //					string[] accountOpeningFiles = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');

                    //					foreach (string file in accountOpeningFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//IC With Authorized Trading Person
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
                    //			{
                    //				if (changeICWithTradingPerson)
                    //				{
                    //					string[] tradingPersonFiles = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');

                    //					foreach (string file in tradingPersonFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//IC With Director
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
                    //			{
                    //				if (changeICWithDirector)
                    //				{
                    //					string[] directorFiles = customerDocumentChecklists.Company_ICWithDirectors.Split(',');

                    //					foreach (string file in directorFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Business Profile from Acra
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
                    //			{
                    //				if (changeBusinessAcra)
                    //				{
                    //					string[] businessAcraFiles = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');

                    //					foreach (string file in businessAcraFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //		else
                    //		{
                    //			//IC of Customer
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
                    //			{
                    //				if (changeICOfCustomer)
                    //				{
                    //					string[] icOfCustomerFiles = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');

                    //					foreach (string file in icOfCustomerFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//Business Name Card
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
                    //			{
                    //				if (changeBusinessNameCard)
                    //				{
                    //					string[] businessNameCardFiles = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');

                    //					foreach (string file in businessNameCardFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}

                    //			//KYC Form
                    //			if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
                    //			{
                    //				if (changeKYCForm)
                    //				{
                    //					string[] kycFormFiles = customerDocumentChecklists.Natural_KYCForm.Split(',');

                    //					foreach (string file in kycFormFiles)
                    //					{
                    //						string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                    //						string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                    //						if (System.IO.File.Exists(sourceFile))
                    //						{
                    //							if (!System.IO.File.Exists(destinationFile))
                    //							{
                    //								System.IO.File.Move(sourceFile, destinationFile);
                    //							}
                    //						}
                    //						else
                    //						{
                    //							string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                    //							if (System.IO.File.Exists(sourceFile2))
                    //							{
                    //								if (!System.IO.File.Exists(destinationFile))
                    //								{
                    //									System.IO.File.Move(sourceFile2, destinationFile);
                    //								}
                    //							}
                    //						}
                    //					}
                    //				}
                    //			}
                    //		}
                    //	}
                    //}

                    //Save Customer Sanctions and PEP Screening Report
                    //Delete Old Data
                    //List<CustomerScreeningReport> oldReports = oldParticulars.PEPScreeningReports;

                    //remove tempdata and add new
                    bool RemoveTempCustomerScreeningReport = _TempCustomerScreeningReportsModel.DeleteAll(oldParticulars.ID);

                    //if (oldParticulars.PEPScreeningReports.Count > 0)
                    //{
                    //	bool AddTempCustomerScreeningReport = _TempCustomerScreeningReportsModel.Add(oldParticulars.PEPScreeningReports);
                    //}

                    //bool del_report = false;

                    //foreach (CustomerScreeningReport old in oldReports)
                    //{
                    //	bool result_del_report = _customerScreeningReportsModel.Delete(old.ID);

                    //	if (result_del_report)
                    //	{
                    //		if (!del_report)
                    //		{
                    //			del_report = true;
                    //		}
                    //	}
                    //}

                    //if (del_report)
                    //{
                    //	AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerScreeningReports", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Sanctions and PEP Screening Report");
                    //}

                    //Save Screening Report Data
                    bool hasScreeningReport = false;

                    foreach (string report in screeningReports)
                    {
                        string[] r = report.Split('|');

                        Temp_CustomerScreeningReports screeningReport = new Temp_CustomerScreeningReports();
                        screeningReport.CustomerParticularId = oldParticulars.ID;
                        screeningReport.Date = Convert.ToDateTime(r[0]);
                        screeningReport.DateOfAcra = Convert.ToDateTime(r[1]);
                        screeningReport.ScreenedBy = r[2];
                        screeningReport.ScreeningReport_1 = r[3];
                        screeningReport.ScreeningReport_2 = r[4];
                        screeningReport.Remarks = r[5];

                        bool result_screeningReport = _TempCustomerScreeningReportsModel.AddSingle(screeningReport);

                        if (result_screeningReport)
                        {
                            if (!hasScreeningReport)
                            {
                                hasScreeningReport = true;
                            }
                        }
                    }

                    if (hasScreeningReport)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerScreeningReports", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Sanctions and PEP Screening Report");
                    }

                    //Start Activity Log
                    //Delete Old Data
                    List<CustomerActivityLog> oldActivities = oldParticulars.ActivityLogs;

                    bool del_activities = false;

                    //remove tempdata and add new
                    bool RemoveTempCustomerActivityLogs = _TempCustomerActivityLogsModel.DeleteAll(oldParticulars.ID);

                    if (oldParticulars.ActivityLogs.Count > 0)
                    {
                        bool AddTempAcivityLog = _TempCustomerActivityLogsModel.Add(oldParticulars.ActivityLogs);
                    }

                    //foreach (CustomerActivityLog old in oldActivities)
                    //{
                    //	bool result_del_activities = _customerActivityLogsModel.Delete(old.ID);

                    //	if (result_del_activities)
                    //	{
                    //		if (!del_activities)
                    //		{
                    //			del_activities = true;
                    //		}
                    //	}
                    //}

                    if (del_activities)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Activity Logs");
                    }

                    //Save Screening Report Data
                    bool hasActivityLogs = false;

                    foreach (string report in ActivityLogs)
                    {
                        string[] r = report.Split('|');

                        CustomerActivityLog activityLogs = new CustomerActivityLog();
                        activityLogs.CustomerParticularId = oldParticulars.ID;
                        activityLogs.Title = r[0];
                        activityLogs.ActivityLog_DateTime = Convert.ToDateTime(r[1]);
                        activityLogs.ActivityLog_Note = r[2];

                        bool result_activityLogs = _customerActivityLogsModel.Add(activityLogs);

                        if (result_activityLogs)
                        {
                            if (!hasActivityLogs)
                            {
                                hasActivityLogs = true;
                            }
                        }
                    }

                    if (hasActivityLogs)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Activity Logs");
                    }
                    //End of Activity Log

                    //Save Customer Others
                    //if (string.IsNullOrEmpty(customerOthers.Status))
                    //               {
                    //                   if (oldParticulars.Others[0].Status == "Active")
                    //                   {
                    //                       customerOthers.Status = "Pending Approval";
                    //                   }
                    //                   else
                    //                   {
                    //                       customerOthers.Status = oldParticulars.Others[0].Status;
                    //                   }
                    //               }
                    //               else
                    //               {
                    //                   //if (customerOthers.Status == "Active")
                    //                   //{
                    //                       //customerOthers.Status = "Pending Approval";
                    //                   //}
                    //               }
                    customerOthers.ApprovalBy = Convert.ToInt32(Session["UserId"]);

                    //Append Bank Account No
                    foreach (string bankAccount in bankAccountNos)
                    {
                        if (!string.IsNullOrEmpty(bankAccount))
                        {
                            customerOthers.BankAccountNo += bankAccount + "|";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerOthers.BankAccountNo))
                    {
                        customerOthers.BankAccountNo = customerOthers.BankAccountNo.Substring(0, customerOthers.BankAccountNo.Length - 1);
                    }

                    //Update OtherStatus Here
                    //check customer others models (Temp)
                    var checkCustomerOthers = _TempCustomerOthersModel.GetOthers(oldParticulars.ID);

                    if (checkCustomerOthers != null)
                    {
                        //update
                        bool updateCheckOthers = _TempCustomerOthersModel.Update(checkCustomerOthers.ID, customerOthers, customerOthers.Status);
                    }
                    else
                    {
                        //add
                        customerOthers.CustomerParticularId = oldParticulars.ID;
                        bool addCheckOthers = _TempCustomerOthersModel.Add(customerOthers, customerOthers.Status);
                    }

                    //customerOthers.Status = "Pending Approval";
                    bool result_Others = _customerOthersModel.UpdateStatus(oldParticulars.Others[0].ID, "Pending Approval");

                    if (result_Others)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Updated Customer Others");

                        //Screening Results Document
                        if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
                        {
                            if (changeScreeningResultsDocument)
                            {
                                string[] screeningResultsDocument = customerOthers.ScreeningResultsDocument.Split(',');

                                foreach (string file in screeningResultsDocument)
                                {
                                    string sourceFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), file);
                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                                    if (System.IO.File.Exists(sourceFile))
                                    {
                                        if (!System.IO.File.Exists(destinationFile))
                                        {
                                            System.IO.File.Move(sourceFile, destinationFile);
                                        }
                                    }
                                    else
                                    {
                                        string sourceFile2 = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;//Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString()), file);
                                        if (System.IO.File.Exists(sourceFile2))
                                        {
                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(sourceFile2, destinationFile);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Save Custome Custom Rate
                    //Delete Old Data
                    //List<CustomerCustomRate> oldRates = oldParticulars.CustomRates;

                    //remove tempdata and add new
                    bool RemoveTempCustomerCustomRate = _TempCustomerCustomRatesModel.DeleteAll(oldParticulars.ID);

                    //if (oldParticulars.CustomRates.Count > 0)
                    //{
                    //	bool AddTempCustomRate = _TempCustomerCustomRatesModel.Add(oldParticulars.CustomRates);
                    //}

                    //bool del_customRate = false;

                    //foreach (CustomerCustomRate oldRate in oldRates)
                    //{
                    //	bool del_OldRate = _customerCustomRatesModel.Delete(oldRate.ID);

                    //	if (del_OldRate)
                    //	{
                    //		if (!del_customRate)
                    //		{
                    //			del_customRate = true;
                    //		}
                    //	}
                    //}

                    //if (del_customRate)
                    //{
                    //	AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerCustomRates", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Custom Rates");
                    //}

                    List<string> customRateKeys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

                    bool res_CustomRates = false;

                    foreach (string key in customRateKeys)
                    {
                        string _id = key.Substring(14);

                        if (string.IsNullOrEmpty(form["default-rate-" + _id]))
                        {
                            Temp_CustomerCustomRates rate = new Temp_CustomerCustomRates();
                            rate.CustomerParticularId = oldParticulars.ID;
                            rate.ProductId = Convert.ToInt32(form[key]);
                            int check = 0;

                            if (!string.IsNullOrEmpty(form["CustomBuyRate_" + _id]))
                            {
                                rate.BuyRate = Convert.ToDecimal(form["CustomBuyRate_" + _id]);
                                check = 1;
                            }

                            if (!string.IsNullOrEmpty(form["CustomSellRate_" + _id]))
                            {
                                rate.SellRate = Convert.ToDecimal(form["CustomSellRate_" + _id]);
                                check = 1;
                            }

                            if (!string.IsNullOrEmpty(form["CustomEncashmentRate_" + _id]))
                            {
                                rate.EncashmentRate = Convert.ToDecimal(form["CustomEncashmentRate_" + _id]);
                                check = 1;
                            }

                            //rate.BuyRate = Convert.ToDecimal(form["CustomBuyRate_" + _id]);
                            //rate.SellRate = Convert.ToDecimal(form["CustomBuyRate_" + _id]);
                            //rate.EncashmentRate = Convert.ToDecimal(form["CustomEncashmentRate_" + _id]);

                            if (check == 1)
                            {
                                bool result_CustomRate = _TempCustomerCustomRatesModel.AddSingle(rate);

                                if (result_CustomRate)
                                {
                                    if (!res_CustomRates)
                                    {
                                        res_CustomRates = true;
                                    }
                                }
                            }
                        }
                    }

                    if (res_CustomRates)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerCustomRates", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Custom Rates");
                    }

                    //Save and Delete Remittance Product Rate
                    //Delete Old Data
                    //List<CustomerRemittanceProductCustomRate> oldRemittanceProuctRates = oldParticulars.CustomerRemittanceProductCustomRates;

                    //remove tempdata and add new
                    bool RemoveTempCustomerRemittanceProdductRate = _TempCustomerRemittanceProductCustomRatesModel.DeleteAll(oldParticulars.ID);

                    //if (oldParticulars.CustomerRemittanceProductCustomRates.Count > 0)
                    //{
                    //	bool AddTempRemittanceProdductCustomRate = _TempCustomerRemittanceProductCustomRatesModel.Add(oldParticulars.CustomerRemittanceProductCustomRates);
                    //}

                    //bool del_remittanceProductCustomRate = false;

                    //foreach (CustomerRemittanceProductCustomRate oldRPRate in oldRemittanceProuctRates)
                    //{
                    //	bool del_OldRPRate = _CustomerRemittanceProductCustomRatesModel.Delete(oldRPRate.ID);

                    //	if (del_OldRPRate)
                    //	{
                    //		if (!del_remittanceProductCustomRate)
                    //		{
                    //			del_remittanceProductCustomRate = true;
                    //		}
                    //	}
                    //}

                    //if (del_remittanceProductCustomRate)
                    //{
                    //	AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerRemittanceProductCustomFee", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Remittance Product Custom Fee");
                    //}

                    //Save Remittance Product Custom Rate
                    List<string> RemittanceProductCustomRateKeys = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();

                    bool res_RemittanceProductCustomFee = false;
                    int checkSave = 0;
                    foreach (string key in RemittanceProductCustomRateKeys)
                    {
                        string rpid = key.Substring(19);

                        if (string.IsNullOrEmpty(form["default-fee-" + rpid]))
                        {
                            Temp_CustomerRemittanceProductCustomRates fee = new Temp_CustomerRemittanceProductCustomRates();
                            fee.CustomerParticularId = oldParticulars.ID;
                            fee.RemittanceProductId = Convert.ToInt32(form[key]);

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomFee_" + rpid]))
                            {
                                fee.Fee = Convert.ToDecimal(form["RemittanceProductsCustomFee_" + rpid]);
                                checkSave = 1;
                            }

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomBuyRate_" + rpid]))
                            {
                                fee.PayRateAdjustment = Convert.ToDecimal(form["RemittanceProductsCustomBuyRate_" + rpid]);
                                checkSave = 1;
                            }

                            if (!string.IsNullOrEmpty(form["RemittanceProductsCustomSellRate_" + rpid]))
                            {
                                fee.GetRateAdjustment = Convert.ToDecimal(form["RemittanceProductsCustomSellRate_" + rpid]);
                                checkSave = 1;
                            }

                            if (checkSave == 1)
                            {
                                checkSave = 0;
                                bool result_CustomFee = _TempCustomerRemittanceProductCustomRatesModel.AddSingle(fee);

                                if (result_CustomFee)
                                {
                                    if (!res_RemittanceProductCustomFee)
                                    {
                                        res_RemittanceProductCustomFee = true;
                                    }
                                }
                            }
                        }
                    }

                    if (res_RemittanceProductCustomFee)
                    {
                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerRemittanceProductCustomFee", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Custom Fee");
                    }
                    //End

                    TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully updated!");

                    return RedirectToAction("Listing", new { @page = page });
                }
                else
                {
                    TempData.Add("Result", "danger|An error occured while saving customer record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There is something wrong in the form!");
            }

            //ViewData["EditStatus"] Postback get status if is KYCVerifiedAndReupdate, this is to get

            IList<CustomerActivityLog> getActivityLogs = _customerActivityLogsModel.GetAll(customerParticulars.ID);

            List<string> activityLogsList = new List<string>();

            foreach (CustomerActivityLog _activitylogs in getActivityLogs)
            {
                activityLogsList.Add(_activitylogs.Title + "|" + _activitylogs.ActivityLog_DateTime.ToString("dd/MM/yyyy hh:mm") + "|" + _activitylogs.ActivityLog_Note);
            }

            ViewData["ActivityLog"] = activityLogsList;
            ViewData["CustomerParticular"] = customerParticulars;
            ViewData["CustomerSourceOfFund"] = customerSourceOfFunds;
            ViewData["CustomerActingAgent"] = customerActingAgents;
            ViewData["CustomerDocumentCheckList"] = customerDocumentChecklists;
            ViewData["CustomerOther"] = customerOthers;
            ViewData["PendingApproval"] = "No";
            ViewData["CustomerID"] = customerParticulars.ID;
            if (oldParticulars.Others[0].Status == "Pending Approval")
            {
                ViewData["PendingApproval"] = "Yes";
            }

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);

            Dropdown[] typeOfEntityDDL = TypeOfEntityDDL();
            ViewData["TypeOfEntityDropdown"] = new SelectList(typeOfEntityDDL, "val", "name", customerParticulars.Company_TypeOfEntity);

            Dropdown[] gradingDDL = GradingDDL();
            ViewData["GradingDropdown"] = new SelectList(gradingDDL, "val", "name", customerOthers.Grading);

            Dropdown[] screeningResultsDDL = ScreeningResultsDDL();
            ViewData["ScreeningResultsDropdown"] = new SelectList(screeningResultsDDL, "val", "name", customerOthers.ScreeningResults);

            Dropdown[] customerProfileDDL = CustomerProfileDDL();
            ViewData["CustomerProfileDropdown"] = new SelectList(customerProfileDDL, "val", "name", customerOthers.CustomerProfile);

            if (oldParticulars.Others[0].Status != "Pending Approval")
            {
                Dropdown[] statusDDL = StatusDDL();
                ViewData["StatusDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);
            }

            ViewData["CompanyForm"] = "";
            ViewData["NaturalForm"] = "";

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                ViewData["NaturalForm"] = "display:none;";
            }
            else
            {
                ViewData["CompanyForm"] = "display:none;";
            }

            ViewData["NaturalEmployedForm"] = "display:none;";
            ViewData["NaturalSelfEmployedForm"] = "display:none;";

            ViewData["NaturalEmploymentEmployedRadio"] = "";
            ViewData["NaturalEmploymentSelfEmployedRadio"] = "";

            if (!string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
            {
                if (customerParticulars.Natural_EmploymentType == "Employed")
                {
                    ViewData["NaturalEmployedForm"] = "";
                    ViewData["NaturalEmploymentEmployedRadio"] = "checked";
                }
                else
                {
                    ViewData["NaturalSelfEmployedForm"] = "";
                    ViewData["NaturalEmploymentSelfEmployedRadio"] = "checked";
                }
            }

            ViewData["CompanySOFBankCreditCheckbox"] = "";
            ViewData["CompanySOFInvestmentCheckbox"] = "";
            ViewData["CompanySOFOthersCheckbox"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["NaturalSOFSalaryCheckbox"] = "";
            ViewData["NaturalSOFBusinessProfitCheckbox"] = "";
            ViewData["NaturalSOFSavingsCheckbox"] = "";
            ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "";
            ViewData["NaturalSOFGiftCheckbox"] = "";
            ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "";
            ViewData["NaturalSOFOthersCheckbox"] = "";
            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "";
            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "";
            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "";
            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "";
            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "";
            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "";
            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "";
            ViewData["EnableTransactionTypeRemittanceCheckbox"] = "";
            ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "";
            ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "";

            if (!string.IsNullOrEmpty(form["customerParticulars.EnableTransactionType"]))
            {
                string[] type = form["customerParticulars.EnableTransactionType"].ToString().Split(',');

                if (Array.IndexOf(type, "Remittance") >= 0)
                {
                    ViewData["EnableTransactionTypeRemittanceCheckbox"] = "checked";
                }

                if (Array.IndexOf(type, "Currency Exchange") >= 0)
                {
                    ViewData["EnableTransactionTypeCurrencyExchangeCheckbox"] = "checked";
                }

                if (Array.IndexOf(type, "Withdrawal") >= 0)
                {
                    ViewData["EnableTransactionTypeWithdrawalCheckbox"] = "checked";
                }
            }

            if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
            {
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    ViewData["CompanySourceOfFund"] = form["Company_source_of_fund"];
                    if (!string.IsNullOrEmpty(form["customerSourceOfFunds.Company_SourceOfFund"]))
                    {
                        //if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Bank Credit Line"))
                        //{
                        //	ViewData["CompanySOFBankCreditCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Directors' / Shareholders' / Sole Proprietor's Investments"))
                        //{
                        //	ViewData["CompanySOFInvestmentCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Others"))
                        //{
                        //	ViewData["CompanySOFOthersCheckbox"] = "checked";
                        //}
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
                    {
                        if (customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3 == "Yes")
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanySOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                        }
                    }
                }
                else
                {
                    ViewData["NaturalSourceOfFund"] = form["Natural_source_of_fund"];
                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_SourceOfFund))
                    {
                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Salary"))
                        //{
                        //	ViewData["NaturalSOFSalaryCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Business Profits"))
                        //{
                        //	ViewData["NaturalSOFBusinessProfitCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Savings"))
                        //{
                        //	ViewData["NaturalSOFSavingsCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Sale of Real Estate"))
                        //{
                        //	ViewData["NaturalSOFSaleOfRealEstateCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Gift/Inheritance"))
                        //{
                        //	ViewData["NaturalSOFGiftCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Investment Earnings"))
                        //{
                        //	ViewData["NaturalSOFInvestmentEarningsCheckbox"] = "checked";
                        //}

                        //if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Others"))
                        //{
                        //	ViewData["NaturalSOFOthersCheckbox"] = "checked";
                        //}
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
                    {
                        if (customerSourceOfFunds.Natural_AnnualIncome == "$10K TO $25K")
                        {
                            ViewData["NaturalSOFAnnualIncome10to25Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$25K TO $50K")
                        {
                            ViewData["NaturalSOFAnnualIncome25to50Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$50K TO $100K")
                        {
                            ViewData["NaturalSOFAnnualIncome50to100Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$100K TO $200K")
                        {
                            ViewData["NaturalSOFAnnualIncome100to200Radio"] = "checked";
                        }
                        else if (customerSourceOfFunds.Natural_AnnualIncome == "$200K TO $500K")
                        {
                            ViewData["NaturalSOFAnnualIncome200to500Radio"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFAnnualIncomeAbove500Radio"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_1"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_1"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_2"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_2"] = "checked";
                        }
                    }

                    if (!string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
                    {
                        if (customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3 == "Yes")
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsYesRadio_3"] = "checked";
                        }
                        else
                        {
                            ViewData["NaturalSOFPoliticallyExposedIndividualsNoRadio_3"] = "checked";
                        }
                    }
                }
            }

            ViewData["ActingAgentForm"] = "display:none;";
            ViewData["CompanyActingAgentYesRadio"] = "";
            ViewData["CompanyActingAgentNoRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "";
            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "";

            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
            {
                if (customerActingAgents.ActingAgent == "Yes")
                {
                    ViewData["ActingAgentForm"] = "";
                    ViewData["CompanyActingAgentYesRadio"] = "checked";

                    if (!string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                    {
                        if (customerActingAgents.Company_CustomerType == "Entity")
                        {
                            ViewData["CompanyActingAgentCustomerTypeEntityRadio"] = "checked";
                        }
                        else
                        {
                            ViewData["CompanyActingAgentCustomerTypeIndividualRadio"] = "checked";
                        }
                    }
                }
                else
                {
                    ViewData["CompanyActingAgentNoRadio"] = "checked";
                }
            }

            if (!string.IsNullOrEmpty(customerParticulars.CustomerType))
            {
                if (customerParticulars.CustomerType == "Corporate & Trading Company")
                {
                    List<string> personnelKeys = form.AllKeys.Where(e => e.Contains("Personnel_FullName_")).ToList();
                    List<string> personnels = new List<string>();
                    int count = 1;

                    foreach (string key in personnelKeys)
                    {
                        string _id = key.Substring(19);
                        personnels.Add(count + "|" + form[key].ToString() + "|" + form["Personnel_ICPassport_" + _id].ToString() + "|" + form["Personnel_Nationality_" + _id].ToString() + "|" + form["Personnel_JobTitle_" + _id].ToString());
                        //if (!string.IsNullOrEmpty(form["Personnel_SpecimenSignature_Old_" + _id]))
                        //{
                        //    personnels.Add(count + "|" + form[key].ToString() + "|" + form["Personnel_ICPassport_" + _id].ToString() + "|" + form["Personnel_Nationality_" + _id].ToString() + "|" + form["Personnel_JobTitle_" + _id].ToString() + "|" + form["Personnel_SpecimenSignature_" + _id].ToString() + "|Old");
                        //}
                        //else
                        //{
                        //    personnels.Add(count + "|" + form[key].ToString() + "|" + form["Personnel_ICPassport_" + _id].ToString() + "|" + form["Personnel_Nationality_" + _id].ToString() + "|" + form["Personnel_JobTitle_" + _id].ToString() + "|" + form["Personnel_SpecimenSignature_" + _id].ToString());
                        //}

                        count++;
                    }

                    ViewData["Personnels"] = personnels;

                }
            }

            //ViewData for File Uploads Domain Folders
            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePassporWorkingPass))
            {
                ViewData["CompanySelfiePassporWorkingPass"] = customerDocumentChecklists.Company_SelfiePassporWorkingPass;

                string[] files = customerDocumentChecklists.Company_SelfiePassporWorkingPass.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_SelfiePassporWorkingPass))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_SelfiePassporWorkingPass.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString();
                            }
                            else
                            {
                                ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["CompanySelfieWorkingPassFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_SelfiePhotoID))
            {
                ViewData["CompanySelfiePhotoID"] = customerDocumentChecklists.Company_SelfiePhotoID;

                string[] files = customerDocumentChecklists.Company_SelfiePhotoID.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_SelfiePhotoID))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_SelfiePhotoID.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString();
                            }
                            else
                            {
                                ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["CompanySelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_SelfiePhotoID))
            {
                ViewData["NaturalSelfiePhotoID"] = customerDocumentChecklists.Natural_SelfiePhotoID;

                string[] files = customerDocumentChecklists.Natural_SelfiePhotoID.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Natural_SelfiePhotoID))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Natural_SelfiePhotoID.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString();
                            }
                            else
                            {
                                ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["NaturalSelfiePhotoFolder_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
            {
                ViewData["BasisOfAuthorityFiles"] = customerActingAgents.BasisOfAuthority;

                string[] files = customerActingAgents.BasisOfAuthority.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.ActingAgents[0].BasisOfAuthority))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.ActingAgents[0].BasisOfAuthority.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString();
                            }
                            else
                            {
                                ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["BasisOfAuthorityFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_AccountOpeningForm))
            {
                ViewData["CompanyAccountOpeningFormFiles"] = customerDocumentChecklists.Company_AccountOpeningForm;

                string[] files = customerDocumentChecklists.Company_AccountOpeningForm.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_AccountOpeningForm))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_AccountOpeningForm.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString();
                            }
                            else
                            {
                                ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["AccountOpeningFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons))
            {
                ViewData["CompanyICAuthorizedFormFiles"] = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons;

                string[] files = customerDocumentChecklists.Company_ICWithAuthorizedTradingPersons.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_ICWithAuthorizedTradingPersons))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_ICWithAuthorizedTradingPersons.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString();
                            }
                            else
                            {
                                ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["ICWithTradingPersonFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_ICWithDirectors))
            {
                ViewData["CompanyICDirectorFiles"] = customerDocumentChecklists.Company_ICWithDirectors;

                string[] files = customerDocumentChecklists.Company_ICWithDirectors.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_ICWithDirectors))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_ICWithDirectors.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString();
                            }
                            else
                            {
                                ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["ICWithDirectorFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Company_BusinessProfileFromAcra))
            {
                ViewData["CompanyBusinessFromAcraFiles"] = customerDocumentChecklists.Company_BusinessProfileFromAcra;

                string[] files = customerDocumentChecklists.Company_BusinessProfileFromAcra.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Company_BusinessProfileFromAcra))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Company_BusinessProfileFromAcra.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString();
                            }
                            else
                            {
                                ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["BusinessAcraFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_ICOfCustomer))
            {
                ViewData["NaturalICOfCustomerFiles"] = customerDocumentChecklists.Natural_ICOfCustomer;

                string[] files = customerDocumentChecklists.Natural_ICOfCustomer.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Natural_ICOfCustomer))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Natural_ICOfCustomer.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString();
                            }
                            else
                            {
                                ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["ICOfCustomerFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_BusinessNameCard))
            {
                ViewData["NaturalBusinessNameCardFiles"] = customerDocumentChecklists.Natural_BusinessNameCard;

                string[] files = customerDocumentChecklists.Natural_BusinessNameCard.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Natural_BusinessNameCard))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Natural_BusinessNameCard.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString();
                            }
                            else
                            {
                                ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["BusinessNameCardFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerDocumentChecklists.Natural_KYCForm))
            {
                ViewData["NaturalKYCFormFiles"] = customerDocumentChecklists.Natural_KYCForm;

                string[] files = customerDocumentChecklists.Natural_KYCForm.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.DocumentCheckLists[0].Natural_KYCForm))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.DocumentCheckLists[0].Natural_KYCForm.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["KYCFormFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["KYCFormFolder"].ToString();
                            }
                            else
                            {
                                ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["KYCFormFolderDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(customerOthers.ScreeningResultsDocument))
            {
                ViewData["ScreeningResultsDocument"] = customerOthers.ScreeningResultsDocument;

                string[] files = customerOthers.ScreeningResultsDocument.Split(',');
                int count = 1;

                if (!string.IsNullOrEmpty(oldParticulars.Others[0].ScreeningResultsDocument))
                {
                    foreach (string file in files)
                    {
                        if (oldParticulars.Others[0].ScreeningResultsDocument.Contains(file))
                        {
                            //Check Main first, if dont have, check shared 
                            string getfilepath = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString() + file;
                            if (System.IO.File.Exists(getfilepath))
                            {
                                ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString();
                            }
                            else
                            {
                                ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString();
                            }
                        }
                        else
                        {
                            ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();
                        }

                        count++;
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        ViewData["ScreeningResultsDocumentDomain_" + count] = ConfigurationManager.AppSettings["TempFolder"].ToString();

                        count++;
                    }
                }
            }

            List<string> keys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();
            List<string> customRates = new List<string>();

            foreach (string key in keys)
            {
                string _id = key.Substring(14);

                if (!string.IsNullOrEmpty(form["default-rate-" + _id]))
                {
                    customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + _id].ToString() + "|" + form["CustomBuyRate_" + _id] + "|" + form["CustomSellRate_" + _id].ToString() + "|" + form["CustomEncashmentRate_" + _id].ToString() + "|checked|" + form["CustomBuyRateDefault_" + _id].ToString() + "|" + form["CustomSellRateDefault_" + _id].ToString());
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked");
                }
                else
                {
                    customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + _id].ToString() + "|" + form["CustomBuyRate_" + _id] + "|" + form["CustomSellRate_" + _id].ToString() + "|" + form["CustomEncashmentRate_" + _id].ToString() + "||" + form["CustomBuyRateDefault_" + _id].ToString() + "|" + form["CustomSellRateDefault_" + _id].ToString());
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|");
                }
            }
            ViewData["CustomRates"] = customRates;

            List<string> Ckeys = form.AllKeys.Where(e => e.Contains("RemittanceProducts_")).ToList();
            List<string> customFees = new List<string>();

            foreach (string key in Ckeys)
            {
                string remittanceid = key.Substring(19);

                if (!string.IsNullOrEmpty(form["default-fee-" + remittanceid]))
                {
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked");
                    //customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "|checked");
                    customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomFee_" + remittanceid].ToString() + "|checked|" + form["RemittanceProductsCustomBuyRate_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomSellRate_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomBuyRateDefault_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomSellRateDefault_" + remittanceid].ToString());
                }
                else
                {
                    //customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|");
                    //customFees.Add(id + "|" + form["RemittanceProductsCustomFeeCode_" + id].ToString() + "|" + form["RemittanceProductsCustomFee_" + id].ToString() + "|");
                    customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomFee_" + remittanceid].ToString() + "||" + form["RemittanceProductsCustomBuyRate_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomSellRate_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomBuyRateDefault_" + remittanceid].ToString() + "|" + form["RemittanceProductsCustomSellRateDefault_" + remittanceid].ToString());
                }
            }

            //List<string> Ckeys = form.AllKeys.Where(e => e.Contains("CustomFee_")).ToList();
            //List<string> customFees = new List<string>();

            //foreach (string key in Ckeys)
            //{
            //	string rpid = key.Substring(28);

            //	if (!string.IsNullOrEmpty(form["default-fee-" + rpid]))
            //	{
            //		//customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|checked");
            //		customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + rpid].ToString() + "|" + form["RemittanceProductsCustomFee_" + rpid].ToString() + "|checked");
            //	}
            //	else
            //	{
            //		//customRates.Add(form[key].ToString() + "|" + form["CustomProductCode_" + id].ToString() + "|" + form["CustomBuyRate_" + id] + "|" + form["CustomSellRate_" + id].ToString() + "|" + form["CustomEncashmentRate_" + id].ToString() + "|");
            //		customFees.Add(form[key].ToString() + "|" + form["RemittanceProductsCustomFeeCode_" + rpid].ToString() + "|" + form["RemittanceProductsCustomFee_" + rpid].ToString() + "|");
            //	}
            //}
            ViewData["remittanceProductCustomFees"] = customFees;

            if (changeAccountOpening)
            {
                ViewData["AccountOpeningFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeICWithTradingPerson)
            {
                ViewData["ICWithTradingPersonFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeICWithDirector)
            {
                ViewData["ICWithDirectorFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeBusinessAcra)
            {
                ViewData["BusinessAcraFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeICOfCustomer)
            {
                ViewData["ICOfCustomerFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeBusinessNameCard)
            {
                ViewData["BusinessNameCardFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeKYCForm)
            {
                ViewData["KYCFormFolderDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            if (changeScreeningResultsDocument)
            {
                ViewData["ScreeningResultsDocumentDomain"] = ConfigurationManager.AppSettings["TempFolder"].ToString();
            }

            ViewData["ActivityLog"] = ActivityLogs;
            ViewData["BankAccountNo"] = bankAccountNos;
            ViewData["PEPScreeningReports"] = screeningReports;

            ViewData["CustomerCountry"] = form["Country"];
            ViewData["CustomerCountryCode"] = form["CountryCode"];
            ViewData["CompanyServiceLikeToUse"] = form["Company_service_like_to_use"];
            ViewData["CompanyPurposeOfIntendedTransactions"] = form["Company_purpose_of_intended_transactions"];
            ViewData["CompanyHearAboutUs"] = form["Company_hear_about_us"];

            ViewData["NaturalServiceLikeToUse"] = form["Natural_service_like_to_use"];
            ViewData["NaturalPurposeOfIntendedTransactions"] = form["Natural_purpose_of_intended_transactions"];
            ViewData["NaturalHearAboutUs"] = form["Natural_hear_about_us"];

            ViewData["CustomerDOB"] = form["dob-datepicker"];

            //List Search Tag
            IList<SearchTags> getSearchTags = _searchTagsModel.GetAll().ToList();
            ViewData["SearchTagList"] = getSearchTags;
            ViewData["SearchTagSelectedItem"] = "";

            if (!string.IsNullOrEmpty(form["Search_Tag_Dropdown_Form"]))
            {
                ViewData["SearchTagSelectedItem"] = form["Search_Tag_Dropdown_Form"];
            }

            //Start
            //Has Customer Account DDL
            Dropdown[] hascustomerAccountDDL = HasCustomerAccountDDL();
            ViewData["HasCustomerAccountDropdown"] = new SelectList(hascustomerAccountDDL, "val", "name", form["customerParticulars.hasCustomerAccount"]);

            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", form["customerParticulars.IsVerify"]);

            //Is Main Account DDL
            //0 - Main Account
            //1 - Sub Account (then need to get the Main Account Customer ID and assign into this IsSubAccount field)
            Dropdown[] isMainAccountDDL = IsMainAccountDDL();
            ViewData["IsMainAccountDropdown"] = new SelectList(isMainAccountDDL, "val", "name", form["customerParticulars.IsSubAccount"]);

            //Main Account Customer DDL
            int MainAccountID = 0;
            if (form["MainAccountCustomer"] != null)
            {
                MainAccountID = Convert.ToInt32(form["MainAccountCustomer"]);
            }

            Dropdown[] mainAccountCustomerDDL = MainAccountCustomerDDL(MainAccountID);
            ViewData["MainAccountCustomerDropdown"] = new SelectList(mainAccountCustomerDDL, "val", "name", form["MainAccountCustomer"]);
            //End

            ViewData["CustomerTitle"] = form["Customer_Title"];

            using (var context = new DataAccess.GreatEastForex())
            {
                ViewBag.CountryList = context.Countries.Where(e => e.IsDeleted == 0).ToList();
                ViewBag.CountryCodeList = context.CountryCodeLists.Where(e => e.IsDeleted == 0).ToList();
            }

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        private string getArtemisStatus(int id,string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = "/api/customer/" + id.ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("X-Domain-ID", "174");
                var response = client.GetAsync(Api.baseAddress + url).Result;
                string stringData = response.Content.ReadAsStringAsync().Result;
                
                if (response.IsSuccessStatusCode)
                {
                    using (JsonTextReader reader = new JsonTextReader(new StringReader(stringData)))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Resp data = serializer.Deserialize<Resp>(reader);
                        return data.status;
                    }
                }               
            }
            return "no data";
        }
        private string GetTokenDemo()
        {
            using (HttpClient client = new HttpClient())
            {
                Models.Token tk = new Models.Token();
                //var baseAddress = "https://api1.artemisuat.cynopsis.co";
                //var baseAddress = "http://localhost:5132";
                var api = "";
                //var baseAddress = "https://crm-demo.cynopsis.co/oauth/token";
                //var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImNybS1rZXktaWQifQ.eyJzdWIiOm51bGwsInNjb3BlIjpbInJlYWQsd3JpdGUsYXJ0ZW1pcyJdLCJpc3MiOiJodHRwczovL2NybS1kZW1vLmN5bm9wc2lzLmNvIiwiZXhwIjoxNzEyOTEzMDEyLCJhdXRob3JpdGllcyI6WyJST0xFX1NFUlZJQ0UiXSwianRpIjoiZjUyNTdjY2MtNDRmMS00YzIxLWJiM2UtYTUzYTU5ODFhZjcwIiwiY2xpZW50X2lkIjoiOWQ1OWU5N2MtNzM5Ni00ZTc3LThjNzAtNWU0MGY0MDc5OTI0In0.SRoVJcpjI43CqtLP8KOEb_ChSuVBDU2Xs0UPoC4Y3zqvy74LAAAgFgCRJdbH8GBOSIUWXYvZwbumH2EwHL6cYRixlTbPRFmCGWYl5pmEoBhm_Lvogg_xb1szKcPH30qezRiW6zQL087si7W3uxpGSjhji3G0MxPTjp-F5_eQFmRSkFJtvGNgZlcKpfwsK3ffIGgklXh1j9Rk0w3AC9EA4rcBssv_WhCsxynlf-CZlp_VnmgF1PUKyD1ydubVix1k6pCbH5z0Nq5Sy-ACHBUkJq_7hiA3mvrPHUttfeIbJsxHS5sK33gPbdRXGrKzqwBvRnzBK3mZWS5EsbW3_N5qGg";

                //client.BaseAddress = new Uri(baseAddress);
                var contentType = new ContentType("application/x-www-form-urlencoded");
                //client.DefaultRequestHeaders.Add(contentType);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //client.DefaultRequestHeaders.Add("X-Domain-ID", "174");

                //var postData = JsonConvert.SerializeObject(userRequest);
                //Dictionary<string, Customers> postContent = new Dictionary<string, Customers>();
                //postContent.Add("customerDTO", cust);

                tk.client_id = "9d59e97c-7396-4e77-8c70-5e40f4079924";
                tk.grant_type = "client_credentials";
                tk.client_secret = "72dbfc03c148aa5f930f1c5d84779f54";
                var postData = JsonConvert.SerializeObject(tk);
                var contentData = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                var formData = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", "9d59e97c-7396-4e77-8c70-5e40f4079924"),
                        new KeyValuePair<string, string>("client_secret", "72dbfc03c148aa5f930f1c5d84779f54")
                        // Add more key-value pairs as needed
                });

                var response = client.PostAsync(Api.tokenAddress + api, formData).Result;
                if (response.IsSuccessStatusCode)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;

                    using (JsonTextReader reader = new JsonTextReader(new StringReader(stringData)))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Resp data = serializer.Deserialize<Resp>(reader);
                        //return data.access_token;
                        //string connString = Configuration.GetConnectionString("myDb1");
                        //using (SqlConnection myConnection = new SqlConnection(connString))
                        //{
                        //    myConnection.Open();
                        //    string query = "INSERT INTO ApiToken(token,project) VALUES (@res,1)";
                        //    using (SqlCommand command = new SqlCommand(query, myConnection))
                        //    {
                        //        command.Parameters.AddWithValue("@res", data.access_token);
                        //        command.ExecuteNonQuery();
                        //    }
                        //    myConnection.Close();
                        return data.access_token;
                        //}
                    }
                }
                return "false";
            }
        }

        //End New Update
        [RedirectingActionWithDLFNIVOMCSGMSA]
        [HttpPost]
        public ActionResult EditActivityLog(FormCollection form)
        {
            //Activity Logs
            List<string> ActivityLogsKey = form.AllKeys.Where(e => e.Contains("CustomerActivityLogs_Key_Title")).ToList();
            List<string> ActivityLogs = new List<string>();
            responseActivityLog data2 = new responseActivityLog();

            data2.success = false;

            try
            {
                //Start Activity Log
                //Delete Old Data
                var getCustomerID = form["CustomerID"];
                int id = 0;
                int checkError = 0;
                if (!string.IsNullOrEmpty(getCustomerID))
                {
                    id = Convert.ToInt32(getCustomerID);

                    CustomerParticular oldParticulars = _customerParticularsModel.GetSingle(id);
                    List<CustomerActivityLog> oldActivities = oldParticulars.ActivityLogs;

                    int rowCount_Activity = 1;

                    if (ActivityLogsKey.Count != 0)
                    {
                        foreach (string key in ActivityLogsKey)
                        {
                            string rowId = key.Split('_')[3];

                            if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_Title_" + rowId]))
                            {
                                data2.error = "Title is required.";
                                checkError = 1;

                            }

                            if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_DateTime_" + rowId]))
                            {
                                data2.error = "Activity Log DateTime is required.";
                                checkError = 1;


                            }
                            else
                            {
                                try
                                {
                                    DateTime date = Convert.ToDateTime(form["CustomerActivityLogs_Key_DateTime_" + rowId]);
                                }
                                catch
                                {
                                    data2.error = "Activity Log DateTime is not valid.";
                                    checkError = 1;
                                }
                            }

                            if (string.IsNullOrEmpty(form["CustomerActivityLogs_Key_Note_" + rowId]))
                            {
                                data2.error = "Note is required.";
                                checkError = 1;
                            }

                            ActivityLogs.Add(form["CustomerActivityLogs_Key_Title_" + rowId] + "|" + form["CustomerActivityLogs_Key_DateTime_" + rowId] + "|" + form["CustomerActivityLogs_Key_Note_" + rowId]);
                        }
                    }

                    if (checkError == 0)
                    {
                        bool del_activities = false;

                        //remove tempdata
                        bool RemoveTempCustomerActivityLogs = _TempCustomerActivityLogsModel.DeleteAll(oldParticulars.ID);

                        foreach (CustomerActivityLog old in oldActivities)
                        {
                            bool result_del_activities = _customerActivityLogsModel.Delete(old.ID);

                            if (result_del_activities)
                            {
                                if (!del_activities)
                                {
                                    del_activities = true;
                                }
                            }
                        }

                        if (del_activities)
                        {
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Activity Logs");
                        }

                        //Save Screening Report Data
                        bool hasActivityLogs = false;

                        foreach (string report in ActivityLogs)
                        {
                            string[] r = report.Split('|');

                            CustomerActivityLog activityLogs = new CustomerActivityLog();
                            activityLogs.CustomerParticularId = id;
                            activityLogs.Title = r[0];
                            activityLogs.ActivityLog_DateTime = Convert.ToDateTime(r[1]);
                            activityLogs.ActivityLog_Note = r[2];

                            bool result_activityLogs = _customerActivityLogsModel.Add(activityLogs);

                            if (result_activityLogs)
                            {
                                if (!hasActivityLogs)
                                {
                                    hasActivityLogs = true;
                                }
                            }
                        }

                        if (hasActivityLogs)
                        {
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Activity Logs");
                            data2.success = true;
                        }
                        //End of Activity Log
                    }
                }
                else
                {
                    data2.error = "Invalid Customer ID";
                }
            }
            catch
            {
                data2.error = "Something went wrong when adding activity log into database.";
            }

            return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);
        }

        //GET: Delete [DL, GM, SA]
        [RedirectingActionWithDLGMSA]
        public ActionResult Delete(int id)
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
            }

            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

            if (customerParticulars != null)
            {
                bool result = _customerParticularsModel.Delete(id);

                if (result)
                {
                    AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerParticulars", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Particular");

                    TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully deleted!");
                }
                else
                {
                    TempData.Add("Result", "danger|An error occured while deleting customer record!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Customer record not found!");
            }

            return RedirectToAction("Listing", new { @page = page });
        }

        //GET: CustomerApproval
        //public ActionResult CustomerApproval(int id, string approval, string reason = "")
        //{
        //	int page = 1;

        //	if (TempData["Page"] != null)
        //	{
        //		page = Convert.ToInt32(TempData["Page"]);
        //	}

        //	CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

        //	string userRole = Session["UserRole"].ToString();
        //	string[] userRoleList = userRole.Split(',');

        //	if (customerParticulars != null)
        //	{
        //		if (Array.IndexOf(userRoleList, "General Manager") >= 0)
        //		{
        //			string status = "Active";

        //			if (approval == "disapprove")
        //			{
        //				status = "Rejected";

        //				//means this one is created from Customer Portal
        //				if (customerParticulars.isKYCVerify == 0)
        //				{
        //					//one more check is from Customer portal or not
        //					if (customerParticulars.lastKYCPageUpdate != 0)
        //					{
        //						//This is from Customer Portal creation
        //						//this is new created account so dont have item to edit, direct update status
        //						//this is reject, direct update status only.
        //						int approvalID = Convert.ToInt32(Session["UserId"]);
        //						bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

        //						//set last kycpageupdate to 1, because customer portal need to show 10% instead of 75%
        //						using (var context = new DataAccess.GreatEastForex())
        //						{
        //							var getCustomerData = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

        //							if (getCustomerData != null)
        //							{
        //								getCustomerData.lastKYCPageUpdate = 1;
        //								context.Configuration.ValidateOnSaveEnabled = false;
        //								context.SaveChanges();
        //								context.Configuration.ValidateOnSaveEnabled = true;

        //								//Move File
        //								//All Data Get From Main Table and move files
        //								CustomerActingAgent getActingAgent = _customerActingAgentsModel.GetActingAgent(getCustomerData.ID);

        //								if (getActingAgent != null)
        //								{
        //									if (!string.IsNullOrEmpty(getActingAgent.BasisOfAuthority))
        //									{
        //										string[] multipleFiles = getActingAgent.BasisOfAuthority.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

        //												if(!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}
        //								}

        //								//Document Check List
        //								CustomerDocumentCheckList getDocumentCheckList = _customerDocumentCheckListsModel.GetDocumentCheckList(getCustomerData.ID);

        //								if (getDocumentCheckList != null)
        //								{
        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePassporWorkingPass))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_SelfiePassporWorkingPass.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePhotoID))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_SelfiePhotoID.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_AccountOpeningForm))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_AccountOpeningForm.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithAuthorizedTradingPersons))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_ICWithAuthorizedTradingPersons.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithDirectors))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_ICWithDirectors.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Company_BusinessProfileFromAcra))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Company_BusinessProfileFromAcra.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_SelfiePhotoID))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Natural_SelfiePhotoID.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}

        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_ICOfCustomer))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Natural_ICOfCustomer.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}

        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_BusinessNameCard))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Natural_BusinessNameCard.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}

        //									if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_KYCForm))
        //									{
        //										string[] multipleFiles = getDocumentCheckList.Natural_KYCForm.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}
        //								}

        //								//Customer Others
        //								CustomerOther getCustomerOthers = _customerOthersModel.GetOthers(getCustomerData.ID);

        //								if (getCustomerOthers != null)
        //								{
        //									if (!string.IsNullOrEmpty(getCustomerOthers.ScreeningResultsDocument))
        //									{
        //										string[] multipleFiles = getCustomerOthers.ScreeningResultsDocument.Split(',');

        //										foreach (string file in multipleFiles)
        //										{
        //											//Check Shared folder file is exist or not
        //											string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

        //											if (System.IO.File.Exists(filepath))
        //											{
        //												string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

        //												if (!System.IO.File.Exists(destinationFile))
        //												{
        //													System.IO.File.Move(filepath, destinationFile);
        //												}
        //											}
        //										}
        //									}
        //								}
        //							}
        //						}

        

        //New Update 2
        //GET: CustomerApproval
        public ActionResult CustomerApproval(int id, string approval, string reason = "")
        {
            int page = 1;

            if (TempData["Page"] != null)
            {
                page = Convert.ToInt32(TempData["Page"]);
            }

            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

            string userRole = Session["UserRole"].ToString();
            string[] userRoleList = userRole.Split(',');

            if (customerParticulars != null)
            {
                if (Array.IndexOf(userRoleList, "General Manager") >= 0)
                {
                    string status = "Active";

                    if (approval == "disapprove")
                    {
                        status = "Rejected";

                        //New Update
                        //All Update Status to rejected only.
                        //This is from Customer Portal creation
                        //this is new created account so dont have item to edit, direct update status
                        //this is reject, direct update status only.
                        int approvalID = Convert.ToInt32(Session["UserId"]);
                        bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //set last kycpageupdate to 1, because customer portal need to show 10% instead of 75%
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var getCustomerData = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

                            if (getCustomerData != null)
                            {
                                getCustomerData.lastKYCPageUpdate = 1;
                                context.Configuration.ValidateOnSaveEnabled = false;
                                context.SaveChanges();
                                context.Configuration.ValidateOnSaveEnabled = true;

                                ////Move File
                                ////All Data Get From Main Table and move files
                                //CustomerActingAgent getActingAgent = _customerActingAgentsModel.GetActingAgent(getCustomerData.ID);

                                //if (getActingAgent != null)
                                //{
                                //	if (!string.IsNullOrEmpty(getActingAgent.BasisOfAuthority))
                                //	{
                                //		string[] multipleFiles = getActingAgent.BasisOfAuthority.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}
                                //}

                                ////Document Check List
                                //CustomerDocumentCheckList getDocumentCheckList = _customerDocumentCheckListsModel.GetDocumentCheckList(getCustomerData.ID);

                                //if (getDocumentCheckList != null)
                                //{
                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePassporWorkingPass))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_SelfiePassporWorkingPass.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePhotoID))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_SelfiePhotoID.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_AccountOpeningForm))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_AccountOpeningForm.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithAuthorizedTradingPersons))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_ICWithAuthorizedTradingPersons.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithDirectors))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_ICWithDirectors.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Company_BusinessProfileFromAcra))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Company_BusinessProfileFromAcra.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_SelfiePhotoID))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Natural_SelfiePhotoID.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}

                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_ICOfCustomer))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Natural_ICOfCustomer.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}

                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_BusinessNameCard))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Natural_BusinessNameCard.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}

                                //	if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_KYCForm))
                                //	{
                                //		string[] multipleFiles = getDocumentCheckList.Natural_KYCForm.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}
                                //}

                                //Customer Others
                                //CustomerOther getCustomerOthers = _customerOthersModel.GetOthers(getCustomerData.ID);

                                //if (getCustomerOthers != null)
                                //{
                                //	if (!string.IsNullOrEmpty(getCustomerOthers.ScreeningResultsDocument))
                                //	{
                                //		string[] multipleFiles = getCustomerOthers.ScreeningResultsDocument.Split(',');

                                //		foreach (string file in multipleFiles)
                                //		{
                                //			//Check Shared folder file is exist or not
                                //			string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                //			if (System.IO.File.Exists(filepath))
                                //			{
                                //				string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                                //				if (!System.IO.File.Exists(destinationFile))
                                //				{
                                //					System.IO.File.Move(filepath, destinationFile);
                                //				}
                                //			}
                                //		}
                                //	}
                                //}
                            }
                        }

                        if (result)
                        {
                            //Send Email
                            string subject = "GEFX Customer Portal Verify Account";
                            string recipient = customerParticulars.Company_Email;//form["Email"];

                            if (customerParticulars.CustomerType == "Natural Person")
                            {
                                recipient = customerParticulars.Natural_Email;
                            }

                            //if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                            //{
                            //	recipient = customerParticulars.Natural_Email;
                            //}

                            //string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                            int userid = Convert.ToInt32(Session["UserId"]);
                            ListDictionary replacements = new ListDictionary();

                            if (approval == "approve")
                            {
                                if (customerParticulars.hasCustomerAccount == 1)
                                {
                                    //Comment this
                                    //bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX Account Status");
                                }

                                AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                                TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                            }
                            else
                            {

                                if (customerParticulars.hasCustomerAccount == 1)
                                {
                                    //Comment this
                                    //bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX Account Status");
                                }

                                AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                                TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                            }
                        }
                        else
                        {
                            TempData.Add("Result", "danger|An error occured while approving customer!");
                        }
                        //End New Update

                        //means this one is created from Customer Portal
                        //if (customerParticulars.isKYCVerify == 0)
                        //{
                        //	//one more check is from Customer portal or not
                        //	if (customerParticulars.lastKYCPageUpdate != 0)
                        //	{

                        //	}
                        //	else
                        //	{
                        //		//This is from Admin portal creation
                        //		//update customer particular item first.
                        //		Temp_CustomerParticulars getCustomerParticularItem = _TempCustomerParticularsModel.GetSingle2(customerParticulars.ID);

                        //		if (getCustomerParticularItem != null)
                        //		{
                        //			//update customer particular data
                        //			bool updateCustomerParticularData = _customerParticularsModel.UpdateRejectZeroKYC(customerParticulars.ID, getCustomerParticularItem, status);

                        //			Temp_CustomerActingAgents Temp_CustomerActingAgents = _TempCustomerActingAgentsModel.GetActingAgent(customerParticulars.ID);

                        //			if (Temp_CustomerActingAgents != null)
                        //			{
                        //				//update acting agent
                        //				bool updateCustomerActingAgents = _customerActingAgentsModel.UpdateReject(customerParticulars.ID, Temp_CustomerActingAgents);
                        //			}
                        //			else
                        //			{
                        //				if (customerParticulars.ActingAgents.Count == 0)
                        //				{
                        //					bool removeCustomerActingAgents = _customerActingAgentsModel.DeleteAll(customerParticulars.ID);
                        //				}
                        //			}

                        //			//Activity Log
                        //			//IList<Temp_CustomerActivityLogs> temp_activitylogs = _TempCustomerActivityLogsModel.GetAll(customerParticulars.ID);

                        //			//if (temp_activitylogs.Count > 0)
                        //			//{
                        //			//	//remove all and readd again
                        //			//	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);

                        //			//	//readd
                        //			//	bool addCustomerActivityLogs = _customerActivityLogsModel.AddReject(temp_activitylogs);
                        //			//}
                        //			//else
                        //			//{
                        //			//	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);
                        //			//}

                        //			//Appointment of staff
                        //			IList<Temp_CustomerAppointmentOfStaffs> temp_Appointmentofstaff = _TempCustomerAppointmentOfStaffsModel.GetAppointments(customerParticulars.ID);

                        //			if (temp_Appointmentofstaff.Count > 0)
                        //			{
                        //				//remove all and readd again
                        //				bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);

                        //				//readd
                        //				bool addCustomerAppointmentOfStaff = _customerAppointmentOfStaffsModel.AddReject(temp_Appointmentofstaff);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Customer Custom Rate
                        //			IList<Temp_CustomerCustomRates> temp_CustomRate = _TempCustomerCustomRatesModel.GetCustomRates(customerParticulars.ID);

                        //			if (temp_CustomRate.Count > 0)
                        //			{
                        //				bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);

                        //				bool addCustomerCustomRate = _customerCustomRatesModel.AddReject(temp_CustomRate);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Customer Remittance Product Custom Rate
                        //			IList<Temp_CustomerRemittanceProductCustomRates> temp_RPCustomRate = _TempCustomerRemittanceProductCustomRatesModel.GetCustomRates(customerParticulars.ID);

                        //			if (temp_RPCustomRate.Count > 0)
                        //			{
                        //				bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);

                        //				bool addCustomerRPCustomRate = _CustomerRemittanceProductCustomRatesModel.AddReject(temp_RPCustomRate);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Document Check List
                        //			Temp_CustomerDocumentCheckLists temp_DocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(customerParticulars.ID);

                        //			if (temp_DocumentCheckList != null)
                        //			{
                        //				if (customerParticulars.DocumentCheckLists.Count > 0)
                        //				{
                        //					//udpate
                        //					bool updateDocumentCheckList = _customerDocumentCheckListsModel.UpdateReject(customerParticulars.ID, temp_DocumentCheckList);
                        //				}
                        //				else
                        //				{
                        //					//if tempdata have but origin data dont have, add new.
                        //					bool addDocumentCheckList = _customerDocumentCheckListsModel.AddReject(temp_DocumentCheckList);
                        //				}
                        //			}
                        //			else
                        //			{
                        //				bool removeDocumentCheckList = _customerDocumentCheckListsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Others
                        //			Temp_CustomerOthers temp_CustomerOthers = _TempCustomerOthersModel.GetOthers(customerParticulars.ID);

                        //			if (temp_CustomerOthers != null)
                        //			{
                        //				if (customerParticulars.Others.Count > 0)
                        //				{
                        //					//update
                        //					bool updateCustomerOthers = _customerOthersModel.UpdateReject(customerParticulars.ID, temp_CustomerOthers);
                        //				}
                        //				else
                        //				{
                        //					//add
                        //					bool addCustomerOthers = _customerOthersModel.AddReject(temp_CustomerOthers);
                        //				}
                        //			}

                        //			//Screening Report
                        //			IList<Temp_CustomerScreeningReports> temp_screeningReports = _TempCustomerScreeningReportsModel.GetAll(customerParticulars.ID);

                        //			if (temp_screeningReports.Count > 0)
                        //			{
                        //				//remove previous and readd
                        //				bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);

                        //				bool addScreeningReports = _customerScreeningReportsModel.AddReject(temp_screeningReports);
                        //			}
                        //			else
                        //			{
                        //				//bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Source of fund
                        //			Temp_CustomerSourceOfFunds temp_SourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(customerParticulars.ID);

                        //			if (temp_SourceOfFund != null)
                        //			{
                        //				if (customerParticulars.SourceOfFunds.Count > 0)
                        //				{
                        //					//update
                        //					bool updateSourceOfFund = _customerSourceOfFundsModel.UpdateReject(customerParticulars.ID, temp_SourceOfFund);
                        //				}
                        //				else
                        //				{
                        //					bool addSourceOfFund = _customerSourceOfFundsModel.AddReject(temp_SourceOfFund);
                        //				}
                        //			}
                        //			else
                        //			{
                        //				bool deleteSourceOfFund = _customerSourceOfFundsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//this is approve, direct update status only.
                        //			int approvalID = Convert.ToInt32(Session["UserId"]);
                        //			bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //			if (result)
                        //			{
                        //				//Send Email
                        //				string subject = "GEFX Customer Portal Verify Account";
                        //				string recipient = customerParticulars.Company_Email;//form["Email"];

                        //				if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        //				{
                        //					recipient = customerParticulars.Natural_Email;
                        //				}
                        //				//string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                        //				int userid = Convert.ToInt32(Session["UserId"]);
                        //				ListDictionary replacements = new ListDictionary();

                        //				if (approval == "approve")
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                        //				}
                        //				else
                        //				{

                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                        //				}
                        //			}
                        //			else
                        //			{
                        //				TempData.Add("Result", "danger|An error occured while rejecting customer!");
                        //			}
                        //		}
                        //		else
                        //		{
                        //			//this is new created account so dont have item to edit, direct update status
                        //			//this is reject, direct update status only.
                        //			int approvalID = Convert.ToInt32(Session["UserId"]);
                        //			bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //			if (result)
                        //			{
                        //				//Send Email
                        //				string subject = "GEFX Customer Portal Verify Account";
                        //				string recipient = customerParticulars.Company_Email;//form["Email"];

                        //				if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        //				{
                        //					recipient = customerParticulars.Natural_Email;
                        //				}
                        //				//string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                        //				int userid = Convert.ToInt32(Session["UserId"]);
                        //				ListDictionary replacements = new ListDictionary();

                        //				if (approval == "approve")
                        //				{

                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                        //				}
                        //				else
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                        //				}
                        //			}
                        //			else
                        //			{
                        //				TempData.Add("Result", "danger|An error occured while approving customer!");
                        //			}

                        //			//TempData.Add("Result", "danger|An error occured while rejecting customer!");
                        //		}
                        //	}
                        //}
                        //else
                        //{   //update all to previous record. (this is when all records is verified before and customer/staff update again.
                        //	//isKYCVerify = 1;
                        //	//lasttemppage = 4 (Update from Admin Portal)
                        //	//lasttemppage = 5 (update from customer portal)
                        //	//update customer particular item first.
                        //	if (customerParticulars.lastTempPageUpdate == 5)
                        //	{
                        //		//update status only
                        //		//this is approve, direct update status only.
                        //		int approvalID = Convert.ToInt32(Session["UserId"]);
                        //		bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //		using (var context = new DataAccess.GreatEastForex())
                        //		{
                        //			//update the lastTempPageUpdate to 0
                        //			//Update UpdatedOn to datetimeNow;
                        //			var getCustomerData = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

                        //			if (getCustomerData != null)
                        //			{
                        //				getCustomerData.UpdatedOn = DateTime.Now;
                        //				getCustomerData.lastTempPageUpdate = 1;
                        //				context.Configuration.ValidateOnSaveEnabled = false;
                        //				context.SaveChanges();
                        //				context.Configuration.ValidateOnSaveEnabled = true;

                        //				//Move File
                        //				//All Data Get From Main Table and move files
                        //				CustomerActingAgent getActingAgent = _customerActingAgentsModel.GetActingAgent(getCustomerData.ID);

                        //				if (getActingAgent != null)
                        //				{
                        //					if (!string.IsNullOrEmpty(getActingAgent.BasisOfAuthority))
                        //					{
                        //						string[] multipleFiles = getActingAgent.BasisOfAuthority.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}
                        //					}
                        //				}

                        //				//Document Check List
                        //				CustomerDocumentCheckList getDocumentCheckList = _customerDocumentCheckListsModel.GetDocumentCheckList(getCustomerData.ID);

                        //				if (getDocumentCheckList != null)
                        //				{
                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePassporWorkingPass))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_SelfiePassporWorkingPass.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePhotoID))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_SelfiePhotoID.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_AccountOpeningForm))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_AccountOpeningForm.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithAuthorizedTradingPersons))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_ICWithAuthorizedTradingPersons.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}
                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithDirectors))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_ICWithDirectors.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Company_BusinessProfileFromAcra))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Company_BusinessProfileFromAcra.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_SelfiePhotoID))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Natural_SelfiePhotoID.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_ICOfCustomer))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Natural_ICOfCustomer.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_BusinessNameCard))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Natural_BusinessNameCard.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}

                        //					if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_KYCForm))
                        //					{
                        //						string[] multipleFiles = getDocumentCheckList.Natural_KYCForm.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}
                        //				}

                        //				//Customer Others
                        //				CustomerOther getCustomerOthers = _customerOthersModel.GetOthers(getCustomerData.ID);

                        //				if (getCustomerOthers != null)
                        //				{
                        //					if (!string.IsNullOrEmpty(getCustomerOthers.ScreeningResultsDocument))
                        //					{
                        //						string[] multipleFiles = getCustomerOthers.ScreeningResultsDocument.Split(',');

                        //						foreach (string file in multipleFiles)
                        //						{
                        //							//Check Shared folder file is exist or not
                        //							string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                        //							if (System.IO.File.Exists(filepath))
                        //							{
                        //								string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                        //								if (!System.IO.File.Exists(destinationFile))
                        //								{
                        //									System.IO.File.Move(filepath, destinationFile);
                        //								}
                        //							}
                        //						}

                        //					}
                        //				}
                        //			}
                        //		}
                        //		//bool updateIsKYCApprove = _customerParticularsModel.UpdateApproveKYC(id);

                        //		//Send Email
                        //		string subject = "GEFX Customer Portal Verify Account";
                        //		string recipient = customerParticulars.Company_Email;//form["Email"];

                        //		if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        //		{
                        //			recipient = customerParticulars.Natural_Email;
                        //		}
                        //		//string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                        //		int userid = Convert.ToInt32(Session["UserId"]);
                        //		ListDictionary replacements = new ListDictionary();

                        //		if (result)
                        //		{
                        //			if (approval == "approve")
                        //			{

                        //				if (customerParticulars.hasCustomerAccount == 1)
                        //				{
                        //					bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX KYC Status");
                        //				}

                        //				AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                        //				TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                        //			}
                        //			else
                        //			{
                        //				if (customerParticulars.hasCustomerAccount == 1)
                        //				{
                        //					bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX KYC Status");
                        //				}

                        //				AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                        //				TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                        //			}
                        //		}
                        //		else
                        //		{
                        //			TempData.Add("Result", "danger|An error occured while rejecting customer!");
                        //		}
                        //	}
                        //	else
                        //	{
                        //		Temp_CustomerParticulars getCustomerParticularItem = _TempCustomerParticularsModel.GetSingle2(customerParticulars.ID);

                        //		if (getCustomerParticularItem != null)
                        //		{
                        //			//update customer particular data
                        //			bool updateCustomerParticularData = _customerParticularsModel.UpdateReject(customerParticulars.ID, getCustomerParticularItem);

                        //			Temp_CustomerActingAgents Temp_CustomerActingAgents = _TempCustomerActingAgentsModel.GetActingAgent(customerParticulars.ID);

                        //			if (Temp_CustomerActingAgents != null)
                        //			{
                        //				//update acting agent
                        //				bool updateCustomerActingAgents = _customerActingAgentsModel.UpdateReject(customerParticulars.ID, Temp_CustomerActingAgents);
                        //			}
                        //			else
                        //			{
                        //				if (customerParticulars.ActingAgents.Count == 0)
                        //				{
                        //					bool removeCustomerActingAgents = _customerActingAgentsModel.DeleteAll(customerParticulars.ID);
                        //				}
                        //			}

                        //			//Activity Log
                        //			//IList<Temp_CustomerActivityLogs> temp_activitylogs = _TempCustomerActivityLogsModel.GetAll(customerParticulars.ID);

                        //			//if (temp_activitylogs.Count > 0)
                        //			//{
                        //			//	//remove all and readd again
                        //			//	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);

                        //			//	//readd
                        //			//	bool addCustomerActivityLogs = _customerActivityLogsModel.AddReject(temp_activitylogs);
                        //			//}
                        //			//else
                        //			//{
                        //			//	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);
                        //			//}

                        //			//Appointment of staff
                        //			IList<Temp_CustomerAppointmentOfStaffs> temp_Appointmentofstaff = _TempCustomerAppointmentOfStaffsModel.GetAppointments(customerParticulars.ID);

                        //			if (temp_Appointmentofstaff.Count > 0)
                        //			{
                        //				//remove all and readd again
                        //				bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);

                        //				//readd
                        //				bool addCustomerAppointmentOfStaff = _customerAppointmentOfStaffsModel.AddReject(temp_Appointmentofstaff);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Customer Custom Rate
                        //			IList<Temp_CustomerCustomRates> temp_CustomRate = _TempCustomerCustomRatesModel.GetCustomRates(customerParticulars.ID);

                        //			if (temp_CustomRate.Count > 0)
                        //			{
                        //				bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);

                        //				bool addCustomerCustomRate = _customerCustomRatesModel.AddReject(temp_CustomRate);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Customer RP Custom Rate
                        //			IList<Temp_CustomerRemittanceProductCustomRates> temp_RPCustomRate = _TempCustomerRemittanceProductCustomRatesModel.GetCustomRates(customerParticulars.ID);

                        //			if (temp_RPCustomRate.Count > 0)
                        //			{
                        //				bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);

                        //				bool addCustomerRPCustomRate = _CustomerRemittanceProductCustomRatesModel.AddReject(temp_RPCustomRate);
                        //			}
                        //			else
                        //			{
                        //				bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Document Check List
                        //			Temp_CustomerDocumentCheckLists temp_DocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(customerParticulars.ID);

                        //			if (temp_DocumentCheckList != null)
                        //			{
                        //				if (customerParticulars.DocumentCheckLists.Count > 0)
                        //				{
                        //					//udpate
                        //					bool updateDocumentCheckList = _customerDocumentCheckListsModel.UpdateReject(customerParticulars.ID, temp_DocumentCheckList);
                        //				}
                        //				else
                        //				{
                        //					//if tempdata have but origin data dont have, add new.
                        //					bool addDocumentCheckList = _customerDocumentCheckListsModel.AddReject(temp_DocumentCheckList);
                        //				}
                        //			}
                        //			else
                        //			{
                        //				bool removeDocumentCheckList = _customerDocumentCheckListsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Others
                        //			Temp_CustomerOthers temp_CustomerOthers = _TempCustomerOthersModel.GetOthers(customerParticulars.ID);

                        //			if (temp_CustomerOthers != null)
                        //			{
                        //				if (customerParticulars.Others.Count > 0)
                        //				{
                        //					//update
                        //					bool updateCustomerOthers = _customerOthersModel.UpdateReject(customerParticulars.ID, temp_CustomerOthers);
                        //				}
                        //				else
                        //				{
                        //					//add
                        //					bool addCustomerOthers = _customerOthersModel.AddReject(temp_CustomerOthers);
                        //				}
                        //			}

                        //			//Screening Report
                        //			IList<Temp_CustomerScreeningReports> temp_screeningReports = _TempCustomerScreeningReportsModel.GetAll(customerParticulars.ID);

                        //			if (temp_screeningReports.Count > 0)
                        //			{
                        //				//remove previous and readd
                        //				bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);

                        //				bool addScreeningReports = _customerScreeningReportsModel.AddReject(temp_screeningReports);
                        //			}
                        //			else
                        //			{
                        //				//bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			//Source of fund
                        //			Temp_CustomerSourceOfFunds temp_SourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(customerParticulars.ID);

                        //			if (temp_SourceOfFund != null)
                        //			{
                        //				if (customerParticulars.SourceOfFunds.Count > 0)
                        //				{
                        //					//update
                        //					bool updateSourceOfFund = _customerSourceOfFundsModel.UpdateReject(customerParticulars.ID, temp_SourceOfFund);
                        //				}
                        //				else
                        //				{
                        //					bool addSourceOfFund = _customerSourceOfFundsModel.AddReject(temp_SourceOfFund);
                        //				}
                        //			}
                        //			else
                        //			{
                        //				bool deleteSourceOfFund = _customerSourceOfFundsModel.DeleteAll(customerParticulars.ID);
                        //			}

                        //			using (var context = new DataAccess.GreatEastForex())
                        //			{
                        //				//update the lastTempPageUpdate to 0
                        //				//Update UpdatedOn to datetimeNow;
                        //				var getCustomerData = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

                        //				if (getCustomerData != null)
                        //				{
                        //					getCustomerData.UpdatedOn = DateTime.Now;
                        //					getCustomerData.lastTempPageUpdate = 0;
                        //					context.Configuration.ValidateOnSaveEnabled = false;
                        //					context.SaveChanges();
                        //					context.Configuration.ValidateOnSaveEnabled = true;
                        //				}
                        //			}

                        //			//this is approve, direct update status only.
                        //			int approvalID = Convert.ToInt32(Session["UserId"]);
                        //			bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //			//Send Email
                        //			string subject = "GEFX Customer Portal Verify Account";
                        //			string recipient = customerParticulars.Company_Email;//form["Email"];

                        //			if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        //			{
                        //				recipient = customerParticulars.Natural_Email;
                        //			}
                        //			//string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                        //			int userid = Convert.ToInt32(Session["UserId"]);
                        //			ListDictionary replacements = new ListDictionary();

                        //			if (result)
                        //			{
                        //				if (approval == "approve")
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                        //				}
                        //				else
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                        //				}
                        //			}
                        //			else
                        //			{
                        //				TempData.Add("Result", "danger|An error occured while rejecting customer!");
                        //			}
                        //		}
                        //		else
                        //		{
                        //			//this is new created account so dont have item to edit, direct update status
                        //			//this is approve, direct update status only.
                        //			int approvalID = Convert.ToInt32(Session["UserId"]);
                        //			bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                        //			//Send Email
                        //			string subject = "GEFX Customer Portal Verify Account";
                        //			string recipient = customerParticulars.Company_Email;//form["Email"];

                        //			if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                        //			{
                        //				recipient = customerParticulars.Natural_Email;
                        //			}
                        //			//string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                        //			int userid = Convert.ToInt32(Session["UserId"]);
                        //			ListDictionary replacements = new ListDictionary();

                        //			if (result)
                        //			{
                        //				if (approval == "approve")
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                        //				}
                        //				else
                        //				{
                        //					if (customerParticulars.hasCustomerAccount == 1)
                        //					{
                        //						bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX KYC Status");
                        //					}

                        //					AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                        //					TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                        //				}
                        //			}
                        //			else
                        //			{
                        //				TempData.Add("Result", "danger|An error occured while approving customer!");
                        //			}

                        //			//TempData.Add("Result", "danger|An error occured while rejecting customer!");
                        //		}
                        //	}
                        //}
                    }
                    else
                    {
                        //New Update
                        //If Approve, move all the Temp Record to Main Table
                        //This is created from admin portal, need to copy the temp record to main table.
                        Temp_CustomerParticulars getCustomerParticularItem = _TempCustomerParticularsModel.GetSingle2(customerParticulars.ID);

                        if (getCustomerParticularItem != null)
                        {
                            //update customer particular data
                            bool updateCustomerParticularData = _customerParticularsModel.UpdateReject(customerParticulars.ID, getCustomerParticularItem);

                            Temp_CustomerActingAgents Temp_CustomerActingAgents = _TempCustomerActingAgentsModel.GetActingAgent(customerParticulars.ID);

                            if (Temp_CustomerActingAgents != null)
                            {
                                if (!string.IsNullOrEmpty(Temp_CustomerActingAgents.BasisOfAuthority))
                                {
                                    string[] multipleFiles = Temp_CustomerActingAgents.BasisOfAuthority.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                //update acting agent

                                //check main table have acting agent or not
                                CustomerActingAgent checkActingAgents = _customerActingAgentsModel.GetActingAgent(customerParticulars.ID);

                                if (checkActingAgents != null)
                                {
                                    bool updateCustomerActingAgents = _customerActingAgentsModel.UpdateReject(customerParticulars.ID, Temp_CustomerActingAgents);
                                }
                                else
                                {
                                    //add acting agents
                                    bool addCustomerActingAgents = _customerActingAgentsModel.AddSingle(customerParticulars.ID, Temp_CustomerActingAgents);
                                }
                            }
                            else
                            {
                                if (customerParticulars.ActingAgents.Count == 0)
                                {
                                    bool removeCustomerActingAgents = _customerActingAgentsModel.DeleteAll(customerParticulars.ID);
                                }
                            }

                            //Activity Log
                            //IList<Temp_CustomerActivityLogs> temp_activitylogs = _TempCustomerActivityLogsModel.GetAll(customerParticulars.ID);

                            //if (temp_activitylogs.Count > 0)
                            //{
                            //	//remove all and readd again
                            //	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);

                            //	//readd
                            //	bool addCustomerActivityLogs = _customerActivityLogsModel.AddReject(temp_activitylogs);
                            //}
                            //else
                            //{
                            //	bool removeCurrentActivityLogs = _customerActivityLogsModel.DeleteAll(customerParticulars.ID);
                            //}

                            //Appointment of staff
                            IList<Temp_CustomerAppointmentOfStaffs> temp_Appointmentofstaff = _TempCustomerAppointmentOfStaffsModel.GetAppointments(customerParticulars.ID);

                            if (temp_Appointmentofstaff.Count > 0)
                            {
                                //remove all and readd again
                                bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);

                                //readd
                                bool addCustomerAppointmentOfStaff = _customerAppointmentOfStaffsModel.AddReject(temp_Appointmentofstaff);
                            }
                            else
                            {
                                bool removeCurrentAppointmentOfStaff = _customerAppointmentOfStaffsModel.DeleteAll(customerParticulars.ID);
                            }

                            //Customer Custom Rate
                            IList<Temp_CustomerCustomRates> temp_CustomRate = _TempCustomerCustomRatesModel.GetCustomRates(customerParticulars.ID);

                            if (temp_CustomRate.Count > 0)
                            {
                                bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);

                                bool addCustomerCustomRate = _customerCustomRatesModel.AddReject(temp_CustomRate);
                            }
                            else
                            {
                                bool removeCurrentCustomRate = _customerCustomRatesModel.DeleteAll(customerParticulars.ID);
                            }

                            //Customer RP Custom Rate
                            IList<Temp_CustomerRemittanceProductCustomRates> temp_RPCustomRate = _TempCustomerRemittanceProductCustomRatesModel.GetCustomRates(customerParticulars.ID);

                            if (temp_RPCustomRate.Count > 0)
                            {
                                bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);

                                bool addCustomerRPCustomRate = _CustomerRemittanceProductCustomRatesModel.AddReject(temp_RPCustomRate);
                            }
                            else
                            {
                                bool removeCurrentRPCustomRate = _CustomerRemittanceProductCustomRatesModel.DeleteAll(customerParticulars.ID);
                            }

                            //Document Check List
                            Temp_CustomerDocumentCheckLists temp_DocumentCheckList = _TempCustomerDocumentsCheckListModel.GetDocumentCheckList(customerParticulars.ID);

                            if (temp_DocumentCheckList != null)
                            {
                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_SelfiePassporWorkingPass))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_SelfiePassporWorkingPass.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_SelfiePhotoID))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_SelfiePhotoID.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_AccountOpeningForm))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_AccountOpeningForm.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_ICWithAuthorizedTradingPersons))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_ICWithAuthorizedTradingPersons.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_ICWithDirectors))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_ICWithDirectors.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Company_BusinessProfileFromAcra))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Company_BusinessProfileFromAcra.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Natural_SelfiePhotoID))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Natural_SelfiePhotoID.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Natural_ICOfCustomer))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Natural_ICOfCustomer.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Natural_BusinessNameCard))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Natural_BusinessNameCard.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (!string.IsNullOrEmpty(temp_DocumentCheckList.Natural_KYCForm))
                                {
                                    string[] multipleFiles = temp_DocumentCheckList.Natural_KYCForm.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (customerParticulars.DocumentCheckLists.Count > 0)
                                {
                                    //udpate
                                    bool updateDocumentCheckList = _customerDocumentCheckListsModel.UpdateReject(customerParticulars.ID, temp_DocumentCheckList);
                                }
                                else
                                {
                                    //if tempdata have but origin data dont have, add new.
                                    bool addDocumentCheckList = _customerDocumentCheckListsModel.AddReject(temp_DocumentCheckList);
                                }
                            }
                            else
                            {
                                bool removeDocumentCheckList = _customerDocumentCheckListsModel.DeleteAll(customerParticulars.ID);
                            }

                            //Others
                            Temp_CustomerOthers temp_CustomerOthers = _TempCustomerOthersModel.GetOthers(customerParticulars.ID);

                            if (temp_CustomerOthers != null)
                            {
                                if (!string.IsNullOrEmpty(temp_CustomerOthers.ScreeningResultsDocument))
                                {
                                    string[] multipleFiles = temp_CustomerOthers.ScreeningResultsDocument.Split(',');

                                    foreach (string file in multipleFiles)
                                    {
                                        //Check Shared folder file is exist or not
                                        string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                        if (System.IO.File.Exists(filepath))
                                        {
                                            string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                                            if (!System.IO.File.Exists(destinationFile))
                                            {
                                                System.IO.File.Move(filepath, destinationFile);
                                            }
                                        }
                                    }

                                }

                                if (customerParticulars.Others.Count > 0)
                                {
                                    //update
                                    if (approval != "disapprove")
                                    {
                                        //This is approved.
                                        bool updateCustomerOthers = _customerOthersModel.UpdateReject(customerParticulars.ID, temp_CustomerOthers, "Active");
                                    }
                                    else
                                    {
                                        //This is rejected
                                        bool updateCustomerOthers = _customerOthersModel.UpdateReject(customerParticulars.ID, temp_CustomerOthers, "Rejected");
                                    }
                                }
                                else
                                {
                                    //add
                                    if (approval != "disapprove")
                                    {
                                        //This is approved.
                                        bool addCustomerOthers = _customerOthersModel.AddReject(temp_CustomerOthers, "Active");
                                    }
                                    else
                                    {
                                        //This is rejected
                                        bool addCustomerOthers = _customerOthersModel.AddReject(temp_CustomerOthers, "Rejected");
                                    }

                                }
                            }

                            //Screening Report
                            IList<Temp_CustomerScreeningReports> temp_screeningReports = _TempCustomerScreeningReportsModel.GetAll(customerParticulars.ID);

                            if (temp_screeningReports.Count > 0)
                            {
                                //remove previous and readd
                                bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);

                                bool addScreeningReports = _customerScreeningReportsModel.AddReject(temp_screeningReports);
                            }
                            else
                            {
                                //bool removeCurrentScreeningReports = _customerScreeningReportsModel.DeleteAll(customerParticulars.ID);
                            }

                            //Source of fund
                            Temp_CustomerSourceOfFunds temp_SourceOfFund = _TempCustomerSourceOfFundsModel.GetSourceOfFund(customerParticulars.ID);

                            if (temp_SourceOfFund != null)
                            {
                                if (customerParticulars.SourceOfFunds.Count > 0)
                                {
                                    //update
                                    bool updateSourceOfFund = _customerSourceOfFundsModel.UpdateReject(customerParticulars.ID, temp_SourceOfFund);
                                }
                                else
                                {
                                    bool addSourceOfFund = _customerSourceOfFundsModel.AddReject(temp_SourceOfFund);
                                }
                            }
                            else
                            {
                                bool deleteSourceOfFund = _customerSourceOfFundsModel.DeleteAll(customerParticulars.ID);
                            }

                            //this is approve, direct update status only.
                            int approvalID = Convert.ToInt32(Session["UserId"]);
                            string getCustomerStatus = "Active";

                            if (temp_CustomerOthers != null)
                            {
                                getCustomerStatus = temp_CustomerOthers.Status;
                            }

                            //This is when the customer is Unapproved status
                            //Set Status to "Active" when main customer status is Unapproved
                            if (customerParticulars.Others != null)
                            {
                                if (customerParticulars.Others.FirstOrDefault().Status == "Unapproved")
                                {
                                    getCustomerStatus = "Active";
                                }
                            }

                            //bool result = _customerOthersModel.ApproveCustomer(id, getCustomerStatus, approvalID);

                            //if (getCustomerParticularItem.hasCustomerAccount == 1)
                            //{
                            //	if (getCustomerParticularItem.isVerify == 0)
                            //	{
                            //		using (var context3 = new DataAccess.GreatEastForex())
                            //		{
                            //			var getCustomerData = context3.CustomerParticulars.Where(e => e.ID == customerParticulars.ID).FirstOrDefault();

                            //			if (getCustomerData != null)
                            //			{
                            //				//set new token and send email.
                            //				string key = EncryptionHelper.GenerateRandomString(8);
                            //				string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());

                            //				getCustomerData.VerifyAccountToken = token;
                            //				context3.SaveChanges();

                            //				string subject = "GEFX Customer Portal Verify Account";
                            //				string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/VerifyAccount.html"));
                            //				string recipient = customerParticulars.Company_Email;//form["Email"];

                            //				if (getCustomerData.CustomerType == "Corporate & Trading Company")
                            //				{
                            //					recipient = getCustomerData.Company_Email;
                            //				}
                            //				else
                            //				{
                            //					recipient = getCustomerData.Natural_Email;
                            //				}

                            //				//if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                            //				//{
                            //				//	recipient = customerParticulars.Natural_Email;
                            //				//}
                            //				//string recipient = "hongguan@thedottsolutions.com";//form["Email"];

                            //				if (getCustomerData.CustomerType == "Natural Person")
                            //				{
                            //					ListDictionary replacements = new ListDictionary();
                            //					replacements.Add("<%Name%>", getCustomerData.Natural_Name);
                            //					replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                            //					//replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                            //					int userid = Convert.ToInt32(Session["UserId"]);
                            //					bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");
                            //				}
                            //				else
                            //				{
                            //					ListDictionary replacements = new ListDictionary();
                            //					replacements.Add("<%Name%>", getCustomerData.Surname + " " + getCustomerData.GivenName);
                            //					replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                            //					//replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                            //					int userid = Convert.ToInt32(Session["UserId"]);
                            //					bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");
                            //				}
                            //			}
                            //		}
                            //	}
                            //}

                            if (customerParticulars.IsSubAccount != 0)
                            {
                                //If is sub-account, then add into Authorised Appointment List
                                CustomerAppointmentOfStaff appOfStaff = new CustomerAppointmentOfStaff();
                                appOfStaff.CustomerParticularId = Convert.ToInt32(customerParticulars.IsSubAccount);

                                using (var context4 = new DataAccess.GreatEastForex())
                                {
                                    var checkExist = context4.CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == customerParticulars.ID).FirstOrDefault();

                                    if (checkExist != null)
                                    {
                                        if (customerParticulars.CustomerType == "Natural Person")
                                        {
                                            appOfStaff.FullName = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_Name) ? getCustomerParticularItem.Natural_Name : getCustomerParticularItem.Natural_SelfEmployedBusinessName) ?? getCustomerParticularItem.Surname + " " + getCustomerParticularItem.GivenName;
                                            appOfStaff.JobTitle = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_EmployedJobTitle) ? getCustomerParticularItem.Natural_EmployedJobTitle : "") ?? "";
                                            appOfStaff.Nationality = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_Nationality) ? getCustomerParticularItem.Natural_Nationality : "") ?? "";
                                            appOfStaff.SpecimenSignature = null;
                                            appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_ICPassportNo) ? getCustomerParticularItem.Natural_ICPassportNo : "") ?? "";
                                        }
                                        else
                                        {
                                            appOfStaff.FullName = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_ContactName) ? getCustomerParticularItem.Company_ContactName : getCustomerParticularItem.Company_ContactName) ?? getCustomerParticularItem.Surname + " " + getCustomerParticularItem.GivenName;
                                            appOfStaff.JobTitle = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_JobTitle) ? getCustomerParticularItem.Company_JobTitle : "") ?? "";
                                            appOfStaff.Nationality = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_Nationality) ? getCustomerParticularItem.Company_Nationality : "") ?? "";
                                            appOfStaff.SpecimenSignature = null;
                                            appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_ICPassport) ? getCustomerParticularItem.Company_ICPassport : "") ?? "";
                                        }

                                        bool CustomerAppointment = _customerAppointmentOfStaffsModel.Update(checkExist.ID, appOfStaff);
                                    }
                                    else
                                    {
                                        if (customerParticulars.CustomerType == "Natural Person")
                                        {
                                            appOfStaff.FullName = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_Name) ? getCustomerParticularItem.Natural_Name : getCustomerParticularItem.Natural_SelfEmployedBusinessName) ?? getCustomerParticularItem.Surname + " " + getCustomerParticularItem.GivenName;
                                            appOfStaff.JobTitle = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_EmployedJobTitle) ? getCustomerParticularItem.Natural_EmployedJobTitle : "") ?? "";
                                            appOfStaff.Nationality = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_Nationality) ? getCustomerParticularItem.Natural_Nationality : "") ?? "";
                                            appOfStaff.SpecimenSignature = null;
                                            appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(getCustomerParticularItem.Natural_ICPassportNo) ? getCustomerParticularItem.Natural_ICPassportNo : "") ?? "";
                                        }
                                        else
                                        {
                                            appOfStaff.FullName = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_ContactName) ? getCustomerParticularItem.Company_ContactName : getCustomerParticularItem.Company_ContactName) ?? getCustomerParticularItem.Surname + " " + getCustomerParticularItem.GivenName;
                                            appOfStaff.JobTitle = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_JobTitle) ? getCustomerParticularItem.Company_JobTitle : "") ?? "";
                                            appOfStaff.Nationality = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_Nationality) ? getCustomerParticularItem.Company_Nationality : "") ?? "";
                                            appOfStaff.SpecimenSignature = null;
                                            appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(getCustomerParticularItem.Company_ICPassport) ? getCustomerParticularItem.Company_ICPassport : "") ?? "";
                                        }

                                        bool CustomerAppointment = _customerAppointmentOfStaffsModel.Add(appOfStaff);
                                    }
                                }
                            }

                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var getCustomerData = context.CustomerParticulars.Where(e => e.ID == customerParticulars.ID).FirstOrDefault();

                                if (getCustomerData != null)
                                {
                                    //update all the subaccount under this main account
                                    var BulkUpdate = context.CustomerParticulars.Where(e => e.IsSubAccount == id).ToList();
                                    var KYCBulkUpdate = context.KYC_CustomerParticulars.Where(e => e.IsSubAccount == id).ToList();
                                    var TempBulkUpdate = context.Temp_CustomerParticulars.Where(e => e.IsSubAccount == id).ToList();

                                    if (BulkUpdate.Count > 0)
                                    {
                                        BulkUpdate.ForEach(a =>
                                        {
                                            a.Company_RegisteredName = customerParticulars.Company_RegisteredName;
                                            a.Company_BusinessAddress1 = customerParticulars.Company_BusinessAddress1;
                                            a.Company_BusinessAddress2 = customerParticulars.Company_BusinessAddress2;
                                            a.Company_BusinessAddress3 = customerParticulars.Company_BusinessAddress3;
                                            a.Company_PostalCode = customerParticulars.Company_PostalCode;

                                            a.Shipping_Address1 = customerParticulars.Shipping_Address1;
                                            a.Shipping_Address2 = customerParticulars.Shipping_Address2;
                                            a.Shipping_Address3 = customerParticulars.Shipping_Address3;
                                            a.Shipping_PostalCode = customerParticulars.Shipping_PostalCode;

                                            a.Company_Country = customerParticulars.Company_Country;
                                            a.Company_CountryCode = customerParticulars.Company_CountryCode;

                                            a.Company_TelNo = customerParticulars.Company_TelNo;
                                            a.Company_FaxNo = customerParticulars.Company_FaxNo;

                                            a.Company_PlaceOfRegistration = customerParticulars.Company_PlaceOfRegistration;
                                            a.Company_DateOfRegistration = customerParticulars.Company_DateOfRegistration;

                                            a.Company_RegistrationNo = customerParticulars.Company_RegistrationNo;
                                            a.Company_TypeOfEntity = customerParticulars.Company_TypeOfEntity;
                                            a.Company_TypeOfEntityIfOthers = customerParticulars.Company_TypeOfEntityIfOthers;
                                        });
                                    }

                                    if (KYCBulkUpdate.Count > 0)
                                    {
                                        KYCBulkUpdate.ForEach(a =>
                                        {
                                            a.Company_RegisteredName = customerParticulars.Company_RegisteredName;
                                            a.Company_BusinessAddress1 = customerParticulars.Company_BusinessAddress1;
                                            a.Company_BusinessAddress2 = customerParticulars.Company_BusinessAddress2;
                                            a.Company_BusinessAddress3 = customerParticulars.Company_BusinessAddress3;
                                            a.Company_PostalCode = customerParticulars.Company_PostalCode;

                                            a.Shipping_Address1 = customerParticulars.Shipping_Address1;
                                            a.Shipping_Address2 = customerParticulars.Shipping_Address2;
                                            a.Shipping_Address3 = customerParticulars.Shipping_Address3;
                                            a.Shipping_PostalCode = customerParticulars.Shipping_PostalCode;

                                            a.Company_Country = customerParticulars.Company_Country;
                                            a.Company_CountryCode = customerParticulars.Company_CountryCode;

                                            a.Company_TelNo = customerParticulars.Company_TelNo;
                                            a.Company_FaxNo = customerParticulars.Company_FaxNo;

                                            a.Company_PlaceOfRegistration = customerParticulars.Company_PlaceOfRegistration;
                                            a.Company_DateOfRegistration = customerParticulars.Company_DateOfRegistration;

                                            a.Company_RegistrationNo = customerParticulars.Company_RegistrationNo;
                                            a.Company_TypeOfEntity = customerParticulars.Company_TypeOfEntity;
                                            a.Company_TypeOfEntityIfOthers = customerParticulars.Company_TypeOfEntityIfOthers;
                                        });
                                    }

                                    if (TempBulkUpdate.Count > 0)
                                    {
                                        TempBulkUpdate.ForEach(a =>
                                        {
                                            a.Company_RegisteredName = customerParticulars.Company_RegisteredName;
                                            a.Company_BusinessAddress1 = customerParticulars.Company_BusinessAddress1;
                                            a.Company_BusinessAddress2 = customerParticulars.Company_BusinessAddress2;
                                            a.Company_BusinessAddress3 = customerParticulars.Company_BusinessAddress3;
                                            a.Company_PostalCode = customerParticulars.Company_PostalCode;

                                            a.Shipping_Address1 = customerParticulars.Shipping_Address1;
                                            a.Shipping_Address2 = customerParticulars.Shipping_Address2;
                                            a.Shipping_Address3 = customerParticulars.Shipping_Address3;
                                            a.Shipping_PostalCode = customerParticulars.Shipping_PostalCode;

                                            a.Company_Country = customerParticulars.Company_Country;
                                            a.Company_CountryCode = customerParticulars.Company_CountryCode;

                                            a.Company_TelNo = customerParticulars.Company_TelNo;
                                            a.Company_FaxNo = customerParticulars.Company_FaxNo;

                                            a.Company_PlaceOfRegistration = customerParticulars.Company_PlaceOfRegistration;
                                            a.Company_DateOfRegistration = customerParticulars.Company_DateOfRegistration;

                                            a.Company_RegistrationNo = customerParticulars.Company_RegistrationNo;
                                            a.Company_TypeOfEntity = customerParticulars.Company_TypeOfEntity;
                                            a.Company_TypeOfEntityIfOthers = customerParticulars.Company_TypeOfEntityIfOthers;
                                        });
                                    }

                                    getCustomerData.lastTempPageUpdate = 0;
                                    context.Configuration.ValidateOnSaveEnabled = false;
                                    context.SaveChanges();
                                    context.Configuration.ValidateOnSaveEnabled = true;
                                }
                            }

                            //if (result)
                            //{
                            //Send Email
                            string subject = "GEFX Customer Portal Verify Account";
                            string recipient = customerParticulars.Company_Email;//form["Email"];

                            if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                            {
                                recipient = customerParticulars.Natural_Email;
                            }
                            //string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                            int userid = Convert.ToInt32(Session["UserId"]);
                            ListDictionary replacements = new ListDictionary();

                            if (approval == "approve")
                            {
                                if (customerParticulars.hasCustomerAccount == 1)
                                {
                                    //Comment this
                                    //bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX Account Status");
                                }

                                AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                                TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                            }
                            else
                            {
                                if (customerParticulars.hasCustomerAccount == 1)
                                {
                                    //Comment this
                                    //bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX Account Status");
                                }

                                AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                                TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                            }
                            //}
                            //else
                            //{
                            //	TempData.Add("Result", "danger|An error occured while rejecting customer!");
                            //}
                        }
                        else
                        {
                            int approvalID = Convert.ToInt32(Session["UserId"]);
                            bool result = _customerOthersModel.ApproveCustomer(id, status, approvalID);

                            //set last kycpageupdate to 1, because customer portal need to show 10% instead of 75%
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var getCustomerData = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

                                if (getCustomerData != null)
                                {
                                    getCustomerData.lastKYCPageUpdate = 0;
                                    context.Configuration.ValidateOnSaveEnabled = false;
                                    context.SaveChanges();
                                    context.Configuration.ValidateOnSaveEnabled = true;

                                    //Move File
                                    //All Data Get From Main Table and move files
                                    CustomerActingAgent getActingAgent = _customerActingAgentsModel.GetActingAgent(getCustomerData.ID);

                                    if (getActingAgent != null)
                                    {
                                        if (!string.IsNullOrEmpty(getActingAgent.BasisOfAuthority))
                                        {
                                            string[] multipleFiles = getActingAgent.BasisOfAuthority.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BasisOfAuthorityFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //Document Check List
                                    CustomerDocumentCheckList getDocumentCheckList = _customerDocumentCheckListsModel.GetDocumentCheckList(getCustomerData.ID);

                                    if (getDocumentCheckList != null)
                                    {
                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePassporWorkingPass))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_SelfiePassporWorkingPass.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfieWorkingPassFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_SelfiePhotoID))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_SelfiePhotoID.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CompanySelfiePhotoFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_AccountOpeningForm))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_AccountOpeningForm.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AccountOpeningFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithAuthorizedTradingPersons))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_ICWithAuthorizedTradingPersons.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithTradingPersonFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_ICWithDirectors))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_ICWithDirectors.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICWithDirectorFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Company_BusinessProfileFromAcra))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Company_BusinessProfileFromAcra.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessAcraFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_SelfiePhotoID))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Natural_SelfiePhotoID.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["NaturalSelfiePhotoFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_ICOfCustomer))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Natural_ICOfCustomer.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ICOfCustomerFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_BusinessNameCard))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Natural_BusinessNameCard.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["BusinessNameCardFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(getDocumentCheckList.Natural_KYCForm))
                                        {
                                            string[] multipleFiles = getDocumentCheckList.Natural_KYCForm.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["KYCFormFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //Customer Others
                                    CustomerOther getCustomerOthers = _customerOthersModel.GetOthers(getCustomerData.ID);

                                    if (getCustomerOthers != null)
                                    {
                                        if (!string.IsNullOrEmpty(getCustomerOthers.ScreeningResultsDocument))
                                        {
                                            string[] multipleFiles = getCustomerOthers.ScreeningResultsDocument.Split(',');

                                            foreach (string file in multipleFiles)
                                            {
                                                //Check Shared folder file is exist or not
                                                string filepath = ConfigurationManager.AppSettings["SharedNetworkTempFolder"].ToString() + file;

                                                if (System.IO.File.Exists(filepath))
                                                {
                                                    string destinationFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ScreeningResultsFolder"].ToString()), file);

                                                    if (!System.IO.File.Exists(destinationFile))
                                                    {
                                                        System.IO.File.Move(filepath, destinationFile);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (customerParticulars.IsSubAccount != 0)
                                    {
                                        //If is sub-account, then add into Authorised Appointment List
                                        CustomerAppointmentOfStaff appOfStaff = new CustomerAppointmentOfStaff();
                                        appOfStaff.CustomerParticularId = customerParticulars.ID;

                                        using (var context4 = new DataAccess.GreatEastForex())
                                        {
                                            var checkExist = context4.CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == customerParticulars.ID).FirstOrDefault();

                                            if (checkExist != null)
                                            {
                                                appOfStaff.FullName = (!string.IsNullOrEmpty(customerParticulars.Natural_Name) ? customerParticulars.Natural_Name : customerParticulars.Natural_SelfEmployedBusinessName) ?? customerParticulars.Surname + "" + customerParticulars.GivenName;
                                                appOfStaff.JobTitle = (!string.IsNullOrEmpty(customerParticulars.Natural_EmployedJobTitle) ? customerParticulars.Natural_EmployedJobTitle : "") ?? "";
                                                appOfStaff.Nationality = (!string.IsNullOrEmpty(customerParticulars.Natural_Nationality) ? customerParticulars.Natural_Nationality : "") ?? "";
                                                appOfStaff.SpecimenSignature = null;
                                                appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(customerParticulars.Natural_ICPassportNo) ? customerParticulars.Natural_ICPassportNo : "") ?? "";
                                                bool CustomerAppointment = _customerAppointmentOfStaffsModel.Update(checkExist.ID, appOfStaff);
                                            }
                                            else
                                            {
                                                appOfStaff.FullName = (!string.IsNullOrEmpty(customerParticulars.Natural_Name) ? customerParticulars.Natural_Name : customerParticulars.Natural_SelfEmployedBusinessName) ?? customerParticulars.Surname + "" + customerParticulars.GivenName;
                                                appOfStaff.JobTitle = (!string.IsNullOrEmpty(customerParticulars.Natural_EmployedJobTitle) ? customerParticulars.Natural_EmployedJobTitle : "") ?? "";
                                                appOfStaff.Nationality = (!string.IsNullOrEmpty(customerParticulars.Natural_Nationality) ? customerParticulars.Natural_Nationality : "") ?? "";
                                                appOfStaff.SpecimenSignature = null;
                                                appOfStaff.ICPassportNo = (!string.IsNullOrEmpty(customerParticulars.Natural_ICPassportNo) ? customerParticulars.Natural_ICPassportNo : "") ?? "";
                                                bool CustomerAppointment = _customerAppointmentOfStaffsModel.Add(appOfStaff);
                                            }
                                        }
                                    }

                                    //if (getCustomerData.isVerify == 0)
                                    //{
                                    //	using (var context3 = new DataAccess.GreatEastForex())
                                    //	{
                                    //		var getCustomerData2 = context3.CustomerParticulars.Where(e => e.ID == customerParticulars.ID).FirstOrDefault();

                                    //		if (getCustomerData2 != null)
                                    //		{
                                    //			//set new token and send email.
                                    //			string key = EncryptionHelper.GenerateRandomString(8);
                                    //			string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());

                                    //			getCustomerData2.VerifyAccountToken = token;
                                    //			context3.SaveChanges();

                                    //			string subject = "GEFX Customer Portal Verify Account";
                                    //			string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/VerifyAccount.html"));
                                    //			string recipient = customerParticulars.Company_Email;//form["Email"];

                                    //			if (getCustomerData2.CustomerType == "Corporate & Trading Company")
                                    //			{
                                    //				recipient = getCustomerData2.Company_Email;
                                    //			}
                                    //			else
                                    //			{
                                    //				recipient = getCustomerData2.Natural_Email;
                                    //			}

                                    //			//if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                                    //			//{
                                    //			//	recipient = customerParticulars.Natural_Email;
                                    //			//}
                                    //			//string recipient = "hongguan@thedottsolutions.com";//form["Email"];

                                    //			if (getCustomerData2.CustomerType == "Natural Person")
                                    //			{
                                    //				ListDictionary replacements = new ListDictionary();
                                    //				replacements.Add("<%Name%>", getCustomerData2.Natural_Name);
                                    //				replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                                    //				//replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                                    //				int userid = Convert.ToInt32(Session["UserId"]);
                                    //				bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");
                                    //			}
                                    //			else
                                    //			{
                                    //				ListDictionary replacements = new ListDictionary();
                                    //				replacements.Add("<%Name%>", getCustomerData2.Surname + " " + getCustomerData2.GivenName);
                                    //				replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                                    //				//replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                                    //				int userid = Convert.ToInt32(Session["UserId"]);
                                    //				bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");
                                    //			}
                                    //		}
                                    //	}
                                    //}
                                }

                                if (result)
                                {
                                    //Send Email
                                    string subject = "GEFX Customer Portal Verify Account";
                                    string recipient = customerParticulars.Company_Email;//form["Email"];

                                    if (string.IsNullOrEmpty(customerParticulars.Company_Email))
                                    {
                                        recipient = customerParticulars.Natural_Email;
                                    }

                                    //string recipient = "hongguan@thedottsolutions.com";//form["Email"];
                                    int userid = Convert.ToInt32(Session["UserId"]);
                                    ListDictionary replacements = new ListDictionary();

                                    if (approval == "approve")
                                    {
                                        if (customerParticulars.hasCustomerAccount == 1)
                                        {
                                            //Comment this
                                            //bool sent = EmailHelper.sendEmail(subject, "Your account is approved!", replacements, recipient, userid, "GEFX Account Status");
                                        }

                                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Approved Customer [" + customerParticulars.CustomerCode + "]");
                                        TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully approved!");
                                    }
                                    else
                                    {

                                        if (customerParticulars.hasCustomerAccount == 1)
                                        {
                                            //Comment this
                                            //bool sent = EmailHelper.sendEmail(subject, "Your account is rejected!<br/>Reason: " + reason, replacements, recipient, userid, "GEFX Account Status");
                                        }

                                        AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Disapproved Customer [" + customerParticulars.CustomerCode + "]");
                                        TempData.Add("Result", "success|" + customerParticulars.CustomerCode + " has been successfully disapproved!");
                                    }
                                }
                                else
                                {
                                    TempData.Add("Result", "danger|An error occured while approving customer!");
                                }
                            }
                        }
                        //End New Update
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|You have no access to this module.");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Customer record not found.");
            }

            return RedirectToAction("Index", "TaskList", new { @page = page });
        }
        //End New Update

        public ActionResult AccessQuickUpdate(int id)
        {
            if (id > 0)
            {
                CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);

                if (customerParticulars != null)
                {
                    if (customerParticulars.Others[0].Status != "Pending Approval")
                    {
                        CustomerOther customerOthers = _customerOthersModel.GetOthers(customerParticulars.ID);
                        ViewData["CustomerRecord"] = customerParticulars;
                        ViewData["CustomerOther"] = customerOthers;
                        Dropdown[] customerTypeDDL = CustomerTypeDDL();
                        ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerParticulars.CustomerType);
                        Dropdown[] statusDDL = StatusQuickUpdateDDL();
                        ViewData["StatusQuickUpdateDropdown"] = new SelectList(statusDDL, "val", "name", customerOthers.Status);

                        Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
                        ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerParticulars.isVerify);

                        Dropdown[] customerTitleDDL = CustomerTitleDDL();
                        ViewData["CustomerTitleDropdown"] = new SelectList(customerTitleDDL, "val", "name", customerParticulars.Customer_Title);

                        ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
                        return View();
                    }
                    else
                    {
                        TempData.Add("Result", "danger|Pending Approval is not allow to update.");
                        return RedirectToAction("Listing");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|Customer record not found.");
                    return RedirectToAction("Listing");
                }
            }
            else
            {
                TempData.Add("Result", "danger|Something went wrong.");
                return RedirectToAction("Listing");
            }
        }

        [HttpPost]
        public ActionResult SendVerifyEmail(int id)
        {
            responseLoadCustomerRecord data2 = new responseLoadCustomerRecord();

            data2.success = false;

            try
            {
                using (var context = new DataAccess.GreatEastForex())
                {
                    if (id > 0)
                    {
                        var checkCustomer = context.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

                        if (checkCustomer != null)
                        {
                            //generate random code and send email.
                            //set new token and send email.
                            string key = EncryptionHelper.GenerateRandomString(8);
                            string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());

                            checkCustomer.VerifyAccountToken = token;
                            context.Configuration.ValidateOnSaveEnabled = false;
                            context.SaveChanges();

                            string subject = "GEFX Customer Portal Verify Account";
                            string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/VerifyAccount.html"));
                            string recipient = checkCustomer.Company_Email;//form["Email"];

                            if (checkCustomer.CustomerType == "Corporate & Trading Company")
                            {
                                recipient = checkCustomer.Company_Email;
                            }
                            else
                            {
                                recipient = checkCustomer.Natural_Email;
                            }

                            if (checkCustomer.CustomerType == "Natural Person")
                            {
                                ListDictionary replacements = new ListDictionary();

                                string NaturalName = "";

                                if (!string.IsNullOrEmpty(checkCustomer.Natural_Name))
                                {
                                    NaturalName = checkCustomer.Natural_Name;
                                }

                                replacements.Add("<%Name%>", NaturalName);
                                replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                                //replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                                int userid = Convert.ToInt32(Session["UserId"]);
                                bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");

                                if (sent)
                                {
                                    AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Sent Customer Verify Email to [" + checkCustomer.Natural_Name + "]");
                                }
                            }
                            else
                            {
                                ListDictionary replacements = new ListDictionary();

                                string CompanyName = "";

                                if (!string.IsNullOrEmpty(checkCustomer.Natural_Name))
                                {
                                    CompanyName = checkCustomer.Company_ContactName;
                                }

                                replacements.Add("<%Name%>", CompanyName);
                                replacements.Add("<%Url%>", ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + recipient + "&key=" + key);
                                //replacements.Add("<%Url%>", Url.Action("VerifyAccount", "Home", new { @email = recipient, @key = key }, "https"));
                                int userid = Convert.ToInt32(Session["UserId"]);
                                bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userid, "Verify Your Account");

                                if (sent)
                                {
                                    AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerOthers", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Sent Customer Verify Email to [" + checkCustomer.Company_ContactName + "]");
                                }

                            }

                            data2.success = true;
                        }
                        else
                        {
                            data2.error = "Customer record not found.";
                        }
                    }
                    else
                    {
                        data2.error = "Invalid Customer ID.";
                    }
                }
            }
            catch
            {
                data2.error = "Something went wrong when sending verify email.";
            }

            return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AccessQuickUpdate(FormCollection form, CustomerParticular customerparticular, CustomerOther other)
        {
            //validation part
            if (form["customerParticulars.Customer_Title"] != null)
            {
                if (form["customerParticulars.Customer_Title"] != "Mr" && form["customerParticulars.Customer_Title"] != "Ms" && form["customerParticulars.Customer_Title"] != "Mrs" && form["customerParticulars.Customer_Title"] != "Mdm" && form["customerParticulars.Customer_Title"] != "Dr")
                {
                    ModelState.AddModelError("customerParticulars.Customer_Title", "Something went wrong in selecting customer title!");
                }
            }
            else
            {
                ModelState.AddModelError("customerParticulars.Customer_Title", "Please select customer title!");
            }

            if (string.IsNullOrEmpty(form["customerParticulars.Surname"]))
            {
                ModelState.AddModelError("customerParticulars.Surname", "Customer Surname is required!");
            }

            if (string.IsNullOrEmpty(form["customerParticulars.GivenName"]))
            {
                ModelState.AddModelError("customerParticulars.GivenName", "Customer Given Name is required!");
            }

            if (form["customerParticulars.CustomerType"] != null)
            {
                switch (form["customerParticulars.CustomerType"])
                {
                    case "Corporate & Trading Company":
                        if (form["customerParticulars.Company_Email"] == null || string.IsNullOrEmpty(form["customerParticulars.Company_Email"]))
                        {
                            //ModelState.AddModelError("customerParticulars.Company_Email", "Company Email is required!");
                            ModelState["CustomerParticulars.Company_Email"].Errors.Clear();
                            ModelState["CustomerType"].Errors.Clear();
                            ModelState.Remove("CustomerParticulars.Company_Email");
                            ModelState.Remove("CustomerType");
                        }
                        else
                        {
                            string Email = form["customerParticulars.Company_Email"];
                            //check email format
                            bool checkEmailFormat = FormValidationHelper.EmailValidation(Email);

                            if (!checkEmailFormat)
                            {
                                ModelState.AddModelError("customerParticulars.Company_Email", "Email format is invalid!");
                            }
                            else
                            {
                                //New Update
                                using (var context = new DataAccess.GreatEastForex())
                                {
                                    var getCustomerRecord = context.CustomerParticulars.Where(e => e.ID == customerparticular.ID).FirstOrDefault();

                                    if (getCustomerRecord != null)
                                    {
                                        if (getCustomerRecord.hasCustomerAccount == 1)
                                        {
                                            //This is edit form
                                            using (var context2 = new DataAccess.GreatEastForex())
                                            {
                                                var GetAllSameEmailCustomer = context2.CustomerParticulars.Where(e => e.Company_Email == Email || e.Natural_Email == Email && e.IsDeleted == "N").Where(e => e.ID != customerparticular.ID).Where(e => e.hasCustomerAccount == 1);
                                                int FinalCheck = 0;

                                                if (GetAllSameEmailCustomer.Count() > 0)
                                                {
                                                    ModelState.AddModelError("customerParticulars.Company_Email", "Email already existed.");
                                                    //errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                    FinalCheck = 1;
                                                }

                                                if (FinalCheck == 0)
                                                {
                                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == Email || e.Natural_Email == Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != customerparticular.ID).Where(e => e.hasCustomerAccount == 1);

                                                    if (GetTempSameEmailCustomer.Count() > 0)
                                                    {
                                                        using (var context3 = new DataAccess.GreatEastForex())
                                                        {
                                                            int CheckCountTemp = 0;

                                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                                            {
                                                                if (CheckCountTemp == 0)
                                                                {
                                                                    //check others is pending approval or not, if yes, then cannot go
                                                                    var checkExist = context3.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID);

                                                                    if (checkExist != null)
                                                                    {
                                                                        if (checkExist.FirstOrDefault().Status != "Active")
                                                                        {
                                                                            ModelState.AddModelError("customerParticulars.Company_Email", "Email already existed.");
                                                                            //errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                                            CheckCountTemp = 1;
                                                                            FinalCheck = 1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (FinalCheck == 0)
                                                {
                                                    customerparticular.Company_Email = Email;
                                                    ModelState["CustomerParticulars.Natural_Email"].Errors.Clear();
                                                    ModelState["CustomerType"].Errors.Clear();
                                                    ModelState.Remove("CustomerParticulars.Natural_Email");
                                                    ModelState.Remove("CustomerType");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            customerparticular.Company_Email = Email;
                                            ModelState["CustomerParticulars.Natural_Email"].Errors.Clear();
                                            ModelState["CustomerType"].Errors.Clear();
                                            ModelState.Remove("CustomerParticulars.Natural_Email");
                                            ModelState.Remove("CustomerType");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("customerParticulars.Company_Email", "Customer record not found.");
                                    }
                                }
                                //End New Update


                                //check unique customer email
                                //int checkEmailExist = _customerParticularsModel.FindEmailNotOwn(customerparticular.ID, Email);

                                //if (checkEmailExist != 0)
                                //{
                                //	ModelState.AddModelError("customerParticulars.Company_Email", "Email is exist!");
                                //}
                                //else
                                //{
                                //	customerparticular.Company_Email = Email;
                                //	ModelState["CustomerParticulars.Natural_Email"].Errors.Clear();
                                //	ModelState["CustomerType"].Errors.Clear();
                                //	ModelState.Remove("CustomerParticulars.Natural_Email");
                                //	ModelState.Remove("CustomerType");
                                //}
                            }
                        }
                        break;
                    case "Natural Person":
                        if (form["customerParticulars.Natural_Email"] == null || string.IsNullOrEmpty(form["customerParticulars.Natural_Email"]))
                        {
                            //ModelState.AddModelError("customerParticulars.Natural_Email", "Natural Person Email is required!");
                            ModelState["CustomerParticulars.Natural_Email"].Errors.Clear();
                            ModelState["CustomerType"].Errors.Clear();
                            ModelState.Remove("CustomerParticulars.Natural_Email");
                            ModelState.Remove("CustomerType");
                        }
                        else
                        {
                            string Email = form["customerParticulars.Natural_Email"];
                            //check email format
                            bool checkEmailFormat = FormValidationHelper.EmailValidation(Email);

                            if (!checkEmailFormat)
                            {
                                ModelState.AddModelError("customerParticulars.Natural_Email", "Email format is invalid!");
                            }
                            else
                            {
                                //New Update
                                using (var context = new DataAccess.GreatEastForex())
                                {
                                    var getCustomerRecord = context.CustomerParticulars.Where(e => e.ID == customerparticular.ID).FirstOrDefault();

                                    if (getCustomerRecord != null)
                                    {
                                        if (getCustomerRecord.hasCustomerAccount == 1)
                                        {
                                            //this is edit form
                                            using (var context2 = new DataAccess.GreatEastForex())
                                            {
                                                var GetAllSameEmailCustomer = context2.CustomerParticulars.Where(e => e.Company_Email == Email || e.Natural_Email == Email && e.IsDeleted == "N").Where(e => e.ID != getCustomerRecord.ID).Where(e => e.hasCustomerAccount == 1);
                                                int FinalCheck = 0;

                                                if (GetAllSameEmailCustomer.Count() > 0)
                                                {
                                                    ModelState.AddModelError("customerParticulars.Natural_Email", "Email is exist.");
                                                    //errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                    FinalCheck = 1;
                                                }

                                                if (FinalCheck == 0)
                                                {
                                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == Email || e.Natural_Email == Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != getCustomerRecord.ID).Where(e => e.hasCustomerAccount == 1);

                                                    if (GetTempSameEmailCustomer.Count() > 0)
                                                    {
                                                        using (var context3 = new DataAccess.GreatEastForex())
                                                        {
                                                            int CheckCountTemp = 0;
                                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                                            {
                                                                if (CheckCountTemp == 0)
                                                                {
                                                                    //check others is pending approval or not, if yes, then cannot go
                                                                    if (context3.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID) != null)
                                                                    {
                                                                        if (context3.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID).FirstOrDefault().Status != "Active")
                                                                        {
                                                                            ModelState.AddModelError("customerParticulars.Natural_Email", "Email is exist.");
                                                                            //errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                                            CheckCountTemp = 1;
                                                                            FinalCheck = 1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (FinalCheck == 0)
                                                {
                                                    customerparticular.Natural_Email = Email;
                                                    ModelState["CustomerParticulars.Company_Email"].Errors.Clear();
                                                    ModelState["CustomerType"].Errors.Clear();
                                                    ModelState.Remove("CustomerParticulars.Company_Email");
                                                    ModelState.Remove("CustomerType");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            customerparticular.Natural_Email = Email;
                                            ModelState["CustomerParticulars.Company_Email"].Errors.Clear();
                                            ModelState["CustomerType"].Errors.Clear();
                                            ModelState.Remove("CustomerParticulars.Company_Email");
                                            ModelState.Remove("CustomerType");
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("customerParticulars.Natural_Email", "Customer record not found.");
                                    }
                                }
                                //End New Update

                                //check unique customer email
                                //int checkEmailExist = _customerParticularsModel.FindEmailNotOwn(customerparticular.ID, Email);

                                //if (checkEmailExist != 0)
                                //{
                                //	ModelState.AddModelError("customerParticulars.Natural_Email", "Email is exist!");
                                //}
                                //else
                                //{
                                //	customerparticular.Natural_Email = Email;
                                //	ModelState["CustomerParticulars.Company_Email"].Errors.Clear();
                                //	ModelState["CustomerType"].Errors.Clear();
                                //	ModelState.Remove("CustomerParticulars.Company_Email");
                                //	ModelState.Remove("CustomerType");
                                //}
                            }
                        }
                        break;
                    default:
                        ModelState.AddModelError("customerParticulars.CustomerType", "Customer Type is invalid!");
                        break;
                }

                if (!string.IsNullOrEmpty(form["Password"].Trim()))
                {
                    if (string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                    {
                        ModelState.AddModelError("ConfirmPassword", "Confirm Password is required!");
                    }
                }

                if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()))
                {
                    if (string.IsNullOrEmpty(form["Password"].Trim()))
                    {
                        ModelState.AddModelError("Password", "Password is required!");
                    }
                }

                if (!string.IsNullOrEmpty(form["ConfirmPassword"].Trim()) && !string.IsNullOrEmpty(form["Password"].Trim()))
                {
                    string Password = form["Password"].Trim();
                    string ConfirmPassword = form["ConfirmPassword"].Trim();

                    if (Password.Length >= 8 && ConfirmPassword.Length >= 8)
                    {
                        //check regex
                        Regex regexPattern = new Regex("^(?=.*[A-Za-z])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})");

                        if (!regexPattern.IsMatch(Password))
                        {
                            ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters and 1 special characters!");
                        }
                        else
                        {
                            if (form["Password"].Trim() != form["ConfirmPassword"].Trim())
                            {
                                ModelState.AddModelError("Password", "New Password and Confirm Password not match!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password and Confirm Password must contain at least 8 characters!");
                    }
                }

                if (form["customerOthers.Status"] != null)
                {
                    if (form["customerOthers.Status"] != "Active" && form["customerOthers.Status"] != "Suspended" && form["customerOthers.Status"] != "Rejected" && form["customerOthers.Status"] != "Unapproved")
                    {
                        ModelState.AddModelError("customerOthers.Status", "Something went wrong in selecting customer status!");
                    }
                }
                else
                {
                    ModelState.AddModelError("customerOthers.Status", "Please select customer status!");
                }

                if (form["customerParticulars.isVerify"] != null)
                {
                    if (form["customerParticulars.isVerify"] != "1" && form["customerParticulars.isVerify"] != "0")
                    {
                        ModelState.AddModelError("customerParticulars.isVerify", "Something went wrong in selecting customer account validated!");
                    }
                }
                else
                {
                    ModelState.AddModelError("customerParticulars.isVerify", "Please select customer account validated!");
                }

                //clear some customer modal error not mandatory field in this part
                ModelState["CustomerParticulars.CustomerCode"].Errors.Clear();
                ModelState["CustomerCode"].Errors.Clear();
                ModelState["Grading"].Errors.Clear();
                ModelState["CustomerProfile"].Errors.Clear();
                ModelState.Remove("CustomerParticulars.CustomerCode");
                ModelState.Remove("CustomerCode");
                ModelState.Remove("Grading");
                ModelState.Remove("CustomerProfile");

                if (ModelState.IsValid)
                {
                    if (form["customerParticulars.CustomerType"] == "Corporate & Trading Company")
                    {
                        customerparticular.CustomerType = form["customerParticulars.CustomerType"];

                        if (form["customerParticulars.Company_Email"] == null || string.IsNullOrEmpty(form["customerParticulars.Company_Email"]))
                        {

                        }
                        else
                        {
                            customerparticular.Company_Email = form["customerParticulars.Company_Email"];
                        }
                    }
                    else
                    {
                        customerparticular.CustomerType = form["customerParticulars.CustomerType"];

                        if (form["customerParticulars.Natural_Email"] == null || string.IsNullOrEmpty(form["customerParticulars.Natural_Email"]))
                        {

                        }
                        else
                        {
                            customerparticular.Natural_Email = form["customerParticulars.Natural_Email"];
                        }
                    }

                    if (form["Password"].Trim() != "" || !string.IsNullOrEmpty(form["Password"].Trim()))
                    {
                        customerparticular.Password = EncryptionHelper.Encrypt(form["Password"]);
                    }

                    customerparticular.Customer_Title = form["customerParticulars.Customer_Title"];
                    customerparticular.Surname = form["customerParticulars.Surname"];
                    customerparticular.GivenName = form["customerParticulars.GivenName"];
                    customerparticular.isVerify = Convert.ToInt32(form["customerParticulars.isVerify"]);

                    if (customerparticular.isVerify == 1)
                    {
                        customerparticular.VerifyAccountToken = null;
                    }
                    else
                    {
                        //if is 0, then reset the verify token
                        string key = EncryptionHelper.GenerateRandomString(8);
                        string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());
                        customerparticular.VerifyAccountToken = token;
                    }

                    other.Status = form["customerOthers.Status"];

                    //save into db
                    //CustomerParticular
                    bool checkUpdateParticular = _customerParticularsModel.UpdateEmergency(customerparticular);

                    if (checkUpdateParticular)
                    {
                        bool checkUpdateOther = _customerOthersModel.UpdateEmergency(customerparticular.ID, other);

                        if (checkUpdateOther)
                        {
                            TempData.Add("Result", "success|Updated Successfully!");

                            //update tempRecord if found
                            bool checkUpdateTempParticular = _TempCustomerParticularsModel.UpdateEmergency(customerparticular);

                            if (checkUpdateTempParticular)
                            {
                                bool checkUpdateTempOther = _TempCustomerOthersModel.UpdateEmergency(customerparticular.ID, other);
                            }

                            //Audit log
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string tableAffected = "Customer";
                            string description = Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Customr Quick Access updated [" + customerparticular.ID + " - " + (string.IsNullOrEmpty(customerparticular.Company_Email) ? customerparticular.Natural_Email : customerparticular.Company_Email) + "] successfully.";

                            AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                            return RedirectToAction("Listing");
                        }
                        else
                        {
                            TempData.Add("Result", "success|Somthing went wrong when updating status!");
                            return RedirectToAction("Listing");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "success|Somthing went wrong when updating customer record!");
                        return RedirectToAction("Listing");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("customerParticulars.CustomerType", "Please select customer type!");
            }

            customerparticular.CustomerType = form["customerParticulars.CustomerType"];
            customerparticular.Company_Email = form["customerParticulars.Company_Email"];
            customerparticular.Natural_Email = form["customerParticulars.Natural_Email"];
            other.Status = form["customerOthers.Status"];

            ViewData["CustomerRecord"] = customerparticular;
            ViewData["CustomerOther"] = other;

            Dropdown[] customerTypeDDL = CustomerTypeDDL();
            ViewData["CustomerTypeDropdown"] = new SelectList(customerTypeDDL, "val", "name", customerparticular.CustomerType);

            Dropdown[] statusDDL = StatusQuickUpdateDDL();
            ViewData["StatusQuickUpdateDropdown"] = new SelectList(statusDDL, "val", "name", other.Status);

            Dropdown[] customerEmailVerifyDDL = CustomerEmailVerifyDDL();
            ViewData["CustomerEmailVerifyDropdown"] = new SelectList(customerEmailVerifyDDL, "val", "name", customerparticular.isVerify);

            Dropdown[] customerTitleDDL = CustomerTitleDDL();
            ViewData["CustomerTitleDropdown"] = new SelectList(customerTitleDDL, "val", "name", customerparticular.Customer_Title);

            ViewData["SiteName"] = ConfigurationManager.AppSettings["SiteName"].ToString();
            return View();
        }

        //Validation for Corporate & Trading Company
        public List<ModelStateValidation> ValidateModelStateForCorporateCompany(CustomerParticular customerParticulars, CustomerSourceOfFund customerSourceOfFunds, CustomerActingAgent customerActingAgents, CustomerDocumentCheckList customerDocumentChecklists, CustomerOther customerOthers, FormCollection form, int cid = 0)
        {
            List<ModelStateValidation> errors = new List<ModelStateValidation>();

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Particular---------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            //if (string.IsNullOrEmpty(customerParticulars.Company_RegisteredName))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_RegisteredName", ErrorMessage = "Registered Name is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_RegisteredAddress))
            //{
            //    errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_RegisteredAddress", ErrorMessage = "Registered Address is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_BusinessAddress1))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_BusinessAddress1", ErrorMessage = "Business Address is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_PostalCode))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_PostalCode", ErrorMessage = "Postal Code is required!" });
            //}

            if (string.IsNullOrEmpty(customerParticulars.Company_ContactName))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_ContactName", ErrorMessage = "Contact Name is required!" });
            }

            //if (string.IsNullOrEmpty(customerParticulars.Company_TelNo))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_TelNo", ErrorMessage = "Tel No is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_FaxNo))
            //{
            //    errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_FaxNo", ErrorMessage = "Fax No is required!" });
            //}

            if (string.IsNullOrEmpty(customerParticulars.Company_Email))
            {
                //errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email is required!" });
            }
            else
            {
                bool checkEmailFormat = FormValidationHelper.EmailValidation(customerParticulars.Company_Email);

                if (!checkEmailFormat)
                {
                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email is not valid!" });
                }
                else
                {
                    //CustomerParticular checkUniqueEmail = _customerParticularsModel.FindEmail(customerParticulars.Company_Email);

                    if (customerParticulars.hasCustomerAccount == 1)
                    {
                        if (cid > 0)
                        {
                            //This is edit form, have cid
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var GetAllSameEmailCustomer = context.CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.ID != cid).Where(e => e.hasCustomerAccount == 1);
                                int FinalCheck = 0;

                                if (GetAllSameEmailCustomer.Count() > 0)
                                {
                                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                    FinalCheck = 1;
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetKYCSameEmailCustomer = context.KYC_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != cid).Where(e => e.hasCustomerAccount == 1);

                                    if (GetKYCSameEmailCustomer.Count() > 0)
                                    {
                                        int CheckCount = 0;
                                        foreach (var Check in GetKYCSameEmailCustomer)
                                        {
                                            if (CheckCount == 0)
                                            {
                                                //check others is pending approval or not, if yes, then cannot go
                                                if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID) != null)
                                                {
                                                    if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID).FirstOrDefault().Status != "Active")
                                                    {
                                                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                        CheckCount = 1;
                                                        FinalCheck = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }



                                if (FinalCheck == 0)
                                {
                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != cid).Where(e => e.hasCustomerAccount == 1);

                                    if (GetTempSameEmailCustomer.Count() > 0)
                                    {

                                        using (var context2 = new DataAccess.GreatEastForex())
                                        {
                                            int CheckCountTemp = 0;
                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                            {
                                                if (CheckCountTemp == 0)
                                                {
                                                    //check others is pending approval or not, if yes, then cannot go
                                                    var checkExist = context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID);
                                                    if (checkExist != null)
                                                    {
                                                        if (checkExist.FirstOrDefault().Status != "Active")
                                                        {
                                                            errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                            CheckCountTemp = 1;
                                                            FinalCheck = 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //This is create form
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var GetAllSameEmailCustomer = context.CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);
                                int FinalCheck = 0;

                                if (GetAllSameEmailCustomer.Count() > 0)
                                {
                                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                    FinalCheck = 1;
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetKYCSameEmailCustomer = context.KYC_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);

                                    if (GetKYCSameEmailCustomer.Count() > 0)
                                    {
                                        int CheckCount = 0;
                                        foreach (var Check in GetKYCSameEmailCustomer)
                                        {
                                            if (CheckCount == 0)
                                            {
                                                //check others is pending approval or not, if yes, then cannot go
                                                if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID) != null)
                                                {
                                                    if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID).FirstOrDefault().Status != "Active")
                                                    {
                                                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                        CheckCount = 1;
                                                        FinalCheck = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Company_Email || e.Natural_Email == customerParticulars.Company_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);

                                    if (GetTempSameEmailCustomer.Count() > 0)
                                    {
                                        using (var context2 = new DataAccess.GreatEastForex())
                                        {
                                            int CheckCountTemp = 0;
                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                            {
                                                if (CheckCountTemp == 0)
                                                {
                                                    //check others is pending approval or not, if yes, then cannot go
                                                    if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID) != null)
                                                    {
                                                        if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID).FirstOrDefault().Status != "Active")
                                                        {
                                                            errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                                                            CheckCountTemp = 1;
                                                            FinalCheck = 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    //if (checkUniqueEmail != null)
                    //{
                    //	if (checkUniqueEmail.ID != cid)
                    //	{
                    //		errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_Email", ErrorMessage = "Email already existed!" });
                    //	}
                    //}
                }
            }

            //if (string.IsNullOrEmpty(customerParticulars.Company_PlaceOfRegistration))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_PlaceOfRegistration", ErrorMessage = "Place of Registration is required!" });
            //}

            //if (customerParticulars.Company_DateOfRegistration == null)
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_DateOfRegistration", ErrorMessage = "Date of Registration is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_RegistrationNo))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_RegistrationNo", ErrorMessage = "Registration No is required!" });
            //}

            //if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntity))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_TypeOfEntity", ErrorMessage = "Type of Entity is required!" });
            //}
            //else
            //{
            //	if (customerParticulars.Company_TypeOfEntity == "Others")
            //	{
            //		if (string.IsNullOrEmpty(customerParticulars.Company_TypeOfEntityIfOthers))
            //		{
            //			errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_TypeOfEntityIfOthers", ErrorMessage = "If Others is required!" });
            //		}
            //	}
            //}

            if (string.IsNullOrEmpty(customerParticulars.Company_PurposeAndIntended))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Company_PurposeAndIntended", ErrorMessage = "Purpose and Intended Nature of Account Relationship is required!" });
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Particular--------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Source of Fund-----------------------------*/
            /*-------------------------------------------------------------------------------------*/

            //if (string.IsNullOrEmpty(form["customerSourceOfFunds.Company_SourceOfFund"]))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Company_SourceOfFund", ErrorMessage = "Source of Funds is required!" });
            //}
            //else
            //{
            //	if (form["customerSourceOfFunds.Company_SourceOfFund"].ToString().Contains("Others"))
            //	{
            //		if (string.IsNullOrEmpty(customerSourceOfFunds.Company_SourceOfFundIfOthers))
            //		{
            //			errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Company_SourceOfFundIfOthers", ErrorMessage = "If Others is required!" });
            //		}
            //	}
            //}

            if (string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Company_PoliticallyExposedIndividuals_1", ErrorMessage = "Please answer this question!" });
            }

            if (string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Company_PoliticallyExposedIndividuals_2", ErrorMessage = "Please answer this question!" });
            }

            if (string.IsNullOrEmpty(customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Company_PoliticallyExposedIndividuals_3", ErrorMessage = "Please answer this question!" });
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Source of Fund----------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Agent Acting-------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
            {
                if (customerActingAgents.ActingAgent == "Yes")
                {
                    if (string.IsNullOrEmpty(customerActingAgents.Company_CustomerType))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Company_CustomerType", ErrorMessage = "Please answer this question!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Company_Address))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Company_Address", ErrorMessage = "Address is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Company_PlaceOfRegistration))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Company_PlaceOfRegistration", ErrorMessage = "Place of Registration is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Company_RegistrationNo))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Company_RegistrationNo", ErrorMessage = "Registration No/Identification No is required!" });
                    }

                    if (customerActingAgents.Company_DateOfRegistration == null)
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Company_DateOfRegistration", ErrorMessage = "Date of Registration/Date of Birth is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Relationship))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Relationship", ErrorMessage = "Relationship between Agent(s) and Client is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.BasisOfAuthority", ErrorMessage = "Basis of Authority is required!" });
                    }
                }
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Agent Acting------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Others-------------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            if (string.IsNullOrEmpty(customerOthers.Grading))
            {
                errors.Add(new ModelStateValidation { Key = "customerOthers.Grading", ErrorMessage = "Grading is required!" });
            }

            if (!FormValidationHelper.IntegerValidation(customerOthers.GMApprovalAbove.ToString()))
            {
                errors.Add(new ModelStateValidation { Key = "customerOthers.GMApprovalAbove", ErrorMessage = "GM Approval Above is not valid!" });
            }
            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Others------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Customer Rates-----------------------------*/
            /*-------------------------------------------------------------------------------------*/

            List<string> keys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

            foreach (string key in keys)
            {
                string id = key.Substring(14);

                if (string.IsNullOrEmpty(form["default-rate-" + id]))
                {
                    //if (string.IsNullOrEmpty(form["CustomBuyRate_" + id]))
                    //{
                    //    errors.Add(new ModelStateValidation { Key = "CustomBuyRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Buy Rate is required!" });
                    //}
                    //else
                    //{
                    //    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomBuyRate_" + id].ToString());

                    //    if (!checkAmount)
                    //    {
                    //        errors.Add(new ModelStateValidation { Key = "CustomBuyRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Buy Rate is not valid!" });
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(form["CustomSellRate_" + id]))
                    //{
                    //    errors.Add(new ModelStateValidation { Key = "CustomSellRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Sell Rate is required!" });
                    //}
                    //else
                    //{
                    //    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomSellRate_" + id].ToString());

                    //    if (!checkAmount)
                    //    {
                    //        errors.Add(new ModelStateValidation { Key = "CustomSellRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Sell Rate is not valid!" });
                    //    }
                    //}

                    if (string.IsNullOrEmpty(form["CustomEncashmentRate_" + id]))
                    {
                        errors.Add(new ModelStateValidation { Key = "CustomEncashmentRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Encashment Rate is required!" });
                    }
                    else
                    {
                        bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomEncashmentRate_" + id].ToString());

                        if (!checkAmount)
                        {
                            errors.Add(new ModelStateValidation { Key = "CustomEncashmentRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Encashment Rate is not valid!" });
                        }
                    }
                }
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Customer Rates----------------------*/
            /*-------------------------------------------------------------------------------------*/

            return errors;
        }

        //Validation for Natural Person
        public List<ModelStateValidation> ValidateModelStateForNaturalPerson(CustomerParticular customerParticulars, CustomerSourceOfFund customerSourceOfFunds, CustomerActingAgent customerActingAgents, CustomerDocumentCheckList customerDocumentChecklists, CustomerOther customerOthers, FormCollection form, int cid = 0)
        {
            List<ModelStateValidation> errors = new List<ModelStateValidation>();

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Particular---------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            if (string.IsNullOrEmpty(customerParticulars.Natural_Name))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Name", ErrorMessage = "Name is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_PermanentAddress))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_PermanentAddress", ErrorMessage = "Permanent Address is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_MailingAddress))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_MailingAddress", ErrorMessage = "Mailing Address is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_Nationality))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Nationality", ErrorMessage = "Nationality is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_ICPassportNo))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_ICPassportNo", ErrorMessage = "IC/Passport No is required!" });
            }

            if (customerParticulars.Natural_DOB == null)
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_DOB", ErrorMessage = "Date of Birth is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_ContactNoH))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_ContactNoH", ErrorMessage = "Contact No (H) is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_ContactNoO))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_ContactNoO", ErrorMessage = "Contact No (O) is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_ContactNoM))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_ContactNoM", ErrorMessage = "Contact No (M) is required!" });
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_Email))
            {
                //errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email is required!" });
            }
            else
            {
                bool checkEmailFormat = FormValidationHelper.EmailValidation(customerParticulars.Natural_Email);

                if (!checkEmailFormat)
                {
                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email is not valid!" });
                }
                else
                {

                    if (customerParticulars.hasCustomerAccount == 1)
                    {
                        if (cid > 0)
                        {
                            //this is edit form
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var GetAllSameEmailCustomer = context.CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.ID != cid).Where(e => e.hasCustomerAccount == 1);
                                int FinalCheck = 0;

                                if (GetAllSameEmailCustomer.Count() > 0)
                                {
                                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                    FinalCheck = 1;
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetKYCSameEmailCustomer = context.KYC_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != cid).Where(e => e.hasCustomerAccount == 1);

                                    if (GetKYCSameEmailCustomer.Count() > 0)
                                    {
                                        int CheckCount = 0;
                                        foreach (var Check in GetKYCSameEmailCustomer)
                                        {
                                            if (CheckCount == 0)
                                            {
                                                //check others is pending approval or not, if yes, then cannot go
                                                if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID) != null)
                                                {
                                                    if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID).FirstOrDefault().Status != "Active")
                                                    {
                                                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                        CheckCount = 1;
                                                        FinalCheck = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.Customer_MainID != cid).Where(e => e.hasCustomerAccount == 1);

                                    if (GetTempSameEmailCustomer.Count() > 0)
                                    {
                                        using (var context2 = new DataAccess.GreatEastForex())
                                        {
                                            int CheckCountTemp = 0;
                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                            {
                                                if (CheckCountTemp == 0)
                                                {
                                                    //check others is pending approval or not, if yes, then cannot go
                                                    if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID) != null)
                                                    {
                                                        if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID).FirstOrDefault().Status != "Active")
                                                        {
                                                            errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                            CheckCountTemp = 1;
                                                            FinalCheck = 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //this is create form
                            using (var context = new DataAccess.GreatEastForex())
                            {
                                var GetAllSameEmailCustomer = context.CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);
                                int FinalCheck = 0;

                                if (GetAllSameEmailCustomer.Count() > 0)
                                {
                                    errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                    FinalCheck = 1;
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetKYCSameEmailCustomer = context.KYC_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);

                                    if (GetKYCSameEmailCustomer.Count() > 0)
                                    {
                                        int CheckCount = 0;
                                        foreach (var Check in GetKYCSameEmailCustomer)
                                        {
                                            if (CheckCount == 0)
                                            {
                                                //check others is pending approval or not, if yes, then cannot go
                                                if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID) != null)
                                                {
                                                    if (context.CustomerOthers.Where(e => e.CustomerParticularId == Check.Customer_MainID).FirstOrDefault().Status != "Active")
                                                    {
                                                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                        CheckCount = 1;
                                                        FinalCheck = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (FinalCheck == 0)
                                {
                                    var GetTempSameEmailCustomer = context.Temp_CustomerParticulars.Where(e => e.Company_Email == customerParticulars.Natural_Email || e.Natural_Email == customerParticulars.Natural_Email && e.IsDeleted == "N").Where(e => e.hasCustomerAccount == 1);

                                    if (GetTempSameEmailCustomer.Count() > 0)
                                    {

                                        using (var context2 = new DataAccess.GreatEastForex())
                                        {
                                            int CheckCountTemp = 0;
                                            foreach (var CheckTemp in GetTempSameEmailCustomer)
                                            {
                                                if (CheckCountTemp == 0)
                                                {
                                                    //check others is pending approval or not, if yes, then cannot go
                                                    if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID) != null)
                                                    {
                                                        if (context2.CustomerOthers.Where(e => e.CustomerParticularId == CheckTemp.Customer_MainID).FirstOrDefault().Status != "Active")
                                                        {
                                                            errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                                                            CheckCountTemp = 1;
                                                            FinalCheck = 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }


                                    }
                                }
                            }
                        }
                    }

                    //CustomerParticular checkUniqueEmail = _customerParticularsModel.FindEmail(customerParticulars.Natural_Email);

                    //if (checkUniqueEmail != null)
                    //{
                    //	if (checkUniqueEmail.ID != cid)
                    //	{
                    //		errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_Email", ErrorMessage = "Email already existed!" });
                    //	}
                    //}
                }
            }

            if (string.IsNullOrEmpty(customerParticulars.Natural_EmploymentType))
            {
                errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_EmploymentType", ErrorMessage = "Please answer this question!" });
            }
            else
            {
                if (customerParticulars.Natural_EmploymentType == "Employed")
                {
                    if (string.IsNullOrEmpty(customerParticulars.Natural_EmployedEmployerName))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_EmployedEmployerName", ErrorMessage = "Name of Employer is required!" });
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Natural_EmployedJobTitle))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_EmployedJobTitle", ErrorMessage = "Job Title is required!" });
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Natural_EmployedRegisteredAddress))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_EmployedRegisteredAddress", ErrorMessage = "Registered Address of Employer is required!" });
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(customerParticulars.Natural_SelfEmployedBusinessName))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_SelfEmployedBusinessName", ErrorMessage = "Name of Business is required!" });
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Natural_SelfEmployedRegistrationNo))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_SelfEmployedRegistrationNo", ErrorMessage = "Business Registration No is required!" });
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Natural_SelfEmployedBusinessAddress))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_SelfEmployedBusinessAddress", ErrorMessage = "Registered Business Address is required!" });
                    }

                    if (string.IsNullOrEmpty(customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerParticulars.Natural_SelfEmployedBusinessPrincipalPlace", ErrorMessage = "Principal Place of Business is required!" });
                    }
                }
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Particular--------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Source of Fund-----------------------------*/
            /*-------------------------------------------------------------------------------------*/

            //if (string.IsNullOrEmpty(form["customerSourceOfFunds.Natural_SourceOfFund"]))
            //{
            //	errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_SourceOfFund", ErrorMessage = "Source of Funds is required!" });
            //}
            //else
            //{
            //	if (form["customerSourceOfFunds.Natural_SourceOfFund"].ToString().Contains("Others"))
            //	{
            //		if (string.IsNullOrEmpty(customerSourceOfFunds.Natural_SourceOfFundIfOthers))
            //		{
            //			errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_SourceOfFundIfOthers", ErrorMessage = "If Others is required!" });
            //		}
            //	}
            //}

            if (string.IsNullOrEmpty(customerSourceOfFunds.Natural_AnnualIncome))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_AnnualIncome", ErrorMessage = "Please answer this question!" });
            }

            if (string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_1", ErrorMessage = "Please answer this question!" });
            }

            if (string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_2", ErrorMessage = "Please answer this question!" });
            }

            if (string.IsNullOrEmpty(customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3))
            {
                errors.Add(new ModelStateValidation { Key = "customerSourceOfFunds.Natural_PoliticallyExposedIndividuals_3", ErrorMessage = "Please answer this question!" });
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Source of Fund----------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Agent Acting-------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            if (!string.IsNullOrEmpty(customerActingAgents.ActingAgent))
            {
                if (customerActingAgents.ActingAgent == "Yes")
                {
                    if (string.IsNullOrEmpty(customerActingAgents.Natural_Name))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Natural_Name", ErrorMessage = "Name is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Natural_PermanentAddress))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Natural_PermanentAddress", ErrorMessage = "Permanent Address is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Natural_Nationality))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Natural_Nationality", ErrorMessage = "Nationality is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Natural_ICPassportNo))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Natural_ICPassportNo", ErrorMessage = "IC/Passport No is required!" });
                    }

                    if (customerActingAgents.Natural_DOB == null)
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Natural_DOB", ErrorMessage = "Date of Birth is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.Relationship))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.Relationship", ErrorMessage = "Relationship between Agent(s) and Client is required!" });
                    }

                    if (string.IsNullOrEmpty(customerActingAgents.BasisOfAuthority))
                    {
                        errors.Add(new ModelStateValidation { Key = "customerActingAgents.BasisOfAuthority", ErrorMessage = "Basis of Authority is required!" });
                    }
                }
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Agent Acting------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Others-------------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            if (string.IsNullOrEmpty(customerOthers.Grading))
            {
                errors.Add(new ModelStateValidation { Key = "customerOthers.Grading", ErrorMessage = "Grading is required!" });
            }

            if (!FormValidationHelper.IntegerValidation(customerOthers.GMApprovalAbove.ToString()))
            {
                errors.Add(new ModelStateValidation { Key = "customerOthers.GMApprovalAbove", ErrorMessage = "GM Approval Above is not valid!" });
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Others------------------------------*/
            /*-------------------------------------------------------------------------------------*/

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------Customer Customer Rates-----------------------------*/
            /*-------------------------------------------------------------------------------------*/

            List<string> keys = form.AllKeys.Where(e => e.Contains("CustomProduct_")).ToList();

            foreach (string key in keys)
            {
                string id = key.Substring(14);

                if (string.IsNullOrEmpty(form["default-rate-" + id]))
                {
                    //if (string.IsNullOrEmpty(form["CustomBuyRate_" + id]))
                    //{
                    //    errors.Add(new ModelStateValidation { Key = "CustomBuyRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Buy Rate is required!" });
                    //}
                    //else
                    //{
                    //    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomBuyRate_" + id].ToString());

                    //    if (!checkAmount)
                    //    {
                    //        errors.Add(new ModelStateValidation { Key = "CustomBuyRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Buy Rate is not valid!" });
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(form["CustomSellRate_" + id]))
                    //{
                    //    errors.Add(new ModelStateValidation { Key = "CustomSellRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Sell Rate is required!" });
                    //}
                    //else
                    //{
                    //    bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomSellRate_" + id].ToString());

                    //    if (!checkAmount)
                    //    {
                    //        errors.Add(new ModelStateValidation { Key = "CustomSellRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Sell Rate is not valid!" });
                    //    }
                    //}

                    if (string.IsNullOrEmpty(form["CustomEncashmentRate_" + id]))
                    {
                        errors.Add(new ModelStateValidation { Key = "CustomEncashmentRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Encashment Rate is required!" });
                    }
                    else
                    {
                        bool checkAmount = FormValidationHelper.NonNegativeAmountValidation(form["CustomEncashmentRate_" + id].ToString());

                        if (!checkAmount)
                        {
                            errors.Add(new ModelStateValidation { Key = "CustomEncashmentRate_" + id, ErrorMessage = form["CustomProductCode_" + id].ToString() + "'s Encashment Rate is not valid!" });
                        }
                    }
                }
            }

            /*-------------------------------------------------------------------------------------*/
            /*---------------------------------End of Customer Customer Rates----------------------*/
            /*-------------------------------------------------------------------------------------*/

            return errors;
        }

        //GET: AddPersonnel [DL, FN, IV, OM, CS, GM, SA]
        public ActionResult AddPersonnel(string fullname, string icpassport, string nationality, string jobtitle, int count = 1)
        {
            ViewData["FullName"] = fullname;
            ViewData["ICPassport"] = icpassport;
            ViewData["Nationality"] = nationality;
            ViewData["JobTitle"] = jobtitle;
            ViewData["Count"] = count;
            return View();
        }

        //POST: AddScreeningReport
        [HttpPost]
        public ActionResult AddScreeningReport(int rowId, string date, string dateOfAcra, string screenedBy, string screeningReport_1, string screeningReport_2, string remarks)
        {
            ViewData["RowId"] = rowId;
            ViewData["Date"] = date;
            ViewData["DateOfAcra"] = dateOfAcra;
            ViewData["ScreenedBy"] = screenedBy;
            ViewData["ScreeningReport_1"] = screeningReport_1;
            ViewData["ScreeningReport_2"] = screeningReport_2;
            ViewData["Remarks"] = remarks;
            return View();
        }

        //POST: AddActivityLog
        [HttpPost]
        public ActionResult AddActivityLog(int rowId, string title, string activityDateTime, string Note, string CustomerID)
        {

            CustomerActivityLog log = new CustomerActivityLog();
            responseActivityLog data2 = new responseActivityLog();

            data2.success = false;
            int checkError = 0;
            try
            {
                if (!string.IsNullOrEmpty(CustomerID))
                {

                    if (string.IsNullOrEmpty(title))
                    {
                        data2.error = "Title is required.";
                        checkError = 1;
                    }

                    if (string.IsNullOrEmpty(activityDateTime))
                    {
                        data2.error = "Activity Log DateTime is required.";
                        checkError = 1;
                    }
                    else
                    {
                        try
                        {
                            DateTime date = Convert.ToDateTime(activityDateTime);
                        }
                        catch
                        {
                            data2.error = "Activity Log DateTime is not valid.";
                            checkError = 1;
                        }
                    }

                    if (string.IsNullOrEmpty(Note))
                    {
                        data2.error = "Note is required.";
                        checkError = 1;
                    }

                    if (checkError == 0)
                    {
                        CustomerActivityLog activityLogs = new CustomerActivityLog();
                        data2.log = new CustomerActivityLog();

                        activityLogs.CustomerParticularId = Convert.ToInt32(CustomerID);
                        activityLogs.Title = title;
                        activityLogs.ActivityLog_DateTime = Convert.ToDateTime(activityDateTime);
                        activityLogs.ActivityLog_Note = Note;

                        bool result_activityLogs = _customerActivityLogsModel.Add(activityLogs);

                        if (result_activityLogs)
                        {
                            data2.log = activityLogs;
                            data2.ActivityTime = activityDateTime;
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Created Customer Activity Logs");
                            data2.success = true;
                        }
                        //End of Activity Log
                    }
                }
                else
                {
                    data2.error = "Invalid Customer ID";
                }
            }
            catch
            {
                data2.error = "Something went wrong when adding activity log into database.";
            }

            return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);

            //ViewData["RowId"] = rowId;
            //ViewData["title"] = title;
            //ViewData["activityDateTime"] = activityDateTime;
            //ViewData["Note"] = Note;
            //return View();
        }

        //POST: DeleteActivityLog
        [HttpPost]
        public ActionResult DeleteActivityLog(int rowId)
        {
            responseActivityLog data2 = new responseActivityLog();
            data2.success = false;
            try
            {
                if (rowId > 0)
                {
                    CustomerActivityLog getSingle = _customerActivityLogsModel.GetSingle(rowId);

                    if (getSingle != null)
                    {
                        bool DeleteActivityLog = _customerActivityLogsModel.Delete(rowId);

                        if (DeleteActivityLog)
                        {
                            AuditLogHelper.WriteAuditLog(Convert.ToInt32(Session["UserId"]), "CustomerActivityLogs", Session["UserRole"].ToString() + " [" + Session["UserName"].ToString() + "] Deleted Customer Activity Logs.");
                            data2.success = true;
                        }
                    }
                    else
                    {
                        data2.error = "Activity Log not found.";
                    }
                }
                else
                {
                    data2.error = "Invalid Activity Log ID";
                }
            }
            catch
            {
                data2.error = "Something went wrong when delete activity log.";
            }

            return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);

        }

        //GET: DownloadCustomerForm
        public ActionResult DownloadCustomerForm(int id)
        {
            CustomerParticular customerParticulars = _customerParticularsModel.GetSingle(id);
            ViewData["CustomerParticular"] = customerParticulars;

            if (customerParticulars.CustomerType == "Corporate & Trading Company")
            {
                string header = Server.MapPath("~/Views/Customer/CorporateCompanyFormHeader.html");
                string footer = Server.MapPath("~/Views/Customer/CorporateCompanyFormFooter.html");
                string customSwitch = string.Format("--header-html \"{0}\" " + "--header-spacing \"0\" " + "--footer-html \"{1}\" " + "--footer-spacing \"0\" ", header, footer);

                return new Rotativa.ViewAsPdf("CorporateCompanyFormPDF")
                {
                    FileName = customerParticulars.CustomerCode + "-customer-form-" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf",
                    PageSize = Size.A4,
                    CustomSwitches = customSwitch
                };
            }
            else
            {
                string header = Server.MapPath("~/Views/Customer/NaturalPersonFormHeader.html");
                string footer = Server.MapPath("~/Views/Customer/NaturalPersonFormFooter.html");
                string customSwitch = string.Format("--header-html \"{0}\" " + "--header-spacing \"0\" " + "--footer-html \"{1}\" " + "--footer-spacing \"0\" ", header, footer);

                return new Rotativa.ViewAsPdf("NaturalPersonFormPDF")
                {
                    FileName = customerParticulars.CustomerCode + "-customer-form-" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf",
                    PageSize = Size.A4,
                    CustomSwitches = customSwitch
                };
            }
        }

        [HttpPost]
        public ActionResult LoadCustomerRecord(int customerid)
        {
            responseLoadCustomerRecord data2 = new responseLoadCustomerRecord();

            data2.success = false;

            try
            {
                using (var context = new DataAccess.GreatEastForex())
                {
                    var GetCustomer = context.CustomerParticulars.Where(e => e.ID == customerid).FirstOrDefault();

                    if (GetCustomer != null)
                    {
                        if (GetCustomer.Others.FirstOrDefault().Status == "Pending Approval")
                        {
                            if (GetCustomer.isKYCVerify == 0)
                            {
                                data2.customerRecord = new CustomerParticular();
                                data2.success = true;

                                data2.customerRecord.CustomerType = GetCustomer.CustomerType;
                                data2.customerRecord.Company_RegisteredName = GetCustomer.Company_RegisteredName;
                                data2.customerRecord.Company_BusinessAddress1 = GetCustomer.Company_BusinessAddress1;
                                data2.customerRecord.Company_BusinessAddress2 = GetCustomer.Company_BusinessAddress2;
                                data2.customerRecord.Company_BusinessAddress3 = GetCustomer.Company_BusinessAddress3;
                                data2.customerRecord.Company_PostalCode = GetCustomer.Company_PostalCode;
                                data2.customerRecord.Shipping_Address1 = GetCustomer.Shipping_Address1;
                                data2.customerRecord.Shipping_Address2 = GetCustomer.Shipping_Address2;
                                data2.customerRecord.Shipping_Address3 = GetCustomer.Shipping_Address3;
                                data2.customerRecord.Shipping_PostalCode = GetCustomer.Shipping_PostalCode;
                                data2.customerRecord.Company_Country = GetCustomer.Company_Country;
                                data2.customerRecord.Company_CountryCode = GetCustomer.Company_CountryCode;
                                data2.customerRecord.Company_TelNo = GetCustomer.Company_TelNo;
                                data2.customerRecord.Company_FaxNo = GetCustomer.Company_FaxNo;
                                data2.customerRecord.Company_PlaceOfRegistration = GetCustomer.Company_PlaceOfRegistration;
                                data2.customerRecord.Company_DateOfRegistration = GetCustomer.Company_DateOfRegistration;
                                data2.customerRecord.Company_RegistrationNo = GetCustomer.Company_RegistrationNo;
                                data2.customerRecord.Company_TypeOfEntity = GetCustomer.Company_TypeOfEntity;
                                data2.customerRecord.Company_TypeOfEntityIfOthers = GetCustomer.Company_TypeOfEntityIfOthers;
                                data2.customerRecord.EnableTransactionType = GetCustomer.EnableTransactionType;
                            }
                            else
                            {
                                if (GetCustomer.lastTempPageUpdate >= 0 && GetCustomer.lastTempPageUpdate <= 4)
                                {
                                    var CheckTempRecord = context.Temp_CustomerParticulars.Where(e => e.Customer_MainID == GetCustomer.ID).FirstOrDefault();

                                    data2.customerRecord = new CustomerParticular();
                                    data2.success = true;

                                    data2.customerRecord.CustomerType = CheckTempRecord.CustomerType;
                                    data2.customerRecord.Company_RegisteredName = CheckTempRecord.Company_RegisteredName;
                                    data2.customerRecord.Company_BusinessAddress1 = CheckTempRecord.Company_BusinessAddress1;
                                    data2.customerRecord.Company_BusinessAddress2 = CheckTempRecord.Company_BusinessAddress2;
                                    data2.customerRecord.Company_BusinessAddress3 = CheckTempRecord.Company_BusinessAddress3;
                                    data2.customerRecord.Company_PostalCode = CheckTempRecord.Company_PostalCode;
                                    data2.customerRecord.Shipping_Address1 = CheckTempRecord.Shipping_Address1;
                                    data2.customerRecord.Shipping_Address2 = CheckTempRecord.Shipping_Address2;
                                    data2.customerRecord.Shipping_Address3 = CheckTempRecord.Shipping_Address3;
                                    data2.customerRecord.Shipping_PostalCode = CheckTempRecord.Shipping_PostalCode;
                                    data2.customerRecord.Company_Country = CheckTempRecord.Company_Country;
                                    data2.customerRecord.Company_CountryCode = CheckTempRecord.Company_CountryCode;
                                    data2.customerRecord.Company_TelNo = CheckTempRecord.Company_TelNo;
                                    data2.customerRecord.Company_FaxNo = CheckTempRecord.Company_FaxNo;
                                    data2.customerRecord.Company_PlaceOfRegistration = CheckTempRecord.Company_PlaceOfRegistration;
                                    data2.customerRecord.Company_DateOfRegistration = CheckTempRecord.Company_DateOfRegistration;
                                    data2.customerRecord.Company_RegistrationNo = CheckTempRecord.Company_RegistrationNo;
                                    data2.customerRecord.Company_TypeOfEntity = CheckTempRecord.Company_TypeOfEntity;
                                    data2.customerRecord.Company_TypeOfEntityIfOthers = CheckTempRecord.Company_TypeOfEntityIfOthers;
                                    data2.customerRecord.EnableTransactionType = CheckTempRecord.EnableTransactionType;
                                }
                                else if (GetCustomer.lastTempPageUpdate == 5)
                                {
                                    data2.customerRecord = new CustomerParticular();
                                    data2.success = true;

                                    data2.customerRecord.CustomerType = GetCustomer.CustomerType;
                                    data2.customerRecord.Company_RegisteredName = GetCustomer.Company_RegisteredName;
                                    data2.customerRecord.Company_BusinessAddress1 = GetCustomer.Company_BusinessAddress1;
                                    data2.customerRecord.Company_BusinessAddress2 = GetCustomer.Company_BusinessAddress2;
                                    data2.customerRecord.Company_BusinessAddress3 = GetCustomer.Company_BusinessAddress3;
                                    data2.customerRecord.Company_PostalCode = GetCustomer.Company_PostalCode;
                                    data2.customerRecord.Shipping_Address1 = GetCustomer.Shipping_Address1;
                                    data2.customerRecord.Shipping_Address2 = GetCustomer.Shipping_Address2;
                                    data2.customerRecord.Shipping_Address3 = GetCustomer.Shipping_Address3;
                                    data2.customerRecord.Shipping_PostalCode = GetCustomer.Shipping_PostalCode;
                                    data2.customerRecord.Company_Country = GetCustomer.Company_Country;
                                    data2.customerRecord.Company_CountryCode = GetCustomer.Company_CountryCode;
                                    data2.customerRecord.Company_TelNo = GetCustomer.Company_TelNo;
                                    data2.customerRecord.Company_FaxNo = GetCustomer.Company_FaxNo;
                                    data2.customerRecord.Company_PlaceOfRegistration = GetCustomer.Company_PlaceOfRegistration;
                                    data2.customerRecord.Company_DateOfRegistration = GetCustomer.Company_DateOfRegistration;
                                    data2.customerRecord.Company_RegistrationNo = GetCustomer.Company_RegistrationNo;
                                    data2.customerRecord.Company_TypeOfEntity = GetCustomer.Company_TypeOfEntity;
                                    data2.customerRecord.Company_TypeOfEntityIfOthers = GetCustomer.Company_TypeOfEntityIfOthers;
                                    data2.customerRecord.EnableTransactionType = GetCustomer.EnableTransactionType;
                                }
                                else
                                {
                                    data2.customerRecord = new CustomerParticular();
                                    data2.success = true;

                                    data2.customerRecord.CustomerType = GetCustomer.CustomerType;
                                    data2.customerRecord.Company_RegisteredName = GetCustomer.Company_RegisteredName;
                                    data2.customerRecord.Company_BusinessAddress1 = GetCustomer.Company_BusinessAddress1;
                                    data2.customerRecord.Company_BusinessAddress2 = GetCustomer.Company_BusinessAddress2;
                                    data2.customerRecord.Company_BusinessAddress3 = GetCustomer.Company_BusinessAddress3;
                                    data2.customerRecord.Company_PostalCode = GetCustomer.Company_PostalCode;
                                    data2.customerRecord.Shipping_Address1 = GetCustomer.Shipping_Address1;
                                    data2.customerRecord.Shipping_Address2 = GetCustomer.Shipping_Address2;
                                    data2.customerRecord.Shipping_Address3 = GetCustomer.Shipping_Address3;
                                    data2.customerRecord.Shipping_PostalCode = GetCustomer.Shipping_PostalCode;
                                    data2.customerRecord.Company_Country = GetCustomer.Company_Country;
                                    data2.customerRecord.Company_CountryCode = GetCustomer.Company_CountryCode;
                                    data2.customerRecord.Company_TelNo = GetCustomer.Company_TelNo;
                                    data2.customerRecord.Company_FaxNo = GetCustomer.Company_FaxNo;
                                    data2.customerRecord.Company_PlaceOfRegistration = GetCustomer.Company_PlaceOfRegistration;
                                    data2.customerRecord.Company_DateOfRegistration = GetCustomer.Company_DateOfRegistration;
                                    data2.customerRecord.Company_RegistrationNo = GetCustomer.Company_RegistrationNo;
                                    data2.customerRecord.Company_TypeOfEntity = GetCustomer.Company_TypeOfEntity;
                                    data2.customerRecord.Company_TypeOfEntityIfOthers = GetCustomer.Company_TypeOfEntityIfOthers;
                                    data2.customerRecord.EnableTransactionType = GetCustomer.EnableTransactionType;
                                }
                            }
                        }
                        else
                        {
                            data2.customerRecord = new CustomerParticular();
                            data2.success = true;

                            data2.customerRecord.CustomerType = GetCustomer.CustomerType;
                            data2.customerRecord.Company_RegisteredName = GetCustomer.Company_RegisteredName;
                            data2.customerRecord.Company_BusinessAddress1 = GetCustomer.Company_BusinessAddress1;
                            data2.customerRecord.Company_BusinessAddress2 = GetCustomer.Company_BusinessAddress2;
                            data2.customerRecord.Company_BusinessAddress3 = GetCustomer.Company_BusinessAddress3;
                            data2.customerRecord.Company_PostalCode = GetCustomer.Company_PostalCode;
                            data2.customerRecord.Shipping_Address1 = GetCustomer.Shipping_Address1;
                            data2.customerRecord.Shipping_Address2 = GetCustomer.Shipping_Address2;
                            data2.customerRecord.Shipping_Address3 = GetCustomer.Shipping_Address3;
                            data2.customerRecord.Shipping_PostalCode = GetCustomer.Shipping_PostalCode;
                            data2.customerRecord.Company_Country = GetCustomer.Company_Country;
                            data2.customerRecord.Company_CountryCode = GetCustomer.Company_CountryCode;
                            data2.customerRecord.Company_TelNo = GetCustomer.Company_TelNo;
                            data2.customerRecord.Company_FaxNo = GetCustomer.Company_FaxNo;
                            data2.customerRecord.Company_PlaceOfRegistration = GetCustomer.Company_PlaceOfRegistration;
                            data2.customerRecord.Company_DateOfRegistration = GetCustomer.Company_DateOfRegistration;
                            data2.customerRecord.Company_RegistrationNo = GetCustomer.Company_RegistrationNo;
                            data2.customerRecord.Company_TypeOfEntity = GetCustomer.Company_TypeOfEntity;
                            data2.customerRecord.Company_TypeOfEntityIfOthers = GetCustomer.Company_TypeOfEntityIfOthers;
                            data2.customerRecord.EnableTransactionType = GetCustomer.EnableTransactionType;
                        }
                    }
                    else
                    {
                        data2.error = "Customer record not found.";
                    }

                    return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                data2.error = "Something went wrong when retriving customer record.";
                return Json(new { res = data2 }, JsonRequestBehavior.AllowGet);
            }


        }

        //CustomerType Dropdown
        public Dropdown[] CustomerTypeDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Corporate & Trading Company", val = "Corporate & Trading Company" };
            ddl[1] = new Dropdown { name = "Natural Person", val = "Natural Person" };
            return ddl;
        }

        //TypeOfEntity Dropdown
        public Dropdown[] TypeOfEntityDDL()
        {
            Dropdown[] ddl = new Dropdown[7];
            ddl[0] = new Dropdown { name = "Company", val = "Company" };
            ddl[1] = new Dropdown { name = "Sole Proprietorship", val = "Sole Proprietorship" };
            ddl[2] = new Dropdown { name = "Partnership", val = "Partnership" };
            ddl[3] = new Dropdown { name = "Limited Liability Partnership", val = "Limited Liability Partnership" };
            ddl[4] = new Dropdown { name = "Express Trust", val = "Express Trust" };
            ddl[5] = new Dropdown { name = "Other Legal Arrangement", val = "Other Legal Arrangement" };
            ddl[6] = new Dropdown { name = "Others", val = "Others" };
            return ddl;
        }

        //Status Dropdown
        public Dropdown[] StatusDDL()
        {
            Dropdown[] ddl = new Dropdown[3];
            ddl[0] = new Dropdown { name = "Active", val = "Active" };
            ddl[1] = new Dropdown { name = "Suspended", val = "Suspended" };
            ddl[2] = new Dropdown { name = "Rejected", val = "Rejected" };
            return ddl;
        }

        //Has Customer Account Dropdown
        public Dropdown[] HasCustomerAccountDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Yes", val = "1" };
            ddl[1] = new Dropdown { name = "No", val = "0" };
            return ddl;
        }

        //Is Main Account Dropdown
        public Dropdown[] IsMainAccountDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Yes", val = "0" };
            ddl[1] = new Dropdown { name = "No", val = "1" };
            return ddl;
        }

        //Main Account Customer Dropdown
        public Dropdown[] MainAccountCustomerDDL(long id = 0)
        {
            List<CustomerParticular> MainAccountCustomerList = _customerParticularsModel.GetAllMainCustomer();

            if (MainAccountCustomerList.Count > 0)
            {
                int count = 0;

                if (id > 0)
                {
                    if (MainAccountCustomerList.Where(e => e.ID == id).FirstOrDefault() == null)
                    {
                        CustomerParticular SingleItemInList = new CustomerParticular();

                        //means is in pending approval
                        int ConvertID = Convert.ToInt32(id);

                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var GetSingle = context.CustomerParticulars.Where(e => e.ID == ConvertID).FirstOrDefault();
                            var CheckStatus = GetSingle.Others.FirstOrDefault().Status;

                            if (CheckStatus == "Pending Approval")
                            {
                                //pending approval
                                //get Temp Record
                                var TempRecord = context.Temp_CustomerParticulars.Where(e => e.Customer_MainID == GetSingle.ID).FirstOrDefault();

                                if (TempRecord != null)
                                {
                                    SingleItemInList.Company_RegisteredName = TempRecord.Company_RegisteredName;
                                    SingleItemInList.Natural_Name = TempRecord.Natural_Name;
                                    SingleItemInList.ID = GetSingle.ID;
                                }
                                else
                                {
                                    SingleItemInList.Company_RegisteredName = GetSingle.Company_RegisteredName;
                                    SingleItemInList.Natural_Name = GetSingle.Natural_Name;
                                    SingleItemInList.ID = GetSingle.ID;
                                }
                            }
                            else
                            {
                                //rejected
                                //get CustomerParticular Record
                                SingleItemInList.Company_RegisteredName = GetSingle.Company_RegisteredName;
                                SingleItemInList.Natural_Name = GetSingle.Natural_Name;
                                SingleItemInList.ID = GetSingle.ID;
                            }
                        }

                        Dropdown[] ddl = new Dropdown[MainAccountCustomerList.Count + 1];
                        ddl[0] = new Dropdown { name = (string.IsNullOrEmpty(SingleItemInList.Company_RegisteredName) ? SingleItemInList.Natural_Name : SingleItemInList.Company_RegisteredName), val = SingleItemInList.ID.ToString() };
                        count = 1;

                        foreach (CustomerParticular cus in MainAccountCustomerList)
                        {
                            ddl[count] = new Dropdown { name = (string.IsNullOrEmpty(cus.Company_RegisteredName) ? cus.Natural_Name : cus.Company_RegisteredName), val = cus.ID.ToString() };
                            count++;
                        }
                        return ddl;
                    }
                    else
                    {
                        Dropdown[] ddl = new Dropdown[MainAccountCustomerList.Count];

                        foreach (CustomerParticular cus in MainAccountCustomerList)
                        {
                            ddl[count] = new Dropdown { name = (string.IsNullOrEmpty(cus.Company_RegisteredName) ? cus.Natural_Name : cus.Company_RegisteredName), val = cus.ID.ToString() };
                            count++;
                        }
                        return ddl;
                    }
                }
                else
                {
                    Dropdown[] ddl = new Dropdown[MainAccountCustomerList.Count];

                    foreach (CustomerParticular cus in MainAccountCustomerList)
                    {
                        ddl[count] = new Dropdown { name = (string.IsNullOrEmpty(cus.Company_RegisteredName) ? cus.Natural_Name : cus.Company_RegisteredName), val = cus.ID.ToString() };
                        count++;
                    }
                    return ddl;
                }
            }
            else
            {
                Dropdown[] ddl = new Dropdown[0];
                return ddl;
            }
        }

        //Status Quick UpdateDropdown
        public Dropdown[] StatusQuickUpdateDDL()
        {
            Dropdown[] ddl = new Dropdown[4];
            ddl[0] = new Dropdown { name = "Active", val = "Active" };
            ddl[1] = new Dropdown { name = "Suspended", val = "Suspended" };
            ddl[2] = new Dropdown { name = "Rejected", val = "Rejected" };
            ddl[3] = new Dropdown { name = "Unapproved", val = "Unapproved" };
            return ddl;
        }

        //Customer Email Verify Dropdown
        public Dropdown[] CustomerEmailVerifyDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Yes", val = "1" };
            ddl[1] = new Dropdown { name = "No", val = "0" };
            return ddl;
        }

        //Customer Title Dropdown
        public Dropdown[] CustomerTitleDDL()
        {
            Dropdown[] ddl = new Dropdown[5];
            ddl[0] = new Dropdown { name = "Mr", val = "Mr" };
            ddl[1] = new Dropdown { name = "Ms", val = "Ms" };
            ddl[2] = new Dropdown { name = "Mrs", val = "Mrs" };
            ddl[3] = new Dropdown { name = "Mdm", val = "Mdm" };
            ddl[4] = new Dropdown { name = "Dr", val = "Dr" };
            return ddl;
        }

        //Grading Dropdown
        public Dropdown[] GradingDDL()
        {
            Dropdown[] ddl = new Dropdown[3];
            ddl[0] = new Dropdown { name = "High", val = "High" };
            ddl[1] = new Dropdown { name = "Medium", val = "Medium" };
            ddl[2] = new Dropdown { name = "Low", val = "Low" };
            return ddl;
        }

        //ScreeningResults Dropdown
        public Dropdown[] ScreeningResultsDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Positive", val = "Positive" };
            ddl[1] = new Dropdown { name = "Negative", val = "Negative" };
            return ddl;
        }

        //Customer Profile Dropdown
        public Dropdown[] CustomerProfileDDL()
        {
            Dropdown[] ddl = new Dropdown[2];
            ddl[0] = new Dropdown { name = "Complete", val = "Complete" };
            ddl[1] = new Dropdown { name = "Incomplete", val = "Incomplete" };
            return ddl;
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

                                        //if (mimeType == "application/pdf")
                                        //{
                                        //    oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), "ori_" + newFileName);

                                        //    attachment.SaveAs(oriPath);

                                        //    string resizedPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

                                        //    int maxWidth = int.Parse(ConfigurationManager.AppSettings["MaxImgWidth"].ToString());
                                        //    int maxHeight = int.Parse(ConfigurationManager.AppSettings["MaxImgHeight"].ToString());

                                        //    int width = 0;
                                        //    int height = 0;

                                        //    using (System.Drawing.Image Img = System.Drawing.Image.FromFile(oriPath))
                                        //    {
                                        //        width = Img.Width;
                                        //        height = Img.Height;
                                        //    }

                                        //    if (width >= maxWidth || height >= maxHeight)
                                        //    {
                                        //        ImageResizer.ImageJob i = new ImageResizer.ImageJob(oriPath, resizedPath, new ImageResizer.ResizeSettings(
                                        //   "width=" + maxWidth + ";height=" + maxHeight + ";format=jpg;mode=pad"));//mode=null, max, pad(default), crop, carve, stretch

                                        //        i.Build();
                                        //    }
                                        //    else
                                        //    {
                                        //        ImageResizer.ImageJob i = new ImageResizer.ImageJob(oriPath, resizedPath, new ImageResizer.ResizeSettings(
                                        //   "width=" + maxWidth + ";height=" + maxHeight + ";format=jpg;scale=canvas"));

                                        //        i.Build();
                                        //    }

                                        //    System.IO.File.Delete(oriPath);
                                        //}
                                        //else
                                        //{
                                        //    oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

                                        //    attachment.SaveAs(oriPath);
                                        //}

                                        oriPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"].ToString()), newFileName);

                                        attachment.SaveAs(oriPath);

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
    }

    public class RedirectingActionWithDLFNIVOMCSGMSA : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userRole = HttpContext.Current.Session["UserRole"].ToString();
            string[] userRoleList = userRole.Split(',');

            //Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "Inventory") >= 0 || Array.IndexOf(userRoleList, "Ops Exec") >= 0 || Array.IndexOf(userRoleList, "Cashier") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Customer Viewer") >= 0
            if (!(Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Finance") >= 0 || Array.IndexOf(userRoleList, "Inventory") >= 0 || Array.IndexOf(userRoleList, "Ops Exec") >= 0 || Array.IndexOf(userRoleList, "Cashier") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Customer Viewer") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0))
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

    public class RedirectingActionWithDLGMSA : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userRole = HttpContext.Current.Session["UserRole"].ToString();
            string[] userRoleList = userRole.Split(',');

            //if (Array.IndexOf(userRoleList, "Finance") >= 0)
            //Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Customer Viewer") >= 0
            if (!(Array.IndexOf(userRoleList, "Dealer") >= 0 || Array.IndexOf(userRoleList, "Super Admin") >= 0 || Array.IndexOf(userRoleList, "General Manager") >= 0 || Array.IndexOf(userRoleList, "Sales Admin") >= 0))
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

    //This is Json Return
    public class responseActivityLog
    {
        public bool success { get; set; }

        public string error { get; set; }

        public CustomerActivityLog log { get; set; }

        public string ActivityTime { get; set; }
    }

    //This is Json Return
    public class responseLoadCustomerRecord
    {
        public bool success { get; set; }

        public string error { get; set; }

        public CustomerParticular customerRecord { get; set; }
    }
}