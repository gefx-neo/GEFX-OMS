using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class SingleCustomerCRP
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty("recordType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string recordType { get; set; }
        [JsonProperty("roles", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<roles> roles { get; set; }
        [JsonProperty("latestScreeningConclusion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public latestScreeningConclusion latestScreeningConclusion { get; set; }
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string status { get; set; }
        [JsonProperty("corporateRecord", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public corporateRecords corporateRecord { get; set; }
        [JsonProperty("individualRecord", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public individualRecords individualRecord { get; set; }
        [JsonProperty("primary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool primary { get; set; }
        [JsonProperty("customer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int customer { get; set; }
        [JsonProperty("primaryCrpOf", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int primaryCrpOf { get; set; }
        [JsonProperty("record", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int record { get; set; }
        [JsonProperty("fileBucket", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int fileBucket { get; set; }
        [JsonProperty("dialog", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int dialog { get; set; }
        [JsonProperty("createdAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string createdAt { get; set; }
        [JsonProperty("updatedAt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string updatedAt { get; set; }
    }
}