using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class KYC_CustomerOthers
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		[Display(Name = "Status*:")]
		public string Status { get; set; }

		public string NewStatus { get; set; }

		[Display(Name = "Approval By*:")]
		public int ApprovalBy { get; set; }

		[Display(Name = "Screening Results*:")]
		public string ScreeningResults { get; set; }

		[Display(Name = "Screening Results Document:")]
		public string ScreeningResultsDocument { get; set; }

		[Display(Name = "Grading*:")]
		[Required(ErrorMessage = "Grading is required!")]
		public string Grading { get; set; }

		[Display(Name = "Next Review Date:")]
		public DateTime? NextReviewDate { get; set; }

		[Display(Name = "Acra Expiry:")]
		public DateTime? AcraExpiry { get; set; }

		[Display(Name = "Bank Account No.")]
		public string BankAccountNo { get; set; }

		[Display(Name = "GM Approval Above*:")]
		[Required(ErrorMessage = "GM Approval Above is required!")]
		public int GMApprovalAbove { get; set; }

		[Display(Name = "Customer Profile*:")]
		[Required(ErrorMessage = "Customer Profile is required!")]
		public string CustomerProfile { get; set; }

		[Display(Name = "Sales Remarks:")]
		public string SalesRemarks { get; set; }

		[ForeignKey("CustomerParticularId")]
		public virtual KYC_CustomerParticulars CustomerParticulars { get; set; }
	}
}