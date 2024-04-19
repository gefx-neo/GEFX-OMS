using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.Report
{
    public class CurrencyClosingBalance
    {
        public int ProductId { get; set; }

        public string Code { get; set; }

        public string Currency { get; set; }

        public string AmountForeignFormat { get; set; }

        public decimal ForeignCurrencyClosingBal { get; set; }

        public decimal AveragePurchaseCostOrLastBuyingRate { get; set; }

        public decimal ClosingBalAtAveragePurchaseOrLastBuying { get; set; }

        public string Description { get; set; }
    }
}