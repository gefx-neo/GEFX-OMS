using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Remittances
    {
        [Key]
        public int ID { get; set; }

		[Display(Name = "Memo ID:")]
		public string MemoID { get; set; }

		[Display(Name = "Customer*:")]
		public int CustomerParticularId { get; set; }

		[Display(Name = "Contact No.:")]
		public string ContactNo { get; set; }

		[Display(Name = "Address:")]
		public string Address1 { get; set; }

		[Display(Name = "")]
		public string Address2 { get; set; }

		[Display(Name = "")]
		public string Address3 { get; set; }

		[Display(Name = "Shipping Address:")]
		public string ShippingAddress1 { get; set; }

		[Display(Name = "")]
		public string ShippingAddress2 { get; set; }

		[Display(Name = "")]
		public string ShippingAddress3 { get; set; }

		[Display(Name = "Issue Date*:")]
		public DateTime IssueDate { get; set; }

		[Display(Name = "Created By*:")]
		public int CreatedBy { get; set; }

        public string CreatedFrom { get; set; }

		[Display(Name = "Urgent*:")]
		public int IsUrgent { get; set; }

		[Display(Name = "Remarks:")]
		public string Remarks { get; set; }

		[Display(Name = "Customer Remarks:")]
		public string CustomerRemarks { get; set; }

		[Display(Name = "Agent:")]
		public int AgentId { get; set; }

		[Display(Name = "Cost Price:")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Only allow two decimal points.")]
        public decimal CostPrice { get; set; }

        [Display(Name = "Agent Rate:")]
        [RegularExpression(@"^\d+.?\d{0,12}$", ErrorMessage = "Only allow twelve decimal points.")]
        public decimal? AgentRate { get; set; }

        [Display(Name = "Agent Fee:")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Only allow two decimal points.")]
        public decimal? AgentFee { get; set; }

        [Display(Name = "Status *:")]
        public string Status { get; set; }

        public decimal TotalPayAmount { get; set; }

        public decimal TotalGetAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime LastApprovalOn { get; set; }

        public string IsDeleted { get; set; }
        public string DisapprovedReason { get; set; }

        [ForeignKey("CustomerParticularId")]
        public virtual CustomerParticular CustomerParticulars { get; set; }

        public virtual List<RemittanceOrders> RemittanceOders { get; set; }

        public virtual Agents Agent { get; set; }
    }
}