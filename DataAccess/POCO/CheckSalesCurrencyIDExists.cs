using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class CheckSalesCurrencyIDExists
    {
        public int ID { get; set; }

        public string isDeleted { get; set; }

        public int SaleId { get; set; }

        public int CurrencyId { get; set; }
    }
}