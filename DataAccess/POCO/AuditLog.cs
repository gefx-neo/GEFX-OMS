using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class AuditLog
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        [Display(Name = "User Triggering")]
        public int UserTriggering { get; set; }

        [Display(Name = "Table Affected")]
        public string TableAffected { get; set; }

        [Display(Name = "Desctiprion")]
        public string Description { get; set; }

        [ForeignKey("UserTriggering")]
        public virtual User Users { get; set; }
    }
}