using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class MemoCurrencyTable
    {
        public int RowId { get; set; }

        public string Currency { get; set; }

        public string Rate { get; set; }

        public string CrossRate { get; set; }

        public string Amount { get; set; }

        public string PaymentMode { get; set; }

        public string TotalAmount { get; set; }

		public string VesselName { get; set; }
    }
}