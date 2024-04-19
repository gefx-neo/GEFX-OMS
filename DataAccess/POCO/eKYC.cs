using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.POCO
{
    [Table("eKYC")]
    public class KYC
    {
        [Key]
        public int ID { get; set; }
        public int cust_id { get; set; }
        [Display(Name = "Artemis Screening Status:")]
        public int status { get; set; }
        public int Artemis_custId { get; set; }
        public DateTime date_created { get; set; }
    }
}