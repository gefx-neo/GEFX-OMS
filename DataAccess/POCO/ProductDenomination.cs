using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class ProductDenomination
    {
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Denomination Value:")]
        public int DenominationValue { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}