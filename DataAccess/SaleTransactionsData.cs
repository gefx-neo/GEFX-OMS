using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataAccess
{
    public class SaleTransactionsData
    {
        public string RowId { get; set; }

        public int ID { get; set; }

        public string TransactionID { get; set; }

        public string Type { get; set; }

        public SelectList CurrencyDDL { get; set; }

        public string Symbol { get; set; }

        public string Rate { get; set; }

        public string EncashmentRate { get; set; }

        public string CrossRate { get; set; }

        public int Unit { get; set; }

        public string AmountForeign { get; set; }

        public string AmountLocal { get; set; }

        public SelectList PaymentModeDDL { get; set; }

        public string DisabledChequeNo { get; set; }

        public string ChequeNo { get; set; }

        public string DisabledBankTransferNo { get; set; }

        public string BankTransferNo { get; set; }

        public string VesselName { get; set; }

        public IList<DenominationsData> Denominations { get; set; }

        public string DenominationCurrencyCode { get; set; }

        public string DenominationTotalCalculatedForeign { get; set; }

        public string DenominationRemainingForeign { get; set; }

        public string DenominationTotalOrderForeign { get; set; }
    }

    public class DenominationsData
    {
        public string DenoId { get; set; }

        public string DenominationValue { get; set; }

        public string Pieces { get; set; }

        public string AmountForeign { get; set; }
    }

    public class CountAccountCode
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}