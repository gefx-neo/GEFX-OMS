using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class EndDayTradeTransaction
    {
        [Key]
        public int ID { get; set; }

        public int TradeId { get; set; }

        public int SaleTransactionId { get; set; }

        public decimal AmountForeign { get; set; }

        public int Unit { get; set; }

        public decimal AmountLocal { get; set; }

        [ForeignKey("TradeId")]
        public virtual EndDayTrade EndDayTrade { get; set; }

        [ForeignKey("SaleTransactionId")]
        public virtual SaleTransaction SaleTransaction { get; set; }
    }
}