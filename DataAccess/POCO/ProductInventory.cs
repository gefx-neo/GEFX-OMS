using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class ProductInventory
    {
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Total in Account:")]
        public decimal TotalInAccount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}