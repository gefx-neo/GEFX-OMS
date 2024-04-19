using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class KYC_CustomerParticulars
	{
		[Key]
		public int ID { get; set; }

		public int Customer_MainID { get; set; }

		[Display(Name = "Customer Code*:")]
		public string CustomerCode { get; set; }

		[Display(Name = "Customer Type*:")]
		public string CustomerType { get; set; }

		[Display(Name = "Registered Name*:")]
		public string Company_RegisteredName { get; set; }

		[Display(Name = "Registered Address*:")]
		public string Company_RegisteredAddress { get; set; }

		[Display(Name = "Business Address*:")]
		public string Company_BusinessAddress1 { get; set; }

		[Display(Name = "Business Address*:")]
		public string Company_BusinessAddress2 { get; set; }

		public string Company_BusinessAddress3 { get; set; }

		[Display(Name = "Postal Code*:")]
		public string Company_PostalCode { get; set; }

		[Display(Name = "Contact Name*:")]
		public string Company_ContactName { get; set; }

		[Display(Name = "Tel No.*:")]
		public string Company_TelNo { get; set; }

		[Display(Name = "Fax No.:")]
		public string Company_FaxNo { get; set; }

		[Display(Name = "Email*:")]
		public string Company_Email { get; set; }

		[Display(Name = "Place of Registration*:")]
		public string Company_PlaceOfRegistration { get; set; }

		[Display(Name = "Date of Registration*:")]
		public DateTime? Company_DateOfRegistration { get; set; }

		[Display(Name = "Registration No.*:")]
		public string Company_RegistrationNo { get; set; }

		[Display(Name = "Type of Entity*:")]
		public string Company_TypeOfEntity { get; set; }

		[Display(Name = "If Others")]
		public string Company_TypeOfEntityIfOthers { get; set; }

		[Display(Name = "Purpose and Intended Nature of Account Relationship and/or Relevant Business Transaction Undertaken")]
		public string Company_PurposeAndIntended { get; set; }

		[Display(Name = "Contact No. (Home)*:")]
		public string Company_ContactNoH { get; set; }

		[Display(Name = "Contact No. (Office)*:")]
		public string Company_ContactNoO { get; set; }

		[Display(Name = "Contact No. (Mobile)*:")]
		public string Company_ContactNoM { get; set; }

		[Display(Name = "IC/Passport No.*:")]
		public string Company_ICPassport { get; set; }

		[Display(Name = "Job Title*:")]
		public string Company_JobTitle { get; set; }

		[Display(Name = "Nationality*:")]
		public string Company_Nationality { get; set; }

		public int Company_Country { get; set; }

		public int Company_CountryCode { get; set; }

		[Display(Name = "Name*:")]
		public string Natural_Name { get; set; }

		[Display(Name = "Permanent Address*:")]
		public string Natural_PermanentAddress { get; set; }

		public string Natural_PermanentAddress2 { get; set; }

		public string Natural_PermanentAddress3 { get; set; }

		[Display(Name = "Permanent Postal Code*:")]
		public string Natural_PermanentPostalCode { get; set; }

		[Display(Name = "Mailing Address*:")]
		public string Natural_MailingAddress { get; set; }

		public string Natural_MailingAddress2 { get; set; }

		public string Natural_MailingAddress3 { get; set; }

		[Display(Name = "Nationality*:")]
		public string Natural_Nationality { get; set; }

		[Display(Name = "IC/Passport No.*:")]
		public string Natural_ICPassportNo { get; set; }

		[Display(Name = "Date of Birth*:")]
		public DateTime? Natural_DOB { get; set; }

		[Display(Name = "Contact No. (H)*:")]
		public string Natural_ContactNoH { get; set; }

		[Display(Name = "Contact No. (O)*:")]
		public string Natural_ContactNoO { get; set; }

		[Display(Name = "Contact No. (M)*:")]
		public string Natural_ContactNoM { get; set; }

		[Display(Name = "Email:")]
		public string Natural_Email { get; set; }

		[Display(Name = "Employment Type*:")]
		public string Natural_EmploymentType { get; set; }

		[Display(Name = "Name of Employer*:")]
		public string Natural_EmployedEmployerName { get; set; }

		[Display(Name = "Job Title*:")]
		public string Natural_EmployedJobTitle { get; set; }

		[Display(Name = "Registered Address of Employer*:")]
		public string Natural_EmployedRegisteredAddress { get; set; }

		public string Natural_EmployedRegisteredAddress2 { get; set; }

		public string Natural_EmployedRegisteredAddress3 { get; set; }

		[Display(Name = "Name of Business*:")]
		public string Natural_SelfEmployedBusinessName { get; set; }

		[Display(Name = "Business Registration No.*:")]
		public string Natural_SelfEmployedRegistrationNo { get; set; }

		[Display(Name = "Registered Business Address*:")]
		public string Natural_SelfEmployedBusinessAddress { get; set; }

		[Display(Name = "Principal Place of Business*:")]
		public string Natural_SelfEmployedBusinessPrincipalPlace { get; set; }

		[Display(Name = "Shipping Address*:")]
		public string Shipping_Address1 { get; set; }

		[Display(Name = "Shipping Address 2:")]
		public string Shipping_Address2 { get; set; }

		[Display(Name = "Shipping Address 3:")]
		public string Shipping_Address3 { get; set; }

		[Display(Name = "Mailing Postal Code*:")]
		public string Shipping_PostalCode { get; set; }

		[Display(Name = "Mailing Postal Code*:")]
		public string Mailing_PostalCode { get; set; }

		[Display(Name = "Search Tag:")]
		public string SearchTags { get; set; }

		public string Password { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime UpdatedOn { get; set; }

		public string IsDeleted { get; set; }

		public long IsSubAccount { get; set; }

		public string ResetPasswordToken { get; set; }

		public DateTime? LastPasswordUpdated { get; set; }

		public string Customer_Title { get; set; }

		public string Surname { get; set; }

		public string GivenName { get; set; }

		public int hasCustomerAccount { get; set; }

		public int isVerify { get; set; }

		public int isKYCVerify { get; set; }

		public string VerifyAccountToken { get; set; }

		[Display(Name = "Birthday*:")]
		public DateTime? DOB { get; set; }

		[Display(Name = "Transaction Allowed Type (Customer Portal):")]
		public string EnableTransactionType { get; set; }

		public virtual List<KYC_CustomerOthers> Others { get; set; }
	}
}