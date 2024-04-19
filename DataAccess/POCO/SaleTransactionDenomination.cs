using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class SaleTransactionDenomination
    {
        [Key]
        public int ID { get; set; }

        public int SaleTransactionId { get; set; }

        public int Denomination { get; set; }

        public int Pieces { get; set; }

        public decimal AmountForeign { get; set; }

        [ForeignKey("SaleTransactionId")]
        public virtual SaleTransaction SaleTransactions { get; set; }
    }
}