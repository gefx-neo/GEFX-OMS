using GreatEastForex.Helper;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DataAccess.POCO;
using System.Collections.Specialized;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using DataAccess.Artemis;
using Microsoft.Owin.Security.Twitter.Messages;
using System.Web.Helpers;
using RestSharp;
using System.Windows.Input;
using System.Threading;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace GreatEastForex.Controllers
{
    [HandleError]
    public class AccessController : Controller
    {
        private IUserRepository _usersModel;
        public static string GetAccessToken;
        public int MaxRetries = 3;

        public AccessController()
            : this(new UserRepository())
        {

        }

        public AccessController(IUserRepository usersModel)
        {
            _usersModel = usersModel;
        }

        //Start RestSharp
        //Get Bearer Token
        //public Auth.Result GetToken()
        //{
        //    Auth.Result objToken = new Auth.Result();
        //    try
        //    {
        //        Auth.GetBody p = new Auth.GetBody
        //        {
        //            username = WebConfigurationManager.AppSettings["Artemis_username"],
        //            password = WebConfigurationManager.AppSettings["Artemis_password"],
        //            clientId = WebConfigurationManager.AppSettings["Artemis_clientid"],
        //            userPoolId = WebConfigurationManager.AppSettings["Artemis_userpoolid"]
        //        };

        //        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_getTokenUrl"], Method.POST);
        //        request.AddJsonBody(p);
        //        IRestResponse response = client.Execute(request);
        //        if (response != null && !string.IsNullOrEmpty(response.Content))
        //        {
        //            objToken = JsonConvert.DeserializeObject<Auth.Result>(response.Content);
        //            GetAccessToken = objToken.AuthenticationResult.AccessToken;
        //            Console.Write("Success");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //    }
        //    return objToken;
        //}

        ////Refresh Token
        //public void RefreshTokenRest()
        //{
        //    Auth.Result objToken = GetToken();
        //    Auth.Result newObjToken = new Auth.Result();
        //    try
        //    {
        //        Auth.RefreshTokenParam p = new Auth.RefreshTokenParam
        //        {
        //            clientId = WebConfigurationManager.AppSettings["Artemis_clientid"],
        //            refreshToken = objToken.AuthenticationResult.RefreshToken
        //        };

        //        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_refreshTokenUrl"], Method.POST);
        //        request.AddJsonBody(p);
        //        IRestResponse response = client.Execute(request);
        //        if (response != null && !string.IsNullOrEmpty(response.Content))
        //        {
        //            var check = response.Content;
        //            newObjToken = JsonConvert.DeserializeObject<Auth.Result>(response.Content);
        //            Console.Write("Success");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //    }
        //}

        //public void ListCustomerRest()
        //{
        //    ListCustomers listCustomers = new ListCustomers();
        //    try
        //    {
        //        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"], Method.GET);

        //        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        IRestResponse response = client.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var check = response.Content;
        //            listCustomers = JsonConvert.DeserializeObject<ListCustomers>(response.Content);
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //            if (!string.IsNullOrEmpty(check2.detail))
        //            {
        //                if (MaxRetries != 0)
        //                {
        //                    if (check2.detail.ToString() == "The token provided is invalid")
        //                    {
        //                        //rerun the Access Token
        //                        MaxRetries--;
        //                        GetToken();
        //                        ListCustomerRest();
        //                    }
        //                    else
        //                    {
        //                        //Other failed reason
        //                        Console.Write(check2.detail.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write(MaxRetries);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //    }
        //}

        //public SingleCustomerByID GetSingleCustomerByIDRest(int id)
        //{
        //    Auth.Result objToken = Test();
        //    SingleCustomerByID SingleCustomerByID = new SingleCustomerByID();

        //    if (id > 0)
        //    {
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id, Method.GET);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                SingleCustomerByID = JsonConvert.DeserializeObject<SingleCustomerByID>(response.Content);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            GetSingleCustomerByIDRest(id);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else 
        //    {
        //        Console.WriteLine("ID Cannot be null or 0.");
        //    }

        //    return SingleCustomerByID;
        //}

        //public void CreateCustomerRest()
        //{
        //    Auth.Result objToken = GetToken();
        //    string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //    string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //    string getCustomerType = "CORPORATE";

        //    SingleCustomerByID customer = new SingleCustomerByID();

        //    customer.onboardingMode = "FACE-TO-FACE";
        //    customer.productServiceComplexity = "SIMPLE";
        //    customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //    customer.profileType = "";
        //    customer.isActiveCustomer = true;
        //    customer.natureOfBusinessRelationship = "";
        //    customer.referenceId = "";
        //    customer.domains = new int[] { 36 };
        //    customer.users = new string[0];

        //    if (getCustomerType == "INDIVIDUAL")
        //    {
        //        customer.individualRecords = new List<individualRecords>();
        //        individualRecords individual = new individualRecords();

        //        individual.primary = true;
        //        individual.name = "tds name 2";
        //        individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //        individual.phoneNumbers = new string[] { "123", "456" };
        //        individual.addresses = new string[] { "add 1", "add 2" };
        //        individual.sourceOfFunds = "SALARY";
        //        individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        individual.gender = "MALE";
        //        individual.nationality = "SINGAPORE";
        //        individual.countryOfResidence = "SINGAPORE";
        //        individual.title = "MR";
        //        individual.countryOfBirth = "SINGAPORE";
        //        individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //        individual.occupation = "ACCOUNTANT";
        //        individual.idType = "idType";
        //        individual.idNumber = "idNumber";
        //        individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");

        //        customer.individualRecords.Add(individual);
        //    }
        //    else
        //    {
        //        //This is Corporate Type
        //        customer.corporateRecords = new List<corporateRecords>();
        //        corporateRecords corporate = new corporateRecords();

        //        corporate.primary = true;
        //        corporate.name = "tds name 2";
        //        corporate.aliasNames = new string[] { "tds 1", "tds 2" };
        //        corporate.phoneNumbers = new string[] { "123", "456" };
        //        corporate.addresses = new string[] { "add 1", "add 2" };
        //        corporate.sourceOfFunds = "SALARY";
        //        corporate.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        corporate.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        corporate.recordType = "CORPORATE";
        //        corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        corporate.isIncorporated = true;
        //        corporate.entityType = "BANKS";
        //        corporate.countryOfOperations = "SINGAPORE";
        //        corporate.countryOfIncorporation = "SINGAPORE";
        //        corporate.ownershipStructureLayers = "3 OR MORE";
        //        corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //        corporate.website = "https://google.com";
        //        corporate.incorporationNumber = "123";

        //        customer.corporateRecords.Add(corporate);
                
        //    }

        //    try
        //    {
        //        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"], Method.POST);
                
        //        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //        request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //        IRestResponse response = client.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var check = response.Content;
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //            if (!string.IsNullOrEmpty(check2.detail))
        //            {
        //                if (MaxRetries != 0)
        //                {
        //                    if (check2.detail.ToString() == "The token provided is invalid")
        //                    {
        //                        //rerun the Access Token
        //                        MaxRetries--;
        //                        GetToken();
        //                        CreateCustomerRest();
        //                    }
        //                    else
        //                    {
        //                        //Other failed reason
        //                        Console.Write(check2.detail.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write(MaxRetries);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //    }
        //}

        //public void DeleteCustomerRest(int id)
        //{
        //    Auth.Result objToken = GetToken();
        //    if (id > 0)
        //    {
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id, Method.DELETE);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            DeleteCustomerRest(id);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else 
        //    {
        //        Console.WriteLine("ID cannot be 0.");
        //    }
        //}

        //public void UpdateCustomerRest(int id)
        //{
        //    if (id > 0)
        //    {
        //        Auth.Result objToken = GetToken();
        //        ListCustomers list = GetSingleCustomerRecordByIDRest(id);

        //        if (list.count > 0)
        //        {
                    
        //            if (list.results.FirstOrDefault().customer == id)
        //            {
        //                var check22 = list.results.FirstOrDefault().id;
        //                string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //                string getCustomerType = "CORPORATE";

        //                SingleCustomerByID customer = new SingleCustomerByID();

        //                customer.onboardingMode = "FACE-TO-FACE";
        //                customer.productServiceComplexity = "SIMPLE";
        //                customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //                customer.profileType = "";
        //                customer.isActiveCustomer = true;
        //                customer.natureOfBusinessRelationship = "";
        //                customer.referenceId = "";
        //                customer.domains = new int[] { 36 };
        //                customer.users = new string[0];

        //                if (getCustomerType == "INDIVIDUAL")
        //                {
        //                    customer.individualRecords = new List<individualRecords>();
        //                    individualRecords individual = new individualRecords();

        //                    individual.primary = true;
        //                    individual.name = "tds name 2";
        //                    individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //                    individual.phoneNumbers = new string[] { "123", "456" };
        //                    individual.addresses = new string[] { "add 1", "add 2" };
        //                    individual.sourceOfFunds = "SALARY";
        //                    individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //                    individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //                    individual.gender = "MALE";
        //                    individual.nationality = "SINGAPORE";
        //                    individual.countryOfResidence = "SINGAPORE";
        //                    individual.title = "MR";
        //                    individual.countryOfBirth = "SINGAPORE";
        //                    individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                    individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //                    individual.occupation = "ACCOUNTANT";
        //                    individual.idType = "idType";
        //                    individual.idNumber = "idNumber";
        //                    individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                    individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                    individual.id = list.results.FirstOrDefault().record;

        //                    customer.individualRecords.Add(individual);
        //                }
        //                else
        //                {
        //                    //This is Corporate Type
        //                    customer.corporateRecords = new List<corporateRecords>();
        //                    corporateRecords corporate = new corporateRecords();

        //                    corporate.primary = true;
        //                    corporate.name = "tds name 3333366";
        //                    corporate.aliasNames = new string[] { "tds 3", "tds 3" };
        //                    corporate.phoneNumbers = new string[] { "123333", "456333" };
        //                    corporate.addresses = new string[] { "add 1333", "add 2333" };
        //                    corporate.sourceOfFunds = "SALARY";
        //                    corporate.emailAddresses = new string[] { "emailAddresses 1333", "emailAddresses 2333" };
        //                    corporate.bankAccounts = new string[] { "bank 1333", "bank 2" };
        //                    corporate.recordType = "CORPORATE";
        //                    corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                    corporate.isIncorporated = true;
        //                    corporate.entityType = "BANKS";
        //                    corporate.countryOfOperations = "SINGAPORE";
        //                    corporate.countryOfIncorporation = "SINGAPORE";
        //                    corporate.ownershipStructureLayers = "3 OR MORE";
        //                    corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //                    corporate.website = "https://google.com";
        //                    corporate.incorporationNumber = "123";
        //                    corporate.id = list.results.FirstOrDefault().record;

        //                    customer.corporateRecords.Add(corporate);
        //                }

        //                try
        //                {
        //                    var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                    var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id, Method.PATCH);

        //                    request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                    request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                    request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //                    request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //                    IRestResponse response = client.Execute(request);
        //                    if (response.IsSuccessful)
        //                    {
        //                        var check = response.Content;
        //                        Console.Write("Success");
        //                    }
        //                    else
        //                    {
        //                        var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                        if (!string.IsNullOrEmpty(check2.detail))
        //                        {
        //                            if (MaxRetries != 0)
        //                            {
        //                                if (check2.detail.ToString() == "The token provided is invalid")
        //                                {
        //                                    //rerun the Access Token
        //                                    MaxRetries--;
        //                                    GetToken();
        //                                    UpdateCustomerRest(id);
        //                                }
        //                                else
        //                                {
        //                                    //Other failed reason
        //                                    Console.Write(check2.detail.ToString());
        //                                }
        //                            }
        //                            else
        //                            {
        //                                Console.Write(MaxRetries);
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("Input ID and customer id not match.");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Records cprs not found.");
        //        }
        //    }
        //    else 
        //    {
        //        Console.WriteLine("ID cannot be 0.");
        //    }
            
        //}

        //public void UpdateCustomerCRP(int id)
        //{
        //    if (id > 0)
        //    {
        //        Auth.Result objToken = GetToken();
        //        ListCustomers list = GetSingleCustomerRecordByIDRest(id);

        //        if (list.count > 0)
        //        {
        //            foreach(var single in list.results)
        //            {
        //                if (!single.primary)
        //                {
        //                    string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                    string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //                    string getCustomerType = "INDIVIDUAL";//"CORPORATE";

        //                    SingleCustomerByID customer = new SingleCustomerByID();

        //                    customer.onboardingMode = "FACE-TO-FACE";
        //                    customer.productServiceComplexity = "SIMPLE";
        //                    customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //                    customer.profileType = "";
        //                    customer.isActiveCustomer = true;
        //                    customer.natureOfBusinessRelationship = "";
        //                    customer.referenceId = "";
        //                    customer.domains = new int[] { 36 };
        //                    customer.users = new string[0];

        //                    if (getCustomerType == "INDIVIDUAL")
        //                    {
        //                        customer.individualRecords = new List<individualRecords>();
        //                        individualRecords individual = new individualRecords();

        //                        individual.primary = true;
        //                        individual.name = "tds name 2";
        //                        individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //                        individual.phoneNumbers = new string[] { "123", "456" };
        //                        individual.addresses = new string[] { "add 1", "add 2" };
        //                        individual.sourceOfFunds = "SALARY";
        //                        individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //                        individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //                        individual.gender = "MALE";
        //                        individual.nationality = "SINGAPORE";
        //                        individual.countryOfResidence = "SINGAPORE";
        //                        individual.title = "MR";
        //                        individual.countryOfBirth = "SINGAPORE";
        //                        individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                        individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //                        individual.occupation = "ACCOUNTANT";
        //                        individual.idType = "idType";
        //                        individual.idNumber = "idNumber";
        //                        individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                        individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                        individual.id = single.record;

        //                        customer.individualRecords.Add(individual);
        //                    }
        //                    else
        //                    {
        //                        //This is Corporate Type
        //                        customer.corporateRecords = new List<corporateRecords>();
        //                        corporateRecords corporate = new corporateRecords();

        //                        //corporate.primary = true;
        //                        corporate.name = "tds name 3333366";
        //                        corporate.aliasNames = new string[] { "tds 3", "tds 3" };
        //                        corporate.phoneNumbers = new string[] { "123333", "456333" };
        //                        corporate.addresses = new string[] { "add 1333", "add 2333" };
        //                        corporate.sourceOfFunds = "SALARY";
        //                        corporate.emailAddresses = new string[] { "emailAddresses 1333", "emailAddresses 2333" };
        //                        corporate.bankAccounts = new string[] { "bank 1333", "bank 2" };
        //                        corporate.recordType = "CORPORATE";
        //                        corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                        corporate.isIncorporated = true;
        //                        corporate.entityType = "BANKS";
        //                        corporate.countryOfOperations = "SINGAPORE";
        //                        corporate.countryOfIncorporation = "SINGAPORE";
        //                        corporate.ownershipStructureLayers = "3 OR MORE";
        //                        corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //                        corporate.website = "https://google.com";
        //                        corporate.incorporationNumber = "123";
        //                        corporate.id = single.record;

        //                        customer.corporateRecords.Add(corporate);
        //                    }

        //                    try
        //                    {
        //                        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_CRPS"] + single.id, Method.PATCH);

        //                        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //                        request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //                        IRestResponse response = client.Execute(request);
        //                        if (response.IsSuccessful)
        //                        {
        //                            var check = response.Content;
        //                            Console.Write("Success");
        //                        }
        //                        else
        //                        {
        //                            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                            if (!string.IsNullOrEmpty(check2.detail))
        //                            {
        //                                if (MaxRetries != 0)
        //                                {
        //                                    if (check2.detail.ToString() == "The token provided is invalid")
        //                                    {
        //                                        //rerun the Access Token
        //                                        MaxRetries--;
        //                                        GetToken();
        //                                        UpdateCustomerCRP(id);
        //                                    }
        //                                    else
        //                                    {
        //                                        //Other failed reason
        //                                        Console.Write(check2.detail.ToString());
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Console.Write(MaxRetries);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Records cprs not found.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ID cannot be 0.");
        //    }

        //}

        //public ListCustomers GetSingleCustomerRecordByIDRest(int id)
        //{
        //    ListCustomers listCustomers = new ListCustomers();
        //    if (id > 0)
        //    {
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id + "/crps", Method.GET);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                listCustomers = JsonConvert.DeserializeObject<ListCustomers>(response.Content);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            GetSingleCustomerRecordByIDRest(id);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ID cannot be 0.");
        //    }

        //    return listCustomers;
        //}

        //public void CreateNewCRPForCustomerRest(int id)
        //{
        //    Auth.Result objToken = GetToken();
        //    string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //    string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //    string getCustomerType = "INDIVIDUAL";

        //    SingleCustomerCRP customer = new SingleCustomerCRP();

        //    customer.status = "CURRENT";
        //    customer.primary = true;
        //    customer.customer = 0;
        //    customer.record = 0;

        //    if (getCustomerType == "INDIVIDUAL")
        //    {
        //        customer.individualRecord = new individualRecords();
        //        customer.roles = new List<roles>();
        //        roles roles = new roles();

        //        roles.dateAppointed = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        roles.dateResigned = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        roles.role = "DIRECTOR";
        //        customer.roles.Add(roles);

        //        customer.individualRecord.primary = false;
        //        customer.individualRecord.name = "tds name 2";
        //        customer.individualRecord.aliasNames = new string[] { "tds 1", "tds 2" };
        //        customer.individualRecord.phoneNumbers = new string[] { "123", "456" };
        //        customer.individualRecord.addresses = new string[] { "add 1", "add 2" };
        //        customer.individualRecord.sourceOfFunds = "SALARY";
        //        customer.individualRecord.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        customer.individualRecord.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        customer.individualRecord.gender = "MALE";
        //        customer.individualRecord.nationality = "SINGAPORE";
        //        customer.individualRecord.countryOfResidence = "SINGAPORE";
        //        customer.individualRecord.title = "MR";
        //        customer.individualRecord.countryOfBirth = "SINGAPORE";
        //        customer.individualRecord.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        customer.individualRecord.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //        customer.individualRecord.occupation = "ACCOUNTANT";
        //        customer.individualRecord.idType = "idType";
        //        customer.individualRecord.idNumber = "idNumber";
        //        customer.individualRecord.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        customer.individualRecord.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //    }
        //    else
        //    {
        //        //This is Corporate Type
        //        customer.corporateRecord = new corporateRecords();
        //        customer.roles = new List<roles>();
        //        roles roles = new roles();

        //        roles.dateAppointed = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        roles.dateResigned = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        roles.role = "GENERAL PARTNER";
        //        customer.roles.Add(roles);

        //        latestScreeningConclusion latestScreeningConclusion = new latestScreeningConclusion();
        //        latestScreeningConclusion.pep = true;
        //        latestScreeningConclusion.sanction = true;
        //        latestScreeningConclusion.adverseNews = true;
        //        latestScreeningConclusion.noHit= true;
        //        latestScreeningConclusion.isSystemGenerated = true;
        //        latestScreeningConclusion.crp = null;
        //        customer.latestScreeningConclusion = latestScreeningConclusion;

        //        customer.corporateRecord.primary = false;
        //        customer.corporateRecord.name = "tds name 2";
        //        customer.corporateRecord.aliasNames = new string[] { "tds 1", "tds 2" };
        //        customer.corporateRecord.phoneNumbers = new string[] { "123", "456" };
        //        customer.corporateRecord.addresses = new string[] { "add 1", "add 2" };
        //        customer.corporateRecord.sourceOfFunds = "SALARY";
        //        customer.corporateRecord.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        customer.corporateRecord.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        customer.corporateRecord.isIncorporated = true;
        //        customer.corporateRecord.entityType = "BANKS";
        //        customer.corporateRecord.countryOfOperations = "SINGAPORE";
        //        customer.corporateRecord.countryOfIncorporation = "SINGAPORE";
        //        customer.corporateRecord.ownershipStructureLayers = "3 OR MORE";
        //        customer.corporateRecord.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //        customer.corporateRecord.website = "https://google.com";
        //        customer.corporateRecord.incorporationNumber = "123";
        //        customer.corporateRecord.incorporationDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        customer.corporateRecord.incorporationExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        customer.corporateRecord.inFatfJurisdiction = true;
        //    }

        //    try
        //    {
        //        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id + "/crps", Method.POST);

        //        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //        request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //        IRestResponse response = client.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var check = response.Content;
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //            if (!string.IsNullOrEmpty(check2.detail))
        //            {
        //                if (MaxRetries != 0)
        //                {
        //                    if (check2.detail.ToString() == "The token provided is invalid")
        //                    {
        //                        //rerun the Access Token
        //                        MaxRetries--;
        //                        GetToken();
        //                        CreateNewCRPForCustomerRest(id);
        //                    }
        //                    else
        //                    {
        //                        //Other failed reason
        //                        Console.Write(check2.detail.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write(MaxRetries);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //    }
        //}

        //public void GenerateRiskReportBeforeApprove(int id)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial RiskReport Model
        //        ApproveCustomerRiskReport RiskReport = new ApproveCustomerRiskReport();

        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id + "/risk_reports", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            RiskReport.riskRating = "LOW";
        //            RiskReport.isOutdated = false;
        //            RiskReport.customer = id;

        //            request.AddJsonBody(JsonConvert.SerializeObject(RiskReport));

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                RiskReport = JsonConvert.DeserializeObject<ApproveCustomerRiskReport>(response.Content);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            GenerateRiskReportBeforeApprove(id);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        GenerateRiskReportBeforeApprove(id);
        //    }
        //}

        //public void LoopCreateScreeningConclusionReport(int id)
        //{
        //    //id = Customer ID
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial Screening Conculsion Model Class
        //        latestScreeningConclusion screening = new latestScreeningConclusion();
        //        //Call Customer crps to get all the crps id.
        //        ListCustomers list = GetSingleCustomerRecordByIDRest(id);
        //        try
        //        {
        //            if (list.results.Count > 0)
        //            {
        //                //means have results
        //                foreach (var crpid in list.results)
        //                {
        //                    //if is null, then create screening result
        //                    if (crpid.latestScreeningConclusion == null)
        //                    {
        //                        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_CRPS"] + crpid.id + "/screening_conclusions/", Method.POST);

        //                        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //                        screening.pep = false;
        //                        screening.sanction = false;
        //                        screening.adverseNews = false;
        //                        screening.noHit = true;
        //                        screening.isSystemGenerated = true;
        //                        screening.crp = Convert.ToString(crpid.id);

        //                        request.AddJsonBody(JsonConvert.SerializeObject(screening));

        //                        IRestResponse response = client.Execute(request);
        //                        if (response.IsSuccessful)
        //                        {
        //                            var check = response.Content;
        //                            Console.Write("Success");
        //                        }
        //                        else
        //                        {
        //                            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                            if (!string.IsNullOrEmpty(check2.detail))
        //                            {
        //                                if (MaxRetries != 0)
        //                                {
        //                                    if (check2.detail.ToString() == "The token provided is invalid")
        //                                    {
        //                                        //rerun the Access Token
        //                                        MaxRetries--;
        //                                        GetToken();
        //                                        LoopCreateScreeningConclusionReport(id);
        //                                    }
        //                                    else
        //                                    {
        //                                        //Other failed reason
        //                                        Console.Write(check2.detail.ToString());
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Console.Write(MaxRetries);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("No CRPID found.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        LoopCreateScreeningConclusionReport(id);
        //    }
        //}

        //public void CreateApprovalStatus(int id)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        latestApprovalStatus approvalStatus = new latestApprovalStatus();
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_RiskReport"] + id + "/approval_statuses/", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            approvalStatus.overrideRisk = "LOW";
        //            approvalStatus.approvalStatus = "ACCEPTED";
        //            approvalStatus.riskReport = 0;

        //            request.AddJsonBody(JsonConvert.SerializeObject(approvalStatus));
        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            CreateApprovalStatus(id);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        CreateApprovalStatus(id);
        //    }
        //}

        //public void CheckEmptyScreeningReportAndCreate(int id)
        //{
        //    //get all data from Customer CRPS
        //    ListCustomers customer = GetSingleCustomerRecordByIDRest(id);

        //    if (customer.count > 0)
        //    {
        //        latestScreeningConclusion screening = new latestScreeningConclusion();
        //        //means have result
        //        foreach (var item in customer.results)
        //        {
        //            if (item.latestScreeningConclusion == null)
        //            {
        //                screening = new latestScreeningConclusion();

        //                //if null then need to create screening report and update approval status
        //                var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_CRPS"] + item.id + "/screening_conclusions/", Method.POST);

        //                request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //                screening.pep = false;
        //                screening.sanction = false;
        //                screening.adverseNews = false;
        //                screening.noHit = true;
        //                screening.isSystemGenerated = true;
        //                screening.crp = Convert.ToString(item.id);

        //                request.AddJsonBody(JsonConvert.SerializeObject(screening));

        //                IRestResponse response = client.Execute(request);
        //                if (response.IsSuccessful)
        //                {
        //                    var check = response.Content;
        //                    Console.Write("Success");
        //                }
        //            }
        //        }
        //    }
        //}

        //public void DeleteCustomerCRP(int id)
        //{
        //    Auth.Result objToken = GetToken();
        //    ListCustomers customers = GetSingleCustomerRecordByIDRest(id);

        //    if (id > 0)
        //    {
        //        if (customers.count > 0)
        //        {
        //            foreach (var customer in customers.results)
        //            {
        //                if (!customer.primary)
        //                {
        //                    try
        //                    {
        //                        var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                        var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_CRPS"] + customer.id, Method.DELETE);

        //                        request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                        request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                        request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //                        IRestResponse response = client.Execute(request);
        //                        if (response.IsSuccessful)
        //                        {
        //                            var check = response.Content;
        //                            Console.Write("Success");
        //                        }
        //                        else
        //                        {
        //                            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                            if (!string.IsNullOrEmpty(check2.detail))
        //                            {
        //                                if (MaxRetries != 0)
        //                                {
        //                                    if (check2.detail.ToString() == "The token provided is invalid")
        //                                    {
        //                                        //rerun the Access Token
        //                                        MaxRetries--;
        //                                        GetToken();
        //                                        DeleteCustomerCRP(id);
        //                                    }
        //                                    else
        //                                    {
        //                                        //Other failed reason
        //                                        Console.Write(check2.detail.ToString());
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Console.Write(MaxRetries);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ID cannot be 0.");
        //    }
        //}
        ////End RestSharp

        ////Full Flow Here
        //public void Full_CreateCustomer_StepOne()
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        string getCustomerType = "CORPORATE";

        //        //Initial Result Needed
        //        SingleCustomerCRP CustomerCRP = new SingleCustomerCRP();
        //        FlowNeededItem flow_need_item = new FlowNeededItem();

        //        SingleCustomerByID customer = new SingleCustomerByID();

        //        customer.onboardingMode = "FACE-TO-FACE";
        //        customer.productServiceComplexity = "SIMPLE";
        //        customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //        customer.profileType = "";
        //        customer.isActiveCustomer = true;
        //        customer.natureOfBusinessRelationship = "";
        //        customer.referenceId = "";
        //        customer.domains = new int[] { 36 };
        //        customer.users = new string[0];

        //        if (getCustomerType == "INDIVIDUAL")
        //        {
        //            customer.individualRecords = new List<individualRecords>();
        //            individualRecords individual = new individualRecords();

        //            individual.primary = true;
        //            individual.name = "tds name 2";
        //            individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //            individual.phoneNumbers = new string[] { "123", "456" };
        //            individual.addresses = new string[] { "add 1", "add 2" };
        //            individual.sourceOfFunds = "SALARY";
        //            individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //            individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //            individual.gender = "MALE";
        //            individual.nationality = "SINGAPORE";
        //            individual.countryOfResidence = "SINGAPORE";
        //            individual.title = "MR";
        //            individual.countryOfBirth = "SINGAPORE";
        //            individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //            individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //            individual.occupation = "ACCOUNTANT";
        //            individual.idType = "idType";
        //            individual.idNumber = "idNumber";
        //            individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //            individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");

        //            customer.individualRecords.Add(individual);
        //        }
        //        else
        //        {
        //            //This is Corporate Type
        //            customer.corporateRecords = new List<corporateRecords>();
        //            corporateRecords corporate = new corporateRecords();

        //            corporate.primary = true;
        //            corporate.name = "tds name 2";
        //            corporate.aliasNames = new string[] { "tds 1", "tds 2" };
        //            corporate.phoneNumbers = new string[] { "123", "456" };
        //            corporate.addresses = new string[] { "add 1", "add 2" };
        //            corporate.sourceOfFunds = "SALARY";
        //            corporate.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //            corporate.bankAccounts = new string[] { "bank 1", "bank 2" };
        //            corporate.recordType = "CORPORATE";
        //            corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //            corporate.isIncorporated = true;
        //            corporate.entityType = "BANKS";
        //            corporate.countryOfOperations = "SINGAPORE";
        //            corporate.countryOfIncorporation = "SINGAPORE";
        //            corporate.ownershipStructureLayers = "3 OR MORE";
        //            corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //            corporate.website = "https://google.com";
        //            corporate.incorporationNumber = "123";

        //            customer.corporateRecords.Add(corporate);

        //        }

        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"], Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //            request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                //Customer Created
        //                //1. Risk Report
        //                var check = response.Content;
        //                CustomerCRP = JsonConvert.DeserializeObject<SingleCustomerCRP>(response.Content);
        //                flow_need_item.CustomerID = CustomerCRP.id;

        //                //Pass Data to Create RiskReport
        //                //CreateNewCRPForCustomerRest(flow_need_item.CustomerID);
        //                Full_GetCRPID_StepTwo(flow_need_item);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_CreateCustomer_StepOne();
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else 
        //    {
        //        GetToken();
        //        Full_CreateCustomer_StepOne();
        //    }
        //}

        //public void Full_GetCRPID_StepTwo(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial CRP Model Class
        //        ListCustomers customerCRP = new ListCustomers();
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + flow_need_item.CustomerID + "/crps", Method.GET);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                customerCRP = JsonConvert.DeserializeObject<ListCustomers>(response.Content);
        //                flow_need_item.CRPID = new List<int>();

        //                foreach (var getid in customerCRP.results)
        //                {
        //                    flow_need_item.CRPID.Add(getid.id);
        //                }

        //                Full_CreateScreeningConclusion_StepThree(flow_need_item);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_GetCRPID_StepTwo(flow_need_item);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_GetCRPID_StepTwo(flow_need_item);
        //    }
        //}

        //public void Full_CreateScreeningConclusion_StepThree(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial Screening Conculsion Model Class
        //        latestScreeningConclusion screening = new latestScreeningConclusion();

        //        try
        //        {

        //            if (flow_need_item.CRPID.Count > 0)
        //            {
        //                foreach (var crpid in flow_need_item.CRPID)
        //                {
        //                    var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                    var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_CRPS"] + crpid + "/screening_conclusions/", Method.POST);

        //                    request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                    request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                    request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //                    screening.pep = false;
        //                    screening.sanction = false;
        //                    screening.adverseNews = false;
        //                    screening.noHit = true;
        //                    screening.isSystemGenerated = true;
        //                    screening.crp = Convert.ToString(crpid);

        //                    request.AddJsonBody(JsonConvert.SerializeObject(screening));

        //                    IRestResponse response = client.Execute(request);
        //                    if (response.IsSuccessful)
        //                    {
        //                        var check = response.Content;
        //                        //Last Step is Generate Approval Status
        //                        Full_CreateRiskReport_StepFour(flow_need_item);
        //                        Console.Write("Success");
        //                    }
        //                    else
        //                    {
        //                        var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                        if (!string.IsNullOrEmpty(check2.detail))
        //                        {
        //                            if (MaxRetries != 0)
        //                            {
        //                                if (check2.detail.ToString() == "The token provided is invalid")
        //                                {
        //                                    //rerun the Access Token
        //                                    MaxRetries--;
        //                                    GetToken();
        //                                    Full_CreateScreeningConclusion_StepThree(flow_need_item);
        //                                }
        //                                else
        //                                {
        //                                    //Other failed reason
        //                                    Console.Write(check2.detail.ToString());
        //                                }
        //                            }
        //                            else
        //                            {
        //                                Console.Write(MaxRetries);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("No CRPID found.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_CreateScreeningConclusion_StepThree(flow_need_item);
        //    }
        //}

        //public void Full_CreateRiskReport_StepFour(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial RiskReport Model
        //        ApproveCustomerRiskReport RiskReport = new ApproveCustomerRiskReport();

        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + flow_need_item.CustomerID + "/risk_reports", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            RiskReport.riskRating = "LOW";
        //            RiskReport.isOutdated = false;
        //            RiskReport.customer = flow_need_item.CustomerID;

        //            request.AddJsonBody(JsonConvert.SerializeObject(RiskReport));

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                RiskReport = JsonConvert.DeserializeObject<ApproveCustomerRiskReport>(response.Content);
        //                flow_need_item.RiskReportID = RiskReport.id;

        //                //Risk Report ID Get
        //                //Next is create new Screening Result
        //                Full_CreateApprovalStatus_StepFive(flow_need_item);

        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_CreateRiskReport_StepFour(flow_need_item);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_CreateRiskReport_StepFour(flow_need_item);
        //    }
        //}

        //public void Full_CreateApprovalStatus_StepFive(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        latestApprovalStatus approvalStatus = new latestApprovalStatus();
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_RiskReport"] + flow_need_item.RiskReportID + "/approval_statuses/", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            approvalStatus.overrideRisk = "LOW";
        //            approvalStatus.approvalStatus = "ACCEPTED";
        //            approvalStatus.riskReport = 0;

        //            request.AddJsonBody(JsonConvert.SerializeObject(approvalStatus));
        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_CreateApprovalStatus_StepFive(flow_need_item);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_CreateApprovalStatus_StepFive(flow_need_item);
        //    }
        //}


        ////PATCHING FLOW
        //public void Full_PatchCustomer_GetCRPS(int id)
        //{
        //    if (id > 0)
        //    {
        //        if (!string.IsNullOrEmpty(GetAccessToken))
        //        {
        //            FlowNeededItem flow_need_item = new FlowNeededItem();
        //            //Initial CRP Model Class
        //            ListCustomers customerCRP = new ListCustomers();
        //            try
        //            {
        //                var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + id + "/crps", Method.GET);

        //                request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //                IRestResponse response = client.Execute(request);
        //                if (response.IsSuccessful)
        //                {
        //                    var check = response.Content;
        //                    customerCRP = JsonConvert.DeserializeObject<ListCustomers>(response.Content);
        //                    flow_need_item.CustomerID = id;
        //                    flow_need_item.RecordID = customerCRP.results.FirstOrDefault().record;
        //                    Full_PatchCustomer_Patch(flow_need_item);
        //                    Console.Write("Success");
        //                }
        //                else
        //                {
        //                    var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                    if (!string.IsNullOrEmpty(check2.detail))
        //                    {
        //                        if (MaxRetries != 0)
        //                        {
        //                            if (check2.detail.ToString() == "The token provided is invalid")
        //                            {
        //                                //rerun the Access Token
        //                                MaxRetries--;
        //                                GetToken();
        //                                Full_PatchCustomer_GetCRPS(id);
        //                            }
        //                            else
        //                            {
        //                                //Other failed reason
        //                                Console.Write(check2.detail.ToString());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Console.Write(MaxRetries);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            GetToken();
        //            Full_PatchCustomer_GetCRPS(id);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Customer ID cannot be empty.");
        //    }
        //}

        //public void Full_PatchCustomer_Patch(FlowNeededItem flow_need_item)
        //{
        //    if (flow_need_item.CustomerID > 0)
        //    {
        //        if (!string.IsNullOrEmpty(GetAccessToken))
        //        {
        //            string getCustomerType = "CORPORATE";

        //            SingleCustomerByID customer = new SingleCustomerByID();

        //            customer.onboardingMode = "FACE-TO-FACE";
        //            customer.productServiceComplexity = "SIMPLE";
        //            customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //            customer.profileType = "";
        //            customer.isActiveCustomer = true;
        //            customer.natureOfBusinessRelationship = "";
        //            customer.referenceId = "";
        //            customer.domains = new int[] { 36 };
        //            customer.users = new string[0];

        //            if (getCustomerType == "INDIVIDUAL")
        //            {
        //                customer.individualRecords = new List<individualRecords>();
        //                individualRecords individual = new individualRecords();

        //                individual.primary = true;
        //                individual.name = "tds name 2";
        //                individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //                individual.phoneNumbers = new string[] { "123", "456" };
        //                individual.addresses = new string[] { "add 1", "add 2" };
        //                individual.sourceOfFunds = "SALARY";
        //                individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //                individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //                individual.gender = "MALE";
        //                individual.nationality = "SINGAPORE";
        //                individual.countryOfResidence = "SINGAPORE";
        //                individual.title = "MR";
        //                individual.countryOfBirth = "SINGAPORE";
        //                individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //                individual.occupation = "ACCOUNTANT";
        //                individual.idType = "idType";
        //                individual.idNumber = "idNumber";
        //                individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                individual.id = flow_need_item.RecordID;

        //                customer.individualRecords.Add(individual);
        //            }
        //            else
        //            {
        //                //This is Corporate Type
        //                customer.corporateRecords = new List<corporateRecords>();
        //                corporateRecords corporate = new corporateRecords();

        //                corporate.primary = true;
        //                corporate.name = "tds name 33333";
        //                corporate.aliasNames = new string[] { "tds 3", "tds 3" };
        //                corporate.phoneNumbers = new string[] { "123333", "456333" };
        //                corporate.addresses = new string[] { "add 1333", "add 2333" };
        //                corporate.sourceOfFunds = "SALARY";
        //                corporate.emailAddresses = new string[] { "emailAddresses 1333", "emailAddresses 2333" };
        //                corporate.bankAccounts = new string[] { "bank 1333", "bank 2" };
        //                corporate.recordType = "CORPORATE";
        //                corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //                corporate.isIncorporated = true;
        //                corporate.entityType = "BANKS";
        //                corporate.countryOfOperations = "SINGAPORE";
        //                corporate.countryOfIncorporation = "SINGAPORE";
        //                corporate.ownershipStructureLayers = "3 OR MORE";
        //                corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //                corporate.website = "https://google.com";
        //                corporate.incorporationNumber = "123";
        //                corporate.id = flow_need_item.RecordID;

        //                customer.corporateRecords.Add(corporate);
        //            }

        //            try
        //            {
        //                var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //                var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + flow_need_item.CustomerID, Method.PATCH);

        //                request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //                request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //                request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);
        //                request.AddJsonBody(JsonConvert.SerializeObject(customer));

        //                IRestResponse response = client.Execute(request);
        //                if (response.IsSuccessful)
        //                {
        //                    var check = response.Content;
        //                    Full_PatchCustomer_CreateRiskReport(flow_need_item);
        //                    Console.Write("Success");
        //                }
        //                else
        //                {
        //                    var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                    if (!string.IsNullOrEmpty(check2.detail))
        //                    {
        //                        if (MaxRetries != 0)
        //                        {
        //                            if (check2.detail.ToString() == "The token provided is invalid")
        //                            {
        //                                //rerun the Access Token
        //                                MaxRetries--;
        //                                GetToken();
        //                                Full_PatchCustomer_Patch(flow_need_item);
        //                            }
        //                            else
        //                            {
        //                                //Other failed reason
        //                                Console.Write(check2.detail.ToString());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Console.Write(MaxRetries);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            GetToken();
        //            Full_PatchCustomer_Patch(flow_need_item);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Customer ID cannot be empty.");
        //    }
        //}

        //public void Full_PatchCustomer_CreateRiskReport(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        //Initial RiskReport Model
        //        ApproveCustomerRiskReport RiskReport = new ApproveCustomerRiskReport();

        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + flow_need_item.CustomerID + "/risk_reports", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            RiskReport.riskRating = "LOW";
        //            RiskReport.isOutdated = false;
        //            RiskReport.customer = flow_need_item.CustomerID;

        //            request.AddJsonBody(JsonConvert.SerializeObject(RiskReport));

        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                RiskReport = JsonConvert.DeserializeObject<ApproveCustomerRiskReport>(response.Content);
        //                flow_need_item.RiskReportID = RiskReport.id;
        //                Full_PatchCustomer_ApprovalStatus(flow_need_item);
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_PatchCustomer_CreateRiskReport(flow_need_item);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_PatchCustomer_CreateRiskReport(flow_need_item);
        //    }
        //}

        //public void Full_PatchCustomer_ApprovalStatus(FlowNeededItem flow_need_item)
        //{
        //    if (!string.IsNullOrEmpty(GetAccessToken))
        //    {
        //        latestApprovalStatus approvalStatus = new latestApprovalStatus();
        //        try
        //        {
        //            var client = new RestClient(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //            var request = new RestRequest(WebConfigurationManager.AppSettings["Artemis_Customer_RiskReport"] + flow_need_item.RiskReportID + "/approval_statuses/", Method.POST);

        //            request.AddHeader("Authorization", "Bearer " + GetAccessToken);
        //            request.AddOrUpdateHeader("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //            request.AddHeader("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //            approvalStatus.overrideRisk = "LOW";
        //            approvalStatus.approvalStatus = "CLEARED";
        //            approvalStatus.riskReport = 0;

        //            request.AddJsonBody(JsonConvert.SerializeObject(approvalStatus));
        //            IRestResponse response = client.Execute(request);
        //            if (response.IsSuccessful)
        //            {
        //                var check = response.Content;
        //                Console.Write("Success");
        //            }
        //            else
        //            {
        //                var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content);

        //                if (!string.IsNullOrEmpty(check2.detail))
        //                {
        //                    if (MaxRetries != 0)
        //                    {
        //                        if (check2.detail.ToString() == "The token provided is invalid")
        //                        {
        //                            //rerun the Access Token
        //                            MaxRetries--;
        //                            GetToken();
        //                            Full_PatchCustomer_ApprovalStatus(flow_need_item);
        //                        }
        //                        else
        //                        {
        //                            //Other failed reason
        //                            Console.Write(check2.detail.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.Write(MaxRetries);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception while getting the Bearer Token : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        GetToken();
        //        Full_PatchCustomer_ApprovalStatus(flow_need_item);
        //    }
        //}
        //End Full Flow Here

        //ADD CRP FLOW
        //END ADD CRP FLOW

        //DELETE ALL CRP AND RE-ADD
        //END DELETE ALL CRP AND RE-ADD

        //public Auth.Result Test()
        //{
        //    Auth.Result objToken = new Auth.Result();
        //    using (var client = new HttpClient())
        //    {
        //        Auth.GetBody p = new Auth.GetBody 
        //        { 
        //            username = WebConfigurationManager.AppSettings["Artemis_username"],
        //            password = WebConfigurationManager.AppSettings["Artemis_password"],
        //            clientId = WebConfigurationManager.AppSettings["Artemis_clientid"],
        //            userPoolId = WebConfigurationManager.AppSettings["Artemis_userpoolid"]
        //        };

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.PostAsJsonAsync(WebConfigurationManager.AppSettings["Artemis_getTokenUrl"], p).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            objToken = JsonConvert.DeserializeObject<Auth.Result>(response.Content.ReadAsStringAsync().Result);
        //            GetAccessToken = objToken.AuthenticationResult.AccessToken;
        //            Console.Write("Success: " + objToken.ToString());
        //        }
        //        else
        //            Console.Write("Error");
        //    }
        //    return objToken;
        //}

        //public void RefreshToken()
        //{
        //    Auth.Result objToken = Test();
        //    Auth.Result newObjToken = new Auth.Result();
        //    using (var client = new HttpClient())
        //    {
        //        Auth.RefreshTokenParam p = new Auth.RefreshTokenParam
        //        {
        //            clientId = WebConfigurationManager.AppSettings["Artemis_clientid"],
        //            refreshToken = objToken.AuthenticationResult.RefreshToken
        //        };

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.PostAsJsonAsync(WebConfigurationManager.AppSettings["Artemis_refreshTokenUrl"], p).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            newObjToken = JsonConvert.DeserializeObject<Auth.Result>(response.Content.ReadAsStringAsync().Result);
        //            Console.Write("Success");
        //        }
        //        else
        //            Console.Write("Error");
        //    }
        //}

        //public void GetCustomerByID()
        //{
        //    //To get Access Token
        //    //Auth.Result objToken = Test();
            
        //    ListCustomers listCustomers = new ListCustomers();
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        client.DefaultRequestHeaders.Add("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.GetAsync(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"]).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            listCustomers = JsonConvert.DeserializeObject<ListCustomers>(response.Content.ReadAsStringAsync().Result);
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content.ReadAsStringAsync().Result);

        //            if (!string.IsNullOrEmpty(check2.detail))
        //            {
        //                if (MaxRetries != 0)
        //                {
        //                    if (check2.detail.ToString() == "The token provided is invalid")
        //                    {
        //                        //rerun the Access Token
        //                        MaxRetries--;
        //                        Test();
        //                        GetCustomerByID();
        //                    }
        //                    else 
        //                    {
        //                        //Other failed reason
        //                        Console.Write(check2.detail.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write(MaxRetries);
        //                }
        //            }
        //        }
        //    }
        //}

        //public void GetSingleCustomerByID()
        //{
        //    //To get Access Token
        //    Auth.Result objToken = Test();
        //    SingleCustomerByID SingleCustomerByID = new SingleCustomerByID();

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objToken.AuthenticationResult.AccessToken);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        client.DefaultRequestHeaders.Add("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.GetAsync(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + 780316).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            SingleCustomerByID = JsonConvert.DeserializeObject<SingleCustomerByID>(response.Content.ReadAsStringAsync().Result);
        //            Console.Write("Success");
        //        }
        //        else
        //            Console.Write("Error");
        //    }
        //}

        //public void CreateCustomer()
        //{
        //    //To get Access Token
        //    Auth.Result objToken = Test();
        //    string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //    string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //    string getCustomerType = "INDIVIDUAL";

        //    SingleCustomerByID customer = new SingleCustomerByID();

        //    customer.onboardingMode = "FACE-TO-FACE";
        //    customer.productServiceComplexity = "SIMPLE";
        //    customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER" };
        //    customer.profileType = "";
        //    customer.isActiveCustomer = true;
        //    customer.natureOfBusinessRelationship = "";
        //    customer.referenceId = "";
        //    customer.domains = new int[] { 36 };
        //    customer.users = new string[0];

        //    if (getCustomerType == "INDIVIDUAL")
        //    {
        //        customer.individualRecords = new List<individualRecords>();
        //        individualRecords individual = new individualRecords();

        //        individual.primary = true;
        //        individual.name = "tds name 2";
        //        individual.aliasNames = new string[] { "tds 1", "tds 2" };
        //        individual.phoneNumbers = new string[] { "123", "456" };
        //        individual.addresses = new string[] { "add 1", "add 2" };
        //        individual.sourceOfFunds = "SALARY";
        //        individual.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        individual.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        individual.gender = "MALE";
        //        individual.nationality = "SINGAPORE";
        //        individual.countryOfResidence = "SINGAPORE";
        //        individual.title = "MR";
        //        individual.countryOfBirth = "SINGAPORE";
        //        individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //        individual.occupation = "ACCOUNTANT";
        //        individual.idType = "idType";
        //        individual.idNumber = "idNumber";
        //        individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");

        //        customer.individualRecords.Add(individual);
        //    }
        //    else
        //    {
        //        //This is Corporate Type
        //        customer.corporateRecords = new List<corporateRecords>();
        //        corporateRecords corporate = new corporateRecords();

        //        corporate.primary = true;
        //        corporate.name = "tds name 2";
        //        corporate.aliasNames = new string[] { "tds 1", "tds 2" };
        //        corporate.phoneNumbers = new string[] { "123", "456" };
        //        corporate.addresses = new string[] { "add 1", "add 2" };
        //        corporate.sourceOfFunds = "SALARY";
        //        corporate.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        corporate.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        corporate.recordType = "CORPORATE";
        //        corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        corporate.isIncorporated = true;
        //        corporate.entityType = "BANKS";
        //        corporate.countryOfOperations = "SINGAPORE";
        //        corporate.countryOfIncorporation = "SINGAPORE";
        //        corporate.ownershipStructureLayers = "3 OR MORE";
        //        corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //        corporate.website = "https://google.com";
        //        corporate.incorporationNumber = "123";

        //        customer.corporateRecords.Add(corporate);
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objToken.AuthenticationResult.AccessToken);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        client.DefaultRequestHeaders.Add("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.PostAsJsonAsync(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"], customer).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content.ReadAsStringAsync().Result);
        //            Console.Write("Error");
        //        }
        //    }
        //}

        //public void UpdateCustomer()
        //{
        //    //To get Access Token
        //    Auth.Result objToken = Test();
        //    string datetimenow = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //    string checktime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");//2023-03-28T07:28:34.664Z
        //    string getCustomerType = "INDIVIDUAL";

        //    SingleCustomerByID customer = new SingleCustomerByID();

        //    customer.onboardingMode = "UNKNOWN";
        //    customer.productServiceComplexity = "HIGH RISK";
        //    customer.paymentModes = new string[] { "TELEGRAPHIC TRANSFER", "CREDIT CARD", "CASH" };
        //    customer.profileType = "";
        //    customer.isActiveCustomer = true;
        //    customer.natureOfBusinessRelationship = "";
        //    customer.referenceId = "";
        //    customer.domains = new int[] { 36 };
        //    customer.users = new string[0];

        //    if (getCustomerType == "INDIVIDUAL")
        //    {
        //        customer.individualRecords = new List<individualRecords>();
        //        individualRecords individual = new individualRecords();

        //        individual.primary = true;
        //        individual.name = "tds name 3";
        //        individual.aliasNames = new string[] { "tds 3", "tds4" };
        //        individual.phoneNumbers = new string[] { "123555", "456999" };
        //        individual.addresses = new string[] { "add 1111", "add 2222" };
        //        individual.sourceOfFunds = "SALARY";
        //        individual.emailAddresses = new string[] { "emailAddresses 13", "emailAddresses 23" };
        //        individual.bankAccounts = new string[] { "bank 13", "bank 23" };
        //        individual.gender = "MALE";
        //        individual.nationality = "SINGAPORE";
        //        individual.countryOfResidence = "SINGAPORE";
        //        individual.title = "ms";
        //        individual.countryOfBirth = "SINGAPORE";
        //        individual.dateOfBirth = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.industry = "ACCOUNTING/AUDIT/CONSULTING";
        //        individual.occupation = "ACCOUNTANT";
        //        individual.idType = "idType";
        //        individual.idNumber = "idNumber";
        //        individual.idIssueDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        individual.idExpiryDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");

        //        customer.individualRecords.Add(individual);
        //    }
        //    else
        //    {
        //        //This is Corporate Type
        //        customer.corporateRecords = new List<corporateRecords>();
        //        corporateRecords corporate = new corporateRecords();

        //        corporate.primary = true;
        //        corporate.name = "tds name 2";
        //        corporate.aliasNames = new string[] { "tds 1", "tds 2" };
        //        corporate.phoneNumbers = new string[] { "123", "456" };
        //        corporate.addresses = new string[] { "add 1", "add 2" };
        //        corporate.sourceOfFunds = "SALARY";
        //        corporate.emailAddresses = new string[] { "emailAddresses 1", "emailAddresses 2" };
        //        corporate.bankAccounts = new string[] { "bank 1", "bank 2" };
        //        corporate.recordType = "CORPORATE";
        //        corporate.createdAt = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss.fffZ");
        //        corporate.isIncorporated = true;
        //        corporate.entityType = "BANKS";
        //        corporate.countryOfOperations = "SINGAPORE";
        //        corporate.countryOfIncorporation = "SINGAPORE";
        //        corporate.ownershipStructureLayers = "3 OR MORE";
        //        corporate.businessActivity = "ACCOUNTING/AUDIT/CONSULTING";
        //        corporate.website = "https://google.com";
        //        corporate.incorporationNumber = "123";

        //        customer.corporateRecords.Add(corporate);
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        HttpClient c = new HttpClient();

        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objToken.AuthenticationResult.AccessToken);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        client.DefaultRequestHeaders.Add("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.PostAsJsonAsync(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + "/" + 1099066, customer).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            var check2 = JsonConvert.DeserializeObject<Auth.Error>(response.Content.ReadAsStringAsync().Result);
        //            Console.Write("Error");
        //        }
        //    }
        //}

        //public void DeleteCustomer()
        //{
        //    //To get Access Token
        //    Auth.Result objToken = Test();

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objToken.AuthenticationResult.AccessToken);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", WebConfigurationManager.AppSettings["Artemis_Content_Type"]);
        //        client.DefaultRequestHeaders.Add("X-ARTEMIS-DOMAIN", WebConfigurationManager.AppSettings["Artemis_X_ARTEMIS_DOMAIN"]);

        //        client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["Artemis_baseurl"]);
        //        var response = client.DeleteAsync(WebConfigurationManager.AppSettings["Artemis_Customer_ListCustomers"] + 1099045).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var check = response.Content.ReadAsStringAsync().Result;
        //            Console.Write("Success");
        //        }
        //        else
        //        {
        //            Console.Write("Error");
        //        }
        //    }
        //}

        public void Test()
        {
            using (var context = new DataAccess.GreatEastForex())
            {
                var param1 = new SqlParameter();
                param1.ParameterName = "@id";
                param1.DbType = DbType.Int32;
                param1.Value = 7;

                var record = context.CheckSalesCurrencyIDExists.SqlQuery("dbo.CheckSalesCurrencyIDExist @id", param1).Count();

            }
        }

        //Test
        //public void Test()
        //{
        //	using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
        //	{
        //		SqlCommand command = new SqlCommand("GetAllCustomerActiveListInSalesModule", connection);
        //		command.Connection.Open();

        //		IAsyncResult result = command.BeginExecuteNonQuery();

        //		int count = 0;
        //		while (!result.IsCompleted)
        //		{
        //			Console.WriteLine("Waiting ({0})", count++);
        //			System.Threading.Thread.Sleep(1000);
        //		}
        //		Console.WriteLine("Command complete. Affected {0} rows.",
        //		command.EndExecuteNonQuery(result));
        //	}


        //	//using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
        //	//{
        //	//	conn.Open();
        //	//	List<GetAllCustomerActiveListInSalesModule> customerList = new List<GetAllCustomerActiveListInSalesModule>();
        //	//	GetAllCustomerActiveListInSalesModule singleItem = new GetAllCustomerActiveListInSalesModule();

        //	//	SqlCommand cmd = new SqlCommand("GetAllCustomerActiveListInSalesModule", conn);

        //	//	cmd.ExecuteNonQuery();
        //	//	cmd.CommandType = CommandType.StoredProcedure;
        //	//	//cmd.CommandTimeout = 5000;

        //	//	// execute the command
        //	//	using (SqlDataReader reader = cmd.ExecuteReader())
        //	//	{
        //	//		customerList = reader.Cast<IDataRecord>()
        //	//		.Select(x => new GetAllCustomerActiveListInSalesModule
        //	//		{
        //	//			ID = (int)x["ID"],
        //	//			IsSubAccount = (long)x["IsSubAccount"],
        //	//			IsDeleted = (string)x["IsDeleted"],
        //	//			CustomerType = (string)x["CustomerType"],
        //	//			Company_RegisteredName = (x["Company_RegisteredName"] == DBNull.Value) ? "" : (string)x["Company_RegisteredName"],
        //	//			Natural_Name = (x["Natural_Name"] == DBNull.Value) ? "" : (string)x["Natural_Name"],
        //	//			Customer_Profile = (string)x["CustomerProfile"],
        //	//			CustomerCode = (x["CustomerCode"] == DBNull.Value) ? "" : (string)x["CustomerCode"],
        //	//			Status = (string)x["Status"]
        //	//		}).ToList();
        //	//	}
        //	//	conn.Close();
        //	//}


        //	//ListDictionary replacement = new ListDictionary();
        //	//var GetUrl = ConfigurationManager.AppSettings["VerifyAccountUrl"].ToString() + "?Email=" + "hongguan@thedottsolutions.com" + "&key=" + "test1234";
        //	//var test1234 = "";
        //	//replacement.Add();

        //	//Dictionary<int, SaleTransaction> rollBack_saleTransactions = new Dictionary<int, SaleTransaction>();

        //	//rollBack_saleTransactions.Add(1, new SaleTransaction());
        //	////rollBack_saleTransactions.Add(1, new SaleTransaction());
        //	//if (rollBack_saleTransactions.ContainsKey(1))
        //	//{
        //	//	rollBack_saleTransactions.Add(2, new SaleTransaction());
        //	//	string test = "ok";
        //	//}
        //	//else
        //	//{
        //	//	string test = "not ok";
        //	//}

        //	//TempData["Result"] = "1st";
        //	//TempData["Result"] = "2nd";
        //}

        // GET: Access
        public ActionResult Index()
        {
            string environment = ConfigurationManager.AppSettings["Environment"].ToString();

            if (environment == "live")
            {
                //Live

                if (Session["UserName"] != null)
                {
                    return RedirectToAction("Index", "TaskList");
                }
                else
                {
                    return RedirectToAction("Login", "Access");
                }

                //End of Live
            }
            else
            {
                //Development

                string nric = ConfigurationManager.AppSettings["DevNric"].ToString();
                string password = EncryptionHelper.Encrypt(ConfigurationManager.AppSettings["DevPassword"].ToString());

                User userData = _usersModel.FindNric(nric);

                if (userData != null)
                {
                    if (userData.Password == password)
                    {
                        if (userData.Status == "Active")
                        {
                            Session.Add("UserID", userData.ID);
                            Session.Add("UserName", userData.Name);
                            Session.Add("UserRole", userData.Role);

                            return RedirectToAction("Index", "TaskList");
                        }
                        else
                        {
                            TempData.Add("Result", "danger|User suspended!");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|Password incorrect!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found!");
                }

                return RedirectToAction("Login", "Access");

                //End of Development
            }
        }

        //
        // GET: Login

        public ActionResult Login()
        {
			if (Request.Cookies["GreatEastForex"] != null)
            {
                var cookie = Request.Cookies["GreatEastForex"];

                User users = _usersModel.GetSingle(Convert.ToInt32(cookie.Values["UserId"]));

                if (users != null)
                {
                    if (users.Status == "Active")
                    {
                        bool login = _usersModel.Login(users.ID);

                        int userid = Convert.ToInt32(users.ID);
                        string tableAffected = "Login Transactions";
                        string description = users.Role + " [" + users.Name + "] Logged In";
                        bool result = AuditLogHelper.WriteAuditLog(userid, tableAffected, description);

                        Session.Add("UserId", users.ID);
                        Session.Add("UserName", users.Name);
                        Session.Add("UserRole", users.Role);

                        return RedirectToAction("Index", "TaskList");
                    }
                    else
                    {
                        TempData.Add("Result", "danger|User suspended!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found!");
                }
            }
            else
            {
                if (Session["UserName"] != null)
                {
                    return RedirectToAction("Index", "TaskList");
                }
            }

            ViewData["userNric"] = "";

            return View();
        }

        //POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            if (Session["UserName"] != null)
            {
                return RedirectToAction("Index", "TaskList");
            }

            string nric = form["Nric"].Trim();
            string password = form["Password"].Trim();

            ViewData["userNric"] = nric;

            if (string.IsNullOrEmpty(nric))
            {
                ModelState.AddModelError("Nric", "NRIC field is required.");
            }

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password field is required.");
            }
            else
            {
                password = EncryptionHelper.Encrypt(password);
            }

            if (ModelState.IsValid)
            {
                User userData = _usersModel.FindNric(nric);

                if (userData != null)
                {
                    if (userData.Password == password)
                    {
                        if (userData.Status == "Active")
                        {
                            bool login = _usersModel.Login(userData.ID);

                            int user = userData.ID;
                            string tableAffected = "Login Transactions";
                            string description = userData.Role + " [" + userData.Name + "] Logged In";
                            bool result = AuditLogHelper.WriteAuditLog(user, tableAffected, description);

                            Session.Add("UserId", userData.ID);
                            Session.Add("UserName", userData.Name);
                            Session.Add("UserRole", userData.Role);

							if (!string.IsNullOrEmpty(form["remember"]))
                            {
                                int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["RememberMeTimeOut"]);
                                System.Web.HttpCookie cookie = new System.Web.HttpCookie("GreatEastForex");
                                cookie.Values.Add("UserId", userData.ID.ToString());
                                cookie.Expires = DateTime.Now.AddDays(timeout);
                                Response.Cookies.Add(cookie);
                            }

							return RedirectToAction("Index", "TaskList");
							//return RedirectToAction("Index", "Customer");
						}
                        else
                        {
                            TempData.Add("Result", "danger|User suspended!");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|Password incorrect!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There are errors in the form!");
            }

            return View();
        }

        //GET: Forgot Password
        public ActionResult ForgotPassword()
        {
            ViewData["userNric"] = "";

            return View();
        }

        // POST: Forget Password

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            string nric = form["Nric"].Trim();

            ViewData["userNric"] = nric;

            if (string.IsNullOrEmpty(nric))
            {
                ModelState.AddModelError("Nric", "Nric field is required.");
            }

            if (ModelState.IsValid)
            {
                User userData = _usersModel.FindNric(nric);

                if (userData != null)
                {
                    // Email
                    string key = EncryptionHelper.GenerateRandomString(8);
                    string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());

                    bool updated = _usersModel.UpdateResetPasswordToken(userData.ID, token);

                    if (updated)
                    {
                        int user = userData.ID;
                        string tableAffected = "Users";
                        string description = userData.Role + " [" + userData.Name + "] Forgot Password ||";

                        string subject = "Reset Password for Website Administration System";
                        string body = System.IO.File.ReadAllText(Server.MapPath("~/Templates/ForgotPassword.html"));
                        string recipient = userData.Email;
                        string name = userData.Name;
                        ListDictionary replacements = new ListDictionary();
                        replacements.Add("<%Name%>", name);
                        replacements.Add("<%Url%>", Url.Action("ResetPassword", "Access", new { @nric = nric, @key = key }, "http"));

                        bool sent = EmailHelper.sendEmail(subject, body, replacements, recipient, userData.ID, "Reset Password");

                        if (sent)
                        {
                            TempData.Add("Result", "success|An email has been sent to you regarding instructions on how to reset your password.");

                            description = userData.Role + " [" + userData.Name + "] Forgot Password || Email Sent";

                            bool result = AuditLogHelper.WriteAuditLog(user, tableAffected, description);

                            return RedirectToAction("Login", "Access");
                        }
                        else
                        {
                            description = userData.Role + " [" + userData.Name + "] Forgot Password || Email Not Sent";

                            bool result = AuditLogHelper.WriteAuditLog(user, tableAffected, description);

                            TempData.Add("Result", "danger|An error occurred while sending email.");
                        }
                    }
                    else
                    {
                        TempData.Add("Result", "danger|An error occurred while generating email.");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found.");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There are errors in the form!");
            }

            return View();
        }

        // GET: Reset Password

        public ActionResult ResetPassword(string nric = "", string key = "")
        {
            ViewData["userNric"] = nric;

            if (String.IsNullOrEmpty(key))
            {
                TempData.Add("Result", "danger|No key provided!");

                return RedirectToAction("ForgotPassword", "Access");
            }
            else
            {
                string token = EncryptionHelper.GenerateHash(key, ConfigurationManager.AppSettings["Salt"].ToString());

                User userData = _usersModel.FindNric(nric);

                if (userData != null)
                {
                    if (userData.ResetPasswordToken != token)
                    {
                        TempData.Add("Result", "danger|Invalid key provided!");

                        return RedirectToAction("ForgotPassword", "Access");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found!");

                    return RedirectToAction("ForgotPassword", "Access");
                }
            }

            return View();
        }

        //POST: ResetPassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(FormCollection form)
        {
            string nric = form["Nric"].Trim();
            string password = form["Password"].Trim();
            string confirmPassword = form["ConfirmPassword"].Trim();

            ViewData["userNric"] = nric;

            if (string.IsNullOrEmpty(nric))
            {
                ModelState.AddModelError("Nric", "NRIC is required!");
            }

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required!");
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Confirm Password is required!");
            }
            else
            {
                if (!string.IsNullOrEmpty(password) && password != confirmPassword)
                {
                    ModelState.AddModelError("Password", "New Password and Confirm Password not match!");
                }
                else
                {
                    password = EncryptionHelper.Encrypt(password);
                }
            }

            if (ModelState.IsValid)
            {
                User userData = _usersModel.FindNric(nric);

                if (userData != null)
                {
                    userData.Password = password;
                    userData.ResetPasswordToken = null;

                    bool updated = _usersModel.Update(userData.ID, userData);

                    if (updated)
                    {
                        int user = userData.ID;
                        string tableAffected = "Users";
                        string description = userData.Role + " [" + userData.Name + "] Reset Password";

                        bool result = AuditLogHelper.WriteAuditLog(user, tableAffected, description);

                        TempData.Add("Result", "success|Password updated! You may login using your new password!");

                        return RedirectToAction("Login", "Access");
                    }
                    else
                    {
                        TempData.Add("Result", "danger|An error occurred while updating password!");
                    }
                }
                else
                {
                    TempData.Add("Result", "danger|User record not found!");
                }
            }
            else
            {
                TempData.Add("Result", "danger|There are errors in the form!");
            }

            return View();
        }

        // GET: Logout

        public ActionResult Logout()
        {
            string result = "";

            if (TempData["Result"] != null)
            {
                result = TempData["Result"].ToString();
                TempData.Remove("Result");
            }

            Session.RemoveAll();

            if (Response.Cookies["GreatEastForex"] != null)
            {
                System.Web.HttpCookie cookie = Response.Cookies["GreatEastForex"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            if (!string.IsNullOrEmpty(result))
            {
                TempData.Add("Result", result);
            }

            return RedirectToAction("Login", "Access");
        }
    }
}