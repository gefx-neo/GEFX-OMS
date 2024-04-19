using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class ConvertFromResponse
    {
        public string terms { get; set; }
        public string privacy { get; set; }
        public string from { get; set; }
        public double amount { get; set; }
        public string timestamp { get; set; }
        public Rate[] to { get; set; }

        public class Rate
        {
            public string quotecurrency { get; set; }
            public double mid { get; set; }
        }
    }
}