using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class EndDayTrade
    {
        [Key]
        public int ID { get; set; }

        public DateTime LastActivationTime { get; set; }

        public DateTime CurrentActivationTime { get; set; }

        public int TriggeringTime { get; set; }

        public int CurrencyId { get; set; }

        public decimal OpeningBankAmount { get; set; }

        public decimal OpeningCashAmount { get; set; }

        public decimal OpeningForeignCurrencyBalance { get; set; }

        public decimal OpeningAveragePurchaseCost { get; set; }

        public decimal OpeningBalanceAtAveragePurchase { get; set; }

        public decimal OpeningProfitAmount { get; set; }

        public decimal ClosingBankAmount { get; set; }

        public decimal ClosingCashAmount { get; set; }

        public decimal ClosingForeignCurrencyBalance { get; set; }

        public decimal ClosingAveragePurchaseCost { get; set; }

        public decimal ClosingBalanceAtAveragePurchase { get; set; }

        public decimal ClosingProfitAmount { get; set; }

        public decimal CurrentSGDBuyRate { get; set; }

        public string Description { get; set; }

        public string IsDeleted { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Product Products { get; set; }

        public List<EndDayTradeTransaction> EndDayTradeTransactions { get; set; }
    }
}