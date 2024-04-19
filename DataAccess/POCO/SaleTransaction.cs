using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class SaleTransaction
    {
        [Key]
        public int ID { get; set; }

        public int SaleId { get; set; }

        [Display(Name = "ID")]
        [Required(ErrorMessage = "ID is required!")]
        public string TransactionID { get; set; }

        [Display(Name = "Transaction Type")]
        [Required(ErrorMessage = "Transaction Type is required!")]
        public string TransactionType { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required!")]
        public int CurrencyId { get; set; }

        [Display(Name = "Rate")]
        [Required(ErrorMessage = "Rate is required!")]
        public decimal Rate { get; set; }

        [Display(Name = "Encashment Rate")]
        //[Required(ErrorMessage = "Encashment Rate is required!")]
        public decimal? EncashmentRate { get; set; }

        [Display(Name = "Cross Rate")]
        //[Required(ErrorMessage = "Cross Rate is required!")]
        public decimal? CrossRate { get; set; }

        [Display(Name = "Unit")]
        [Required(ErrorMessage = "Unit is required!")]
        public int Unit { get; set; }

        [Display(Name = "Amount (Foreign)")]
        [Required(ErrorMessage = "Amount (Foreign) is required!")]
        public decimal AmountForeign { get; set; }

        [Display(Name = "Amount (Local)")]
        [Required(ErrorMessage = "Amount (Local) is required!")]
        public decimal AmountLocal { get; set; }

        //[Display(Name = "Classification")]
        //[Required(ErrorMessage = "Classification is required!")]
        //public string Classification { get; set; }

        [Display(Name = "Payment Mode")]
        //[Required(ErrorMessage = "Payment Mode is required!")]
        public string PaymentMode { get; set; }

        [Display(Name = "Cheque No.")]
        public string ChequeNo { get; set; }

        [Display(Name = "Bank Transfer No.")]
        public string BankTransferNo { get; set; }

        [Display(Name = "Vessel Name")]
        public string VesselName { get; set; }

		public string YouGetPaymentType { get; set; }

		public string YouPayAccount { get; set; }

		public string BeneficiaryBankAccNo { get; set; }

		public string BeneficiaryBankAddress { get; set; }

		public string BeneficiaryBankName { get; set; }

		[ForeignKey("SaleId")]
        public virtual Sale Sales { get; set; }

        public virtual List<SaleTransactionDenomination> SaleTransactionDenominations { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Product Products { get; set; }
    }
}