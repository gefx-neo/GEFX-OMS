using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Inventory
    {
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Type*:")]
        [Required(ErrorMessage = "Type is required!")]
        public string Type { get; set; }

        [Display(Name = "Amount*:")]
        [Required(ErrorMessage = "Amount is required!")]
        public decimal Amount { get; set; }

        [Display(Name = "Description*:")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}