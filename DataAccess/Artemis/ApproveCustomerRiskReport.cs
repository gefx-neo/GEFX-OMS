using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class ApproveCustomerRiskReport
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("isOutdated", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool isOutdated { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        //[JsonProperty("riskJson", DefaultValueHandling = DefaultValueHandling.Ignore)]
        //public string riskJson { get; set; }
        [JsonProperty("riskRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string riskRating { get; set; }
        [JsonProperty("outdated", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool outdated { get; set; }
        [JsonProperty("customer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int customer { get; set; }
        [JsonProperty("latestApprovalStatus", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public latestApprovalStatus latestApprovalStatus { get; set; }
    }

    public class latestApprovalStatus
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
        [JsonProperty("overrideRisk", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string overrideRisk { get; set; }
        [JsonProperty("approvalStatus", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string approvalStatus { get; set; }
        [JsonProperty("riskReport", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int riskReport { get; set; }
        [JsonProperty("notifyPerson", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string notifyPerson { get; set; }
    }
}