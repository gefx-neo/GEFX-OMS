using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class ScanOutgoing
    {
        [Key]
        public int ID { get; set; }

        public int SaleId { get; set; }

        public string Status { get; set; }

        public DateTime? ConfirmedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string IsDeleted { get; set; }

        public int ScanById { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale Sales { get; set; }
    }
}