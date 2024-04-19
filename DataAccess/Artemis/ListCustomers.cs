using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class ListCustomers
    {
        public int count { get; set; }
        public string next { get; set; }
        public string prev { get; set; }

        public List<results> results { get; set; }
    }

    public class results //customer
    {
        public int id { get; set; }
        public createdBy createdBy { get; set; }
        public updatedBy updatedBy { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string onboardingMode { get; set; }
        public string productServiceComplexity { get; set; }
        public string[] paymentModes { get; set; }
        public string profileType { get; set; }
        public bool isActiveCustomer { get; set; }
        public string natureOfBusinessRelationship { get;set; }
        public string referenceId { get; set; }
        public string integrationStatus { get; set; }
        public string callbackMessage { get; set; }
        public string annualIncomeLevel { get; set; }

        public string[] sourceOfWealth { get; set; }
        public string policyCount { get; set; }
        public string premiumAmount { get; set; }
        public string customerStatus { get; set; }
        public string customerRiskRating { get; set; }
        public int dialog { get; set; }
        public int[] domains { get; set; }
        public string[] users { get; set; }
        public string name { get; set; }

        public string customerType { get; set; }
        public string status { get; set; }
        public string riskRating { get; set; }

        //cprs part
        public string recordType { get; set; }
        public latestScreeningConclusion latestScreeningConclusion { get; set; }
        public bool primary { get; set; }
        public int customer { get; set; }
        public int? primaryCrpOf { get; set; }
        public int record { get; set; }
        public int fileBucket { get; set; }
        public List<roles> roles { get; set; }

        public individualRecords individualRecords { get; set; }
        public corporateRecords corporateRecords { get; set; }
    }

    public class createdBy
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string oauthId { get; set; }
        public bool isActive { get; set; }
        public bool mfaEnabled { get; set; }
    }

    public class updatedBy
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string oauthId { get; set; }
        public bool isActive { get; set; }
        public bool mfaEnabled { get; set; }
    }

    public class individualRecords
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("primary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool primary { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty("recordType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string recordType { get; set; }
        [JsonProperty("aliasNames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] aliasNames { get; set; }
        [JsonProperty("phoneNumbers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] phoneNumbers { get; set;}
        [JsonProperty("addresses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] addresses { get; set; }
        [JsonProperty("oldNames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] oldNames { get; set; }
        [JsonProperty("sourceOfFunds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string sourceOfFunds { get; set;}
        [JsonProperty("emailAddresses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] emailAddresses { get; set; }
        [JsonProperty("bankAccounts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] bankAccounts { get; set; }
        [JsonProperty("gender", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string gender { get; set; }
        [JsonProperty("nationality", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string nationality { get; set; }
        [JsonProperty("countryOfResidence", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string countryOfResidence { get; set; }
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string title { get; set; }
        [JsonProperty("countryOfBirth", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string countryOfBirth { get; set; }
        [JsonProperty("dateOfBirth", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string dateOfBirth { get; set; }
        [JsonProperty("industry", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string industry { get; set; }
        [JsonProperty("occupation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string occupation { get; set; }
        [JsonProperty("idType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string idType { get; set; }
        [JsonProperty("idNumber", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string idNumber { get; set; }
        [JsonProperty("idIssueDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string idIssueDate { get; set; }
        [JsonProperty("idExpiryDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string idExpiryDate { get; set; }
        [JsonProperty("dialog", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int dialog { get; set; }
        [JsonProperty("fileBucket", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int fileBucket { get; set; }
    }

    public class corporateRecords
    {
        public handshakesReports handshakesReport { get; set; }
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("primary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool primary { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty("recordType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string recordType { get; set; }
        [JsonProperty("referenceId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string referenceId { get; set; }
        [JsonProperty("aliasNames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] aliasNames { get; set; }
        [JsonProperty("phoneNumbers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] phoneNumbers { get; set; }
        [JsonProperty("addresses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] addresses { get; set; }
        [JsonProperty("oldNames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] oldNames { get; set; }
        [JsonProperty("sourceOfFunds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string sourceOfFunds { get; set; }
        [JsonProperty("emailAddresses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] emailAddresses { get; set; }
        [JsonProperty("bankAccounts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] bankAccounts { get; set; }
        [JsonProperty("isIncorporated", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool isIncorporated { get; set; }
        [JsonProperty("entityType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string entityType { get; set; }
        [JsonProperty("countryOfOperations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string countryOfOperations { get; set; }
        [JsonProperty("countryOfIncorporation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string countryOfIncorporation { get; set; }
        [JsonProperty("ownershipStructureLayers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ownershipStructureLayers { get; set; }
        [JsonProperty("businessActivity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string businessActivity { get; set; }
        [JsonProperty("website", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string website { get; set; }
        [JsonProperty("incorporationNumber", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string incorporationNumber { get; set; }
        [JsonProperty("incorporationDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string incorporationDate { get; set; }
        [JsonProperty("incorporationExpiryDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string incorporationExpiryDate { get; set; }
        [JsonProperty("inFatfJurisdiction", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool inFatfJurisdiction { get; set; }
        [JsonProperty("dialog", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int dialog { get; set; }
        [JsonProperty("fileBucket", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int fileBucket { get; set; }
    }

    public class handshakesReports
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("reportDateTime", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string reportDateTime { get; set; }
        [JsonProperty("activeName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string activeName { get; set; }
        [JsonProperty("xmlData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string xmlData { get; set; }
        [JsonProperty("directRedScore", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string directRedScore { get; set; }
        [JsonProperty("indirectRedScore", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string indirectRedScore { get; set;}
        [JsonProperty("networkScoreDegreeCentrality", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string networkScoreDegreeCentrality { get; set; }
        [JsonProperty("entityGuid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string entityGuid { get; set; }
        [JsonProperty("dataset", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string dataset { get; set; }
        [JsonProperty("otherNames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string otherNames { get; set; }
        [JsonProperty("identificationNumber", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string identificationNumber { get; set; }
        [JsonProperty("currentAddress", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string currentAddress { get; set; }
        [JsonProperty("otherAddress", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string otherAddress { get; set; }
        [JsonProperty("incorporationDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string incorporationDate { get; set; }
        [JsonProperty("incorporationCitizenship", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string incorporationCitizenship { get; set; }
        [JsonProperty("companyType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string companyType { get; set; }
        [JsonProperty("companyStatus", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string companyStatus { get; set; }
        [JsonProperty("primaryBusinessActivity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string primaryBusinessActivity { get; set; }
        [JsonProperty("secondaryBusinessActivity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string secondaryBusinessActivity { get; set; }
        [JsonProperty("ownershipLayers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ownershipLayers { get; set; }
        [JsonProperty("createdBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdBy { get; set; }
        [JsonProperty("updatedBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedBy { get; set; }
        [JsonProperty("record", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int record { get; set; }
    }

    public class roles
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("createdBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdBy { get; set; }
        [JsonProperty("updatedBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedBy { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("dateAppointed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string dateAppointed { get; set; }
        [JsonProperty("dateResigned", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string dateResigned { get; set; }
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string role { get; set; }
        [JsonProperty("crp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int crp { get; set; }
    }

    public class latestScreeningConclusion
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("invalid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool invalid { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("pep", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool pep { get; set; }
        [JsonProperty("sanction", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool sanction { get; set; }
        [JsonProperty("adverseNews", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool adverseNews { get; set; }
        [JsonProperty("noHit", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool noHit { get; set; }
        [JsonProperty("isSystemGenerated", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool isSystemGenerated { get; set; }
        [JsonProperty("crp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string crp { get; set; }
    }
}