using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class ViewLog
    {
        public int ID { get; set; }

        public string Type { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }
    }
}