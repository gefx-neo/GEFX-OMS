using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class FlowNeededItem
    {
        [JsonProperty("CustomerID", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CustomerID { get; set; }
        [JsonProperty("CRPID", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<int> CRPID { get; set; }
        [JsonProperty("RiskReportID", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RiskReportID { get; set; }
        [JsonProperty("RecordID", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RecordID { get; set; }

    }
}