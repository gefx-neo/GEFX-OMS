using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Beneficiaries
    {
        [Key]
        public long ID { get; set; }

		[Display(Name = "Customer:")]
		public int CustomerParticularId { get; set; }

		[Display(Name = "Who do you want to send the money to?:")]
		public int Type { get; set; }

		[Display(Name = "Is this your account?:")]
		public int IsYourAccount { get; set; }

		[Display(Name = "Beneficiary's Friendly Name:")]
		public string BeneficiaryFriendlyName { get; set; }

		[Display(Name = "Beneficiary's Full Name:")]
		public string BeneficiaryFullName { get; set; }

		[Display(Name = "Beneficiary's Contact No.:")]
		public string BeneficiaryContactNoMain { get; set; }

		[Display(Name = "Beneficiary's Address:")]
		public string BeneficiaryAddressMain { get; set; }

		[Display(Name = "Beneficiary's Bank Name:")]
		public string BeneficiaryBankName { get; set; }

		[Display(Name = "BIC / EUROPE IBAN NO / AUSTRALIA BSB / USA FED WIRE / USA ABA / UK SORT:")]
		public string IBANEuropeBSBAustralia { get; set; }

		[Display(Name = "Beneficiary's Nationality:")]
		public int BeneficiaryNationality { get; set; }

		[Display(Name = "Beneficiary's Nationality If Others:")]
		public string BeneficiaryNationalityIfOthers { get; set; }

		[Display(Name = "Beneficiary's Company Registration No.:")]
		public string BeneficiaryCompanyRegistrationNo { get; set; }

		[Display(Name = "Beneficiary's Category Business:")]
		public int BeneficiaryBusinessCategory { get; set; }

		[Display(Name = "Beneficiary's Category Business If Others:")]
		public string BeneficiaryBusinessCategoryIfOthers { get; set; }

		[Display(Name = "Beneficiary's Company Contact No.:")]
		public string BeneficiaryContactNo { get; set; }

		[Display(Name = "Bank Type:")]
		public int BankType { get; set; }

		[Display(Name = "Bank Code / SWIFT:")]
		public string BankCode { get; set; }

		[Display(Name = "Account Number::")]
		public string BankAccountNo { get; set; }

		[Display(Name = "Bank Country:")]
		public int BankCountry { get; set; }

		[Display(Name = "Bank Country If Others:")]
		public string BankCountryIfOthers { get; set; }

		[Display(Name = "Bank Address:")]
		public string BankAddress { get; set; }

		[Display(Name = "Default Purpose of Payment:")]
		public int PurposeOfPayment { get; set; }

		[Display(Name = "Default Purpose of Payment If Others:")]
		public string PurposeOfPaymentIfOthers { get; set; }

		[Display(Name = "Default Source of Payment:")]
		public int SourceOfPayment { get; set; }

		[Display(Name = "Default Source of Payment If Others:")]
		public string SourceOfPaymentIfOthers { get; set; }

		[Display(Name = "Default Payment Details / Instructions:")]
		public string PaymentDetails { get; set; }

		[Display(Name = "Status:")]
		public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

		[ForeignKey("CustomerParticularId")]
		public virtual CustomerParticular CustomerParticulars { get; set; }
	}
}