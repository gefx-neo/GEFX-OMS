using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class SingleCustomerByID
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("createdBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public createdBy createdBy { get; set; }
        [JsonProperty("updatedBy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public updatedBy updatedBy { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("onboardingMode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string onboardingMode { get; set; }
        [JsonProperty("productServiceComplexity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string productServiceComplexity { get; set; }
        [JsonProperty("paymentModes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] paymentModes { get; set; }
        [JsonProperty("profileType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string profileType { get; set; }
        [JsonProperty("isActiveCustomer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool isActiveCustomer { get; set; }
        [JsonProperty("natureOfBusinessRelationship", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string natureOfBusinessRelationship { get; set; }
        [JsonProperty("referenceId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string referenceId { get; set; }
        [JsonProperty("integrationStatus", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string integrationStatus { get; set; }
        [JsonProperty("callbackMessage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string callbackMessage { get; set; }
        [JsonProperty("annualIncomeLevel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string annualIncomeLevel { get; set; }
        [JsonProperty("sourceOfWealth", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] sourceOfWealth { get; set; }
        [JsonProperty("policyCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string policyCount { get; set; }
        [JsonProperty("premiumAmount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string premiumAmount { get; set; }
        [JsonProperty("customerStatus", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string customerStatus { get; set; }
        [JsonProperty("customerRiskRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string customerRiskRating { get; set; }

        [JsonProperty("dialog", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int dialog { get; set; }
        [JsonProperty("domains", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int[] domains { get; set; }
        [JsonProperty("users", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] users { get; set; }
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("customerType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string customerType { get; set; }
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string status { get; set; }
        [JsonProperty("riskRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string riskRating { get; set; }
        [JsonProperty("individualRecords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<individualRecords> individualRecords { get; set; }

        [JsonProperty("corporateRecords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<corporateRecords> corporateRecords { get; set; }
    }
}