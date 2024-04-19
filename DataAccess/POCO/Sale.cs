using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Sale
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Memo ID:")]
        public string MemoID { get; set; }

        [Display(Name = "Customer*:")]
        [Required(ErrorMessage = "Customer is required!")]
        public int CustomerParticularId { get; set; }

        [Display(Name = "Contact Person:")]
        public string Sale_ContactPerson { get; set; }

        [Display(Name = "Issue Date*:")]
        //[Required(ErrorMessage = "Issue Date is required!")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Collection Date/Time*:")]
        [Required(ErrorMessage = "Collection Date is required!")]
        public DateTime? CollectionDate { get; set; }

        [Required(ErrorMessage = "Collection Time is required!")]
        public string CollectionTime { get; set; }

        [Display(Name = "Created By*:")]
        //[Required(ErrorMessage = "Created By is required!")]
        public int CreatedBy { get; set; }

        [Display(Name = "Urgent*:")]
        [Required(ErrorMessage = "Urgent is required!")]
        public string Urgent { get; set; }

        [Display(Name = "Require Delivery:")]
        public string RequireDelivery { get; set; }

        [Display(Name = "Bag No.:")]
        public string BagNo { get; set; }

        [Display(Name = "Remarks:")]
        public string Remarks { get; set; }

        [Display(Name = "Customer Remarks:")]
        public string CustomerRemarks { get; set; }

        [Display(Name = "Delivery Remarks:")]
        public string Sale_DeliveryRemarks { get; set; }

        [Display(Name = "Transaction Type*:")]
        [Required(ErrorMessage = "Transaction Type is required!")]
        public string TransactionType { get; set; }

        [Display(Name = "Local Payment Mode*:")]
        //[Required(ErrorMessage = "Local Payment Mode is required!")]
        public string LocalPaymentMode { get; set; }

        [Display(Name = "Cash Amount")]
        public decimal? CashAmount { get; set; }

        public string CashBank { get; set; }

        public string Cheque1No { get; set; }

        public decimal? Cheque1Amount { get; set; }

        public string Cheque1Bank { get; set; }

        public string Cheque2No { get; set; }

        public decimal? Cheque2Amount { get; set; }

        public string Cheque2Bank { get; set; }

        public string Cheque3No { get; set; }

        public decimal? Cheque3Amount { get; set; }

        public string Cheque3Bank { get; set; }

        public string BankTransferNo { get; set; }

        public decimal? BankTransferAmount { get; set; }

        public string BankTransferBank { get; set; }

        [Display(Name = "Memo Balance*:")]
        public decimal? MemoBalance { get; set; }

        public decimal TotalAmountLocal { get; set; }

        public decimal TotalAmountForeign { get; set; }

		public string CollectionCode { get; set; }

		[Display(Name = "Status:")]
        public string Status { get; set; }

        public DateTime LastApprovalOn { get; set; }

        public int? PendingDeliveryById { get; set; }

        public string DeliveryConfirmation { get; set; }

        public string DisapprovedReason { get; set; }

		public string CustomerContact { get; set; }

		public string CustomerVesselName { get; set; }

		public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

		[Timestamp]
		public byte[] Version { get; set; }

		public Sale()
        {
            IssueDate = DateTime.Now.Date;
            CollectionDate = DateTime.Now.Date;
            MemoBalance = 0;
        }

        [ForeignKey("CustomerParticularId")]
        public virtual CustomerParticular CustomerParticulars { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Users { get; set; }

        [ForeignKey("PendingDeliveryById")]
        public virtual User Deliveryman { get; set; }

        public virtual List<SaleTransaction> SaleTransactions { get; set; }
    }
}