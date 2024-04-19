using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class EmailLog
    {
        [Key]
        public int ID { get; set; }

        public int UserTriggeringId { get; set; }

        public string EmailType { get; set; }

        public int? SaleId { get; set; }

        public string ReceiverEmail { get; set; }

        public string CcEmail { get; set; }

        public string BccEmail { get; set; }

        public string Subject { get; set; }

        public string EmailContent { get; set; }

        public string Attachments { get; set; }

        public DateTime Timestamp { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        [ForeignKey("UserTriggeringId")]
        public virtual User UserTriggering { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }
    }
}